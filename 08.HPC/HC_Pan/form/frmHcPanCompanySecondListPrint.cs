using ComBase;
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

/// <summary>
/// Class Name      : HC_Pan
/// File Name       : frmHcPanCompanySecondListPrint.cs
/// Description     : 사업장 2차대상자 명단 인쇄
/// Author          : 이상훈
/// Create Date     : 2019-11-06
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm2차검진명단인쇄.frm(Frm2차검진명단인쇄)" />

namespace HC_Pan
{
    public partial class frmHcPanCompanySecondListPrint : Form
    {
        HicJepsuService hicJepsuService = null;
        HicJepsuPatientService hicJepsuPatientService = null;
        HicResultExCodeService hicResultExCodeService = null;

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        frmHcPanExamResultRegChg FrmHcPanExamResultRegChg = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        public frmHcPanCompanySecondListPrint()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicJepsuService = new HicJepsuService();
            hicJepsuPatientService = new HicJepsuPatientService();
            hicResultExCodeService = new HicResultExCodeService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.btnListView.Click += new EventHandler(eBtnClick); 
            this.rdoGbn1.Click += new EventHandler(eRdoClick);
            this.cboYear.Click += new EventHandler(eCboClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SS1.Change += new ChangeEventHandler(eSpdChange);
            //this.SS1.ButtonClicked += new EditorNotifyEventHandler(eSpdBtnClick);
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            int nYEAR = 0;

            ssList_Sheet1.ColumnHeader.Rows.Get(-1).Height = 24;
            SS1_Sheet1.ColumnHeader.Rows.Get(-1).Height = 24;

            ssList_Sheet1.Columns.Get(5).Visible = false;   //회사코드
            SS1_Sheet1.Columns.Get(9).Visible = false;

            txtLtdCode.Text = "";
            ComFunc.ReadSysDate(clsDB.DbCon);

            nYEAR = int.Parse(VB.Left(clsPublic.GstrSysDate, 4));
            cboYear.Items.Clear();
            for (int i = 0; i < 5; i++)
            {
                cboYear.Items.Add(nYEAR);
                nYEAR -= 1;
            }
            cboYear.SelectedIndex = 0;

            dtpFrDate.Value = Convert.ToDateTime(cboYear.Text + "-01-01");
            dtpToDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);

