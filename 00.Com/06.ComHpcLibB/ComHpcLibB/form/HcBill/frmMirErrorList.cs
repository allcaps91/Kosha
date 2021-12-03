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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace ComHpcLibB
{
    public partial class frmMirErrorList : Form
    {
        //long nWRTNO = 0;
        string strFDate = string.Empty;
        string strTDate = string.Empty;
        string FstrProgID;
        bool FbChul = false;

        string strGubun = "";
        string strCHK = "";
        string strMEMO1 = "";
        string strMEMO2 = "";
        string strROWID = "";
        ComFunc cf = new ComFunc();
        clsSpread sp = new clsSpread();

        HicMirErrorTongboService hicMirErrorTongboService = null;

        public delegate void SetJepsuGstrValue(string strPtNo, string strJepDate);
        public static event SetJepsuGstrValue rSetJepsuGstrValue;


        public frmMirErrorList()
        {
            InitializeComponent();
            SetEvents();
            SetControl();
        }


        public frmMirErrorList(string strProgID)
        {
            InitializeComponent();
            SetEvents();
            SetControl();

            FstrProgID = strProgID;
        }

        private void SetControl()
        {
            hicMirErrorTongboService = new HicMirErrorTongboService();


            //SSList.Initialize();
            //SSList.ActiveSheet.ColumnHeader.Rows[0].Height = 30;
            ////SSList.AddColumn("접수번호", nameof(HIC_MIR_ERROR_TONGBO.WRTNO), 88, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsSort = true});
            //SSList.AddColumn("접수번호", nameof(HIC_MIR_ERROR_TONGBO.WRTNO), 88, FpSpreadCellType.TextCellType);
            //SSList.AddColumn("접수일자", nameof(HIC_MIR_ERROR_TONGBO.JEPDATE), 88, FpSpreadCellType.TextCellType);
            //SSList.AddColumn("성명", nameof(HIC_MIR_ERROR_TONGBO.SNAME), 70, FpSpreadCellType.TextCellType);
            //SSList.AddColumn("나이", nameof(HIC_MIR_ERROR_TONGBO.AGE), 70, FpSpreadCellType.TextCellType);
            //SSList.AddColumn("의뢰일자", nameof(HIC_MIR_ERROR_TONGBO.TONGBODATE), 88, FpSpreadCellType.TextCellType);
            //SSList.AddColumn("담당자", nameof(HIC_MIR_ERROR_TONGBO.DAMNAME), 70, FpSpreadCellType.TextCellType);
            //SSList.AddColumn("완료일자", nameof(HIC_MIR_ERROR_TONGBO.OKDATE), 88, FpSpreadCellType.TextCellType);
            //SSList.AddColumn("완료", nameof(HIC_MIR_ERROR_TONGBO.GBDEL), 40, FpSpreadCellType.CheckBoxCellType);
            //SSList.AddColumn("구분", nameof(HIC_MIR_ERROR_TONGBO.GUBUN), 40, FpSpreadCellType.TextCellType);
            //SSList.AddColumn("오류 상세 내역", nameof(HIC_MIR_ERROR_TONGBO.REMARK), 500, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            //SSList.AddColumn("메모창", nameof(HIC_MIR_ERROR_TONGBO.ACTMEMO), 88, FpSpreadCellType.TextCellType);
            //SSList.AddColumn("ROWID", nameof(HIC_MIR_ERROR_TONGBO.WRTNO), 88, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false});
            //SSList.AddColumn("메모", nameof(HIC_MIR_ERROR_TONGBO.ACTMEMO), 88, FpSpreadCellType.TextCellType,new SpreadCellTypeOption { IsVisivle = false});
            //SSList.AddColumn("접수", nameof(HIC_MIR_ERROR_TONGBO.WRTNO), 70, FpSpreadCellType.TextCellType,  new SpreadCellTypeOption { IsSort = true });
            //SSList.AddColumn("계측", nameof(HIC_MIR_ERROR_TONGBO.WRTNO), 70, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsSort = true });
            //SSList.AddColumn("등록번호", nameof(HIC_MIR_ERROR_TONGBO.PTNO), 70, FpSpreadCellType.TextCellType);
        }

        private void SetEvents()
        {
            this.Load += new EventHandler(eFormload);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);

            this.SSList.ButtonClicked += new EditorNotifyEventHandler(eSpdBtnClick);
            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
        }

        private void eFormload(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);
            dtpFDate.Text = cf.DATE_ADD(clsDB.DbCon, clsPublic.GstrSysDate, -90);
            dtpTDate.Text = clsPublic.GstrSysDate;

        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                Screen_Display(SSList);

            }
            else if (sender == btnSave)
            {
                for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
                {
                    strCHK = SSList.ActiveSheet.Cells[i, 7].Text.Trim();
                    strMEMO1 = SSList.ActiveSheet.Cells[i, 10].Text.Trim();
                    strMEMO2 = SSList.ActiveSheet.Cells[i, 12].Text.Trim();
                    strROWID = SSList.ActiveSheet.Cells[i, 11].Text.Trim();

                    if (strCHK == "True" || strMEMO1 != strMEMO2)
                    {
                        int result = hicMirErrorTongboService.UPDATE_OKDATE(strCHK, strMEMO1, strROWID, clsType.User.UserName);
                    }
                }
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

                strTitle = "청구오류명단";
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);
                sp.setSpdPrint(SSList, PrePrint, setMargin, setOption, strHeader, strFooter);
            }

            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
        }
        private void eSpdBtnClick(object sender, EditorNotifyEventArgs e)
        {
            if (sender == SSList)
            {

            }
        }


        void eSpdDClick(object sender, CellClickEventArgs e)
        {

            string strJepDate = "";
            string strPTNO = "";

            if (sender == SSList)
            {
                if (FstrProgID == "frmHcJepMain")
                {
                    strJepDate = SSList.ActiveSheet.Cells[e.Row, 1].Text;
                    strPTNO = SSList.ActiveSheet.Cells[e.Row, 14].Text;

                    if (!rSetJepsuGstrValue.IsNullOrEmpty())
                    {
                        rSetJepsuGstrValue(strPTNO, strJepDate);
                    }

                    //this.Close();
                    return;
                }
            }
        }

        //2020-03-28(테스트)
        private void Screen_Display(FpSpread Spd)
        {

            
            int nRow = 0;
            long nRead = 0;
            long nBuse = 0;
            long nWRTNO = 0;
            long nPrtSabun = 0;
            long nJepsuSabun = 0;
            string strOK = "";
            string strRemark = "";
            string strGjjong = "";
            string strSname = "";



            Spd.ActiveSheet.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            strFDate = dtpFDate.Value.ToShortDateString();
            strTDate = dtpTDate.Value.ToShortDateString();

            if(!txtWrtNo.Text.IsNullOrEmpty())
            {
                nWRTNO = Convert.ToInt32(txtWrtNo.Text);
            }
            
            if(!txtSName.Text.IsNullOrEmpty())
            {
                strSname = txtSName.Text.Trim();
            }
            
            if (rdoGUBUN1.Checked == true)
            {
                strGubun = "1";
            }
            else if (rdoGUBUN2.Checked == true)
            {
                strGubun = "2";
            }
            else if (rdoGUBUN3.Checked == true)
            {
                strGubun = "3";
            }

            List<HIC_MIR_ERROR_TONGBO> list = hicMirErrorTongboService.GetListByItems(nWRTNO, strFDate, strTDate, strGubun, strSname);
            //SSList.DataSource = list;

            SSList.ActiveSheet.RowCount = 0;

            for (int i = 0; i < list.Count; i++)
            {
                if (rdoBUSE1.Checked == true)
                {
                    strOK = "OK";
                }

                else
                {
                    strRemark = list[i].REMARK.Trim();
                    nBuse = 2;
                    if (VB.InStr(strRemark, "본인부담금") > 0) { nBuse = 1; }
                    if (VB.InStr(strRemark, "청구비용") > 0) { nBuse = 1; }
                    if (VB.InStr(strRemark, "차액발생") > 0) { nBuse = 1; }
                    if (VB.InStr(strRemark, "보건소") > 0) { nBuse = 1; }
                    if (VB.InStr(strRemark, "상담비") > 0 ) { nBuse = 1; }
                    if (VB.InStr(strRemark, "삭감됨") > 0 ) { nBuse = 1; }
                    if (VB.InStr(strRemark, "수면비") > 0 ) { nBuse = 1; }
                    if (VB.InStr(strRemark, "공휴 가산 코드") > 0 ) { nBuse = 1; }
                    if (VB.InStr(strRemark, "검진대상") > 0 ) { nBuse = 1; }
                    if (VB.InStr(strRemark, "본인") > 0 ) { nBuse = 1; }
                    if (VB.InStr(strRemark, "가산적용") > 0 ) { nBuse = 1; }
                    if (VB.InStr(strRemark, "대상") > 0 ) { nBuse = 1; }
                    if (VB.InStr(strRemark, "접수") > 0 ) { nBuse = 1; }
                    if (VB.InStr(strRemark, "부담") > 0 ) { nBuse = 1; }
                    if (VB.InStr(strRemark, "이중수검") > 0 ) { nBuse = 1; }
                    if (VB.InStr(strRemark, "자격") > 0 ) { nBuse = 1; }

                    strOK = "";
                    if (rdoBUSE2.Checked == true && nBuse == 1) { strOK = "OK"; }
                    if (rdoBUSE3.Checked == true && nBuse == 2) { strOK = "OK"; }
                }

                if (strOK == "OK")
                {
                    strGjjong = list[i].GJJONG.Trim();
                    nJepsuSabun = list[i].JOBSABUN;
                    nRow = nRow + 1;
                    if ( nRow > SSList.ActiveSheet.RowCount) { SSList.ActiveSheet.RowCount = nRow; }

                    SSList.ActiveSheet.Cells[nRow-1, 0].Text = list[i].WRTNO.ToString();
                    SSList.ActiveSheet.Cells[nRow-1, 1].Text = list[i].JEPDATE;
                    SSList.ActiveSheet.Cells[nRow-1, 2].Text = list[i].SNAME;
                    SSList.ActiveSheet.Cells[nRow-1, 3].Text = list[i].AGE.ToString() + "/" + list[i].SEX;
                    SSList.ActiveSheet.Cells[nRow-1, 4].Text = list[i].TONGBODATE;
                    SSList.ActiveSheet.Cells[nRow-1, 5].Text = list[i].DAMNAME;
                    SSList.ActiveSheet.Cells[nRow-1, 6].Text = list[i].OKDATE;
                    SSList.ActiveSheet.Cells[nRow-1, 7].Text = "";
                    SSList.ActiveSheet.Cells[nRow-1, 8].Text = list[i].GUBUN;
                    SSList.ActiveSheet.Cells[nRow-1, 9].Text = list[i].REMARK;
                    SSList.ActiveSheet.Cells[nRow-1, 10].Text = list[i].ACTMEMO;
                    SSList.ActiveSheet.Cells[nRow-1, 11].Text = list[i].ROWID;
                    SSList.ActiveSheet.Cells[nRow-1, 12].Text = list[i].ACTMEMO;
                    SSList.ActiveSheet.Cells[nRow-1, 13].Text = cf.Read_SabunName(clsDB.DbCon, list[i].JOBSABUN.To<string>());
                    SSList.ActiveSheet.Cells[nRow-1, 14].Text = cf.Read_SabunName(clsDB.DbCon, list[i].PRTSABUN.To<string>());
                    if (list[i].PRTSABUN == "0") { SSList.ActiveSheet.Cells[nRow - 1, 14].Text = ""; }
                    //SSList.ActiveSheet.Cells[nRow-1, 15].Text = list[i].PTNO;
                    //SSList.ActiveSheet.Cells[i, 14].Text = cf.Read_SabunName(clsDB.DbCon, list[i].PRTSABUN.To<string>());

                }
            }
        }




    }
}
