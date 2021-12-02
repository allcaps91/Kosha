using ComBase;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHaUseWard.cs
/// Description     : 상용구 등록 및 조회
/// Author          : 이상훈
/// Create Date     : 2019-10-02
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "HaMain57.frm(FrmUseWard)" />

namespace ComHpcLibB
{
    public partial class frmHaUseWard : Form
    {
        HeaResultwardService heaResultwardService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();

        frmHaJilJong frmHaJilJong = null;

        public frmHaUseWard()
        {
            InitializeComponent();

            SetEvent();
        }

        void SetEvent()
        {
            heaResultwardService = new HeaResultwardService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            int nRow = 0;

            //기존의 자료를 읽음
            List<HEA_RESULTWARD> list = heaResultwardService.GetCodeNameBySabunGubun(clsType.User.IdNumber, "01");

            cboJong.Items.Clear();
            cboJong.Items.Add("*.전 체");
            for (int i = 0; i < list.Count; i++)
            {
                cboJong.Items.Add(list[i].CODE.Trim() + "." + list[i].WARDNAME.Trim());
            }

            SS1_Sheet1.Rows.Get(-1).Height = 24;
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
                sp.Spread_All_Clear(SS1);
                fn_Screen_Display();
            }
            else if (sender == btnSave)
            {
                string strCODE  = "";
                string strWard  = "";
                string strROWID = "";
                string strChk = "";
                int result = 0;

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strChk = "";
                    strCODE = "";
                    strWard = "";
                    strROWID = "";

                    strChk = SS1.ActiveSheet.Cells[i, 0].Text.Trim();

                    if (strChk == "True")
                    {
                        SS1.ActiveSheet.Cells[i, 0].Text = "";
                    }
                    strCODE = SS1.ActiveSheet.Cells[i, 1].Text.Trim();
                    strWard = SS1.ActiveSheet.Cells[i, 2].Text.Trim();
                    strROWID = SS1.ActiveSheet.Cells[i,30].Text.Trim();
                    strWard.Replace("'", "`");

                    if (strChk == "True")   //코드삭제
                    {
                        if (strROWID != "")
                        {
                            result = heaResultwardService.DeleteCode(strROWID);
                        }
                    }
                    else
                    {
                        if (strROWID == "" && strWard != "")
                        {
                            result = heaResultwardService.InsertCode(clsType.User.IdNumber, strCODE, strWard, "02");
                        }
                        else if (strROWID != "")
                        {
                            result = heaResultwardService.UpdateWardNamebyRowId(strWard, strROWID);
                        }
                    }
                }
                sp.Spread_All_Clear(SS1);
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

                strTitle = "종검판정 코드";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "  PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
            else if (sender == btnMenuJong)
            {
                frmHaJilJong f = new frmHaJilJong();
                f.ShowDialog();
            }
        }

        void fn_Screen_Display()
        {
            int nRow = 0;
            int nRead = 0;
            string strJong = "";

            SS1_Sheet1.Columns.Get(3).Visible = false;

            if (cboJong.Text.Trim() != "" && VB.Left(cboJong.Text, 1) != "*")
            {
                strJong = VB.Left(cboJong.Text, 1);
            }

            //기존의 자료를 읽음
            List<HEA_RESULTWARD> list = heaResultwardService.GetCodeNameBySabunGubunJong(clsType.User.IdNumber, "02", strJong);

            nRead = list.Count;
            if (nRead > 50)
            {
                SS1.ActiveSheet.RowCount = nRead;
                SS1_Sheet1.Rows.Get(-1).Height = 24;
            }

            if (nRead > 0)
            {
                for (int i = 0; i < nRead; i++)
                {
                    SS1.ActiveSheet.Cells[i, 1].Text = list[i].CODE.Trim();
                    SS1.ActiveSheet.Cells[i, 2].Text = list[i].WARDNAME.Trim();
                    SS1.ActiveSheet.Cells[i, 3].Text = list[i].ROWID.Trim();
                }
            }
        }
    }
}
