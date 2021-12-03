using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHcExcelList.cs
/// Description     : 분진촬영 누락작업
/// Author          : 이상훈
/// Create Date     : 2019-09-10
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm엑셀명단조회.frm(Frm엑셀명단조회)" />

namespace HC_Main
{
    public partial class frmHcExcelList : Form
    {
        HeaExcelLtdService heaExcelLtdService = null;
        HicReadingService hicReadingService = null;
        HicLtdService hicLtdService = null;
        HeaExcelService heaExcelService = null;
        HicPatientHeaJepsuService hicPatientHeaJepsuService = null;

        public delegate void SetHaJepsuGstrValue(string GSetValue);
        public static event SetHaJepsuGstrValue rSetHaJepsuGstrValue;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();

        string FstrROWID;
        string FstrRetValue;

        public frmHcExcelList()
        {
            InitializeComponent();

            SetEvents();
        }

        private void SetEvents()
        {
            heaExcelLtdService = new HeaExcelLtdService();
            hicReadingService = new HicReadingService();
            hicLtdService = new HicLtdService();
            heaExcelService = new HeaExcelService();
            hicPatientHeaJepsuService = new HicPatientHeaJepsuService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnFind.Click += new EventHandler(eBtnClick);
            this.btnRef.Click += new EventHandler(eBtnClick);
            this.btnSug.Click += new EventHandler(eBtnClick);
            this.btnRsvRemove.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);   
            this.btnMemoSave.Click += new EventHandler(eBtnClick);
            this.btnClose1.Click += new EventHandler(eBtnClick);
            this.btnClose2.Click += new EventHandler(eBtnClick);
            this.btnClose3.Click += new EventHandler(eBtnClick);
            this.btnHelpView.Click += new EventHandler(eBtnClick); 

            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.chkNoRsv.Click += new EventHandler(eChkBoxCheck);
            this.chkNoRsvEndCompany.Click += new EventHandler(eChkBoxCheck);
            this.chkNoRsvEnd.Click += new EventHandler(eChkBoxCheck);
            this.cboYear.Click += new EventHandler(eCboClick);
            this.txtJumin.KeyPress += new KeyPressEventHandler(etxtKeyPress);
            this.txtHPhone.KeyPress += new KeyPressEventHandler(etxtKeyPress);
            this.txtSName.KeyPress += new KeyPressEventHandler(etxtKeyPress);
            this.txtSName1.KeyPress += new KeyPressEventHandler(etxtKeyPress);
            this.txtTel.KeyPress += new KeyPressEventHandler(etxtKeyPress);
            this.txtViewSangho.GotFocus += new EventHandler(etxtGotFocus);
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            int nYY = 0;

            cboLtd.Items.Clear();
            txtSName.Text = "";
            txtBirth.Text = "";
            lblLtd.Text = "";
            txtLtdRemark.Text = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            nYY = VB.Left(clsPublic.GstrSysDate, 4).To<int>();            
            for (int i = 0; i < 5; i++)
            {
                cboYear.Items.Add(string.Format("{0:0000}", nYY));
                nYY -= 1;
            }

            cboYear.SelectedIndex = 0;

            List<HEA_EXCEL_LTD> list = heaExcelLtdService.GetLtdNamebyYear(cboYear.Text, "", "");

            if (list.Count == 0)
            {
                cboLtd.SelectedIndex = -1;
                return;
            }

            cboLtd.Items.Clear();
            cboLtd.Items.Add("*****.전체");
            for (int i = 0; i < list.Count; i++)
            {
                cboLtd.Items.Add(list[i].LTDCODE + "." + list[i].NAME);
            }
            cboLtd.SelectedIndex = 0;
            btnSug.Enabled = false;
        }

