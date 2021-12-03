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
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Pan
/// File Name       : frmHcPanJepsuPanjengCheckList.cs
/// Description     : 접수자 판정 점검표
/// Author          : 이상훈
/// Create Date     : 2019-12-19
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmPanjengList.frm(HcPan08)" />

namespace HC_Pan
{
    public partial class frmHcPanJepsuPanjengCheckList : Form
    {
        HicJepsuLtdExjongService hicJepsuLtdExjongService = null;
        HicResDentalService hicResDentalService = null;
        HicResBohum1Service hicResBohum1Service = null;
        HicResBohum2Service hicResBohum2Service = null;
        HicCancerNewService hicCancerNewService = null;
        HicResSpecialService hicResSpecialService = null;
        HicExjongService hicExjongService = null;

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        frmHcPanExamResultRegChg FrmHcPanExamResultRegChg = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        int nREAD = 0;
        int nRow = 0;
        long nWRTNO = 0;
        string strFDate = "";
        string strTDate = "";
        string strOK = "";
        string strJepDate = "";
        string strGjJong = "";
        string strGjChasu = "";
        string strGbSTS = "";
        string strLtdCode = "";
        string strGbEntry = "";
        string strPanDate = "";
        string strGbDental = "";
        string strDental1 = "";
        string strDental2 = "";
        string strJong = "";
        long nLtdCode = 0;

        public frmHcPanJepsuPanjengCheckList()
        {
            InitializeComponent();

            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicJepsuLtdExjongService = new HicJepsuLtdExjongService();
            hicResDentalService = new HicResDentalService();
            hicResBohum1Service = new HicResBohum1Service();
            hicResBohum2Service = new HicResBohum2Service();
            hicCancerNewService = new HicCancerNewService();
            hicResSpecialService = new HicResSpecialService();
            hicExjongService = new HicExjongService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.cboFYYMM.KeyPress += new KeyPressEventHandler(eCboKeyPress);
            this.cboTYYMM.KeyPress += new KeyPressEventHandler(eCboKeyPress);
            this.cboJong.KeyPress += new KeyPressEventHandler(eCboKeyPress);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eKeyPress);
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            long nYY = 0;
            long nMM = 0;
            string strYYMM = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            txtLtdCode.Text = "";
            nYY = long.Parse(VB.Left(clsPublic.GstrSysDate, 4));
            nMM = long.Parse(VB.Mid(clsPublic.GstrSysDate, 6, 2));

            cboFYYMM.Items.Clear();
            cboTYYMM.Items.Clear();

            for (int i = 0; i < 24; i++)
            {
                strYYMM = string.Format("{0:0000}", nYY) + string.Format("{0:00}", nMM);
                cboFYYMM.Items.Add(strYYMM);
                cboTYYMM.Items.Add(strYYMM);
                nMM -= 1;
                if (nMM == 0)
                {
                    nMM = 12;
                    nYY -= 1;
                }
            }

            cboFYYMM.SelectedIndex = 1;
            cboTYYMM.SelectedIndex = 1;
            //hb.ComboJong_Set(cboJong);
            List<HIC_EXJONG> list = hicExjongService.Read_ExJong_Add(true);

            if (list.Count > 0)
            {
                cboJong.Items.Clear();
                cboJong.Items.Add("**.전체");
                for (int i = 0; i < list.Count; i++)
                {
                    cboJong.Items.Add(list[i].CODE + "." + list[i].NAME);
                }

                cboJong.SelectedIndex = 0;
            }
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
                nRow = 0;
                sp.Spread_All_Clear(SS1);
                Application.DoEvents();

                strFDate = VB.Left(cboFYYMM.Text, 4) + "-" + VB.Right(cboFYYMM.Text, 2) + "-01";
                strTDate = VB.Left(cboTYYMM.Text, 4) + "-" + VB.Right(cboTYYMM.Text, 2) + "-01";
                strTDate = cf.READ_LASTDAY(clsDB.DbCon, strTDate);

                strJong = VB.Left(cboJong.Text, 2);
                nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>();

