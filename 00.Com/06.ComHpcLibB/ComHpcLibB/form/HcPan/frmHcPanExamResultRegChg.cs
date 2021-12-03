using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using ComLibB;
using ComPmpaLibB;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcPanExamResultRegChg.cs
/// Description     : 검사결과 등록/변경 
/// Author          : 이상훈
/// Create Date     : 2019-12-03
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmResult1.frm(HcPan107)" />

namespace ComHpcLibB
{
    public partial class frmHcPanExamResultRegChg : Form
    {
        HicJepsuService hicJepsuService = null;
        HicResBohum1Service hicResBohum1Service = null;
        HicJepsuHeaExjongService hicJepsuHeaExjongService = null;
        ComHpcLibBService comHpcLibBService = null;
        HicResultExCodeService hicResultExCodeService = null;
        HicJepsuPatientService hicJepsuPatientService = null;
        HicPatientService hicPatientService = null;
        HeaJepsuService heaJepsuService = null;
        HicExcodeService hicExcodeService = null;
        HicResultService hicResultService = null;
        HicSunapdtlService hicSunapdtlService = null;
        HicResDentalService hicResDentalService = null;
        HicXrayResultService hicXrayResultService = null;
        XrayResultnewService xrayResultnewService = null;
        EndoJupmstResultService endoJupmstResultService = null;
        HicResultHisService hicResultHisService = null;
        HicJepsuResultService hicJepsuResultService = null;
        HicJepsuLtdResBohum1Service hicJepsuLtdResBohum1Service = null;
        HicRescodeService hicRescodeService = null;
        EtcJupmstService etcJupmstService = null;
        XrayResultnewDrService xrayResultnewDrService = null;
        ExamAnatmstService examAnatmstService = null;
        HicMemoService hicMemoService = null;
        HicCodeService hicCodeService = null;
        HicExjongService hicExjongService = null;
        HeaResultService heaResultService = null;
        HicLtdService hicLtdService = null;
        BasBcodeService basBcodeService = null;

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        frmHcPendListReg FrmHcPendListReg = null;
        frmViewResult FrmViewResult = null;
        frmHcResultInputCheckList FrmHcResultInputCheckList = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        long FnWRTNO;
        long FnWrtno2;
        long FnRowNo;               //메모리타자기 위치 저장용
        int FnClickRow;            //Help를 Click한 Row
        //string FstrPartExam;        //파트별 입력할 검사항목
        List<string> FstrPartExam = new List<string>();
        string FstrSex;             //성별
        string FstrJepDate;     
        string FstrGjJong;      
        string FstrGjChasu;     
        string FstrUCodes;      
        long FnAge;             
        string FstrGjYear;      
                                
        long FnHeaWRTNO;            //종합검진 접수번호
        string FstrPtno;
        string FstrSname;
        string FstrGubun = "";      //Y - 출장,원내 중 전체
        string FstrGubun1 = "";     //Y - 결과공백 체크X



        public frmHcPanExamResultRegChg()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }


        public frmHcPanExamResultRegChg(long nWrtNo, string strGubun, string strGubun1)
        {
            InitializeComponent();

            FnWRTNO = nWrtNo;
            FstrGubun = strGubun;
            FstrGubun1 = strGubun1;

            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicJepsuService = new HicJepsuService();
            hicResBohum1Service = new HicResBohum1Service();
            hicJepsuHeaExjongService = new HicJepsuHeaExjongService();
            comHpcLibBService = new ComHpcLibBService();
            hicResultExCodeService = new HicResultExCodeService();
            hicJepsuPatientService = new HicJepsuPatientService();
            hicPatientService = new HicPatientService();
            heaJepsuService = new HeaJepsuService();
            hicExcodeService = new HicExcodeService();
            hicResultService = new HicResultService();
            hicSunapdtlService = new HicSunapdtlService();
            hicResDentalService = new HicResDentalService();
            hicXrayResultService = new HicXrayResultService();
            xrayResultnewService = new XrayResultnewService();
            endoJupmstResultService = new EndoJupmstResultService();
            hicResultHisService = new HicResultHisService();
            hicJepsuResultService = new HicJepsuResultService();
            hicJepsuLtdResBohum1Service = new HicJepsuLtdResBohum1Service();
            hicRescodeService = new HicRescodeService();
            etcJupmstService = new EtcJupmstService();
            xrayResultnewDrService = new XrayResultnewDrService();
            examAnatmstService = new ExamAnatmstService();
            hicMemoService = new HicMemoService();
            hicCodeService = new HicCodeService();
            hicExjongService = new HicExjongService();
            heaResultService = new HeaResultService();
            hicLtdService = new HicLtdService();
            basBcodeService = new BasBcodeService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnInfo.Click += new EventHandler(eBtnClick);
            this.btnClose.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.btnPacs.Click += new EventHandler(eBtnClick);
            this.btnRef.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnMemoSave.Click += new EventHandler(eBtnClick);
            this.chkPanRslt.Click += new EventHandler(eChkClick);
            this.menuPrint.Click += new EventHandler(eMenuClick);
            this.menuHeaResult.Click += new EventHandler(eMenuClick);
            this.menuXrayno_Set1.Click += new EventHandler(eMenuClick);
            this.menuXrayPanResult.Click += new EventHandler(eMenuClick);
            this.menuSabunChk.Click += new EventHandler(eMenuClick);
            this.menuSetAll.Click += new EventHandler(eMenuClick);
            this.menuPanScreen.Click += new EventHandler(eMenuClick);
            this.menuHistory.Click += new EventHandler(eMenuClick);
            this.menuPatInfoModify.Click += new EventHandler(eMenuClick);
            this.menuPatStat.Click += new EventHandler(eMenuClick);
            this.menuResultInputChk.Click += new EventHandler(eMenuClick);
            this.menuHoldReport.Click += new EventHandler(eMenuClick);
            this.menuExit.Click += new EventHandler(eMenuClick);
            this.menuMemo.Click += new EventHandler(eMenuClick);
            

            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.ssList1.CellClick += new CellClickEventHandler(eSpdClick);

            this.SS2.CellClick += new CellClickEventHandler(eSpdClick);
            this.SS2.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SS2.EditModeOn += new EventHandler(eSpreadEditModeOn);
            this.SS2.EditModeOff += new EventHandler(eSpreadEditModeOff);
            this.SS2.Change += new ChangeEventHandler(eSpdChange);
            this.SS2.KeyDown += new KeyEventHandler(eSpdKeyDown);
            this.SS2.KeyUp += new KeyEventHandler(eSpreadKeyUp);
            this.SS2.LeaveCell += new LeaveCellEventHandler(eSpdLeaveCell);
            this.SS2.EditorFocused += ssUserChart_EditorFocused;

            this.SSList.KeyPress += new KeyPressEventHandler(eSpdKeyPress);
            this.txtWrtNo.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtSName.GotFocus += new EventHandler(eTxtGotFocus);
            //this.txtWrtNo.LostFocus += new EventHandler(eTxtLostFocus);

            
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            SS2_Sheet1.Columns.Get(7).Visible = false;  //결과값코드
            SS2_Sheet1.Columns.Get(8).Visible = false;  //변경
            SS2_Sheet1.Columns.Get(9).Visible = false;  //ROWID
            SS2_Sheet1.Columns.Get(10).Visible = false; //Result Type
            SS2_Sheet1.Columns.Get(11).Visible = false;
            SS2_Sheet1.Columns.Get(13).Visible = false;


            SS_Res.Visible = false;
            txtLtdCode.Text = "";

            dtpFrDate.Text = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-10).ToShortDateString();
            dtpToDate.Text = clsPublic.GstrSysDate;

            txtSName.Text = "";

            //입력담당
            cboPart.Items.Clear();
            cboPart.Items.Add("*.전체항목");

