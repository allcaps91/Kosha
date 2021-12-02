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
/// File Name       : frmHcBillCofirmedListView.cs
/// Description     : 일반검진 확진자조회
/// Author          : 이상훈
/// Create Date     : 2021-02-02
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm확진자조회.frm(Frm확진자조회)" />

namespace HC_Bill
{
    public partial class frmHcBillCofirmedListView : Form
    {
        HicJepsuLtdResBohum1Service hicJepsuLtdResBohum1Service = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();
        clsHcBill chb = new clsHcBill();
        clsHcType hct = new clsHcType();
        clsPrint CP = new clsPrint();

        public frmHcBillCofirmedListView()
        {
            InitializeComponent();

            SetEvent();
        }

        void SetEvent()
        {
            hicJepsuLtdResBohum1Service = new HicJepsuLtdResBohum1Service();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnExcel.Click += new EventHandler(eBtnClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFDate.Text = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-7).ToShortDateString();
            dtpTDate.Text = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-1).ToShortDateString();
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnExcel)
            {
                bool x = false;
                string strDate = "";
                string strDir = "";
                string strMyName = "";
                string strMyPath1 = "";
                string strPathName = "";

                strMyPath1 = @"C:\청구작업\";

                Cursor.Current = Cursors.WaitCursor;
                SS1.ActiveSheet.Protect = false;

                strDate = VB.Left(dtpFDate.Text, 4) + VB.Mid(dtpFDate.Text, 6, 2) + VB.Right(dtpFDate.Text, 2) + "~" + VB.Left(dtpTDate.Text, 4) + VB.Mid(dtpTDate.Text, 6, 2) + VB.Right(dtpTDate.Text, 2);

                x = SS1.SaveExcel(strMyPath1 + strDate + "(확진대상명단).xlsx", FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders);
                {
                    if (x == true)
                    {
                        MessageBox.Show(" C:\\청구작업 폴더에 " + strDate + " 파일생성 완료되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("파일생성에 실패하였습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                SS1.ActiveSheet.Protect = true;
                Cursor.Current = Cursors.Default;
            }
            else if (sender == btnSearch)
            {
                long nWRTNO = 0;
                string strSExams = "";

                sp.Spread_All_Clear(SS1);
                SS1_Sheet1.Rows[-1].Height = 20;

                Cursor.Current = Cursors.WaitCursor;

                List<HIC_JEPSU_LTD_RES_BOHUM1> list = hicJepsuLtdResBohum1Service.GetItembyGjJongPanjengDate(dtpFDate.Text, dtpTDate.Text, "11");

                SS1.ActiveSheet.RowCount = list.Count;
                if (list.Count > 0)
                {
                    progressBar1.Maximum = list.Count;

                    for (int i = 0; i < list.Count; i++)
                    {
                        SS1.ActiveSheet.RowCount += 1;
                        SS1.ActiveSheet.Cells[i, 0].Text = (i + 1).To<string>();
                        SS1.ActiveSheet.Cells[i, 1].Text = list[i].GJYEAR;
                        SS1.ActiveSheet.Cells[i, 2].Text = clsAES.DeAES(list[i].JUMIN2);
                        SS1.ActiveSheet.Cells[i, 3].Text = list[i].SNAME;
                        SS1.ActiveSheet.Cells[i, 4].Text = list[i].JEPDATE;

                        if (list[i].PANJENGR3 == "1" && list[i].PANJENGR6 == "0")
                        {
                            SS1.ActiveSheet.Cells[i, 5].Text = "A";
                        }

                        if (list[i].PANJENGR3 == "0" && list[i].PANJENGR6 == "1")
                        {
                            SS1.ActiveSheet.Cells[i, 5].Text = "B";
                        }

                        if (list[i].PANJENGR3 == "1" && list[i].PANJENGR6 == "1")
                        {
                            SS1.ActiveSheet.Cells[i, 5].Text = "C";
                        }

                        progressBar1.Value = i + 1;
                    }
                }

                Cursor.Current = Cursors.Default;
            }
        }
    }
}