                //접수내역을 읽음
                List<HIC_JEPSU_LTD_EXJONG> list = hicJepsuLtdExjongService.GetItembyJepDate(strFDate, strTDate, strJong, nLtdCode);

                Cursor.Current = Cursors.WaitCursor;

                nREAD = list.Count;
                progressBar1.Maximum = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    nWRTNO = list[i].WRTNO;
                    strJepDate = list[i].JEPDATE;
                    strGjJong = list[i].GJJONG;
                    strGjChasu = list[i].GJCHASU;
                    strLtdCode = list[i].LTDCODE.ToString();
                    strGbSTS = list[i].GBSTS;
                    strGbDental = list[i].GBDENTAL;
                    strOK = "";
                    strDental1 = "";
                    strDental2 = "";

                    switch (strGjJong)
                    {
                        case "11":                            
                        case "12":
                        case "13":
                        case "14":
                            ufn_btnView_First_Check(list, i, nWRTNO);
                            break;
                        case "17":
                        case "18":
                            ufn_CmdView_Second_Check(list, i, nWRTNO);
                            break;
                        case "31":
                            ufn_CmdView_Cancer_Check(list, i, nWRTNO);
                            break;
                        default:
                            ufn_CmdView_Special_Check(list, i, nWRTNO);
                            break;
                    }