            cboBangi.Items.Clear();
            cboBangi.Items.Add("전체");
            cboBangi.Items.Add("상반기");
            cboBangi.Items.Add("하반기");
            cboBangi.SelectedIndex = 0;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnLtdCode)
            {
                FrmHcLtdHelp = new frmHcLtdHelp();
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
            else if (sender == btnListView)
            {
                int nREAD = 0;
                int nRow = 0;
                long nCNT = 0;
                long nCNT1 = 0;
                long nCNT2 = 0;
                long nCNT3 = 0;
                long nCNT4 = 0;
                string strOldData = "";
                string strNewData = "";
                string strOK = "";

                string strFrDate = "";
                string strToDate = "";
                string strBangi = "";
                string strGjJong = "";
                string strGjYear = "";
                string strLtdCode = "";

                sp.Spread_All_Clear(ssList);
                ssList.ActiveSheet.RowCount = 30;

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;

                strGjYear = cboYear.Text;

                if (cboBangi.Text == "상반기")
                {
                    strBangi = "1";
                }
                else if (cboBangi.Text == "하반기")
                {
                    strBangi = "2";
                }
                else
                {
                    strBangi = "";
                }

                if (rdoGbn1.Checked == true)
                {
                    strGjJong = "1";
                }
                else if (rdoGbn2.Checked == true)
                {
                    strGjJong = "2";
                }
                else
                {
                    strGjJong = "";
                }

                strLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1);

                ssList.Enabled = false;

                List<HIC_JEPSU> list = hicJepsuService.GetItembyJepDateLtdCodeGjYearBangi(strFrDate, strToDate, strGjJong, strGjYear, strBangi);

                nREAD = list.Count;
                //ssList.ActiveSheet.RowCount = nREAD;
                nRow = 0;
                nCNT1 = 0; nCNT2 = 0; nCNT3 = 0; nCNT4 = 0;
                strOldData = "";
                for (int i = 0; i < nREAD; i++)
                {
                    strNewData = list[i].LTDCODE.ToString();
                    nCNT = list[i].CNT;
                    if (strOldData.IsNullOrEmpty())
                    {
                        strOldData = strNewData;
                    }

                    if (strOldData == strNewData)
                    {
                        nCNT1 += nCNT;  //접수인원
                        if (list[i].SECOND_FLAG.IsNullOrEmpty())
                        {
                            nCNT2 += nCNT;  //미판정
                        }
                        else if (list[i].SECOND_FLAG == "Y")
                        {
                            if (!list[i].SECOND_TONGBO.ToString().IsNullOrEmpty())
                            {
                                nCNT3 += nCNT; //통보
                            }
                            else
                            {
                                nCNT4 += nCNT; //미통보
                            }
                        }
                    }
                    else
                    {
                        strOK = "OK";
                        if (nCNT1 == 0)
                        {
                            strOK = "NO";
                        }

                        if (chkList.Checked == true && nCNT4 == 0)
                        {
                            strOK = "NO";
                        }

                        if (strOK == "OK")
                        {
                            nRow += 1;
                            if (nRow > ssList.ActiveSheet.RowCount)
                            {
                                ssList.ActiveSheet.RowCount = nRow;
                            }
                            ssList.ActiveSheet.Cells[nRow - 1, 0].Text = hb.READ_Ltd_Name(strOldData);
                            ssList.ActiveSheet.Cells[nRow - 1, 1].Text = string.Format("{0:###,###,##0}", nCNT1);
                            ssList.ActiveSheet.Cells[nRow - 1, 2].Text = string.Format("{0:###,###,##0}", nCNT2);
                            ssList.ActiveSheet.Cells[nRow - 1, 3].Text = string.Format("{0:###,###,##0}", nCNT3);
                            ssList.ActiveSheet.Cells[nRow - 1, 4].Text = string.Format("{0:###,###,##0}", nCNT4);
                            ssList.ActiveSheet.Cells[nRow - 1, 5].Text = strOldData;
                        }

                        strOldData = strNewData;
                        nCNT1 = 0; nCNT2 = 0; nCNT3 = 0; nCNT4 = 0;
                    }
                }

                strOK = "OK";
                if (nCNT1 == 0)
                {
                    strOK = "NO";
                }

                if (chkList.Checked == true && nCNT4 == 0)
                {
                    strOK = "NO";
                }

                if (strOK == "OK")
                {
                    nRow += 1;
                    if (nRow > ssList.ActiveSheet.RowCount)
                    {
                        ssList.ActiveSheet.RowCount = nRow;
                    }
                    ssList.ActiveSheet.Cells[nRow - 1, 0].Text = hb.READ_Ltd_Name(list[nRow - 1].LTDCODE.ToString());
                    ssList.ActiveSheet.Cells[nRow - 1, 1].Text = string.Format("{0:###,###,##0}", nCNT1);
                    ssList.ActiveSheet.Cells[nRow - 1, 2].Text = string.Format("{0:###,###,##0}", nCNT1);
                    ssList.ActiveSheet.Cells[nRow - 1, 3].Text = string.Format("{0:###,###,##0}", nCNT1);
                    ssList.ActiveSheet.Cells[nRow - 1, 4].Text = string.Format("{0:###,###,##0}", nCNT1);
                    ssList.ActiveSheet.Cells[nRow - 1, 5].Text = strOldData;
                }

                ssList.ActiveSheet.RowCount = nRow;
                ssList.Enabled = true;
            }
            else if (sender == btnSearch)
            {
                int nREAD = 0;
                int nRow = 0;
                long nWRTNO = 0;
                int nCNT = 0;
                string strGbSTS = "";
                string strGjJong = "";
                string strGbMunjin = "";
                string strOK = "";
                string strTemp = "";
                string StrJumin = "";

                string strFrDate = "";
                string strToDate = "";
                string strGjYear = "";
                string strBangi = "";
                string strLtdCode = "";
                string strTongbo = "";

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;
                strGjYear = cboYear.Text;
                if (cboBangi.Text == "상반기")
                {
                    strBangi = "1";
                }
                else if (cboBangi.Text == "하반기")
                {
                    strBangi = "2";
                }
                else
                {
                    strBangi = "";
                }

                strLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1);

