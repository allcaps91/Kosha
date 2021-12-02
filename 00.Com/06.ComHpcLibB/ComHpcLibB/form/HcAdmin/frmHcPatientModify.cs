using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComHpcLibB.Dto;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Service;
using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Main.form
/// File Name       : frmHcPatientModify.cs
/// Description     : 수검자 인적사항 수정
/// Author          : 김민철
/// Create Date     : 2019-09-11
/// Update History  : 
/// </summary>
/// <seealso cref= "FrmInjekModify(HcMain17.frm) / FrmInjekModify(HaMain02.frm)" />
namespace ComHpcLibB
{
    public partial class frmHcPatientModify : BaseForm
    {
        clsSpread cSpd                      = null;
        ComFunc CF                          = null;
        HIC_LTD LtdHelpItem                 = null;
        HIC_CODE CodeHelpItem               = null;
        HicPatientService hicPatientService = null;
        HicMemoService    hicMemoService    = null;
        HicCodeService    hicCodeService    = null;
        HicLtdService     hicLtdService     = null;
        HicJepsuService   hicJepsuService   = null;
        HeaJepsuService   heaJepsuService   = null;

        string FstrBuildNo = string.Empty;
        string FstrPtno = string.Empty;
        string FstrPtno1 = string.Empty;
        long FnPano = 0;
        long FnWrtno = 0;
        string FstrCode;
        string FstrName;
        public frmHcPatientModify()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmHcPatientModify(string argPTNO)
        {
            InitializeComponent();
            SetEvent();
            SetControl();

            FstrPtno1 = argPTNO;
        }

