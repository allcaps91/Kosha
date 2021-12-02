using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Spread;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHaExcelImport.cs
/// Description     : 종검예약 엑셀저장
/// Author          : 김민철
/// Create Date     : 2020-03-16
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm종검예약엑셀저장(Frm종검예약엑셀저장.frm)" />
namespace HC_Main
{
    public partial class frmHaExcelImport : Form
    {
        HeaExcelService heaExcelService = null;
        HicPatientService hicPatientService = null;
        ComFunc CF = null;
        HIC_LTD LtdHelpItem = null;

        long FnLtdCode = 0;

        public frmHaExcelImport()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            heaExcelService = new HeaExcelService();
            hicPatientService = new HicPatientService();
            CF = new ComFunc();
            LtdHelpItem = new HIC_LTD();

            int nYY = DateTime.Now.ToShortDateString().Substring(0, 4).To<int>();

            cboYYMM.Items.Clear();
            for (int i = 1; i < 4; i++)
            {
                cboYYMM.Items.Add(VB.Format(nYY, "0000"));
                nYY += 1;
            }

            cboYYMM.SelectedIndex = 0;

            cboExcelType.Items.Clear();
            cboExcelType.Items.Add("01.표준서식(1)");
            cboExcelType.Items.Add("02.표준서식(2)");
            cboExcelType.Items.Add("06.조선내화");

            SetLtdCombo(cboYYMM.Text);

