using ComBase;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ComBase.Controls;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcAmHcAfterOpd.cs
/// Description     : 건강검진후 외래진료 명단
/// Author          : 이상훈
/// Create Date     : 2019-12-31
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm건진후외래.frm(Frm건진후외래)" />

namespace ComHpcLibB
{
    public partial class frmHcAmHcAfterOpd : Form
    {
        ComHpcLibBService comHpcLibBService = null;
        HicJepsuService hicJepsuService = null;

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();

        public frmHcAmHcAfterOpd()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            comHpcLibBService = new ComHpcLibBService();
            hicJepsuService = new HicJepsuService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFrDate.Text = clsPublic.GstrSysDate;
            dtpToDate.Text = clsPublic.GstrSysDate;

            //검진종류 SET
            cboJong.Items.Clear();
            cboJong.Items.Add("00.전체");
            hb.ComboJong_AddItem(cboJong);
            cboJong.SelectedIndex = 0;

            txtLtdCode.Text = "";
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

                Cursor.Current = Cursors.WaitCursor;

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "검진후 외래진료 대상 조회";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("검진회사:" + VB.Pstr(txtLtdCode.Text, ".", 2), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);

                Cursor.Current = Cursors.Default;
            }
            else if (sender == btnSearch)
            {
                int nREAD = 0;
                int nREAD2 = 0;
                int nRow = 0;

                string strFrDate = "";
                string strToDate = "";
                string strJong = "";
                long nLtdCode = 0;

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;
                if (txtLtdCode.Text.IsNullOrEmpty())
                {
                    nLtdCode = 0;
                }
                else
                {
                    nLtdCode = long.Parse(VB.Pstr(txtLtdCode.Text, ".", 1));
                }
                if (VB.Left(cboJong.Text, 2) != "00")
                {
                    strJong = VB.Left(cboJong.Text, 2);
                }

                sp.Spread_All_Clear(SS1);

                List<HIC_JEPSU> list = hicJepsuService.GetItembyJepDateGjJong(strFrDate, strToDate, nLtdCode, strJong);

                nREAD = list.Count;
                SS1.ActiveSheet.RowCount = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    List<COMHPC> list2 = comHpcLibBService.GetItembyOpdMaster(list[i].PTNO, list[i].JEPDATE);

                    nREAD2 = list2.Count;
                    if (nREAD2 > 0)
                    {
                        nRow += 1;
                        for (int j = 0; j < nREAD2; j++)
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].JEPDATE;
                            SS1.ActiveSheet.Cells[nRow - 1, 1].Text = hb.READ_GjJong_Name(list[i].GJJONG.Trim());
                            SS1.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].SNAME;
                            SS1.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].PANO.ToString();
                            SS1.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].WRTNO.ToString();
                            SS1.ActiveSheet.Cells[nRow - 1, 6].Text = list2[j].DEPTCODE;
                            SS1.ActiveSheet.Cells[nRow - 1, 7].Text = list[i].JDATE;
                            SS1.ActiveSheet.Cells[nRow - 1, 8].Text = list2[j].PTNO.ToString();
                        }
                    }
                }
                SS1.ActiveSheet.RowCount = nRow;
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtLtdCode)
            {
                if (e.KeyChar == 13)
                {
                    if (txtLtdCode.Text.Length >= 2)
                    {
                        eBtnClick(btnLtdCode, new EventArgs());
                    }
                }
            }
            else 
            {
                SendKeys.Send("{TAB}");
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