                if (rdoTongbo1.Checked == true)
                {
                    strTongbo = "0";
                }
                else if (rdoTongbo2.Checked == true)
                {
                    strTongbo = "1";
                }
                else
                {
                    strTongbo = "2";
                }

                sp.Spread_All_Clear(SS1);
                SS1.ActiveSheet.RowCount = 30;

                if (txtLtdCode.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("회사코드가 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                List<HIC_JEPSU_PATIENT> list = hicJepsuPatientService.GetItembyJepDateGjYear(strFrDate, strToDate, strGjYear, strBangi, strLtdCode, strTongbo);

                nREAD = list.Count;
                nRow = 0;
                //SS1.ActiveSheet.RowCount = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    nWRTNO = list[i].WRTNO;
                    strGbSTS = list[i].GBSTS;   //검사상태
                    strGjJong = list[i].GJJONG; // 검사종류
                    strGbMunjin = list[i].GBMUNJIN;// 검사종류
                    StrJumin = clsAES.DeAES(list[i].JUMIN2);

                    nRow += 1;
                    if (nRow > SS1.ActiveSheet.RowCount)
                    {
                        SS1.ActiveSheet.RowCount = nRow;
                    }

                    SS1.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].SNAME.Trim();
                    SS1.ActiveSheet.Cells[nRow - 1, 1].Text = VB.Left(StrJumin, 6) + "-" + VB.Right(StrJumin, 7);
                    if (list[i].UCODES.Trim().IsNullOrEmpty())
                    {
                        SS1.ActiveSheet.Cells[nRow - 1, 2].Text = "일반";
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[nRow - 1, 2].Text = "특수+일반";
                    }
                    SS1.ActiveSheet.Cells[nRow - 1, 3].Text = " " + list[i].SECOND_EXAMS.Trim();
                    SS1.ActiveSheet.Cells[nRow - 1, 4].Text = " " + list[i].SECOND_SAYU.Trim();

                    List<HIC_RESULT_EXCODE> list2 = hicResultExCodeService.GetItembyWrtNoExCode(nWRTNO);

                    strTemp = "";
                    if (list2.Count > 0)
                    {
                        for (int j = 0; j < list2.Count; j++)
                        {
                            if (list2[j].GBCODEUSE == "Y")
                            {
                                strTemp += list2[j].YNAME + ":" + hb.READ_ResultName(list2[j].RESCODE, list2[j].RESCODE) + ", ";
                            }
                            else
                            {
                                strTemp += list2[j].YNAME + ":" + list2[j].RESULT + ", ";
                            }
                        }
                    }

                    if (strTemp.Length > 1)
                    {
                        SS1.ActiveSheet.Cells[nRow - 1, 5].Text = VB.Mid(strTemp.Trim(), 1, strTemp.Trim().Length - 1);
                    }

                    if (!list[i].SECOND_DATE.IsNullOrEmpty())
                    {
                        SS1.ActiveSheet.Cells[nRow - 1, 6].Text = VB.Right(list[i].SECOND_DATE, 5);
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[i, 6].Text = "";
                    }

                    SS1.ActiveSheet.Cells[nRow - 1, 7].Text = VB.Right(list[i].JEPDATE.ToString(), 5);

                    if (!list[i].SECOND_TONGBO.IsNullOrEmpty())
                    {
                        SS1.ActiveSheet.Cells[nRow - 1, 8].Text = VB.Right(list[i].SECOND_TONGBO, 5);
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[nRow - 1, 8].Text = "";
                    }
                    SS1.ActiveSheet.Cells[nRow - 1, 9].Text = list[i].WRTNO.ToString();
                }

                SS1.ActiveSheet.RowCount = nRow;

