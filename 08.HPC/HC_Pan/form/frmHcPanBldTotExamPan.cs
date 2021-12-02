using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Pan
/// File Name       : frmHcPanBldTotExamPan.cs
/// Description     : 혈액종합검진 판정
/// Author          : 이상훈
/// Create Date     : 2019-12-16
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm혈액종합검진판정.frm(Frm혈액종합검진판정)" />

namespace HC_Pan
{
    public partial class frmHcPanBldTotExamPan : Form
    {
        HicResEtcService hicResEtcService = null;
        HicJepsuService hicJepsuService = null;
        HicPatientService hicPatientService = null;
        BasPatientService basPatientService = null;
        HicResultExCodeService hicResultExCodeService = null;
        HicJepsuLtdResBohum1Service hicJepsuLtdResBohum1Service = null;
        HicResBohum1Service hicResBohum1Service = null;
        BasIllsService basIllsService = null;
        HicJepsuResEtcSangdamNewLtdService hicJepsuResEtcSangdamNewLtdService = null;

        frmHcLtdHelp frmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        frmHeaResult FrmHeaResult = null;

        frmHcPanJochiHelp FrmHcPanJochiHelp = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        long FnWRTNO;
        long FnPano;
        string FstrJumin;
        string FstrSex;
        string FstrJepDate;
        string FstrPano;    // 원무행정의 등록번호
        string FstrJong;    // 건진종류

        long FnWrtno1;  //1차
        long FnWrtno2;  //2차
        string FstrCOMMIT;
        string FstrUCodes;
        string FstrSaveGbn;

        string FstrGbOHMS;
        string FstrMCode;
        string FstrYuhe;

        string FstrROWID;
        string FstrYROWID;  // 약속판정 ROWID
        string FstrPROWID;
        string FstrPanOk;

        long FnPan2Row;

        int nRow = 0;
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

        string[] strMunjin = new string[3];
        string[] strChiRyo = new string[3];
        string[] strBalYY = new string[3];
        string strGaJokJil = "";

        public frmHcPanBldTotExamPan()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicResEtcService = new HicResEtcService();
            hicJepsuService = new HicJepsuService();
            hicPatientService = new HicPatientService();
            basPatientService = new BasPatientService();
            hicResultExCodeService = new HicResultExCodeService();
            hicJepsuLtdResBohum1Service = new HicJepsuLtdResBohum1Service();
            hicResBohum1Service = new HicResBohum1Service();
            basIllsService = new BasIllsService();
            hicJepsuResEtcSangdamNewLtdService = new HicJepsuResEtcSangdamNewLtdService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnAutoPan.Click += new EventHandler(eBtnClick);
            this.btnJochi2.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnMed.Click += new EventHandler(eBtnClick);
            this.btnPacs.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.chkDoctor.Click += new EventHandler(eChkClick);
            this.SS2.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.txtPanDate.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtPanDrNo.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtSName.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtSName.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtSogen.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtSName.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtSogen.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtPanDate.DoubleClick += new EventHandler(eTxtDblClick);

        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            string strData = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            this.Text = "혈액종합소견 및 판정(" + clsHcVariable.GstrHicDrName + ") (면허:" + clsHcVariable.GnHicLicense + ")";

            SS2_Sheet1.Columns.Get(7).Visible = false;  // 검사코드
            SS2_Sheet1.Columns.Get(8).Visible = false;  // 결과1
            SS2_Sheet1.Columns.Get(9).Visible = false;  // 결과2

            dtpFrDate.Text = Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-90).ToShortDateString();
            dtpToDate.Text = clsPublic.GstrSysDate;

            txtLtdCode.Text = "";
            txtSogen.Text = "";

            fn_Screen_Clear();
            hb.READ_HIC_Doctor(long.Parse(clsType.User.IdNumber));
            hb.READ_Combo_HicDoctor2(cboPan);
            cboPan.Enabled = false;