            SS2.Initialize(new SpreadOption { IsRowSelectColor = true, RowHeight = 20 });
            SS2.AddColumnCheckBox("삭제", nameof(HEA_EXCEL.CHK), 47, new CheckBoxBooleanCellType { IsHeaderCheckBox = true }).ButtonClick += FrmHaExcelImport_ButtonClick; ;
            SS2.AddColumn("부서명",            nameof(HEA_EXCEL.LTDBUSE),       64, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS2.AddColumn("직원성명",          nameof(HEA_EXCEL.JNAME),         64, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS2.AddColumn("관계",              nameof(HEA_EXCEL.REL),           47, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS2.AddColumn("수검자명",          nameof(HEA_EXCEL.SNAME),         68, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS2.AddColumn("주민등록번호",      nameof(HEA_EXCEL.AES_JUMIN),     92, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS2.AddColumn("휴대폰",            nameof(HEA_EXCEL.HPHONE),        78, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS2.AddColumn("종검유형",          nameof(HEA_EXCEL.GJTYPE),        78, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS2.AddColumn("검진희망일",        nameof(HEA_EXCEL.HDATE),         78, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS2.AddColumn("오전오후",          nameof(HEA_EXCEL.AMPM),          44, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS2.AddColumn("공단검진",          nameof(HEA_EXCEL.GBNHIC),        44, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS2.AddColumn("회사부담 선택검사", nameof(HEA_EXCEL.LTDADDEXAM),   140, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS2.AddColumn("본인부담 선택검사", nameof(HEA_EXCEL.BONINADDEXAM), 120, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS2.AddColumn("특수검진 취급물질", nameof(HEA_EXCEL.MCODES),       160, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS2.AddColumn("병원",              nameof(HEA_EXCEL.HOSPITAL),      47, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS2.AddColumn("생년월일",          nameof(HEA_EXCEL.BIRTH),         74, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS2.AddColumn("참고사항",          nameof(HEA_EXCEL.REMARK),       160, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS2.AddColumn("예약일시",          nameof(HEA_EXCEL.RDATE),         78, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS2.AddColumn("등록번호",          nameof(HEA_EXCEL.PTNO),          78, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS2.AddColumn("사무직",            nameof(HEA_EXCEL.GBSAMU),        70, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS2.AddColumn("교대근무",          nameof(HEA_EXCEL.GBNIGHT),       68, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS2.AddColumn("전화번호",          nameof(HEA_EXCEL.TEL),           78, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS2.AddColumn("사번",              nameof(HEA_EXCEL.LTDSABUN),      68, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS2.AddColumn("직책",              nameof(HEA_EXCEL.JIKNAME),       64, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS2.AddColumn("입사일자",          nameof(HEA_EXCEL.IPSADATE),      78, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS2.AddColumn("증번호",            nameof(HEA_EXCEL.GKIHO),         82, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS2.AddColumn("주소",              nameof(HEA_EXCEL.JUSO),         160, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS2.AddColumn("공단자격",          nameof(HEA_EXCEL.NHICINFO),     140, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS2.AddColumn("ROWID",             nameof(HEA_EXCEL.RID),           50, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left, IsVisivle = false });
            SS2.AddColumn("변경",              "",                              50, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
        }

        private void SetLtdCombo(string argYYMM)
        {
            cboLtd.Items.Clear();

            List<HEA_EXCEL> lst = heaExcelService.GetListByYear(cboYYMM.Text);
            if (lst.Count > 0)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    cboLtd.Items.Add(lst[i].LTDCODE.To<string>() + "." + lst[i].LTDNAME.Trim());
                }
            }
        }

        private void FrmHaExcelImport_ButtonClick(object sender, EditorNotifyEventArgs e)
        {
            HEA_EXCEL code = SS2.GetRowData(e.Row) as HEA_EXCEL;

            SS2.DeleteRow(e.Row);
        }

        private void SetEvent()
        {
            this.Load               += new EventHandler(eFormLoad);
            this.btnExit.Click      += new EventHandler(eBtnClick);
            this.btnSearch.Click    += new EventHandler(eBtnClick);
            this.btnLtdHelp.Click   += new EventHandler(eBtnClick);
            this.btnDialog.Click    += new EventHandler(eBtnClick);
            this.btnImport.Click    += new EventHandler(eBtnClick);
            this.btnConvert.Click   += new EventHandler(eBtnClick);
            this.btnSave.Click      += new EventHandler(eBtnClick);
            this.btnSave2.Click     += new EventHandler(eBtnClick);
            this.txtLtdName.KeyPress += new KeyPressEventHandler(eKeyPress);
        }

        private void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtLtdName && e.KeyChar == (char)13)
            {
                if (!txtLtdName.Text.Trim().IsNullOrEmpty()) { Ltd_Code_Help(); }
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
            //저장된 명단 조회
            else if (sender == btnSearch)
            {
                long nLtdCode = VB.Pstr(cboLtd.Text.Trim(), ".", 1).To<long>();
                string strSName = txtSName.Text.Trim();
                string strBirth = txtBirth.Text.Trim();
                string strYear = cboYYMM.Text.Trim();

                if (nLtdCode > 0) { Screen_Display(strYear, nLtdCode, strSName, strBirth); }
            }
            //Excel File 선택하기
            else if (sender == btnDialog)
            {
                OpenFileDialog OFD = new OpenFileDialog();
                OFD.Filter = "Excel 통합 문서 (*.xlsx)|*.xlsx|Excel 97 - 2003 통합 문서 (*.xls)|*.xls";
                if (OFD.ShowDialog() == DialogResult.OK)
                {
                    txtFile1.Clear();
                    txtFile1.Text = OFD.FileName;
                }
            }
            //Excel 파일 스프레드에 Import
            else if (sender == btnImport)
            {
                if (SS1.IsExcelFile(txtFile1.Text) == true)
                {
                    SS1_Sheet1.OpenExcel(txtFile1.Text, 0);
                }
                else
                {
                    MessageBox.Show("엑셀파일이 아니거나 잠겨져 있습니다.", "확인요망");
                }
            }
            //DB형식으로 변환
            else if (sender == btnConvert)
            {
                Convert_ExcelFile_To_DataBaseType();
            }
            //DB로 저장
            else if (sender == btnSave)
            {
                Data_Save("");
            }
            //명단저장
            else if (sender == btnSave2)
            {
                if (SS2.ActiveSheet.NonEmptyRowCount == 0)
                {
                    MessageBox.Show("저장할 자료가 없습니다.", "확인");
                    return;
                }

                Data_Save("수정");
            }
        }

        private void Data_Save(string argSaveGbn)
        {
            if (cboYYMM.Text.Trim() == "") { MessageBox.Show("작업년도가 공란입니다.", "확인요망"); }
            if (VB.Pstr(cboLtd.Text.Trim(), ".", 1) == "") { MessageBox.Show("검색할 회사가 공란입니다.", "확인요망"); }

            if (MessageBox.Show("DB에 저장하시겠습니까?", "확인", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            if (argSaveGbn == "")
            {
                prgBar_Clear(this.prgBar);
                prgBar.Maximum = SS2.ActiveSheet.RowCount;
            }

            //주민번호 점검
            for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
            {
                prgBar.Value = i + 1;
                lblMsg.Text = "(" + (i + 1).To<string>() + "/" + SS2.ActiveSheet.RowCount.To<string>() + ")";

                HEA_EXCEL item = SS1.GetRowData(i) as HEA_EXCEL;

                if (item.CHK == false && item.AES_JUMIN.Length == 13)
                {
                    if (ComFunc.JuminNoCheck(clsDB.DbCon, VB.Left(item.AES_JUMIN, 6), VB.Right(item.AES_JUMIN, 7)) != "")
                    {
                        clsPublic.GstrRetValue = (i + 1).To<string>() + " 번줄 ";
                        clsPublic.GstrRetValue += item.SNAME + " 님 주민번호가 정확하지 않습니다. ";
                        MessageBox.Show(clsPublic.GstrRetValue, "확인");
                        prgBar_Clear(this.prgBar);
                        return;
                    }
                }
            }

            prgBar_Clear(this.prgBar);
            lblMsg.Text = "!! 실제 DB 저장작업 시작 !!";

            //실제저장
            for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
            {
                prgBar.Value = i + 1;
                lblMsg.Text = "(" + (i + 1).To<string>() + "/" + SS2.ActiveSheet.RowCount.To<string>() + ")";

                HEA_EXCEL item = SS1.GetRowData(i) as HEA_EXCEL;

                if (VB.Mid(item.AES_JUMIN, 7, 1) == "1")
                {
                    item.SEX = "M";
                }
                else if (VB.Mid(item.AES_JUMIN, 7, 1) == "2")
                {
                    item.SEX = "F";
                }

                if (item.LTDADDEXAM != "")
                {
                    item.GBLTDADDEXAM = "Y";
                }

                if (item.BIRTH == "")
                {
                    item.BIRTH = VB.Left(item.AES_JUMIN, 6);
                }

                if (item.AES_JUMIN != "")
                {
                    item.AES_JUMIN = clsAES.AES(item.AES_JUMIN);
                }

                if (item.SNAME != "")
                {
                    //신규일경우 검진번호 및 등록번호 검색
                    if (item.RowStatus == ComBase.Mvc.RowStatus.None || item.RowStatus == ComBase.Mvc.RowStatus.Insert)
                    {
                        if (clsAES.DeAES(item.AES_JUMIN).Length == 13)
                        {
                            HIC_PATIENT pat = hicPatientService.GetPanoPtnoByJumin2SName(item.AES_JUMIN, item.SNAME);
                            if (!pat.IsNullOrEmpty())
                            {
                                item.PANO = pat.PANO;
                                item.PTNO = pat.PTNO;
                            }
                        }
                        else if (item.BIRTH.Length == 6)
                        {
                            HIC_PATIENT pat = hicPatientService.GetPanoPtnoByLikeJuminSNameLtdCode(item.BIRTH, item.SNAME, item.LTDCODE);
                            if (!pat.IsNullOrEmpty())
                            {
                                item.PANO = pat.PANO;
                                item.PTNO = pat.PTNO;
                            }
                        }
                    }
                }

                if (item.LTDCODE > 0)
                {
                    item.ENTSABUN = clsType.User.IdNumber.To<long>();
                    item.MODIFIEDUSER = clsType.User.IdNumber;

                    if (!heaExcelService.Save(item))
                    {
                        MessageBox.Show("오류가 발생하였습니다. ");
                        prgBar_Clear(this.prgBar);
                        lblMsg.Text = "DB 저장시 오류발생!! ...";
                        return;
                    }
                }
            }

            SetLtdCombo(cboYYMM.Text);

            MessageBox.Show("저장완료.", "확인");
        }

        private void prgBar_Clear(ProgressBar argBar)
        {
            argBar.Maximum = 0;
            argBar.Minimum = 0;
            argBar.Value = 0;
        }

        /// <summary>
        /// //DB형식으로 변환
        /// </summary>
        private void Convert_ExcelFile_To_DataBaseType()
        {
            if (txtLtdName.Text.IsNullOrEmpty())
            {
                MessageBox.Show("회사코드가 공란입니다.", "확인");
                return;
            }

            if (SS1.ActiveSheet.RowCount < 1)
            {
                MessageBox.Show("엑셀자료가 없습니다.", "확인");
                return;
            }

            FnLtdCode = VB.Pstr(txtLtdName.Text, ".", 1).To<long>();

            string strType = VB.Pstr(cboExcelType.Text, ".", 1);

            switch (strType)
            {
                case "01": Data_Change_01(); break;
                case "02": Data_Change_02(); break;
                case "04": break;  //Data_Change_04()
                case "05": break;  //Data_Change_05()
                case "06": Data_Change_06(); break;
                default: break;
            }

            btnSave.Enabled = true;
        }

        /// <summary>
        /// 표준서식
        /// </summary>
        private void Data_Change_01()
        {
            string strNew = string.Empty;
            string strOLD = string.Empty;

            List<HEA_EXCEL> vList = new List<HEA_EXCEL>();

            for (int i = 0; i < SS1.ActiveSheet.NonEmptyRowCount; i++)
            {
                //SS1 내용을 1 Row Dto 로 담음
                HEA_EXCEL item = new HEA_EXCEL()
                {
                    LTDBUSE      = SS1.ActiveSheet.Cells[i, 0].Text.Trim(),
                    LTDSABUN     = SS1.ActiveSheet.Cells[i, 1].Text.Trim(),
                    JNAME        = SS1.ActiveSheet.Cells[i, 2].Text.Trim(),
                    AES_JUMIN    = SS1.ActiveSheet.Cells[i, 3].Text.Trim().Replace("-", "").Replace(" ", ""),
                    HPHONE       = SS1.ActiveSheet.Cells[i, 4].Text.Trim(),
                    HDATE        = YMD_Conv(SS1.ActiveSheet.Cells[i, 9].Text.Trim()),
                    GJTYPE       = SS1.ActiveSheet.Cells[i, 11].Text.Trim(),
                    HOSPITAL     = SS1.ActiveSheet.Cells[i, 13].Text.Trim(),
                    GBNHIC       = SS1.ActiveSheet.Cells[i, 14].Text.Trim(),
                    MCODES       = SS1.ActiveSheet.Cells[i, 15].Text.Trim(),
                    LTDADDEXAM   = SS1.ActiveSheet.Cells[i, 16].Text.Trim(),
                    BONINADDEXAM = SS1.ActiveSheet.Cells[i, 17].Text.Trim(),
                    REMARK       = SS1.ActiveSheet.Cells[i, 20].Text.Trim(),
                    //JUSO         = SS1.ActiveSheet.Cells[i, 21].Text.Trim(),
                    REL          = "직원"
                };

                if (item.GBNHIC == "○") { item.GBNHIC = "제외"; }

                if (item.JNAME != "")
                {
                    item.AMPM = "오전";
                    if (VB.Right(item.HDATE , 4) == "오후")
                    {
                        item.AMPM = "오후";
                        item.HDATE = VB.Left(item.HDATE, item.HDATE.Length - 4);
                        if (VB.L(item.HDATE, "/") == 2)
                        {
                            item.HDATE = cboYYMM.Text.Trim() + "-" + VB.Format(VB.Pstr(item.HDATE, "/", 1).To<int>(), "00") + "-" + VB.Format(VB.Pstr(item.HDATE, "/", 2).To<int>(), "00"); 
                        }

                        strNew = item.JNAME + "{}" + item.LTDSABUN;

                        if (strNew != strOLD)
                        {
                            strOLD = strNew;
                            vList.Add(item);        //List에 추가
                        }

                        //SS1 내용을 1 Row Dto 로 담음
                        //가족
                        HEA_EXCEL item2 = new HEA_EXCEL()
                        {
                            LTDBUSE      = SS1.ActiveSheet.Cells[i, 0].Text.Trim(),
                            LTDSABUN     = SS1.ActiveSheet.Cells[i, 1].Text.Trim(),
                            JNAME        = SS1.ActiveSheet.Cells[i, 2].Text.Trim(),
                            SNAME        = SS1.ActiveSheet.Cells[i, 5].Text.Trim(),
                            REL          = SS1.ActiveSheet.Cells[i, 6].Text.Trim(),
                            AES_JUMIN    = SS1.ActiveSheet.Cells[i, 7].Text.Trim().Replace("-", "").Replace(" ", ""),
                            HPHONE       = SS1.ActiveSheet.Cells[i, 8].Text.Trim(),
                            HDATE        = YMD_Conv(SS1.ActiveSheet.Cells[i, 10].Text.Trim()),
                            GJTYPE       = SS1.ActiveSheet.Cells[i, 12].Text.Trim(),
                            HOSPITAL     = SS1.ActiveSheet.Cells[i, 13].Text.Trim(),
                            LTDADDEXAM   = SS1.ActiveSheet.Cells[i, 16].Text.Trim(),
                            BONINADDEXAM = SS1.ActiveSheet.Cells[i, 17].Text.Trim(),
                            REMARK       = SS1.ActiveSheet.Cells[i, 20].Text.Trim(),
                            GBNHIC       = "",
                            MCODES       = ""
                        };

                        if (item2.REL == "") { item2.REL = "가족"; }

                        if (item2.SNAME != "")
                        {
                            if (item2.HDATE == "") { item2.HDATE = SS1.ActiveSheet.Cells[i, 11].Text.Trim(); }

                            item2.AMPM = "오전";
                            if (VB.Right(item2.HDATE, 4) == "(오후)")
                            {
                                item2.AMPM = "오후";
                                item2.HDATE = cboYYMM.Text.Trim() + "-" + VB.Format(VB.Pstr(item.HDATE, "/", 1).To<int>(), "00") + "-" + VB.Format(VB.Pstr(item.HDATE, "/", 2).To<int>(), "00");
                            }

                            if (item2.GJTYPE == "") { item2.HDATE = SS1.ActiveSheet.Cells[i, 11].Text.Trim(); }
                        }

                        vList.Add(item2); //List에 추가
                    }
                }
            }

            SS2.DataSource = vList;
            
        }

        /// <summary>
        /// 표준서식2: 영덕,울진군청
        /// </summary>
        private void Data_Change_02()
        {
            string strTemp = string.Empty; 

            List<HEA_EXCEL> vList = new List<HEA_EXCEL>();

            for (int i = 0; i < SS1.ActiveSheet.NonEmptyRowCount; i++)
            {
                //SS1 내용을 1 Row Dto 로 담음
                HEA_EXCEL item = new HEA_EXCEL()
                {
                    LTDBUSE      = SS1.ActiveSheet.Cells[i, 1].Text.Trim(),
                    SNAME        = SS1.ActiveSheet.Cells[i, 2].Text.Trim(),
                    BIRTH        = SS1.ActiveSheet.Cells[i, 3].Text.Trim(),
                    GBNHIC       = SS1.ActiveSheet.Cells[i, 4].Text.Trim(),
                };

                if (item.BIRTH.IndexOf(".") > 0)
                {
                    strTemp = VB.Mid(item.BIRTH, 3, 2) + VB.Format(VB.Pstr(item.BIRTH, ".", 2).To<int>(), "00") + VB.Format(VB.Pstr(item.BIRTH, ".", 3).To<int>(), "00");
                }
                else
                {
                    strTemp = item.BIRTH;
                }

                if (item.SNAME != "" && VB.IsNumeric(item.BIRTH))
                {
                    vList.Add(item);
                }
            }

            SS2.DataSource = vList;
        }

        /// <summary>
        /// 조선내화
        /// </summary>
        private void Data_Change_06()
        {
            string strTemp = string.Empty;

            List<HEA_EXCEL> vList = new List<HEA_EXCEL>();

            for (int i = 0; i < SS1.ActiveSheet.NonEmptyRowCount; i++)
            {
                //SS1 내용을 1 Row Dto 로 담음
                //직원본인
                HEA_EXCEL item = new HEA_EXCEL()
                {
                    REL         = "직원",
                    LTDBUSE     = SS1.ActiveSheet.Cells[i, 1].Text.Trim() + " " + SS1.ActiveSheet.Cells[i, 2].Text.Trim(),
                    JNAME       = SS1.ActiveSheet.Cells[i, 3].Text.Trim(),
                    AES_JUMIN   = SS1.ActiveSheet.Cells[i, 4].Text.Trim().Replace("-", "").Replace(" ", ""),
                    MCODES      = SS1.ActiveSheet.Cells[i, 5].Text.Trim(),
                    LTDADDEXAM  = SS1.ActiveSheet.Cells[i, 6].Text.Trim(),
                    REMARK      = SS1.ActiveSheet.Cells[i, 11].Text.Trim(),
                    HPHONE      = ""
                };

                strTemp = item.LTDADDEXAM;

                if (VB.InStr(strTemp, "흉") > 0) { item.LTDADDEXAM += "저선량흉부CT(폐질환검사),"; }
                if (VB.InStr(strTemp, "갑") > 0) { item.LTDADDEXAM += "갑상선초음파,"; }
                if (VB.InStr(strTemp, "심") > 0) { item.LTDADDEXAM += "심장초음파,"; }
                if (VB.InStr(strTemp, "대") > 0) { item.LTDADDEXAM += "대장내시경,"; }
                if (VB.InStr(strTemp, "유") > 0) { item.LTDADDEXAM += "유방초음파,"; }
                if (VB.InStr(strTemp, "뇌") > 0) { item.LTDADDEXAM += "뇌혈류초음파,"; }
                if (VB.InStr(strTemp, "전") > 0) { item.LTDADDEXAM += "전립선초음파,"; }
                if (VB.Right(item.LTDADDEXAM, 1) == ",")
                {
                    item.LTDADDEXAM = VB.Left(item.LTDADDEXAM, item.LTDADDEXAM.Length - 1);
                }

                if (item.JNAME != "")
                {
                    vList.Add(item);

                    //직원배우자
                    HEA_EXCEL item2 = new HEA_EXCEL()
                    {
                        REL         = "배우자",
                        SNAME       = SS1.ActiveSheet.Cells[i, 7].Text.Trim(),
                        AES_JUMIN   = SS1.ActiveSheet.Cells[i, 8].Text.Trim().Replace("-", "").Replace(" ", ""),
                        HPHONE      = SS1.ActiveSheet.Cells[i, 9].Text.Trim(),
                        LTDADDEXAM  = SS1.ActiveSheet.Cells[i, 10].Text.Trim(),
                        REMARK      = SS1.ActiveSheet.Cells[i, 11].Text.Trim()
                    };

                    strTemp = item2.LTDADDEXAM;

                    if (VB.InStr(strTemp, "흉") > 0) { item2.LTDADDEXAM += "저선량흉부CT(폐질환검사),"; }
                    if (VB.InStr(strTemp, "갑") > 0) { item2.LTDADDEXAM += "갑상선초음파,"; }
                    if (VB.InStr(strTemp, "심") > 0) { item2.LTDADDEXAM += "심장초음파,"; }
                    if (VB.InStr(strTemp, "대") > 0) { item2.LTDADDEXAM += "대장내시경,"; }
                    if (VB.InStr(strTemp, "유") > 0) { item2.LTDADDEXAM += "유방초음파,"; }
                    if (VB.InStr(strTemp, "뇌") > 0) { item2.LTDADDEXAM += "뇌혈류초음파,"; }
                    if (VB.InStr(strTemp, "전") > 0) { item2.LTDADDEXAM += "전립선초음파,"; }
                    if (VB.Right(item2.LTDADDEXAM, 1) == ",")
                    {
                        item2.LTDADDEXAM = VB.Left(item2.LTDADDEXAM, item2.LTDADDEXAM.Length - 1);
                    }

                    vList.Add(item2);
                }
            }

            SS2.DataSource = vList;
        }

        private string YMD_Conv(string argYMD)
        {
            string strDate1 = string.Empty;
            string strDate2 = string.Empty;
            string strDate3 = string.Empty;
            string rtnVal = string.Empty;

            if (argYMD.IndexOf("/") > 0 && VB.Pstr(argYMD, "/", 3).Length == 4)
            {
                strDate1 = VB.Pstr(argYMD, "/", 1);
                strDate2 = VB.Pstr(argYMD, "/", 2);
                strDate3 = VB.Pstr(argYMD, "/", 3);
                rtnVal = strDate3 + "-" + strDate1 + "-" + strDate2;
            }
            else
            {
                rtnVal = argYMD;
            }

            return rtnVal;
        }

        /// <summary>
        /// DB에 저장된 명단 조회
        /// </summary>
        /// <param name="argYear"></param>
        /// <param name="argLtdCode"></param>
        /// <param name="argSName"></param>
        /// <param name="argBirth"></param>
        private void Screen_Display(string argYear, long argLtdCode, string argSName, string argBirth)
        {
            string strJumin = string.Empty;

            List<HEA_EXCEL> list = heaExcelService.GetAll(argYear, argLtdCode, argSName, argBirth, "");
            SS2.DataSource = list;

            if (list.Count > 0)
            {
                for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
                {
                    strJumin = clsAES.DeAES(SS2.ActiveSheet.Cells[i, 5].Text.Trim());
                    SS2.ActiveSheet.Cells[i, 5].Text = strJumin;

                    if (strJumin.Length == 13)
                    {
                        strJumin = VB.Left(strJumin, 6) + "-" + VB.Right(strJumin, 7);
                        SS2.ActiveSheet.Cells[i, 5].Text = strJumin;
                    }
                }
            }

            btnSave2.Enabled = true;
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

            if (!LtdHelpItem.IsNullOrEmpty())
            {
                txtLtdName.Text = LtdHelpItem.CODE.To<string>();
                txtLtdName.Text = txtLtdName.Text + "." + LtdHelpItem.SANGHO;
            }
            else
            {
                txtLtdName.Text = "";
                txtLtdName.Text = "";
            }
        }

        private void ePost_value_LTD(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            prgBar.Minimum = 0;
            prgBar.Value = 0;
            SS2.ActiveSheet.RowCount = 100;
            lblMsg.Text = "작업대기중 ...";

            btnSave.Enabled = false;
            btnSave2.Enabled = false;
        }
    }
}
