using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Pan
/// File Name       : frmHcPanBHelp.cs
/// Description     : 판정B 소견 등록
/// Author          : 이상훈
/// Create Date     : 2019-11-22
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmBHelp.frm(HcPan105)" />

namespace HC_Pan
{
    public partial class frmHcPanBHelp : Form
    {
        HicCodeService hicCodeService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        public frmHcPanBHelp()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            hicCodeService = new HicCodeService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.SS1.Change += new ChangeEventHandler(eSpdChange);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            eBtnClick(btnSearch, new EventArgs());
        }

        void eBtnClick(object sender, EventArgs e)
        {
            int result = 0;

            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSearch)
            {
                sp.Spread_All_Clear(SS1);

                List<HIC_CODE> list = hicCodeService.GetItembyGubun("94", clsType.User.IdNumber);

                SS1.ActiveSheet.RowCount = list.Count + 20;
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        SS1.ActiveSheet.Cells[i, 1].Text = list[i].CODE;
                        SS1.ActiveSheet.Cells[i, 2].Text = list[i].NAME;
                        SS1.ActiveSheet.Cells[i, 4].Text = list[i].ROWID;
                    }
                }
            }
            else if (sender == btnSave)
            {
                string strDel = "";
                string strCODE = "";
                string strName = "";
                string strChange = "";
                string strROWID = "";

                clsDB.setBeginTran(clsDB.DbCon);
                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strDel = SS1.ActiveSheet.Cells[i, 0].Text;
                    strCODE = SS1.ActiveSheet.Cells[i, 1].Text;
                    strName = SS1.ActiveSheet.Cells[i, 2].Text;
                    strChange = SS1.ActiveSheet.Cells[i, 3].Text;
                    strROWID = SS1.ActiveSheet.Cells[i, 4].Text;

                    if (strROWID.IsNullOrEmpty())
                    {
                        if (strDel != "True")
                        {
                            result = hicCodeService.InsertCode("94", strCODE, strName, clsType.User.IdNumber);

                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                MessageBox.Show("자료를 등록중 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (strDel == "True")
                        {
                            result = hicCodeService.DeleteCode(strROWID);

                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                MessageBox.Show("자료를 등록중 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        else if (strChange == "Y")
                        {
                            result = hicCodeService.UpdateCodeName(strCODE, strName, strROWID);

                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                MessageBox.Show("자료를 등록중 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                eBtnClick(btnSearch, new EventArgs());
            }
            else if (sender == btnCancel)
            {
                sp.Spread_All_Clear(SS1);
                eBtnClick(btnSearch, new EventArgs());
            }
        }

        void eSpdChange(object sender, ChangeEventArgs e)
        {
            SS1.ActiveSheet.Cells[e.Row, 3].Text = "Y";
        }
    }
}