                    if (rdoJob1.Checked == true || strOK == "OK")
                    {
                        nRow += 1;
                        if (nRow > SS1.ActiveSheet.RowCount)
                        {
                            SS1.ActiveSheet.RowCount = nRow;
                        }

                        SS1.ActiveSheet.Cells[nRow - 1, 0].Text = strJepDate;
                        SS1.ActiveSheet.Cells[nRow - 1, 1].Text = nWRTNO.To<string>();
                        SS1.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].SNAME;
                        SS1.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].LTDNAME;
                        SS1.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].EXNAME;
                        SS1.ActiveSheet.Cells[nRow - 1, 5].Text = strGbEntry;
                        if (strPanDate == "OK")
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 6].Text = VB.Mid(strPanDate, 6, 2) + "/" + VB.Right(strPanDate, 2);
                        }
                        else if (!strPanDate.IsNullOrEmpty())
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 6].Text = "";
                        }
                        SS1.ActiveSheet.Cells[nRow - 1, 7].Text = strDental1;
                        SS1.ActiveSheet.Cells[nRow - 1, 8].Text = strDental2;
                        SS1.ActiveSheet.Cells[nRow - 1, 9].Text = hm.UCode_Names_Display(list[i].UCODES);
                        SS1.ActiveSheet.Cells[nRow - 1, 10].Text = hm.SExam_Names_Display(list[i].SEXAMS);
                    }
                    progressBar1.Value = i + 1;
                }

                SS1.ActiveSheet.RowCount = nRow;

                Cursor.Current = Cursors.Default;
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

                strTitle = "접수자 판정 점검표";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("점검기간:" + cboFYYMM.Text + " ~ " + cboTYYMM.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += sp.setSpdPrint_String("출력일시:" + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, true, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
        }

        void eCboKeyPress(object sender, KeyPressEventArgs e)
        {
            SendKeys.Send("{TAB}");
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            long nWrtNo = 0;

            if (e.Column == 5)
            {
                nWrtNo = long.Parse(SS1.ActiveSheet.Cells[e.Row, 1].Text.Trim());
                FrmHcPanExamResultRegChg = new frmHcPanExamResultRegChg(nWrtNo, "", "");
                FrmHcPanExamResultRegChg.ShowDialog(this);
            }
        }

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

        void ufn_btnView_First_Check(List<HIC_JEPSU_LTD_EXJONG> list, int i, long ArgWrtNo)
        {
            strGbEntry = "Y";
            if (string.Compare(list[i].GBSTS, "1") <= 0)
            {
                strGbEntry = "N";
            }

            if (list[i].GBMUNJIN1 == "N")
            {
                strGbEntry = "N";
            }
            strDental1 = "";
            strDental2 = "";
            if (strGbDental == "Y")
            {
                strDental1 = "Y";
                if (list[i].GBMUNJIN2 != "Y")
                {
                    strDental1 = "N";
                }
                //구강판정을 읽음
                HIC_RES_DENTAL list2 = hicResDentalService.GetPanjengDatebyWrtNo(ArgWrtNo);

                strDental2 = "N";
                if (!list2.IsNullOrEmpty())
                {
                    if (!list2.PANJENGDATE.IsNullOrEmpty())
                    {
                        strDental2 = "Y";
                    }
                }
            }

            //1차판정 여부를 읽음
            HIC_RES_BOHUM1 list3 = hicResBohum1Service.GetPanjengDatebyWrtNo(ArgWrtNo);
            if (!list3.IsNullOrEmpty())
            {
                strPanDate = list3.PANJENGDATE.To<string>();
                if (list3.GBPANJENG != "Y")
                {
                    strPanDate = "";
                }
            }

            strOK = "";
            if (strPanDate.IsNullOrEmpty())
            {
                strOK = "OK";
            }

            if (strGbEntry == "N")
            {
                strOK = "OK";
            }

            if (strGbDental == "Y")
            {
                if (strDental1 == "N" || strDental2 == "N")
                {
                    strOK = "OK";
                }
            }
        }

        void ufn_CmdView_Second_Check(List<HIC_JEPSU_LTD_EXJONG> list, int i, long ArgWrtNo)
        {
            strGbEntry = "Y";
            if (string.Compare(list[i].GBSTS, "1") <= 0)
            {
                strGbEntry = "N";
            }

            HIC_RES_BOHUM2 list2 = hicResBohum2Service.GetItemByWrtno(ArgWrtNo);

            strPanDate = list2.PANJENGDATE;
            if (list2.PANJENGDRNO == 0)
            {
                strPanDate = "";
            }
            strOK = "";
            if (strPanDate.IsNullOrEmpty())
            {
                strOK = "OK";
            }

            if (strGbEntry == "N")
            {
                strOK = "OK";
            }
        }

        void ufn_CmdView_Cancer_Check(List<HIC_JEPSU_LTD_EXJONG> list, int i, long ArgWrtNo)
        {
            strGbEntry = "Y";
            if (string.Compare(list[i].GBSTS, "1") <= 0)
            {
                strGbEntry = "N";
            }

            strPanDate = "OK";
            if (hicCancerNewService.GetPanjengDrNobyWrtNo(ArgWrtNo) == 0)
            {
                strPanDate = "";
            }

            strOK = "";
            if (strPanDate.IsNullOrEmpty())
            {
                strOK = "OK";
            }

            if (strGbEntry == "N")
            {
                strOK = "OK";
            }
        }

        void ufn_CmdView_Special_Check(List<HIC_JEPSU_LTD_EXJONG> list, int i, long ArgWrtNo)
        {
            strGbEntry = "Y";
            if (string.Compare(list[i].GBSTS, "1") <= 0)
            {
                strGbEntry = "N";
            }

            if (list[i].GBMUNJIN3 == "N")
            {
                strGbEntry = "N";
            }

            //판정일 체크 안함(채용배치전2, 일반채용2, 건강진단, 회사추가)
            if (strGjJong != "29" && strGjJong != "27" && strGjJong != "32" && strGjJong != "69")
            {
                HIC_RES_SPECIAL list2 = hicResSpecialService.GetItembyWrtNo(ArgWrtNo);

                if (!list2.IsNullOrEmpty())
                {
                    strPanDate = list2.PANJENGDATE.To<string>();
                    if (list2.PANJENGDRNO == 0)
                    {
                        strPanDate = "";
                    }
                    else
                    {
                        strPanDate = "";
                    }
                }
                else
                {
                    strPanDate = "OK";
                }

                strOK = "";
                if (strPanDate.IsNullOrEmpty())
                {
                    strOK = "OK";
                }

                if (strGbEntry == "N")
                {
                    strOK = "OK";
                }
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
