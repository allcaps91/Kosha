using ComBase;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHcAutoPanExCode.cs
/// Description     : 판정 검사 코드 조회
/// Author          : 이상훈
/// Create Date     : 2019-09-05
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmAutoPanExCode.frm(FrmAutoPanExCode)" />

namespace HC_Main
{
    public partial class frmHcAutoPanExCode : Form
    {
        HicExcodeService hicExcodeService = null;

        clsSpread sp = new clsSpread();

        public delegate void SetGstrValue(HIC_EXCODE GstrValue);
        public event SetGstrValue rSetGstrValue;

        public frmHcAutoPanExCode()
        {
            InitializeComponent();

            SetEvent();
        }

        void SetEvent()
        {
            hicExcodeService = new HicExcodeService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnFind.Click += new EventHandler(eBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.txtFind.KeyPress += new KeyPressEventHandler(eKeyPress);
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSearch)
            {
                int nRead = 0;
                string strTemp = "";
                string strGubun = "";

                if (rdoGubun1.Checked == true)
                {
                    strGubun = "1";
                }
                else if (rdoGubun2.Checked == true)
                {
                    strGubun = "2";
                }

                strTemp = txtFind.Text.Trim();

                List<HIC_EXCODE> list = hicExcodeService.GetItembyFind(strTemp, strGubun);

                nRead = list.Count;
                if (nRead > 0)
                {
                    SS1.ActiveSheet.RowCount = nRead;
                    SS1.DataSource = list;
                }
            }
        }


        void eFormLoad(object sender, EventArgs e)
        {
            txtFind.Text = "";
            sp.Spread_All_Clear(SS1);
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                if (e.ColumnHeader == true)
                {
                    return;
                }

                if (SS1.ActiveSheet.Cells[e.Row, 0].Text.Trim() == "")
                {
                    MessageBox.Show("선택된 내용이 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                HIC_EXCODE item = new HIC_EXCODE();

                item.CODE = SS1.ActiveSheet.Cells[e.Row, 0].Text;
                item.HNAME = SS1.ActiveSheet.Cells[e.Row, 1].Text;
                item.UNIT = SS1.ActiveSheet.Cells[e.Row, 2].Text;

                rSetGstrValue(item);
            }
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtFind)
            {
                if (e.KeyChar == (char)13)
                {
                    eBtnClick(btnSearch, new EventArgs());
                }
            }
        }

    }
}
