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
/// File Name       : frmHcBillCancerCountInpect.cs
/// Description     : 암검진 판독결과 건수 조회 및 점검
/// Author          : 이상훈
/// Create Date     : 2021-01-28
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "HcBilll05.frm(FrmChk01)" />

namespace HC_Bill
{
    public partial class frmHcBillCancerCountInpect : Form
    {
        HicMirCancerService hicMirCancerService = null;
        HicJepsuCancerNewService hicJepsuCancerNewService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();
        clsHcBill chb = new clsHcBill();
        clsHcType hct = new clsHcType();
        clsPrint CP = new clsPrint();

        public frmHcBillCancerCountInpect()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicMirCancerService = new HicMirCancerService();
            hicJepsuCancerNewService = new HicJepsuCancerNewService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);           
        }

        void SetControl()
        {

        }

        void eFormLoad(object sender, EventArgs e)
        {
            int nYY = 0;

            txtMirno.Text = "";
            ComFunc.ReadSysDate(clsDB.DbCon);

            nYY = VB.Left(clsPublic.GstrSysDate, 4).To<int>();
            cboYear.Items.Clear();
            for (int i = 1; i <= 6; i++)
            {
                cboYear.Items.Add(string.Format("{0:0000}", nYY));
                nYY -= 1;
            }

            sp.Spread_All_Clear(SS1);
            SS1_Sheet1.Rows[-1].Height = 20;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnPrint)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;
                string strChung = "";

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = " 청구대상자 " + strChung + "명단";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("출력일시: " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + "  PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);
                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
            else if (sender == btnSearch)
            {
                string strFrDate = "";
                string strToDate = "";
                int nRead = 0;
                int nSum = 0;

                sp.Spread_All_Clear(SS1);

                if (txtMirno.Text.Trim() == "")
                {
                    MessageBox.Show("청구번호가 공란입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                HIC_MIR_CANCER list = hicMirCancerService.GetFrDateToDatebyMirNo(txtMirno.Text.To<long>());

                if (!list.IsNullOrEmpty())
                {
                    strFrDate = list.FRDATE;
                    strToDate = list.TODATE;
                }
                else
                {
                    MessageBox.Show("청구번호 오류.. 해당청구번호가 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                List<HIC_JEPSU_CANCER_NEW> list2 = hicJepsuCancerNewService.GetItemWrtNoInbyJepDateMirNo(strFrDate, strToDate, txtMirno.Text.To<long>());

                nRead = list2.Count;
                SS1.ActiveSheet.RowCount = nRead;
                progressBar1.Maximum = nRead;
                for (int i = 0; i < nRead; i++)
                {
                    SS1.ActiveSheet.Cells[i, 0].Text = list2[i].SNAME;
                    SS1.ActiveSheet.Cells[i, 1].Text = list2[i].WRTNO.To<string>();
                    SS1.ActiveSheet.Cells[i, 2].Text = list2[i].JEPDATE;
                    SS1.ActiveSheet.Cells[i, 3].Text = list2[i].GBSTOMACH;          //위암구분
                    SS1.ActiveSheet.Cells[i, 4].Text = list2[i].GBRECTUM;           //대장암구분
                    SS1.ActiveSheet.Cells[i, 5].Text = list2[i].GBLIVER;            //간암구분
                    SS1.ActiveSheet.Cells[i, 6].Text = list2[i].GBBREAST;           //유방암구분
                    SS1.ActiveSheet.Cells[i, 7].Text = list2[i].GBWOMB;             //자궁경부암구분
                    SS1.ActiveSheet.Cells[i, 8].Text = list2[i].S_ANATGBN;          //위조직실시
                    SS1.ActiveSheet.Cells[i, 9].Text = list2[i].S_ANAT;             //위조직결과
                    SS1.ActiveSheet.Cells[i, 10].Text = list2[i].C_ANATGBN;         //대장조직실시
                    SS1.ActiveSheet.Cells[i, 11].Text = list2[i].C_ANAT;            //대장조직결과
                    SS1.ActiveSheet.Cells[i, 12].Text = list2[i].JINCHALGBN;        //진촬상담료

                    progressBar1.Value = i + 1;
                }

                SS1.ActiveSheet.RowCount = nRead + 1;
                SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, 0].Text = "- 총인원 -";
                SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, 1].Text = nRead.To<string>();
                SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, 2].Text = "합계";

                for (int i = 3; i <= 12; i++)
                {
                    for (int j = 0; j < SS1.ActiveSheet.RowCount; j++)
                    {
                       nSum +=  SS1.ActiveSheet.Cells[j, i].Text.To<int>();
                    }
                    SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, i].Text = nSum.To<string>();
                    nSum = 0;
                }
            }
        }
    }
}