        void eChkBoxCheck(object sender, EventArgs e)
        {
            if (sender == chkNoRsv)                 //미예약자
            {
                //int nYY = 0;
                //string strChk = "";

                //if (chkNoRsvEndCompany.Checked == true)
                //{
                //    strChk = "1";
                //}

                //cboLtd.Items.Clear();
                //List<HEA_EXCEL_LTD> list = heaExcelLtdService.GetLtdNamebyYear(cboYear.Text, strChk, "");

                //sp.Spread_All_Clear(SS1);
                //SS1.ActiveSheet.RowCount = list.Count;
                //for (int i = 0; i < list.Count; i++)
                //{
                //    SS1.ActiveSheet.Cells[i, 0].Text = string.Format("{0:#00000}", list[i].LTDCODE) + "." + list[i].NAME;
                //}
            }
            else if (sender == chkNoRsvEndCompany)  //예약미완료 회사제외
            {
                string strChk = "";

                if (chkNoRsvEndCompany.Checked == true)
                {
                    strChk = "OK";
                }
                else
                {
                    strChk = "";
                }

                cboLtd.Items.Clear();
                List<HEA_EXCEL_LTD> list = heaExcelLtdService.GetItemAllbyYear(cboYear.Text, strChk);
                cboLtd.Items.Add("*****.전체");
                for (int i = 0; i < list.Count; i++)
                {
                    cboLtd.Items.Add(string.Format("{0:#00000}", list[i].LTDCODE) + "." + list[i].NAME);
                }
                //cboLtd.SetItems(list, "NAME", "CODE", "", "", AddComboBoxPosition.Top);
            }
            else if (sender == chkNoRsvEnd)         //예약완료 제외
            {

            }
        }

        void eCboClick(object sender, EventArgs e)
        {
            if (sender == cboYear)
            {
                string strChk = "";

                List<HEA_EXCEL_LTD> list = heaExcelLtdService.GetLtdNamebyYear(cboYear.Text, strChk, "");

                cboLtd.Items.Clear();
                cboLtd.Items.Add("*****.전체");
                for (int i = 0; i < list.Count; i++)
                {
                    cboLtd.Items.Add(string.Format("{0:#00000}", list[i].LTDCODE) + "." + list[i].NAME);
                }
                cboLtd.SelectedIndex = 0;

                SS1.ActiveSheet.RowCount = 29;
                sp.Spread_All_Clear(SS1);
            }
        }

