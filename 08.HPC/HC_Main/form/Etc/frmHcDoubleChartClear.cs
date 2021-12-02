using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHcDoubleChartClear.cs
/// Description     : 일반건진 이중챠트 수정정리
/// Author          : 김경동
/// Create Date     : 2021-07-22
/// Update History  : 
/// </summary>
/// <seealso cref= "Frm일반건진이중챠트정리(Frm일반건진이중챠트정리.frm)" />
namespace HC_Main
{
    public partial class frmHcDoubleChartClear : Form
    {
        HicPatientService hicPatientService = null;

        public frmHcDoubleChartClear()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }
        void SetEvent()
        {

            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);

        }
        void SetControl()
        {
            hicPatientService = new HicPatientService();
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

                txtPtno.Text = VB.Format(VB.Val(txtPtno.Text), "00000000");
                List <HIC_PATIENT> list = hicPatientService.GetPanobyItem(txtPtno.Text,"5");
                if(!list.IsNullOrEmpty())
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        SS1.ActiveSheet.Cells[i, 1].Text = list[i].PANO.ToString();
                        SS1.ActiveSheet.Cells[i, 2].Text = list[i].SNAME;
                        SS1.ActiveSheet.Cells[i, 3].Text = list[i].PTNO;
                        SS1.ActiveSheet.Cells[i, 4].Text = clsAES.DeAES(list[i].JUMIN2);
                    }
                    
                }
            }
            else if (sender == btnSave)
            {
                string strJumin2 = "";

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    if (SS1.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        strJumin2 = clsAES.AES(SS1.ActiveSheet.Cells[i, 4].Text);
                        hicPatientService.UpdateJumin2ByPtno(txtPtno.Text, strJumin2);
                    }
                }

                MessageBox.Show("저장완료!", "완료", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
        }

    }
}
