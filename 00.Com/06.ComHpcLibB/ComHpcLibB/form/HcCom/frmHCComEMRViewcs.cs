using ComBase;
using ComLibB;
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
using System.ComponentModel;
using ComBase.Controls;
using System.IO;
using System.Linq;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHCComEMRViewcs.cs
/// Description     : EMR 조회
/// Author          : 이상훈
/// Create Date     : 2020-11-23
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "신규" />

namespace ComHpcLibB
{
    public partial class frmHCComEMRViewcs : Form
    {
        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();


        public frmHCComEMRViewcs()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {   
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
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
        }
    }
}
