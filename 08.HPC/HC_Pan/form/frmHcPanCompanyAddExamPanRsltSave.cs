using ComBase;
using ComLibB;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using ComEmrBase;

/// <summary>
/// Class Name      : HC_Pan
/// File Name       : frmHcPanCompanyAddExamPanRsltSave.cs
/// Description     : 회사추가검사 판정결과 등록
/// Author          : 이상훈
/// Create Date     : 2019-11-22
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmPanjeng9.frm(HcPan108)" />

namespace HC_Pan
{
    public partial class frmHcPanCompanyAddExamPanRsltSave : Form
    {
        HicResEtcService hicResEtcService = null;
        HicJepsuService hicJepsuService = null;
        HicPatientService hicPatientService = null;
        HicResultExCodeService hicResultExCodeService = null;
        HicResEtcBohum1Service hicResEtcBohum1Service = null;
        HicJepsuLtdResBohum1Service hicJepsuLtdResBohum1Service = null;
        HicResBohum1Service hicResBohum1Service = null;
        BasIllsService basIllsService = null;
        HicJepsuResEtcExjongLtdService hicJepsuResEtcExjongLtdService = null;

        frmHcPanJochiHelp FrmHcPanJochiHelp = null;
        frmViewResult FrmViewResult = null;
        frmHeaResult FrmHeaResult = null;

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        long FnWRTNO;
        long FnPano;
        string FstrJumin;
        string FstrSex;
        string FstrPano;    //원무행정의 등록번호
        string FstrJong;    //건진종류
        string FstrJepDate; //접수일자
        long FnWrtno1;      //1차
        long FnWrtno2;      //2차
        string FstrCOMMIT;
        string FstrUCodes;
        string FstrSaveGbn;
        string FstrGbOHMS;
        string FstrMCode;
        string FstrYuhe;
        string FstrPtno;
        string FstrROWID;
        string FstrYROWID;  //약속판정 ROWID
        string FstrPROWID;
        string FstrPanOk1;
        string FstrPanOk2;
        long FnPan2Row;

        string[] strMunjin = new string[3];
        string[] strChiRyo = new string[3];
        string[] strBalYY = new string[3];
        string strGaJokJil;

        int nOldCNT = 0;
        //string strExamCode = "";
        List<string> strExamCode = new List<string>();
        string strAllWRTNO = "";
        string strJepDate = "";
        string strExPan = "";

        int nHyelH = 0;
        int nHyelL = 0;
        int nHeight = 0;
        int nWeight = 0;
        int nResult = 0;

        int nREAD = 0;

        string strExCode = "";
        string strResult = "";
        string strResCode = "";
        string strResultType = "";
        string strGbCodeUse = "";
        string strResName = "";
        string strRemark = "";
        string strOldJepsuDate = "";
        string strOldJepDate = "";

