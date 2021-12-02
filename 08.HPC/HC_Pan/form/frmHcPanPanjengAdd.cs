using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Pan
/// File Name       : frmHcPanPanjengAdd.cs
/// Description     : 판정물질 추가/삭제
/// Author          : 이상훈
/// Create Date     : 2019-12-27
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmPanjengAdd.frm(HcPan09)" />

namespace HC_Pan
{
    public partial class frmHcPanPanjengAdd : Form
    {
        HicSpcPanjengJepsuService hicSpcPanjengJepsuService = null;
        HicJepsuService hicJepsuService = null;
        HicMcodeService hicMcodeService = null;
        HicSpcPanjengService hicSpcPanjengService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        long FnWrtNo;

        public frmHcPanPanjengAdd(long nWrtNo)
        {
            InitializeComponent();
            FnWrtNo = nWrtNo;
            SetEvent();
        }

        void SetEvent()
        {
            hicSpcPanjengJepsuService = new HicSpcPanjengJepsuService();
            hicJepsuService = new HicJepsuService();
            hicMcodeService = new HicMcodeService();
            hicSpcPanjengService = new HicSpcPanjengService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.SSPan.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.txtUCodeName.KeyDown += new KeyEventHandler(eTxtKeyDown);
        }

        private void eTxtKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                fn_Screen_Display(txtUCodeName.Text.Trim());
            }
        }

        void eFormLoad(object sender, EventArgs e)
        {
            string strData = "";
            string strUCodes = "";
            //string strUSQL = "";
            List<string> strUSQL = new List<string>();

            sp.Spread_All_Clear(ssList);

            if (FnWrtNo.IsNullOrEmpty())
            {
                MessageBox.Show("선택 된 수검자가 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SSPan_Sheet1.ColumnHeader.Rows.Get(-1).Height = 26F;
            SSPan_Sheet1.Columns.Get(10).Visible = false;   //코드
            SSPan_Sheet1.Columns.Get(12).Visible = false;   //ROWID

            //유해인자를 Display
            strUCodes = hicJepsuService.GetUcodesbyWrtNo(FnWrtNo);

            lblUCodes.Text = hm.UCode_Names_Display(strUCodes);

            strUSQL.Clear();
            for (int i = 0; i < VB.I(strUCodes, ","); i++)
            {
                if (!VB.Pstr(strUCodes, ",", i).IsNullOrEmpty())
                {
                    strUSQL.Add(VB.Pstr(strUCodes, ",", i));
                }
            }

            //취급물질을 List1에 SET
            if (!strUSQL.IsNullOrEmpty())
            {
                List<HIC_MCODE> list = hicMcodeService.GetCodeNamebyCode(strUSQL);
                ssList.ActiveSheet.RowCount = list.Count;
                for (int i = 0; i < list.Count; i++)
                {
                    ssList.ActiveSheet.Cells[i, 0].Text = list[i].CODE;
                    ssList.ActiveSheet.Cells[i, 1].Text = list[i].NAME;
                }
            }

            //나머지 모든 물질을 ADD
            List<HIC_MCODE> list2 = hicMcodeService.GetCodeNamebyNotInCode(strUSQL);
            ssList.ActiveSheet.RowCount = list2.Count;
            for (int i = 0; i < list2.Count; i++)
            {
                ssList.ActiveSheet.Cells[i, 0].Text = list2[i].CODE;
                ssList.ActiveSheet.Cells[i, 1].Text = list2[i].NAME;
            }

            fn_Screen_Display_Panjeng();

            ComFunc.ReadSysDate(clsDB.DbCon);

        }

        void eBtnClick(object sender, EventArgs e)
        {
            int result = 0;

            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSave)
            {
                string strDel = "";
                string strCode = "";
                string strRowId = "";
                string strMCodes = "";
                int nMCnt = 0;

                for (int i = 0; i < SSPan.ActiveSheet.RowCount; i++)
                {
                    strDel = SSPan.ActiveSheet.Cells[i, 0].Text.Trim();
                    strCode = SSPan.ActiveSheet.Cells[i, 10].Text.Trim();    //코드
                    if (strDel != "True")
                    {
                        if (VB.InStr(strMCodes, strCode + ",") > 0)
                        {
                            MessageBox.Show(i + "번줄 취급물질이 중복되어 저장이 불가능합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        else
                        {
                            strMCodes += strCode + ",";
                        }
                    }
                }

                clsDB.setBeginTran(clsDB.DbCon);

                if (lblUCodes.Text.IsNullOrEmpty())
                {
                    nMCnt = 0;
                    for (int i = 0; i < SSPan.ActiveSheet.RowCount; i++)
                    {
                        strDel = SSPan.ActiveSheet.Cells[i, 0].Text.Trim();
                        strCode = SSPan.ActiveSheet.Cells[i, 10].Text.Trim();// 코드
                        if (strDel != "True")
                        {
                            nMCnt += 1;
                        }
                    }

                    if (nMCnt > 0)
                    {
                        result = hicJepsuService.UpdateUcodesbyWrtNo(FnWrtNo);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("유해인자 갱신중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                for (int i = 0; i < SSPan.ActiveSheet.RowCount; i++)
                {
                    strDel = SSPan.ActiveSheet.Cells[i, 0].Text.Trim();
                    strCode = SSPan.ActiveSheet.Cells[i, 10].Text.Trim();   //코드
                    strRowId = SSPan.ActiveSheet.Cells[i, 12].Text.Trim();
                    if (!strRowId.IsNullOrEmpty())
                    {
                        if (strDel == "True")
                        {
                            result = hicSpcPanjengService.UpdateDelDatebyRowId(strRowId);

                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                MessageBox.Show("특수검진 취급물질별 판정내역 삭제일자 갱신중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (strDel != "True")
                        {
                            result = hicSpcPanjengService.SelectInsert(strCode, FnWrtNo);

                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                MessageBox.Show("특수검진 취급물질별 판정내역 저장중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                this.Close();
            }
            else if (sender == btnSearch)
            {
                fn_Screen_Display(txtUCodeName.Text.Trim());
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == ssList)
            {
                string strCode = "";
                string strName = "";

                strCode = ssList.ActiveSheet.Cells[e.Row, 0].Text.Trim();
                strName = ssList.ActiveSheet.Cells[e.Row, 1].Text.Trim();

                if (strCode.IsNullOrEmpty())
                {
                    return;
                }

                SSPan.ActiveSheet.RowCount += 1;
                SSPan.ActiveSheet.Cells[SSPan.ActiveSheet.RowCount - 1, 1].Text = strName;
                SSPan.ActiveSheet.Cells[SSPan.ActiveSheet.RowCount - 1, 10].Text = strCode;
                SSPan.ActiveSheet.Cells[SSPan.ActiveSheet.RowCount - 1, 12].Text = "";
            }
        }

        void fn_Screen_Display(string argName)
        {
            if (txtUCodeName.Text.Trim() == "") { return; }

            //취급물질을 List1에 SET
            List<HIC_MCODE> list = hicMcodeService.GetItemByLikeName(txtUCodeName.Text.Trim());
            ssList.ActiveSheet.RowCount = list.Count;
            for (int i = 0; i < list.Count; i++)
            {
                ssList.ActiveSheet.Cells[i, 0].Text = list[i].CODE;
                ssList.ActiveSheet.Cells[i, 1].Text = list[i].NAME;
            }
        }

        void fn_Screen_Display_Panjeng()
        {
            int nRead = 0;

            //판정항목(취급물질)을 Display
            IList<HIC_SPC_PANJENG> list = hicSpcPanjengService.Read_Spc_Panjeng(FnWrtNo);

            nRead = list.Count;
            SSPan.ActiveSheet.RowCount = nRead;
            for (int i = 0; i < nRead; i++)
            {
                SSPan.ActiveSheet.Cells[i, 1].Text = " " + hb.READ_MCode_Name(list[i].MCODE);
                SSPan.ActiveSheet.Cells[i, 2].Text = "";
                if (!list[i].PANJENGDATE.To<string>("").IsNullOrEmpty())
                {
                    SSPan.ActiveSheet.Cells[i, 2].Text = "◎";
                }
                SSPan.ActiveSheet.Cells[i, 3].Text = list[i].PANJENG;
                SSPan.ActiveSheet.Cells[i, 4].Text = list[i].SOGENCODE;
                SSPan.ActiveSheet.Cells[i, 5].Text = list[i].JOCHICODE;
                SSPan.ActiveSheet.Cells[i, 6].Text = list[i].SAHUCODE;
                SSPan.ActiveSheet.Cells[i, 7].Text = list[i].WORKYN;
                SSPan.ActiveSheet.Cells[i, 8].Text = list[i].SOGENREMARK;
                SSPan.ActiveSheet.Cells[i, 9].Text = list[i].JOCHIREMARK;
                SSPan.ActiveSheet.Cells[i, 10].Text = list[i].MCODE;
                SSPan.ActiveSheet.Cells[i, 12].Text = list[i].RID;
            }
        }
    }
}
