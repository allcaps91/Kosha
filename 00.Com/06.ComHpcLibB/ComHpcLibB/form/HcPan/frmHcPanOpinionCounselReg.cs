using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcPanOpinionCounselReg.cs
/// Description     : 유소견자(D1,D2) 유소견자 상담 등록
/// Author          : 이상훈
/// Create Date     : 2019-10-23
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm유소견자상담등록.frm(Frm유소견자상담등록)" />

namespace ComHpcLibB
{
    public partial class frmHcPanOpinionCounselReg : Form
    {
        HicBcodeService hicBcodeService = null;
        HicJepsuLtdResBohum1Service hicJepsuLtdResBohum1Service = null;
        HicResultExCodeService hicResultExCodeService = null;
        HicResSahusangdamService hicResSahusangdamService = null;
        HicJepsuService hicJepsuService = null;
        HicPatientService hicPatientService = null;
        HicSpcPanjengService hicSpcPanjengService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        frmHcLtdHelp FrmHcLtdHelp = null;
        frmHcSangTotalCounsel FrmHcSangTotalCounsel = null;

        HIC_LTD LtdHelpItem = null;

        string FstrROWID;
        long FnWRTNO;
        string FstrJepDate;
        string FstrSogen;
        string FstrPanjengGbn;

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

