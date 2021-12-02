using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Bill
/// File Name       : frmHcHemsHazardousFactorCode.cs
/// Description     : HEMS 유해인자별 검사항목코드
/// Author          : 이상훈
/// Create Date     : 2021-02-05
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmHems_코드3.frm(FrmHems_코드3)" />

namespace HC_Bill
{
    public partial class frmHcHemsHazardousFactorCode : Form
    {
        HicCodeService hicCodeService = null;
        HicHemsCodeService hicHemsCodeService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();
        clsHcBill chb = new clsHcBill();
        clsHcType hct = new clsHcType();
        clsPrint CP = new clsPrint();

        /// TODO : 이상훈 (2021.02.08)
        //clsHcHems cHems = new clsHcHems();

        public frmHcHemsHazardousFactorCode()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            hicCodeService = new HicCodeService();
            hicHemsCodeService = new HicHemsCodeService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);

            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            int nRow = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);
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
                fn_ReadCode();
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                string strCode = "";

                sp.Spread_All_Clear(SS2);
                sp.Spread_All_Clear(SS3);

                strCode = SS1.ActiveSheet.Cells[e.Row, 4].Text;
                fn_ReadPromiseCode(strCode);
                fn_ReadExamCode(strCode);
            }
        }

        /// <summary>
        /// 인자코드읽기
        /// </summary>
        void fn_ReadCode()
        {
            int nRead = 0;

            sp.Spread_All_Clear(SS1);

            List<HIC_CODE> list = hicCodeService.GetCodeNamebyGubun("55");  //인자별 취급물질 코드

            nRead = list.Count;
            SS1.ActiveSheet.RowCount = nRead;
            for (int i = 0; i < nRead; i++)
            {
                SS1.ActiveSheet.Cells[i, 0].Text = list[i].CODE;
                SS1.ActiveSheet.Cells[i, 1].Text = list[i].NAME;

                /// TODO : 이상훈 (2021.02.08) HcHems.bas 파일 컨버전 후 적용할것
                //SS1.ActiveSheet.Cells[i, 2].Text = cHems.HIC_READ_HEMS_CODE("01", "1", "2", list[i].GCODE);   //인자코드
                //SS1.ActiveSheet.Cells[i, 3].Text = cHems.HIC_READ_HEMS_CODE("01", "1", "2", "02", "1", "5", cHems.HIC_READ_HEMS_CODE("01", "1", "2", list[i].GCODE));  //인자명칭

                SS1.ActiveSheet.Cells[i, 4].Text = list[i].GCODE;
                //SS1.ActiveSheet.Cells[i, 5].Text = cHems.HIC_READ_HEMS_CODE("01", "1", "5", list[i].GCODE);

                //SS1.ActiveSheet.Cells[i, 6].Text = cHems.약속코드읽기2(list[i].GCODE);
                //SS1.ActiveSheet.Cells[i, 7].Text = cHems.약속코드읽기2(list[i].GCODE);
            }
        }

        /// <summary>
        /// 약속코드읽기
        /// </summary>
        /// <param name="argCode"></param>
        void fn_ReadPromiseCode(string argCode)
        {
            int nRead = 0;

            sp.Spread_All_Clear(SS3);

            List<HIC_HEMS_CODE> list = hicHemsCodeService.GetCode2byGubunCode1("04", argCode);

            nRead = list.Count;
            SS3.ActiveSheet.RowCount = nRead;
            for (int i = 0; i < nRead; i++)
            {
                SS3.ActiveSheet.Cells[i, 0].Text = list[i].CODE2;
                /// TODO : 이상훈 (2021.02.08) HcHems.bas 파일 컨버전 후 적용할것
                //SS3.ActiveSheet.Cells[i, 1].Text = cHems.HIC_READ_HEMS_CODE("05", "1", "5", list[i].CODE2);
            }
        }

        /// <summary>
        ///  검사코드읽기
        /// </summary>
        /// <param name="argCode"></param>
        void fn_ReadExamCode(string argCode)
        {
            int nRead = 0;

            sp.Spread_All_Clear(SS2);

            List<HIC_HEMS_CODE> list = hicHemsCodeService.GetCode2byGubunCode1("03", argCode);

            nRead = list.Count;
            SS3.ActiveSheet.RowCount = nRead;
            for (int i = 0; i < nRead; i++)
            {
                /// TODO : 이상훈 (2021.02.08) HcHems.bas 파일 컨버전 후 적용할것
                //SS3.ActiveSheet.Cells[i, 0].Text = cHems.병원검사코드읽기(list[i].CODE2);
                SS3.ActiveSheet.Cells[i, 1].Text = list[i].CODE2;
                //SS3.ActiveSheet.Cells[i, 2].Text = cHems.HIC_READ_HEMS_CODE("06", "1", "5", list[i].CODE2);
            }
        }
    }
}