            List<HIC_CODE> list = hicCodeService.GetCodeNamebyGubun("72");

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    cboPart.Items.Add(list[i].CODE + "." + list[i].NAME);
                }
            }

            cboPart.SelectedIndex = 0;

            fn_Screen_Clear();
            cboJong.Items.Clear();
            cboJong.Items.Add("**.전체");
            List<HIC_EXJONG> list2 = hicExjongService.Read_ExJong_Add(true);
            if (list2.Count > 0)
            {
                for (int i = 0; i < list2.Count; i++)
                {
                    cboJong.Items.Add(list2[i].CODE + "." + list2[i].NAME);
                }
            }
            cboJong.SelectedIndex = 0;

            SS3.Visible = false;
            //SS4.Visible = false;
            btnRef.Visible = false;

            if (FnWRTNO > 0)
            {
                if (FstrGubun == "Y") { rdoGubun3.Checked = true; }
                if (FstrGubun1 == "Y") { chkNew.Checked = false; }
                txtWrtNo.Text = FnWRTNO.To<string>();
                eTxtKeyPress(txtWrtNo, new KeyPressEventArgs((char)Keys.Enter));
                
                //fn_Screen_Display();
            }
            txtLtdCode.Text = "";            
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            if (sender == btnClose)
            {
                panInfo.Visible = false;
            }

            else if (sender== btnInfo)
            {
                string strMsg = "";
                string strJuso = "";
                string strPrint = "";
                string strHtel = "";
                string strPtno = "";
                string strSname = "";
                string strJumin = "";
                string strMailCode = "";

                HIC_JEPSU_PATIENT item = hicJepsuPatientService.GetItembyWrtNo(FnWRTNO);

                strPtno = item.PTNO;
                strSname = item.SNAME;
                strMailCode = item.MAILCODE;

                if (!item.WEBPRINTREQ.IsNullOrEmpty())
                {
                    strPrint = "알림톡";
                }
                else if( item.GBCHK3 =="Y")
                {
                    strPrint = "방문수령";
                }
                else if (item.GBJUSO =="Y")
                {
                    strPrint = "우편(집)";
                }
                else if (item.GBJUSO == "N")
                {
                    strPrint = "우편(회사)";
                }


                strJuso = item.JUSO1 + " " + item.JUSO2;
                strHtel = item.HPHONE;
                strJumin = clsAES.DeAES(item.JUMIN2);

                //strMsg = "주소: "+ strJuso + "\r\n";
                //strMsg += "결과지수령방식: "+ strPrint + "\r\n";
                //strMsg += "연락처: "+ strHtel;


                SSInfo.ActiveSheet.Cells[0, 1].Text = ""; 
                SSInfo.ActiveSheet.Cells[0, 3].Text = ""; 
                SSInfo.ActiveSheet.Cells[1, 1].Text = ""; 
                SSInfo.ActiveSheet.Cells[1, 3].Text = ""; 
                SSInfo.ActiveSheet.Cells[2, 1].Text = ""; 
                SSInfo.ActiveSheet.Cells[2, 3].Text = ""; 
                SSInfo.ActiveSheet.Cells[3, 1].Text = "";
                //SSInfo.ActiveSheet.Cells[4, 1].Text = "";

                SSInfo.ActiveSheet.Cells[0, 1].Text = strPtno;
                SSInfo.ActiveSheet.Cells[0, 3].Text = strSname;
               
                SSInfo.ActiveSheet.Cells[1, 1].Text = VB.Left(strJumin, 6) + "-" + VB.Right(strJumin, 7);
                SSInfo.ActiveSheet.Cells[1, 3].Text = VB.Left(strJumin, 2) + "년" + VB.Mid(strJumin, 3, 2) + "월" + VB.Mid(strJumin, 5, 2) + "일";
                SSInfo.ActiveSheet.Cells[2, 1].Text = strPrint;
                SSInfo.ActiveSheet.Cells[2, 3].Text = strHtel;
                SSInfo.ActiveSheet.Cells[3, 1].Text = strJuso + "(" + strMailCode + ")";
                //SSInfo.ActiveSheet.Cells[4, 1].Text = strJuso;

                panInfo.Visible = true;

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
                int nRead = 0;
                int nREAD = 0;
                int nRow = 0;
                long nHeaPano = 0;
                long nHeaWRTNO = 0;

                string strPart = "";
                string strInOut = "";
                string strLtdCode = "";
                string strOK = "";

                string strFrDate = "";
                string strToDate = "";

                string strSName = "";
                string strChkNew = "";
                string strGbChul = "";
                long   nLtdCode = 0;
                string sgrGjJong = "";

                Cursor.Current = Cursors.WaitCursor;

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;

                strSName = txtSName.Text.Trim();
                if (chkNew.Checked == true)
                {
                    strChkNew = "1";
                }

                if (rdoGubun1.Checked == true)
                {
                    strGbChul = "1";
                }
                else if (rdoGubun2.Checked == true)
                {
                    strGbChul = "2";
                }

                if (txtLtdCode.Text.IsNullOrEmpty())
                {
                    nLtdCode = 0;
                }
                else
                {
                    nLtdCode = long.Parse(VB.Pstr(txtLtdCode.Text, ".", 1));
                }
                sgrGjJong = VB.Left(cboJong.Text, 2);

                fn_Screen_Clear();
                sp.Spread_All_Clear(SSList);
                SSList.ActiveSheet.RowCount = 50;

                txtSName.Text = txtSName.Text.Trim();
                if (clsType.User.IdNumber != "23515")
                {
                    menuResultInputChk.Visible = false;
                }

                strPart = VB.Left(cboPart.Text, 1);
                FstrPartExam.Clear();
                if (strPart == "*")
                {
                    FstrPartExam.Add("전체");
                }
                else
                {
                    List<HIC_EXCODE> list3 = hicExcodeService.GetCodebyEntPart(strPart);
                    FstrPartExam.Clear();

                    for (int i = 0; i < list3.Count; i++)
                    {
                        FstrPartExam.Add(list3[i].CODE);
                    }
                }

                //자료를 SELECT
                List<HIC_JEPSU> list = hicJepsuService.GetItembyJepDate(strFrDate, strToDate, strSName, strChkNew, strGbChul, nLtdCode, sgrGjJong);

                nREAD = list.Count;
                SSList.ActiveSheet.RowCount = nREAD;

                nRow = 0;
                progressBar1.Maximum = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    //해당 파트의 미입력 검사가 있는지 점검
                    if (strPart != "*" && FstrPartExam.IsNullOrEmpty())
                    {
                        strOK = "NO";
                    }
                    else
                    {
                        strOK = "NO";
                        if (hicResultService.GetCountbyWrtNoExCodeChkNew(list[i].WRTNO, strPart, strChkNew, FstrPartExam) > 0)
                        {
                            strOK = "OK";
                        }
                    }


                    if (strOK == "OK")
                    {
                        nRow += 1;
                        if (nRow > SSList.ActiveSheet.RowCount)
                        {
                            SSList.ActiveSheet.RowCount = nRow;
                        }

                        //SSList.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].SNAME;
                        //SSList.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].JEPDATE.ToString();
                        //SSList.ActiveSheet.Cells[nRow - 1, 2].Text = hb.READ_Ltd_Name(list[i].LTDCODE.ToString());
                        //SSList.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].GJJONG;
                        //SSList.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].WRTNO.To<string>();

                        //2021-05-10
                        SSList.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].WRTNO.To<string>();
                        SSList.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].SNAME;
                        SSList.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].GJJONG;
                        SSList.ActiveSheet.Cells[nRow - 1, 3].Text = hb.READ_Ltd_Name(list[i].LTDCODE.ToString());
                        SSList.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].JEPDATE.ToString();
                        SSList.ActiveSheet.Cells[nRow - 1, 5].Text = list[i].SEX;
                        SSList.ActiveSheet.Cells[nRow - 1, 6].Text = list[i].AGE.ToString();

                        //----------------------------------
                        //  종합건진 접수자인지 점검
                        //---------------------------------- 
                        nHeaPano = 0;
                        nHeaWRTNO = 0;

                        //종검 마스타에서 종검번호를 찾음
                        nHeaPano = hicPatientService.GetPanobyPaNo(list[i].PANO);

                        if (nHeaPano > 0)
                        {
                            nHeaWRTNO = heaJepsuService.GetWrtNobyHeaPaNoJepDateGbsts(nHeaPano, list[i].JEPDATE);

                            if (nHeaWRTNO > 0)
                            {
                                SSList.ActiveSheet.Cells[nRow - 1, 1].BackColor = lblHea.BackColor;
                            }
                        }
                        //2019-09-03(폐암검진 대상자 표시)
                        if (hicSunapdtlService.GetCountbyWrtNoCode(list[i].WRTNO, "3170") > 0)
                        {
                            SSList.ActiveSheet.Cells[nRow - 1, 0].BackColor = Color.FromArgb(255, 255, 0);
                        }
                    }
                    progressBar1.Value = i + 1;
                }

                SSList.ActiveSheet.RowCount = nRow;
                    
                Cursor.Current = Cursors.Default;
            }
            else if (sender == btnCancel)
            {
                fn_Screen_Clear();
                txtWrtNo.Focus();
            }
            else if (sender == btnSave)
            {
                string strResult  = "";
                string strCODE    = "";
                string strROWID   = "";
                string strPanjeng = "";
                string strNewPan  = "";
                string strChange  = "";
                string strResCode = "";
                int nHeight = 0;
                int nWeight = 0;
                int nResult = 0;
                string strBiman = "";
                double nEyeL = 0;
                double nEyeR = 0;
                int nEarL = 0;
                int nEarR = 0;
                int nBloodH = 0;
                int nBloodL = 0;
                long nSabun = 0;

                int result = 0;

                //접수마스타의 상태 설정용 변수
                string strGbSTS = "";
                int nNullCNT = 0;
                txtWrtNo.Focus();

                ///판정완료된 자료 수정 못하도록 
                HIC_JEPSU list = hicJepsuService.GetItembyWrtNo(txtWrtNo.Text.To<long>());

                if (!list.IsNullOrEmpty())
                {
                    if (clsType.User.IdNumber != "32971")
                    {
                        if (list.GBSTS == "3")
                        {
                            MessageBox.Show("판정이 완료된 자료는 Data를 수정할 수 없습니다!!", "수정불가", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            return;
                        }
                    }
                }

                if (fn_Police_Result_Check() == false)
                {
                    MessageBox.Show("경찰공무원 채용검진의 청력검사 결과를 수치로 입력해주세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //--------------------------------
                //  자료에 오류가 있는지 점검함
                //--------------------------------
                for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
                {
                    strCODE = SS2.ActiveSheet.Cells[i, 0].Text;
                    strResult = SS2.ActiveSheet.Cells[i, 2].Text;
                    if (!strResult.IsNullOrEmpty())
                    {
                        //혈압은 숫자만 가능함
                        if (strCODE == "A108" || strCODE == "A109")
                        {
                            if (VB.IsNumeric(strResult) == false)
                            {
                                MessageBox.Show(i + "번줄 헐압은 숫자만 가능합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                    }
                    if (!strResult.IsNullOrEmpty())
                    {
                        //혈압은 숫자만 가능함
                        if (strCODE == "A104" || strCODE == "A105" || strCODE == "C203" || strCODE == "C204")
                        {
                            if ((Convert.ToDouble(strResult.Replace("(", "").Replace(")", "")) > 2 || Convert.ToDouble(strResult.Replace("(", "").Replace(")", "")) < 0.1) && strResult.Replace("(", "").Replace(")", "") != ".")
                            {
                                if (Convert.ToDouble(strResult.Replace("(", "").Replace(")", "")) != 9.9)
                                {
                                    MessageBox.Show(i + "번줄 시력결과가 잘못 입력되었습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                            }
                        }
                    }
                }

                clsDB.setBeginTran(clsDB.DbCon);

                for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
                {
                    strCODE = SS2.ActiveSheet.Cells[i, 0].Text.Trim();
                    strResult = SS2.ActiveSheet.Cells[i, 2].Text.Trim();
                    strPanjeng = SS2.ActiveSheet.Cells[i, 5].Text.Trim();
                    strResCode = SS2.ActiveSheet.Cells[i, 7].Text.Trim();
                    strChange = SS2.ActiveSheet.Cells[i, 8].Text.Trim();
                    strROWID = SS2.ActiveSheet.Cells[i, 9].Text.Trim();
                    nSabun = long.Parse(SS2.ActiveSheet.Cells[i, 11].Text);

                    strNewPan = hm.ExCode_Result_Panjeng(strCODE, strResult, FstrSex, FstrJepDate, "").Trim();
                    if(strCODE=="ZD99") { strNewPan = strPanjeng; }

                    if ((strChange == "Y" || strPanjeng != strNewPan) && (nSabun != 111))
                    {
                        //History에 INSERT
                        result = hicResultHisService.Result_History_Insert(clsType.User.IdNumber, strResult, strROWID, "");

                        //결과를 저장
                        result = hicResultHisService.UpdatebyRowId(strResult, strNewPan, strResCode, clsType.User.IdNumber, strROWID);
                    }

                    if (result < 0)
                    {
                        MessageBox.Show(i + " 번줄 검사결과를 등록중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                //비만도 계산 및 Update
                if (FstrGjJong == "56")
                {
                    ///TODO : 이상훈(2019-12-04) 학생검진 비만도 계산 Method 확인 필요
                    //hm.Biman_Gesan_School(FnWRTNO, FstrSex); //학생검진 비만도 자동 계산 'A103
                    hm.Biman_Gesan(FnWRTNO, "HIC");    //체질량 자동계산 A117
                }
                else
                {
                    hm.Biman_Gesan(FnWRTNO, "HIC");    //체질량 자동계산 A117

                }
                hm.Update_Audiometry(FnWRTNO);                  //기도청력 시 기본청력 정상입력               

                //hm.MDRD_GFR_Gesan(FnWRTNO, FstrSex, FnAge, "HIC");     //MDRD-GFR 자동계산 2012년부터
                hm.AIR3_AUTO(FnWRTNO, "HIC");                          //AIR 3분법 자동계산
                hm.AIR6_AUTO(FnWRTNO, "HIC");                          //AIR 6분법 자동계산
                hm.LDL_Gesan(FnWRTNO);                          //LDL콜레스테롤 계산
                hm.TIBC_Gesan(FnWRTNO);                         //TIBC총철결합능 계산  

                //구강검사(ZD00) 누락자는 "." 찍기
                //if (hicResultService.GetItembyWrtNoExCode(FnWRTNO) > 0)
                //{
                //    result = hicResultService.UpdatebyWrtNoExCode(nSabun, FnWRTNO);

                //    if (result < 0)
                //    {
                //        MessageBox.Show("검사결과 등록중 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //        clsDB.setRollbackTran(clsDB.DbCon);
                //        return;
                //    }
                //}

                //접수마스타의 상태를 변경
                hm.Result_EntryEnd_Check(FnWRTNO);
                clsDB.setCommitTran(clsDB.DbCon);

                //메모사항 등록
                fn_Hic_Memo_Save();
                fn_Screen_Clear();
            }
            else if (sender == btnMemoSave) //메모저장
            {
                //메모사항 등록
                fn_Hic_Memo_Save();
                fn_Hic_Memo_Screen();
            }
            else if (sender == btnPacs)
            {
                FrmViewResult = new frmViewResult(FstrPtno);
                FrmViewResult.ShowDialog();
            }
        }

        void fn_Hic_Memo_Save()
        {
            long nPano = 0;
            string strCODE = "";
            string strMEMO = "";
            string strROWID = "";
            string strOK = "";
            string strTime = "";
            int result = 0;

            nPano = ssPatInfo.ActiveSheet.Cells[0, 0].Text.To<long>();
            if (nPano == 0) return;

            clsDB.setBeginTran(clsDB.DbCon);

            for (int i = 0; i < SS_ETC.ActiveSheet.RowCount; i++)
            {
                HIC_MEMO item2 = new HIC_MEMO();

                strOK = SS_ETC.ActiveSheet.Cells[i, 0].Text.Trim();
                strTime = SS_ETC.ActiveSheet.Cells[i, 1].Text.Trim();
                strMEMO = SS_ETC.ActiveSheet.Cells[i, 2].Text.Trim();
                strROWID = SS_ETC.ActiveSheet.Cells[i, 4].Text.Trim();
                if (!strROWID.IsNullOrEmpty())
                {
                    if (strOK == "True")
                    {
                        result = hicMemoService.UpdatebyRowId(strROWID);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("수검자 메모 저장 오류", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                else if (strTime.IsNullOrEmpty() && !strMEMO.IsNullOrEmpty())
                {
                    HIC_MEMO item = new HIC_MEMO();

                    item.PANO = nPano;
                    item.MEMO = strMEMO;
                    item.JOBSABUN = long.Parse(clsType.User.IdNumber);
                    item.PTNO = string.Format("{0:00000000}", FstrPtno);

                    result = hicMemoService.Insert(item);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("수검자 메모 저장 오류", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            clsDB.setCommitTran(clsDB.DbCon);
        }

        /// <summary>
        /// 경찰공무원 채용검진 청력결과 정상 여부(True=정상,False=결과값오류)
        /// </summary>
        /// <returns></returns>
        bool fn_Police_Result_Check()
        {
            bool rtnVal = false;
            string strCODE = "";
            string strResult = "";
            long nWRTNO = 0;
            bool bOK = false;
            string sCode = "";

            sCode = "2116";

            nWRTNO = long.Parse(txtWrtNo.Text);

            if (hicSunapdtlService.GetRowIdbyWrtNo(nWRTNO, sCode) == 0)
            {
                rtnVal = true;
                return rtnVal;
            }

            for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
            {
                strCODE = SS2.ActiveSheet.Cells[i, 0].Text.Trim();
                strResult = SS2.ActiveSheet.Cells[i, 2].Text.Trim();
                if (!strResult.IsNullOrEmpty())
                {
                    switch (strCODE)
                    {
                        case "A106":
                        case "A107":
                        case "TH12":
                        case "TH22":
                            bOK = true;
                            break;
                        default:
                            bOK = false;
                            break;
                    }
                    if (bOK == true)
                    {
                        if (strResult == "." || strResult == "정상" || strResult == "비정상")
                        {
                            rtnVal = false;
                            return rtnVal;
                        }
                    }
                }
            }

            return rtnVal;
        }

        void eChkClick(object sender, EventArgs e)
        {
            if (sender == chkPanRslt)
            {
                if (chkPanRslt.Checked == true)
                {
                    if (!txtWrtNo.Text.IsNullOrEmpty())
                    {
                        fn_Screen_Display();
                    }
                }
                else
                {
                    SS_Res.ActiveSheet.RowCount = 0;
                    SS_Res.Visible = false;
                    SSList.Visible = true;
                }
            }
        }

        void eMenuClick(object sender, EventArgs e)
        {
            if (sender == menuPrint)
            {
                long nWRTNO = 0;
                string strPANO = "";
                string strSname = "";
                string strAge = "";
                string strGDate = "";
                string strLtdName = "";
                string strYN = "";
                string strGjJong = "";
                string strFoot = "";
                string strJumin = "";

                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = false;

                //ssPatInfo_Sheet1.Columns.Get(11).Visible = false;
                //ssPatInfo_Sheet1.Columns.Get(12).Visible = false;

                Cursor.Current = Cursors.WaitCursor;
                strYN = "N";
                for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
                {
                    if (SSList.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        strYN = "Y";
                        break;
                    }
                }

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;


                Print_Display();
                
                if (strYN == "Y")
                {
                    for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
                    {
                        if (SSList.ActiveSheet.Cells[i, 0].Text == "True")
                        {
                            SSList.ActiveSheet.Cells[i, 0].Text = "";
                            FnWrtno2 = long.Parse(SSList.ActiveSheet.Cells[i, 1].Text);
                            fn_Print_Display();
                            strPANO = ssPatInfo.ActiveSheet.Cells[i, 0].Text.Trim();
                            strSname = ssPatInfo.ActiveSheet.Cells[i, 1].Text.Trim();
                            strAge = ssPatInfo.ActiveSheet.Cells[i, 2].Text.Trim();
                            strLtdName = ssPatInfo.ActiveSheet.Cells[i, 3].Text.Trim();
                            strGDate = ssPatInfo.ActiveSheet.Cells[i, 4].Text.Trim();
                            strGjJong = ssPatInfo.ActiveSheet.Cells[i, 5].Text.Trim();


                            strTitle = "접수번호(" + nWRTNO + ")" + strSname + " 검진결과" + "\r\n";
                            strHeader = sp.setSpdPrint_String(strTitle, new Font("맑은 고딕", 18, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true) + "\r\n";
                            strHeader += sp.setSpdPrint_String("검진번호 : " + strPANO + "  ▶성명:" + strSname + "(" + strAge + ")  ▶검진일자:" + strGDate, new Font("맑은 고딕", 11, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true) + "\r\n";                            
                            strHeader += sp.setSpdPrint_String("회 사 명: " + strLtdName + "   ▶종류:" + strGjJong, new Font("맑은 고딕", 11, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true) + "\r\n";
                            strHeader += sp.setSpdPrint_String("인쇄시각: " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime, new Font("맑은 고딕", 11, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, true, true) + "\r\n";
                            strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                            sp.setSpdPrint(SS3, PrePrint, setMargin, setOption, strHeader, strFooter);
                        }
                    }
                }
                else
                {
                    nWRTNO = long.Parse(txtWrtNo.Text);
                    if (nWRTNO == 0)
                    {
                        MessageBox.Show("인쇄할 데이터가 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    strPANO = ssPatInfo.ActiveSheet.Cells[0, 0].Text.Trim();
                    strSname = ssPatInfo.ActiveSheet.Cells[0, 1].Text.Trim();
                    strAge = ssPatInfo.ActiveSheet.Cells[0, 2].Text.Trim();
                    strLtdName =  ssPatInfo.ActiveSheet.Cells[0, 3].Text.Trim();
                    //strLtdName = hicLtdService.READ_Ltd_One_Name(ssPatInfo.ActiveSheet.Cells[0, 3].Text.Trim());
                    strGDate = ssPatInfo.ActiveSheet.Cells[0, 4].Text.Trim();
                    strGjJong = ssPatInfo.ActiveSheet.Cells[0, 5].Text.Trim();

                    //주민번호
                    HIC_PATIENT item = hicPatientService.GetItembyPaNo(Convert.ToInt32(strPANO));
                    strJumin = clsAES.DeAES(item.JUMIN2);
                    strJumin = VB.Left(strJumin, 6) +"-"+ VB.Mid(strJumin, 7, 1) + "******";

                    //strTitle = "접수번호(" + nWRTNO + ")" + strSname + " 검진결과" + "\r\n";
                    //strHeader = sp.setSpdPrint_String(strTitle, new Font("맑은 고딕", 18, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true) + "\r\n";
                    //strHeader += sp.setSpdPrint_String("검진번호 : " + strPANO + "  ▶성명:" + strSname + "(" + strAge + ")  ▶검진일자:" + strGDate, new Font("맑은 고딕", 11, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true) + "\r\n";
                    //strHeader += sp.setSpdPrint_String("회 사 명: " + strLtdName + "   ▶종류:" + strGjJong, new Font("맑은 고딕", 11, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true) + "\r\n";
                    //strHeader += sp.setSpdPrint_String("인쇄시각: " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime, new Font("맑은 고딕", 11, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, true, true) + "\r\n";
                    strTitle = "<추가검진 결과지>" + "\r\n";
                    strHeader = sp.setSpdPrint_String(strTitle, new Font("맑은 고딕", 18, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true) + "\r\n";
                    strHeader += sp.setSpdPrint_String("▶성명 : " + strSname + "(" + strAge + ")  ▶검진일자 : " + strGDate + " ▶주민번호 : "+ strJumin, new Font("맑은 고딕", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Left, false, true) + "\r\n";
                    strHeader += sp.setSpdPrint_String("▶회 사 명 : " + strLtdName , new Font("맑은 고딕", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Left, false, true) + "\r\n";
                    strFoot = "포항성모병원";
                    strFooter = sp.setSpdPrint_String(strFoot, new Font("맑은 고딕", 18, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

                    setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                    setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                    sp.setSpdPrint(SS3, PrePrint, setMargin, setOption, strHeader, strFooter);
                }

                //ssPatInfo_Sheet1.Columns.Get(11).Visible = true;
                //ssPatInfo_Sheet1.Columns.Get(12).Visible = true;

                Cursor.Current = Cursors.Default;
            }
            else if (sender == menuSabunChk)
            {
                SS2_Sheet1.Columns.Get(4).Visible = false;  //결과값코드
                SS2_Sheet1.Columns.Get(5).Visible = false;  //변경
                SS2_Sheet1.Columns.Get(6).Visible = false;  //ROWID
            }
            else if (sender == menuResultInputChk)
            {
                int nREAD = 0;
                int nRow = 0;
                long nWRTNO = 0;
                long nSabun = 0;
                string strOldSTS = "";
                string strNewSTS = "";
                string strResult = "";
                int result = 0;

                sp.Spread_All_Clear(SSList);
                SSList.ActiveSheet.RowCount = 50;

                clsDB.setBeginTran(clsDB.DbCon);

                //PFT 결과 정상으로 자동 등록
                List<HIC_JEPSU_RESULT> list = hicJepsuResultService.GetItemAll();

                nREAD = list.Count;
                SSList.ActiveSheet.RowCount = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    lblProcess.Text = "상태:" + (i + 1) + "/" + nREAD;
                    Application.DoEvents();

                    nWRTNO = list[i].WRTNO;
                    strResult = hm.PFT_Auto_Panjeng(nWRTNO);

                    if (strResult == "01")
                    {
                        result = hicResultService.UpdateResultbyWrtNoExCode(strResult, nWRTNO, 222, "TR11");

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("PFT 결과 자동등록중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        SSList.ActiveSheet.Cells[i, 0].Text = list[i].SNAME;
                        SSList.ActiveSheet.Cells[i, 1].Text = list[i].JEPDATE;
                        SSList.ActiveSheet.Cells[i, 2].Text = "PFT 결과 정상으로 자동 등록";
                        SSList.ActiveSheet.Cells[i, 3].Text = list[i].GJJONG;
                        SSList.ActiveSheet.Cells[i, 4].Text = nWRTNO.ToString();

                    }
                }
                
                //접수마스타의 상태를 변경
                List<HIC_JEPSU> list2 = hicJepsuService.GetItembyJepDatePanjengDate();

                nREAD = list2.Count;
                SSList.ActiveSheet.RowCount = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    lblProcess.Text = "상태:" + (i + 1) + "/" + nREAD;
                    Application.DoEvents();

                    nWRTNO = list2[i].WRTNO;
                    strOldSTS = list2[i].GBSTS;

                    //A135(채혈) 자동 완료 처리
                    if (string.Compare(list[i].JEPDATE, clsPublic.GstrSysDate) < 0)
                    {
                        hm.TIBC_Gesan(FnWRTNO);             //TIBC총철결합능 계산  
                        hm.Biman_Gesan(FnWRTNO, "HIC");     //체질량 자동계산 A117

                        result = hicResultService.UpdatebyWrtNoExCode(nWRTNO);

                        if (result < 0)
                        {
                            MessageBox.Show("접수마스터 변경시 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }
                    }

                    //접수마스타의 상태를 변경
                    strNewSTS = hm.Result_EntryEnd_Check(nWRTNO);
                    clsDB.setCommitTran(clsDB.DbCon);

                    if (strOldSTS != strNewSTS)
                    {
                        nRow += 1;
                        if (nRow > SSList.ActiveSheet.RowCount) SSList.ActiveSheet.RowCount = nRow;
                        SSList.ActiveSheet.Cells[i, 0].Text = list2[i].SNAME;
                        SSList.ActiveSheet.Cells[i, 1].Text = list2[i].JEPDATE;
                        SSList.ActiveSheet.Cells[i, 2].Text = strOldSTS + " => " + strNewSTS;
                        SSList.ActiveSheet.Cells[i, 3].Text = list2[i].GJJONG;
                        SSList.ActiveSheet.Cells[i, 4].Text = nWRTNO.ToString();
                    }
                }

                List<HIC_JEPSU_LTD_RES_BOHUM1> list3 = hicJepsuLtdResBohum1Service.GetItembyJepDate();

                nREAD = list3.Count;
                for (int i = 0; i < nREAD; i++)
                {
                    nWRTNO = list3[i].WRTNO;
                    nSabun = list3[i].SANGDAMDRNO;
                    hb.READ_HIC_Doctor(nSabun);

                    result = hicResBohum1Service.UpdateMunjinDrNobyWrtNo(clsHcVariable.GnHicLicense, nWRTNO);

                    if (result < 0)
                    {
                        MessageBox.Show("건강검진1차 문진 및 판정결과 변경시 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);

                if (nRow == 0)
                {
                    MessageBox.Show("결과입력완료 점검 시 오류가 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(nRow + " 건 입력완료 점검시 변동이 있었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (sender == menuXrayPanResult)
            {
                int nREAD = 0;
                int nCNT = 0;
                string strGTime = "";
                string strJepDate = "";
                string strXrayno = "";
                string strMsg = "";
                string strResult1 = "";
                string strResult2 = "";
                long nPano = 0;
                long nWRTNO = 0;
                int result = 0;

                strMsg = "1시간 이전에 방사선 판독이 완료된 흉부촬영 정상자를" + "\r\n";
                strMsg += "건진결과에 업데이트를 하시겠습니까?";
                if (MessageBox.Show(strMsg, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                ComFunc.ReadSysDate(clsDB.DbCon);

                //1시간전 시간을 계산함
                if (VB.Left(clsPublic.GstrSysTime, 2) == "00")
                {
                    strGTime = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-1).ToShortDateString() + "23" + VB.Right(clsPublic.GstrSysTime, 3);
                }
                else
                {
                    strGTime = clsPublic.GstrSysDate + " " + string.Format("0:00", int.Parse(VB.Left(clsPublic.GstrSysTime, 2)) - 1) + VB.Right(clsPublic.GstrSysTime, 3);
                }

                clsDB.setBeginTran(clsDB.DbCon);

                string[] strExCode = { "A141", "A142" };
                string[] strExCode1 = { "A154" };

                //판독한 명단을 검색
                List<HIC_XRAY_RESULT> list = hicXrayResultService.GetItembyReadTime1(strGTime);

                nREAD = list.Count;
                for (int i = 0; i < nREAD; i++)
                {
                    nPano = list[i].PANO;
                    strJepDate = list[i].JEPDATE.ToString();
                    strXrayno = list[i].XRAYNO;
                    strResult1 = list[i].RESULT1;
                    strResult2 = list[i].RESULT3;
                    if (!list[i].RESULT4.IsNullOrEmpty())
                    {
                        strResult2 += "/" + list[i].RESULT4;
                    }
                    if (strResult2.IsNullOrEmpty()) strResult2 = ".";

                    clsDB.setBeginTran(clsDB.DbCon);

                    //판정결과값을 건진 결과에 업데이트
                    result = hicResultService.UpdatebyWrtNoSabunPano(clsType.User.IdNumber, strJepDate, nPano, "01", strExCode);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show(nPano + "(" + strXrayno + ") 판정결과값 등록중 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    //촬영결과를 업데이트
                    result = hicResultService.UpdatebyWrtNoSabunPano(clsType.User.IdNumber, strJepDate, nPano, "정상", strExCode1);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show(nPano + "(" + strXrayno + ") 촬영결과를 등록중 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    //전송완료 SET
                    result = hicXrayResultService.UpdateGbResultSendbyRowId(list[i].ROWID);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show(nPano + "(" + strXrayno + ") 전송완료 등록중 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                }

                MessageBox.Show("작업 완료", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (sender == menuHoldReport)
            {
                if (txtWrtNo.Text.To<long>() > 0)
                {
                    frmHcPendListReg f = new frmHcPendListReg(txtWrtNo.Text.To<long>(), "1");
                    f.ShowDialog(this);
                }
            }
            else if (sender == menuPatStat)
            {
                clsHcVariable.GnWRTNO = FnWRTNO;
                FrmHcResultInputCheckList = new frmHcResultInputCheckList(FnWRTNO);
                FrmHcResultInputCheckList.ShowDialog(this);
            }
            else if (sender == menuPanScreen)
            {
                clsHcVariable.GnWRTNO = FnWRTNO;

                if (FstrGjChasu == "1" && string.Compare(FstrJepDate, "2019-01-01") >= 0)
                {   
                    //Frm1차일반특수판정_2019 f = new Frm1차일반특수판정_2019();
                    //f.ShowDialog(this);
                }
                else if (FstrGjChasu == "1" && string.Compare(FstrJepDate, "2018-12-31") <= 0)
                {
                    ///TODO : 이상훈(2019.12.05) Frm1차일반특수판정 컨버전 후 주석해제.
                    //Frm1차일반특수판정 f = new Frm1차일반특수판정();
                    //f.ShowDialog(this);menuPanScreen
                }
                else if (FstrGjChasu == "2" && FstrUCodes.IsNullOrEmpty() && (FstrGjJong == "17" || FstrGjJong == "18" || FstrGjJong == "45" || FstrGjJong == "46"))
                {
                    ///TODO : 이상훈(2019.12.05) Frm2차일반판정 컨버전 후 주석해제.
                    //Frm2차일반판정 f = new Frm2차일반판정();
                    //f.ShowDialog(this);
                }
                else
                {
                    ///TODO : 이상훈(2019.12.05) Frm특수및일특2차판정 컨버전 후 주석해제.
                    //Frm특수및일특2차판정 f = new Frm특수및일특2차판정();
                    //f.ShowDialog(this);
                }
            }
            else if (sender == menuXrayno_Set1)
            {
                int nREAD = 0;
                int nRow = 0;
                long nRate = 0;
                long nWRTNO = 0;
                long nSabun = 0;
                string strResult = "";
                string strOldSTS = "";
                string strNewSTS = "";
                string strPath = "";
                string strLastTime = "";
                string strSex = "";
                long nAge = 0;
                string strMsg = "";
                int result = 0;

                strMsg = "방사선직촬번호 자동 등록을 하시겠습니까?";
                if (MessageBox.Show(strMsg, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

                clsDB.setBeginTran(clsDB.DbCon);

                //방사선직촬번호 자동 등록
                List<HIC_JEPSU_RESULT> list = hicJepsuResultService.GetItembyExCode("A215");

                nREAD = list.Count;
                for (int i = 0; i < nREAD; i++)
                {
                    nWRTNO = list[i].WRTNO;
                    strResult = list[i].XRAYNO;
                    if (strResult.IsNullOrEmpty())
                    {
                        strResult = hicXrayResultService.GetXrayNobyPtNoJepDate(list[i].PTNO, list[i].JEPDATE);
                        if (!strResult.IsNullOrEmpty())
                        {
                            result = hicJepsuService.UpdateXrayNo(strResult, nWRTNO);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                MessageBox.Show("결과 저장중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }

                    if (!strResult.IsNullOrEmpty())
                    {
                        result = hicResultService.UpdateResultbyWrtNoExCode(strResult, nWRTNO, 111, "A215");
                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("결과 저장중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        //접수마스타의 상태를 변경
                        strNewSTS = hm.Result_EntryEnd_Check(nWRTNO);
                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);
                MessageBox.Show("작업 완료!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (sender == menuHeaResult)
            {
                int nREAD = 0;
                string strExCode  = "";
                string strNewCode = "";
                string strResult  = "";
                string strNewPan = "";
                string strResult_New = "";
                string strJepDate = "";
                int result = 0;

                ComFunc.ReadSysDate(clsDB.DbCon);

                strJepDate = hm.GET_HEA_JepsuDate(FnHeaWRTNO);

                if (strJepDate == clsPublic.GstrSysDate)
                {
                    MessageBox.Show("당일 종검결과는 가져올 수 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsDB.setBeginTran(clsDB.DbCon);

                //종검의 결과를 읽음
                List<HEA_RESULT> list = heaResultService.GetExCodeResult(FnHeaWRTNO);

                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        strExCode = list[i].EXCODE;
                        strResult = list[i].RESULT;

                        switch (strExCode)
                        {
                            case "C203":
                                if (strResult == ".")
                                {
                                    strNewCode = strExCode; //교정시력(좌)
                                }
                                else
                                {
                                    strNewCode = "A104"; //교정시력(좌)
                                }
                                break;
                            case "C204":
                                if (strResult == ".")
                                {
                                    strNewCode = strExCode; //교정시력(우)
                                }
                                else
                                {
                                    strNewCode = "A105"; //교정시력(우)
                                }
                                break;
                            case "A151":
                                strNewCode = "A153";    //심전도결과
                                break;
                            case "TH12":
                                strNewCode = "A106";    //청력(좌)
                                break;
                            case "TH22":
                                strNewCode = "A107";    //청력(우)
                                break;
                            case "A258":
                                strNewCode = "A131";    //HBs-Ag
                                break;
                            case "A259":
                                strNewCode = "A132";    //HBs-Ab
                                break;
                            case "A264":
                                strNewCode = "A257";    //알파피토단백
                                break;
                            case "A272":
                                strNewCode = "LU53";    //요침사
                                break;
                            case "LU48":
                                strNewCode = "A112";    //요단백
                                break;
                            case "LU44":
                                strNewCode = "LU44";    //요잠혈
                                break;
                            case "LU51":
                                strNewCode = "LU51";    //요산도 PH
                                break;
                            case "TZ46":
                                strNewCode = "TZ46";    //소변니코틴검사
                                break;
                            default:
                                break;
                        }

                        strResult = list[i].RESULT;
                        if (strExCode == "TH12" || strExCode == "TH22")
                        {
                            strResult_New = list[i].RESULT;
                        }

                        //청력일경우 숫자를 -> 정상 , 비정상으로
                        if ((strNewCode == "A106" || strNewCode == "A107") && strResult != "본인제외")
                        {
                            if (!strResult.IsNullOrEmpty())
                            {
                                if (long.Parse(strResult) <= 39)
                                {
                                    strResult = "정상";
                                }
                                else if (long.Parse(strResult) >= 40)
                                {
                                    strResult = "비정상";
                                }
                            }
                        }

                        //치아검사는 . 으로
                        if (strNewCode == "ZD00")
                        {
                            strResult = ".";
                        }

                        if (strNewCode == "A111" || strNewCode == "A112" || strNewCode == "A113")
                        {
                            switch (strResult)
                            {
                                case "음성":
                                    strResult = "01";
                                    break;
                                case "-":
                                    strResult = "01";
                                    break;
                                case "+-":
                                    strResult = "02";
                                    break;
                                case "양성":
                                    strResult = "03";
                                    break;
                                case "+":
                                    strResult = "03";
                                    break;
                                case "++":
                                    strResult = "04";
                                    break;
                                case "+++":
                                    strResult = "05";
                                    break;
                                case "++++":
                                    strResult = "06";
                                    break;
                                default:
                                    strResult = "";
                                    break;
                            }
                        }

                        strNewPan = hm.ExCode_Result_Panjeng(strNewCode, strResult, FstrSex, strJepDate, VB.Left(strJepDate, 4));

                        //일반건진 검사결과를 읽음
                        HIC_RESULT list2 = hicResultService.GetResultRowIdbyWrtNo(FnWRTNO, strNewCode, strExCode);

                        if (list2.IsNullOrEmpty())
                        {
                            if (strResult.Length > 200)
                            {
                                strResult = "종검결과 길이초과";

                                result = hicResultService.UpdateItembyRowId(strResult, strNewPan, list2.RID);
                            }
                        }

                        //검사결과 수치 추가 업데이트
                        if (strExCode == "TH12" || strExCode == "TH22")
                        {
                            HIC_RESULT list3 = hicResultService.GetResultRowIdbyWrtNoExCode(FnWRTNO, strExCode);
                        }
                    }
                }
            }
            else if ( sender == menuHistory)
            {
                frmHcPanPersonResult frm = new frmHcPanPersonResult("",FstrPtno, FstrSname);
                frm.ShowDialog();
            }

            else if (sender == menuPatInfoModify)
            {
                frmHcPatientModify frm = new frmHcPatientModify();
                frm.ShowDialog();
            }
            else if (sender == menuExit)
            {
                this.Close();
                return;
            }
            else if (sender == menuMemo)
            {
                frmHcMemo frm = new frmHcMemo(FstrPtno);
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog();
            }
        }

        bool PFT_Nomal_Auto_Set(long argWrtNo)
        {
            bool rtnVal = false;
            string ArgResult = "";
            int result = 0;

            ArgResult = hm.PFT_Auto_Panjeng(argWrtNo);

            if (ArgResult == "01")
            {
                result = hicResultService.UpdateResultbyWrtNoExCode(ArgResult, argWrtNo, 111, "TX11");

                if (result < 0)
                {
                    MessageBox.Show("PTF 결과 저장중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                rtnVal = true;
            }

            return rtnVal;
        }

        void eSpdChange(object sender, ChangeEventArgs e)
        {
            if (sender == SS2)
            {
                SS2.ActiveSheet.Cells[e.Row, 8].Text = "Y";
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS2)
            {
                if (e.Column != 1)
                {
                    return;
                }

                MessageBox.Show(SS2.ActiveSheet.Cells[e.Row, 2].Text, "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (sender == SSList)
            {
                long nWRTNO = 0;

                nWRTNO = SSList.ActiveSheet.Cells[e.Row, 0].Text.To<long>();

                fn_Screen_Clear();

                txtWrtNo.Text = nWRTNO.To<string>();
                fn_Screen_Display();
            }
        }

        void eSpdClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS2)
            {
                string strResCode = "";
                string strData = "";

                if (e.Column != 3)
                {
                    return;
                }

                strResCode = SS2.ActiveSheet.Cells[e.Row, 7].Text.Trim();

                if (strResCode.IsNullOrEmpty())
                {
                    sp.Spread_All_Clear(ssList1);
                    return;
                }

                FnClickRow = e.Row;

                sp.Spread_All_Clear(ssList1);
                //자료를 READ
                List<HIC_RESCODE> list = hicRescodeService.Read_Hic_ResCode_All(strResCode);

                ssList1.ActiveSheet.RowCount = list.Count;
                for (int i = 0; i < ssList1.ActiveSheet.RowCount; i++)
                {
                    ssList1.ActiveSheet.Cells[i, 0].Text = list[i].CODE;
                    ssList1.ActiveSheet.Cells[i, 1].Text = list[i].NAME;
                }
            }
            else if (sender == ssList1)
            {
                string strGubun = "";
                string strCode = "";

                if (e.RowHeader == true)
                {
                    return;
                }

                strCode = ssList1.ActiveSheet.Cells[e.Row, 0].Text;
                strGubun = SS2.ActiveSheet.Cells[FnClickRow, 7].Text;

                SS2.ActiveSheet.Cells[FnClickRow, 2].Text = strCode;
                SS2.ActiveSheet.Cells[FnClickRow, 4].Text = hb.READ_ResultName(strGubun, strCode);
                SS2.ActiveSheet.Cells[FnClickRow, 8].Text = "Y"; //변경여부
            }
        }

        void eSpdKeyDown(object sender, KeyEventArgs e)
        {
            string strResult  = "";
            string strResCode = "";
            string strResType = "";

            if (sender == SS2)
            {
                if (FnRowNo < 0 || FnRowNo > SS2.ActiveSheet.RowCount)
                {
                    return;
                }

                int nRow = this.SS2.ActiveSheet.ActiveRow.Index;
                int nCol = this.SS2.ActiveSheet.ActiveColumn.Index;

                strResult = "";
                switch (e.KeyCode)
                {
                    case Keys.F4:
                        strResult = "정상";
                        break;
                    case Keys.F5:
                        strResult = "과체중";
                        break;
                    case Keys.F6:
                        strResult = "비만";
                        break;
                    case Keys.F7:
                        strResult = "염증";
                        break;
                    case Keys.F8:
                        strResult = "비정상";
                        break;
                    case Keys.F9:
                        strResult = "미실시";
                        break;
                    case Keys.F11:
                        strResult = "이상소견없음";
                        break;
                    default:
                        break;
                }

                strResCode = SS2.ActiveSheet.Cells[(int)FnRowNo, 7].Text.Trim();
                strResType = SS2.ActiveSheet.Cells[(int)FnRowNo, 10].Text.Trim();

                if (!strResult.IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[(int)FnRowNo, 2].Text = strResult;
                    SS2.ActiveSheet.Cells[(int)FnRowNo, 8].Text = "Y";
                    //FnRowNo += 1;
                    //if (FnRowNo > SS2.ActiveSheet.RowCount) FnRowNo = SS2.ActiveSheet.RowCount;
                    //SS2.ActiveSheet.SetActiveCell((int)FnRowNo, 2);

                    FnRowNo += 1;
                    if (FnRowNo > SS2.ActiveSheet.RowCount)
                    {
                        FnRowNo = SS2.ActiveSheet.RowCount;
                    }
                    SS2.ActiveSheet.SetActiveCell((int)FnRowNo, 2);
                }
            }
        }

        void eSpreadKeyUp(object sender, KeyEventArgs e)
        {
        }


        void eSpdKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (sender == SSList)
                {
                    SendKeys.Send("{TAB}");
                }
                else if (sender == txtWrtNo)
                {
                    if (!txtWrtNo.Text.IsNullOrEmpty())
                    {
                        fn_Screen_Display();
                    }
                }
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (sender == txtWrtNo)
                {
                    fn_Screen_Display();
                    //SendKeys.Send("{TAB}");
                }
            }
        }

        //void eTxtLostFocus(object sender, EventArgs e)
        //{
        //    if (sender == txtWrtNo)
        //    {
        //        if (txtWrtNo.Text.Trim().IsNullOrEmpty()) return;
        //        fn_Screen_Display();
        //    }
        //}

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtLtdCode)
            {
                if (e.KeyChar == (char)13)
                {
                    if (txtLtdCode.Text.Length >= 2)
                    {
                        eBtnClick(btnLtdCode, new EventArgs());
                    }
                }
            }
        }

        void eTxtGotFocus(object sender, EventArgs e)
        {
            if (sender == txtSName)
            {
                txtSName.ImeMode = ImeMode.Hangul;
            }
        }

        void eSpdLeaveCell(object sender, LeaveCellEventArgs e)
        {
            string strGubun = "";
            string strCode = "";

            FnRowNo = e.NewRow;

            if (e.Column != 2)
            {
                return;
            }

            strCode = SS2.ActiveSheet.Cells[e.Row, 2].Text.Trim();
            if (strCode.IsNullOrEmpty()) return;
            if (strCode == "미실시") return;

            strGubun = SS2.ActiveSheet.Cells[e.Row, 7].Text.Trim();
            if (strGubun.IsNullOrEmpty()) return;

            if (strCode.IsNullOrEmpty())
            {
                SS2.ActiveSheet.Cells[e.Row, 4].Text = "";
            }
            else
            {
                SS2.ActiveSheet.Cells[e.Row, 4].Text = hb.READ_ResultName(strGubun, strCode);
                if (SS2.ActiveSheet.Cells[e.Row, 2].Text.IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[e.Row, 2].Text = "";
                    MessageBox.Show(strCode + "가 결과코드값에 등록이 안됨", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            strCode = SS2.ActiveSheet.Cells[e.Row, 0].Text.Trim();

            //2020.10.14 진단검사결과(검체결과) 전송된 데이터는 변경 불가능하게 막는다.
            //HIC_RESULT_EXCODE list = hicResultExCodeService.GetExam_ResultCSendYN();
        }



        void fn_Print_Display()
        {

        }

        void fn_Screen_Clear()
        {
            btnPacs.Enabled = false;
            //FnWRTNO = 0;
            txtWrtNo.Text = "";
            FnClickRow = 0;
            FnWrtno2 = 0;
            FnHeaWRTNO = 0;
            sp.Spread_All_Clear(ssList1);
            sp.Spread_All_Clear(ssPatInfo);
            menuHeaResult.Enabled = false;            
            txtXrayNo.Text = "";
            menuPanScreen.Enabled = false;
            menuHistory.Enabled = false;
            menuPatInfoModify.Enabled = false;
            menuPatStat.Enabled = false;
            SS_Res.ActiveSheet.RowCount = 0;
            SS_Res.Visible = false;
            SSList.Visible = true;
            sp.Spread_All_Clear(SS_ETC);
            sp.Spread_All_Clear(SS2);
            SS2.ActiveSheet.RowCount = 50;
        }

        void Print_Display()
        {
            int nREAD = 0;
            int nRow = 0;

            string strJepDate = "";
            string strSex = "";
            string strPart = "";
            string strExCode = "";
            string strResult = "";
            string strResCode = "";
            string strResultType = "";
            string strGbCodeUse = "";
            string strNomal = "";

            //Print_Injek_display //인적사항을 Display
            HIC_JEPSU listInfo = hicJepsuService.GetItembyWrtNo(FnWRTNO);

            if (listInfo == null)
            {
                MessageBox.Show("접수번호가 등록 안됨", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //strSex = listInfo.SEX;
            //strJepDate = listInfo.JEPDATE;

            //ssPatInfo.ActiveSheet.Cells[0, 0].Text = listInfo.PANO.ToString();
            //ssPatInfo.ActiveSheet.Cells[0, 1].Text = listInfo.SNAME;
            //ssPatInfo.ActiveSheet.Cells[0, 2].Text = listInfo.AGE + "/" + listInfo.SEX;
            //ssPatInfo.ActiveSheet.Cells[0, 3].Text = listInfo.LTDCODE.ToString();
            //ssPatInfo.ActiveSheet.Cells[0, 4].Text = strJepDate;
            //ssPatInfo.ActiveSheet.Cells[0, 5].Text = hb.READ_GjJong_Name(listInfo.GJJONG);

            //Print_Exam_Items_display //검사항목을 Display
            //strPart = VB.Left(cboPart.Text, 1);

            List<HIC_RESULT_EXCODE> list = hicResultExCodeService.GetItembyWrtNoPart(FnWRTNO);

            nREAD = list.Count;
            SS3.ActiveSheet.RowCount = nREAD;
            nRow = 0;
            for (int i = 0; i < nREAD; i++)
            {
                strExCode = list[i].EXCODE;
                strResult = list[i].RESULT;
                strResCode = list[i].RESCODE;
                strResultType = list[i].RESULTTYPE;
                strGbCodeUse = list[i].GBCODEUSE;

                nRow += 1;
                if (nRow > SS3.ActiveSheet.RowCount)
                {
                    SS3.ActiveSheet.RowCount = nRow;
                }

                SS3.ActiveSheet.Cells[i, 0].Text = list[i].HNAME;
                if (strGbCodeUse == "Y")
                {
                    if (!strResult.IsNullOrEmpty())
                    {
                        SS3.ActiveSheet.Cells[i, 1].Text = hb.READ_ResultName(strResCode, strResult);
                    }
                }
                else
                {
                    SS3.ActiveSheet.Cells[i, 1].Text = strResult;
                }

                //참고치를 Dispaly
                strNomal = hm.EXAM_NomalValue_SET(strExCode, FstrJepDate, FstrSex, list[i].MIN_M, list[i].MAX_M, list[i].MIN_F, list[i].MAX_F);

                if (strNomal.IsNullOrEmpty())
                {
                    SS3.ActiveSheet.Cells[i, 2].Text = " ";
                }
                else
                {
                    SS3.ActiveSheet.Cells[i, 2].Text = strNomal;
                }
                
            }
        }

        void fn_Screen_Display()
        {
            int nREAD = 0;
            int nRow = 0;
            int nROW2 = 0;

            string strJepDate = "";
            string strSex = "";
            string strPart = "";
            string strExCode = "";
            string strResult = "";
            string strResCode = "";
            string strResultType = "";
            string strGbCodeUse  = "";
            string strNomal = "";
            string strGbHelp = "";
            string strExName = "";

            string StrJumin = "";
            long nHeaPano = 0;
            string strXrayno = "";
            string strAllEndoRes = "";
            string strEndo = "";

            string StrRDate = "";
            string strEmrNo = "";
            string strXmlData = "";
            string strData = "";
            string strTemp = "";
            string strOK = "";
            string strStartJepDate = "";
            string strChkNew = "";

            FnWRTNO = txtWrtNo.Text.To<long>();

            //삭제된것 체크
            if (hb.READ_JepsuSTS(FnWRTNO) == "D")
            {
                MessageBox.Show(FnWRTNO + " 접수번호는 삭제된것 입니다. 확인하십시오", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SS2.ActiveSheet.RowCount = 50;

            //Screen_Injek_display //인적사항을 Display 
            HIC_JEPSU_PATIENT list = hicJepsuPatientService.GetItembyWrtNo(FnWRTNO);

            if (list.IsNullOrEmpty())
            {
                MessageBox.Show(FnWRTNO + " 접수번호가 등록 안됨", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            FstrPtno = list.PTNO;
            FstrSname = list.SNAME;
            if (!FstrPtno.IsNullOrEmpty())
            {
                btnPacs.Enabled = true;
            }
            strSex = list.SEX;
            FstrGjYear = list.GJYEAR;
            FstrSex = strSex;
            FnAge = list.AGE;
            StrJumin = clsAES.DeAES(list.JUMIN2);
            strJepDate = list.JEPDATE;
            strStartJepDate = VB.Left(strJepDate, 4) + "-01-01";
            FstrJepDate = strJepDate;
            FstrGjChasu = list.GJCHASU;
            FstrGjJong = list.GJJONG;
            FstrUCodes = list.UCODES;

            ssPatInfo.ActiveSheet.RowCount = 1;

            ssPatInfo.ActiveSheet.Cells[0, 0].Text = list.PANO.To<string>();
            ssPatInfo.ActiveSheet.Cells[0, 1].Text = list.SNAME;
            ssPatInfo.ActiveSheet.Cells[0, 2].Text = list.AGE + " / " + list.SEX;
            //ssPatInfo.ActiveSheet.Cells[0, 3].Text = list.LTDCODE.To<string>();
            ssPatInfo.ActiveSheet.Cells[0, 3].Text = hicLtdService.READ_Ltd_One_Name(list.LTDCODE.To<string>());
            ssPatInfo.ActiveSheet.Cells[0, 4].Text = list.JEPDATE.To<string>();
            ssPatInfo.ActiveSheet.Cells[0, 5].Text = hb.READ_GjJong_Name(list.GJJONG);

            //종합검진 여부 읽음
            menuHeaResult.Enabled = false;
            nHeaPano = 0;
            FnHeaWRTNO = 0;

            string strJumin2 = clsAES.AES(StrJumin);

            //종검 마스타에서 종검번호를 찾음
            nHeaPano = hicPatientService.GetPanobyJumin(strJumin2);

            if (nHeaPano > 0)
            {
                FnHeaWRTNO = heaJepsuService.GetWrtNobyHeaPaNoJepDate(nHeaPano, strStartJepDate, strJepDate);
            }

            if (FnHeaWRTNO > 0)
            {
                menuHeaResult.Enabled = true;
            }

            //Screen_Exam_Items_display //검사항목을 Display
            SS_Res.ActiveSheet.RowCount = 0;

            if (chkNew.Checked == true)
            {
                strChkNew = "1";
            }
            else
            {
                strChkNew = "";
            }

            List<HIC_RESULT_EXCODE> list2 = hicResultExCodeService.GetItembyWrtNoExCodeCheckNew(FnWRTNO, FstrPartExam, strChkNew);

            nREAD = list2.Count;
            SS2.ActiveSheet.RowCount = 0;
            SS2.ActiveSheet.RowCount = nREAD;
            for (int i = 0; i < nREAD; i++)
            {
                strExCode = list2[i].EXCODE;
                strResult =  list2[i].RESULT;
                strResCode = list2[i].RESCODE;
                strResultType = list2[i].RESULTTYPE;
                strGbCodeUse = list2[i].GBCODEUSE;
                strExName = list2[i].HNAME;

                //2019년도 변경사항
                if (strResCode == "113" && string.Compare(FstrJepDate, "2019-01-01") >= 0 && (string.Compare(FstrPtno, "81000000") > 0) && string.Compare(FstrPtno, "81000015") < 0)
                {
                    strResCode = "114";
                }
                //2019-04-10(부인과추가소견 보완)
                if (strResCode == "028" && string.Compare(FstrJepDate, "2019-01-01") >= 0)
                {
                    strResCode = "031";
                }

                //수가코드는 DISPLAY 에서 제외
                List<HIC_EXCODE> list3 = hicExcodeService.GetCodebyPart("9");

                for (int k = 0; k < list3.Count; k++)
                {
                    if (list3[k].CODE == strExCode)
                    {
                        SS2_Sheet1.Rows.Get(i).Visible = false;
                    }
                }

                SS2.ActiveSheet.Cells[i, 0].Text = list2[i].EXCODE;
                SS2.ActiveSheet.Cells[i, 1].Text = list2[i].HNAME;
                SS2.ActiveSheet.Cells[i, 2].Text = strResult;
                //A103(비만도)는 자동계산(입력금지)
                if (strGbCodeUse == "N" || strExCode == "A103")
                {
                    if (strExCode != "A151" && strExCode != "TH01" && strExCode != "TH02")
                    {
                        FarPoint.Win.Spread.CellType.TextCellType txt = new FarPoint.Win.Spread.CellType.TextCellType();

                        SS2.ActiveSheet.Cells[i, 3].CellType = txt;
                        SS2.ActiveSheet.Cells[i, 3].Text = "";
                        SS2.ActiveSheet.Cells[i, 3].Locked = true;
                    }
                    else
                    {
                        FarPoint.Win.Spread.CellType.ButtonCellType btn = new FarPoint.Win.Spread.CellType.ButtonCellType();

                        btn.ButtonColor2 = System.Drawing.SystemColors.ButtonFace;
                        btn.Text = "?";
                        SS2.ActiveSheet.Cells[i, 3].CellType = btn;
                        SS2.ActiveSheet.Cells[i, 3].Locked = false;
                    }
                }
                else
                {
                    FarPoint.Win.Spread.CellType.ButtonCellType btn = new FarPoint.Win.Spread.CellType.ButtonCellType();

                    btn.ButtonColor2 = System.Drawing.SystemColors.ButtonFace;
                    btn.Text = "?";
                    SS2.ActiveSheet.Cells[i, 3].CellType = btn;
                    SS2.ActiveSheet.Cells[i, 3].Locked = false;
                }

                //자동계산은 선택못함.
                switch (strExCode)
                {
                    case "A103":
                    case "TH91":
                    case "TH90":
                    case "A117":
                    case "A116":
                        SS2.ActiveSheet.Cells[i, 2].Locked = true;
                        break;
                    default:
                        break;
                }

                if (strGbCodeUse == "Y")
                {
                    if (!strResult.IsNullOrEmpty())
                    {
                        SS2.ActiveSheet.Cells[i, 4].Text = hb.READ_ResultName(strResCode, strResult);
                    }
                }
                SS2.ActiveSheet.Cells[i, 5].Text = list2[i].PANJENG;

                //참고치를 Dispaly                
                strNomal = hm.EXAM_NomalValue_SET(strExCode, FstrJepDate, FstrSex, list2[i].MIN_M, list2[i].MAX_M, list2[i].MIN_F, list2[i].MAX_F);
                SS2.ActiveSheet.Cells[i, 6].Text = strNomal;
                SS2.ActiveSheet.Cells[i, 7].Text = strResCode;
                if (list2[i].EXCODE == "A151")
                {
                    SS2.ActiveSheet.Cells[i, 7].Text = "007";
                }

                if (list2[i].EXCODE == "TH01" || list2[i].EXCODE == "TH02")
                {
                    SS2.ActiveSheet.Cells[i, 7].Text = "022";
                }
                SS2.ActiveSheet.Cells[i, 8].Text = "";
                SS2.ActiveSheet.Cells[i, 9].Text = list2[i].RID;
                SS2.ActiveSheet.Cells[i, 10].Text = list2[i].RESULTTYPE;
                SS2.ActiveSheet.Cells[i, 11].Text = list2[i].ENTSABUN;
                SS2.ActiveSheet.Cells[i, 12].Text = hb.READ_HIC_InsaName(list2[i].ENTSABUN).Trim();
                SS2.ActiveSheet.Cells[i, 13].Text = list2[i].GBAUTOSEND;

                //TX24는 결과가 없으면 자동으로 점을 찍음
                if (strExCode == "TX24" && strResult .IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[i, 2].Text = ".";
                    SS2.ActiveSheet.Cells[i, 8].Text = "Y";
                }

                if (list2[i].GBAUTOSEND == "Y")
                {
                    SS2.ActiveSheet.Cells[i, 2].Locked = true;
                }
                else
                {
                    SS2.ActiveSheet.Cells[i, 2].Locked = false;
                }
            }

            if (chkPanRslt.Checked == true)
            {
                //Screen_Exam_Res_display //검사판독결과를 Display
                SS_Res.ActiveSheet.RowCount = 0;
                nROW2 = 0;

                //-------------------------------------
                //  0.구강검사
                //-------------------------------------
                HIC_RES_DENTAL list3 = hicResDentalService.GetItemByWrtnoPanjenGDrNo(long.Parse(txtWrtNo.Text));

                if (!list3.IsNullOrEmpty())
                {
                    //종합판정
                    switch (list3.T_PANJENG1)
                    {
                        case "1":
                            strResult = "▶종합판정:정상A" + "\r\n";
                            break;
                        case "2":
                            strResult = "▶종합판정:정상B" + "\r\n";
                            break;
                        case "3":
                            strResult = "▶종합판정:주의" + "\r\n";
                            break;
                        default:
                            strResult = "▶종합판정:치료필요" + "\r\n";
                            break;
                    }
                    strData = list3.RES_JOCHI;
                    if (VB.Mid(strData, 1, 1) == "1" || VB.Mid(strData, 2, 1) == "1" || VB.Mid(strData, 3, 1) == "1")
                    {
                        strResult += "▶필요 보건교육:";
                        if (VB.Mid(strData, 1, 1) == "1") strResult += "설탕섭취(영양),";
                        if (VB.Mid(strData, 2, 1) == "1") strResult += "구강위생,";
                        if (VB.Mid(strData, 3, 1) == "1") strResult += "불소이용,";
                        strResult = VB.Left(strResult, strResult.Length - 1) + "\r\n"; //마지막컴마 제거
                    }

                    if (VB.Mid(strData, 4, 1) == "1" || VB.Mid(strData, 5, 1) == "1" || VB.Mid(strData, 6, 1) == "1" || VB.Mid(strData, 7, 1) == "1")
                    {
                        strResult += "▶사후관리 권고:";
                        if (VB.Mid(strData, 4, 1) == "1") strResult += "구강정밀검진,";
                        if (VB.Mid(strData, 5, 1) == "1") strResult += "전문가 구강위생관리,";
                        if (VB.Mid(strData, 6, 1) == "1") strResult += "치아우식치료필요,";
                        if (VB.Mid(strData, 7, 1) == "1") strResult += "치주관리필요,";
                        strResult = VB.Left(strResult, strResult.Length - 1) + "\r\n"; //마지막컴마 제거
                    }
                    strResult += "▶결과해석:" + list3.SANGDAM + "\r\n";
                    strResult += "▶판정일:" + list3.PANJENGDATE + VB.Space(2);
                    strResult += "판정의사:" + hb.READ_License_DrName(list3.PANJENGDRNO);

                    nROW2 += 1;
                    if (nROW2 > SS_Res.ActiveSheet.RowCount)
                    {
                        SS_Res.ActiveSheet.RowCount = nROW2;
                    }
                    SS_Res.ActiveSheet.Cells[nROW2 - 1, 0, nROW2 - 1, SS_Res.ActiveSheet.ColumnCount - 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFCF1EB"));
                    SS_Res.ActiveSheet.Cells[nROW2 - 1, 0].Text = "◈구강검사◈";
                    SS_Res.ActiveSheet.Cells[nROW2 - 1, 1].Text = "【ZD00】";

                    nROW2 += 1;
                    if (nROW2 > SS_Res.ActiveSheet.RowCount)
                    {
                        SS_Res.ActiveSheet.RowCount = nROW2;
                    }

                    //SS_Res_Sheet1.Cells[0, 0, nROW2-1, 1].ColumnSpan = 2;
                    SS_Res_Sheet1.AddSpanCell(nROW2-1, 0, 1, 2);
                    SS_Res.ActiveSheet.Cells[nROW2 - 1, 0].Text = strResult;
                    //SS_Res_Sheet1.SetRowHeight(nROW2 - 1, 20 + 20);

                    //Row 높이 설정 2021-04-29 
                    FarPoint.Win.Spread.Row row;
                    row = SS_Res.ActiveSheet.Rows[nROW2-1];
                    float rowSize = row.GetPreferredHeight();
                    row.Height = rowSize;

                }

                //-------------------------------------
                //  1.HIC_XRAY_RESULT
                //-------------------------------------
                List<HIC_XRAY_RESULT> list4 = hicXrayResultService.GetListItemByPtnoJepDate(FstrPtno, FstrJepDate);

                nREAD = list4.Count;
                //SS_Res.ActiveSheet.RowCount = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    nROW2 += 1;
                    if (nROW2 > SS_Res.ActiveSheet.RowCount)
                    {
                        SS_Res.ActiveSheet.RowCount = nROW2;
                    }

                    SS_Res.ActiveSheet.Cells[nROW2 - 1, 0, nROW2 - 1, SS_Res.ActiveSheet.ColumnCount - 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFCF1EB"));
                    SS_Res.ActiveSheet.Cells[nROW2 - 1, 0].Text = "◈흉부방사선검사(직접)◈";
                    SS_Res.ActiveSheet.Cells[nROW2 - 1, 1].Text = "【A142】";

                    if (list4[i].RESULT2.IsNullOrEmpty())
                    {
                        strResult = "판독분류명:" + list4[i].RESULT3 + "\r\n";
                        strResult += "판독소견:" + list4[i].RESULT4 + "\r\n";
                        strResult += "판독일시:" + list4[i].READDATE1 + "\r\n" + VB.Space(2);

                        BAS_BCODE item = basBcodeService.GetAllByGubunCode("XRAY_외주판독의사", list4[i].READDOCT1.ToString());
                        if (!item.IsNullOrEmpty())
                        {
                            strResult += "판독의사:" + item.NAME;
                        }
                        else
                        {
                            strResult += "판독의사:" + clsHelpDesk.READ_INSA_NAME(clsDB.DbCon, list4[i].READDOCT1.ToString());
                        }
                        
                    }
                    else
                    {
                        strResult = "판독분류:" + list4[i].RESULT2 + VB.Space(2);
                        strResult += "판독분류명:" + list4[i].RESULT3 + "\r\n";
                        strResult += "판독소견:" + list4[i].RESULT4 + "\r\n";
                        strResult += "판독일시:" + list4[i].READDATE1 + VB.Space(2);

                        BAS_BCODE item = basBcodeService.GetAllByGubunCode("XRAY_외주판독의사", list4[i].READDOCT1.ToString());
                        if (!item.IsNullOrEmpty())
                        {
                            strResult += "판독의사:" + item.NAME;
                        }
                        else
                        {
                            strResult += "판독의사:" + clsHelpDesk.READ_INSA_NAME(clsDB.DbCon, list4[i].READDOCT1.ToString());
                        }
                    }

                    nROW2 += 1;
                    if (nROW2 > SS_Res.ActiveSheet.RowCount)
                    {
                        SS_Res.ActiveSheet.RowCount = nROW2;
                    }

                    //SS_Res_Sheet1.Cells[0, 0, nROW2, 1].ColumnSpan = 2;
                    SS_Res_Sheet1.AddSpanCell(nROW2 - 1, 0, 1, 2);
                    SS_Res.ActiveSheet.Cells[nROW2 - 1, 0].Text = strResult;
                    SS_Res_Sheet1.SetRowHeight(nROW2 - 1, 20 + 20);

                    strResult = fn_BlankLine_Delete(strResult);
                    strResult = VB.TR(strResult, "\r\n", "##");
                    strResult = VB.TR(strResult, "\n", "\r\n");
                    strResult = VB.TR(strResult, "##", "\r\n");

                    if (!strResult.IsNullOrEmpty())
                    {
                        SS_Res.ActiveSheet.Cells[nROW2 - 1, 0].Text = strResult;
                        //SS_Res_Sheet1.SetRowHeight(nROW2 - 1, 20 + 20);
                        Size size = SS_Res.ActiveSheet.GetPreferredCellSize(nROW2 - 1, 0);
                        SS_Res.ActiveSheet.Rows[nROW2 - 1].Height = size.Height;
                    }
                    else
                    {
                        SS_Res.ActiveSheet.Cells[nROW2 - 1, 0].Text = strResult;
                        //SS_Res_Sheet1.SetRowHeight(nROW2 - 1, 20 + 40);
                        Size size = SS_Res.ActiveSheet.GetPreferredCellSize(nROW2 - 1, 0);
                        SS_Res.ActiveSheet.Rows[nROW2 - 1].Height = size.Height;
                    }
                }

                //-------------------------------------
                //  2.방사선 판독 결과지
                //-------------------------------------
                List<XRAY_RESULTNEW> list5 = xrayResultnewService.GetItembyPtNoJepDate(FstrPtno, FstrJepDate);

                nREAD = list5.Count;
                //SS_Res.ActiveSheet.RowCount = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    nROW2 += 1;
                    if (nROW2 > SS_Res.ActiveSheet.RowCount)
                    {
                        SS_Res.ActiveSheet.RowCount = nROW2;
                    }

                    SS_Res.ActiveSheet.Cells[nROW2 - 1, 0, nROW2-1, SS_Res.ActiveSheet.ColumnCount - 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFCF1EB"));
                    SS_Res.ActiveSheet.Cells[nROW2 - 1, 0].Text = "◈" + list5[i].XNAME + "◈";
                    SS_Res.ActiveSheet.Cells[nROW2 - 1, 1].Text = "【" + list5[i].XCODE + "】";

                    strResult = list5[i].RESULT;

                    nROW2 += 1;
                    if (nROW2 > SS_Res.ActiveSheet.RowCount)
                    {
                        SS_Res.ActiveSheet.RowCount = nROW2;
                    }

                    //SS_Res_Sheet1.Cells[0, 0, nROW2, 1].ColumnSpan = 2;
                    SS_Res_Sheet1.AddSpanCell(nROW2 - 1, 0, 1, 2);

                    strResult = fn_BlankLine_Delete(strResult);
                    strResult = VB.TR(strResult, "\r\n", "##");
                    strResult = VB.TR(strResult, "\n", "\r\n");
                    strResult = VB.TR(strResult, "##", "\r\n");

                    if (!strResult.IsNullOrEmpty())
                    {
                        SS_Res.ActiveSheet.Cells[nROW2 - 1, 0].Text = strResult;
                        //SS_Res_Sheet1.SetRowHeight(nROW2 - 1, 20 + 20);
                        Size size = SS_Res.ActiveSheet.GetPreferredCellSize(nROW2 - 1, 0);
                        SS_Res.ActiveSheet.Rows[nROW2 - 1].Height = size.Height;

                    }

                    else
                    {
                        SS_Res.ActiveSheet.Cells[nROW2 - 1, 0].Text = strResult;
                        //SS_Res_Sheet1.SetRowHeight(nROW2 - 1, 20 + 40);
                        Size size = SS_Res.ActiveSheet.GetPreferredCellSize(nROW2 - 1, 0);
                        SS_Res.ActiveSheet.Rows[nROW2 - 1].Height = size.Height;
                    }
                }

                //-------------------------------------
                //  3.내시경 판독 결과지
                //-------------------------------------
                strAllEndoRes = fn_GET_Endo_Result(FstrPtno, FstrJepDate);
                nREAD = VB.I(strAllEndoRes, "{{$}}");
                for (int i = 0; i < nREAD; i++)
                {
                    strEndo = VB.Pstr(strAllEndoRes, "{{$}}", i).Trim();
                    if (!strEndo.IsNullOrEmpty())
                    {
                        strExCode = VB.Pstr(strEndo, "{*}", 1);
                        strResult = VB.Pstr(strEndo, "{*}", 2);
                        nROW2 += 1;
                        if (nROW2 > SS_Res.ActiveSheet.RowCount)
                        {
                            SS_Res.ActiveSheet.RowCount = nROW2;
                        }

                        SS_Res.ActiveSheet.Cells[nROW2 - 1, 0, nROW2 - 1, SS_Res.ActiveSheet.ColumnCount - 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFCF1EB"));
                        switch (strExCode)
                        {
                            case "2":   //위내시경
                                SS_Res.ActiveSheet.Cells[i, 0].Text = "◈위내시경◈";
                                SS_Res.ActiveSheet.Cells[i, 1].Text = "【TX23】";
                                break;
                            case "3":   //대장내시경
                                SS_Res.ActiveSheet.Cells[i, 0].Text = "◈대장내시경◈";
                                SS_Res.ActiveSheet.Cells[i, 1].Text = "【TX64】";
                                break;
                            default:
                                SS_Res.ActiveSheet.Cells[i, 0].Text = "◈내시경◈";
                                SS_Res.ActiveSheet.Cells[i, 1].Text = "";
                                break;
                        }

                        nROW2 += 1;
                        if (nROW2 > SS_Res.ActiveSheet.RowCount) SS_Res.ActiveSheet.RowCount = nROW2;

                        //SS_Res_Sheet1.Cells[0, 0, nROW2, 1].ColumnSpan = 2;
                        SS_Res_Sheet1.AddSpanCell(nROW2 - 1, 0, 1, 2);

                        strResult = fn_BlankLine_Delete(strResult);
                        strResult = VB.TR(strResult, "\r\n", "##");
                        strResult = VB.TR(strResult, "\n", "\r\n");
                        strResult = VB.TR(strResult, "\r", "\r\n");
                        strResult = VB.TR(strResult, "##", "\r\n");

                        if (!strResult.IsNullOrEmpty())
                        {
                            SS_Res.ActiveSheet.Cells[nROW2 - 1, 0].Text = strResult;
                            //SS_Res_Sheet1.SetRowHeight(nROW2 - 1, 20 + 20);
                            Size size = SS_Res.ActiveSheet.GetPreferredCellSize(nROW2 - 1, 0);
                            SS_Res.ActiveSheet.Rows[nROW2 - 1].Height = size.Height;
                        }
                        else
                        {
                            SS_Res.ActiveSheet.Cells[nROW2 - 1, 0].Text = strResult;
                            //SS_Res_Sheet1.SetRowHeight(nROW2 - 1, 20 + 40);
                            Size size = SS_Res.ActiveSheet.GetPreferredCellSize(nROW2 - 1, 0);
                            SS_Res.ActiveSheet.Rows[nROW2 - 1].Height = size.Height;
                        }
                    }
                }
                //-------------------------------------
                //  4.스트레스검사 결과 전송
                //-------------------------------------
                List<ETC_JUPMST> list6 = etcJupmstService.GetStress_SogenbyPtNo(FstrPtno, FstrJepDate);

                nREAD = list6.Count;
                for (int i = 0; i < nREAD; i++)
                {
                    strResult = list6[i].STRESS_SOGEN;

                    //결과가 있으면 전송을 안함
                    if (!strResult.IsNullOrEmpty())
                    {
                        strResult = strResult.Replace("1. ", "\r\n" + "1. ");
                        strResult = strResult.Replace("2. ", "\r\n" + "2. ");
                        strResult = strResult.Replace("3. ", "\r\n" + "3. ");
                        strResult = strResult.Replace(" 1) ", "\r\n" + "1) ");
                        strResult = strResult.Replace(" 2) ", "\r\n" + "2) ");
                        strResult = strResult.Replace(" 3) ", "\r\n" + "3) ");
                        strResult = strResult.Replace(" 4) ", "\r\n" + "4) ");
                        strResult = strResult.Replace(" 5) ", "\r\n" + "5) ");
                        strResult = strResult.Replace(" 6) ", "\r\n" + "6) ");
                        strResult = strResult.Replace(" 7) ", "\r\n" + "7) ");
                        strResult = strResult.Replace(" 8) ", "\r\n" + "8) ");
                        strResult = strResult.Replace(" 9) ", "\r\n" + "9) ");
                    }
                    else
                    {
                        strResult = "미검사";
                    }

                    nROW2 += 1;
                    if (nROW2 > SS_Res.ActiveSheet.RowCount) SS_Res.ActiveSheet.RowCount = nROW2;

                    SS_Res.ActiveSheet.Cells[nROW2 - 1, 0, nROW2 - 1, SS_Res.ActiveSheet.ColumnCount - 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFCF1EB"));
                    SS_Res.ActiveSheet.Cells[nROW2 - 1, 0].Text = "◈스트레스검사◈";
                    SS_Res.ActiveSheet.Cells[nROW2 - 1, 1].Text = "【TX87】";

                    nROW2 += 1;
                    if (nROW2 > SS_Res.ActiveSheet.RowCount)
                    {
                        SS_Res.ActiveSheet.RowCount = nROW2;
                    }

                    //SS_Res_Sheet1.Cells[0, 0, nROW2, 1].ColumnSpan = 2;
                    SS_Res_Sheet1.AddSpanCell(nROW2 - 1, 0, 1, 2);

                    strResult = fn_BlankLine_Delete(strResult);
                    strResult = VB.TR(strResult, "\r\n", "##");
                    strResult = VB.TR(strResult, "\n", "\r\n");
                    strResult = VB.TR(strResult, "\r", "\r\n");
                    strResult = VB.TR(strResult, "##", "\r\n");

                    if (!strResult.IsNullOrEmpty())
                    {
                        SS_Res.ActiveSheet.Cells[nROW2 - 1, 0].Text = strResult;
                        //SS_Res_Sheet1.SetRowHeight(nROW2 - 1, 20 + 20);
                        Size size = SS_Res.ActiveSheet.GetPreferredCellSize(nROW2 - 1, 0);
                        SS_Res.ActiveSheet.Rows[nROW2 - 1].Height = size.Height;
                    }
                    else
                    {
                        SS_Res.ActiveSheet.Cells[nROW2 - 1, 0].Text = strResult;
                        //SS_Res_Sheet1.SetRowHeight(nROW2 - 1, 20 + 40);
                        Size size = SS_Res.ActiveSheet.GetPreferredCellSize(nROW2 - 1, 0);
                        SS_Res.ActiveSheet.Rows[nROW2 - 1].Height = size.Height;
                    }
                }

                //-------------------------------------
                //  5.TCD(TZ16) 뇌혈류초음파 결과전송
                //-------------------------------------
                long nFormNo = 2085;
                string sJepDate = "";

                sJepDate = FstrJepDate.Replace("-", "");

                List <ETC_JUPMST> list7 = etcJupmstService.GetItembyPtNoJepDate(FstrPtno, FstrJepDate);

                nREAD = list7.Count;
                for (int i = 0; i < nREAD; i++)
                {
                    strEmrNo =comHpcLibBService.GetEmrNobyPtNoFormNo(FstrPtno, nFormNo, sJepDate).ToString();

                    strXmlData = "";
                    strResult = "";
                    if (!strEmrNo.IsNullOrEmpty())
                    {
                        ///TODO 이상훈(2019.12.05) NEW EMR XMLDATA Read 부분 추가할 것
                        strXmlData = GetXMLData(strEmrNo);
                        if (!strXmlData.IsNullOrEmpty())
                        {
                            strResult = TCD_Result_Edit(strXmlData);
                        }
                    }

                    if (strResult.IsNullOrEmpty())
                    {
                        strResult = "미실시";
                    }

                    nROW2 += 1;
                    if (nROW2 > SS_Res.ActiveSheet.RowCount)
                    {
                        SS_Res.ActiveSheet.RowCount = nROW2;
                    }

                    SS_Res.ActiveSheet.Cells[nROW2 - 1, 0, nROW2 - 1, SS_Res.ActiveSheet.ColumnCount - 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFCF1EB"));
                    SS_Res.ActiveSheet.Cells[nROW2 - 1, 0].Text = "◈뇌혈류초음파 결과전송◈";
                    SS_Res.ActiveSheet.Cells[nROW2 - 1, 1].Text = "【TZ16】";

                    nROW2 += 1;
                    if (nROW2 > SS_Res.ActiveSheet.RowCount)
                    {
                        SS_Res.ActiveSheet.RowCount = nROW2;
                    }

                    //SS_Res_Sheet1.Cells[0, 0, nROW2, 1].ColumnSpan = 2;
                    SS_Res_Sheet1.AddSpanCell(nROW2 - 1, 0, 1, 2);

                    strResult = fn_BlankLine_Delete(strResult);
                    strResult = VB.TR(strResult, "\r\n", "##");
                    strResult = VB.TR(strResult, "\n", "\r\n");
                    strResult = VB.TR(strResult, "\r", "\r\n");
                    strResult = VB.TR(strResult, "##", "\r\n");

                    if (!strResult.IsNullOrEmpty())
                    {
                        SS_Res.ActiveSheet.Cells[nROW2 - 1, 0].Text = strResult;
                        //SS_Res_Sheet1.SetRowHeight(nROW2 - 1, 20 + 20);
                        Size size = SS_Res.ActiveSheet.GetPreferredCellSize(nROW2 - 1, 0);
                        SS_Res.ActiveSheet.Rows[nROW2 - 1].Height = size.Height;
                    }
                    else
                    {
                        SS_Res.ActiveSheet.Cells[nROW2 - 1, 0].Text = strResult;
                        //SS_Res_Sheet1.SetRowHeight(nROW2 - 1, 20 + 40);
                        Size size = SS_Res.ActiveSheet.GetPreferredCellSize(nROW2 - 1, 0);
                        SS_Res.ActiveSheet.Rows[nROW2 - 1].Height = size.Height;
                    }
                }

                //-------------------------------------
                //  6.GY-SONO(TX98) 산부인과 초음파
                //-------------------------------------
                List<XRAY_RESULTNEW_DR> list8 = xrayResultnewDrService.GetItembyPaNoReadDate(FstrPtno, FstrJepDate);

                nREAD = list8.Count;
                for (int i = 0; i < nREAD; i++)
                {
                    strResult = list8[i].RESULT + list8[i].RESULT1;

                    if (strResult.IsNullOrEmpty())
                    {
                        strResult = "미실시";
                    }

                    nROW2 += 1;
                    if (nROW2 > SS_Res.ActiveSheet.RowCount)
                    {
                        SS_Res.ActiveSheet.RowCount = nROW2;
                    }

                    SS_Res.ActiveSheet.Cells[nROW2 - 1, 0, nROW2 - 1, SS_Res.ActiveSheet.ColumnCount - 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFCF1EB"));
                    SS_Res.ActiveSheet.Cells[nROW2 - 1, 0].Text = "◈산부인과 초음파◈";
                    SS_Res.ActiveSheet.Cells[nROW2 - 1, 1].Text = "【TX98】";

                    nROW2 += 1;
                    if (nROW2 > SS_Res.ActiveSheet.RowCount)
                    {
                        SS_Res.ActiveSheet.RowCount = nROW2;
                    }

                    //SS_Res_Sheet1.Cells[0, 0, nROW2, 1].ColumnSpan = 2;
                    SS_Res_Sheet1.AddSpanCell(nROW2 - 1, 0, 1, 2);

                    strResult = fn_BlankLine_Delete(strResult);
                    strResult = VB.TR(strResult, "\r\n", "##");
                    strResult = VB.TR(strResult, "\n", "\r\n");
                    strResult = VB.TR(strResult, "\r", "\r\n");
                    strResult = VB.TR(strResult, "##", "\r\n");

                    if (!strResult.IsNullOrEmpty())
                    {
                        SS_Res.ActiveSheet.Cells[nROW2 - 1, 0].Text = strResult;
                        //SS_Res_Sheet1.SetRowHeight(nROW2 - 1, 20 + 20);
                        Size size = SS_Res.ActiveSheet.GetPreferredCellSize(nROW2 - 1, 0);
                        SS_Res.ActiveSheet.Rows[nROW2 - 1].Height = size.Height;
                    }
                    else
                    {
                        SS_Res.ActiveSheet.Cells[nROW2 - 1, 0].Text = strResult;
                        //SS_Res_Sheet1.SetRowHeight(nROW2 - 1, 20 + 40);
                        Size size = SS_Res.ActiveSheet.GetPreferredCellSize(nROW2 - 1, 0);
                        SS_Res.ActiveSheet.Rows[nROW2 - 1].Height = size.Height;
                    }
                }

                //-------------------------------------
                //  7.위,대장 조직검사 결과 가져오기
                //-------------------------------------
                List<EXAM_ANATMST> list9 = examAnatmstService.GetItembyPtNoJepDate(FstrPtno, FstrJepDate);

                nREAD = list9.Count;
                for (int i = 0; i < nREAD; i++)
                {
                    strExCode = list9[i].ORDERCODE;
                    if (strExCode == "PAP`S")
                    {
                        strResult = list9[i].REMARK5;
                    }
                    else
                    {
                        strResult = list9[i].RESULT1 + list9[i].RESULT2;
                    }
                    if (!strResult.IsNullOrEmpty())
                    {
                        nROW2 += 1;
                        if (nROW2 > SS_Res.ActiveSheet.RowCount)
                        {
                            SS_Res.ActiveSheet.RowCount = nROW2;
                        }

                        SS_Res.ActiveSheet.Cells[nROW2 - 1, 0, nROW2 - 1, SS_Res.ActiveSheet.ColumnCount - 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFCF1EB"));
                        strExCode = list9[i].ORDERCODE;
                        switch (strExCode)
                        {
                            case "PAP`S":
                            case "R2001":
                            case "R2008":
                            case "R2009":
                                SS_Res.ActiveSheet.Cells[nROW2 - 1, 0].Text = "◈위조직검사◈";
                                SS_Res.ActiveSheet.Cells[nROW2 - 1, 1].Text = "【TX20】";
                                break;
                            default:
                                SS_Res.ActiveSheet.Cells[nROW2 - 1, 0].Text = "◈대장조직검사◈";
                                SS_Res.ActiveSheet.Cells[nROW2 - 1, 1].Text = "【TX64】";
                                break;
                        }

                        nROW2 += 1;
                        if (nROW2 > SS_Res.ActiveSheet.RowCount)
                        {
                            SS_Res.ActiveSheet.RowCount = nROW2;
                        }

                        //SS_Res_Sheet1.Cells[0, 0, nROW2, 1].ColumnSpan = 2;
                        SS_Res_Sheet1.AddSpanCell(nROW2 - 1, 0, 1, 2);

                        strResult = fn_BlankLine_Delete(strResult);
                        strResult = VB.TR(strResult, "\r\n", "##");
                        strResult = VB.TR(strResult, "\n", "\r\n");
                        strResult = VB.TR(strResult, "\r", "\r\n");
                        strResult = VB.TR(strResult, "##", "\r\n");

                        if (!strResult.IsNullOrEmpty())
                        {
                            SS_Res.ActiveSheet.Cells[nROW2 - 1, 0].Text = strResult;
                            //SS_Res_Sheet1.SetRowHeight(nROW2 - 1, 20 + 20);
                            Size size = SS_Res.ActiveSheet.GetPreferredCellSize(nROW2 - 1, 0);
                            SS_Res.ActiveSheet.Rows[nROW2 - 1].Height = size.Height;
                        }
                        else
                        {
                            SS_Res.ActiveSheet.Cells[nROW2 - 1, 0].Text = strResult;
                            //SS_Res_Sheet1.SetRowHeight(nROW2 - 1, 20 + 40);
                            Size size = SS_Res.ActiveSheet.GetPreferredCellSize(nROW2 - 1, 0);
                            SS_Res.ActiveSheet.Rows[nROW2 - 1].Height = size.Height;
                        }
                    }
                }

                //-------------------------------------
                //  8.Cytology 결과 가져오기
                //-------------------------------------
                FrmViewResult = new frmViewResult();

                List<COMHPC> list10 = comHpcLibBService.GetCytologybyPtNoJepDate(FstrPtno, FstrJepDate);

                nREAD = list10.Count;
                for (int i = 0; i < nREAD; i++)
                {
                    strExCode = list10[i].ORDERCODE;
                    strResult = FrmViewResult.GetCytologyResult(list10[i].ROWID);

                    if (!strResult.IsNullOrEmpty())
                    {
                        nROW2 += 1;
                        if (nROW2 > SS_Res.ActiveSheet.RowCount)
                        {
                            SS_Res.ActiveSheet.RowCount = nROW2;
                        }

                        SS_Res.ActiveSheet.Cells[nROW2 - 1, 0, nROW2 - 1, SS_Res.ActiveSheet.ColumnCount - 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFCF1EB"));
                        SS_Res.ActiveSheet.Cells[nROW2 - 1, 0].Text = "◈" + list10[i].ORDERNAME + "◈";
                        SS_Res.ActiveSheet.Cells[nROW2 - 1, 1].Text = "【" + list10[i].ORDERCODE + "】";

                        nROW2 += 1;
                        if (nROW2 > SS_Res.ActiveSheet.RowCount)
                        {
                            SS_Res.ActiveSheet.RowCount = nROW2;
                        }

                        //SS_Res_Sheet1.Cells[0, 0, nROW2, 1].ColumnSpan = 2;
                        SS_Res_Sheet1.AddSpanCell(nROW2 - 1, 0, 1, 2);

                        strResult = fn_BlankLine_Delete(strResult);
                        strResult = VB.TR(strResult, "\r\n", "##");
                        strResult = VB.TR(strResult, "\n", "\r\n");
                        strResult = VB.TR(strResult, "\r", "\r\n");
                        strResult = VB.TR(strResult, "##", "\r\n"); 

                        if (!strResult.IsNullOrEmpty())
                        {
                            SS_Res.ActiveSheet.Cells[nROW2 - 1, 0].Text = strResult;
                            //SS_Res_Sheet1.SetRowHeight(nROW2 - 1, 20 + 20);
                            Size size = SS_Res.ActiveSheet.GetPreferredCellSize(nROW2 - 1, 0);
                            SS_Res.ActiveSheet.Rows[nROW2 - 1].Height = size.Height;
                        }
                        else
                        {
                            SS_Res.ActiveSheet.Cells[nROW2 - 1, 0].Text = strResult;
                            //SS_Res_Sheet1.SetRowHeight(nROW2 - 1, 20 + 40);
                            Size size = SS_Res.ActiveSheet.GetPreferredCellSize(nROW2 - 1, 0);
                            SS_Res.ActiveSheet.Rows[nROW2 - 1].Height = size.Height;
                        }
                    }
                }

                SSList.Visible = false;
                SS_Res.Visible = true;
            }
            else
            {
                SS_Res.Visible = false;
                SSList.Visible = true;
            }

            fn_Hic_Memo_Screen();

            menuPanScreen.Enabled = true;
            menuHistory.Enabled = true;
            menuPatStat.Enabled = true;
            menuPatInfoModify.Enabled = true;
            if (FstrGjJong == "31" || FstrGjJong == "35")
            {
                menuPanScreen.Enabled = false;
            }
        }

        /// <summary>
        /// TODO : 이상훈(2019.12.05) NEW EMR XMLDATA Read 부분 추가할 것
        /// </summary>
        /// <param name="strEmrNo"></param>
        /// <returns></returns>
        string GetXMLData(string strEmrNo)
        {
            string rtnVal = "";

            return rtnVal;
        }

        /// <summary>
        /// TODO : 이상훈(2019.12.05) NEW EMR XMLDATA Read 부분 추가할 것
        /// </summary>
        /// <param name="ArgTemp"></param>
        /// <returns></returns>
        string TCD_Result_Edit(string ArgTemp)
        {
            string rtnVal = "";

            ////Dim strCUT1$, strCUT2$, strResult$

            ////strCUT1 = "label=" & Chr$(34) & "판독결과지" & Chr$(34) & "><![CDATA["
            ////strCUT2 = "]]></ta1><im1"
            ////'Finding 및 Result가 없으면 원본을 그대로 리턴함
            ////If InStr(ArgTemp, strCUT1) = 0 Then
            ////    TCD_Result_Edit = ""
            ////    Exit Function
            ////End If


            ////'결과의 라인피드,공란을 정리함
            ////strResult = STRCUT(ArgTemp, strCUT1, strCUT2)
            ////strResult = BlankLine_Delete(strResult)


            ////TCD_Result_Edit = strResult

            return rtnVal;
        }

        string fn_GET_Endo_Result(string argPTNO, string ArgJepDate)
        {
            string rtnVal = "";
            int nREAD = 0;
            int nRead1 = 0;
            string strDate = "";
            string strPANO = "";
            string strResult = "";
            string strGbJob = "";
            string strResult1 = "";
            string strResult2 = "";
            string strResult3 = "";
            string strResult4 = "";
            string strResult5 = "";
            string strResult6 = "";
            string strResult6_2 = "";
            string strResult6_3 = "";
            string strRemark = "";
            string strNew = "";
            string strInfo = "";
            string strRESULTDATE = "";
            string strResultDrCode = "";
            string strKorName = "";
            string strEndoAllResult = "";

            List<ENDO_JUPMST_RESULT> list = endoJupmstResultService.GetItembyPtNoJDate(argPTNO, ArgJepDate);

            nREAD = list.Count;
            strEndoAllResult = "";
            for (int i = 0; i < nREAD; i++)
            {
                strPANO = list[i].PTNO;
                strDate = list[i].JDATE;
                strGbJob = list[i].GBJOB;
                strNew = list[i].GBNEW;

                if (list[i].SEQNO == 0)
                {
                    strEndoAllResult += strGbJob + "{*}미실시{{$}}";
                }
                else
                {
                    //결과입력 의사명
                    strResultDrCode = list[i].RESULTDRCODE;
                    if (!strResultDrCode.IsNullOrEmpty())
                    {
                        strKorName = hm.READ_HIC_Doctor_Name(strResultDrCode);
                    }
                    else
                    {
                        strResultDrCode = "";
                        strKorName = "";
                    }

                    strResult1 = "";
                    strResult2 = "";
                    strResult3 = "";
                    strResult4 = "";
                    strResult5 = "";
                    strResult6 = "";
                    strResult6_2 = "";
                    strResult6_3 = "";
                    strRemark = "";

                    strResult1 = list[i].REMARK1;
                    strResult2 = list[i].REMARK2;
                    strResult3 = list[i].REMARK3;
                    strResult4 = list[i].REMARK4;
                    strResult5 = list[i].REMARK5;
                    strResult6 = list[i].REMARK6; //Biposy

                    if (strNew == "Y")
                    {
                        strResult6_2 = list[i].REMARK6_2;
                        strResult6_3 = list[i].REMARK6_3;
                        strRemark = list[i].REMARK;
                    }

                    //Premedication--------------------------------------------------------------------------
                    strInfo = "▶Premedication:" + "\r\n";
                    strInfo += list[i].GBPRE_1 == "Y" ? "None" : "";
                    strInfo += list[i].GBPRE_2 == "Y" ? "Aigiron " : "";

                    if (!list[i].GBPRE_21.IsNullOrEmpty())
                    {
                        strInfo += list[i].GBPRE_21 + "mg " + list[i].GBPRE_22 + ", ";
                    }

                    if (list[i].GBPRE_3 == "Y")
                    {
                        strInfo += " " + list[i].GBPRE_31;
                    }
                    else
                    {
                        strInfo += list[i].GBPRE_31;
                    }

                    //Conscious Sedation---------------------------------------------------------------------
                    strInfo += "\r\n" + "▶Conscious Sedation:" + "\r\n";
                    strInfo += list[i].GBCON_1 == "Y" ? "None" : "";
                    strInfo += list[i].GBCON_2 == "Y" ? "Mediazolam " : "";

                    if (!list[i].GBCON_21.IsNullOrEmpty())
                    {
                        strInfo += list[i].GBCON_21 + "mg " + list[i].GBCON_22 + ", ";
                    }
                    strInfo += list[i].GBCON_3 == "Y" ? "Propofol " : "";

                    if (!list[i].GBCON_31.IsNullOrEmpty())
                    {
                        strInfo += list[i].GBCON_31 + "mg " + list[i].GBCON_32 + ", ";
                    }

                    strInfo += list[i].GBCON_4 == "Y" ? "Pathidine " : "";

                    if (!list[i].GBCON_41.IsNullOrEmpty())
                    {
                        strInfo += list[i].GBCON_41 + "mg " + list[i].GBCON_42 + ", ";
                    }

                    switch (strGbJob)
                    {
                        case "1":   //기관지
                            strResult = "▶Vocal Cord:" + "\r\n" + strResult1 + "\r\n";
                            strResult += "▶Carina:" + "\r\n" + strResult2 + "\r\n";
                            strResult += "▶Bronchi:" + "\r\n" + strResult3 + "\r\n";
                            strResult += "▶EndoScopic Procedure:" + "\r\n" + strResult4;
                            strResult += "▶EndoScopic Biopsy:" + "\r\n" + strResult6;
                            if (strNew == "Y")
                            {
                                strResult += "\r\n" + strResult6_2;
                                strResult += "\r\n" + strResult6_3 + "\r\n";
                                strResult += "처치의사 : " + strResultDrCode + "  " + strKorName;
                            }
                            break;
                        case "2":   //위
                            if (strNew == "Y")
                            {
                                if (!list[i].REMARK6.IsNullOrEmpty())
                                {
                                    strResult6 = "Esophagus:" + list[i].REMARK6;
                                }
                                if (!list[i].REMARK6_2.IsNullOrEmpty())
                                {
                                    strResult6 += "\r\n" + "Stomach:" + list[i].REMARK6_2;
                                }
                                if (!list[i].REMARK6_3.IsNullOrEmpty())
                                {
                                    strResult6 += "\r\n" + "Duodenum:" + list[i].REMARK6_3;
                                }
                            }

                            if (!strResult1.IsNullOrEmpty())
                            {
                                strResult = "▶Esophagus:" + "\r\n" + strResult1 + "\r\n";
                            }
                            else
                            {
                                strResult = "▶Esophagus:" + "\r\n" + strResult1;
                            }
                            if (!strResult2.IsNullOrEmpty())
                            {
                                strResult += "▶Stomach:" + "\r\n" + strResult2 + "\r\n";
                            }
                            else
                            {
                                strResult += "▶Stomach:" + "\r\n" + strResult2;
                            }
                            if (!strResult3.IsNullOrEmpty())
                            {
                                strResult += "▶Duodenum:" + "\r\n" + strResult3 + "\r\n";
                            }
                            else
                            {
                                strResult += "▶Duodenum:" + "\r\n" + strResult3;
                            }
                            if (!strResult4.IsNullOrEmpty())
                            {
                                strResult += "▶Endoscopic Diagnosis:" + "\r\n" + strResult4 + "\r\n";
                            }
                            else
                            {
                                strResult += "▶Endoscopic Diagnosis:" + "\r\n" + strResult4;
                            }

                            strResult += strInfo + "\r\n"; //add

                            if (list[i].GBPRO_2 == "Y")
                            {
                                if (!strResult5.IsNullOrEmpty())
                                {
                                    strResult += "▶Endoscopic Procedure:" + "\r\n" + "CLO" + "\r\n" + strResult5 + "\r\n";
                                }
                                else
                                {
                                    strResult += "▶Endoscopic Procedure:" + "\r\n" + "CLO" + "\r\n" + strResult5;
                                }
                            }
                            else
                            {
                                if (!strResult5.IsNullOrEmpty())
                                {
                                    strResult += "▶Endoscopic Procedure:" + "\r\n" + strResult5 + "\r\n";
                                }
                                else
                                {
                                    strResult += "▶Endoscopic Procedure:" + "\r\n" + strResult5;
                                }
                            }
                            strResult += "▶EndoScopic Biopsy:" + "\r\n" + strResult6 + "\r\n";


                            if (strNew == "Y")
                            {
                                //참고사항
                                if (!strRemark.IsNullOrEmpty())
                                {
                                    strResult += "▶Remark:" + "\r\n" + strRemark + "\r\n";
                                }
                                else
                                {
                                    strResult += "▶Remark:" + "\r\n" + strRemark + "\r\n";
                                }
                                strResult += "처치의사 : " + strResultDrCode + "  " + strKorName;
                            }
                            break;
                        case "3":   //장
                            if (strNew == "Y")
                            {
                                strResult6 = "";
                                if (!list[i].REMARK6.IsNullOrEmpty())
                                {
                                    strResult6 = "smalL INTESTINAL:" + list[i].REMARK6;
                                }
                                if (!list[i].REMARK6_2.IsNullOrEmpty())
                                {
                                    strResult6 += "\r\n" + "LARGE INTESTINAL:" + list[i].REMARK6_2;
                                }
                                if (!list[i].REMARK6_3.IsNullOrEmpty())
                                {
                                    strResult6 += "\r\n" + "RECTUM:" + list[i].REMARK6_3;
                                }
                            }

                            if (!strResult1.IsNullOrEmpty())
                            {
                                strResult += "▶small Intestinal:" + "\r\n" + strResult1 + "\r\n";
                            }
                            else
                            {
                                strResult += "▶small Intestinal:" + "\r\n" + strResult1;
                            }
                            if (!strResult4.IsNullOrEmpty())
                            {
                                strResult += "▶large Intestinal:" + "\r\n" + strResult4 + "\r\n";
                            }
                            else
                            {
                                strResult += "▶large Intestinal:" + "\r\n" + strResult4;
                            }
                            if (!strResult5.IsNullOrEmpty())
                            {
                                strResult += "▶rectum:" + "\r\n" + strResult5 + "\r\n";
                            }
                            else
                            {
                                strResult += "▶rectum:" + "\r\n" + strResult5;
                            }
                            if (!strResult2.IsNullOrEmpty())
                            {
                                strResult += "▶Endoscopic Diagnosis:" + "\r\n" + strResult2 + "\r\n";
                            }
                            else
                            {
                                strResult = strResult + "▶Endoscopic Diagnosis:" + "\r\n" + strResult2;
                            }
                            if (!list[i].GB_CLEAN.IsNullOrEmpty())
                            {
                                strResult += "▶장정결도:" + "\r\n" + list[i].GB_CLEAN + "\r\n";   //2013-06-17
                            }
                            else
                            {
                                strResult += "▶장정결도:" + "\r\n" + list[i].GB_CLEAN;    //2013-06-17
                            }
                            strResult = strResult + strInfo + "\r\n";   //add
                            if (!strResult3.IsNullOrEmpty())
                            {
                                strResult += "▶Endoscopic Procedure:" + "\r\n" + strResult3 + "\r\n";
                            }
                            else
                            {
                                strResult += "▶Endoscopic Procedure:" + "\r\n" + strResult3;
                            }
                            if (!strResult6.IsNullOrEmpty())
                            {
                                strResult += "▶Endoscopic Biopsy:" + "\r\n" + strResult6 + "\r\n";
                            }
                            else
                            {
                                strResult += "▶Endoscopic Biopsy:" + "\r\n" + strResult6;
                            }

                            if (strNew == "Y")
                            {
                                //참고사항
                                if (!strRemark.IsNullOrEmpty())
                                {
                                    strResult += "▶Remark:" + "\r\n" + strRemark + "\r\n";
                                }
                                else
                                {
                                    strResult += "▶Remark:" + "\r\n" + strRemark + "\r\n";
                                }
                                strResult += "처치의사 : " + strResultDrCode + "  " + strKorName;
                            }
                            break;
                        default:
                            //ERCP
                            strResult = "▶ERCP Finding:" + "\r\n" + strResult1 + "\r\n";
                            strResult = strResult + "▶Diagnosis:" + "\r\n" + strResult2 + "\r\n";
                            strResult = strResult + "▶Plan & Tx:" + "\r\n" + strResult3 + "\r\n";
                            strResult = strResult + "▶EndoScopic Procedure:" + "\r\n" + strResult4;
                            strResult = strResult + "▶EndoScopic Biopsy:" + "\r\n" + strResult6;
                            if (strNew == "Y")
                            {
                                strResult = strResult + "\r\n" + strResult6_2;
                                strResult = strResult + "\r\n" + strResult6_3 + "\r\n";
                                strResult = strResult + "처치의사 : " + strResultDrCode + "  " + strKorName;
                            }
                            break;
                    }
                    //------------------------------------------------------------------------------------------------
                    strEndoAllResult += strGbJob + "{*}" + strResult + "{{$}}";
                }
                strResult = "";
                strRESULTDATE = "";
            }

            rtnVal = strEndoAllResult;

            return rtnVal;
        }

        private void ssUserChart_EditorFocused(object sender, EditorNotifyEventArgs e)
        {
            if (e.EditingControl.GetType().Equals(typeof(FarPoint.Win.Spread.CellType.GeneralEditor)))
            {
                FarPoint.Win.Spread.CellType.GeneralEditor ge = (FarPoint.Win.Spread.CellType.GeneralEditor)e.EditingControl;
                ge.Tag = e.Row;
                ge.PreviewKeyDown += Ge_PreviewKeyDown;
            }
        }

        private void Ge_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (SS2_Sheet1.ActiveRowIndex + 1 > SS2_Sheet1.RowCount - 1)
                    return;

                SS2_Sheet1.ActiveRowIndex += 1;
            }
        }

        void eSpreadEditModeOn(object sender, EventArgs e)
        {
            if (sender == SS2)
            {
                if (SS2.ActiveSheet.NonEmptyRowCount == 0) return;

                int nRow = this.SS2.ActiveSheet.ActiveRow.Index;
                int nCol = this.SS2.ActiveSheet.ActiveColumn.Index;
                string strResCode = "";

                if (nCol != 2) return;

                strResCode = SS2.ActiveSheet.Cells[nRow, 7].Text.Trim();
                if (strResCode.IsNullOrEmpty())
                {
                    sp.Spread_All_Clear(ssList1);
                    FnClickRow = -1;
                    return;
                }

                //자료를 READ
                List<HIC_RESCODE> list = hicRescodeService.GetCodeNamebyBindGubun(strResCode);

                ssList1.ActiveSheet.RowCount = list.Count;
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        ssList1.ActiveSheet.Cells[i, 0].Text = list[i].CODE;
                        ssList1.ActiveSheet.Cells[i, 1].Text = list[i].NAME;
                    }
                }
                else
                {
                    sp.Spread_All_Clear(ssList1);
                }
            }
        }


        void eSpreadEditModeOff(object sender, EventArgs e)
        {
            int nRow = this.SS2.ActiveSheet.ActiveRow.Index;

            if (SS2.InputDeviceType.ToString() == "Keyboard")
            {
                if (nRow == SS2.ActiveSheet.RowCount - 1)
                {
                    SS2.ActiveSheet.Cells[nRow, (int)clsHcType.Instrument_Result.CHANGE].Text = "Y";
                    if (MessageBox.Show("결과값을 저장하시겠습니까?", "확인창", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        eBtnClick(btnSave, new EventArgs());
                    }
                    return;
                }
            }
        }

        string fn_BlankLine_Delete(string ArgData)
        {
            string rtnVal = "";
            string strResult = "";
            int nPos1 = 0;
            int nPos2 = 0;
            int nPos3 = 0;
            int nPos4 = 0;
            int nPos5 = 0;
            int nPos6 = 0;

            strResult = ArgData;

            while (nPos1 != 0 || nPos2 != 0 || nPos3 != 0 || nPos4 != 0 || nPos5 != 0)
            {
                nPos1 = VB.InStr(strResult, "\t");
                if (nPos1 > 0) strResult = strResult.Replace("\t", " ");
                nPos2 = VB.InStr(strResult, "\r\n" + "\r\n");
                if (nPos2 > 0) strResult = strResult.Replace("\r\n" + "\r\n", "\r\n");
                nPos3 = VB.InStr(strResult, "\n" + "\n");
                if (nPos3 > 0) strResult = strResult.Replace("\n" + "\n", "\n");
                nPos4 = VB.InStr(strResult, "\r\n" + " " + "\r\n");
                if (nPos4 > 0) strResult = strResult.Replace("\r\n" + " " + "\r\n", "\r\n");
                nPos5 = VB.InStr(strResult, "  ");
                if (nPos5 > 0) strResult = strResult.Replace("  ", " ");
            }

            rtnVal = strResult;
            return rtnVal;
        }

        void fn_Hic_Memo_Screen()
        {
            long nPano = 0;
            int nRow = 0;
            int nRead = 0;

            sp.Spread_All_Clear(SS_ETC);

            nPano = ssPatInfo.ActiveSheet.Cells[0, 0].Text.Trim().To<long>();
            if (nPano == 0) return;

            //참고사항 Display
            List<HIC_MEMO> list = hicMemoService.GetItembyPaNo(nPano);

            nRead = list.Count;
            SS_ETC.ActiveSheet.RowCount = nRead + 5;
            for (int i = 0; i < nRead; i++)
            {
                SS_ETC.ActiveSheet.Cells[i, 1].Text = list[i].ENTTIME.ToString();
                SS_ETC.ActiveSheet.Cells[i, 2].Text = list[i].MEMO;

                SS_ETC.ActiveSheet.Cells[i, 3].Text = hb.READ_HIC_InsaName(list[i].JOBSABUN.ToString()).Trim();
                SS_ETC.ActiveSheet.Cells[i, 4].Text = list[i].RID;
            }
        }

        /// <summary>
        /// 거래처코드 찾기 화면 스프레드더블클릭이벤트
        /// </summary>
        /// <param name="item"></param>
        private void LtdCd_value(HIC_LTD item)
        {
            LtdHelpItem = item;
        }
    }
}
