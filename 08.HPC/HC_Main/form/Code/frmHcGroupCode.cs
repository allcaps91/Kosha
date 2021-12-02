using ComBase;
using System;
using System.Data;
using System.Windows.Forms;
using ComDbB;
using FarPoint.Win.Spread;
using System.Drawing;
using System.Collections.Generic;
using ComHpcLibB;
using ComHpcLibB.Model;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHcGroupCode.cs
/// Description     : 검진접수 그룹코드 선택창
/// Author          : 김민철
/// Create Date     : 2020-04-17
/// Comments        : 기존 그룹코드 선택창을 개선하여 통합된 그룹코드 목록을 보여줌
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmSAmtSelect(HcMain85.frm)" />
namespace HC_Main
{
    public partial class frmHcGroupCode : Form
    {
        public delegate void rSendMsg(List<READ_SUNAP_ITEM> argCode);
        public static event rSendMsg rSndMsg;

        clsSpread SPD = null;

        string FstrJong = string.Empty;
        string FstrKey = string.Empty; 

        List<READ_SUNAP_ITEM> FstrCodeslist = new List<READ_SUNAP_ITEM>();
        READ_SUNAP_ITEM rRSI = new READ_SUNAP_ITEM();

        HicGroupcodeService hicGroupcodeService = null;

        List<string> lstJong = null;

        public frmHcGroupCode()
        {
            InitializeComponent();
            SetEvent();
        }

        public frmHcGroupCode(string ArgJong, string ArgGubun, List<READ_SUNAP_ITEM> ArgCodes)
        {
            InitializeComponent();
            SetEvent();
            FstrJong = ArgJong;
            FstrKey = ArgGubun;
            FstrCodeslist = ArgCodes;
        }

        public frmHcGroupCode(List<string> ArgJong)
        {
            InitializeComponent();
            SetEvent();
            lstJong = new List<string>();
            lstJong = ArgJong;
        }

        void SetEvent()
        {
            SPD = new clsSpread();
            hicGroupcodeService = new HicGroupcodeService();


            this.Load                   += new EventHandler(eFormLoad);
            this.btnExit.Click          += new EventHandler(eBtnClick);
            this.btnSearch.Click        += new EventHandler(eBtnClick);
            this.btnSave.Click          += new EventHandler(eBtnClick);
            this.txtName.KeyPress       += new KeyPressEventHandler(eKeyPress);
            this.SS1.ButtonClicked      += new EditorNotifyEventHandler(eSpdBtnClick);
            this.SS1.CellDoubleClick    += new CellClickEventHandler(eSpdDblClick);
            this.SS1.CellClick          += new CellClickEventHandler(eSpdClick);
            this.rdoGbn1.CheckedChanged += new EventHandler(eRdoChange);
            this.rdoGbn2.CheckedChanged += new EventHandler(eRdoChange);
            this.rdoGbn3.CheckedChanged += new EventHandler(eRdoChange);
            this.rdoGbn9.CheckedChanged += new EventHandler(eRdoChange);
        }

