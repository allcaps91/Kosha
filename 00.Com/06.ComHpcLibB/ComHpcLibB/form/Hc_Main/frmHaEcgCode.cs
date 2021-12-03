using ComBase;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHaEcgCode.cs
/// Description     : ECG코드
/// Author          : 이상훈
/// Create Date     : 2019-09-30
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "HaMain08.frm(FrmECGCode)" />

namespace ComHpcLibB
{
    public partial class frmHaEcgCode : Form
    {
        BasBcodeEcgService basBcodeEcgService = null;

        clsSpread sp = new clsSpread();
        clsHaBase hb = new clsHaBase();
        clsHcMain hm = new clsHcMain();
        ComFunc cf = new ComFunc();

        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        public frmHaEcgCode()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            basBcodeEcgService = new BasBcodeEcgService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            eBtnClick(btnSave, new EventArgs());
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                rEventClosed();
            }
            else if(sender == btnSave)
            {
                string strROWID = "";
                string strCODE = "";
                string strName = "";
                string strDel = "";

                int result = 0;

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strDel = SS1.ActiveSheet.Cells[i, 0].Text;
                    strCODE = SS1.ActiveSheet.Cells[i, 1].Text;
                    strName = SS1.ActiveSheet.Cells[i, 2].Text;
                    strROWID = SS1.ActiveSheet.Cells[i, 3].Text;

                    if (strROWID != "")
                    {
                        if (strDel == "True")
                        {
                            result = basBcodeEcgService.DeletebyRowId(strROWID);
                        }
                        else
                        {
                            result = basBcodeEcgService.UPdate(strCODE, strName, strROWID);
                        }

                        if (result < 0)
                        {
                            MessageBox.Show("등록오류!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        result = basBcodeEcgService.InsertAll(strCODE, strName);
                    }
                }
                eBtnClick(btnSearch, new EventArgs());
            }
            else if (sender == btnSearch)
            {
                List<BAS_BCODE_ECG> list = basBcodeEcgService.GetItemAll();

                SS1.ActiveSheet.RowCount = list.Count;

                for (int i = 0; i < list.Count; i++)
                {
                    SS1.ActiveSheet.Cells[i, 1].Text = list[i].CODE;
                    SS1.ActiveSheet.Cells[i, 2].Text = list[i].NAME;
                    SS1.ActiveSheet.Cells[i, 3].Text = list[i].ROWID.Trim();
                }
            }
        }
    }
}