                //RowHeight 조정
                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    if (!SS1.ActiveSheet.Cells[i, 4].Text.IsNullOrEmpty())
                    {
                        Size size = SS1.ActiveSheet.GetPreferredCellSize(i, 4);
                        SS1.ActiveSheet.Rows[i].Height = size.Height;
                    }
                    else
                    {
                        SS1.ActiveSheet.Rows[i].Height = 22;
                    }
                }

                SS1.Enabled = true;
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

                strTitle = "회사명: " + VB.Pstr(txtLtdCode.Text, ".", 2);
                strHeader = sp.setSpdPrint_String(strTitle, new Font("맑은 고딕", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
            else if (sender == btnUpdate1 || sender == btnUpdate2)
            {
                long nWrtNo = 0;
                string strTongbo = "";
                int result = 0;

                if (MessageBox.Show("정말로 작업을 하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                clsDB.setBeginTran(clsDB.DbCon);

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strTongbo = SS1.ActiveSheet.Cells[i, 7].Text.Trim();
                    nWrtNo = long.Parse(SS1.ActiveSheet.Cells[i, 8].Text);
                    if (sender == btnUpdate1)
                    {
                        if (strTongbo.IsNullOrEmpty())
                        {
                            result = hicJepsuService.UpdateSecond_TongBobyWrtNo(nWrtNo);

                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                MessageBox.Show("통보누락 오늘날짜로 변경시 오류 발생!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                    else
                    {
                        result = hicJepsuService.UpdateSecond_TongBobyWrtNo(nWrtNo);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("통보누락 오늘날짜로 변경시 오류 발생!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                eBtnClick(btnSearch, new EventArgs());
            }
        }

        void eCboClick(object sender, EventArgs e)
        {
            if (sender == cboYear)
            {
                dtpFrDate.Text = cboYear.Text + "-01-01";
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

        void eSpdChange(object sender, ChangeEventArgs e)
        {
            SS1.ActiveSheet.Cells[e.Row, 12].Text = "Y";
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                long nWrtNo = 0;
                string strData = "";

                strData = SS1.ActiveSheet.Cells[e.Row, e.Column].Text.Trim();
                nWrtNo = long.Parse(SS1.ActiveSheet.Cells[e.Row, 9].Text.Trim());   //접수번호

                switch (e.Column)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        break;
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                        if (strData == "N" || strData == "Y")
                        {
                            if (e.Column == 5)
                            {
                                ///TODO : 이상훈(2019.11.07) Frm건강검진문진 컨버전 후 재작업
                                //Frm건강검진문진 f = new Frm건강검진문진();
                                //f.ShowDialog();
                            }
                            if (e.Column == 6)
                            {
                                ///TODO : 이상훈(2019.11.07) Frm구강문진판정 컨버전 후 재작업
                                //Frm구강문진판정 f = new Frm구강문진판정();
                                //f.ShowDialog();
                            }
                            if (e.Column == 7)
                            {
                                ///TODO : 이상훈(2019.11.07) Frm특수검진문진_2010 컨버전 후 재작업
                                //Frm특수검진문진_2010 f = new Frm특수검진문진_2010();
                                //f.ShowDialog();
                            }
                        }
                        break;
                    case 11:
                    case 12:
                    case 13:
                    case 14:
                    case 15:
                    case 16:
                    case 17:
                    case 18:
                        FrmHcPanExamResultRegChg = new frmHcPanExamResultRegChg(nWrtNo, "", "");
                        FrmHcPanExamResultRegChg.StartPosition = FormStartPosition.CenterScreen;
                        FrmHcPanExamResultRegChg.ShowDialog(this);
                        FrmHcPanExamResultRegChg = null;
                        FrmHcPanExamResultRegChg.Dispose();
                        break;
                    default:
                        break;
                }
            }
            else if (sender == ssList)
            {
                txtLtdCode.Text = ssList.ActiveSheet.Cells[e.Row, 5].Text.Trim() + "." + hb.READ_Ltd_Name(ssList.ActiveSheet.Cells[e.Row, 5].Text.Trim());
                eBtnClick(btnSearch, new EventArgs());
            }
        }

        void eSpdClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {

            }
        }

        void eRdoClick(object sender, EventArgs e)
        {
            eBtnClick(btnSearch, new EventArgs());
        }
    }
}