        void etxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {   
                SendKeys.Send("{TAB}");
            }
        }

        void etxtGotFocus(object sender, EventArgs e)
        {
            if (sender == txtViewSangho)
            {
                txtViewSangho.ImeMode = ImeMode.Hangul;
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Hide();
                return;
            }
            else if (sender == btnFind)
            {
                eBtnClick(btnHelpView, new EventArgs());
                pnlLtdSearch.Visible = true;
                txtViewSangho.Text = "";
                txtViewSangho.Focus();
            }
            else if (sender == btnRef)  //참고
            {
                if (VB.Pstr(cboLtd.Text, ".", 1) != "*****")
                {
                    pnlLtdHelp.Visible = true;
                    lblLtd.Text = VB.Pstr(cboLtd.Text, ".", 2);
                    txtLtdRemark.Text = "";
                    txtLtdRemark.Text = hicLtdService.GetHaRemarkbyLtdCode(long.Parse(VB.Pstr(cboLtd.Text, ".", 1)));
                }
            }
            else if (sender == btnSug)
            {
                string strFileName = "";
                string strServerName = "";
                string strExtension = "";
                string strName = "";
                string strROWID = "";
                string strCODE = "";
                string strCODE1 = "";
                string strLtdCode = "";
                string sDirPath = "";
                string strHost = "";

                sDirPath = "C:\\HICEXCEL";

                DirectoryInfo Dir = new DirectoryInfo(sDirPath);

                if (Dir.Exists == false)
                {
                    Dir.Create();
                }

                strLtdCode = VB.Pstr(cboLtd.Text, ".", 1);
                strLtdCode = string.Format("{0:00000}", strLtdCode.To<int>());

                List<HIC_READING> list = hicReadingService.GetListByYear(cboYear.Text, strLtdCode);

                if (list.Count > 0)
                {
                    strCODE = list[0].CODE;
                    strCODE1 = list[0].NAME;
                    strName = list[0].DATA1;
                    strROWID = list[0].RID;
                }

                txtPcDocuFile1.Text = strName;

                if (txtPcDocuFile1.Text == "")
                {
                    MessageBox.Show("첨부파일이 없습니다", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (VB.Mid(VB.Right(txtPcDocuFile1.Text.Trim(), 4), 1, 1) == ".")
                {
                    strExtension = "*" + VB.Right(txtPcDocuFile1.Text.Trim(), 4);
                }
                else if (VB.Mid(VB.Right(txtPcDocuFile1.Text.Trim(), 5), 1, 1) == ".")
                {
                    strExtension = "*" + VB.Right(txtPcDocuFile1.Text.Trim(), 5);
                }

                if (VB.Right(txtPcDocuFile1.Text, 4) == "xlsx")
                {
                    strFileName = "c:\\HICEXCEL\\AAA.xlsx";
                }
                else
                {
                    strFileName = "c:\\HICEXCEL\\AAA" + VB.Right(txtPcDocuFile1.Text.Trim(), 4);
                }

                strHost = "/data/DOCU_READING/";
                strServerName = "/data/DOCU_READING/" + txtPcDocuFile1.Text.Trim();

                Ftpedt FtpedtX = new Ftpedt();

                if (FtpedtX.FtpDownload("192.168.100.31", "oracle", FtpedtX.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle"), strFileName, strServerName, strHost) == true)
                {
                    Process.Start(strFileName);
                }
                else
                {
                    MessageBox.Show("파일이 존재하지 않습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else if (sender == btnSearch)
            {
                int nREAD = 0;
                int nRow = 0;
                string strYEAR = "";
                long nLtd = 0;
                string strSname = "";
                string strBirth = "";
                string strOK = "";
                string strOLD = "";
                string strLTDNAME = "";
                string strJumin = "";
                string strLtdCode = "";
                string strNoRsv = "";

                //btnRef.Enabled = false;

                if (cboYear.Text.Trim() == "")
                {
                    MessageBox.Show("검진년도를 선택하세요", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                nLtd = VB.Pstr(cboLtd.Text, ".", 1).To<long>();
                strSname = txtSName.Text.Trim();
                strBirth = txtBirth.Text.Trim();
                if (nLtd != 0)
                {
                    strLTDNAME = VB.Pstr(cboLtd.Text, ".", 2);
                }

                SS1.ActiveSheet.RowCount = 29;

                if (chkNoRsv.Checked == true)
                {
                    strNoRsv = "Y";
                }
                else if (chkNoRsv.Checked == false)
                {
                    strNoRsv = "";
                }

                List<HEA_EXCEL> list = heaExcelService.GetAll(cboYear.Text, nLtd, strSname, strBirth, strNoRsv);

                nREAD = list.Count;
                SS1.ActiveSheet.RowCount = nREAD;
                nRow = 0;
                strOLD = "";

                progressBar1.Maximum = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    strOK = "OK";                    
                    //if (nRow > SS1.ActiveSheet.RowCount)
                    //{
                    //    SS1.ActiveSheet.RowCount = nRow;
                    //}
                    if (nLtd == 0)
                    {
                        if (string.Format("{0:#00000}", list[i].LTDCODE) != strOLD)
                        {
                            strOLD = string.Format("{0:#00000}", list[i].LTDCODE);
                            strLTDNAME = hb.READ_Ltd_Name(strOLD);
                        }
                    }
                    strJumin = clsAES.DeAES(list[i].AES_JUMIN);
                    SS1.ActiveSheet.Cells[i, 0].Text = list[i].SNAME;
                    SS1.ActiveSheet.Cells[i, 1].Text = "";
                    if (!list[i].MEMO.IsNullOrEmpty())
                    {
                        SS1.ActiveSheet.Cells[i, 1].Text = "◎";
                    }
                    SS1.ActiveSheet.Cells[i, 2].Text = list[i].ENTDATE;
                    //SS1.ActiveSheet.Cells[i, 3].Text = VB.Left(strJumin, 7) + "******";
                    //2021-06-04(김동열 부장님 요청사항으로 생년월일 전체표시)
                    SS1.ActiveSheet.Cells[i, 3].Text = strJumin;
                    SS1.ActiveSheet.Cells[i, 4].Text = list[i].HDATE; 
                    if (!list[i].RDATE.IsNullOrEmpty())
                    {
                        SS1.ActiveSheet.Cells[i, 4].Text = list[i].RDATE;
                        SS1.ActiveSheet.Cells[i, 5].Text = "Y";
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[i, 5].Text = "N";
                    }
                    SS1.ActiveSheet.Cells[i, 6].Text = list[i].AMPM;
                    SS1.ActiveSheet.Cells[i, 7].Text = list[i].REL;
                    SS1.ActiveSheet.Cells[i, 8].Text = list[i].GJTYPE;
                    SS1.ActiveSheet.Cells[i, 9].Text = list[i].LTDADDEXAM;
                    SS1.ActiveSheet.Cells[i, 10].Text = list[i].BONINADDEXAM;
                    SS1.ActiveSheet.Cells[i, 11].Text = list[i].GAJOKADDEXAM;
                    SS1.ActiveSheet.Cells[i, 12].Text = list[i].HPHONE;
                    SS1.ActiveSheet.Cells[i, 13].Text = list[i].JUSO;
                    SS1.ActiveSheet.Cells[i, 14].Text = list[i].REMARK;
                    SS1.ActiveSheet.Cells[i, 15].Text = list[i].JNAME;
                    SS1.ActiveSheet.Cells[i, 16].Text = list[i].TEL;
                    SS1.ActiveSheet.Cells[i, 17].Text = list[i].MCODES;
                    SS1.ActiveSheet.Cells[i, 18].Text = list[i].GBSAMU;
                    SS1.ActiveSheet.Cells[i, 19].Text = list[i].GBNIGHT;
                    SS1.ActiveSheet.Cells[i, 20].Text = list[i].GBNHIC;
                    SS1.ActiveSheet.Cells[i, 21].Text = list[i].HOSPITAL;
                    SS1.ActiveSheet.Cells[i, 22].Text = list[i].IPSADATE;
                    SS1.ActiveSheet.Cells[i, 23].Text = list[i].GKIHO;
                    SS1.ActiveSheet.Cells[i, 24].Text = list[i].LTDBUSE;
                    SS1.ActiveSheet.Cells[i, 25].Text = list[i].JIKNAME;
                    SS1.ActiveSheet.Cells[i, 26].Text = list[i].LTDSABUN;
                    SS1.ActiveSheet.Cells[i, 27].Text = list[i].LTDCODE.To<string>();
                    SS1.ActiveSheet.Cells[i, 28].Text = strLTDNAME;
                    SS1.ActiveSheet.Cells[i, 29].Text = list[i].BIRTH;
                    SS1.ActiveSheet.Cells[i, 30].Text = strJumin;
                    SS1.ActiveSheet.Cells[i, 31].Text = list[i].RID;
                    SS1.ActiveSheet.Cells[i, 32].Text = list[i].ENTDATE1;
                    SS1.ActiveSheet.Cells[i, 33].Text = hb.READ_HIC_InsaName(list[i].ENTSABUN.To<string>());
                    SS1.ActiveSheet.Cells[i, 34].Text = list[i].UPDATETIME;
                    SS1.ActiveSheet.Cells[i, 35].Text = hb.READ_HIC_InsaName(list[i].MODIFIEDUSER);

                    progressBar1.Value = i + 1;
                }

                strLtdCode = VB.Pstr(cboLtd.Text, ".", 1);
                strLtdCode = string.Format("{0:00000}", strLtdCode.To<int>());
                List<HIC_READING> list2 = hicReadingService.GetListByYear(cboYear.Text, strLtdCode);

                if (list2.Count > 0)
                {
                    btnSug.Enabled = true;
                }
            }
            else if (sender == btnRsvRemove)
            {
                int nREAD = 0;
                int nRate = 0;
                string strROWID = "";
                string strJumin = "";
                string strSname = "";
                string strBirth = "";
                string StrRDate = "";
                string strRDate1 = "";
                string strSEX = "";
                long nAge = 0;
                long nPano = 0;
                long nWRTNO = 0;

                string strAesJumin = "";
                string strYearFr = "";
                string strYearTo = "";

                //Frame작업상태.Visible = True
                //ProgressBar1.Value = 0
                //LabelRate.Caption = ""
                //DoEvents

                List<HEA_EXCEL> list = heaExcelService.GetRowIdbyLtdCode(VB.Left(clsPublic.GstrSysDate, 4), VB.Pstr(cboLtd.Text, ".", 1));

                nREAD = list.Count;
                progressBar1.Maximum = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    strROWID = list[i].RID;

                    HEA_EXCEL list2 = heaExcelService.GetAllbyRowId(strROWID);

                    strJumin = clsAES.DeAES(list2.AES_JUMIN);
                    strAesJumin = list2.AES_JUMIN;
                    StrRDate = list2.RDATE;
                    strSname = list2.SNAME;
                    strBirth = list2.BIRTH;
                    strYearFr = cboYear.Text + "-01-01";
                    strYearTo = cboYear.Text + "-12-31";

                    //예약내역을 읽음
                    if (strJumin == "" && strBirth == "")
                    {
                        strRDate1 = "";
                    }
                    else
                    {
                        HIC_PATIENT_HEA_JEPSU list3 = hicPatientHeaJepsuService.GetItembyJuminOrBirth(strJumin, strAesJumin, strBirth, strYearFr, strYearTo, strSname);

                        nPano = 0;
                        nWRTNO = 0;
                        if (!list3.IsNullOrEmpty())
                        {
                            nPano = list3.PANO;
                            nWRTNO = list3.WRTNO;
                            strSname = list3.SNAME;
                            strSEX = list3.SEX;
                            nAge = list3.AGE;
                            strRDate1 = list3.SDATE;
                        }
                        else
                        {
                            strRDate1 = "";
                        }
                    }

                    if (StrRDate != strRDate1 && !strRDate1.IsNullOrEmpty())
                    {
                        HEA_EXCEL item = new HEA_EXCEL();

                        item.RDATE = strRDate1;
                        item.SEX = strSEX;
                        item.AGE = nAge;
                        item.MODIFIEDUSER = clsType.User.IdNumber;
                        item.RID = strROWID;

                        int result = heaExcelService.Update_RDate(item);

                        if (result < 0)
                        {
                            MessageBox.Show("종합검진 예약용 엑셀 자료 갱신 중 오류발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    progressBar1.Value = i + 1;
                }

                eBtnClick(btnSearch, new EventArgs());

                MessageBox.Show("예약정리 완료!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                strTitle = "종검 예약 엑셀 명단";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("검진회사:" + cboLtd.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                strHeader += sp.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);

                ///TODO : 이상훈(2019.09.11) 출력 Log 여부 확인
                //SQL_LOG("", SS1.PrintHeader);
            }
            else if (sender == btnClose1)   //회사찾기 닫기
            {
                pnlLtdSearch.Visible = false;
            }
            else if (sender == btnClose2)   //회사명 닫기
            {
                lblLtd.Text = "";
                txtLtdRemark.Text = "";
                pnlLtdHelp.Visible = false;
            }
            else if (sender == btnClose3)   //메모닫기
            {
                lblSName.Text = "";
                txtRemark.Text = "";
                pnlMsg.Visible = false;
            }
            else if (sender == btnHelpView)
            {
                int nYY = 0;
                string strChk = "";

                if (chkNoRsvEndCompany.Checked == true)
                {
                    strChk = "1";
                }

                txtViewSangho.Text = txtViewSangho.Text.Trim();

                List<HEA_EXCEL_LTD> list = heaExcelLtdService.GetLtdNamebyYear(cboYear.Text, strChk, txtViewSangho.Text.Trim());
                SSList.ActiveSheet.RowCount = list.Count;
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        SSList.ActiveSheet.Cells[i, 0].Text = list[i].LTDCODE.To<string>();
                        SSList.ActiveSheet.Cells[i, 1].Text = list[i].NAME;
                    }
                }
            }
            else if (sender == btnMemoSave) //메모저장
            {
                string strJumin = "";
                string strSName = "";
                string strBirth = "";

                strSName = txtSName.Text.Trim();
                strJumin = VB.TR(txtJumin.Text.Trim(), "-", "");
                if (strJumin.Length == 13)
                {
                    string ErrCheck = ComFunc.JuminNoCheck(clsDB.DbCon, VB.Left(strJumin, 6), VB.Right(strJumin, 7));

                    if (ErrCheck.Trim() != "")
                    {
                        MessageBox.Show(strSName + "님 주민번호가 정확하지 않습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                HEA_EXCEL item = new HEA_EXCEL();

                item.MEMO = txtRemark.Text.Trim();
                item.AES_JUMIN = clsAES.AES(strJumin);
                item.BIRTH = VB.Left(strJumin, 6);
                item.SNAME = strSName;
                item.HPHONE = txtHPhone.Text.Trim();
                item.TEL = txtTel.Text.Trim();
                item.MODIFIEDUSER = clsType.User.IdNumber;
                item.RID = FstrROWID;

                int result = heaExcelService.Update(item);

                if (result < 0)
                {
                    MessageBox.Show("종합검진 예약용 엑셀 자료 갱신 중 오류 발생!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                lblSName.Text = "";
                txtRemark.Text = "";
                pnlMsg.Visible = false;
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            string strJumin = "";
            string strBirth = "";
            string strSname = "";
            string strLtdCode = "";
            string strLtdBuse = "";
            string strJuso = "";
            string strSabun = "";
            string strHPhone = "";
            string strTel = "";
            string strRel = "";
            string strROWID = "";

            if (sender == SS1)
            {
                if (e.ColumnHeader == true || e.RowHeader == true)
                {
                    sp.setSpdSort(SS1, e.Column, true);
                    return;
                }

                //메모 등록
                if (e.Column == 1)
                {
                    strSname = SS1.ActiveSheet.Cells[e.Row, 0].Text.Trim();
                    strHPhone = SS1.ActiveSheet.Cells[e.Row, 12].Text.Trim();
                    strTel = SS1.ActiveSheet.Cells[e.Row, 16].Text.Trim();
                    strJumin = SS1.ActiveSheet.Cells[e.Row, 30].Text.Trim();
                    FstrROWID = SS1.ActiveSheet.Cells[e.Row, 31].Text.Trim();
                    lblSName.Text = strSname + " " + VB.Left(strJumin, 6) + " " + strHPhone;

                    //메모를 읽음                    
                    if (!FstrROWID.IsNullOrEmpty())
                    {
                        pnlMsg.Visible = true;
                        txtRemark.Text = heaExcelService.GetMemobyRowId(FstrROWID);
                    }
                    
                    if (strJumin.Length == 13)
                    {
                        txtJumin.Text = VB.Left(strJumin, 6) + "-" + VB.Right(strJumin, 7);
                    }
                    else
                    {
                        txtJumin.Text = strJumin;
                    }
                    txtSName1.Text = strSname;
                    txtHPhone.Text = strHPhone;
                    txtTel.Text = strTel;

                    //Frame메세지.Visible = True;
                }
                else
                {
                    strSname = SS1.ActiveSheet.Cells[e.Row, 0].Text.Trim();
                    strRel = SS1.ActiveSheet.Cells[e.Row, 7].Text.Trim();
                    strHPhone = SS1.ActiveSheet.Cells[e.Row, 12].Text.Trim();
                    strJuso = SS1.ActiveSheet.Cells[e.Row, 13].Text.Trim();
                    strTel = SS1.ActiveSheet.Cells[e.Row, 16].Text.Trim();
                    strLtdBuse = SS1.ActiveSheet.Cells[e.Row, 24].Text.Trim();
                    strSabun = SS1.ActiveSheet.Cells[e.Row, 26].Text.Trim();
                    strLtdCode = SS1.ActiveSheet.Cells[e.Row, 27].Text.Trim();
                    strBirth = SS1.ActiveSheet.Cells[e.Row, 29].Text.Trim();
                    strJumin = SS1.ActiveSheet.Cells[e.Row, 30].Text.Trim();
                    strROWID = SS1.ActiveSheet.Cells[e.Row, 31].Text.Trim();

                    if (strJumin.Length == 13)
                    {
                        FstrRetValue = VB.Left(strJumin, 6) + "{}" + VB.Mid(strJumin, 7, 7) + "{}";
                    }
                    else if (strBirth.Length == 6)
                    {
                        FstrRetValue = strBirth + "{}{}";
                    }
                    else
                    {
                        FstrRetValue = VB.Left(strJumin, 6) + "{}" + VB.Mid(strJumin, 7, 7) + "{}";
                    }
                    FstrRetValue += strSname + "{}";        //3 
                    FstrRetValue += strLtdCode + "{}";      //4
                    FstrRetValue += cboYear.Text + "{}";    //5
                    FstrRetValue += strLtdBuse + "{}";      //6
                    FstrRetValue += strJuso + "{}";         //7
                    FstrRetValue += strSabun + "{}";        //8
                    FstrRetValue += strHPhone + "{}";       //9
                    FstrRetValue += strTel + "{}";          //10
                    FstrRetValue += strRel + "{}";          //11
                    FstrRetValue += strROWID + "{}";        //12

                    if (!rSetHaJepsuGstrValue.IsNullOrEmpty())
                    {
                        rSetHaJepsuGstrValue(FstrRetValue);
                        this.Hide();
                        return;
                    }
                }
            }
            else if (sender == SSList)
            {
                cboLtd.Text = SSList.ActiveSheet.Cells[e.Row, 0].Text + "." + SSList.ActiveSheet.Cells[e.Row, 1].Text;
                pnlLtdSearch.Visible = false;
                eBtnClick(btnSearch, new EventArgs());
            }
        }
    }
}
