using ComBase;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHaJilJong.cs
/// Description     : 질환종류등록
/// Author          : 이상훈
/// Create Date     : 2019-10-07
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "HaMain59.frm(FrmJilJong)" />

namespace ComHpcLibB
{
    public partial class frmHaJilJong : Form
    {
        HeaResultwardService heaResultwardService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();

        public frmHaJilJong()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            heaResultwardService = new HeaResultwardService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            int nRead = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);

            SS1_Sheet1.Columns.Get(3).Visible = false;

            //기존의 자료를 읽음
            List<HEA_RESULTWARD> list = heaResultwardService.GetCodeNameBySabunGubunJong(clsType.User.IdNumber, "01", "");

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

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSave)
            {
                string strCODE = "";
                string strWard = "";
                string strROWID = "";
                string strChk = "";
                int result = 0;

                for (int i = 0; i < 50; i++)
                {
                    strChk = "";
                    strCODE = "";
                    strWard = "";
                    strROWID = "";

                    if (strChk == "True")
                    {
                        SS1.ActiveSheet.Cells[i, 0].Text = "";
                    }
                    strCODE = VB.Left(SS1.ActiveSheet.Cells[i, 0].Text, 1);
                    strWard = SS1.ActiveSheet.Cells[i, 0].Text.Trim();
                    strROWID = SS1.ActiveSheet.Cells[i, 0].Text.Trim();
                    strWard = strWard.Replace("'", "`");

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
                            result = heaResultwardService.InsertCode(clsType.User.IdNumber, strCODE, strWard, "01");
                        }
                        else if (strROWID != "")
                        {
                            result = heaResultwardService.UpdateWardNamebyRowId(strWard, strROWID);
                        }
                    }
                }
                this.Close();
                return;
            }
        }
    }
}