        private void eSpdClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader)
            {
                SPD.setSpdSort(SS1, e.Column, true);
                return;
            }
        }

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            if (e.RowHeader || e.ColumnHeader) { return; }

            rRSI = new READ_SUNAP_ITEM();

            if (SS1.ActiveSheet.Cells[e.Row, 0].Text == "True")
            {
                SS1.ActiveSheet.Cells[e.Row, 0].Text = "";
                SS1.ActiveSheet.Cells[e.Row, 0, e.Row, SS1.ActiveSheet.ColumnCount - 1].ForeColor = Color.Black;

                if (FstrKey == "MCODE")
                {
                    rRSI.GRPCODE = SS1.ActiveSheet.Cells[e.Row, (int)clsHcMainSpd.enmHcSExam.GRPCD].Text.Trim();
                    rRSI.UCODE = SS1.ActiveSheet.Cells[e.Row, (int)clsHcMainSpd.enmHcSExam.CODE].Text.Trim();
                    HIC_GROUPCODE item = hicGroupcodeService.GetItemByCode(rRSI.GRPCODE);
                    rRSI.HANG = item.HANG;
                }
                else
                {
                    rRSI.GRPCODE = SS1.ActiveSheet.Cells[e.Row, (int)clsHcMainSpd.enmHcSExam.CODE].Text.Trim();
                    HIC_GROUPCODE item = hicGroupcodeService.GetItemByCode(rRSI.GRPCODE);
                    rRSI.HANG = item.HANG;
                }

                for (int i = 0; i < FstrCodeslist.Count; i++)
                {
                    if (FstrCodeslist[i].GRPCODE.Trim() == rRSI.GRPCODE.To<string>("").Trim())
                    {
                        FstrCodeslist.RemoveAt(i);
                    }
                }
            }
            else
            {
                SS1.ActiveSheet.Cells[e.Row, 0].Text = "True";
                SS1.ActiveSheet.Cells[e.Row, 0, e.Row, SS1.ActiveSheet.ColumnCount - 1].ForeColor = Color.Blue;

                if (FstrKey == "MCODE")
                {
                    rRSI.GRPCODE = SS1.ActiveSheet.Cells[e.Row, (int)clsHcMainSpd.enmHcSExam.GRPCD].Text.Trim();
                    rRSI.UCODE = SS1.ActiveSheet.Cells[e.Row, (int)clsHcMainSpd.enmHcSExam.CODE].Text.Trim();
                    HIC_GROUPCODE item = hicGroupcodeService.GetItemByCode(rRSI.GRPCODE);
                    rRSI.HANG = item.HANG;
                }
                else
                {
                    rRSI.GRPCODE = SS1.ActiveSheet.Cells[e.Row, (int)clsHcMainSpd.enmHcSExam.CODE].Text.Trim();
                    HIC_GROUPCODE item = hicGroupcodeService.GetItemByCode(rRSI.GRPCODE);
                    rRSI.HANG = item.HANG;
                }
                

                FstrCodeslist.Add(rRSI);
            }
        }

        void eRdoChange(object sender, EventArgs e)
        {
            string strSDName = ((RadioButton)sender).Name;
            string strJong = "";
            string strCode = string.Empty;

            if (strSDName.Substring(0, 6) == "rdoGbn")
            {
                if (((RadioButton)sender).Checked == true)
                {
                    switch (strSDName)
                    {
                        case "rdoGbn1": FstrKey = "";      strJong= FstrJong; break;
                        case "rdoGbn2": FstrKey = "I";     strJong= FstrJong; break;
                        case "rdoGbn3": FstrKey = "H";     strJong= FstrJong; break;
                        case "rdoGbn9": FstrKey = "MCODE"; strJong = "";      break; 
                        default: FstrKey = ""; break;
                    }

                    Screen_Display(clsDB.DbCon, FstrJong, FstrKey, txtName.Text.Trim());

                    for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                    {
                        strCode = SS1.ActiveSheet.Cells[i, 1].Text.Trim();

                        for (int j = 0; j < FstrCodeslist.Count; j++)
                        {
                            if (VB.L(strCode, FstrCodeslist[j].GRPCODE.To<string>("").Trim()) > 1)
                            {
                                SS1.ActiveSheet.Cells[i, 0].Text = "True";
                                SS1.ActiveSheet.Cells[i, 0, i, SS1.ActiveSheet.ColumnCount - 1].ForeColor = Color.Blue;
                            }

                            if (VB.L(strCode, FstrCodeslist[j].UCODE.To<string>("").Trim()) > 1)
                            {
                                SS1.ActiveSheet.Cells[i, 0].Text = "True";
                                SS1.ActiveSheet.Cells[i, 0, i, SS1.ActiveSheet.ColumnCount - 1].ForeColor = Color.Blue;
                            }
                        }
                    }

                    txtName.Focus();
                }
            }
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtName && e.KeyChar == (char)13)
            {
                Screen_Display(clsDB.DbCon, "", FstrKey, txtName.Text.Trim());
                btnSearch.Focus();
            }
        }

        void eSpdBtnClick(object sender, EditorNotifyEventArgs e)
        {
            if (e.Column >= 0 && e.Row >= 0)
            {
                if (e.Column == (int)clsHcMainSpd.enmHcSExam.GBSEL)
                {
                    clsSpread cSpd = new clsSpread();

                    rRSI = new READ_SUNAP_ITEM();

                    if (SS1.ActiveSheet.Cells[e.Row, (int)clsHcMainSpd.enmHcSExam.GBSEL].Text == "True")
                    {
                        cSpd.setSpdForeColor(SS1, e.Row, 0, e.Row, SS1_Sheet1.ColumnCount - 1, Color.Blue);

                        if (FstrKey == "MCODE")
                        {
                            rRSI.GRPCODE = SS1.ActiveSheet.Cells[e.Row, (int)clsHcMainSpd.enmHcSExam.GRPCD].Text.Trim();
                            rRSI.UCODE = SS1.ActiveSheet.Cells[e.Row, (int)clsHcMainSpd.enmHcSExam.CODE].Text.Trim();
                            HIC_GROUPCODE item = hicGroupcodeService.GetItemByCode(rRSI.GRPCODE);
                            rRSI.HANG = item.HANG;
                        }
                        else
                        {
                            rRSI.GRPCODE = SS1.ActiveSheet.Cells[e.Row, (int)clsHcMainSpd.enmHcSExam.CODE].Text.Trim();
                            HIC_GROUPCODE item = hicGroupcodeService.GetItemByCode(rRSI.GRPCODE);
                            rRSI.HANG = item.HANG;
                        }
                        
                        FstrCodeslist.Add(rRSI);
                    }
                    else
                    {
                        cSpd.setSpdForeColor(SS1, e.Row, 0, e.Row, SS1_Sheet1.ColumnCount - 1, Color.Black);

                        if (FstrKey == "MCODE")
                        {
                            rRSI.GRPCODE = SS1.ActiveSheet.Cells[e.Row, (int)clsHcMainSpd.enmHcSExam.GRPCD].Text.Trim();
                            rRSI.UCODE = SS1.ActiveSheet.Cells[e.Row, (int)clsHcMainSpd.enmHcSExam.CODE].Text.Trim();
                            HIC_GROUPCODE item = hicGroupcodeService.GetItemByCode(rRSI.GRPCODE);
                            rRSI.HANG = item.HANG;
                        }
                        else
                        {
                            rRSI.GRPCODE = SS1.ActiveSheet.Cells[e.Row, (int)clsHcMainSpd.enmHcSExam.CODE].Text.Trim();
                            HIC_GROUPCODE item = hicGroupcodeService.GetItemByCode(rRSI.GRPCODE);
                            rRSI.HANG = item.HANG;
                        }

                        for (int i = 0; i < FstrCodeslist.Count; i++)
                        {
                            if (FstrCodeslist[i].GRPCODE.Trim() == rRSI.GRPCODE.Trim())
                            {
                                FstrCodeslist.RemoveAt(i);
                            }
                        }
                    }

                    cSpd = null;
                }
            }
        }

        void eFormLoad(object sender, EventArgs e)
        {
            clsHcMainSpd cHM = new clsHcMainSpd();
            clsSpread cSPD = new clsSpread();

            string strJong = FstrJong;
            string strCode = string.Empty;

            cSPD.Spread_All_Clear(SS1);
            cHM.sSpd_enmHcSExam(SS1, cHM.sHcSExam, cHM.nHcSExam, 10, 0);

            cSPD = null;
            cHM = null;

            if (FstrKey != "") { strJong = ""; }

            if (FstrKey == "MCODE")
            {
                rdoGbn9.Checked = true;
            }
            else
            {
                Screen_Display(clsDB.DbCon, strJong, FstrKey, txtName.Text.Trim(), lstJong);

            }

            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                strCode = SS1.ActiveSheet.Cells[i, 1].Text.Trim();

                for (int j = 0; j < FstrCodeslist.Count; j++)
                {
                    if (VB.L(strCode, FstrCodeslist[j].GRPCODE.Trim()) > 1)
                    {
                        SS1.ActiveSheet.Cells[i, 0].Text = "True";
                        SS1.ActiveSheet.Cells[i, 0, i, SS1.ActiveSheet.ColumnCount - 1].ForeColor = Color.Blue;
                    }

                    if (VB.L(strCode, FstrCodeslist[j].UCODE.To<string>("").Trim()) > 1)
                    {
                        SS1.ActiveSheet.Cells[i, 0].Text = "True";
                        SS1.ActiveSheet.Cells[i, 0, i, SS1.ActiveSheet.ColumnCount - 1].ForeColor = Color.Blue;
                    }
                }
            }

        }

        void Screen_Display(PsmhDb pDbCon, string ArgJong, string ArgKey, string ArgName, List<string> lstJong = null)
        {
            DataTable dt = null;
            clsHcMainQuery cHMQ = new clsHcMainQuery();

            SS1.ActiveSheet.RowCount = 0;

            if (ArgKey == "MCODE")
            {
                dt = cHMQ.sel_HcGroupCode_MCode(pDbCon, ArgName);
            }
            else
            {
                dt = cHMQ.sel_HcGroupCode(pDbCon, ArgJong, ArgKey, ArgName, lstJong);
            }
            

            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("조회된 Data가 없습니다.");
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                return;
            }

            try
            {
                if (dt.Rows.Count > 0)
                {
                    SS1.ActiveSheet.RowCount = dt.Rows.Count;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SS1.ActiveSheet.Cells[i, (int)clsHcMainSpd.enmHcSExam.CODE].Text = dt.Rows[i]["CODE"].ToString().Trim();
                        SS1.ActiveSheet.Cells[i, (int)clsHcMainSpd.enmHcSExam.NAME].Text = dt.Rows[i]["NAME"].ToString().Trim();
                        SS1.ActiveSheet.Cells[i, (int)clsHcMainSpd.enmHcSExam.GBSELF].Text = dt.Rows[i]["GBSELF"].ToString().Trim();
                        SS1.ActiveSheet.Cells[i, (int)clsHcMainSpd.enmHcSExam.GRPCD].Text = dt.Rows[i]["GRPCD"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                SS1.ActiveSheet.Columns[0].AllowAutoSort = true;



            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);

                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
            }
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
                if (txtName.Text.Trim() != "")
                {
                    Screen_Display(clsDB.DbCon, "", FstrKey, txtName.Text.Trim());
                }
            }
            else if (sender == btnSave)
            {
                rSndMsg(FstrCodeslist);

                this.Close();
            }
        }
    }
}
