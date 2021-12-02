using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcSecondList.cs
/// Description     : 2차 대상자 명단 조회
/// Author          : 김민철
/// Create Date     : 2020-06-02
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmSecondList(HcMain104.frm)" />
namespace ComHpcLibB
{
    public partial class frmHcSecondList :Form
    {
        ComFunc   CF        = null;
        clsSpread cSpd      = null;
        HIC_LTD LtdHelpItem = null;

        HicJepsuPatientService hicJepsuPatientService = null;
        HicCodeService hicCodeService = null;

        public frmHcSecondList()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnLtdHelp.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.txtLtdName.KeyPress += new KeyPressEventHandler(eKeyPress);
        }

        private void SetControl()
        {
            CF = new ComFunc();
            cSpd = new clsSpread();
            LtdHelpItem = new HIC_LTD();
            hicJepsuPatientService = new HicJepsuPatientService();
            hicCodeService = new HicCodeService();

            cboJong.Items.Clear();
            cboJong.Items.Add("0.전체");
            cboJong.Items.Add("1.특수검진");
            cboJong.Items.Add("2.배치전");
            cboJong.Items.Add("3.채용및배치전");


            //cboJong.Items.Add("0.전체");
            //cboJong.Items.Add("1.일반검진");
            //cboJong.Items.Add("2.공무원");
            //cboJong.Items.Add("3.성인병");
            //cboJong.Items.Add("4.채용및배치전");
            //cboJong.Items.Add("5.생애");
            //cboJong.Items.Add("6.기타");

            #region SS1 Spread Set
            //SS1.Initialize(new SpreadOption { IsRowSelectColor = true, RowHeight = 20, RowHeaderVisible = true });
            SS1.Initialize(new SpreadOption { RowHeight = 20, RowHeaderVisible = true });
            SS1.AddColumn("회사명", nameof(HIC_JEPSU_PATIENT.LTDNAME), 160, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left, IsSort = true });
            SS1.AddColumn("수검자명", nameof(HIC_JEPSU_PATIENT.SNAME), 88, new SpreadCellTypeOption { IsSort = true });
            SS1.AddColumn("접수번호", nameof(HIC_JEPSU_PATIENT.WRTNO), 74, new SpreadCellTypeOption { IsSort = true });
            SS1.AddColumn("주민등록번호", nameof(HIC_JEPSU_PATIENT.JUMIN), 110, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("종검", nameof(HIC_JEPSU_PATIENT.JONGGUMYN), 44, new SpreadCellTypeOption { });
            SS1.AddColumn("검진종류", nameof(HIC_JEPSU_PATIENT.GJJONG), 47, new SpreadCellTypeOption { IsVisivle = false });
            SS1.AddColumn("검진명", nameof(HIC_JEPSU_PATIENT.GJNAME), 88, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left, IsSort = true });
            SS1.AddColumn("필요검사코드", nameof(HIC_JEPSU_PATIENT.SECOND_EXAMS), 88, new SpreadCellTypeOption { IsVisivle = false });
            SS1.AddColumn("필요검사", nameof(HIC_JEPSU_PATIENT.SECOND_EXAMS_NNM), 120, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left, IsSort = true });
            SS1.AddColumn("2차검진 사유", nameof(HIC_JEPSU_PATIENT.SECOND_SAYU), 160, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left, IsSort = true });
            SS1.AddColumn("미실시 사유", nameof(HIC_JEPSU_PATIENT.SECOND_MISAYU), 120, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left, IsSort = true });
            SS1.AddColumn("전화번호", nameof(HIC_JEPSU_PATIENT.TEL), 94, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("통보일자", nameof(HIC_JEPSU_PATIENT.SECOND_TONGBO), 75, new SpreadCellTypeOption { IsSort = true, isFilter = true });
            SS1.AddColumn("재검일자", nameof(HIC_JEPSU_PATIENT.SECOND_DATE), 75, new SpreadCellTypeOption { IsSort = true, isFilter = true });
            SS1.AddColumn("검진일자", nameof(HIC_JEPSU_PATIENT.JEPDATE), 75, new SpreadCellTypeOption { IsSort = true });
            SS1.AddColumn("등록번호", nameof(HIC_JEPSU_PATIENT.PTNO), 84, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("검진년도", nameof(HIC_JEPSU_PATIENT.GJYEAR), 62, new SpreadCellTypeOption { });
            SS1.AddColumn("검진반기", nameof(HIC_JEPSU_PATIENT.GJBANGI), 62, new SpreadCellTypeOption { });
            SS1.AddColumn("결과지출력일자", nameof(HIC_JEPSU_PATIENT.TONGBODATE), 130, new SpreadCellTypeOption { });
            #endregion
        }

        private void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtLtdName && e.KeyChar == (char)13)
            {
                if (!txtLtdName.Text.Trim().IsNullOrEmpty()) { Ltd_Code_Help(); }
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnLtdHelp)
            {
                Ltd_Code_Help();
            }
            else if (sender == btnSearch)
            {
                cSpd.Spread_Clear_Simple(SS1);
                Screen_Display();
            }
            else if (sender == btnPrint)
            {
                Spread_Print();
            }
        }

        /// <summary>
        /// 사업장 코드 검색창 연동
        /// </summary>
        private void Ltd_Code_Help()
        {
            string strFind = "";

            if (txtLtdName.Text.Contains("."))
            {
                strFind = VB.Pstr(txtLtdName.Text, ".", 2).Trim();
            }
            else
            {
                strFind = txtLtdName.Text.Trim();
            }

            frmHcLtdHelp frm = new frmHcLtdHelp(strFind);
            frm.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(ePost_value_LTD);
            frm.ShowDialog();

            if (!LtdHelpItem.IsNullOrEmpty() && LtdHelpItem.CODE > 0)
            {
                txtLtdName.Text = LtdHelpItem.CODE.To<string>();
                txtLtdName.Text = txtLtdName.Text + "." + LtdHelpItem.SANGHO;
            }
            else
            {
                txtLtdName.Text = "";
            }
        }

        private void ePost_value_LTD(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            cboJong.SelectedIndex = 0;
            dtpFDate.Text = CF.DATE_ADD(clsDB.DbCon, DateTime.Now.ToShortDateString(), -30);
            dtpTDate.Text = DateTime.Now.ToShortDateString();
        }

        private void Spread_Print()
        {
            string  strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "2차검진 대상자 명단";
            strHeader = cSpd.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += cSpd.setSpdPrint_String("조회기간: " + dtpFDate.Text + "~" + dtpTDate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += cSpd.setSpdPrint_String("검진종류: " + cboJong.Text.Trim(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += cSpd.setSpdPrint_String("사업장명: " + VB.Pstr(txtLtdName.Text, ".", 2), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += cSpd.setSpdPrint_String("인쇄시각: " + DateTime.Now.ToString() + " " + "  PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strFooter = cSpd.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            cSpd.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void Screen_Display()
        {
            string strSecExam = string.Empty;
            string strFDate = dtpFDate.Text;
            string strTDate = dtpTDate.Text;
            string strView = rdoGbn2.Checked == true ? "1" : rdoGbn3.Checked == true ? "2" : "";
            string strJong = cboJong.Text.Substring(0, 1);
            string strHea = chkHea.Checked == true ? "Y" : "";


            //if (chkHea.Checked) { strHea = "Y"; }

      List<HIC_JEPSU_PATIENT> list = hicJepsuPatientService.GetSecondListByTongboDate(strFDate, strTDate, strView, strJong, txtSName.Text.Trim(), VB.Pstr(txtLtdName.Text, ".", 1).To<long>(), strHea);

            if (list.Count > 0)
            {
                SS1.DataSource = list;
            }

            Cursor.Current = Cursors.WaitCursor;

            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                strSecExam = SS1.ActiveSheet.Cells[i,7].Text.Trim();  //재검
                if (!strSecExam.IsNullOrEmpty())
                {
                    SS1.ActiveSheet.Cells[i, 8].Text = READ_SECOND_Exams_Name(strSecExam);
                }
            }

            Cursor.Current = Cursors.Default;
        }

        private string READ_SECOND_Exams_Name(string argSecExam)
        {
            string strNames = string.Empty;
            string strCode = string.Empty;
            string stNew = string.Empty;

            for (int i = 1; i <= VB.L(argSecExam, ","); i++)
            {
                strCode = VB.Pstr(argSecExam, ",", i);

                if (strCode != "3" && strCode != "6")
                {
                    stNew = hicCodeService.GetGCode1ByGubunCode("53", strCode);
                    if (stNew.IsNullOrEmpty())
                    {
                        strNames += strCode + ",";
                    }
                    else
                    {
                        strNames += hicCodeService.GetGCode1ByGubunCode("53", strCode) + ",";
                    }
                }
            }

            return strNames;
        }
    }
}
