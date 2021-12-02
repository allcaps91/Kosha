using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Spread;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcGaJepsuVIew.cs
/// Description     : 일반검진 가접수 명단 조회
/// Author          : 김민철
/// Create Date     : 2020-05-26
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmGaJepsuView(HcMain74.frm)" />
namespace ComHpcLibB
{
    public partial class frmHcGaJepsuVIew : Form
    {
        HicSunapdtlWorkService hicSunapdtlWorkService = null;
        HicXrayResultService hicXrayResultService = null;
        BasPatientService basPatientService = null;
        HicGroupexamService hicGroupexamService = null;

        ComFunc   CF        = null;
        clsHaBase cHB       = null;
        clsSpread cSpd      = null;
        clsHcOrderSend hOrdSend = null;
        clsHcMain cHcMain = null;

        HIC_LTD LtdHelpItem = null;
        HicJepsuWorkService hicJepsuWorkService = null;

        public frmHcGaJepsuVIew()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }

        private void SetEvent()
        {
            hicSunapdtlWorkService = new HicSunapdtlWorkService();
            hicXrayResultService = new HicXrayResultService();
            basPatientService = new BasPatientService();
            hicGroupexamService = new HicGroupexamService();

            this.Load               += new EventHandler(eFormLoad);
            this.btnExit.Click      += new EventHandler(eBtnClick);
            this.btnLtdHelp.Click   += new EventHandler(eBtnClick);
            this.btnDelete.Click    += new EventHandler(eBtnClick);
            this.btnSearch.Click    += new EventHandler(eBtnClick);
            this.btnPrint.Click     += new EventHandler(eBtnClick);
            this.btnXrayChul.Click  += new EventHandler(eBtnClick);
            this.btnXrayChulPrt.Click += new EventHandler(eBtnClick);
            this.btnExcel.Click += new EventHandler(eBtnClick);
            this.txtLtdName.KeyPress += new KeyPressEventHandler(eKeyPress);
        }