        public frmHcPanOpinionCounselReg(long nWrtNo)
        {
            InitializeComponent();
            FnWRTNO = nWrtNo;
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicBcodeService = new HicBcodeService();
            hicJepsuLtdResBohum1Service = new HicJepsuLtdResBohum1Service();
            hicResultExCodeService = new HicResultExCodeService();
            hicResSahusangdamService = new HicResSahusangdamService();
            hicJepsuService = new HicJepsuService();
            hicPatientService = new HicPatientService();
            hicSpcPanjengService = new HicSpcPanjengService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnAdd.Click += new EventHandler(eBtnClick);
            this.btnDel.Click += new EventHandler(eBtnClick);
            this.btnDelete.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnSelect.Click += new EventHandler(eBtnClick);
            this.btnSangView.Click += new EventHandler(eBtnClick);
            this.btnSangClose.Click += new EventHandler(eBtnClick);
            this.btnSangDamView.Click += new EventHandler(eBtnClick);
            this.btnMemo.Click += new EventHandler(eBtnClick);
            this.SS2.Change += new ChangeEventHandler(eSpdChange);

            //this.txtSabun.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.SS2.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SS2.ButtonClicked += new EditorNotifyEventHandler(eSpdBtnClick);
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            txtWrtNo.Text = "";
            dtpDate.Text = clsPublic.GstrSysDate;
            txtRemark.Text = "";
            FstrROWID = "";

            SS3.ActiveSheet.Columns[7].Visible = false; //결과값코드
            SS3.ActiveSheet.Columns[8].Visible = false; //변경
            SS3.ActiveSheet.Columns[9].Visible = false; //ROWID
            SS3.ActiveSheet.Columns[10].Visible = false; //Result Type

            SS3.ActiveSheet.Columns[7].Visible = false; //결과값코드
            SS3.ActiveSheet.Columns[8].Visible = false; //변경
            SS3.ActiveSheet.Columns[9].Visible = false; //ROWID

            fn_Display_UseCode();   //상담 상용구 표시

            txtWrtNo.Text = FnWRTNO.ToString();

            if (FnWRTNO > 0)
            {
                fn_Screen_Dispaly();
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnAdd)
            {
                int nMaxCode = 0;
                int nCode = 0;
                string strCODE = "";
                string strName = "";
                string strGubun = "";
                string strSort = "";

                //strName = txtRemark.Text.Replace("'", "`");
                strGubun = "HIC_D1,D2상담상용구";


                for ( int i =0; i < SS2.ActiveSheet.RowCount; i++)
                {
                    if (SS2.ActiveSheet.Cells[i,4].Text == "Y")
                    {

                        strName = SS2.ActiveSheet.Cells[i, 1].Text;
                        strSort = SS2.ActiveSheet.Cells[i, 2].Text;
                        

                        if (SS2.ActiveSheet.Cells[i, 2].Text.IsNullOrEmpty())
                        {
                            nMaxCode = hicBcodeService.GetMaxCodebyGubun(strGubun);
                            int result = hicBcodeService.InsertCode(strGubun, nMaxCode, strName);

                            if (result < 0)
                            {
                                MessageBox.Show("등록 오류", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            MessageBox.Show("등록 완료", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        else
                        {
                            nCode = Convert.ToInt32(SS2.ActiveSheet.Cells[i, 3].Text);
                            int result = hicBcodeService.UpdateCode(strGubun, nCode, strName, strSort);
                            if (result < 0)
                            {
                                MessageBox.Show("업데이트 오류", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            MessageBox.Show("업데이트 완료", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }


                //if (hicBcodeService.GetCountbyName(strName, strGubun) > 0)
                //{
                //    MessageBox.Show("동일한 상담 상용구가 등록되어 있습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}

                //nMaxCode = hicBcodeService.GetMaxCodebyGubun(strGubun);

                //int result = hicBcodeService.InsertCode(strGubun, nMaxCode, strName);

                //if (result < 0)
                //{
                //    MessageBox.Show("등록 오류", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}

                //MessageBox.Show("등록 완료", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);

                fn_Display_UseCode();
            }
            else if (sender == btnDel)
            {
                string strChk = "";
                string strCode = "";
                string strGubun = "";
                int result = 0;

                strGubun = "HIC_D1,D2상담상용구";

                if (MessageBox.Show("선택한 상담 상용구를 삭제 하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
                {
                    strChk = SS2.ActiveSheet.Cells[i, 0].Text.Trim();
                    if (strChk == "True")
                    {
                        strCode = SS2.ActiveSheet.Cells[i, 3].Text.Trim();

                        result = hicBcodeService.DeletebyCode(strGubun, strCode);

                        if (result < 0)
                        {
                            MessageBox.Show("상담상용구 삭제중 오류 발생!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                MessageBox.Show("삭제 완료!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);

                fn_Display_UseCode();   //상담 상용구를 다시 표시함
            }
            else if (sender == btnDelete)
            {
                int result = 0;

                if (MessageBox.Show("정말로 삭제를 하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                result = hicResSahusangdamService.DeletebyRowId(FstrROWID);

                if (result < 0)
                {
                    MessageBox.Show("삭제 완료", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("삭제 오류", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (sender == btnSave)
            {
                long nWRTNO = 0;
                string strGbn = "";
                string strRemark = "";
                int result = 0;

                if (dtpDate.Text == "")
                {
                    MessageBox.Show("상담일자가 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (txtRemark.Text == "")
                {
                    MessageBox.Show("상담내역이 공란입니다..", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                strRemark = txtRemark.Text;
                strRemark = strRemark.Replace("'", "`");

                if (rdoGbn1.Checked == true)
                {
                    strGbn = "1";
                }
                else if (rdoGbn2.Checked == true)
                {
                    strGbn = "2";
                }
                else if (rdoGbn3.Checked == true)
                {
                    strGbn = "3";
                }
                else
                {
                    strGbn = "4";
                }

                if (FstrROWID == "")
                {
                    result = hicResSahusangdamService.Insert(FnWRTNO, FstrJepDate, dtpDate.Text, clsType.User.IdNumber, FstrSogen, FstrPanjengGbn, strGbn, strRemark);

                    if (result < 0)
                    {
                        MessageBox.Show("저장 오류", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    this.Close();
                }
                else
                {
                    result = hicResSahusangdamService.Update(FstrJepDate, dtpDate.Text, FstrSogen, FstrPanjengGbn, strGbn, clsType.User.IdNumber, strRemark, FstrROWID);

                    if (result < 0)
                    {
                        MessageBox.Show("저장 오류", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    this.Close();
                }
            }
            else if (sender == btnSelect)
            {
                string strRemark = "";

                for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
                {
                    if (SS2.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        strRemark += SS2.ActiveSheet.Cells[i, 1].Text.Trim() + ", ";
                        SS2.ActiveSheet.Cells[i, 0].Text = "";
                        SS2.ActiveSheet.Cells[i, 0, i, 2].ForeColor = Color.Black;
                    }
                }

                if (strRemark != "")
                {
                    strRemark = VB.Left(strRemark, strRemark.Length - 1);
                    txtRemark.Text = strRemark;
                }
            }
            else if (sender == btnSangView)
            {
                lblSang.Visible = true;
            }
            else if (sender == btnSangClose)
            {
                lblSang.Visible = false;
            }
            else if ( sender == btnSangDamView)
            {
                Form frm = hf.OpenForm_Check_Return("frmHcSangTotalCounsel");
                if (!frm.IsNullOrEmpty())
                {
                    frm.Dispose();
                }

                FrmHcSangTotalCounsel = new frmHcSangTotalCounsel(FnWRTNO, true);
                FrmHcSangTotalCounsel.StartPosition = FormStartPosition.CenterScreen;
                FrmHcSangTotalCounsel.Show();
            }
            else if (sender == btnMemo)
            {
                //통합메모관리
                HIC_JEPSU item = hicJepsuService.GetItemByWRTNO(FnWRTNO);

                frmHcMemo frm = new frmHcMemo(item.PTNO);
                frm.ShowDialog();
            }
        }

        /// <summary>
        /// 상담 상용구를 다시 표시함
        /// </summary>
        void fn_Display_UseCode()
        {
            int nREAD = 0;
            int nRow = 0;
            string strGubun = "";

            strGubun = "HIC_D1,D2상담상용구";

            List<HIC_BCODE> list = hicBcodeService.GetCodeNamebyGubun(strGubun);

            nREAD = list.Count;
            SS2.ActiveSheet.RowCount = nREAD+3;
            //SS2_Sheet1.Rows.Get(-1).Height = 24;

            for (int i = 0; i < nREAD; i++)
            {
                if (!list[i].NAME.IsNullOrEmpty())
                {
                    nRow += 1;
                    //if (nRow > SS2.ActiveSheet.RowCount)
                    //{
                        //SS2.ActiveSheet.RowCount = nRow;
                        SS2.ActiveSheet.Cells[i, 0].Text = "";
                        SS2.ActiveSheet.Cells[i, 1].Text = list[i].NAME;
                        SS2.ActiveSheet.Cells[i, 2].Text = list[i].SORT.ToString();
                        SS2.ActiveSheet.Cells[i, 3].Text = list[i].CODE;
                    //}
                    //Row 높이 설정 2020-09-23 
                    FarPoint.Win.Spread.Row row;
                    row = SS2.ActiveSheet.Rows[i];
                    float rowSize = row.GetPreferredHeight();
                    row.Height = rowSize;
                }
            }
        }

        //void eKeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (sender == txtSabun)
        //    {
        //        if (e.KeyChar == (char)13)
        //        {

        //        }
        //    }
        //}

        void eSpdChange(object sender, ChangeEventArgs e)
        {
            if (sender == SS2)
            {
                SS2.ActiveSheet.Cells[e.Row, 4].Text = "Y";
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS2)
            {
                string strRemark = "";

                if (e.ColumnHeader == false && e.RowHeader == false)
                {
                    strRemark = SS2.ActiveSheet.Cells[e.Row, 1].Text.Trim();
                    if (VB.InStr(txtRemark.Text, strRemark) == 0)
                    {
                        if (txtRemark.Text.Trim() == "")
                        {
                            txtRemark.Text = strRemark + ComNum.VBLF;
                        }
                        else
                        {
                            txtRemark.Text += strRemark + ComNum.VBLF;
                        }
                    }
                }
            }
        }

        void eSpdBtnClick(object sender, EditorNotifyEventArgs e)
        {
            if (sender == SS2)
            {
                if (SS2.ActiveSheet.Cells[e.Row, 0].Text == "True")
                {
                    SS2.ActiveSheet.Cells[e.Row, 0, e.Row, 2].ForeColor = Color.Blue;
                }
                else
                {
                    SS2.ActiveSheet.Cells[e.Row, 0, e.Row, 2].ForeColor = Color.Black;
                }
            }
        }

        void fn_OLD_Result_Display(long argPano, string argJepDate, string argSex)
        {
            // 검사항목을 Setting
            strExamCode.Clear();

            for (int i = 0; i < SS3.ActiveSheet.RowCount; i++)
            {
                if (SS3.ActiveSheet.Cells[i, 7].Text.Trim() != "")
                {
                    //strExamCode = strExamCode + "'" + SS3.ActiveSheet.Cells[i, 7].Text.Trim() + "',";
                    strExamCode.Add(SS3.ActiveSheet.Cells[i, 8].Text.Trim());
                }
            }

            //마지막의 컴마를 제거
            //if (strExamCode != "")
            //{
            //    strExamCode = VB.Left(strExamCode, strExamCode.Length - 1);
            //}

            //1차검사 종전 접수번호를 읽음
            List<HIC_JEPSU_LTD_RES_BOHUM1> list = hicJepsuLtdResBohum1Service.GetItembyPaNoJepDateGjYear(argPano, argJepDate, VB.Left(argJepDate, 4));

            nOldCNT = list.Count;
            strAllWRTNO = "";

            for (int i = 0; i < nOldCNT; i++)
            {
                strAllWRTNO += list[i].WRTNO.ToString() + ",";
                strJepDate = list[i].JEPDATE.ToString();
                SS3_Sheet1.ColumnHeader.Cells.Get(0, i + 5).Value = VB.Left(strJepDate, 4) + VB.Mid(strJepDate, 6, 2) + VB.Right(strJepDate, 2);
                fn_OLD_Result_Display_SUB(list[i].WRTNO, strExamCode, argSex, i);
                if (i >= 2) break;
            }

            return;
        }

        /// <summary>
        /// 종전 1회 검사의 결과를 Display
        /// </summary>
        void fn_OLD_Result_Display_SUB(long nWrtNo, List<string> argExamCode, string argSex, int nRowNum)
        {
            //판정결과를 strRemark에 보관
            strOldJepsuDate = hm.GET_HIC_JepsuDate(nWrtNo);

            //검사항목 및 결과를 READ
            List<HIC_RESULT_EXCODE> list = hicResultExCodeService.GetItembyWrtNoNewExCode(nWrtNo, argExamCode, "N");

            nREAD = list.Count;
            
            for (int i = 0; i < nREAD; i++)
            {
                strExCode = list[i].EXCODE.Trim();                 //검사코드
                strResult = list[i].RESULT;                 //검사실 결과값
                strResCode = list[i].RESCODE;               //결과값 코드
                strResultType = list[i].RESULTTYPE;         //결과값 TYPE
                strGbCodeUse = list[i].GBCODEUSE;           //결과값코드 사용여부

                //해당검사코드가 있는 Row를 찾음
                nRow = 0;
                for (int j = 0; j < SS3.ActiveSheet.RowCount; j++)
                {
                    if (SS3.ActiveSheet.Cells[j, 8].Text.Trim() == strExCode)
                    {
                        nRow = j;
                        break;
                    }
                }

                //해당검사가 시트에 있으면 결과를 표시함
                if (nRow >= 0)
                {
                    SS3.ActiveSheet.Cells[nRow, nRowNum + 5].Text = strResult;
                    if (strGbCodeUse == "Y")
                    {
                        if (strResult != "")
                        {
                            strResName = hb.READ_ResultName(strResCode, strResult);
                            SS3.ActiveSheet.Cells[nRow, nRowNum + 5].Text = strResName;
                            if (!strResName.IsNullOrEmpty())
                            {
                                if (strResName.Length > 7)
                                {
                                    strRemark += "▷" + list[i].HNAME + ": ";
                                    strRemark += strResName + "\r\n";
                                }
                            } 
                        }
                        else if (strResult.Length > 7)
                        {
                            strResult += "▷" + list[nRowNum].HNAME + ": ";
                            strRemark += strResult + "\r\n";
                        }
                        SS3.ActiveSheet.Cells[nRow, nRowNum + 10].Text = strResult;   //정상값 점검용
                        strExPan = hm.ExCode_Result_Panjeng(strExCode, strResult, argSex, FstrJepDate, "");
                        //판정결과별 바탕색상을 다르게 표시함
                        switch (strExPan)
                        {
                            case "B":
                                SS3.ActiveSheet.Cells[nRow, nRowNum + 5].BackColor = Color.FromArgb(250, 210, 222);   //정상B
                                break;
                            case "R":
                                SS3.ActiveSheet.Cells[nRow, nRowNum + 5].BackColor = Color.FromArgb(250, 170, 170);   //질환의심(R)
                                break;
                            default:
                                SS3.ActiveSheet.Cells[nRow, nRowNum + 5].BackColor = Color.FromArgb(190, 250, 220);   //정상A 또는 기타
                                break;
                        }
                    }
                }
            }
        }

        void fn_Screen_Dispaly()
        {
            int nREAD = 0;
            int nRow = 0;
            long nD1 = 0;
            long nD2 = 0;
            long nDN = 0;
            long nPano = 0;
            string strExCode = "";
            string strResult = "";
            string strResCode = "";
            string strResultType = "";
            string strGbCodeUse = "";
            string strExName = "";
            string strJepDate = "";
            string strSex = "";
            long nAge = 0;
            string strNomal = "";
            string strRemark = "";
            string strResName = "";
            string strExPan = "";
            string strGjYear = "";
            string strChasu = "";
            FnWRTNO = long.Parse(txtWrtNo.Text);

            //인적사항을 READ
            HIC_JEPSU list = hicJepsuService.GetItembyWrtNo(FnWRTNO);

            if (list == null)
            {
                MessageBox.Show("접수번호가 등록 안됨", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            nPano = list.PANO;
            FstrJepDate = list.JEPDATE.ToString();
            strJepDate = FstrJepDate;
            strSex = list.SEX;
            nAge = list.AGE;
            strGjYear = list.GJYEAR;

            ssPatInfo.ActiveSheet.Cells[0, 0].Text = list.SNAME;
            ssPatInfo.ActiveSheet.Cells[0, 1].Text = list.AGE.ToString() + "/" + list.SEX;
            ssPatInfo.ActiveSheet.Cells[0, 2].Text = cf.Read_Ltd_Name(clsDB.DbCon, list.LTDCODE.ToString());
            ssPatInfo.ActiveSheet.Cells[0, 3].Text = FstrJepDate;
            ssPatInfo.ActiveSheet.Cells[0, 4].Text = hb.READ_GjJong_Name(list.GJJONG);  //건진유형

            //전화번호 찾기
            HIC_PATIENT list2 = hicPatientService.GetHphoneTelbyPano(nPano);

            if (list2.HPHONE != "" && list2.HPHONE != null)
            {
                ssPatInfo.ActiveSheet.Cells[0, 5].Text = list2.HPHONE;
            }
            else if (list2.TEL != "")
            {
                ssPatInfo.ActiveSheet.Cells[0, 5].Text = list2.TEL;
            }
            else
            {
                ssPatInfo.ActiveSheet.Cells[0, 5].Text = "";
            }

            //판정항목(취급물질)을 Display
            List<HIC_SPC_PANJENG> list3 = hicSpcPanjengService.GetPanjengbyWrtNo(FnWRTNO);

            nREAD = list3.Count;
            SSPan.ActiveSheet.RowCount = nREAD;
            FstrSogen = "";
            nD1 = 0;
            nD2 = 0;
            for (int i = 0; i < nREAD; i++)
            {
                SSPan.ActiveSheet.Cells[i, 0].Text = " " + hb.READ_MCode_Name(list3[i].MCODE);
                switch (list3[i].PANJENG)
                {
                    case "5":
                        SSPan.ActiveSheet.Cells[i, 1].Text = "D1";
                        nD1 += 1;
                        break;
                    case "6":
                        SSPan.ActiveSheet.Cells[i, 1].Text = "D2";
                        nD2 += 1;
                        break;
                    case "A":
                        SSPan.ActiveSheet.Cells[i, 1].Text = "DN";
                        nDN += 1;
                        break;
                    default:
                        break;
                }
                SSPan.ActiveSheet.Cells[i, 2].Text = list3[i].SOGENREMARK;
                SSPan.ActiveSheet.Cells[i, 3].Text = list3[i].JOCHIREMARK;
                //SSPan.ActiveSheet.Cells[i, 4].Text = list3[i].WRTNO.ToString();
                SSPan.ActiveSheet.Cells[i, 4].Text = fn_GET_Second_Wrtno(FnWRTNO);

                //소견문구 보관
                if (!list3[i].SOGENREMARK.IsNullOrEmpty())
                {
                    if (VB.InStr(FstrSogen, list3[i].SOGENREMARK) == 0)
                    {
                        FstrSogen += list3[i].SOGENREMARK + "\r\n";
                    }
                }
            }

            FstrPanjengGbn = "";
            if (nD1 > 0) FstrPanjengGbn += "D1,";
            if (nD2 > 0) FstrPanjengGbn += "D2,";
            if (nDN > 0) FstrPanjengGbn += "DN,";
            if (FstrPanjengGbn != "")
            {
                FstrPanjengGbn = VB.Left(FstrPanjengGbn, FstrPanjengGbn.Length - 1);
            }

            if (FstrPanjengGbn == "")
            {
                FstrPanjengGbn = "";
                FstrROWID = "";
                dtpDate.Text = clsPublic.GstrSysDate;
                rdoGbn1.Checked = true;
                txtRemark.Text = "";
                btnSave.Enabled = false;
                btnDelete.Enabled = false;

                MessageBox.Show("유소견자 상담 대상자가 아닙니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //상담내역이 있으면 표시함
            HIC_RES_SAHUSANGDAM list4 = hicResSahusangdamService.GetItembyWrtNo(FnWRTNO);

            if (list4 == null)
            {
                FstrROWID = "";
                dtpDate.Text = clsPublic.GstrSysDate;
                rdoGbn1.Checked = true;
                //txtSabun.Text = cf.Read_SabunName(clsDB.DbCon, clsType.User.IdNumber);
                txtRemark.Text = "";
                btnSave.Enabled = true;
                btnDelete.Enabled = false;
            }
            else
            {
                FstrROWID = list4.ROWID;
                dtpDate.Text = list4.SDATE.ToString();
                //txtSabun.Text = cf.Read_SabunName(clsDB.DbCon, list4.SABUN.ToString());
                switch (list4.GBN)
                {
                    case "1":
                        rdoGbn1.Checked = true;
                        break;
                    case "2":
                        rdoGbn2.Checked = true;
                        break;
                    case "3":
                        rdoGbn3.Checked = true;
                        break;
                    default:
                        rdoGbn4.Checked = true;
                        break;
                }
                txtRemark.Text = list4.REMARK;
            }

            //검사항목을 Display
            List<HIC_RESULT_EXCODE> list5 = hicResultExCodeService.GetItembyPaNoGjYear(nPano, strGjYear);


            SS3.ActiveSheet.RowCount = 0;
            SS3.ActiveSheet.RowCount = list5.Count;
            nREAD = list5.Count;
            nRow = 0;
            strRemark = "";
            for (int i = 0; i < nREAD; i++)
            {
                if (list5[i].EXCODE != null)
                {
                    strExCode = list5[i].EXCODE;                 //검사코드
                }

                if (list5[i].RESULT != null)
                {
                    strResult = list5[i].RESULT;                 //검사실 결과값
                }

                if (list5[i].RESCODE != null)
                {
                    strResCode = list5[i].RESCODE;               //결과값 코드
                }

                if (list5[i].RESULTTYPE != null)
                {
                    strResultType = list5[i].RESULTTYPE;         //결과값 TYPE
                }

                if (list5[i].GBCODEUSE != null)
                {
                    strGbCodeUse = list5[i].GBCODEUSE;           //결과값코드 사용여부
                }

                //SS3에 검사실 결과값을 DISPLAY
                nRow += 1;
                if (nRow > SS3.ActiveSheet.RowCount)
                {
                    SS3.ActiveSheet.RowCount = nRow;
                }
                SS3.ActiveSheet.Cells[i, 0].Text = " " + list5[i].HNAME;
                SS3.ActiveSheet.Cells[i, 1].Text = strResult;

                if (strExCode == "A103")
                {
                    //if (string.Compare(clsHcVariable.GstrGjYear, "2005") >= 0 && string.Compare(clsHcVariable.GstrGjYear, "2007") <= 0)
                    //{
                    //    strResCode = "061";
                    //}
                    //else if (string.Compare(clsHcVariable.GstrGjYear, "2008") >= 0)
                    //{
                    //    strResCode = "065";
                    //}
                    strResCode = "065";
                }

                if (strGbCodeUse == "Y")
                {
                    if (strResult != "")
                    {
                        strResName = hb.READ_ResultName(strResCode, strResult);
                        if (strResName == "") strResName = strResult;
                        SS3.ActiveSheet.Cells[i, 1].Text = strResName;
                        if (strResName != null && strResName.Length > 7)
                        {
                            strRemark += "▷" + list5[i].HNAME + ":";
                            strRemark += strResName + "\r\n";
                        }
                    }
                }
                else if (strResult != null && strResult.Length > 7)
                {
                    strRemark += "▷" + list5[i].HNAME + ":";
                    strRemark += strResult + "\r\n";
                }

                if (list5[i].PANJENG == "2")
                {
                    SS3.ActiveSheet.Cells[i, 2].Text = "*";
                }

                //참고치를 Dispaly
                //참고치를 Dispaly
                strNomal = hm.EXAM_NomalValue_SET(strExCode, FstrJepDate, strSex, list5[i].MIN_M, list5[i].MAX_M, list5[i].MIN_F, list5[i].MAX_F);

                HIC_JEPSU item = hicJepsuService.Read_Jepsu_Wrtno(list5[i].WRTNO);
                if(!item.IsNullOrEmpty())
                {
                    SS3.ActiveSheet.Cells[i, 3].Text = item.GJCHASU.ToString("");
                }
                SS3.ActiveSheet.Cells[i, 4].Text = strNomal;
                SS3.ActiveSheet.Cells[i, 8].Text = strExCode;
                SS3.ActiveSheet.Cells[i, 9].Text = strResult;   //정상값 점검용
                strExPan = list5[i].PANJENG;
                SS3.ActiveSheet.Cells[i, 2].Text = strExPan;

                //야간작업 검사결과가 비정상이면 R로 처리
                if (strExCode == "TZ72" || strExCode == "TZ85" || strExCode == "TZ86")
                {   
                    if (SS3.ActiveSheet.Cells[i, 1].Text == "비정상")
                    {
                        strExPan = "R";
                        SS3.ActiveSheet.Cells[i, 2].Text = strExPan;
                    }
                }

                //판정결과별 바탕색상을 다르게 표시함
                switch (strExPan)
                {
                    case "B":
                        SS3.ActiveSheet.Cells[i, 1].BackColor = Color.FromArgb(250, 210, 222);  //정상B
                        break;
                    case "C":
                        SS3.ActiveSheet.Cells[i, 1].BackColor = ColorTranslator.FromWin32(int.Parse("&HC0E0FF", System.Globalization.NumberStyles.AllowHexSpecifier)); ;  //주의C
                        break;
                    case "R":
                        SS3.ActiveSheet.Cells[i, 1].BackColor = Color.FromArgb(250, 170, 170);  //질환의심(R)
                        break;
                    default:
                        SS3.ActiveSheet.Cells[i, 1].BackColor = Color.FromArgb(190, 250, 220);  //정상A 또는 기타
                        break;
                }
            }

            //검사결과의 판정값이 R,B인것을 위에 표시
            FarPoint.Win.Spread.SortInfo[] si = new FarPoint.Win.Spread.SortInfo[] {
               new FarPoint.Win.Spread.SortInfo(2,true)
            ,  new FarPoint.Win.Spread.SortInfo(7,true)};

            SS3_Sheet1.SortRows(0, SS3_Sheet1.RowCount, si);

            fn_OLD_Result_Display(nPano, strJepDate, strSex);
        }

        //2차 접수번호 찾기
        string fn_GET_Second_Wrtno(long argWrtNo)
        {
            string rtnVal = "";
            long nPano = 0;
            string strGjYear = "";

            HIC_JEPSU list = hicJepsuService.GetPaNoGjYearbyWrtNo(argWrtNo);

            nPano = list.PANO;
            strGjYear = list.GJYEAR;

            //2차 통보일자를 읽음
            rtnVal = hicJepsuService.GetWrtnobyPanoGjYear(nPano, strGjYear);

            return rtnVal;
        }
    }
    
}