        private void SetControl()
        {
            LtdHelpItem         = new HIC_LTD();
            CodeHelpItem        = new HIC_CODE();
            hicPatientService   = new HicPatientService();
            hicMemoService      = new HicMemoService();
            hicCodeService      = new HicCodeService();
            hicLtdService       = new HicLtdService();
            hicJepsuService     = new HicJepsuService();
            heaJepsuService     = new HeaJepsuService();
            cSpd                = new clsSpread();
            CF                  = new ComFunc();

            chkSMS.SetOptions(new CheckBoxOption  { DataField = nameof(HIC_PATIENT.GBSMS),      CheckValue = "Y", UnCheckValue = "N" });
            chkSuchep.SetOptions(new CheckBoxOption { DataField = nameof(HIC_PATIENT.GBSUCHEP), CheckValue = "Y", UnCheckValue = "N" });
            chkForeign.SetOptions(new CheckBoxOption { DataField = nameof(HIC_PATIENT.GBFOREIGNER), CheckValue = "Y", UnCheckValue = "N" });

            #region 수검자 메모 Spread
            ssETC.Initialize();
            ssETC.AddColumn("삭제",       nameof(HIC_MEMO.GBDEL),     34, FpSpreadCellType.CheckBoxCellType);
            ssETC.AddColumn("구분",       nameof(HIC_MEMO.JOBGBN),    42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            ssETC.AddColumn("입력시각",   nameof(HIC_MEMO.ENTTIME),  160, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsEditble = true,   Aligen = CellHorizontalAlignment.Left, BackColor = Color.FromArgb(192, 255, 192) });
            ssETC.AddColumn("내용",       nameof(HIC_MEMO.MEMO),     440, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsEditble = false,  Aligen = CellHorizontalAlignment.Left, IsMulti = true });
            ssETC.AddColumn("작업자사번", nameof(HIC_MEMO.JOBSABUN),  48, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsVisivle = false, IsEditble = true });
            ssETC.AddColumn("작업자명",   nameof(HIC_MEMO.JOBNAME),   90, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsEditble = true });
            ssETC.AddColumn("CHANGE",     "",                         30, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsVisivle = false });
            ssETC.AddColumn("ROWID",      nameof(HIC_MEMO.RID),       30, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsVisivle = false });
            ssETC.AddColumn("PANO",       nameof(HIC_MEMO.PANO),      30, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsVisivle = false });
            ssETC.AddColumn("PTNO",       nameof(HIC_MEMO.PTNO),      30, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsVisivle = false });
            #endregion

            #region 조회 명단 Spread
            SSList.Initialize();
            SSList.AddColumn("등록번호",   nameof(HIC_PATIENT.PTNO),     74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true });
            SSList.AddColumn("수검자명",   nameof(HIC_PATIENT.SNAME),    74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("주민번호",   nameof(HIC_PATIENT.JUMIN),    74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("회사명",     nameof(HIC_PATIENT.LTDNAME), 160, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("접수번호",   nameof(HIC_PATIENT.WRTNO), 80, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });

            #endregion

            dtpIpsaDate.SetOptions(new DateTimePickerOption { DataField = nameof(HIC_PATIENT.IPSADATE), DisplayFormat = DateTimeType.YYYY_MM_DD, DataBaseFormat = DateTimeType.None });
            dtpJenipDate.SetOptions(new DateTimePickerOption { DataField = nameof(HIC_PATIENT.BUSEIPSA), DisplayFormat = DateTimeType.YYYY_MM_DD, DataBaseFormat = DateTimeType.None });
        }

        private void SetEvent()
        {
            this.Load                   += new EventHandler(eFormLoad);
            this.btnExit.Click          += new EventHandler(eBtnClick);
            this.btnVLtdHelp.Click      += new EventHandler(eBtnClick);
            this.btnView.Click          += new EventHandler(eBtnClick);
            this.btnSave.Click          += new EventHandler(eBtnClick);
            this.btnSave_ETC.Click      += new EventHandler(eBtnClick);         //수검자 메모저장
            this.btnHelp_Mail.Click     += new EventHandler(eBtnClick);
            this.btnLtdHelp.Click       += new EventHandler(eBtnClick);
            this.btnJikjong.Click       += new EventHandler(eBtnClick);
            this.btnGongjeng.Click      += new EventHandler(eBtnClick);
            this.btnBogen.Click         += new EventHandler(eBtnClick);
            this.btnJisa.Click          += new EventHandler(eBtnClick);
            this.btnKiho.Click          += new EventHandler(eBtnClick);

            this.txtVPtno.KeyDown       += new KeyEventHandler(eKeyDown);
            this.txtVName.KeyDown       += new KeyEventHandler(eKeyDown);

            this.SSList.CellClick       += new CellClickEventHandler(eCellClick);
            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);
            this.ssETC.EditModeOff      += new EventHandler(eSpdEditOff);
            this.ssETC.ButtonClicked    += new EditorNotifyEventHandler(eSpdBtnClick);
        }

        private void eCellClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader)
            {
                cSpd.setSpdSort(SSList, e.Column, true);
                return;
            }
        }

        private void eKeyDown(object sender, KeyEventArgs e)
        {
            if (sender == txtVPtno)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtVPtno.Text = VB.Format(VB.Val(txtVPtno.Text), "00000000");
                    if (txtVPtno.Text.Trim().Equals("00000000")) { txtVPtno.Text = ""; return; }

                    Search_Data();
                }
            }
            else if (sender == txtVName)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    Search_Data();
                }
            }
        }

        private void eSpdBtnClick(object sender, EditorNotifyEventArgs e)
        {
            if (sender == ssETC)
            {
                if (e.Column == (int)clsHcSpd.enmHcMemo.GBDEL)
                {
                    if (ssETC.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcMemo.GBDEL].Text == "True")
                    {
                        cSpd.setSpdForeColor(ssETC, e.Row, 0, e.Row, ssETC_Sheet1.ColumnCount - 1, Color.Red);

                    }
                    else
                    {
                        if (ssETC.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcMemo.CHANGE].Text == "Y")
                        {
                            cSpd.setSpdForeColor(ssETC, e.Row, 0, e.Row, ssETC_Sheet1.ColumnCount - 1, Color.Blue);
                        }
                        else
                        {
                            cSpd.setSpdForeColor(ssETC, e.Row, 0, e.Row, ssETC_Sheet1.ColumnCount - 1, Color.Black);
                        }
                    }
                }
            }

        }

        private void eSpdEditOff(object sender, EventArgs e)
        {
            int nRow = 0, nCol = 0;

            if (sender == ssETC)
            {
                nRow = ssETC.ActiveSheet.ActiveRowIndex;
                nCol = ssETC.ActiveSheet.ActiveColumnIndex;

                if (nCol == (int)clsHcSpd.enmHcMemo.MEMO)
                {
                    Size size = ssETC.ActiveSheet.GetPreferredCellSize(nRow, nCol);
                    ssETC.ActiveSheet.Rows[nRow].Height = size.Height;

                    if (ssETC.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcMemo.CHANGE].Text == "")
                    {
                        ssETC.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcMemo.CHANGE].Text = "Y";
                        cSpd.setSpdForeColor(ssETC, nRow, 0, nRow, ssETC_Sheet1.ColumnCount - 1, Color.Blue);
                    }
                }
            }
        }

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            if (sender == SSList)
            {
                FnWrtno = Convert.ToInt32(SSList.ActiveSheet.Cells[e.Row, 4].Text);
                panJob.Initialize();
                HIC_PATIENT pat = SSList.GetRowData(e.Row) as HIC_PATIENT;
                Display_PatientInfo(pat.PTNO);  //수검자 인적정보
                Hic_Memo_Screen(pat.PTNO);      //수검자 메모
            }
        }

        private void Display_PatientInfo(string pTNO)
        {
            HIC_PATIENT pat = hicPatientService.GetPatInfoByPtno(pTNO);

            panJob.SetData(pat);

            string strJumin = clsAES.DeAES(pat.JUMIN2);

            txtJumin1.Text = VB.Left(strJumin, 6);
            txtJumin2.Text = VB.Right(strJumin, 7);

            //lblLtdName.Text  = hicLtdService.READ_Ltd_One_Name(txtLtdName.Text.Trim());              //회사명
            txtLtdName.Text  = pat.LTDCODE + "." + pat.LTDNAME;                                      //회사명
            lblJisaName.Text = hicCodeService.GetNameByGubunCode("21", txtJisa.Text.Trim());         //지사
            lblJikjong.Text  = hicCodeService.GetNameByGubunCode("05", txtJikJong.Text.Trim());      //직종
            lblGongjeng.Text = hicCodeService.GetNameByGubunCode("A2", txtGongjeng.Text.Trim());     //공정명
            lblKihoName.Text = hicCodeService.GetNameByGubunCode("18", txtKiho.Text.Trim());         //회사기호
            lblBoName.Text   = hicCodeService.GetNameByGubunCode("25", txtBoKiho.Text.Trim());       //보건소
            
            FstrBuildNo = pat.BUILDNO;

            //개인정보동의
            chkPrvAgr.Checked = pat.GBPRIVACY != null ? true : false;
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnVLtdHelp)
            {
                Ltd_Code_Help(txtVLtdCode);
            }
            else if (sender == btnView)
            {
                Search_Data();
            }
            else if (sender == btnSave_ETC)
            {
                Hic_Memo_Save();
            }
            #region 우편번호
            else if (sender == btnHelp_Mail)
            {
                Post_Code_Help();
            }
            #endregion
            #region 사업장코드찾기
            else if (sender == btnLtdHelp)
            {
                Ltd_Code_Help(txtLtdName);
            }
            #endregion
            #region 현직종 찾기
            else if (sender == btnJikjong)
            {
                Hic_Code_Help("05", txtJikJong, lblJikjong); //직종
            }
            #endregion
            #region 현공정 찾기
            else if (sender == btnGongjeng)
            {
                Hic_Code_Help("A2", txtGongjeng, lblGongjeng); //공정
            }
            #endregion
            #region 보건소기호 찾기
            else if (sender == btnBogen)
            {
                Hic_Code_Help("25", txtBoKiho, lblBoName); //보건소
            }
            #endregion
            #region 지사코드 찾기
            else if (sender == btnJisa)
            {
                Hic_Code_Help("21", txtJisa, lblJisaName); //지사
            }
            #endregion
            #region 회사기호 찾기
            else if (sender == btnKiho)
            {
                Hic_Code_Help("18", txtKiho, lblKihoName); //회사기호
            }
            #endregion
            else if (sender == btnSave)
            {
                Save_Data();
            }
        }

        private void Save_Data()
        {
            HIC_PATIENT item = panJob.GetData<HIC_PATIENT>();
            item.REMARK = item.PAT_REMARK;

            if (chkPrvAgr.Checked)
            {
                item.GBPRIVACY = DateTime.Now.ToShortDateString();
            }
            else
            {
                item.GBPRIVACY = "";
            }

            string strNum = VB.Left(item.CT_JUMIN2, 1);

            item.SEX = "M";

            if (strNum.Equals("1") || strNum.Equals("3") || strNum.Equals("5") || strNum.Equals("7"))
            {
                item.SEX = "M";
            }
            else if (strNum.Equals("2") || strNum.Equals("4") || strNum.Equals("6") || strNum.Equals("8"))
            {
                item.SEX = "F";
            }

            item.JUMIN = item.CT_JUMIN1 + VB.Left(item.CT_JUMIN2, 1) + "******";
            item.JUMIN2 = clsAES.AES(item.CT_JUMIN1 + item.CT_JUMIN2);
            item.BUILDNO = FstrBuildNo;
            item.LTDCODE = VB.Pstr(txtLtdName.Text, ".", 1).To<long>();
            item.HPHONE = txtHphone.Text;

            if (!panJob.RequiredValidate())
            {
                MessageBox.Show("필수 입력항목이 누락되었습니다.");
                return;
            }


            int result = hicPatientService.UpDate(item);
            if (result <= 0)
            {
                MessageBox.Show("저장실패!");
            }
            
            if (rdoBuse2.Checked == true)
            {

                HEA_JEPSU item1 = new HEA_JEPSU
                {
                    WRTNO = FnWrtno,
                    MAILCODE = txtMail.Text.Trim(),
                    JUSO1 = txtJuso1.Text.Trim(),
                    JUSO2 = txtJuso2.Text.Trim(),
                };

                if (!heaJepsuService.UpDateJusoMailCodeByItem(item1))
                {
                    MessageBox.Show("결과지 주소 UPDATE시 오류발생", "오류");
                    return;
                }
            }

            //접수이름 UPDATE
            //int result1= hicJepsuService.UpdateSnamebyWrtNo(FnWrtno, item.SNAME);
            //if (result1 <= 0)
            //{
            //    MessageBox.Show("접수 수검자명 UPDATE오류!");
            //}

            //WebSEND UPDATE오류
            //int result2 = hicPatientService.UpdateWebSendByPtno(item.PTNO);
            //if (result2 <= 0)
            //{
            //    MessageBox.Show("웹서버 UPDATE오류!");
            //}

            Screen_Clear();
        }

        /// <summary>
        /// 검진 기초코드 검색창 연동
        /// </summary>
        /// <param name="strGB"></param>
        /// <param name="tx"></param>
        /// <param name="lb"></param>
        private void Hic_Code_Help(string strGB, TextBox tx, Label lb)
        {
            frmHcCodeHelp frm = new frmHcCodeHelp(strGB);
            frm.rSetGstrValue += new frmHcCodeHelp.SetGstrValue(ePost_value_CODE);
            frm.ShowDialog();

            //if (!CodeHelpItem.CODE.IsNullOrEmpty())
            if (!FstrCode.IsNullOrEmpty())
            {
                //tx.Text = CodeHelpItem.CODE.Trim();
                //lb.Text = CodeHelpItem.NAME.Trim();
                tx.Text = FstrCode.Trim();
                lb.Text = FstrName.Trim();
            }
            else
            {
                tx.Text = "";
                lb.Text = "";
            }
        }

        //private void ePost_value_CODE(HIC_CODE item)
        private void ePost_value_CODE(string strCode, string strName)
        {
            //CodeHelpItem = item;
            FstrCode = strCode;
            FstrName = strName;
        }

        /// <summary>
        /// 도로명 주소 검색창 연동
        /// </summary>
        private void Post_Code_Help()
        {
            clsHcVariable.GstrValue = "";
            clsPublic.GstrMsgList = "";

            frmSearchRoadWeb frm = new frmSearchRoadWeb();
            frm.rSetGstrValue += new frmSearchRoadWeb.SetGstrValue(ePost_value);
            frm.ShowDialog();

            if (!clsHcVariable.GstrValue.IsNullOrEmpty())
            {
                txtMail.Text = VB.Left(VB.Pstr(clsHcVariable.GstrValue, "|", 1), 3);
                txtMail.Text += VB.Mid(VB.Pstr(clsHcVariable.GstrValue, "|", 1), 4, 2);
                txtJuso1.Text = VB.Pstr(clsHcVariable.GstrValue, "|", 2).Trim();
                txtJuso2.Text = "";

                FstrBuildNo = VB.Pstr(clsHcVariable.GstrValue, "|", 5).Trim();
                txtJuso2.Focus();
            }
            else
            {
                FstrBuildNo = "";
                txtJuso2.Focus();
            }
        }

        private void ePost_value(string GstrValue)
        {
            clsHcVariable.GstrValue = GstrValue;
        }

        /// <summary>
        /// 수검자 메모 저장 및 삭제
        /// </summary>
        /// <param name="item"></param>
        private void Hic_Memo_Save()
        {
            int result = 0;

            for (int i = 0; i < ssETC.ActiveSheet.RowCount; i++)
            {
                if (ssETC.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcMemo.GBDEL].Text == "True")
                {
                    //Delete Data
                    string strGbn = ssETC.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcMemo.JOBGBN].Text;
                    string strRid = ssETC.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcMemo.ROWID].Text;

                    result = hicMemoService.DeleteData(ssETC.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcMemo.ROWID].Text, strGbn);
                }
                else if (ssETC.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcMemo.ROWID].Text == "")
                {
                    if (ssETC.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcMemo.MEMO].Text != "")
                    {
                        //Insert Data
                        HIC_MEMO item = new HIC_MEMO
                        {
                            MEMO = ssETC.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcMemo.MEMO].Text,
                            JOBSABUN = clsType.User.IdNumber.To<long>(),
                            PTNO = FstrPtno,
                            PANO = FnPano,
                            JOBGBN = rdoJob1.Checked ? "일반" : "종검"
                        };

                        result = hicMemoService.Insert(item);
                    }
                }
            }

            if (result <= 0)
            {
                MessageBox.Show("수검자 메모 저장 오류.", "ERROR");
                return;
            }

            ssETC.ActiveSheet.ClearRange(0, 0, ssETC.ActiveSheet.Rows.Count, ssETC.ActiveSheet.ColumnCount, true);
            ssETC.ActiveSheet.Rows.Count = 0;

            Hic_Memo_Screen(FstrPtno);
        }

        private void Search_Data()
        {
            string strSName = txtVName.Text.Trim();
            string strPtno = txtVPtno.Text;
            long nLtdCode = txtVLtdCode.Text.To<long>();
            string strFDate = dtpFDate.Text;
            string strTDate = dtpTDate.Text;

            //if (strSName.IsNullOrEmpty() && strPtno.IsNullOrEmpty() && nLtdCode == 0)
            //{
            //    MessageBox.Show("검색조건을 입력하세요.");
            //    return;
            //}

            if ( rdoBuse1.Checked == true)
            {
                SSList.DataSource = hicPatientService.GetHicListByItem(strSName, strPtno, nLtdCode, strFDate, strTDate);
            }
            else if (rdoBuse2.Checked == true)
            {
                SSList.DataSource = hicPatientService.GetHeaListByItem(strSName, strPtno, nLtdCode, strFDate, strTDate);
            }
        }


        /// <summary>
        /// 사업장 코드 검색창 연동
        /// </summary>
        private void Ltd_Code_Help(TextBox tx)
        {
            string strFind = "";

            if (tx.Text.Contains("."))
            {
                strFind = VB.Pstr(tx.Text, ".", 2).Trim();
            }
            else
            {
                strFind = tx.Text.Trim();
            }

            frmHcLtdHelp frm = new frmHcLtdHelp(strFind);
            frm.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(ePost_value_LTD);
            frm.ShowDialog();

            if (LtdHelpItem.CODE > 0 && !LtdHelpItem.IsNullOrEmpty())
            {
                tx.Text = LtdHelpItem.CODE.To<string>();
                tx.Text += "." + LtdHelpItem.SANGHO;
            }
            else
            {
                tx.Text = "";
            }
        }

        private void ePost_value_LTD(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            panJob.AddRequiredControl(txtPtno);
            panJob.AddRequiredControl(txtSName);
            panJob.AddRequiredControl(txtJumin1);
            panJob.AddRequiredControl(txtJumin2);
            panJob.AddRequiredControl(txtMail);

            Screen_Clear();

            if (!FstrPtno1.IsNullOrEmpty())
            {
                rdoBuse2.Checked = true;
                txtVPtno.Text = FstrPtno1;
                eBtnClick(btnView, new EventArgs());
                eSpdDblClick(SSList, new CellClickEventArgs(new SpreadView(), 0, 0, 0, 0, MouseButtons.Left, false, false));
            }
        }

        private void Screen_Clear()
        {
            panMain.Initialize();
            FstrPtno = "";
            FnPano = 0;
            FstrBuildNo = "";

            txtLtdName.Text = "";
            lblJisaName.Text = "";
            lblJikjong.Text = "";
            lblGongjeng.Text = "";
            lblKihoName.Text = "";
            lblBoName.Text = "";
            txtVLtdCode.Text = "";

            ssETC.ActiveSheet.ClearRange(0, 0, ssETC.ActiveSheet.Rows.Count, ssETC.ActiveSheet.ColumnCount, true);
            ssETC.ActiveSheet.Rows.Count = 5;

            dtpIpsaDate.Text = "";
            dtpJenipDate.Text = "";

            dtpFDate.Text = DateTime.Now.AddDays(-30).ToShortDateString();
            dtpTDate.Text = DateTime.Now.ToShortDateString();

            rdoBuse1.Checked = true;
        }

        /// <summary>
        /// 수검자 메모 Display
        /// </summary>
        /// <param name="argPtno"></param>
        private void Hic_Memo_Screen(string argPtno)
        {
            List<HIC_MEMO> list = hicMemoService.GetItembyPaNo(argPtno, "");

            ssETC.DataSource = list;

            if (!list.IsNullOrEmpty() && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    Size size = ssETC.ActiveSheet.GetPreferredCellSize(i, 2);
                    ssETC.ActiveSheet.Rows[i].Height = size.Height;
                    //ssETC.ActiveSheet.Cells[i, 4].Text = CF.Read_SabunName(clsDB.DbCon, list[i].JOBSABUN.To<string>());
                }

                ssETC.AddRows(5);
            }

        }
    }
}
