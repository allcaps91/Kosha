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
/// Class Name      : ComHpcLibB
/// File Name       : frmHcAmResponse.cs
/// Description     : 암검진 여부확인
/// Author          : 이상훈
/// Create Date     : 2020-01-01
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmResponse.frm(HcAm07)" />

namespace ComHpcLibB
{
    public partial class frmHcAmResponse : Form
    {
        HicJepsuPatientService hicJepsuPatientService = null;
        HicCancerChkService hicCancerChkService = null;
        HicJepsuService hicJepsuService = null;

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        public frmHcAmResponse()
        {
            InitializeComponent();

            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicJepsuPatientService = new HicJepsuPatientService();
            hicCancerChkService = new HicCancerChkService();
            hicJepsuService = new HicJepsuService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick); 
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.SS1.Change += new ChangeEventHandler(eSpdChange);
            this.SS1.LeaveCell += new LeaveCellEventHandler(eSpdLeaveCell);
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            long nYY = 0;
            long nMM = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);

            cboFYYMM.Items.Clear();
            cboTYYMM.Items.Clear();
            nYY = long.Parse(VB.Left(clsPublic.GstrSysDate, 4)) - 2;
            nMM = 1;

            for (int i = 1; i <= 12; i++)
            {
                cboFYYMM.Items.Add(nYY + "년" + string.Format("{0:00}", nMM) + "월");
                cboTYYMM.Items.Add(nYY + "년" + string.Format("{0:00}", nMM) + "월");
                nMM += 1;
            }

            cboFYYMM.SelectedIndex = 0;
            cboTYYMM.SelectedIndex = 0;

            cboJong.Items.Add("31.암검진");
            cboJong.SelectedIndex = 0;

            txtLtdCode.Text = "";
            lblSts.Text = "";

            SS1_Sheet1.Columns.Get(6).Visible = false;
            SS1_Sheet1.Columns.Get(7).Visible = false;
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
                int nREAD = 0;
                int nREAD2 = 0;
                string strDate1 = "";
                string strDate2 = "";
                string strDate3 = "";
                string strDate4 = "";
                int nRow = 0;
                string strOK = "";
                string StrJumin = "";
                int nCNT = 0;
                string strJong = "";
                long nLtdCode = 0;
                string strRemark = "";

                sp.Spread_All_Clear(SS1);

                strDate1 = VB.Left(cboFYYMM.Text, 4) + "-" + VB.Mid(cboFYYMM.Text, 6, 2) + "-01";
                strDate2 = cf.READ_LASTDAY(clsDB.DbCon, VB.Left(cboFYYMM.Text, 4) + "-" + VB.Mid(cboFYYMM.Text, 6, 2) + "-01");

                strDate3 = long.Parse(VB.Left(cboFYYMM.Text, 4)) + 2 + "-01-01";
                strDate4 = long.Parse(VB.Left(cboFYYMM.Text, 4)) + 2 + "-12-31";

                if (VB.Left(cboJong.Text, 2) != "00")
                {
                    strJong = VB.Left(cboJong.Text, 2);
                }
                if (!txtLtdCode.Text.IsNullOrEmpty())
                {
                    nLtdCode = long.Parse(VB.Pstr(txtLtdCode.Text, ".", 1));
                }
                else
                {
                    nLtdCode = 0;
                }

                List<HIC_JEPSU_PATIENT> list = hicJepsuPatientService.GetItembyJepDateGjjongLtdCode(strDate1, strDate2, strJong, nLtdCode);

                nREAD = list.Count;
                SS1.ActiveSheet.RowCount = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    strOK = "OK";
                    //당해년도 자료 조회
                    HIC_JEPSU list2 = hicJepsuService.GetJepDAtebyJepDatePano(strDate3, strDate4, list[i].PANO, strJong, nLtdCode);

                    if (!list2.IsNullOrEmpty())
                    {
                        strOK = "";
                    }

                    if (chkOK.Checked == true)
                    {
                        strOK = "OK";
                    }

                    if (strOK == "OK")
                    {
                        nRow += 1;
                        StrJumin = clsAES.DeAES(list[i].JUMIN2.Trim());

                        SS1.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].SNAME;
                        SS1.ActiveSheet.Cells[nRow - 1, 1].Text = VB.Left(StrJumin, 6) + "-" + VB.Mid(StrJumin, 7, 1) + "******";
                        SS1.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].TEL;
                        SS1.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].JEPDATE.ToString();
                        SS1.ActiveSheet.Cells[nRow - 1, 6].Text = list[i].PANO.ToString();

                        strRemark = hicCancerChkService.GetRemarkbyPanoYear(list[i].PANO, VB.Left(cboFYYMM.Text, 4));

                        if (!strRemark.IsNullOrEmpty())
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 5].Text = strRemark;
                        }
                        if (!list2.IsNullOrEmpty())
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 4].Text = list2.JEPDATE;
                            nCNT += 1;
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 4].Text = "";
                        }
                    }
                }

                if (chkOK.Checked == true)
                {
                    lblSts.Text = nCNT + "/" + nREAD;
                }
                else
                {
                    lblSts.Text = "";
                }
            }
            else if (sender == btnPrint)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ComFunc.ReadSysDate(clsDB.DbCon);

                Cursor.Current = Cursors.WaitCursor;

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "암검진 미실시 대상 조회";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("검진회사:" + VB.Pstr(txtLtdCode.Text, ".", 2), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, false);
                strHeader += sp.setSpdPrint_String("출력일시:" + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);

                Cursor.Current = Cursors.Default;
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

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
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
            else
            {
                if (e.KeyChar == 13)
                {
                    SendKeys.Send("{TAB}");
                }
            }
        }

        void eSpdChange(object sender, ChangeEventArgs e)
        {
            if (sender == SS1)
            {
                SS1.ActiveSheet.Cells[e.Row, 7].Text = "Y";
            }
        }

        void eSpdLeaveCell(object sender, LeaveCellEventArgs e)
        {
            string strYear = "";
            string strPano = "";
            string strOK = "";
            string strROWID = "";
            string strRemark = "";
            int result = 0;

            strYear = VB.Left(cboFYYMM.Text, 4);

            clsDB.setBeginTran(clsDB.DbCon);

            for (int i = 0; i < SS1.ActiveSheet.NonEmptyRowCount; i++)
            {
                strRemark = SS1.ActiveSheet.Cells[i, 5].Text.Trim();
                strPano = SS1.ActiveSheet.Cells[i, 6].Text.Trim();
                strOK = SS1.ActiveSheet.Cells[i, 7].Text.Trim();

                if (strOK == "Y")
                {
                    strROWID = hicCancerChkService.GetRowIdbyPanoYear(strPano, strYear);

                    HIC_CANCER_CHK item = new HIC_CANCER_CHK();

                    item.PANO = long.Parse(strPano);
                    item.YEAR = strYear;
                    item.REMARK = strRemark;
                    item.JOBSABUN = long.Parse(clsType.User.IdNumber);
                    item.ROWID = strROWID;

                    if (strROWID.IsNullOrEmpty())
                    {
                        result = hicCancerChkService.Insert(item);
                    }
                    else
                    {
                        result = hicCancerChkService.Update(item);
                    }

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                    }
                }
            }
            clsDB.setCommitTran(clsDB.DbCon);
        }
    }
}
