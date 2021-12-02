using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHcDaeSangView.cs
/// Description     : 가접수 명단
/// Author          : 이상훈
/// Create Date     : 2020-06-26
/// Update History  : 
/// </summary>
/// <seealso cref= "FrmDaeSangView(HaMain75.frm)" />

namespace ComHpcLibB
{
    public partial class frmHcDaeSangView : Form
    {
        HicJepsuWorkPatientHeaJepsuService hicJepsuWorkPatientHeaJepsuService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hc = new clsHcFunc();
        ComFunc cf = new ComFunc();

        public delegate void SetGstrValue(HIC_JEPSU_WORK_PATIENT_HEA_JEPSU GstrValue);
        public static event SetGstrValue rSetGstrValue;

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        public frmHcDaeSangView()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicJepsuWorkPatientHeaJepsuService = new HicJepsuWorkPatientHeaJepsuService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtSName.KeyPress += new KeyPressEventHandler(eKeyPress);
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();

            SS1.Initialize();
            SS1.ActiveSheet.ColumnHeader.Rows[0].Height = 30;
            SS1.AddColumn("수검자명", nameof(HIC_JEPSU_WORK_PATIENT_HEA_JEPSU.SNAME),    84, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsSort = true });
            SS1.AddColumn("주민번호", nameof(HIC_JEPSU_WORK_PATIENT_HEA_JEPSU.JUMINNO), 120, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("사업장명", nameof(HIC_JEPSU_WORK_PATIENT_HEA_JEPSU.LTDCODE),  48, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsSort = true, Aligen = CellHorizontalAlignment.Left, IsVisivle = false });
            SS1.AddColumn("사업장명", nameof(HIC_JEPSU_WORK_PATIENT_HEA_JEPSU.LTDNAME),  84, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsSort = true, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("등록번호", nameof(HIC_JEPSU_WORK_PATIENT_HEA_JEPSU.PTNO),     92, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsSort = true });
            SS1.AddColumn("검진종류", nameof(HIC_JEPSU_WORK_PATIENT_HEA_JEPSU.GJJONG),   94, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsSort = true });
            SS1.AddColumn("접수일자", nameof(HIC_JEPSU_WORK_PATIENT_HEA_JEPSU.JEPDATE),  160, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsSort = true, dateTimeType = DateTimeType.YYYY_MM_DD });
            SS1.AddColumn("검진년도", nameof(HIC_JEPSU_WORK_PATIENT_HEA_JEPSU.GJYEAR),   42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsSort = true });

        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            //SS1_Sheet1.Columns.Get(3).Visible = false;
            //SS1_Sheet1.Columns.Get(4).Visible = false;
            //SS1_Sheet1.Columns.Get(5).Visible = false;
            SS1_Sheet1.Columns.Get(7).Visible = false;

            fn_Screen_Clear();
            if (!txtLtdCode.Text.IsNullOrEmpty())
            {
                eBtnClick(btnSearch, new EventArgs());
            }

            cboJong.Items.Clear();
            cboJong.Items.Add("00.전체");
            hb.ComboJong_AddItem(cboJong, false);

            SS1_Sheet1.ColumnHeader.Rows.Get(0).Height = 25F;
        }

        void fn_Screen_Clear()
        {
            txtLtdCode.Text = "";
            sp.Spread_All_Clear(SS1);
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Hide();
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
                //int nREAD = 0;
                string strLtdCode = "";
                string strLtdName = "";
                string strSname = "";
                string strGjjong = "";
                //List<HIC_JEPSU_WORK_PATIENT_HEA_JEPSU> listGaJepsu;

                sp.Spread_All_Clear(SS1);

                strLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1);
                strLtdName = VB.Pstr(txtLtdCode.Text, ".", 2);
                strSname = txtSName.Text;
                strGjjong = VB.Left(cboJong.Text, 2);

                //생활습관 누락 체크박스 제거 요청함
                //if (chkLifeHabit.Checked == true)
                //{
                //    List<HIC_JEPSU_WORK_PATIENT_HEA_JEPSU> list = hicJepsuWorkPatientHeaJepsuService.GetItembyGjJong("11");
                //    listGaJepsu = list;
                //}
                //else
                //{
                    List<HIC_JEPSU_WORK_PATIENT_HEA_JEPSU> list = hicJepsuWorkPatientHeaJepsuService.GetItembyLtdCodeSNameGjJong(strLtdCode, strSname, strGjjong);
                    //listGaJepsu = list;
                    SS1.DataSource = list;
                //}

                //nREAD = listGaJepsu.Count;
                //SS1.ActiveSheet.RowCount = nREAD;
                //for (int i = 0; i < nREAD; i++)
                //{
                //    SS1.ActiveSheet.Cells[i, 0].Text = listGaJepsu[i].SNAME;
                //    SS1.ActiveSheet.Cells[i, 1].Text = clsAES.DeAES(listGaJepsu[i].JUMINNO2);
                //    SS1.ActiveSheet.Cells[i, 2].Text = hb.READ_Ltd_Name(listGaJepsu[i].LTDCODE);
                //    SS1.ActiveSheet.Cells[i, 3].Text = listGaJepsu[i].PANO;
                //    SS1.ActiveSheet.Cells[i, 4].Text = listGaJepsu[i].GJJONG;
                //    SS1.ActiveSheet.Cells[i, 6].Text = listGaJepsu[i].GJYEAR;
                //}

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
            if (e.RowHeader || e.ColumnHeader)
            {
                return;
            }

            if (sender == SS1)
            {
                foreach (Form frm2 in Application.OpenForms) //떠있는지 체크
                {
                    if (frm2.Name == "frmHcJepMain")
                    {
                        HIC_JEPSU_WORK_PATIENT_HEA_JEPSU item = SS1.GetCurrentRowData() as HIC_JEPSU_WORK_PATIENT_HEA_JEPSU;

                        if (rSetGstrValue.IsNullOrEmpty())
                        {
                            this.Hide();
                            return;
                        }
                        else
                        {
                            rSetGstrValue(item);
                            this.Hide();
                            return;
                        }
                    }
                }
            }
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (sender == txtLtdCode)
                {
                    if (txtLtdCode.Text.Length >= 2)
                    {
                        eBtnClick(btnLtdCode, new EventArgs());
                    }

                    SendKeys.Send("{TAB}");
                }
                else if (sender == txtSName)
                {
                    btnSearch.Focus();
                }
            }
        }
    }
}
