using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Spread;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcXrayChulList.cs
/// Description     : 출장검진 방사선명단출력 및 파일다운로드
/// Author          : 김경동
/// Create Date     : 2021-05-26
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm출장명단(Frm출장명단.frm)" />

namespace ComHpcLibB
{
    public partial class frmHcXrayChulList : Form
    {

        ComFunc CF = null;
        clsHaBase cHB = null;
        clsSpread cSpd = null;
        clsHcOrderSend hOrdSend = null;
        clsHcMain cHcMain = null;

        HIC_LTD LtdHelpItem = null;
        HicXrayResultWorkService hicXrayResultWorkService = null;
        BasPatientService basPatientService = null;

        public frmHcXrayChulList()
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
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.txtLtdName.KeyPress += new KeyPressEventHandler(eKeyPress);
        }

        private void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtLtdName && e.KeyChar == (char)13)
            {
                if (!txtLtdName.Text.Trim().IsNullOrEmpty()) { Ltd_Code_Help(); }
            }
        }

        private void SetControl()
        {
            CF = new ComFunc();
            cHB = new clsHaBase();
            cSpd = new clsSpread();
            hOrdSend = new clsHcOrderSend();
            cHcMain = new clsHcMain();
            LtdHelpItem = new HIC_LTD();
            hicXrayResultWorkService = new HicXrayResultWorkService();
            basPatientService = new BasPatientService();



            #region SS1 Spread Set
            SS1.Initialize(new SpreadOption { IsRowSelectColor = true, RowHeight = 20 });
            SS1.AddColumn("순번", "", 50, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("건진번호", nameof(HIC_XRAY_RESULT_WORK.PANO), 70, new SpreadCellTypeOption { IsEditble = false});
            SS1.AddColumn("이름", nameof(HIC_XRAY_RESULT_WORK.SNAME), 100, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("확인", "", 40, new SpreadCellTypeOption { IsEditble = false});
            SS1.AddColumn("촬영번호", nameof(HIC_XRAY_RESULT_WORK.XRAYNO), 130, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("촬영구분", nameof(HIC_XRAY_RESULT_WORK.GBREAD), 70, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("나이", nameof(HIC_XRAY_RESULT_WORK.AGE), 40, new SpreadCellTypeOption { IsEditble = false});
            SS1.AddColumn("성별", nameof(HIC_XRAY_RESULT_WORK.SEX), 40, new SpreadCellTypeOption { IsEditble = false});
            SS1.AddColumn("회사명", nameof(HIC_XRAY_RESULT_WORK.LTDNAME), 150, new SpreadCellTypeOption { IsEditble = false});
            SS1.AddColumn("비고", " ", 40, new SpreadCellTypeOption { IsEditble = false});
            #endregion
           
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            dtpDate.Text = CF.DATE_ADD(clsDB.DbCon, DateTime.Now.ToShortDateString(), 0);
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
                Screen_Display();
            }
            else if (sender == btnPrint)
            {
                Spread_Print();
            }
            else if ( sender == btnSave)
            {
                Spread_Save();
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

        private void Screen_Display()
        {
            string strNewLtd = "";
            string strOldLtd = "";
            string strDate = dtpDate.Text;
            string strLtdName = "";

            long nCNT = 0;
            long nLtdCode = VB.Pstr(txtLtdName.Text, ".", 1).To<long>();
            


            IList<HIC_XRAY_RESULT_WORK> list = hicXrayResultWorkService.GetChulListByItem(strDate, nLtdCode);

            SS1.ActiveSheet.RowCount = 0;

            if (list.Count > 0)
            {
                SS1.DataSource = list;

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strNewLtd = list[i].LTDCODE.ToString();
                    if (strNewLtd != strOldLtd)
                    {
                        nCNT = 0;
                        nCNT += 1;
                        SS1.ActiveSheet.Cells[i, 0].Text = nCNT.ToString();
                    }
                    else
                    {
                        nCNT += 1;
                        SS1.ActiveSheet.Cells[i, 0].Text = nCNT.ToString();
                    }

                    strOldLtd = list[i].LTDCODE.ToString();
                }
            }
            else
            {
                MessageBox.Show("접수된 자료가 없습니다.", "확인");
                return;
            }

        }
        private void Spread_Print()
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "출장 방사선 촬영자 명단";
            strHeader = cSpd.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += cSpd.setSpdPrint_String("출장일자: " + dtpDate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += cSpd.setSpdPrint_String("인쇄시각: " + DateTime.Now.ToString() + " " + "  PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strFooter = cSpd.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            cSpd.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void Spread_Save()
        {

            int nRow = 0;
            long X = 0;
            long nXrayCnt = 0;
            string strDir = "";
            string strMyPath1 = "";
            string strMyName = "";
            string strFileName = "";
            string strLtdName = "";
            string strREC = "";
            string strXrayno = "";
            string strLtdCode = "";
            string strJepDate = "";
            string strDate = "";
            string strPANO = "";
            string strPtNo = "";
            string strName = "";
            string strSex = "";
            string strAge = "";
            string strJong = "";
            string strExams = "";
            string strExCode = "";
            string strXrayName = "";
            string strDate1 = "";
            string strCHUL = "";
            string strXChk = "";
            string strJumin = "";

            DirectoryInfo Dir = new DirectoryInfo(@"c:\출장Xray");
            if (!Dir.Exists)
            {
                Dir.Create();
            }


            if (SS1.ActiveSheet.RowCount == 0)
            {
                MessageBox.Show("저장할 자료가 1건도 없습니다..", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            for (int i = 0; i <= SS1.ActiveSheet.RowCount-1; i++)
            {
                strPANO = SS1.ActiveSheet.Cells[i, 1].Text.Trim();
                strXrayno = VB.Replace(SS1.ActiveSheet.Cells[i, 4].Text, " ", "");
                strXChk = "";
                if (SS1.ActiveSheet.Cells[i, 5].Text.Trim() =="분진") { strXChk = "Y"; }

                List<HIC_XRAY_RESULT_WORK> list = hicXrayResultWorkService.GetListByItem(dtpDate.Text, strPANO);
                if (!list.IsNullOrEmpty())
                {
                    strPtNo = list[0].PTNO.Trim();
                    strLtdCode = list[0].LTDCODE.ToString();
                    strDate = dtpDate.Text;
                    strLtdName = CF.Read_Ltd_Name(clsDB.DbCon, strLtdCode);
                    strName = list[0].SNAME.Trim();
                    strSex = list[0].SEX.Trim();
                    strAge = VB.Format(list[0].AGE, "#0");
                    strJong = list[0].GJJONG.Trim();
                    strExCode = list[0].XCODE.Trim();
                    strXrayName = "Chest";
                    if( strXChk =="Y") { strXrayName = "Chest-dust"; }

                    strJumin = "";
                    BAS_PATIENT item =  basPatientService.GetItembyPano(strPtNo);
                    if (!item.IsNullOrEmpty())
                    {
                        strJumin = item.JUMIN1.Trim();
                    }

                    nRow = nRow + 1;
                    if (nRow > SS2.ActiveSheet.RowCount) { SS2.ActiveSheet.RowCount = nRow; }
                    SS2.ActiveSheet.Cells[nRow - 1, 0].Text = strPtNo;
                    SS2.ActiveSheet.Cells[nRow - 1, 1].Text = strLtdName;
                    SS2.ActiveSheet.Cells[nRow - 1, 2].Text = strXrayno;
                    SS2.ActiveSheet.Cells[nRow - 1, 3].Text = strName;
                    SS2.ActiveSheet.Cells[nRow - 1, 4].Text = strSex;
                    SS2.ActiveSheet.Cells[nRow - 1, 5].Text = strAge;
                    SS2.ActiveSheet.Cells[nRow - 1, 6].Text = strXrayName;
                    SS2.ActiveSheet.Cells[nRow - 1, 7].Text = strJumin;
                }

                

            }

            SS2.ActiveSheet.RowCount = nRow;
            strFileName = @"c:\출장Xray\XRAY_" + VB.Replace(dtpDate.Text,"-","") + ".xls";

            SS2.SaveExcel(strFileName, "");

            MessageBox.Show(@"C:\출장Xray 폴더에 파일생성 완료되었습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);



        }

    }
}