            eBtnClick(btnSearch, new EventArgs());
            if (clsHcVariable.GnWRTNO > 0)
            {
                FnWRTNO = clsHcVariable.GnWRTNO;
                fn_Screen_Display();
                fn_Genjin_Histroy_SET();
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnAutoPan)
            {
                if (FnWRTNO == 0) return;

                txtSogen.Text += "\r\n" + "\r\n" + hm.ReadAutoPan(FnWRTNO.ToString());
            }
            else if (sender == btnJochi2)
            {
                FrmHcPanJochiHelp = new frmHcPanJochiHelp();
                FrmHcPanJochiHelp.SpreadDoubleClick += new frmHcPanJochiHelp.Spread_DoubleClick(frmHcPanJochiHelp_SpreadDoubleClick);
                FrmHcPanJochiHelp.ShowDialog(this);
                FrmHcPanJochiHelp.SpreadDoubleClick -= new frmHcPanJochiHelp.Spread_DoubleClick(frmHcPanJochiHelp_SpreadDoubleClick);
            }
            else if (sender == btnLtdCode)
            {
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
                frmViewResult f = new frmViewResult(FstrPano);
                f.ShowDialog(this);
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
                long nLicense = 0;
                int nPan = 0;

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;

                if (!txtLtdCode.Text.IsNullOrEmpty())
                {
                    nLtdCode = long.Parse(VB.Pstr(txtLtdCode.Text, ".", 1));
                }

                if (!txtSName.Text.IsNullOrEmpty())
                {
                    strSName = txtSName.Text.Trim();
                }

                if (rdoJob1.Checked == true)
                {
                    strJob = "1";
                }
                else
                {
                    strJob = "2";
                }

                if (rdoSort1.Checked == true)
                {
                    strSort = "1";
                }
                else if (rdoSort2.Checked == true)
                {
                    strSort = "2";
                }
                else if (rdoSort3.Checked == true)
                {
                    strSort = "3";
                }

                //결과 테이블에 자료 INSERT
                fn_HIC_RES_ETC_INSERT();

                sp.Spread_All_Clear(ssList);
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

                nLicense = clsHcVariable.GnHicLicense;
                nPan = int.Parse(VB.Pstr(cboPan.Text, ".", 1));

                //신규접수 및 접수수정 자료를 SELECT
                List<HIC_JEPSU_RES_ETC_SANGDAM_NEW_LTD> list = hicJepsuResEtcSangdamNewLtdService.GetItembyJepDate(strFrDate, strToDate, nLtdCode, strSName, strJob, strSort, nLicense, nPan);

                nREAD = list.Count;
                ssList.ActiveSheet.RowCount = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    strOK = "OK";

                    ssList.ActiveSheet.Cells[i, 0].Text = list[i].SNAME.Trim();
                    ssList.ActiveSheet.Cells[i, 1].Text = hb.READ_GjJong_Name(list[i].GJJONG.Trim());
                    ssList.ActiveSheet.Cells[i, 2].Text = list[i].LTDNAME.Trim();
                    ssList.ActiveSheet.Cells[i, 3].Text = list[i].JEPDATE.ToString();
                    if (!list[i].GBPANJENG.IsNullOrEmpty())
                    {
                        if (list[i].PANJENGDRNO == 0)
                        {
                            ssList.ActiveSheet.Cells[i, 4].Text += "*";
                            ssList.ActiveSheet.Cells[i, 4].BackColor = Color.FromArgb(255, 255, 0);
                        }
                    }
                    ssList.ActiveSheet.Cells[i, 5].Text = list[i].WRTNO.ToString();
                    ssList.ActiveSheet.Cells[i, 6].Text = list[i].SEX;
                    ssList.ActiveSheet.Cells[i, 7].Text = list[i].AGE.ToString();
                    ssList.ActiveSheet.Cells[i, 8].Text = list[i].GBPANJENG;
                }
                txtSName.Text = "";
            }
            else if (sender == btnCancel)
            {
                string sMsg = "";

                sMsg = "변경한 내용을 저장하지 않습니다." + "\r\n";
                sMsg += "정말 취소하시겠습니까?";
                if (MessageBox.Show(sMsg, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    fn_Screen_Clear();
                    ssList.Focus();
                    return;
                }
            }
            else if (sender == btnOK)
            {
                fn_DB_Update_Panjeng("판정");
                fn_Panjeng_End_Check();
            }
        }

        void frmHcPanJochiHelp_SpreadDoubleClick(string strRemark)
        {
            txtSogen.Text += strRemark;
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (sender == txtPanDate)
                {
                    SendKeys.Send("{TAB}");
                }
                else if (sender == txtPanDrNo)
                {
                    lblDrName.Text = hb.READ_License_DrName(long.Parse(txtPanDrNo.Text));
                }
                else if (sender == txtSName)
                {
                    btnSearch.Focus();
                }
            }
        }

        void eTxtGotFocus(object sender, EventArgs e)
        {
            if (sender == txtSName) 
            {
                txtSName.ImeMode = ImeMode.Hangul;
            }
            else if (sender == txtSogen)
            {
                txtSogen.ImeMode = ImeMode.Hangul;
            }
        }

        void eTxtLostFocus(object sender, EventArgs e)
        {
            if (sender == txtSName)
            {
                txtSName.ImeMode = ImeMode.Alpha;
            }
            else if (sender == txtSogen)
            {
                txtSogen.ImeMode = ImeMode.Hangul;
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

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS2)
            {
                clsHcVariable.GstrRefValue = "";    //약속처방
                clsHcVariable.GstrValue = "";       //결과값

                clsHcVariable.GstrRefValue = SS2.ActiveSheet.Cells[e.Row, 6].Text.Trim() + "^^";
                clsHcVariable.GstrRefValue += SS2.ActiveSheet.Cells[e.Row, 2].Text.Trim();

                clsHcVariable.GstrValue = SS2.ActiveSheet.Cells[e.Row, 6].Text.Trim();
                clsHcVariable.GstrValue += "【" + SS2.ActiveSheet.Cells[e.Row, 0].Text.Trim() + " 】";
                clsHcVariable.GstrValue += "결과값:" + SS2.ActiveSheet.Cells[e.Row, 1].Text.Trim();
                clsHcVariable.GstrValue += "  【레벨 " + SS2.ActiveSheet.Cells[e.Row, 2].Text.Trim() + " 】";

                frmHaPanjeng_New f = new frmHaPanjeng_New(clsHcVariable.GstrValue, clsHcVariable.GstrRefValue);
                f.ShowDialog(this);

                if (!clsHcVariable.GstrRefValue.IsNullOrEmpty())
                {
                    txtSogen.Text += clsHcVariable.GstrRefValue;
                }

                clsHcVariable.GstrRefValue = "";
            }
            else if (sender == SSHistory)
            {
                if (SSHistory.ActiveSheet.Cells[e.Row, 4].Text == "1") return;

                if (SSHistory.ActiveSheet.Cells[e.Row, 4].Text == "2")
                {
                    clsPublic.GstrRetValue = SSHistory.ActiveSheet.Cells[e.Row, 2].Text.Trim();
                    frmHcPanView f = new frmHcPanView(FnWRTNO);
                    f.ShowDialog(this);
                }
                else if (SSHistory.ActiveSheet.Cells[e.Row, 4].Text == "3")
                {
                    clsPublic.GstrRetValue = SSHistory.ActiveSheet.Cells[e.Row, 2].Text.Trim();
                }
            }
            else if (sender == ssList)
            {
                string strOK = "";

                fn_Screen_Clear();

                FnWRTNO = long.Parse(ssList.ActiveSheet.Cells[e.Row, 5].Text);
                FstrJong = ssList.ActiveSheet.Cells[e.Row, 1].Text;
                FstrPanOk = ssList.ActiveSheet.Cells[e.Row, 8].Text;

                txtWrtNo.Text = FnWRTNO.ToString();

                //삭제된것 체크
                if (hb.READ_JepsuSTS(FnWRTNO) == "D")
                {
                    MessageBox.Show(FnWRTNO + "접수번호는 삭제된것 입니다. 확인하십시오", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                fn_Screen_Clear();
                fn_Genjin_Histroy_SET();
            }
        }

        void eChkClick(object sender, EventArgs e)
        {
            if (sender == chkDoctor)
            {
                if (chkDoctor.Checked == true)
                {
                    cboPan.Enabled = true;
                }
                else
                {
                    cboPan.SelectedIndex = 0;
                    cboPan.Enabled = false;
                }
            }
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
            FnPano = 0;             FstrJumin = "";             FnWRTNO = 0;
            FstrSex = "";           FstrROWID = "";
            FstrUCodes = "";        FstrCOMMIT = "";
            FnWrtno1 = 0;           FnWrtno2 = 0;
            FstrSaveGbn = "";       FstrPano = "";              FstrJumin = "";
            FstrGbOHMS = "";
            FstrPanOk = "";

            txtPanDate.Text = "";
            txtSogen.Text = "";
            txtSName.Text = "";
            txtResult1.Text = ""; txtResult21.Text = ""; txtResult22.Text = "";
            txtWrtNo.Text = "";

            ssPatInfo.ActiveSheet.Cells[0, 0].Text = ""; ssPatInfo.ActiveSheet.Cells[0, 0].Text = "";
            ssPatInfo.ActiveSheet.Cells[0, 1].Text = "";
            ssPatInfo.ActiveSheet.Cells[0, 2].Text = "";
            ssPatInfo.ActiveSheet.Cells[0, 3].Text = "";
            ssPatInfo.ActiveSheet.Cells[0, 4].Text = "";
            ssPatInfo.ActiveSheet.Cells[0, 5].Text = "";

            sp.Spread_All_Clear(SS2);
            SS2.ActiveSheet.RowCount = 40;

            txtPanDrNo.Text = "";
            lblDrName.Text = "";
        }

        void fn_Panjeng_End_Check()
        {
            string strOK = "";
            long nPanDrno = 0;
            string strPanDate = "";
            int result = 0;
            string strGbPanjeng = "";

            strOK = "OK";
            nPanDrno = 0;
            strPanDate = "";

            clsDB.setBeginTran(clsDB.DbCon);

            //건강보험1차 판정완료 Check
            HIC_RES_ETC list = hicResEtcService.GetItembyWrtNo(FnWRTNO, "1");

            if (list.IsNullOrEmpty())
            {
                strOK = "NO";
            }
            else
            {
                if (list.PANJENGDRNO == 0)
                {
                    strOK = "NO";
                }
                nPanDrno = list.PANJENGDRNO;
                strPanDate = list.PANJENGDATE.ToString();
            }

            if (strOK == "OK")
            {
                strGbPanjeng = "Y";
            }
            else
            {
                strGbPanjeng = "";
            }

            //판정완료/미완료 SET
            result = hicResEtcService.UpdatebyWrtNoGubun(strGbPanjeng, FnWRTNO, "1");

            if (result < 0)
            {
                MessageBox.Show("판정완료/미완료 설정 중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }

            //접수마스타에 판정일자 Update
            result = hicJepsuService.UpdatePanjengDatebyWrtNo(strPanDate, nPanDrno, FnWRTNO, strOK);

            if (result < 0)
            {
                MessageBox.Show("판정완료/미완료 설정 중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);

            if (strOK == "OK")
            {
                if (MessageBox.Show("판정이 완료 되었습니다." + "\r\n\r\n" + "화면을 Clear 한 후 다음 환자를 판정하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question)  == DialogResult.Yes)
                {
                    fn_Screen_Clear();
                    eBtnClick(btnSearch, new EventArgs());
                    ssList.Focus();
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
            string strExPan = "";    //검사1건의 판정결과
            string strGbSPC = "";

            int nGan1 = 0;
            int nGan2 = 0;
            int nGanResult = 0;

            txtResult1.Text = "";
            txtResult21.Text = "";
            txtResult22.Text = "";

            tab11.Text = "";           
            tab12.Text = "";

            //Screen_Injek_display       '인적사항을 Display
            HIC_JEPSU list = hicJepsuService.GetItembyWrtNo(FnWRTNO);

            if (list.IsNullOrEmpty())
            {
                MessageBox.Show(FnWRTNO + "접수번호가 등록 안됨", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            strIpsadate = list.IPSADATE.ToString();
            FnPano = list.PANO;
            strSex = list.SEX;
            strJepDate = list.JEPDATE;
            clsHcVariable.GstrGjYear = list.GJYEAR;
            FstrSex = strSex;
            FstrJepDate = strJepDate;

            ssPatInfo.ActiveSheet.Cells[0, 0].Text = list.PANO.ToString();
            ssPatInfo.ActiveSheet.Cells[0, 1].Text = list.SNAME.Trim();
            ssPatInfo.ActiveSheet.Cells[0, 2].Text = list.AGE + "/" + strSex;
            ssPatInfo.ActiveSheet.Cells[0, 3].Text = hb.READ_Ltd_Name(list.LTDCODE.ToString());
            ssPatInfo.ActiveSheet.Cells[0, 4].Text = strJepDate;
            ssPatInfo.ActiveSheet.Cells[0, 5].Text = hb.READ_GjJong_Name(list.GJJONG);  //건진유형
            strGjJong = list.GJJONG;
            strIpsadate = list.IPSADATE.ToString();

            btnPacs.Enabled = false;
            btnMed.Enabled = false;

            //주민등록번호로 원무행정의 등록번호를 찾음
            HIC_PATIENT list2 = hicPatientService.GetItembyPaNo(FnPano);

            if (!list2.IsNullOrEmpty())
            {
                FstrJumin = clsAES.DeAES(list2.JUMIN2);
                FstrPano = list.PTNO.Trim();
            }

            //주민등록번호로 환자 등록번호 찾기
            if (FstrPano.IsNullOrEmpty())
            {
                FstrPano = basPatientService.GetPaNOByJuminNo(VB.Left(FstrJumin, 6), VB.Right(FstrJumin, 7), clsAES.AES(VB.Right(FstrJumin, 7)));
            }

            if (!FstrPano.IsNullOrEmpty())
            {
                btnPacs.Enabled = true;
            }

            //종합검진을 하였는지 점검함
            if (hicPatientService.GetCountbyJumin2(clsAES.AES(FstrJumin)) > 0)
            {
                btnMed.Enabled = true;
            }

            //검사결과를 재판정
            hm.ExamResult_RePanjeng(FnWRTNO, FstrSex, FstrJepDate, "");

            //Screen_Exam_Items_display  '검사항목을 Display
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

            //Screen_Munjin_Display      '문지표를 Display
            //건강검진 문진표 및 결과를  READ
            HIC_RES_ETC list4 = hicResEtcService.GetItembyWrtNo(FnWRTNO, "1");

            if (list4 == null)
            {
                MessageBox.Show("접수번호 " + FnWRTNO + " 는 결과 및 판정이 등록 안됨", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //----------( 판정일자,판정의사,판정결과 )---------------------------------
            txtPanDate.Text = "";
            if (!list4.PANJENGDATE.ToString().IsNullOrEmpty())    //판정일자
            {
                txtPanDate.Text = list4.PANJENGDATE.ToString();
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
            nLicense = list4.PANJENGDRNO;  //의사면허번호
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
                txtPanDate.Text = list4.PANJENGDATE.ToString();
            }
            else
            {
                txtPanDate.Text = clsPublic.GstrSysDate;
            }

            //혈액종합판정 및 소견
            txtSogen.Text = list4.SOGEN;

            fn_OLD_Result_Display(FnPano, strJepDate, strSex);  //종전결과 3개를 Display

            tabControl1.SelectedTab = tab11;
            //의사 이외 저장 금지
            if (clsHcVariable.GnHicLicense == 0) btnOK.Enabled = false;
        }

        void fn_OLD_Result_Display(long ArgPano, string ArgJepDate, string ArgSex)
        {
            //int nRow = 0;
            //int nOldCNT = 0;
            //string strExamCode = "";
            //string strAllWRTNO = "";
            //string strJepDate = "";
            //string strExPan = "";

            //int nHyelH = 0;
            //int nHyelL = 0;
            //int nHeight = 0;
            //int nWeight = 0;
            //int nResult = 0;

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
            List<HIC_JEPSU_LTD_RES_BOHUM1> list = hicJepsuLtdResBohum1Service.GetItembyPaNoJepDate(ArgPano, ArgJepDate, "Y");

            nOldCNT = list.Count;
            strAllWRTNO = "";
            
            for (int i = 0; i < nOldCNT; i++)
            {
                if (i >= 2) break;
                strAllWRTNO += list[i].WRTNO.ToString() + ",";
                strJepDate = list[i].JEPDATE.ToString();
                SS2_Sheet1.ColumnHeader.Cells.Get(0, i + 5).Value = VB.Left(strJepDate, 4) + VB.Mid(strJepDate, 6, 2) + VB.Right(strJepDate, 2);
                tabControl1.SelectedTabIndex = i + 1;
                tabControl1.SelectedTab.Text = strJepDate;
                fn_OLD_Panjeng_Display(i, list[i].WRTNO);
                fn_OLD_Result_Display_SUB(list[i].WRTNO, strExamCode, ArgSex, i);
            }
        }

        /// <summary>
        /// 종전 1회 검사의 결과를 Display
        /// </summary>
        void fn_OLD_Result_Display_SUB(long nWrtNo, List<string> argExamCode, string argSex, int index)
        {
            int nRow = 0;

            strOldJepsuDate = hm.GET_HIC_JepsuDate(nWrtNo);
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
            if (!list.SOGEN.IsNullOrEmpty())
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

        string READ_BAS_ILLS(string argCode)
        {
            string rtnVal = "";

            if (argCode.Trim().IsNullOrEmpty())
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

            if (txtPanDrNo.Text.Trim().IsNullOrEmpty())
            {
                MessageBox.Show("판정의사공란", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                clsHcVariable.GnHicLicense = long.Parse(txtPanDrNo.Text);
            }
            txtSogen.Text = txtSogen.Text.Trim();

            if (txtPanDate.Text.Trim().IsNullOrEmpty())
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
            item.GUBUN = "1";

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