        private void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtLtdName && e.KeyChar == (char)13)
            {
                if (!txtLtdName.Text.Trim().IsNullOrEmpty()) { Ltd_Code_Help(); }
            }
        }

        private void SetControl()
        {
            CF = new ComFunc();
            cHB = new clsHaBase();
            cSpd = new clsSpread();
            hOrdSend = new clsHcOrderSend();
            cHcMain = new clsHcMain();
            LtdHelpItem = new HIC_LTD();
            hicJepsuWorkService = new HicJepsuWorkService();

            int nYY = DateTime.Now.ToShortDateString().Substring(0, 4).To<int>();

            cboYYMM.Items.Clear();
            for (int i = 1; i < 4; i++)
            {
                cboYYMM.Items.Add(VB.Format(nYY, "0000"));
                nYY += 1;
            }

            cboYYMM.SelectedIndex = 0;

            cboJong.Items.Clear();
            cboJong.Items.Add("**.전체");
            cHB.ComboJong_AddItem(cboJong, true);
            cboJong.SelectedIndex = 0;


            #region SS1 Spread Set
            SS1.Initialize(new SpreadOption { IsRowSelectColor = true, RowHeight = 20 });
            SS1.AddColumnCheckBox("삭제",     "", 47, new CheckBoxBooleanCellType { IsHeaderCheckBox = true }).ButtonClick += new EditorNotifyEventHandler(eSpreadBtnClicked);
            SS1.AddColumn("검진년도",     nameof(HIC_JEPSU_WORK.GJYEAR),      58, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("접수일자",     nameof(HIC_JEPSU_WORK.JEPDATE),     92, new SpreadCellTypeOption { IsEditble = false, IsSort = true });
            SS1.AddColumn("검진번호",     nameof(HIC_JEPSU_WORK.PANO),        88, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("수검자명",     nameof(HIC_JEPSU_WORK.SNAME),       92, new SpreadCellTypeOption { IsEditble = false, IsSort = true, Aligen = CellHorizontalAlignment.Left, isFilter = true });
            SS1.AddColumn("성별",         nameof(HIC_JEPSU_WORK.SEX),         44, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("나이",         nameof(HIC_JEPSU_WORK.AGE),         44, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("검진종류",     nameof(HIC_JEPSU_WORK.GJJONG),      44, new SpreadCellTypeOption { IsEditble = false, IsSort = true });
            SS1.AddColumn("검진차수",     nameof(HIC_JEPSU_WORK.GJCHASU),     44, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("검진반기",     nameof(HIC_JEPSU_WORK.GJBANGI),     44, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("출장여부",     nameof(HIC_JEPSU_WORK.GBCHUL),      44, new SpreadCellTypeOption { IsEditble = false, IsSort = true });
            SS1.AddColumn("사업장코드",   nameof(HIC_JEPSU_WORK.LTDCODE),     68, new SpreadCellTypeOption { IsEditble = false, IsSort = true, isFilter = true });
            SS1.AddColumn("사업장명",     nameof(HIC_JEPSU_WORK.LTDNAME),    120, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left, IsSort = true, isFilter = true });
            SS1.AddColumn("우편번호",     nameof(HIC_JEPSU_WORK.MAILCODE),    78, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("주소1",        nameof(HIC_JEPSU_WORK.JUSO1),      180, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("주소2",        nameof(HIC_JEPSU_WORK.JUSO2),      200, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("등록번호",     nameof(HIC_JEPSU_WORK.PTNO),        88, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("부담율",       nameof(HIC_JEPSU_WORK.BURATE),      47, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("지사코드",     nameof(HIC_JEPSU_WORK.JISA),        54, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("사업장기호",   nameof(HIC_JEPSU_WORK.KIHO),        88, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("증번호",       nameof(HIC_JEPSU_WORK.GKIHO),       88, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("직종구분",     nameof(HIC_JEPSU_WORK.JIKGBN),      44, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("유해인자",     nameof(HIC_JEPSU_WORK.UCODES),     160, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("선택검사",     nameof(HIC_JEPSU_WORK.SEXAMS),     180, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("직종",         nameof(HIC_JEPSU_WORK.JIKJONG),     68, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("사원번호",     nameof(HIC_JEPSU_WORK.SABUN),       68, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("최초입사일자", nameof(HIC_JEPSU_WORK.IPSADATE),    92, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("현작업부서명", nameof(HIC_JEPSU_WORK.BUSENAME),    88, new SpreadCellTypeOption { IsEditble = false, IsSort = true, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("전직직종",     nameof(HIC_JEPSU_WORK.OLDJIKJONG),  88, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("현직전입일자", nameof(HIC_JEPSU_WORK.BUSEIPSA),    92, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("전직시작일자", nameof(HIC_JEPSU_WORK.OLDSDATE),    92, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("전직종료일자", nameof(HIC_JEPSU_WORK.OLDEDATE),    92, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("2차검진일자",  nameof(HIC_JEPSU_WORK.SECOND_DATE), 92, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("수첩소지여부", nameof(HIC_JEPSU_WORK.GBSUCHEP),    47, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("발급연도",     nameof(HIC_JEPSU_WORK.BALYEAR),     44, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("발급일련번호", nameof(HIC_JEPSU_WORK.BALSEQ),      54, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("접수취소일자", nameof(HIC_JEPSU_WORK.DELDATE),     92, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("작업자사번",   nameof(HIC_JEPSU_WORK.JOBSABUN),    88, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("최종작업시각", nameof(HIC_JEPSU_WORK.ENTTIME),     92, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("바코드발행",   nameof(HIC_JEPSU_WORK.GBEXAM),      47, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("검진문진상태", nameof(HIC_JEPSU_WORK.GBMUNJIN1),   47, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("구강문진상태", nameof(HIC_JEPSU_WORK.GBMUNJIN2),   47, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("구강검사여부", nameof(HIC_JEPSU_WORK.GBDENTAL),    47, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("인원통계",     nameof(HIC_JEPSU_WORK.GBINWON),     47, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("보건소",       nameof(HIC_JEPSU_WORK.BOGUNSO),     88, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("2차간염",      nameof(HIC_JEPSU_WORK.LIVER2),      44, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("영업소코드",   nameof(HIC_JEPSU_WORK.YOUNGUPSO),   88, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("마일리지암",   nameof(HIC_JEPSU_WORK.MILEAGEAM),   47, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("무료암여부",   nameof(HIC_JEPSU_WORK.MURYOAM),     47, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("검진대상자",   nameof(HIC_JEPSU_WORK.GUMDAESANG),  47, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("주민번호",     nameof(HIC_JEPSU_WORK.JUMINNO2),   120, new SpreadCellTypeOption { IsEditble = false , IsVisivle = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("생년월일",     nameof(HIC_JEPSU_WORK.JUMINNO),   120, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("휴대폰번호",   nameof(HIC_JEPSU_WORK.HPHONE),     120, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("비고",         nameof(HIC_JEPSU_WORK.REMARK),     180, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("ROWID",        nameof(HIC_JEPSU_WORK.RID),         47, new SpreadCellTypeOption { IsVisivle = false });
            #endregion
        }

        private void eSpreadBtnClicked(object sender, EditorNotifyEventArgs e)
        {
            if (sender == SS1)
            {
                SS1.DeleteRow(e.Row);
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnLtdHelp)
            {
                Ltd_Code_Help();
            }
            else if (sender == btnSearch)
            {
                Screen_Display();
            }
            else if (sender == btnPrint)
            {
                Spread_Print();
            }
            else if (sender == btnXrayChul)
            {
                Make_Xray_List();
            }
            else if ( sender == btnXrayChulPrt)
            {
                frmHcXrayChulList frm = new frmHcXrayChulList();
                frm.ShowDialog();
            }
            else if (sender == btnDelete)
            {
                Data_Delete();
            }
            else if (sender == btnExcel)
            {
                Excel_Down();
            }
        }

        private void Excel_Down()
        {
            long nGubun = 0;
            string strTitle = "";
            string strDate = "";
            string strFileName = "";

            strDate = "가접수명단(" + VB.Replace(DateTime.Now.ToShortDateString(), " - ", "")+")" ;

            DirectoryInfo Dir = new DirectoryInfo(@"c:\WORK");
            if (!Dir.Exists)
            {
                Dir.Create();
            }

            strFileName = @"c:\WORK\" + strDate + ".xls";

            SS1.SaveExcel(strFileName, "");

            MessageBox.Show(@"엑셀파일 저장 완료", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void Data_Delete()
        {
            if (MessageBox.Show("선택하신 가접수 자료를 삭제하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            IList<HIC_JEPSU_WORK> list = SS1.GetEditbleData<HIC_JEPSU_WORK>();

            if (list.Count > 0)
            {
                if (hicJepsuWorkService.Save(list))
                {
                    MessageBox.Show("삭제하였습니다");
                    Screen_Display();
                }
                else
                {
                    MessageBox.Show("오류가 발생하였습니다. ");
                }
            }
        }

        private void Make_Xray_List()
        {
            //string strFileName = "";
            string strLtdName = "";
            string strROWID = "";
            string strXrayno = "";
            string strLtdCode = "";
            string strJepDate = "";
            string strDate = "";
            string strPANO = "";
            string strPtNo = "";
            string strName = "";
            string strSex = "";
            string strAge = "";
            string strJong = "";
            string strExCode = "";
            string strXrayName = "";
            string strDate1 = "";
            string strCHUL = "";
            string strXChk = "";
            string strJumin = "";

            string strOK = "";
            string strOK2 = "";
            int nRow = 0;
            int nXrayNoCnt = 0;

            string sDirPath = "C:\\출장Xray";

            int result = 0;

            string strFDate = "";
            string strTDate = "";
            string strGjYear = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            strDate = VB.InputBox("출장나갈 일자는 ? (YYYY-MM-DD) : ", "출장일자(촬영일자세팅)", clsPublic.GstrSysDate);

            if (!VB.IsDate(strDate))
            {
                return;
            }

            if (SS1.ActiveSheet.RowCount < 1)
            {
                return;
            }

            if (txtLtdName.Text.IsNullOrEmpty())
            {
                MessageBox.Show("회사명이 공란입니다.", "작업불가", MessageBoxButtons.OK, MessageBoxIcon.Error);
                strLtdName = "회사명공란";
                return;                
            }
            else
            {
                strLtdName = VB.Pstr(txtLtdName.Text, ".", 2);
            }

            //폴더생성
            DirectoryInfo Dir = new DirectoryInfo(sDirPath);

            if (Dir.Exists == false)
            {
                Dir.Create();
            }

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsDB.setBeginTran(clsDB.DbCon);

            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                strOK = "";
                strOK2 = "";

                strOK2 = SS1.ActiveSheet.Cells[i, 0].Text;
                strJepDate = SS1.ActiveSheet.Cells[i, 2].Text;
                strPANO = SS1.ActiveSheet.Cells[i, 3].Text;

                if (strOK2 == "True")    //체크된것만
                {
                    //방사선촬영이 있는지 CHECK
                    HIC_SUNAPDTL_WORK list = hicSunapdtlWorkService.GetExCodeHNamebyPaNoSudate(strPANO, strJepDate);

                    if (!list.IsNullOrEmpty())
                    {
                        strOK = "OK";
                    }

                    //방사선코드가 있다면
                    if (strOK == "OK")
                    {
                        //출장인 사람만
                        strCHUL = SS1.ActiveSheet.Cells[i, 10].Text;
                        strPtNo = SS1.ActiveSheet.Cells[i, 16].Text;
                        strROWID = SS1.ActiveSheet.Cells[i, 52].Text;
                        if (strCHUL == "Y")
                        {
                            strDate1 = string.Format("{0:yyyy-MM-dd}", strDate);

                            //한사람이 2건 가접수가 있어도 XRay 번호는 1건만 생성함
                            strXrayno = hicXrayResultService.GetXrayNoByJepDate(strDate, strPtNo);

                            if (strXrayno.IsNullOrEmpty())
                            {
                                do
                                {   
                                    strXrayno = strDate1 + string.Format("{0:00000}", hOrdSend.HicNew_XrayNo_Create_Chul());

                                    //동일한 방사선번호가 있으면 다른 번호를 생성함
                                    nXrayNoCnt = hicXrayResultService.GetCountbyJepDateXrayno(strDate, strXrayno); 
                                   
                                } while (nXrayNoCnt != 0);

                                strLtdCode = SS1.ActiveSheet.Cells[i, 11].Text;
                                strLtdName = cHB.READ_Ltd_Name(SS1.ActiveSheet.Cells[i, 11].Text);
                                strPANO = SS1.ActiveSheet.Cells[i, 3].Text;
                                strName = SS1.ActiveSheet.Cells[i, 4].Text;
                                strSex = SS1.ActiveSheet.Cells[i, 5].Text;
                                strAge = SS1.ActiveSheet.Cells[i, 6].Text;
                                strJong = SS1.ActiveSheet.Cells[i, 7].Text;
                                strExCode = list.EXCODE;
                                strXrayName = list.HNAME;

                                //분진체크
                                if (hicJepsuWorkService.GetCountbyPaNoSuDate(strPANO, strJepDate) > 0)
                                {
                                    strXChk = "Y";
                                }
                                //분진일경우 Chest-dust 적용
                                if (strXChk == "Y")
                                {
                                    strXrayName = "Chest-dust";
                                }

                                strJumin = basPatientService.GetJuminbyPaNo(strPtNo);

                                //건진번호에서 -> 외래번호로 수정
                                nRow += 1;
                                if (nRow > SS2.ActiveSheet.RowCount)
                                {
                                    SS2.ActiveSheet.RowCount = nRow;
                                }
                                SS2.ActiveSheet.Cells[nRow - 1, 0].Text = strPtNo;
                                SS2.ActiveSheet.Cells[nRow - 1, 1].Text = strLtdName;
                                SS2.ActiveSheet.Cells[nRow - 1, 2].Text = strXrayno;
                                SS2.ActiveSheet.Cells[nRow - 1, 3].Text = strName;
                                SS2.ActiveSheet.Cells[nRow - 1, 4].Text = strSex;
                                SS2.ActiveSheet.Cells[nRow - 1, 5].Text = strAge;
                                SS2.ActiveSheet.Cells[nRow - 1, 6].Text = strXrayName;
                                SS2.ActiveSheet.Cells[nRow - 1, 7].Text = strJumin;

                                //Hic_Xray_Result_Insert

                                HIC_XRAY_RESULT item = new HIC_XRAY_RESULT();

                                item.JEPDATE = strDate;
                                item.XRAYNO = VB.Replace(strXrayno,"-","");
                                item.PANO = long.Parse(strPANO);
                                item.PTNO = strPtNo;
                                item.SNAME = strName;
                                item.SEX = strSex;
                                item.AGE = long.Parse(strAge);
                                item.GJJONG = strJong;
                                item.GBCHUL = strCHUL;
                                item.LTDCODE = long.Parse(strLtdCode);
                                item.XCODE = strExCode;
                                if (strXChk == "Y")
                                {
                                    item.GBREAD = "2";
                                }
                                else
                                {
                                    item.GBREAD = "1";
                                }
                                item.GBSTS = "0";
                                item.ENTSABUN = long.Parse(clsType.User.IdNumber);
                                item.GBCONV = "Y";

                                result = hicXrayResultService.SaveHicXrayResultWork(item);

                                //if (result < 0)
                                //{
                                //    clsDB.setRollbackTran(clsDB.DbCon);
                                //    MessageBox.Show("자료(HIC_XRAY_RESULT_WORK) 저장시 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                //    return;
                                //}

                                //가접수에 촬영번호 Update
                                strFDate = strDate;
                                strTDate = VB.Left(strDate, 4) + "-01-01";
                                strGjYear = VB.Left(strDate, 4);
                                strXrayno = VB.Replace(strXrayno, "-", "");

                                result = hicJepsuWorkService.UpdateXrayNo(strXrayno, strFDate, strTDate, strLtdCode, strGjYear, long.Parse(strPANO));

                                if (result < 0)
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    MessageBox.Show("자료(HIC_JEPSU_WORK) 저장시 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                SS1.ActiveSheet.Cells[i, 0, i, 3].BackColor = Color.FromArgb(240, 180, 0);
                                SS1.ActiveSheet.Cells[i, 0, i, 9].BackColor = Color.FromArgb(240, 180, 0);
                            }
                        }
                    }
                }
            }

            clsDB.setCommitTran(clsDB.DbCon);
            SS2.ActiveSheet.RowCount = nRow;
            MessageBox.Show(SS1.ActiveSheet.RowCount + "( " + nRow + " ) 건의 자료가 생성되었습니다.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Spread_Print()
        {
            string  strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "가 접  수  자    명  단";
            strHeader = cSpd.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += cSpd.setSpdPrint_String("조회기간: " + dtpFDate.Text + "~" + dtpTDate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += cSpd.setSpdPrint_String("검진회사: " + VB.Pstr(txtLtdName.Text, ".", 2), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += cSpd.setSpdPrint_String("인쇄시각: " + DateTime.Now.ToString() + " " + "  PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strFooter = cSpd.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            cSpd.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void Screen_Display()
        {
            string strPano = "";
            string strSudate = "";
            string strGjjong = "";

            string strUcodes = "";
            string strJumin = string.Empty;
            string strYear = cboYYMM.Text;
            string strFDate = dtpFDate.Text;
            string strTDate = CF.DATE_ADD(clsDB.DbCon, dtpTDate.Text, 1);
            string strGbChul = rdoGbn1.Checked == true ? "Y" : rdoGbn2.Checked == true ? "N" : "";
            string strGjJong = VB.Pstr(cboJong.Text, ".", 1);
            long nLtdCode = VB.Pstr(txtLtdName.Text, ".", 1).To<long>();

            int nA151 = 0;
            int nTR11 = 0;
            int nMU15 = 0;
            int nMU27 = 0;
            int nLM11 = 0;
            int nTH15 = 0;

            string strCountMsg = "";

            List<string> strGroupCodes = null;
            List<string> strCodes = null;

            strGroupCodes = new List<string>();
            strCodes = new List<string>();

            strCodes.Add("A151");
            strCodes.Add("TR11");
            strCodes.Add("MU15");
            strCodes.Add("MU27");
            strCodes.Add("LM11");
            strCodes.Add("TH15");



            IList<HIC_JEPSU_WORK> list = hicJepsuWorkService.GetListByItem(strYear, strFDate, strTDate, strGbChul, strGjJong, nLtdCode);

            SS1.ActiveSheet.RowCount = 0;

            if (list.Count > 0)
            {
                SS1.DataSource = list;

                
                if(chkChul.Checked == true)
                {
                    for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                    {
                        strUcodes = SS1.ActiveSheet.Cells[i, 22].Text;
                        SS1.ActiveSheet.Cells[i, 22].Text = cHcMain.UCode_Names_Display(strUcodes);
                        strJumin = clsAES.DeAES(SS1.ActiveSheet.Cells[i, 49].Text.Trim());
                        SS1.ActiveSheet.Cells[i, 49].Text = VB.Left(strJumin, 6) + "-" + VB.Right(strJumin, 7);


                        // A151 심전도
                        // TR11 폐기능
                        // MU15 요중메틸마뇨산
                        // MU27 오르토-크레졸
                        // LM11 요세포검사 
                        // JL04,JL05 특수청력
                        strSudate = SS1.ActiveSheet.Cells[i, 2].Text;
                        strPano = SS1.ActiveSheet.Cells[i, 3].Text;
                        strGjjong = SS1.ActiveSheet.Cells[i, 7].Text;

                        strGroupCodes = new List<string>();
                        List<HIC_SUNAPDTL_WORK> list1 = hicSunapdtlWorkService.GetItembyPaNoSuDateGjJong(strPano, strSudate, strGjjong);
                        if (!list1.IsNullOrEmpty())
                        {
                            for (int j = 0; j < list1.Count; j++)
                            {
                                strGroupCodes.Add(list1[j].CODE);
                            }
                        }

                        List<HIC_GROUPEXAM> list2 = hicGroupexamService.GetExcodeByGroupCode(strCodes, strGroupCodes);
                        if (!list2.IsNullOrEmpty())
                        {
                            for (int j = 0; j < list2.Count; j++)
                            {
                                if (list2[j].EXCODE.Trim() == "A151")
                                {
                                    nA151 += 1;
                                }
                                else if (list2[j].EXCODE.Trim() == "TR11")
                                {
                                    nTR11 += 1;
                                }
                                else if (list2[j].EXCODE.Trim() == "MU15")
                                {
                                    nMU15 += 1;
                                }
                                else if (list2[j].EXCODE.Trim() == "MU27")
                                {
                                    nMU27 += 1;
                                }
                                else if (list2[j].EXCODE.Trim() == "LM11")
                                {
                                    nLM11 += 1;
                                }
                                else if (list2[j].EXCODE.Trim() == "TH15")
                                {
                                    nTH15 += 1;
                                }

                            }
                        }
                    }

                    SS1.ActiveSheet.RowCount = list.Count + 1;
                    //검사항목 Count표시
                    strCountMsg = "●총인원: " + SS1.ActiveSheet.RowCount + "명";
                    SS1.ActiveSheet.Cells[list.Count, 2].Text = strCountMsg +"  "+ "심전도: " + nA151 + "건, " + "폐기능: " + nTR11 + "건, " + "메틸마뇨산: " + nMU15 + "건, " + "크레졸: " + nMU27 + "건, " + "요세포: " + nLM11 + "건, " + "특수청력: " + nTH15 + "건";
                    SS1_Sheet1.AddSpanCell(list.Count, 2, 1, 12);
                }
                
            }
            else
            {
                MessageBox.Show("접수된 자료가 없습니다.", "확인");
                return;
            }
        }

        /// <summary>
        /// 사업장 코드 검색창 연동
        /// </summary>
        private void Ltd_Code_Help()
        {
            string strFind = "";

            if (txtLtdName.Text.Contains("."))
            {
                strFind = VB.Pstr(txtLtdName.Text, ".", 2).Trim();
            }
            else
            {
                strFind = txtLtdName.Text.Trim();
            }
            
            frmHcLtdHelp frm = new frmHcLtdHelp(strFind);
            frm.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(ePost_value_LTD);
            frm.ShowDialog();

            if (!LtdHelpItem.IsNullOrEmpty() && LtdHelpItem.CODE > 0)
            {
                txtLtdName.Text = LtdHelpItem.CODE.To<string>();
                txtLtdName.Text = txtLtdName.Text + "." + LtdHelpItem.SANGHO;
            }
            else
            {
                txtLtdName.Text = "";
            }
        }

        private void ePost_value_LTD(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        private void frmHcGaJepsuVIew_ButtonClick(object sender, EditorNotifyEventArgs e)
        {
            HIC_JEPSU_WORK code = SS1.GetRowData(e.Row) as HIC_JEPSU_WORK;

            SS1.DeleteRow(e.Row);
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            dtpFDate.Text = CF.DATE_ADD(clsDB.DbCon, DateTime.Now.ToShortDateString(), - 10);
            dtpTDate.Text = DateTime.Now.ToShortDateString();
        }

        
    }
}
