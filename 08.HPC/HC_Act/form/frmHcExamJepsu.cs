using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Act
/// File Name       : frmHcExamJepsu.cs
/// Description     : 건진센터 바코드 발행
/// Author          : 김민철
/// Create Date     : 2019-10-21
/// Update History  : 
/// </summary>
/// <history>  
/// 기존 FrmJepsu_Hic(ExHic01.frm) + FrmJepsu_Hic(ExHea05.frm)
/// </history>
/// <seealso cref= "ExHic01.frm(FrmAutoJepsu) + ExHea05.frm(FrmJepsu_Hic)" />
namespace HC_Act
{
    public partial class frmHcExamJepsu : Form
    {
        clsHcExam cHE = null;
        clsSpread cSPD = null;
        HeaJepsuService heaJepsuService = null;
        HicJepsuService hicJepsuService = null;
        ComHpcLibBService comHpcLibBService = null;
        ExamMasterService examMasterService = null;
        HicResultService hicResultService = null;
        ExamSpecmstService examSpecmstService = null;
        ExamResultcService examResultcService = null;

        string strJob = "";

        public frmHcExamJepsu()
        {
            InitializeComponent();
            SetEvent();
            SetConttrol();
        }

        private void SetConttrol()
        {
            cHE = new clsHcExam();
            cSPD = new clsSpread();
            heaJepsuService = new HeaJepsuService();
            hicJepsuService = new HicJepsuService();
            comHpcLibBService = new ComHpcLibBService();
            examMasterService = new ExamMasterService();
            hicResultService = new HicResultService();
            examSpecmstService = new ExamSpecmstService();
            examResultcService = new ExamResultcService();

            ssList.Initialize();
            ssList.ActiveSheet.ColumnHeader.Rows[0].Height = 30;
            ssList.AddColumn("접수번호", nameof(COMHPC.WRTNO),     84, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsSort = true });
            ssList.AddColumn("수검자명", nameof(COMHPC.SNAME),     44, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsSort = true });
            ssList.AddColumn("종류",     "",                       44, FpSpreadCellType.TextCellType);
            ssList.AddColumn("상태",     nameof(COMHPC.GBSTS_NM),  56, FpSpreadCellType.TextCellType);
            ssList.AddColumn("접수일자", nameof(COMHPC.SDATE),     74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsSort = true });
            ssList.AddColumn("등록번호", nameof(COMHPC.PTNO),      84, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsSort = true });
            ssList.AddColumn("종검유무", nameof(COMHPC.JONGGUMYN), 44, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsSort = true });
            ssList.AddColumn("검진종류", "",                       44, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });

            SS2.Initialize();
            SS2.ActiveSheet.ColumnHeader.Rows[0].Height = 30;
            SS2.AddColumn("검진코드",  "",  68, FpSpreadCellType.TextCellType);
            SS2.AddColumn("검사코드",  "",  68, FpSpreadCellType.TextCellType);
            SS2.AddColumn("검사명",    "", 180, FpSpreadCellType.TextCellType);
            SS2.AddColumn("검체",      "",  74, FpSpreadCellType.TextCellType);
            SS2.AddColumn("용기",      "",  74, FpSpreadCellType.TextCellType);
            SS2.AddColumn("검체코드",  "",  74, FpSpreadCellType.TextCellType);
            SS2.AddColumn("용기코드",  "",  74, FpSpreadCellType.TextCellType);
            SS2.AddColumn("발행여부",  "",  33, FpSpreadCellType.TextCellType);

        }

        private void SetEvent()
        {
            this.Load                   += new EventHandler(eFormload);
            this.btnExit.Click          += new EventHandler(eBtnClick);
            this.btnSearch.Click        += new EventHandler(eBtnClick);
            this.btnSave.Click          += new EventHandler(eBtnClick);
            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);
            this.rdoJong1.CheckedChanged += new EventHandler(eRdoChkEvent);
        }

        private void eRdoChkEvent(object sender, EventArgs e)
        {
            if (sender == rdoJob1)
            {
                if (rdoJob1.Checked)
                {
                    chkLDL.Visible = true;
                }
                else
                {
                    chkLDL.Visible = false;
                }
            }
        }

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            if (e.RowHeader || e.ColumnHeader) { return; }

            if (sender == ssList)
            {
                long nWrtno = ssList.ActiveSheet.Cells[e.Row, 0].Text.To<long>();
                string strDept = ssList.ActiveSheet.Cells[e.Row, 2].Text.Trim();
                string strBDate = ssList.ActiveSheet.Cells[e.Row, 4].Text.Trim();

                strJob = rdoJob1.Checked == true ? "HIC" : "HEA";

                //검사 상세항목을 Display
                Display_Exam_View(nWrtno, strDept, strBDate);
            }
        }

        private void Display_Exam_View(long nWrtno, string strDept, string strBDate)
        {
            string strPano = string.Empty;
            
            SS2.ActiveSheet.ClearRange(0, 0, SS2.ActiveSheet.Rows.Count, SS2.ActiveSheet.ColumnCount, true);
            SS2.ActiveSheet.Rows.Count = 0;

            clsHcType.TORD_CLEAR();

            //인적사항 READ
            HEA_JEPSU item1 = heaJepsuService.GetItembyWrtNo(nWrtno);
            if (!item1.IsNullOrEmpty()) 
            {
                strPano = item1.PTNO;
                txtWrtno.Text = item1.WRTNO.To<string>();

                clsHcType.TORD.Pano = item1.PTNO;
                clsHcType.TORD.HICNO = item1.WRTNO;
                clsHcType.TORD.Bi = "61";    //종합검진
                clsHcType.TORD.sName = item1.SNAME;
                clsHcType.TORD.IpdOpd = "O";
                clsHcType.TORD.Age = item1.AGE;
                clsHcType.TORD.Sex = item1.SEX;
                clsHcType.TORD.DeptCode = "TO";
                clsHcType.TORD.DrCode = "7102";
                clsHcType.TORD.Ward = "";
                clsHcType.TORD.Room = 0;
                clsHcType.TORD.BDate = item1.SDATE.ToString();
                clsHcType.TORD.Job = "종검";

                lblPtno.Text = item1.PTNO;
                lblSName.Text = clsHcType.TORD.sName;
                lblBi.Text = clsHcType.TORD.Bi;
                lblAge.Text = clsHcType.TORD.Age.To<string>();
            }

            if (clsHcType.TORD.Pano.IsNullOrEmpty())
            {
                return;
            }

            string strSpecNo = string.Empty;

            //검체번호
            List<COMHPC> listSpecNo = comHpcLibBService.GetListSpecNobyHicNo(clsHcType.TORD.Pano, clsHcType.TORD.HICNO);
            
            if (listSpecNo.Count > 0)
            {
                for (int i = 0; i < listSpecNo.Count; i++)
                {
                    strSpecNo = strSpecNo + "'" + listSpecNo[i].SPECNO.Trim() + "',";
                }
            }

            if (!strSpecNo.IsNullOrEmpty())
            {
                strSpecNo = VB.Left(strSpecNo, strSpecNo.Length - 1);
            }

            int nRow = 0;
            string strMasterCode = string.Empty;
            string strSubCode = string.Empty;
            string strSpecCode = string.Empty;
            string strTubeCode = string.Empty;
            string strTubeName = string.Empty;
            string strSpecName = string.Empty;
            string strDrComment = string.Empty;

            //검사할 항목을 SELECT
            List<EXAM_MASTER> exlist = examMasterService.GetHeaBarcodeBySunalDtl(clsHcType.TORD.HICNO);
            if (exlist.Count > 0)
            {
                for (int i = 0; i < exlist.Count; i++)
                {
                    strMasterCode = exlist[i].MASTERCODE;  //검사코드
                    strSubCode = cHE.AllSubCode2(strMasterCode);

                    strSpecCode = exlist[i].SPECCODE;  //검체코드
                    strTubeCode = exlist[i].TUBECODE;  //용기코드
                    strSpecName = "";

                    //2014-10-20 ABO검사 바코드 2장 인쇄
                    //If GstrSysDate >= "2014-10-20" Then
                    //    If strMasterCode = "BB01" Then strMasterCode = "BB001"
                    //    If strMasterCode = "BB05" Then strMasterCode = ""
                    //End If

                    if (strMasterCode == "BB01") { strMasterCode = "BB001"; }
                    if (strMasterCode == "BB05") { strMasterCode = ""; }

                    nRow = nRow + 1;
                    if (nRow > SS2.ActiveSheet.RowCount) { SS2.ActiveSheet.RowCount = nRow; }

                    SS2.ActiveSheet.Cells[nRow - 1, 0].Text = exlist[i].EXAMCODE;
                    SS2.ActiveSheet.Cells[nRow - 1, 1].Text = strMasterCode;
                    SS2.ActiveSheet.Cells[nRow - 1, 2].Text = exlist[i].EXAMNAME;       //검사명

                    //검체명
                    strSpecName = cHE.GetYNameBySpecCode(strSpecCode, "14");

                    //용기명
                    strTubeName = cHE.GetYNameBySpecCode(strSpecCode, "15");
                    
                    strDrComment = "";

                    SS2.ActiveSheet.Cells[nRow - 1, 3].Text = strSpecName;
                    SS2.ActiveSheet.Cells[nRow - 1, 4].Text = strTubeName;
                    SS2.ActiveSheet.Cells[nRow - 1, 5].Text = strSpecCode;                            //검체코드
                    SS2.ActiveSheet.Cells[nRow - 1, 6].Text = strTubeCode;                            //용기코드

                    //이미 발행했는지 여부 확인
                    if (strSpecNo != "" && strPano != "")
                    {
                        SS2.ActiveSheet.Cells[nRow - 1, 7].Text = cHE.ChkExcuteExamByPanoCode(strPano, strSpecNo, strMasterCode, strSubCode);
                    }
                }
            }

        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSave)
            {
                Print_BarCode_Main();
            }
            else if (sender == btnSearch)
            {
                Display_List();
            }
        }

        private void Print_BarCode_Main()
        {
            int result = 0;

            if (SS2.ActiveSheet.RowCount == 0) { return; }

            if (chkMBarcode.Checked && rdoJob2.Checked)
            {
                MessageBox.Show("접수자는 다시 접수를 못함.", "오류");
                return;
            }

            if (SS2.ActiveSheet.RowCount == 0)
            {
                string strMsg = "접수할 검사내역이 없습니다." + ComNum.VBLF;
                strMsg = strMsg + "바코드 발행 완료로 처리를 하시겠습니까?";

                if (ComFunc.MsgBoxQ(strMsg, "선택", MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
                else
                {
                    if (strJob.Equals("HIC"))
                    {
                        //일반검진 접수마스타에 바코드인쇄 Flag SET
                        result = hicJepsuService.UpdateGbExamByWrtno(txtWrtno.Text.To<long>());
                        if (result <= 0)
                        {
                            MessageBox.Show("작업실패!");
                            return;
                        }
                    }
                    else
                    {
                        //종합검진 접수마스타에 바코드인쇄 Flag SET
                        result = heaJepsuService.UpdateGbExamByWrtno(txtWrtno.Text.To<long>());
                        if (result <= 0)
                        {
                            MessageBox.Show("작업실패!");
                            return;
                        }
                    }
                }
            }

            btnSave.Enabled = false;

            //SS2에 있는 내용을 검체번호 발생 및 DB에 저장
            if (JEPSU_Main_Rtn() == "NO")
            {
                return;
            }

            frmHcExamBarCode frm = new frmHcExamBarCode();
            frm.ShowDialog();
            

            if (clsHcVariable.GstrTubeMsg != "")
            {
                lblTube.Text = "접수번호:" + txtWrtno.Text + "의 Tube내역" + ComNum.VBLF + clsHcVariable.GstrTubeMsg;
                lblWB.Text = VB.Format(clsHcVariable.GnWBVolume, "###0.0") + " ml";
            }

            if (strJob.Equals("HIC"))
            {
                //종합검진 접수마스타에 바코드인쇄 Flag SET
                result = hicJepsuService.UpdateGbExamByWrtno(txtWrtno.Text.To<long>());
                if (result <= 0)
                {
                    MessageBox.Show("일반검진접수 MASTER 검체발행여부 UPDATE 오류", "오류");
                    return;
                }
            }
            else
            {
                //종합검진 접수마스타에 바코드인쇄 Flag SET
                result = heaJepsuService.UpdateGbExamByWrtno(txtWrtno.Text.To<long>());
                if (result <= 0)
                {
                    MessageBox.Show("종검접수 MASTER 검체발행여부 UPDATE 오류", "오류");
                    return;
                }
            }
            

            clsHcVariable.GstrJepsuJob = "";
            Screen_Clear();

            Display_List();
        }

        /// <summary>
        /// SS2에 있는 내용을 검체번호 발생 및 DB에 저장
        /// </summary>
        /// <returns></returns>
        private string JEPSU_Main_Rtn()
        {
            int j = 0;
            string rtnVal = string.Empty;
            string strROWID = "";
            string strBloodTime = "";  //채혈시간
            string strMasterCode = "";
            string strSpecCode = "";
            string strOrder = "";

            clsHcVariable.gsBarSpecNo = "";

            #region 접수및 바코드인쇄용 SHEET에 SET

            string strSTRT = "R";
            string strDrComment = "";

            //오더를 보관할 배열을 Clear
            clsHcType.TORD.OrderCNT = 0;
            for (int i = 0; i < 81; i++)
            {
                clsHcType.TORD.Order[i] = "";
            }

            j = 0;   //오더의 건수

            for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
            {
                strSTRT = "";       //응급여부
                strDrComment = "";  //의사컴멘트
                strROWID = "";
                strBloodTime = "";  //채혈시간

                if (SS2.ActiveSheet.Cells[i, 7].Text != "Y")
                {
                    //발행이 안된것만 발행
                    strMasterCode = SS2.ActiveSheet.Cells[i, 1].Text;   //검사코드
                    strSpecCode = SS2.ActiveSheet.Cells[i, 5].Text;     //의사오더의 검체코드

                    strOrder = strMasterCode + "^" + strSpecCode + "^";
                    strOrder = strOrder + strSTRT + "^" + strDrComment + "^" + strROWID + "^";
                    strOrder = strOrder + strBloodTime + "^";
                    j = j + 1;
                    clsHcType.TORD.Order[j] = strOrder;
                }
            }

            clsHcType.TORD.OrderCNT = j;

            #endregion

            #region 바코드 출력 Main Logic
            if (cHE.EXAM_SPEC_WRITE_TLA(this.spBarCode) == "OK") //DB에 등록
            { 
                rtnVal = "OK";
            }
            else
            {
                MessageBox.Show(clsPublic.GstrMsgList, "RollBack");
                rtnVal = "NO";
            }
            #endregion

            return rtnVal;
        }

        private void Display_List()
        {
            int nRow = 0;
            bool bShow = false;
            string strGbExam = rdoJob2.Checked == true ? "Y" : "";
            string strPano = string.Empty;
            string strRowid = string.Empty;
            string strMasterCode = string.Empty;

            cSPD.Spread_Clear_Simple(ssList);

            if (rdoJong1.Checked)
            {
                //일반검진
                bool bLDL = chkLDL.Checked ? true : false;

                List<COMHPC> list = comHpcLibBService.GetHicExamListByItems(dtpFDate.Text, dtpTDate.Text, strGbExam, clsHcVariable.GbHicChul);

                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        strPano = list[i].PTNO;

                        if (bLDL)
                        {
                            strRowid = hicResultService.GetRowidByOneExcodeWrtno("C405", list[i].WRTNO);
                            if (!strRowid.IsNullOrEmpty())
                            {
                                nRow = nRow + 1;
                                if (nRow > ssList.ActiveSheet.RowCount) { ssList.ActiveSheet.RowCount = nRow; }

                                ssList.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].WRTNO.To<string>();
                                ssList.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].SNAME;
                                ssList.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].GJJONG;
                                ssList.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].JEPDATE;
                                bShow = true;
                            }
                        }
                        else
                        {
                            nRow = nRow + 1;
                            if (nRow > ssList.ActiveSheet.RowCount) { ssList.ActiveSheet.RowCount = nRow; }

                            ssList.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].WRTNO.To<string>();
                            ssList.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].SNAME;
                            ssList.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].GJJONG;
                            ssList.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].JEPDATE;
                            bShow = true;
                        }

                        if (bShow)
                        {
                            //검체번호
                            List<string> lstSpecno = new List<string>();
                            List<EXAM_SPECMST> lstEspc = examSpecmstService.GetSpecNoByHicNo(list[i].WRTNO, strPano);

                            if (lstEspc.Count > 0)
                            {
                                for (int j = 0; j < lstEspc.Count; j++)
                                {
                                    lstSpecno.Add(lstEspc[j].SPECNO);
                                }
                            }

                            //검사할 항목을 SELECT
                            List<COMHPC> lstMastCode = comHpcLibBService.GetListMasterCodeByHicWrtno(list[i].WRTNO);
                            if (lstMastCode.Count > 0)
                            {
                                for (int j = 0; j < lstMastCode.Count; j++)
                                {
                                    strMasterCode = lstMastCode[j].MASTERCODE;

                                    if (lstSpecno.Count > 0 && strPano != "")
                                    {
                                        strRowid = examResultcService.GetRowidByMstCodeSpecNoIN(strPano, strMasterCode, lstSpecno);
                                        if (!strRowid.IsNullOrEmpty())
                                        {
                                            ssList.ActiveSheet.Cells[nRow - 1, 3].Text = "Y";
                                        }
                                    }
                                }
                            }
                        }

                        bShow = false;

                    }
                }
            }
            else
            {
                //종합검진
                ssList.DataSource = comHpcLibBService.GetHeaExamListByItems(dtpFDate.Text, dtpTDate.Text, strGbExam);
            }

            
        }

        private void eFormload(object sender, EventArgs e)
        {
            Screen_Clear();
        }

        private void Screen_Clear()
        {
            txtWrtno.Text = "";

            lblPtno.Text = "";
            lblBi.Text = "";
            lblAge.Text = "";

            ssList.ActiveSheet.ClearRange(0, 0, ssList.ActiveSheet.Rows.Count, ssList.ActiveSheet.ColumnCount, true);
            ssList.ActiveSheet.Rows.Count = 0;

            SS2.ActiveSheet.ClearRange(0, 0, SS2.ActiveSheet.Rows.Count, SS2.ActiveSheet.ColumnCount, true);
            SS2.ActiveSheet.Rows.Count = 0;

            spBarCode.ActiveSheet.ClearRange(0, 0, spBarCode.ActiveSheet.Rows.Count, spBarCode.ActiveSheet.ColumnCount, true);
            spBarCode.ActiveSheet.Rows.Count = 0;
        }
    }
}