        public frmHcPanCompanyAddExamPanRsltSave()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicResEtcService = new HicResEtcService();
            hicJepsuService = new HicJepsuService();
            hicResultExCodeService = new HicResultExCodeService();
            hicPatientService = new HicPatientService();
            hicResEtcBohum1Service = new HicResEtcBohum1Service();
            hicJepsuLtdResBohum1Service = new HicJepsuLtdResBohum1Service();
            hicResBohum1Service = new HicResBohum1Service();
            basIllsService = new BasIllsService();
            hicJepsuResEtcExjongLtdService = new HicJepsuResEtcExjongLtdService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnJochi1.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.txtLtdCode.Click += new EventHandler(eTxtClick);
            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SSHistory.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.txtPanDate.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtPanDate.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtPanDrNo.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtSName.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtSName.LostFocus += new EventHandler(eTxtLostFocus);
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);
            dtpFrDate.Text = long.Parse(VB.Left(clsPublic.GstrSysDate, 4)) - 1 + "-01-01";
            dtpToDate.Text = long.Parse(VB.Left(clsPublic.GstrSysDate, 4)) - 1 + "-12-31";
        }

        void eBtnClick(object sender, EventArgs e)
        {
            int result = 0;

            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnEMR)
            {
                string strPara = "";

                CallEmrViewNew();

                ///TODO : 이상훈(2019.11.25) New EMR 호출 추가
                //strPara = ""
                //Set CallTextView = New clsCallEmrView

                //Call CallTextView.EXECUTE_TextEmrViewEx(FstrPtno, GnJobSabun)
                //Set CallTextView = Nothing
            }
            else if (sender == btnJochi1)
            {
                FrmHcPanJochiHelp = new frmHcPanJochiHelp();
                FrmHcPanJochiHelp.SpreadDoubleClick += new frmHcPanJochiHelp.Spread_DoubleClick(frmHcPanJochiHelp_SpreadDoubleClick);
                FrmHcPanJochiHelp.ShowDialog(this);
                FrmHcPanJochiHelp.SpreadDoubleClick -= new frmHcPanJochiHelp.Spread_DoubleClick(frmHcPanJochiHelp_SpreadDoubleClick);
            }
            else if (sender == btnLtdCode)
            {
                clsPublic.GstrRetName = "";

                frmHcLtdHelp frm = new frmHcLtdHelp();
                frm.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(LtdCd_value);
                frm.ShowDialog();
                frm.rSetGstrValue -= new frmHcLtdHelp.SetGstrValue(LtdCd_value);

                if (!LtdHelpItem.IsNullOrEmpty())
                {
                    txtLtdCode.Text = LtdHelpItem.CODE.ToString() + "." + LtdHelpItem.SANGHO;
                }
                else
                {
                    txtLtdCode.Text = "";
                }
            }
            else if (sender == btnMed)
            {
                FrmHeaResult = new frmHeaResult(FstrJumin);
                FrmHeaResult.ShowDialog(this);
            }
            else if (sender == btnPacs)
            {
                clsPublic.GstrHelpCode = FstrPano;

                FrmViewResult = new frmViewResult(FstrPano);
                FrmViewResult.ShowDialog(this);
            }
            else if (sender == btnSearch)
            {
                int nREAD = 0;
                string strOK = "";
                string strFrDate = "";
                string strToDate = "";
                long nLtdCode = 0;
                string strSName = "";
                string strJob = "";
                string strSort = "";

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;

                if (!txtLtdCode.Text.Trim().IsNullOrEmpty())
                {
                    nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>();
                }

                if (!txtSName.Text.IsNullOrEmpty())
                {
                    strSName = txtSName.Text.Trim();
                }

                if (rdoGubun1.Checked == true)
                {
                    strJob = "1";
                }
                else
                {
                    strJob = "2";
                }

                if (chkSort1.Checked == true)
                {
                    strSort = "1";
                }
                else
                {
                    strSort = "2";
                }

                //결과 테이블에 자료 INSERT
                fn_HIC_RES_ETC_INSERT();

                sp.Spread_All_Clear(SSList);
                txtSName.Text = txtSName.Text.Trim();

                if (!txtLtdCode.Text.IsNullOrEmpty())
                {
                    if (txtLtdCode.Text.IndexOf(".") == -1)
                    {
                        txtLtdCode.Text = txtLtdCode.Text.Trim() + "." + hb.READ_Ltd_Name(txtLtdCode.Text.Trim());
                    }
                    else
                    {
                        txtLtdCode.Text = VB.Pstr(txtLtdCode.Text.Trim(), ".", 1) + "." + hb.READ_Ltd_Name(VB.Pstr(txtLtdCode.Text.Trim(), ".", 1));
                    }
                }

                //신규접수 및 접수수정 자료를 SELECT
                List<HIC_JEPSU_RES_ETC_EXJONG_LTD> list = hicJepsuResEtcExjongLtdService.GetItembyJepDate(strFrDate, strToDate, nLtdCode, strSName, strJob, strSort);

                nREAD = list.Count;
                SSList.ActiveSheet.RowCount = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    strOK = "OK";
                    SSList.ActiveSheet.Cells[i, 0].Text = list[i].SNAME.Trim();
                    SSList.ActiveSheet.Cells[i, 1].Text = list[i].GJJONG.Trim();
                    for (int j = 1; j < VB.L(list[i].SEXAMS, ",") - 1; j++)
                    {
                        switch (VB.Pstr(list[i].SEXAMS, ",", j))
                        {
                            case "7054":
                            case "7055":
                                SSList.ActiveSheet.Cells[i, 1].Text = "뇌심혈관1차";
                                break;
                            default:
                                break;
                        }
                    }
                    SSList.ActiveSheet.Cells[i, 2].Text = list[i].LTDNAME.Trim();
                    SSList.ActiveSheet.Cells[i, 3].Text = list[i].JEPDATE.ToString();
                    if (!list[i].GBPANJENG.IsNullOrEmpty())
                    {
                        if (list[i].PANJENGDRNO == 0)
                        {
                            SSList.ActiveSheet.Cells[i, 4].Text += "*";
                            SSList.ActiveSheet.Cells[i, 4].BackColor = Color.FromArgb(255, 255, 0);
                        }
                    }
                    SSList.ActiveSheet.Cells[i, 5].Text = list[i].WRTNO.ToString();
                    SSList.ActiveSheet.Cells[i, 6].Text = list[i].SEX;
                    SSList.ActiveSheet.Cells[i, 7].Text = list[i].AGE.ToString();
                }
                txtSName.Text = "";
            }
            else if (sender == btnOK)
            {
                if (clsHcVariable.GnHicLicense == 0)
                {
                    MessageBox.Show("판정의사만 판정을 할수 있습니다", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                fn_DB_Update_Panjeng("판정");
                fn_Panjeng_End_Check();
            }
        }

        /// <summary>
        /// 신규 EMR을 호출한다
        /// </summary>
        void CallEmrViewNew()
        {
            EmrPatient AcpEmr = null;
            AcpEmr = clsEmrChart.ClearPatient();
            AcpEmr = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, FstrPtno, "O", FstrJepDate.Replace("-", ""), "HR");

            if (AcpEmr != null)
            {
                #region //Call EMR New
                if (clsMedOrderPublic.MedOrderEmr != null)
                {
                    if (clsMedOrderPublic.MedOrderEmr.Visible == false)
                    {
                        clsMedOrderPublic.MedOrderEmr.Dispose();
                        clsMedOrderPublic.MedOrderEmr = null;
                        clsMedOrderPublic.MedOrderEmr = new frmEmrViewMain(AcpEmr, this);
                        clsMedOrderPublic.MedOrderEmr.Show();
                    }
                    else
                    {
                        clsMedOrderPublic.MedOrderEmr.SetNewPatient(AcpEmr, this);
                    }
                }
                else
                {
                    clsMedOrderPublic.MedOrderEmr = new frmEmrViewMain(AcpEmr, this);
                    clsMedOrderPublic.MedOrderEmr.Show();
                }
                Application.DoEvents();
                #endregion //Call EMR New
            }
            else
            {
                ComFunc.MsgBox("접수내역을 찾을 수 없습니다.");
            }
        }

        void frmHcPanJochiHelp_SpreadDoubleClick(string strRemark)
        {
            txtSogen.Text += strRemark;
        }

        void fn_HIC_RES_ETC_INSERT()
        {
            int nREAD = 0;
            long nWrtNo = 0;
            string strJepDate = "";
            string strFrDate = "";
            string strToDate = "";
            int result = 0;

            strFrDate = dtpFrDate.Text;
            strToDate = dtpToDate.Text;

            List<HIC_JEPSU> list = hicJepsuService.GetItembyJepdateGbAddPan(strFrDate, strToDate,"69");

            nREAD = list.Count;
            if (nREAD == 0) return;

            clsDB.setBeginTran(clsDB.DbCon);
            for (int i = 0; i < nREAD; i++)
            {
                nWrtNo = list[i].WRTNO;
                strJepDate = list[i].JEPDATE;

                result = hicResEtcService.SaveHicResEtc(nWrtNo, strJepDate, "2");
            }

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("자료를 등록중 오류가 발생함!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);

        }

        /// <summary>
        /// 거래처코드 찾기 화면 스프레드더블클릭이벤트
        /// </summary>
        /// <param name="item"></param>
        private void LtdCd_value(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        void fn_Screen_Clear()
        {
            FnPano = 0;
            FstrJumin = "";
            FnWRTNO = 0;
            FstrSex = "";
            FstrROWID = "";
            FstrUCodes = "";
            FstrCOMMIT = "";
            FnPano = 0;
            FnWrtno1 = 0;
            FnWrtno2 = 0;
            FstrSaveGbn = "";
            FstrPano = "";
            FstrJumin = "";
            FstrGbOHMS = "";
            FstrPanOk1 = "";
            FstrPanOk2 = "";
            FstrPtno = "";

            txtPanDate.Text = "";
            txtSogen.Text = "";
            txtOldByengName.Text = "";
            txtSName.Text = "";
            txtResult1.Text = "";
            txtResult21.Text = "";
            txtResult22.Text = "";

            sp.Spread_All_Clear(SSPatInfo);
            SSPatInfo.ActiveSheet.RowCount = 1;
            sp.Spread_All_Clear(SS2);
            SS2.ActiveSheet.RowCount = 20;

            txtPanDrNo.Text = "";
            lblDrName.Text = "";
            btnEMR.Enabled = false;
        }

        void fn_Panjeng_End_Check()        
        {
            string strOK = "";
            long nPanDrno = 0;
            string strPanDate = "";
            int result = 0;
            string sMsg = "";

            strOK = "OK";

            clsDB.setBeginTran(clsDB.DbCon);

            //건강보험1차 판정완료 Check
            HIC_RES_ETC list = hicResEtcService.GetItembyWrtNo(FnWRTNO, "2");

            if (list != null)
            {
                strOK = "NO";
            }
            else
            {
                if (list.PANJENGDRNO == 0)
                {
                    strOK = "NO";
                    nPanDrno = list.PANJENGDRNO;
                    strPanDate = list.PANJENGDATE.ToString();
                }
            }

            //판정완료/미완료 SET
            result = hicResEtcService.UpdatebyWrtNo(FnWRTNO, strOK, "2");

            if (result < 0)
            {
                MessageBox.Show("판정완료/미완료 저장 중 오류발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }

            //접수마스타에 판정일자 Update
            result = hicJepsuService.UpdatePanjengDatebyWrtNo(FnWRTNO, strOK, strPanDate, nPanDrno);

            clsDB.setCommitTran(clsDB.DbCon);

            if (strOK == "OK")
            {
                sMsg = "판정이 완료되었습니다.";
                sMsg += "화면을 Clear후 다음 환자를 판정하시겠습니까?";
                if (MessageBox.Show(sMsg, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    fn_Screen_Clear();
                    eBtnClick(btnSearch, new EventArgs());
                    SSList.Focus();
                }
            }
        }

        void fn_Screen_Display()
        {
            int nREAD = 0;
            int nRow = 0;
            string strSex = "";
            string strPart = "";
            string strExCode = "";
            string strResult = "";
            string strResCode = "";
            string strResultType = "";
            string strResName = "";
            string strGbCodeUse = "";
            string strNomal = "";
            string strIpsadate = "";
            string strJepDate = "";
            string strGjJong = "";
            long nLicense = 0;
            string strDrname = "";
            int nAscii = 0;
            int nHyelH = 0;
            int nHyelL = 0;
            int nHeight = 0;
            int nWeight = 0;
            int nResult = 0;
            string strRemark = "";
            string strExPan = "";//검사1건의 판정결과
            string strGbSPC = "";
            int nGan1 = 0;
            int nGan2 = 0;
            int nGanResult = 0;

            FstrPanOk1 = "";
            FstrPanOk2 = "";

            txtResult1.Text = "";
            txtResult21.Text = "";
            txtResult22.Text = "";

            tabControl1.SelectedTab = tab12;
            tab12.Text = "";
            tabControl1.SelectedTab = tab13;
            tab13.Text = "";

            //Screen_Injek_display       //인적사항을 Display
            HIC_JEPSU list = hicJepsuService.GetItembyWrtNo(FnWRTNO);

            if (list == null)
            {
                MessageBox.Show("접수번호가 등록 안됨", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            strIpsadate = list.IPSADATE.ToString();
            FnPano = list.PANO;
            strSex = list.SEX;
            clsHcVariable.GstrGjYear = list.GJYEAR;
            FstrSex = strSex;
            strJepDate = list.JEPDATE;
            FstrPtno = list.PTNO.Trim();

            SSPatInfo.ActiveSheet.Cells[0, 0].Text = list.PANO.ToString();
            SSPatInfo.ActiveSheet.Cells[0, 1].Text = list.SNAME;
            SSPatInfo.ActiveSheet.Cells[0, 2].Text = list.AGE + "/" + list.SEX;
            SSPatInfo.ActiveSheet.Cells[0, 3].Text = hb.READ_Ltd_Name(list.LTDCODE.ToString());
            SSPatInfo.ActiveSheet.Cells[0, 4].Text = strJepDate;
            SSPatInfo.ActiveSheet.Cells[0, 5].Text = hb.READ_GjJong_Name(list.GJJONG);
            strGjJong = list.GJJONG;

            btnPacs.Enabled = false;
            btnMed.Enabled = false;

            //주민등록번호로 환자 등록번호 찾기
            HIC_PATIENT list2 = hicPatientService.GetItembyPaNo(FnPano);

            FstrJumin = "";
            FstrPano = "";

            if (list2 != null)
            {
                FstrJumin = clsAES.DeAES(list2.JUMIN2.Trim());
                FstrPano = list2.PTNO;
            }

            //주민등록번호로 환자 등록번호 찾기
            if (!FstrPano.IsNullOrEmpty())
            {
                btnPacs.Enabled = true;
            }

            //종합검진을 하였는지 점검함
            if (hicPatientService.GetCountbyJumin2(clsAES.AES(FstrJumin)) > 0)
            {
                btnMed.Enabled = true;
            }

            hm.ExamResult_RePanjeng(FnWRTNO, FstrSex, strJepDate, ""); //검사결과를 재판정

            //Screen_Exam_Items_display //검사항목을 Display
            List<HIC_RESULT_EXCODE> list3 = hicResultExCodeService.GetItembyWrtNoOrderbyPanjengPartExCode(FnWRTNO);

            nREAD = list3.Count;
            nRow = 0;
            strRemark = "";
            for (int i = 0; i < nREAD; i++)
            {
                strExCode = list3[i].EXCODE;                        //검사코드
                strResult = list3[i].RESULT.Trim();                 //검사실 결과값
                strResCode = list3[i].RESCODE.Trim();               //결과값 코드
                strResultType = list3[i].RESULTTYPE.Trim();         //결과값 TYPE
                strGbCodeUse = list3[i].GBCODEUSE.Trim();           //결과값코드 사용여부

                //SS2에 검사실 결과값을 DISPLAY
                SS2.ActiveSheet.Cells[i, 0].Text = " " + list3[i].HNAME.Trim();
                SS2.ActiveSheet.Cells[i, 1].Text = strResult;
                //비만도
                if (strExCode == "A103")
                {
                    strResCode = "061";
                }

                if (strGbCodeUse == "Y")
                {
                    if (!strResult.IsNullOrEmpty())
                    {
                        strResName = hb.READ_ResultName(strResCode, strResult);
                        SS2.ActiveSheet.Cells[i, 1].Text = hb.READ_ResultName(strResCode, strResult);
                        if (strResName.Length > 7)
                        {
                            strRemark += "▷" + list3[i].HNAME.Trim() + ":";
                            strRemark += strResName + "\r\n";
                        }
                    }
                }
                else if (strResult.Length > 7)
                {
                    strRemark += "▷" + list3[i].HNAME.Trim() + ":";
                    strRemark += strResName + "\r\n";
                }

                if (list3[i].PANJENG.Trim() == "2")
                {
                    SS2.ActiveSheet.Cells[i, 2].Text = "*";
                }

                //참고치를 Dispaly
                //참고치를 Dispaly
                strNomal = hm.EXAM_NomalValue_SET(strExCode, FstrJepDate, FstrSex, list3[i].MIN_M, list3[i].MAX_M, list3[i].MIN_F, list3[i].MAX_F);

                SS2.ActiveSheet.Cells[i, 3].Text = strNomal;
                SS2.ActiveSheet.Cells[i, 7].Text = strExCode;
                SS2.ActiveSheet.Cells[i, 8].Text = strResult; //정상값 점검용
                strExPan = list3[i].PANJENG.Trim();
                //판정결과별 바탕색상을 다르게 표시함
                switch (strExPan)
                {
                    case "B":
                        SS2.ActiveSheet.Cells[i, 1].BackColor = Color.FromArgb(250, 210, 222);  //정상B
                        break;
                    case "R":
                        SS2.ActiveSheet.Cells[i, 1].BackColor = Color.FromArgb(250, 170, 170);  //질환의심(R)
                        break;
                    default:
                        SS2.ActiveSheet.Cells[i, 1].BackColor = Color.FromArgb(190, 250, 220);  //정상A 또는 기타
                        break;
                }

                //간염검사
                if (strExCode == "A131")     //간염항원
                {
                    nGan1 = int.Parse(strResult);
                }
                else if (strExCode == "A132")     //간염항체
                {
                    nGan2 = int.Parse(strResult);
                }
            }

            if (!strRemark.IsNullOrEmpty())
            {
                txtResult1.Text = strRemark;
            }

            //건강검진 문진표 및 결과를  READ
            HIC_RES_ETC_BOHUM1 list4 = hicResEtcBohum1Service.GetItembyWrtNo(FnWRTNO);

            if (list4 == null)
            {
                MessageBox.Show("접수번호 " + FnWRTNO + " 는 결과 및 판정이 등록 안됨", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //----------( 판정일자,판정의사,판정결과 )---------------------------------
            txtPanDate.Text = "";
            if (!list4.PANJENGDATE.IsNullOrEmpty())    //판정일자
            {
                txtPanDate.Text = list4.PANJENGDATE;
            }

            //성인병일경우 이홍주, 공무원일경우 배삼덕 강제세팅
            switch (FstrJong)
            {
                case "성인병1차":
                case "성인병2차":
                    nLicense = 1809;
                    break;
                case "공무원1차":
                case "공무원2차":
                    nLicense = 10936;
                    break;
                default:
                    nLicense = list4.PANJENGDRNO;
                    break;
            }
            nLicense = list4.PANJENGDRNO1;  //의사면허번호
            txtPanDrNo.Text = "";
            lblDrName.Text = "";
            if (nLicense > 0)
            {
                txtPanDrNo.Text = nLicense.ToString();
                lblDrName.Text = hb.READ_License_DrName(long.Parse(txtPanDrNo.Text));
            }
            else
            {
                txtPanDrNo.Text = clsHcVariable.GnHicLicense.ToString();
                lblDrName.Text = hb.READ_License_DrName(clsHcVariable.GnHicLicense);
            }

            if (list4.PANJENGDRNO > 0)
            {
                txtPanDate.Text = list4.PANJENGDATE;
            }
            else
            {
                txtPanDate.Text = clsPublic.GstrSysDate;
            }

            //판정완료자는 판정한 의사만 변경이 가능함
            pnlPanjeng.Enabled = false;
            if (nLicense == 0 || (nLicense == clsHcVariable.GnHicLicense && clsHcVariable.GnHicLicense > 0))
            {
                pnlPanjeng.Enabled = true;
            }

            //--------------- 문진표 DISPLAY --------------------------------------------------
            
            switch (list4.SICK11.Trim())
            {
                case "1":
                    strMunjin[0] = "결핵";
                    break;
                case "2":
                    strMunjin[0] = "간염";
                    break;
                case "3":
                    strMunjin[0] = "간장질환";
                    break;
                case "4":
                    strMunjin[0] = "고혈압";
                    break;
                case "5":
                    strMunjin[0] = "심장병";
                    break;
                case "6":
                    strMunjin[0] = "뇌졸중";
                    break;
                case "7":
                    strMunjin[0] = "당뇨병";
                    break;
                case "8":
                    strMunjin[0] = "암";
                    break;
                case "9":
                    strMunjin[0] = "기타";
                    break;                        
                default:
                    strMunjin[0] = "";
                    break;
            }

            switch (list4.SICK21.Trim())
            {
                case "1":
                    strMunjin[1] = "결핵";
                    break;
                case "2":
                    strMunjin[1] = "간염";
                    break;
                case "3":
                    strMunjin[1] = "간장질환";
                    break;
                case "4":
                    strMunjin[1] = "고혈압";
                    break;
                case "5":
                    strMunjin[1] = "심장병";
                    break;
                case "6":
                    strMunjin[1] = "뇌졸중";
                    break;
                case "7":
                    strMunjin[1] = "당뇨병";
                    break;
                case "8":
                    strMunjin[1] = "암";
                    break;
                case "9":
                    strMunjin[1] = "기타";
                    break;
                default:
                    strMunjin[1] = "";
                    break;
            }

            switch (list4.SICK31.Trim())
            {
                case "1":
                    strMunjin[2] = "결핵";
                    break;
                case "2":
                    strMunjin[2] = "간염";
                    break;
                case "3":
                    strMunjin[2] = "간장질환";
                    break;
                case "4":
                    strMunjin[2] = "고혈압";
                    break;
                case "5":
                    strMunjin[2] = "심장병";
                    break;
                case "6":
                    strMunjin[2] = "뇌졸중";
                    break;
                case "7":
                    strMunjin[2] = "당뇨병";
                    break;
                case "8":
                    strMunjin[2] = "암";
                    break;
                case "9":
                    strMunjin[2] = "기타";
                    break;
                default:
                    strMunjin[2] = "";
                    break;
            }

            strBalYY[0] = list4.SICK12;
            strBalYY[1] = list4.SICK22;
            strBalYY[2] = list4.SICK32;

            switch (list4.SICK13)
            { 
                case "1":
                    strChiRyo[0] = "완치";
                break;
                case "2":
                    strChiRyo[0] = "치료중";
                    break;
                default:
                    strChiRyo[0] = "";
                    break;
            }

            switch (list4.SICK23)
            {
                case "1":
                    strChiRyo[1] = "완치";
                    break;
                case "2":
                    strChiRyo[1] = "치료중";
                    break;
                default:
                    strChiRyo[1] = "";
                    break;
            }

            switch (list4.SICK33)
            {
                case "1":
                    strChiRyo[2] = "완치";
                    break;
                case "2":
                    strChiRyo[2] = "치료중";
                    break;
                default:
                    strChiRyo[2] = "";
                    break;
            }

            SSMunjin.ActiveSheet.Cells[1, 0].Text = "";
            for (int i = 0; i < 3; i++)
            {
                if (!strMunjin[i].IsNullOrEmpty())
                {
                    SSMunjin.ActiveSheet.Cells[1, 0].Text += (strMunjin[i] + "(" + strBalYY[i] + ")" + strChiRyo[i] + "," + " ").Trim();
                }
            }

            if (list4.GAJOK1 == "2")
            {
                strGaJokJil += " 간장질환";
            }

            if (list4.GAJOK2 == "2")
            {
                strGaJokJil += " 고혈압";
            }

            if (list4.GAJOK3 == "2")
            {
                strGaJokJil += " 뇌졸증";
            }

            if (list4.GAJOK4 == "2")
            {
                strGaJokJil += " 심장병";
            }

            if (list4.GAJOK5 == "2")
            {
                strGaJokJil += " 당뇨병";
            }

            if (list4.GAJOK6 == "2")
            {
                strGaJokJil += " 암";
            }

            SSMunjin.ActiveSheet.Cells[3, 0].Text = strGaJokJil;

            if (list4.ROSICK == "1")
            {
                SSMunjin.ActiveSheet.Cells[4, 1].Text = "없음";
            }
            else if (list4.ROSICK == "2")
            {
                SSMunjin.ActiveSheet.Cells[4, 1].Text = "있음";
            }

            //식습관
            switch (list4.SIKSENG)
            {
                case "1":
                    SSMunjin.ActiveSheet.Cells[5, 1].Text = "주로 채식";
                    break;
                case "2":
                    SSMunjin.ActiveSheet.Cells[5, 1].Text = "채식,육식 골고루";
                    break;
                case "3":
                    SSMunjin.ActiveSheet.Cells[5, 1].Text = "주로 육식";
                    break;
                default:
                    SSMunjin.ActiveSheet.Cells[5, 1].Text = "";
                    break;
            }

            //음주횟수            
            switch (list4.DRINK1)
            {
                case "1":
                    SSMunjin.ActiveSheet.Cells[6, 1].Text = "월 1회미만";
                    break;
                case "2":
                    SSMunjin.ActiveSheet.Cells[6, 1].Text = "월 2~3회";
                    break;
                case "3":
                    SSMunjin.ActiveSheet.Cells[6, 1].Text = "일주일 1~2회";
                    break;
                case "4":
                    SSMunjin.ActiveSheet.Cells[6, 1].Text = "일주일 3~4회";
                    break;
                case "5":
                    SSMunjin.ActiveSheet.Cells[6, 1].Text = "거의 매일";
                    break;
                default:
                    break;
            }

            //음주량
            switch (list4.DRINK2)
            {
                case "N":
                    SSMunjin.ActiveSheet.Cells[7, 1].Text = "음주않함";
                    break;
                case "1":
                    SSMunjin.ActiveSheet.Cells[7, 1].Text = "반병";
                    break;
                case "2":
                    SSMunjin.ActiveSheet.Cells[7, 1].Text = "한병";
                    break;
                case "3":
                    SSMunjin.ActiveSheet.Cells[7, 1].Text = "한병 반";
                    break;
                case "4":
                    SSMunjin.ActiveSheet.Cells[7, 1].Text = "두병";
                    break;
                default:
                    break;
            }

            //흡연
            switch (list4.SMOKING1)
            {
                case "1":
                    SSMunjin.ActiveSheet.Cells[8, 1].Text = "금연";
                    break;
                case "2":
                    SSMunjin.ActiveSheet.Cells[8, 1].Text = "과거에 피움";
                    break;
                case "3":
                    SSMunjin.ActiveSheet.Cells[8, 1].Text = "현재 피움";
                    break;
                default:
                    break;
            }

            //흡연량
            switch (list4.SMOKING2)
            {
                case "N":
                    SSMunjin.ActiveSheet.Cells[9, 1].Text = "금연";
                    break;
                case "1":
                    SSMunjin.ActiveSheet.Cells[9, 1].Text = "반갑 미만";
                    break;
                case "2":
                    SSMunjin.ActiveSheet.Cells[9, 1].Text = "반갑이상~한갑미만";
                    break;
                case "3":
                    SSMunjin.ActiveSheet.Cells[9, 1].Text = "한갑이상~두갑미만";
                    break;
                case "4":
                    SSMunjin.ActiveSheet.Cells[9, 1].Text = "두갑이상";
                    break;
                default:
                    break;
            }

            //금연기간
            switch (list4.SMOKING3)
            {
                case "N":
                    SSMunjin.ActiveSheet.Cells[10, 1].Text = "금연";
                    break;
                case "1":
                    SSMunjin.ActiveSheet.Cells[10, 1].Text = "5년미만";
                    break;
                case "2":
                    SSMunjin.ActiveSheet.Cells[10, 1].Text = "5~9년";
                    break;
                case "3":
                    SSMunjin.ActiveSheet.Cells[10, 1].Text = "10~19년";
                    break;
                case "4":
                    SSMunjin.ActiveSheet.Cells[10, 1].Text = "20년~29년";
                    break;
                case "5":
                    SSMunjin.ActiveSheet.Cells[10, 1].Text = "30년이상";
                    break;
                default:
                    break;
            }

            //운동횟수
            switch (list4.SPORTS)
            {
                case "1":
                    SSMunjin.ActiveSheet.Cells[11, 1].Text = "운동않함";
                    break;
                case "2":
                    SSMunjin.ActiveSheet.Cells[11, 1].Text = "1~2회";
                    break;
                case "3":
                    SSMunjin.ActiveSheet.Cells[11, 1].Text = "3~4회";
                    break;
                case "4":
                    SSMunjin.ActiveSheet.Cells[11, 1].Text = "5~6회";
                    break;
                case "5":
                    SSMunjin.ActiveSheet.Cells[11, 1].Text = "거의 매일";
                    break;
                default:
                    break;
            }

            switch (list4.ANNOUNE)
            {
                case "1":
                    SSMunjin.ActiveSheet.Cells[12, 1].Text = "자주";
                    break;
                case "2":
                    SSMunjin.ActiveSheet.Cells[12, 1].Text = "가끔";
                    break;
                case "3":
                    SSMunjin.ActiveSheet.Cells[12, 1].Text = "없다";
                    break;
                case "4":
                    SSMunjin.ActiveSheet.Cells[12, 1].Text = "모르겠다";
                    break;
                default:
                    break;
            }

            switch (list4.WOMAN1)
            {
                case "N":
                    SSMunjin.ActiveSheet.Cells[13, 1].Text = "남자";
                    break;
                case "1":
                    SSMunjin.ActiveSheet.Cells[13, 1].Text = "예";
                    break;
                case "2":
                    SSMunjin.ActiveSheet.Cells[13, 1].Text = "아니오";
                    break;
                default:
                    break;
            }

            switch (list4.WOMAN2)
            {
                case "N":
                    SSMunjin.ActiveSheet.Cells[14, 1].Text = "남자";
                    break;
                case "1":
                    SSMunjin.ActiveSheet.Cells[14, 1].Text = "예";
                    break;
                case "2":
                    SSMunjin.ActiveSheet.Cells[14, 1].Text = "아니오";
                    break;
                default:
                    break;
            }

            //결혼연령
            switch (list4.WOMAN3)
            {
                case "N":
                    SSMunjin.ActiveSheet.Cells[15, 1].Text = "남자 또는 여자미혼";
                    break;
                case "1":
                    SSMunjin.ActiveSheet.Cells[15, 1].Text = "19세 이전";
                    break;
                case "2":
                    SSMunjin.ActiveSheet.Cells[15, 1].Text = "20~25세";
                    break;
                case "3":
                    SSMunjin.ActiveSheet.Cells[15, 1].Text = "26~30세";
                    break;
                case "4":
                    SSMunjin.ActiveSheet.Cells[15, 1].Text = "31~35세";
                    break;
                case "5":
                    SSMunjin.ActiveSheet.Cells[15, 1].Text = "36세";
                    break;
                default:
                    break;
            }

            //외상,휴유증
            if (list4.JINCHAL1 == "1")
            {
                rdoJinchal11.Checked = true;
            }
            else
            {
                rdoJinchal12.Checked = true;
            }

            //일반상태(양호,보통,불량)
            switch (list4.JINCHAL2)
            {
                case "1":
                    rdoJinchal21.Checked = true;
                    break;
                case "2":
                    rdoJinchal22.Checked = true;
                    break;
                case "3":
                    rdoJinchal23.Checked = true;
                    break;
                default:
                    break;
            }

            //과거질환
            if (list4.OLDBYENG1 == "1")
            {
                chkOldByeng1.Checked = true;
            }
            else
            {
                chkOldByeng1.Checked = false;
            }

            if (list4.OLDBYENG2 == "1")
            {
                chkOldByeng2.Checked = true;
            }
            else
            {
                chkOldByeng2.Checked = false;
            }

            if (list4.OLDBYENG3 == "1")
            {
                chkOldByeng3.Checked = true;
            }
            else
            {
                chkOldByeng3.Checked = false;
            }

            if (list4.OLDBYENG4 == "1")
            {
                chkOldByeng4.Checked = true;
            }
            else
            {
                chkOldByeng4.Checked = false;
            }

            if (list4.OLDBYENG5 == "1")
            {
                chkOldByeng5.Checked = true;
            }
            else
            {
                chkOldByeng5.Checked = false;
            }

            if (list4.OLDBYENG6 == "1")
            {
                chkOldByeng6.Checked = true;
            }
            else
            {
                chkOldByeng6.Checked = false;
            }

            if (list4.OLDBYENG7 == "1")
            {
                chkOldByeng7.Checked = true;
            }
            else
            {
                chkOldByeng7.Checked = false;
            }

            //생활습관
            if (list4.HABIT1 == "1")
            {
                chkHabit1.Checked = true;
            }
            else
            {
                chkHabit1.Checked = false;
            }

            if (list4.HABIT2 == "1")
            {
                chkHabit2.Checked = true;
            }
            else
            {
                chkHabit2.Checked = false;
            }

            if (list4.HABIT3 == "1")
            {
                chkHabit3.Checked = true;
            }
            else
            {
                chkHabit3.Checked = false;
            }

            if (list4.HABIT4 == "1")
            {
                chkHabit4.Checked = true;
            }
            else
            {
                chkHabit4.Checked = false;
            }

            if (list4.HABIT5 == "1")
            {
                chkHabit5.Checked = true;
            }
            else
            {
                chkHabit5.Checked = false;
            }

            //혈액종합판정 및 소견
            txtSogen.Text = list4.ADDSO;

            //종전결과 3개를 Display
            fn_OLD_Result_Display(long.Parse(FstrPano), strJepDate, strSex);

            btnEMR.Enabled = true;
            tabControl1.SelectedTab = tab11;
        }

        void fn_OLD_Result_Display(long argPano, string argJepDate, string argSex)
        {
            // 검사항목을 Setting
            strExamCode.Clear();

            for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
            {
                if (!SS2.ActiveSheet.Cells[i, 7].Text.IsNullOrEmpty())
                {
                    strExamCode.Add(SS2.ActiveSheet.Cells[i, 7].Text.Trim());
                }
            }

            //1차검사 종전 접수번호를 읽음
            List<HIC_JEPSU_LTD_RES_BOHUM1> list = hicJepsuLtdResBohum1Service.GetItembyPaNoJepDate(argPano, argJepDate);

            nOldCNT = list.Count;
            strAllWRTNO = "";
            if (nOldCNT > 2) nOldCNT = 2;
            for (int i = 0; i < nOldCNT; i++)
            {
                strAllWRTNO += list[i].WRTNO.ToString() + ",";
                strJepDate = list[i].JEPDATE.ToString();
                SS2_Sheet1.ColumnHeader.Cells.Get(0, i + 5).Value = VB.Left(strJepDate, 4) + VB.Mid(strJepDate, 6, 2) + VB.Right(strJepDate, 2);
                tabControl1.SelectedTabIndex = i + 1;
                tabControl1.SelectedTab.Text = strJepDate;
                fn_OLD_Panjeng_Display(i, list[i].WRTNO);
                fn_OLD_Result_Display_SUB(list[i].WRTNO, strExamCode, argSex, i);
                if (i >= 2) break;
            }
        }

        void fn_OLD_Panjeng_Display(int argNo, long argWrtNo)
        {
            string strPan = "";
            string strPAN1 = "";

            if (argNo == 0)
            {
                txtResult21.Text = "";
            }
            else
            {
                txtResult22.Text = "";
            }

            //건강검진 문진표 및 결과를  READ
            HIC_RES_BOHUM1 list = hicResBohum1Service.GetItemByWrtno(argWrtNo);

            if (list == null)
            {
                return;
            }

            //판정결과,판정일자,판정의사
            strPan = "▶판정결과:";
            switch (list.PANJENG)
            {
                case "1":
                    strPan += "정상A";
                    break;
                case "2":
                    strPan += "정상B";
                    break;
                case "3":
                    strPan += "질환의심(R)";
                    break;
                case "5":
                    strPan += "정상B+질환의심";
                    break;
                default:
                    strPan += "<오류>";
                    break;
            }
            strPan += " ○판정일자:" + list.PANJENGDATE;
            strPan += "  ○판정의사:" + hb.READ_License_DrName(list.PANJENGDRNO) + "\r\n";

            //판정(B)
            strPAN1 = "";

            if (list.PANJENGB1 == "1")
            {
                strPAN1 += "◎비만관리 ";
            }
            if (list.PANJENGB2 == "1")
            {
                strPAN1 += "◎혈압관리 ";
            }
            if (list.PANJENGB3 == "1")
            {
                strPAN1 += "◎콜레스테롤관리 ";
            }
            if (list.PANJENGB4 == "1")
            {
                strPAN1 += "◎간기능관리 ";
            }
            if (list.PANJENGB5 == "1")
            {
                strPAN1 += "◎당뇨관리 ";
            }
            if (list.PANJENGB6 == "1")
            {
                strPAN1 += "◎신장기능관리 ";
            }
            if (list.PANJENGB7 == "1")
            {
                strPAN1 += "◎빈혈관리 ";
            }
            if (list.PANJENGB8 == "1")
            {
                strPAN1 += "◎부인과질환관리 ";
            }

            if (!strPAN1.IsNullOrEmpty())
            {
                strPan += "▶판정(정상B): " + strPAN1 + "\r\n";
            }

            //판정(R)
            strPAN1 = "";
            if (list.PANJENGR1 == "1")
            {
                strPAN1 += "◎폐결핵의심 ";
            }
            if (list.PANJENGR2 == "1")
            {
                strPAN1 += "◎기타흉부질환의심 ";
            }
            if (list.PANJENGR3 == "1")
            {
                strPAN1 += "◎고혈압의심 ";
            }
            if (list.PANJENGR4 == "1")
            {
                strPAN1 += "◎고지혈증의심 ";
            }
            if (list.PANJENGR5 == "1")
            {
                strPAN1 += "◎간장질환의심 ";
            }
            if (list.PANJENGR6 == "1")
            {
                strPAN1 += "◎당뇨질환의심 ";
            }
            if (list.PANJENGR7 == "1")
            {
                strPAN1 += "◎신장질환의심 ";
            }
            if (list.PANJENGR8 == "1")
            {
                strPAN1 += "◎빈혈증의심 ";
            }
            if (list.PANJENGR9 == "1")
            {
                strPAN1 += "◎부인과질환의심 ";
            }
            if (list.PANJENGR10 == "1")
            {
                strPAN1 += "◎자궁경부암의심 ";
            }
            if (list.PANJENGR11 == "1")
            {
                strPAN1 += "◎기타질환의심 ";
            }

            if (!strPAN1.IsNullOrEmpty())
            {
                strPan += "▶판정(질환의심): " + strPAN1 + "\r\n";
            }

            //소견 및 조치사항
            if (!list.SOGEN.Trim().IsNullOrEmpty())
            {
                strPan += "▶소견및조치사항: " + list.SOGEN.Trim() + "\r\n";
            }
            //간염검사
            if (!list.LIVER3.IsNullOrEmpty())
            {
                switch (list.LIVER3.Trim())
                {
                    case "1":
                        strPan += "▶간염검사: 보균자" + "\r\n";
                        break;
                    case "2":
                        strPan += "▶간염검사: 면역자" + "\r\n";
                        break;
                    case "3":
                        strPan += "▶간염검사: 접종대상자" + "\r\n";
                        break;
                    default:
                        break;
                }
            }
            //자궁경부 선상피세포 유/무
            if (list.WOMB02.Trim() == "1")
            {
                strPan += "▶자궁경부 선상피세표 있음" + "\r\n";
            }

            if (argNo == 0)
            {
                txtResult21.Text = strPan;
            }
            else
            {
                txtResult22.Text = strPan;
            }
        }

        void fn_Genjin_Histroy_SET()
        {
            int nRead = 0;
            string strData = "";
            string strJong = "";
            long nHeaPano = 0;

            //종검의 등록번호를 찾음
            nHeaPano = 0;

            nHeaPano = hicPatientService.GetPanobyJumin(clsAES.AES(FstrJumin));

            //일반건진, 종합검진의 접수내역을 Display
            List<HIC_JEPSU> list = hicJepsuService.GetItembyOnlyPaNo(FnPano, nHeaPano);

            nREAD = list.Count;
            SSHistory.ActiveSheet.RowCount = nREAD;
            for (int i = 0; i < nREAD; i++)
            {
                strJong = list[i].GJJONG.Trim();

                SSHistory.ActiveSheet.Cells[i, 0].Text = list[i].JEPDATE;
                if (strJong == "XX")
                {
                    SSHistory.ActiveSheet.Cells[i, 1].Text = "종검";
                }
                else
                {
                    SSHistory.ActiveSheet.Cells[i, 1].Text = hb.READ_GjJong_Name(strJong);
                }
                SSHistory.ActiveSheet.Cells[i, 2].Text = list[i].WRTNO.ToString();
                SSHistory.ActiveSheet.Cells[i, 3].Text = list[i].GJJONG;
                SSHistory.ActiveSheet.Cells[i, 4].Text = list[i].GJCHASU;
            }
        }

        /// <summary>
        /// 종전 1회 검사의 결과를 Display
        /// </summary>
        void fn_OLD_Result_Display_SUB(long nWrtNo, List<string> argExamCode, string argSex, int index)
        {
            int nRow = 0;

            ///TODO : 이상훈(2019.10.31) clsHcMain.cs GET_HIC_JepsuDate Method 확인 필요
            //strOldJepsuDate = hm.GET_HIC_JepsuDate(nWrtNo);
            //판정결과를 strRemark에 보관            
            if (index == 0)
            {
                strRemark = txtResult21.Text + "\r\n";
            }
            else if (index == 1)
            {
                strRemark = txtResult22.Text + "\r\n";
            }            

            //검사항목 및 결과를 READ
            List<HIC_RESULT_EXCODE> list = hicResultExCodeService.GetItembyWrtNoNewExCode(nWrtNo, argExamCode, "N");

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                strExCode = list[i].EXCODE;                 //검사코드
                strResult = list[i].RESULT;                 //검사실 결과값
                strResCode = list[i].RESCODE;               //결과값 코드
                strResultType = list[i].RESULTTYPE;         //결과값 TYPE
                strGbCodeUse = list[i].GBCODEUSE;           //결과값코드 사용여부

                //해당검사코드가 있는 Row를 찾음
                nRow = 0;
                for (int j = 0; j < SS2.ActiveSheet.RowCount; j++)
                {
                    if (SS2.ActiveSheet.Cells[i, 7].Text.Trim() == strExCode)
                    {
                        nRow = j;
                        break;
                    }
                }

                //해당검사가 시트에 있으면 결과를 표시함
                if (nRow > 0)
                {
                    SS2.ActiveSheet.Cells[nRow, i + 5].Text = strResult;
                    if (strGbCodeUse == "Y")
                    {
                        if (!strResult.IsNullOrEmpty())
                        {
                            strResName = hb.READ_ResultName(strResCode, strResult);
                            SS2.ActiveSheet.Cells[nRow, i + 5].Text = strResName;
                            if (strResName.Length > 7)
                            {
                                strRemark += "▷" + list[i].HNAME.Trim() + ": ";
                                strRemark += strResName + "\r\n";
                            }
                        }
                        else if (strResult.Length > 7)
                        {
                            strResult += "▷" + list[i].HNAME.Trim() + ": ";
                            strRemark += strResult + "\r\n";
                        }
                        SS2.ActiveSheet.Cells[nRow, i + 10].Text = strResult;   //정상값 점검용
                        strExPan = hm.ExCode_Result_Panjeng(strExCode, strResult, argSex, strOldJepDate, "");
                        //판정결과별 바탕색상을 다르게 표시함
                        switch (strExPan)
                        {
                            case "B":
                                SS2.ActiveSheet.Cells[nRow, i + 5].BackColor = Color.FromArgb(250, 210, 222);   //정상B
                                break;
                            case "R":
                                SS2.ActiveSheet.Cells[nRow, i + 5].BackColor = Color.FromArgb(250, 170, 170);   //질환의심(R)
                                break;
                            default:
                                SS2.ActiveSheet.Cells[nRow, i + 5].BackColor = Color.FromArgb(190, 250, 220);   //정상A 또는 기타
                                break;
                        }
                    }
                }
            }

            if (index == 0)
            {
                txtResult21.Text = strRemark;
            }
            else
            {
                txtResult22.Text = strRemark;
            }
        }

        void eTxtClick(object sender, EventArgs e)
        {
            if (sender == txtLtdCode)
            {
                eBtnClick(btnLtdCode, new EventArgs());
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SSHistory)
            {
                frmHcPanXrayPracticianView f = new frmHcPanXrayPracticianView(SSHistory.ActiveSheet.Cells[e.Row, 2].Text.Trim().To<long>());
                f.ShowDialog(this);
            }
            else if (sender == SSList)
            {
                string strOK = "";

                fn_Screen_Clear();

                FnWRTNO = long.Parse(SSList.ActiveSheet.Cells[e.Row, 5].Text.Trim());
                FstrJong = SSList.ActiveSheet.Cells[e.Row, 1].Text.Trim();
                FstrJepDate = SSList.ActiveSheet.Cells[e.Row, 3].Text.Trim();

                //삭제된것 체크
                if (hb.READ_JepsuSTS(FnWRTNO) == "D")
                {
                    MessageBox.Show("접수번호는 삭제된것 입니다. 확인하십시오", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                fn_Screen_Display();
                fn_Genjin_Histroy_SET();

                if (clsHcVariable.GnHicLicense == 0)
                {
                    btnOK.Enabled = false;
                }
            }
        }

        void eTxtDblClick(object sender, EventArgs e)
        {
            if (sender == txtPanDate)
            {
                frmCalendar f = new frmCalendar();
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog();

                txtPanDate.Text = clsPublic.GstrCalDate;
                clsPublic.GstrCalDate = "";
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (sender == txtPanDrNo)
                {
                    lblDrName.Text = hb.READ_License_DrName(long.Parse(txtPanDrNo.Text));
                    SendKeys.Send("{TAB}");
                }
                else
                {
                    SendKeys.Send("{TAB}");
                }
            }
        }

        void eTxtGotFocus(object sender, EventArgs e)
        {
            txtSName.ImeMode = ImeMode.Hangul;
        }

        void eTxtLostFocus(object sender, EventArgs e)
        {
            txtSName.ImeMode = ImeMode.Alpha;
        }

        string READ_BAS_ILLS(string argCode)
        {
            string rtnVal = "";

            if (argCode.IsNullOrEmpty())
            {
                return rtnVal;
            }

            rtnVal = basIllsService.GetIllNameKbyIllCode(argCode);

            return rtnVal;
        }

        void fn_DB_Update_Panjeng(string argGbn)
        {
            int nBCnt = 0;
            int nRCNT = 0;
            string strPanjeng = "";
            string strGbErFlag = "";
            int result = 0;

            if (txtPanDrNo.Text.IsNullOrEmpty())
            {
                MessageBox.Show("판정의사공란", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                clsHcVariable.GnHicLicense = long.Parse(txtPanDrNo.Text);
            }
            txtSogen.Text = txtSogen.Text.Trim();
            
            if (txtPanDate.Text.IsNullOrEmpty())
            {
                txtPanDate.Text = clsPublic.GstrSysDate;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            //판정결과를 DB에 UPDATE
            HIC_RES_ETC item = new HIC_RES_ETC();

            item.GBPANJENG = "Y";
            item.SOGEN = txtSogen.Text.Trim().Replace("'", "`");
            item.PANJENGDATE = txtPanDate.Text;
            item.PANJENGDRNO = clsHcVariable.GnHicLicense;
            item.WRTNO = FnWRTNO;
            item.GUBUN = "2";

            result = hicResEtcService.UpdatebyWrtNo(item);

            if (result < 0)
            {
                MessageBox.Show("판정결과 DB에 저장시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);
        }
    }
}
