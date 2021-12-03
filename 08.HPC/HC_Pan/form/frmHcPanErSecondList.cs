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
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Pan
/// File Name       : frmHcPanErSecondList.cs
/// Description     : 응급2차 검진 대상자
/// Author          : 이상훈
/// Create Date     : 2019-12-27
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmErSecondList.frm(HcPan12)" />

namespace HC_Pan
{
    public partial class frmHcPanErSecondList : Form
    {
        HicJepsuService hicJepsuService = null;
        HicJepsuLtdExjongService hicJepsuLtdExjongService = null;
        HicResBohum1Service hicResBohum1Service = null;
        HicResSpecialService hicResSpecialService = null;
        HicExjongService hicExjongService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        frmHcPanExamResultRegChg FrmHcPanExamResultRegChg = null;

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        public frmHcPanErSecondList()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicJepsuService = new HicJepsuService();
            hicJepsuLtdExjongService = new HicJepsuLtdExjongService();
            hicResBohum1Service = new HicResBohum1Service();
            hicResSpecialService = new HicResSpecialService();
            hicExjongService = new HicExjongService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eKeyPress);
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);
            SS1_Sheet1.ColumnHeader.Rows.Get(-1).Height = 26F;
            txtLtdCode.Text = "";
            //hb.ComboJong_Set(cboJong);
            List<HIC_EXJONG> list = hicExjongService.Read_ExJong_Add(true);

            cboJong.Items.Clear();
            cboJong.Items.Add("**.전체");
            for (int i = 0; i < list.Count; i++)
            {
                cboJong.Items.Add(list[i].CODE + "." + list[i].NAME);
            }

            cboJong.SelectedIndex = 0;
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
            else if (sender == btnPrint)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "응급2차 검진 대상자";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("맑은 고딕", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("출력일자:" + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + "\r\n", new Font("굴림체", 11), clsSpread.enmSpdHAlign.Left, true, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
            else if (sender == btnSave)
            {
                int nRow = 0;
                string strExams = "";
                string strSayu = "";
                string strMiSayu = "";
                string strTongbo = "";
                long nWRTNO = 0;
                string strChange = "";
                string strGbTong = "";
                int result = 0;

                clsDB.setBeginTran(clsDB.DbCon);

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strGbTong = SS1.ActiveSheet.Cells[i, 6].Text.Trim();
                    strGbTong = SS1.ActiveSheet.Cells[i, 7].Text.Trim();
                    nWRTNO = long.Parse(SS1.ActiveSheet.Cells[i, 61].Text.Trim());

                    if (strGbTong == "True")
                    {
                        result = hicJepsuService.UpdateErTongbobyWrtNo(nWRTNO);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show(i + "번줄 자료를 저장중 오류 발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);

                eBtnClick(btnSearch, new EventArgs());
            }
            else if (sender == btnTongbo)
            {
                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    if (SS1.ActiveSheet.Cells[i, 7].Text.IsNullOrEmpty())
                    {
                        SS1.ActiveSheet.Cells[i, 6].Text = "True";
                    }
                }
            }
            else if (sender == btnSearch)
            {
                int nREAD = 0;
                int nRow = 0;
                long nWRTNO = 0;
                string strPanDate = "";
                string strGjJong = "";
                long nLtdCode = 0;
                string strTongbo = "";

                strGjJong = VB.Left(cboJong.Text, 2);
                if (strGjJong == ".") strGjJong = "";
                if (txtLtdCode.Text.IsNullOrEmpty())
                {
                    nLtdCode = 0;
                }
                else
                {
                    nLtdCode = long.Parse(VB.Pstr(txtLtdCode.Text, ".", 1));
                }
                if (rdoTongbo2.Checked == true)
                {
                    strTongbo = "2";
                }
                else if (rdoTongbo3.Checked == true)
                {
                    strTongbo = "3";
                }

                sp.Spread_All_Clear(SS1);
                SS1.ActiveSheet.RowCount = 20;

                //최근 판정자중 응급2차 대상자를 찾음
                List<HIC_JEPSU_LTD_EXJONG> list = hicJepsuLtdExjongService.GetItembyGjJong(strGjJong, nLtdCode, strTongbo);

                nREAD = list.Count;
                SS1.ActiveSheet.RowCount = nREAD;
                progressBar1.Maximum = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    nWRTNO = list[i].WRTNO;
                    //판정일자를 읽음
                    strPanDate = "";
                    HIC_RES_BOHUM1 list2 = hicResBohum1Service.GetItemByWrtno(nWRTNO);

                    if (!list2.IsNullOrEmpty())
                    {
                        strPanDate = list2.PANJENGDATE;
                    }
                    else
                    {
                        strPanDate = "";
                    }

                    if (strPanDate.IsNullOrEmpty())
                    {
                        HIC_RES_SPECIAL list3 = hicResSpecialService.GetPanjengDatebyWrtNo(nWRTNO);

                        if (!list3.IsNullOrEmpty())
                        {
                            strPanDate = list3.PANJENGDATE.ToString();
                        }
                        else
                        {
                            strPanDate = "";
                        }
                    }

                    SS1.ActiveSheet.Cells[i, 0].Text = strPanDate;
                    SS1.ActiveSheet.Cells[i, 1].Text = nWRTNO.ToString();
                    SS1.ActiveSheet.Cells[i, 2].Text = list[i].SNAME;
                    SS1.ActiveSheet.Cells[i, 3].Text = list[i].LTDNAME;
                    SS1.ActiveSheet.Cells[i, 4].Text = list[i].EXNAME;
                    SS1.ActiveSheet.Cells[i, 5].Text = list[i].JEPDATE;
                    SS1.ActiveSheet.Cells[i, 7].Text = list[i].ERTONGBO;
                    SS1.ActiveSheet.Cells[i, 8].Text = hm.UCode_Names_Display(list[i].UCODES);
                    SS1.ActiveSheet.Cells[i, 9].Text = hm.SExam_Names_Display(list[i].SEXAMS);

                    progressBar1.Value = i + 1;
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

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                string strWrtNo = "";

                if (e.Column == 5)
                {
                    strWrtNo = SS1.ActiveSheet.Cells[e.Row, 1].Text.Trim();
                    FrmHcPanExamResultRegChg = new frmHcPanExamResultRegChg(long.Parse(strWrtNo), "", "");
                    FrmHcPanExamResultRegChg.ShowDialog(this);
                }
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
    }
}
