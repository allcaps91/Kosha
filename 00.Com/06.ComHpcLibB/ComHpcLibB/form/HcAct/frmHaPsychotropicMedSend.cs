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
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Act
/// File Name       : frmHaPsychotropicMedSend.cs
/// Description     : 종검 향정/마약 처방전송
/// Author          : 이상훈
/// Create Date     : 2019-09-18
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm종검향정약품처방전송.frm(Frm종검향정약품처방전송)" />

namespace ComHpcLibB
{
    public partial class frmHaPsychotropicMedSend : Form
    {
        ComHpcLibBService comHpcLibBService = null;
        HicExcodeService hicExcodeService = null;
        EndoJupmstService endoJupmstService = null;
        HicPatientService hicPatientService = null;
        HicHyangApproveService hicHyangApproveService = null;
        HeaJepsuService heaJepsuService = null;
        HeaResvExamPatientService heaResvExamPatientService = null;
        HicResultExCodeService hicResultExCodeService = null;
        HeaJepsuPatientService heaJepsuPatientService = null;
        HicJepsuService hicJepsuService = null;
        HicJepsuSunapService hicJepsuSunapService = null;
        HicGroupexamExcodeService hicGroupexamExcodeService = null;
        OcsOorderService ocsOorderService = null;
        HicHyangService hicHyangService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        ComFunc cf = new ComFunc();

        FarPoint.Win.Spread.CellType.TextCellType txt = new FarPoint.Win.Spread.CellType.TextCellType();

        bool FbClk;
        double[,] FnHyang = new double[5, 2];
        string FstrJob;
        string FstrDrug;
        long FnWRTNO;
        string FstrPtno;
        string FstrDrSabun;
        string FstrJepDate;
        string FstrBDate;
        string FstrDept;
        string FstrSdate;
        string FstrSQL;
        string FstrOldBDate;
        long FnChasu;
        int FnRow_S = 0;
        int FnRow_T = 0;
        int FnRow_B = 0;
        bool bFlag;
        string FstrNrList;  //간호사명단

        public frmHaPsychotropicMedSend()
        {
            InitializeComponent();

            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            comHpcLibBService = new ComHpcLibBService();
            hicExcodeService = new HicExcodeService();
            endoJupmstService = new EndoJupmstService();
            hicPatientService = new HicPatientService();
            hicHyangApproveService = new HicHyangApproveService();
            heaJepsuService = new HeaJepsuService();
            heaResvExamPatientService = new HeaResvExamPatientService();
            hicResultExCodeService = new HicResultExCodeService();
            heaJepsuPatientService = new HeaJepsuPatientService();
            hicJepsuService = new HicJepsuService();
            hicJepsuSunapService = new HicJepsuSunapService();
            hicGroupexamExcodeService = new HicGroupexamExcodeService();
            ocsOorderService = new OcsOorderService();
            hicHyangService = new HicHyangService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnHyang.Click += new EventHandler(eBtnClick);

            this.btnMenuSave.Click += new EventHandler(eBtnClick);
            this.btnMenuDelete.Click += new EventHandler(eBtnClick);
            this.btnMenuFinishOk.Click += new EventHandler(eBtnClick);
            this.btnMenuFinishCancel.Click += new EventHandler(eBtnClick);
            this.btnNarcotic.Click += new EventHandler(eBtnClick);
            this.btnEmr.Click += new EventHandler(eBtnClick);

            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
        }

        void SetControl()
        {

        }

        void eFormLoad(object sender, EventArgs e)
        {
            string strBuse = "";
            string strJikjong = "";
            string strSDate = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpSDate.Text = clsPublic.GstrSysDate;
            dtpBDate.Text = clsPublic.GstrSysDate;
            FstrOldBDate = dtpBDate.Text;
            sp.Spread_All_Clear(SS1);

            FbClk = false;

            //건강증진센터 검사코드중 수면내시경 코드인것을 미리 세팅
            FstrSQL = "";
            fn_Read_Endo_Scope();

            strBuse = "044510";
            strJikjong = "41";  //간호사
            strSDate = dtpSDate.Text;

            //종합건진 간호사 목록을 표시함
            FstrNrList = ",4349,";
            List<COMHPC> list = comHpcLibBService.GetSabunbyNurse(strBuse, strJikjong, strSDate);

            for (int i = 0; i < list.Count; i++)
            {
                FstrNrList += list[i].SABUN + ",";
            }

            //컬럼숨기기
            SS1.ActiveSheet.Columns[6].Visible = false;
            SS1.ActiveSheet.Columns[7].Visible = false;
            SS1.ActiveSheet.Columns[19].Visible = false;
            SS1.ActiveSheet.Columns[20].Visible = false;
            SS1.ActiveSheet.Columns[21].Visible = false;
            SS1.ActiveSheet.Columns[22].Visible = false;
            SS1.ActiveSheet.Columns[23].Visible = false;
            SS1.ActiveSheet.Columns[25].Visible = false;
            SS1.ActiveSheet.Columns[29].Visible = false;
            SS1.ActiveSheet.Columns[30].Visible = false;
        }

        void fn_Read_Endo_Scope()
        {
            //수면내시경 대상자인지 확인
            List<HIC_EXCODE> list = hicExcodeService.GetCodeEndoScope();

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    FstrSQL += list[i].CODE.Trim() + ",";
                }
            }

            if (VB.Right(FstrSQL, 1) == ".")
            {
                FstrSQL = VB.Left(FstrSQL, FstrSQL.Length - 1);
            }
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
                sp.Spread_All_Clear(SS1);
                SS1.ActiveSheet.RowCount = 31;

                //처방작성
                if (rdoJob1.Checked == true)
                {
                    btnSave.Enabled = true;
                    btnPrint.Enabled = true;
                    btnMenuSave.Enabled = false;
                    fn_Display_Slip_Create();   //처방작성용 자료를 View
                    btnHyang.Enabled = false;
                    SS1.Enabled = true;
                    btnMenuFinishOk.Enabled = false;
                    btnMenuFinishCancel.Enabled = false;
                }
                else if (rdoJob1.Checked == true)   //처방인쇄
                {
                    btnSave.Enabled = false;
                    btnPrint.Enabled = true;
                    fn_Display_Slip_Print();   //처방작성용 자료를 View
                    btnHyang.Enabled = false;
                    btnPrint2.Enabled = false;
                    SS1.Enabled = true;
                    btnMenuSave.Enabled = false;
                    btnMenuDelete.Enabled = false;
                    btnMenuFinishOk.Enabled = false;
                    btnMenuFinishCancel.Enabled = false;
                }
                else
                {
                    btnSave.Enabled = false;
                    btnPrint.Enabled = false;
                    btnMenuSave.Enabled = true;
                    btnHyang.Enabled = true;
                    btnPrint2.Enabled = true;
                    SS1.Enabled = true;
                    fn_Display_Slip_Send(); //사용량 전송용 자료를 View
                }
            }
            else if (sender == btnSave)
            {
                long nPano = 0;
                long nWRTNO = 0;
                long nAge = 0;
                string strSEX = "";
                string strAmPm = "";
                string strJuso = "";
                string strSname = "";
                string strDept = "";
                string strJumin = "";
                string strPtNo = "";
                string strGbSite = "";
                string strNRSABUN = "";
                string strJong = "";
                string strGbSleep = "";

                double nPOL2_Qty = 0;   //A-PLS2
                double nPOL12G_Qty = 0;   //A-POL12G
                double nPOL12GA_Qty = 0;   //A-PO12GA
                double nANE12G_Qty = 0;   //A-ANE12G
                double nPOL8G_Qty = 0;   //A-POL8G
                double nBASCA_Qty = 0;   //A-BASCA
                double nBASCAM_Qty = 0;   //A-BASCAM
                double nPTD25_Qty = 0;   //N-PTD25
                double nOldQty = 0;
                double nNewQty = 0;
                int nChasu = 0;

                if (VB.InStr(FstrNrList, "," + clsType.User.IdNumber + " ,") == 0)
                {
                    MessageBox.Show("승인요청은 간호사만 가능합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                strNRSABUN = string.Format("{0:#00000}", long.Parse(clsType.User.IdNumber));

                if (strNRSABUN == "")
                {
                    MessageBox.Show("승인요청은 간호사만 가능합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    if (SS1.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        nPano = long.Parse(SS1.ActiveSheet.Cells[i, 1].Text);
                        strAmPm = SS1.ActiveSheet.Cells[i, 2].Text.Trim() == "오후" ? "2" : "1";
                        strSname = SS1.ActiveSheet.Cells[i, 3].Text.Trim();
                        nAge = long.Parse(VB.Pstr(SS1.ActiveSheet.Cells[i, 4].Text, "/", 1));
                        strSEX = VB.Pstr(SS1.ActiveSheet.Cells[i, 4].Text, "/", 2).Trim();

                        nWRTNO = long.Parse(SS1.ActiveSheet.Cells[i, 5].Text);
                        strPtNo = SS1.ActiveSheet.Cells[i, 18].Text.Trim();
                        strDept = SS1.ActiveSheet.Cells[i, 26].Text.Trim();
                        strJumin = SS1.ActiveSheet.Cells[i, 32].Text.Trim();
                        strGbSleep = SS1.ActiveSheet.Cells[i, 31].Text.Trim() == "◎" ? "Y" : "N";
                        nChasu = 1;


                        nPOL2_Qty = 0;
                        if (SS1.ActiveSheet.Cells[i, 6].Text == "True")
                        {
                            nPOL2_Qty = double.Parse(SS1.ActiveSheet.Cells[i, 7].Text.Trim());
                        }

                        nANE12G_Qty = 0;
                        if (SS1.ActiveSheet.Cells[i, 8].Text == "True")
                        {
                            nANE12G_Qty = double.Parse(SS1.ActiveSheet.Cells[i, 9].Text.Trim());
                        }

                        nPOL8G_Qty = 0;
                        if (SS1.ActiveSheet.Cells[i, 10].Text == "True")
                        {
                            nPOL8G_Qty = double.Parse(SS1.ActiveSheet.Cells[i, 11].Text.Trim());
                        }

                        nBASCAM_Qty = 0;
                        if (SS1.ActiveSheet.Cells[i, 12].Text == "True")
                        {
                            nBASCAM_Qty = double.Parse(SS1.ActiveSheet.Cells[i, 13].Text.Trim());
                        }

                        nPTD25_Qty = 0;
                        if (SS1.ActiveSheet.Cells[i, 14].Text == "True")
                        {
                            nPTD25_Qty = double.Parse(SS1.ActiveSheet.Cells[i, 15].Text.Trim());
                        }

                        strGbSite = "1";
                        if (SS1.ActiveSheet.Cells[i, 28].Text == "◎")
                        {
                            strGbSite = "2";    //본관내시경실
                        }

                        //재승인 요청시 중복자료 생성으로 삭제 후 새로 등록함
                        int result = comHpcLibBService.DeleteHicHyangApprove(nWRTNO);

                        if (result < 0)
                        {
                            MessageBox.Show("중복자료 삭제중 오류 발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        //HIC_HYANG_APPROVE에 저장
                        for (int j = 1; j <= 5; j++)
                        {
                            switch (j)
                            {
                                case 1:
                                    FstrDrug = "A-POL2";
                                    nNewQty = nPOL2_Qty;
                                    break;
                                case 2:
                                    FstrDrug = "A-POL8G";
                                    nNewQty = nPOL8G_Qty;
                                    break;
                                case 3:
                                    FstrDrug = "A-BASCAM";
                                    nNewQty = nBASCAM_Qty;
                                    break;
                                case 4:
                                    FstrDrug = "N-PTD25";
                                    nNewQty = nPTD25_Qty;
                                    break;
                                case 5:
                                    FstrDrug = "A-ANE12G";
                                    nNewQty = nANE12G_Qty;
                                    break;
                                default:
                                    break;
                            }

                            if (nNewQty != 0)
                            {
                                //주소를 찾음
                                HIC_PATIENT list = hicPatientService.GetJusobyPano(nPano, strSname);

                                strJuso = "";
                                if (list != null)
                                {
                                    strJuso = list.JUSO1 + " " + list.JUSO2;
                                }

                                strJong = "1";  //향정
                                if (VB.Left(FstrDrug, 2) == "N")
                                {
                                    strJong = "2";  //마약
                                }

                                HIC_HYANG_APPROVE item = new HIC_HYANG_APPROVE();

                                item.SDATE = dtpSDate.Text;
                                item.BDATE = dtpBDate.Text;
                                item.WRTNO = nWRTNO;
                                item.PANO = nPano;
                                item.SNAME = strSname;
                                item.JONG = strJong;
                                item.GBSITE = strGbSite;
                                item.DEPTCODE = strDept;
                                item.SUCODE = FstrDrug;
                                item.ENTQTY = nNewQty;
                                item.NRSABUN = strNRSABUN;
                                item.PTNO = strPtNo;
                                item.JUMIN = VB.Left(strJumin, 7) + "******";
                                item.JUMIN2 = clsAES.AES(strJumin);
                                item.SEX = strSEX;
                                item.AGE = nAge;
                                item.DRSABUN = "";
                                item.PRINT = "N";
                                item.AMPM = strAmPm;
                                item.GBSLEEP = strGbSleep;
                                item.QTY = "1";
                                item.REALQTY = 1;
                                item.JUSO = strJuso;
                                item.CHASU = nChasu;
                                 
                                int result1 = hicHyangApproveService.Insert(item);

                                if (result1 < 0)
                                {
                                    MessageBox.Show("승인의뢰용 처방을 등록 중 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                        }
                    }
                }

                MessageBox.Show("저장완료!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (sender == btnPrint)
            {
                //향정마약 서식변경 및 종검에서 약품을 KEEP
                //Dim i%, j %, nWRTNO As Long
                //Dim strPano$, strSname$, strIO$, strDeptCode$, strDRSABUN$
                //Dim strSEX$, nAge%
                //Dim strSabun$, strBDATE$, strSDate$, strNRSABUN$, strJuso$
                //Dim strPtNo$, strDept$, strGBPrint$, strJumin$, strSuCode$
                //Dim strChk$, strGbSite$, strQty$, strPRT$
                //Dim nEntQty1 As Double, nEntQty2 As Double


                //'종합건진 간호사만 인쇄 가능함
                //If InStr(FstrNrList, "," & GnJobSabun & ",") = 0 Then
                //    MsgBox "종합건진 간호사만 처방인쇄가 가능합니다.", , "오류"
                //    Exit Sub
                //End If


                //strBDATE = Trim(TxtBDate.Text) '검진일
                //strSDate = Trim(TxtBDate.Text) '검진일


                //adoConnect.BeginTrans

                //For i = 1 To SS1.DataRowCnt
                //    SS1.Row = i
                //    SS1.Col = 1:  strChk = Trim(SS1.Text)
                //    SS1.Col = 29: strGbSite = Trim(SS1.Text)


                //    '종합건진 내시경만 인쇄함
                //    If strChk = "1" And strGbSite = "" Then
                //        SS1.Col = 4:  strSname = Trim(SS1.Text)
                //        SS1.Col = 5:  strSEX = P(SS1.Text, "/", 2)
                //        SS1.Col = 5:  nAge = Val(P(SS1.Text, "/", 1))
                //        SS1.Col = 6:  nWRTNO = Val(SS1.Text)
                //        SS1.Col = 19: strPtNo = Trim(SS1.Text)
                //        SS1.Col = 25: strGBPrint = Trim(SS1.Text)
                //        SS1.Col = 27: strDept = Trim(SS1.Text)
                //        SS1.Col = 31: strDRSABUN = Trim(SS1.Text)


                //        For j = 1 To 4
                //            strSuCode = "": nEntQty1 = 0: nEntQty2 = 0
                //            Select Case j
                //                Case 1:  strSuCode = "A-POL2":   nEntQty1 = 20
                //                            SS1.Col = 8: nEntQty2 = Val(SS1.Text)
                //                'Case 2:  strSuCode = "A-POL12G": nEntQty1 = 12
                //                Case 2:  strSuCode = "A-PO12GA": nEntQty1 = 12
                //                            SS1.Col = 10: nEntQty2 = Val(SS1.Text)
                //                Case 3:  strSuCode = "A-POL8G":  nEntQty1 = 8
                //                            SS1.Col = 12: nEntQty2 = Val(SS1.Text)
                //                Case 4:  strSuCode = "A-BASCA":  nEntQty1 = 5
                //                            SS1.Col = 14: nEntQty2 = Val(SS1.Text)
                //            End Select


                //            If strSuCode<> "" And nEntQty2 > 0 Then
                //                '인쇄완료 SET
                //                SQL = "UPDATE HIC_HYANG_APPROVE SET Print='Y' "
                //                SQL = SQL & "WHERE WRTNO=" & nWRTNO & " "
                //                SQL = SQL & "  AND BDate=TO_DATE('" & strBDATE & "','YYYY-MM-DD') "
                //                SQL = SQL & "  AND PTno='" & strPtNo & "' "
                //                SQL = SQL & "  AND DelDate IS NULL "
                //                Result = AdoExecute(SQL)


                //                '승인일자를 읽음
                //                SQL = "SELECT TO_CHAR(ApproveTime,'YYYY-MM-DD') ApproveTime,Juso,Jumin "
                //                SQL = SQL & " FROM HIC_HYANG_APPROVE "
                //                SQL = SQL & "WHERE WRTNO=" & nWRTNO & " "
                //                SQL = SQL & "  AND BDate=TO_DATE('" & strBDATE & "','YYYY-MM-DD') "
                //                SQL = SQL & "  AND DEPTCODE ='" & strDept & "' "
                //                SQL = SQL & "  AND SUCODE = '" & strSuCode & "' "
                //                SQL = SQL & "  AND DelDate IS NULL "
                //                Call AdoOpenSet(rs1, SQL)
                //                strSDate = Trim(AdoGetString(rs1, "ApproveTime", 0))
                //                strJuso = Trim(AdoGetString(rs1, "Juso", 0))
                //                strJumin = Trim(AdoGetString(rs1, "Jumin", 0))
                //                Call AdoCloseSet(rs1)


                //                strSDate = Trim(TxtBDate.Text)
                //                strNRSABUN = Format(GnJobSabun, "#00000")


                //                '주소를 읽음
                //                If strJuso = "" Then
                //                    If strDept = "HR" Then
                //                        SQL = "SELECT Juso1,Juso2 FROM HIC_JEPSU "
                //                        SQL = SQL & "WHERE WRTNO=" & nWRTNO & " "
                //                        SQL = SQL & "  AND SName='" & strSname & "' "
                //                    Else
                //                        SQL = "SELECT Juso1,Juso2 FROM HIC_PATIENT "
                //                        SQL = SQL & "WHERE PTno='" & strPtNo & "' "
                //                        SQL = SQL & "  AND SName='" & strSname & "' "
                //                    End If
                //                    Call AdoOpenSet(rs1, SQL)
                //                    strJuso = AdoGetString(rs1, "Juso1", 0) & AdoGetString(rs1, "Juso2", 0)
                //                    Call AdoCloseSet(rs1)
                //                    If strJuso<> "" Then
                //                        SQL = "UPDATE HIC_HYANG_APPROVE SET Juso='" & strJuso & "' "
                //                        SQL = SQL & "WHERE WRTNO=" & nWRTNO & " "
                //                        SQL = SQL & "  AND BDate=TO_DATE('" & strBDATE & "','YYYY-MM-DD') "
                //                        SQL = SQL & "  AND PTno='" & strPtNo & "' "
                //                        SQL = SQL & "  AND DelDate IS NULL "
                //                        Result = AdoExecute(SQL)
                //                    End If
                //                End If


                //                '수량
                //                strQty = "(=" & Format(nEntQty2, "#0.0") & "ml)"
                //                If InStr(strQty, ".0ml") > 0 Then strQty = Replace(strQty, ".0ml", "ml")
                //                If nEntQty1<> 0 And nEntQty2<> 0 Then
                //                    strQty = strQty & "," & Format(nEntQty2 / nEntQty1, "0.00") & "A"
                //                End If


                //                '------------------------------------
                //                '  인쇄폼에 넘겨줄 변수를 설정함
                //                '------------------------------------


                //                '(1) 처방전명
                //                If Left(strSuCode, 2) = "N-" Then
                //                    strPRT = "마  약{$}"
                //                Else
                //                    strPRT = "향정주사{$}"
                //                End If
                //                strPRT = strPRT & strDept & "{$}" '(2) 병동/외래
                //                strPRT = strPRT & strSname & "{$}" '(3) 성명
                //                strPRT = strPRT & strPano & "{$}" '(4) 등록번호
                //                If strDept = "TO" Then
                //                    strPRT = strPRT & "종합건진{$}" '(5) 진료과
                //                Else
                //                    strPRT = strPRT & "일반건진{$}" '(5) 진료과
                //                End If
                //                strPRT = strPRT & "{$}" '(6) 오더번호
                //                strPRT = strPRT & strSEX & "/" & Format(nAge, "#0") & "{$}" '(7) 성별/나이
                //                strPRT = strPRT & Left(strJumin, 6) & "-" & Mid(strJumin, 7, 1) & "******" & "{$}" '(8) 주민등록번호
                //                strPRT = strPRT & strJuso & "{$}" '(9) 주소
                //                strPRT = strPRT & "검사용{$}" '(10) 상병명
                //                strPRT = strPRT & "Pain{$}" '(11) 주요증상
                //                strPRT = strPRT & strSDate & "{$}" '(12) 처방일,검사일
                //                strPRT = strPRT & strBDATE & "{$}" '(13) 처방일,검사일
                //                strPRT = strPRT & strSuCode & "{$}" '(14) 약품코드
                //                strPRT = strPRT & strDRSABUN & "{$}" '(15) 처방승인의사
                //                strPRT = strPRT & strQty & "{$}" '(16) 사용수량


                //                GstrHelpCode = strPRT
                //                Frm향정마약처방전인쇄.Show 1
                //            End If
                //        Next j


                //    End If


                //Next i


                //adoConnect.CommitTrans

                //Call CmdView_Click
            }
            else if (sender == btnHyang)
            {
                string strPtNo = "";
                string strBDate = "";
                string strDept = "";

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strPtNo = SS1.ActiveSheet.Cells[i, 18].Text.Trim();
                    strBDate = SS1.ActiveSheet.Cells[i, 29].Text.Trim();
                    strDept = SS1.ActiveSheet.Cells[i, 26].Text.Trim();

                    ENDO_JUPMST list = endoJupmstService.GetGbConbyPtno(strPtNo, strBDate, strDept);

                    if (list != null)
                    {
                        if (!list.GBCON_31.IsNullOrEmpty())
                        {
                            SS1.ActiveSheet.Cells[i, 9].Text = string.Format("{0:#0.00}", long.Parse(list.GBCON_31) / 10);
                        }
                        if (!list.GBCON_21.IsNullOrEmpty())
                        {
                            SS1.ActiveSheet.Cells[i, 13].Text = string.Format("{0:#0.00}", long.Parse(list.GBCON_21) / 10);
                        }    
                    }
                    else
                        strDept = "HR";
                    {
                        ENDO_JUPMST list2 = endoJupmstService.GetGbConbyPtno(strPtNo, strBDate, strDept);

                        if (list2 != null)
                        {
                            if (!list2.GBCON_31.IsNullOrEmpty())
                            {
                                SS1.ActiveSheet.Cells[i, 9].Text = string.Format("{0:#0.00}", long.Parse(list2.GBCON_31) / 10);
                            }

                            if (!list2.GBCON_21.IsNullOrEmpty())
                            {
                                SS1.ActiveSheet.Cells[i, 13].Text = string.Format("{0:#0.00}", long.Parse(list2.GBCON_21) / 10);
                            }  
                        }
                    }
                }

                eBtnClick(btnMenuSave, new EventArgs());
                eBtnClick(btnSearch, new EventArgs());
            }
            else if (sender == btnPrint2)
            {
                //Dim strFont1    As String
                //Dim strFont2    As String
                //Dim strFont3    As String
                //Dim strHead     As String


                //Call menuSave_Click


                //SS1.Row = 0: SS1.Col = 1:  SS1.ColHidden = True
                //SS1.Row = 0: SS1.Col = 2:  SS1.ColHidden = True


                //For i = 1 To SS1.MaxRows
                //    SS1.Row = i: SS1.Col = 1
                //    If Trim(SS1.Text) = "0" Then
                //        SS1.Col = 0: SS1.RowHidden = True
                //    End If
                //Next i


                //strFont1 = "/l/fn""굴림체""/fz""20"""
                //strFont2 = "/l"
                //strFont3 = "/n/fn""굴림체""/fb0/fu0/fz""11"""
                //SS1.PrintHeader = strFont1 & "/n" & Space(30) & "종검 향정/마약 처방명단" & "/n"
                //SS1.PrintHeader = SS1.PrintHeader & strFont3 & "작업기간: " & TxtBDate.Text & " ~ " & TxtBDate.Text
                //SS1.PrintHeader = SS1.PrintHeader & Space(3) & "인쇄시각: " & GstrSysDate & " " & GstrSysTime & " Page: /p"


                //SS1.PrintMarginLeft = 150
                //SS1.PrintMarginRight = 0
                //SS1.PrintMarginTop = 200
                //SS1.PrintMarginBottom = 100
                //SS1.PrintColHeaders = True
                //SS1.PrintRowHeaders = False
                //SS1.PrintBorder = True
                //SS1.PrintColor = False
                //SS1.PrintGrid = True
                //SS1.PrintShadows = True
                //SS1.PrintUseDataMax = False
                //SS1.PrintOrientation = SS_PRINTORIENT_LANDSCAPE
                //SS1.PrintType = SS_PRINT_ALL

                //' Print the spreadsheet
                //SS1.Action = SS_ACTION_PRINT
                //Call SQL_LOG("", SS1.PrintHeader)
    
                //SS1.Row = 0: SS1.Col = 1:  SS1.ColHidden = False
                //SS1.Row = 0: SS1.Col = 2:  SS1.ColHidden = False


                //For i = 1 To SS1.MaxRows
                //    SS1.Row = i: SS1.Col = 0: SS1.RowHidden = False
                //Next i
            }
            else if (sender == btnMenuSave)
            {
                double nTotal = 0;
                long nPano = 0;
                int nCNT = 0;
                int nChasu = 0;
                int result = 0;
                string strChk = "";
                string strSucode = "";
                string strGbSite = "";

                int nRow = 0;
                int nCol = 0;

                for (int i = 0; i< SS1.ActiveSheet.RowCount-2; i++)

                {
                    for (int j = 0; j <=4; j++)
                    {
                        for (int k = 0; k <= 1; k++)
                        {
                            FnHyang[j, k] = 0;
                        }
                    }

                    nCNT = 0; nTotal = 0;

                    strChk = SS1.ActiveSheet.Cells[i, 0].Text;
                    nPano = Convert.ToInt32(SS1.ActiveSheet.Cells[i, 1].Text);
                    strGbSite = SS1.ActiveSheet.Cells[i, 28].Text;
                    FstrBDate = SS1.ActiveSheet.Cells[i, 29].Text;
                    nChasu = 1;
                    FstrSdate = FstrBDate;
                    FstrJepDate = FstrBDate;

                    for (int j = 0; j <= 4; j++)
                    {
                        nCol = (j * 2) + 7;
                        FnHyang[j, 1] = VB.Val(SS1.ActiveSheet.Cells[i, nCol].Text);
                    }

                    FnWRTNO = Convert.ToInt32(SS1.ActiveSheet.Cells[i, 5].Text);
                    FstrPtno = SS1.ActiveSheet.Cells[i, 18].Text;
                    FstrDept= SS1.ActiveSheet.Cells[i, 26].Text;
                    FstrDrSabun = SS1.ActiveSheet.Cells[i, 30].Text;

                    strSucode = "";

                    for (int j = 0; j <= 4; j++)
                    {
                        switch (j)
                        {
                            case 0: strSucode = "A-POL2"; break;
                            case 1: strSucode = "A-ANE12G"; break;
                            case 2: strSucode = "A-POL8G"; break;
                            case 3: strSucode = "A-BASCA"; break;
                            case 4: strSucode = "N-PTD25"; break;
                            default:
                                break;
                        }

                        result = hicHyangApproveService.UpdateEntqty2(FnHyang[j, 1], FnWRTNO, nPano.ToString(), strSucode);
                        if (result < 0)
                        {
                            MessageBox.Show("실용량 UPDATE 오류!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }
            else if (sender == btnMenuDelete)
            {
                int result = 0;
                int nCNT = 0;
                long nWrtno = 0;
                string strChk = "";
                string strMsg = "";
                string strPtno = "";
                string strDept = "";
                string strBdate = "";


                strMsg = "P 칼럼에 선택한 처방을 삭제합니다." + ComNum.VBLF;
                strMsg += "정말로 삭제를 하시겠습니까?";

                if (MessageBox.Show(strMsg, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
                strBdate = dtpBDate.Text;
                for (int i = 0; i <= SS1.ActiveSheet.RowCount - 1; i++)
                {
                    strChk = SS1.ActiveSheet.Cells[i, 0].Text;

                    if (strChk == "True")
                    {

                        nWrtno = long.Parse(SS1.ActiveSheet.Cells[i, 5].Text);
                        strPtno = SS1.ActiveSheet.Cells[i, 18].Text;
                        strDept = SS1.ActiveSheet.Cells[i, 26].Text;

                        //OCS_OORDER 전송 취소
                        result = ocsOorderService.DeleteOcsOorder(strPtno, strBdate, strDept, nWrtno);
                        if (result < 0)
                        {
                            MessageBox.Show("OCS_OORDER 처방삭제", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        result = hicHyangApproveService.DeleteHicHyangApprove(strBdate, nWrtno);
                        if (result < 0)
                        {
                            MessageBox.Show("HIC_HYANG_APPROVE 처방삭제", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        nCNT = nCNT + 1;
                    }
                }

                MessageBox.Show(nCNT + "건 삭제 완료", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (sender == btnMenuFinishOk)
            {
                //마감작업
                long nRead = 0;
                long nRead2 = 0;
                long nQty = 0;
                long nQty1 = 0;
                long nWrtno = 0;
                long nPano = 0;
                double nEntQty = 0;
                double nEntQty2 = 0;
                double nEntQty3 = 0;
                double nJQty = 0;
                string strBdate = "";
                string strPtno = "";
                string strSname = "";
                string strList = "";
                string strSucode = "";
                string strROWID = "";
                string strROWID2 = "";
                string strQty1 = "";
                string strMsg = "";

                List<string> strList1 = new List<string>();

                strBdate = dtpBDate.Text;
                strMsg = "마감을 하시겠습니까?" + ComNum.VBLF;
                strMsg += "마감을 하시면 내역 변경이 불가능합니다.";

                if (MessageBox.Show(strMsg, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }


                List<HIC_HYANG_APPROVE> list = hicHyangApproveService.GetItemCountByBDate(dtpBDate.Text);

                for (int i = 0; i< list.Count; i++)
                {
                    strList = "";
                    nWrtno = list[i].WRTNO;
                    nPano = list[i].PANO;
                    strPtno = list[i].PTNO;
                    strSname= list[i].SNAME;

                    //승인상세내역을 읽음
                    List<HIC_HYANG_APPROVE> list2 = hicHyangApproveService.GetRowidSucodeByItems(dtpBDate.Text, nWrtno, nPano, strPtno, strSname);

                    for (int j = 0; j < list2.Count; j++)
                    {
                        strSucode = list2[j].SUCODE;
                        nEntQty = list2[j].ENTQTY;
                        nEntQty2 = list2[j].ENTQTY2;
                        strROWID = list2[j].ROWID;
                        strList = strList + strSucode + ",";

                        nQty = 1; nQty1 = 1;
                        if(nEntQty > 0 && nEntQty2 >0)
                        {
                            nQty = cf.FIX_N((nEntQty2 / nEntQty) + 0.99);
                            strQty1 = VB.Format(nEntQty2 / nEntQty, "0.00");
                        }


                        //HIC_HYANG UPDATE
                        strROWID2 = hicHyangService.GetRowIdByItems(dtpBDate.Text, nWrtno, strPtno, strSname, strSucode);

                        if(!strROWID2.IsNullOrEmpty())
                        {
                            //UPDATE

                            hicHyangService.UpdateQtybyRowId(strROWID2, nQty, strQty1, nEntQty, nEntQty2);
                        }
                        else
                        {
                            //INSERT
                            HIC_HYANG item = new HIC_HYANG
                            {
                                QTY = nQty.ToString(),
                                REALQTY = Convert.ToDouble(strQty1),
                                ROWID = strROWID
                            };
                            
                            int result = hicHyangService.InsertSelectbyWorId(item);

                            if (result < 0)
                            {
                                MessageBox.Show("마감작업 오류(HIC_HYANG)", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }


                        //OCS_HYANG, OCS_MAYAK 업데이트

                        strROWID2 = comHpcLibBService.MagamSelect(dtpBDate.Text, strPtno, strSucode);

                        if (!strROWID2.IsNullOrEmpty())
                        {
                            int result = comHpcLibBService.MagamUpdate(nQty, strQty1, nEntQty2, strROWID2, strSucode);

                        }
                        else
                        {
                            COMHPC item1 = new COMHPC
                            {
                                SUCODE = strSucode,
                                QTY = Convert.ToDouble(nQty),
                                REALQTY = Convert.ToDouble(strQty1),
                                ROWID = strROWID
                            };
                            
                            int result = comHpcLibBService.MagamInsert(item1);
                        }
                    }

                    if(strList!= "") { strList = VB.Left(strList, VB.Len(strList) - 1); }



                    strList1.Clear();
                    strList1.Add(strList);

                    //오더가 변경된것 삭제 처리
                    //HIC_HYANG
                    hicHyangService.UpdateDelDateByWrtnoPtnoSnameSucodes(nWrtno, strPtno, strSname, strList1);

                    //OCS_MAYAK
                    comHpcLibBService.OcsMayakDelDateByBdatePtnoSnameSucodes(dtpBDate.Text, strPtno, strSname, strList1);

                    //OCS_HYANG
                    comHpcLibBService.OcsHyangDelDateByBdatePtnoSnameSucodes(dtpBDate.Text, strPtno, strSname, strList1);

                    //HIC_HYANG_APPROVE
                    hicHyangApproveService.UpdateOcsSendTimeByItems(dtpBDate.Text, nWrtno, nPano, strPtno, strSname);

                }


                //약국수불마감작업

                //기존자료 삭제 후 자료형성
                comHpcLibBService.DeleteOcsDrug(strBdate);

                //자기 차수에 비치 수량 만큼만 빌드 처리
                List<COMHPC> list1 = comHpcLibBService.GetItemByJdateWardGbn(strBdate);

                if(list1.Count >0)
                {
                    for (int i = 0; i < list1.Count; i++)
                    {
                        COMHPC item = comHpcLibBService.GetQtySucodeOcsDrugSet(list1[i].JDATE, list1[i].SUCODE);
                        nQty = Convert.ToInt64(item.QTY);
                        nJQty = item.QTY;
                        strSucode = item.SUCODE.Trim();

                        //당일제고
                        COMHPC item1 = new COMHPC
                        {
                            BDATE = strBdate,
                            SUCODE = strSucode,
                            QTY = Convert.ToDouble(nQty),
                            REALQTY = Convert.ToDouble(nQty),
                            BUILDDATE = strBdate
                        };
                        int result = comHpcLibBService.InsertOcsDrug(item1);

                        if (nJQty != 0)
                        {
                            List<HIC_HYANG_APPROVE> list2 = hicHyangApproveService.GetItemByBdateSucode(strBdate, strSucode);
                            for (int j = 0; j < list2.Count; j++)
                            {
                                nEntQty = list2[j].ENTQTY;
                                nEntQty2 = list2[j].ENTQTY2;
                                nQty = 1;

                                if (nEntQty > 0 && nEntQty2 > 0)
                                {
                                    nQty = cf.FIX_N((nEntQty2 / nEntQty) + 0.99);
                                }
                                nEntQty3 = Convert.ToDouble(VB.Format(nEntQty2 / nEntQty, "#0.00"));

                                int result1 = comHpcLibBService.InsertOcsDrug1(nQty, nEntQty3, list2[j].ROWID);
                            }
                        }
                    }
                }
                
                // OCS_DRUG INSERT

                int result2 = comHpcLibBService.InsertOcsDrug2(strBdate);

                //ORDERNO_UPDATE();
                
               
                MessageBox.Show("마감작업 완료", "확인", MessageBoxButtons.OK, MessageBoxIcon.None);

                eBtnClick(btnSearch, new EventArgs());


            }
            else if (sender == btnMenuFinishCancel)
            {

                //마감취소
                if (long.Parse(clsType.User.IdNumber) != 31197 && long.Parse(clsType.User.IdNumber) != 36540)
                {
                    MessageBox.Show("마감취소는 최종숙팀장님만 가능합니다!", "확인" ,MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }


                //마감취소
                int result = hicHyangApproveService.UpdateOcsSendTime(dtpBDate.Text);

                if (result < 0)
                {
                    MessageBox.Show("마감취소 오류!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    MessageBox.Show("마감취소 완료!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                eBtnClick(btnSearch, new EventArgs());

            }
            else if (sender == btnNarcotic)
            {
                frmHaDrugList frm = new frmHaDrugList();
                frm.ShowDialog();
            }
            else if (sender == btnEmr)
            {
                frmHcSangEndoOrderClose frm =new frmHcSangEndoOrderClose();
                frm.ShowDialog();
            }
        }

        void ORDERNO_UPDATE()
        {
            string strOrderNo = "";
            string strRowId = "";
            List<COMHPC> list = comHpcLibBService.GetItemByBDate(dtpBDate.Text);
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    strOrderNo = list[i].ORDERNO.ToString();
                    strRowId = list[i].RID;
                    comHpcLibBService.UpdateHyangOrderNo(strOrderNo, strRowId);
                }
            }
        }


        /// <summary>
        /// 처방작성용 자료를 View
        /// </summary>
        void fn_Display_Slip_Create()
        {
            int nREAD = 0;
            int nRead2 = 0;
            int nRow = 0;
            long nAge = 0;
            int  nCNT11 = 0;
            int nCNT12 = 0;
            int nCNT21 = 0;
            int nCNT22 = 0;
            int nCNT31 = 0;
            int nCNT32 = 0;
            long nWRTNO = 0;
            long nPano = 0;
            string strOK = "";
            string strOK2 = "";
            string strPtNo = "";
            string strJEPDATE = "";
            string strSDate = "";
            string strDeptCode = "";
            string strROWID = "";
            string strDrno = "";
            string strDRSABUN = "";
            string strGbSite = "";
            string strSname = "";
            string strNRSABUN = "";
            string strSEX = "";
            string strJumin = "";
            string strJong = "";
            string strPrint = "";
            double nPOL2 = 0;
            double nPOL12G = 0;
            double nPOL8G = 0;
            double nBASCA = 0;
            double nBASCAM = 0;
            double nPTDHA = 0;
            double nPTD25 = 0;
            double nPOL2_E = 0;
            double nPOL12G_E = 0;
            double nPOL8G_E = 0;
            double nBASCA_E = 0;
            double nBASCAM_E = 0;
            double nPTDHA_E = 0;
            double nPTD25_E = 0;
            double[,] nTOT = new double[4, 1];
            string strHic = "";
            string strSlipList = ""; //이미 발급한 목록 형식: "{}Pano,SName{}...."
            string strGbApprove = "";
            string strGBPrint = "";
            string strGbSend = "";
            bool bTX32 = false;

            string strBDate = "";

            if (string.Compare(dtpBDate.Text, clsPublic.GstrSysDate) > 0)
            {
                MessageBox.Show("처방작성은 오늘까지만 가능합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //마감이 되었으면 처방작성은 불가능
            if (fn_Magam_Check() == true)
            {
                MessageBox.Show(dtpBDate.Text + " 일은 마감이 되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            bFlag = false;
            nRow = 0;
            sp.Spread_All_Clear(SS1);

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    nTOT[i, j] = 0;
                }
            }

            //===================================================
            // 1.이미 승인 의뢰된 내역을 Display
            //===================================================
            strSDate = dtpSDate.Text;
            strBDate = dtpBDate.Text;
            List<HIC_HYANG_APPROVE> list = hicHyangApproveService.GetItembyBDate(strSDate, long.Parse(clsType.User.IdNumber));

            nREAD = list.Count;
            //SS1.ActiveSheet.RowCount = nREAD;
            strSlipList = "{}";
            for (int i = 0; i < nREAD; i++)
            {
                nWRTNO = list[i].WRTNO;
                nPano = list[i].PANO;
                strSname = list[i].SNAME;
                strSlipList += list[i].PTNO + "," + strSname + "{}";

                if(strSname =="전산실연습")
                {
                    strSname = strSname;
                }

                strOK = "OK";
                if (list[i].DEPTCODE != "TO")
                {
                    if (heaJepsuService.GetCountbyPtNo(list[i].PTNO, strBDate) == 0)
                    {
                        strOK = "";
                    }

                    if(hicHyangApproveService.GetCountByPtNoBdate(list[i].PTNO, strBDate) > 0)
                    {
                        strOK = "";
                    }
                }
               
                if (strOK == "OK")
                {

                    //SS1.ActiveSheet.RowCount = nREAD;
                    nRow += 1;
                    if (nRow > SS1.ActiveSheet.RowCount) { SS1.ActiveSheet.RowCount = nRow; }

                    SS1.ActiveSheet.Cells[nRow-1, 0].Text = "";
                    SS1.ActiveSheet.Cells[nRow-1, 1].Text = nPano.ToString();
                    SS1.ActiveSheet.Cells[nRow-1, 3].Text = strSname;
                    SS1.ActiveSheet.Cells[nRow-1, 5].Text = nWRTNO.ToString();

                    //처방발급 상세내역을 Display
                    List<HIC_HYANG_APPROVE> list2 = hicHyangApproveService.GetItembyWrtNo(nWRTNO, strBDate, nPano);

                    nRead2 = list2.Count;
                    strGbApprove = "";
                    strGBPrint = "";
                    strGbSend = "";
                    strDRSABUN = "";

                    nCNT11 = 0;
                    nCNT12 = 0;
                    nCNT21 = 0;
                    nCNT22 = 0;
                    nCNT31 = 0;
                    nCNT32 = 0;
                    nPOL2 = 0;
                    nPOL12G = 0;
                    nPOL8G = 0;
                    nBASCA = 0;
                    nBASCAM = 0;
                    nPTDHA = 0;
                    nPTD25 = 0;

                    for (int j = 0; j < nRead2; j++)
                    {
                        if (j == 0)
                        {
                            strJumin = clsAES.DeAES(list2[j].JUMIN2.Trim());
                            strPtNo = list2[j].PTNO.Trim();
                            strSEX = list2[j].SEX;
                            nAge = list2[j].AGE;
                            if (strNRSABUN == "")
                            {
                                strNRSABUN = list2[j].NRSABUN.Trim();
                            }
                            SS1.ActiveSheet.Cells[nRow-1, 2].Text = list2[j].AMPM == "2" ? "오후" : " ";
                            SS1.ActiveSheet.Cells[nRow-1, 4].Text = nAge + "/" + strSEX;
                            SS1.ActiveSheet.Cells[nRow-1, 16].Text = hb.READ_Sabun_Name(list2[j].DRSABUN).Trim(); ;
                            SS1.ActiveSheet.Cells[nRow-1, 17].Text = VB.Left(strJumin, 6) + "-" + VB.Mid(strJumin, 7, 1) + "******";
                            SS1.ActiveSheet.Cells[nRow-1, 18].Text = strPtNo;
                            if (!list2[j].DRSABUN.IsNullOrEmpty())
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 30].Text = list2[j].DRSABUN.Trim();
                            }
                            
                            SS1.ActiveSheet.Cells[nRow-1, 28].Text = " ";
                            if (list2[j].GBSITE == "2")
                            {
                                SS1.ActiveSheet.Cells[nRow-1, 28].Text = "◎";    //본관내시경실
                                SS1.ActiveSheet.Cells[nRow-1, 28].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H80FFFF"));
                            }
                            if (list2[j].DEPTCODE.Trim() == "HR")
                            {   
                                SS1.ActiveSheet.Cells[nRow-1, 5].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H80C0FF")); 
                                SS1.ActiveSheet.Cells[nRow-1, 26].Text = "HR";
                            }
                            else
                            {
                                SS1.ActiveSheet.Cells[nRow-1, 5].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                                SS1.ActiveSheet.Cells[nRow-1, 26].Text = "TO";
                                //일반건진 암검진 접수
                                if (fn_HIC_Jepsu_Check(strPtNo, strBDate) == true)
                                {
                                    SS1.ActiveSheet.Cells[nRow-1, 5].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFEEFD6"));
                                }
                            }
                            SS1.ActiveSheet.Cells[nRow-1, 31].Text = list2[j].GBSLEEP == "Y" ? "◎" : " "; //수면
                            SS1.ActiveSheet.Cells[nRow-1, 32].Text = VB.Left(strJumin, 6) + "-" + VB.Right(strJumin, 7);
                        }

                        //의사승인
                        if (list2[j].APPROVETIME == null)
                        {
                            nCNT11 += 1;
                        }
                        else
                        {
                            nCNT12 += 1;
                        }
                        //인쇄여부
                        if (list2[j].PRINT != "Y")
                        {
                            nCNT21 += 1;
                        }
                        else
                        {
                            nCNT22 += 1;
                        }
                        //전송여부
                        if (list2[j].OCSSENDTIME != null)
                        {
                            nCNT31 += 1;
                        }
                        else
                        {
                            nCNT32 += 1;
                        }
                        //약품별 처방수량을 표시함
                        switch (list2[j].SUCODE.Trim())
                        {
                            case "A-POL2":
                                nPOL2 = list2[j].ENTQTY;
                                break;
                            case "A-ANE12G":
                                nPOL12G = list2[j].ENTQTY;
                                break;
                            case "A-POL8G":
                                nPOL8G = list2[j].ENTQTY;
                                break;
                            case "A-BASCA":
                                nBASCA = list2[j].ENTQTY;
                                break;
                            case "A-BASCAM":
                                nBASCAM = list2[j].ENTQTY;
                                break;
                            case "N-PTD-HA":
                                nPTDHA = list2[j].ENTQTY;
                                break;
                            case "N-PTD25":
                                nPTD25 = list2[j].ENTQTY;
                                break;
                            default:
                                break;
                        }
                    }

                    SS1.ActiveSheet.Cells[nRow-1, 6].Text = nPOL2 > 0 ? "True" : "";
                    SS1.ActiveSheet.Cells[nRow-1, 7].Text = nPOL2 > 0 ? string.Format("{0:#0.00}", nPOL2): "";
                    SS1.ActiveSheet.Cells[nRow-1, 8].Text = nPOL12G > 0 ? "True" : "";
                    SS1.ActiveSheet.Cells[nRow-1, 9].Text = nPOL12G > 0 ? string.Format("{0:#0.00}", nPOL12G) : "";
                    SS1.ActiveSheet.Cells[nRow-1, 10].Text = nPOL8G > 0 ? "True" : "";
                    SS1.ActiveSheet.Cells[nRow-1, 11].Text = nPOL8G > 0 ? string.Format("{0:#0.00}", nPOL8G) : "";

                    SS1.ActiveSheet.Cells[nRow-1, 12].Text = nBASCA > 0 ? "True" : "";
                    SS1.ActiveSheet.Cells[nRow-1, 13].Text = nBASCA > 0 ? string.Format("{0:#0.00}", nBASCA) : "";
                    if (nBASCAM != 0)
                    {
                        SS1.ActiveSheet.Cells[nRow-1, 12].Text = nBASCAM > 0 ? "True" : "";
                        SS1.ActiveSheet.Cells[nRow-1, 13].Text = nBASCAM > 0 ? string.Format("{0:#0.00}", nBASCAM) : "";
                    }
                    SS1.ActiveSheet.Cells[nRow-1, 14].Text = nPTD25 > 0 ? "True" : "";
                    SS1.ActiveSheet.Cells[nRow-1, 15].Text = nPTD25 > 0 ? string.Format("{0:#0.00}", nPTD25) : "";

                    //출력
                    if (nCNT21 == 0 && nCNT22 == 0)
                    {
                        SS1.ActiveSheet.Cells[nRow-1, 24].Text = "N";
                    }
                    if (nCNT21 == 0 && nCNT22 > 0)
                    {
                        SS1.ActiveSheet.Cells[nRow - 1, 24].Text = "Y";
                    }
                    if (nCNT21 > 0 && nCNT22 > 0)
                    {
                        SS1.ActiveSheet.Cells[nRow - 1, 24].Text = "일부";
                    }

                    //승인
                    if (nCNT11 == 0 && nCNT12 == 0)
                    {
                        SS1.ActiveSheet.Cells[nRow - 1, 27].Text = "N";
                    }
                    if (nCNT11 == 0 && nCNT12 > 0)
                    {
                        SS1.ActiveSheet.Cells[nRow - 1, 27].Text = "Y";
                    }
                    if (nCNT11 > 0 && nCNT12 > 0)
                    {
                        SS1.ActiveSheet.Cells[nRow - 1, 27].Text = "일부";
                    }

                    //OCS 전송
                    if (nCNT31 == 0 && nCNT32 == 0)
                    {
                        strGbSend = "N";
                    }
                    if (nCNT31 == 0 && nCNT32 > 0)
                    {
                        strGbSend = "Y";
                    }
                    if (nCNT31 > 0 && nCNT32 > 0)
                    {
                        strGbSend = "일부";
                    }

                    SS1.ActiveSheet.Cells[nRow-1, 19].Text = "검사용";
                    SS1.ActiveSheet.Cells[nRow-1, 20].Text = "Pain";
                    SS1.ActiveSheet.Cells[nRow-1, 21].Text = "";
                    SS1.ActiveSheet.Cells[nRow-1, 22].Text = "";
                    SS1.ActiveSheet.Cells[nRow-1, 23].Text = "";
                    SS1.ActiveSheet.Cells[nRow - 1, 24].Text = "";
                }
            }

            //===================================================
            // 2.추가로 처방작성 명단 표시
            //===================================================

            string strBDateFr = strBDate;
            string strBDateTo = DateTime.Parse(strBDate).AddDays(1).ToShortDateString();

            List<HEA_RESV_EXAM_PATIENT> list3 = heaResvExamPatientService.GetItembyRTime(strBDateFr, strBDateTo, long.Parse(clsType.User.IdNumber));

            nREAD = list3.Count;
            for (int i = 0; i < nREAD; i++)
            {
                strHic = "";
                strOK = "OK";

                nPano = list3[i].PANO;
                strSname = list3[i].SNAME.Trim();
                strPtNo = list3[i].PTNO.Trim();

                if (VB.InStr(strSlipList, "{}" + strPtNo + "," + strSname + "{}") == 0)
                {
                    strSDate = VB.Left(list3[i].SDATE.ToString(),10);
                    strJumin = clsAES.DeAES(list3[i].JUMIN2);

                    HEA_JEPSU list4 = heaJepsuService.GetWrtNoAgebyPtNo(strPtNo, strSDate);

                    nWRTNO = 0;
                    nAge = 0;
                    if (list4 != null)
                    {
                        nWRTNO = list4.WRTNO;
                        nAge = list4.AGE;
                    }
                    else
                    {
                        nWRTNO = 0;
                    }

                    //수면내시경 대상자인지 확인
                    List<HIC_RESULT_EXCODE> list5 = hicResultExCodeService.GetItembyWrtNo_Stomach(nWRTNO);
                    
                    strOK = "";
                    strGbSite = "1";    //종검 내시경실
                    bTX32 = false;
                    if (list5.Count > 0)
                    {
                        strOK = "OK";
                        for (int j = 0; j < list5.Count; j++)
                        {


                            if(!list5[j].ENDOGUBUN3.IsNullOrEmpty())
                            {
                                if (list5[j].ENDOGUBUN3.Trim() == "Y")
                                {
                                    strOK = "OK";
                                }
                            }

                            if (!list5[j].ENDOGUBUN4.IsNullOrEmpty())
                            {
                                if (list5[j].ENDOGUBUN4.Trim() == "Y")
                                {
                                    strGbSite = "2"; //본관내시경실
                                }
                            }

                            if (!list5[j].ENDOGUBUN5.IsNullOrEmpty())
                            {
                                if (list5[j].ENDOGUBUN5.Trim() == "Y")
                                {
                                    strGbSite = "2"; //본관내시경실
                                }
                            }

                            if (list5[j].EXCODE.Trim() == "TX32")
                            {
                                bTX32 = true;
                            }
                        }
                    }
                    //위수면내시경 대상자가 아니면 일반건진 가접수 내역을 읽음
                    if (strOK == "")
                    {
                        if (fn_Read_Hic_GaJepsu(strPtNo) == "S")
                        {
                            strHic = "OK";
                        }
                    }

                    //일반검진에서 위내시경 대상인지 확인
                    nPOL2 = 0;
                    nPOL12G = 0;
                    nPOL8G = 0;
                    nBASCA = 0;
                    nBASCAM = 0;
                    nPTDHA = 0;
                    nPTD25 = 0;
                    nPOL2_E = 0;
                    nPOL12G_E = 0;
                    nPOL8G_E = 0;
                    nBASCA_E = 0;
                    nBASCAM_E = 0;
                    nPTDHA_E = 0;
                    nPTD25_E = 0;
                    strDRSABUN = "";
                    strJong = "";
                    strPrint = "";

                    if (strOK == "OK")
                    {
                        nRow = nRow + 1;
                        if( SS1.ActiveSheet.RowCount < nRow) { SS1.ActiveSheet.RowCount = nRow; }
                        //기본사용량 세팅
                        if (string.Compare(clsPublic.GstrSysDate, "2015-01-03") >= 0 && string.Compare(clsPublic.GstrSysDate, "2015-01-03") <= 0 && strGbSite == "1")
                        {
                            strGbSite = "2";    //본관내시경실
                            if (bTX32 == false)
                            {
                                if (nPOL12G == 0)
                                {
                                    nPOL12G = 12;
                                }
                            }
                        }
                        else if (strGbSite == "1")  //종합검진
                        {
                            if (nPOL2 == 0)
                            {
                                SS1.ActiveSheet.Cells[nRow-1, 0].Text = "";
                            }
                            else
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 0].Text = "1";
                            }
                            if (string.Compare(strSDate, "2014-12-09") <= 0)
                            {
                                if (nPOL2 == 0)
                                {
                                    nPOL2 = 20;
                                }
                            }
                            else
                            {
                                if (nPOL12G == 0)
                                {
                                    nPOL12G = 12;
                                }
                            }
                        }
                        else
                        {
                            if (bTX32 == false)
                            {
                                if (nPOL12G == 0)
                                {
                                    nPOL12G = 12;
                                }
                                if (nBASCAM == 0)
                                {
                                    nBASCAM = 5;
                                }
                            }
                            if (nPTD25 == 0 || nPTD25 == 1)
                            {
                                nPTD25 = 25;
                            }
                        }
                        SS1.ActiveSheet.Cells[nRow-1, 0].Text = "True";
                        SS1.ActiveSheet.Cells[nRow-1, 1].Text = list3[i].PANO.ToString();
                        SS1.ActiveSheet.Cells[nRow-1, 2].Text = list3[i].AMPM2 == "2" || list3[i].AMPM2 == "P" ? "오후" : " ";
                        SS1.ActiveSheet.Cells[nRow-1, 3].Text = list3[i].SNAME;
                        SS1.ActiveSheet.Cells[nRow-1, 4].Text = nAge + "/" + list3[i].SEX;
                        SS1.ActiveSheet.Cells[nRow - 1, 5].Text = nWRTNO.ToString();

                        SS1.ActiveSheet.Cells[nRow-1, 6].Text = nPOL2 > 0 ? "True" : "0";
                        SS1.ActiveSheet.Cells[nRow-1, 7].Text = nPOL2 > 0 ? string.Format("{0:#0.00}", nPOL2) : "";
                        SS1.ActiveSheet.Cells[nRow-1, 8].Text = nPOL12G > 0 ? "True" : "0";
                        SS1.ActiveSheet.Cells[nRow-1, 9].Text = nPOL12G > 0 ? string.Format("{0:#0.00}", nPOL12G) : "";
                        SS1.ActiveSheet.Cells[nRow-1, 10].Text = nPOL8G > 0 ? "True" : "0";
                        SS1.ActiveSheet.Cells[nRow - 1, 11].Text = nPOL8G > 0 ? string.Format("{0:#0.00}", nPOL8G) : "";

                        SS1.ActiveSheet.Cells[nRow-1, 12].Text = nBASCA > 0 ? "True" : "0";
                        SS1.ActiveSheet.Cells[nRow - 1, 13].Text = nBASCA > 0 ? string.Format("{0:#0.00}", nBASCA) : "";

                        if (nBASCAM != 0)
                        {
                            SS1.ActiveSheet.Cells[nRow-1, 12].Text = nBASCAM > 0 ? "True" : "0";
                            SS1.ActiveSheet.Cells[nRow - 1, 13].Text = nBASCAM > 0 ? string.Format("{0:#0.00}", nBASCAM) : "";
                        }
                        SS1.ActiveSheet.Cells[nRow-1, 14].Text = nPTD25 > 0 ? "True" : "";
                        SS1.ActiveSheet.Cells[nRow-1, 15].Text = nPTD25 > 0 ? string.Format("{0:#0.00}", nPTD25) : "";
                        SS1.ActiveSheet.Cells[nRow-1, 16].Text = ""; //처방의사명
                        SS1.ActiveSheet.Cells[nRow-1, 17].Text = VB.Left(strJumin.Trim(), 6) + "-" + VB.Mid(strJumin.Trim(), 7, 1) + "******";
                        SS1.ActiveSheet.Cells[nRow-1, 18].Text = list3[i].PTNO.Trim();
                        SS1.ActiveSheet.Cells[nRow-1, 19].Text = "검사용";
                        SS1.ActiveSheet.Cells[nRow-1, 20].Text = "Pain";
                        SS1.ActiveSheet.Cells[nRow-1, 21].Text = list3[i].JUSO.Trim();
                        SS1.ActiveSheet.Cells[nRow - 1, 22].Text = list3[i].SDATE.ToString();

                        SS1.ActiveSheet.Cells[nRow-1, 24].Text = strPrint;
                        SS1.ActiveSheet.Cells[nRow-1, 25].Text = "";
                        SS1.ActiveSheet.Cells[nRow-1, 31].Text = bTX32 == true ? " " : "◎";
                        SS1.ActiveSheet.Cells[nRow - 1, 32].Text = VB.Left(strJumin.Trim(), 6) + "-" + VB.Mid(strJumin.Trim(), 7, 7);
                        if (strGbSite == "1")
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 33].Text = "1";
                            if (list3[i].AMPM2 == "2" || list3[i].AMPM2 == "P")
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 33].Text = "2";
                            }
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 33].Text = "";
                        }

                        SS1.ActiveSheet.Cells[nRow - 1, 28].Text = " ";
                        if (strGbSite == "2")
                        {
                            SS1.ActiveSheet.Cells[nRow-1, 28].Text = "◎";    //본관내시경실
                            SS1.ActiveSheet.Cells[nRow - 1, 3].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H80FFFF"));
                        }

                        if (strHic == "OK")
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 5].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H80C0FF"));
                            SS1.ActiveSheet.Cells[nRow-1, 26].Text = "HR";
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[nRow-1, 5].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                            SS1.ActiveSheet.Cells[nRow - 1, 26].Text = "TO";
                            //일반건진 암검진 접수
                            if (fn_HIC_Jepsu_Check(strPtNo, dtpBDate.Text) == true)
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 5].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFEEFD6"));
                            }
                        }
                    }
                }
            }

            //========================================================
            //3.일반검진으로 수면내시경을 한 경우 추가 명단 표시
            //========================================================
            strBDate = dtpBDate.Text;
            List<HEA_JEPSU_PATIENT> list6 = heaJepsuPatientService.GetItembySDate(strBDate, long.Parse(clsType.User.IdNumber));

            nREAD = list6.Count;
            for (int i = 0; i < nREAD; i++)
            {
                nPano = list6[i].PANO;
                strSname = list6[i].SNAME.Trim();
                strPtNo = list6[i].PTNO.Trim();

                if (VB.InStr(strSlipList, "{}" + strPtNo + strSname + "{}") == 0)
                {
                    nWRTNO = list6[i].WRTNO;
                    strSDate = list6[i].SDATE;
                    strJumin = clsAES.DeAES(list6[i].JUMIN2);

                    //일반건진 접수내역을 읽음
                    strOK = "";
                    strHic = "";
                    if (fn_Hic_SleepEndoScope_Check(strPtNo, strBDate) == true)
                    {
                        strOK = "OK";
                        strHic = "OK";
                        strGbSite = "1";
                        if (string.Compare(clsPublic.GstrSysDate, "2015-01-03") >= 0 && string.Compare(clsPublic.GstrSysDate, "2015-05-03") <= 0)
                        {
                            strGbSite = "2";    //본관내시경실
                        }
                    }

                    if (strOK == "OK")
                    {
                        nPOL2 = 0;
                        nPOL12G = 0;
                        nPOL8G = 0;
                        nBASCA = 0;
                        nBASCAM = 0;
                        nPTDHA = 0;
                        nPTD25 = 0;
                        nPOL2_E = 0;
                        nPOL12G_E = 0;
                        nPOL8G_E = 0;
                        nBASCA_E = 0;
                        nBASCAM_E = 0;
                        nPTDHA_E = 0;
                        nPTD25_E = 0;
                        strDRSABUN = "";
                        strJong = "";
                        strPrint = "";

                        nRow = nRow + 1;
                        if(SS1.ActiveSheet.RowCount < nRow) { SS1.ActiveSheet.RowCount = nRow; }


                        HEA_JEPSU list7 = heaJepsuService.GetWrtNoAgebyPtNo(strPtNo, strSDate);

                        nWRTNO = 0;
                        nAge = 0;
                        if (list7 != null)
                        {
                            nWRTNO = list7.WRTNO;
                            nAge = list7.AGE;
                        }

                        //기본사용량 세팅
                        if (strGbSite == "1")   //종합검진
                        {
                            if (nPOL2 == 0)
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 0].Text = "";
                            }
                            else
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 0].Text = "True";
                            }
                            if (string.Compare(strSDate, "2014-12-09") <= 0)
                            {
                                if (nPOL2 == 0)
                                {
                                    nPOL2 = 20;
                                }
                            }
                            else
                            {
                                if (nPOL12G == 0)
                                {
                                    nPOL12G = 12;
                                }
                            }
                        }
                        else
                        {
                            if (bTX32 == false)
                            {
                                if (nPOL8G == 0)
                                {
                                    nPOL8G = 8;
                                }
                                if (nBASCAM == 0)
                                {
                                    nBASCAM = 5;
                                }
                            }
                            if (nPTD25 == 0 || nPTD25 == 1)
                            {
                                nPTD25 = 25;
                            }
                        }
                        SS1.ActiveSheet.Cells[nRow - 1, 0].Text = "True";
                        SS1.ActiveSheet.Cells[nRow - 1, 1].Text = list7.PANO.ToString();
                        SS1.ActiveSheet.Cells[nRow - 1, 2].Text = list7.AMPM2 == "2" || list7.AMPM2 == "P" ? "오후" : " ";
                        SS1.ActiveSheet.Cells[nRow - 1, 3].Text = list7.SNAME.Trim();
                        SS1.ActiveSheet.Cells[nRow - 1, 4].Text = nAge.ToString() + "/" + list7.SEX.Trim();
                        SS1.ActiveSheet.Cells[nRow - 1, 5].Text = nWRTNO.ToString();

                        SS1.ActiveSheet.Cells[nRow - 1, 6].Text = nPOL2 > 0 ? "True" : "0";
                        SS1.ActiveSheet.Cells[nRow - 1, 7].Text = nPOL2 > 0 ? string.Format("{0:#0.00}", nPOL2) : "";
                        SS1.ActiveSheet.Cells[nRow - 1, 8].Text = nPOL12G > 0 ? "True" : "0";
                        SS1.ActiveSheet.Cells[nRow - 1, 9].Text = nPOL12G > 0 ? string.Format("{0:#0.00}", nPOL12G) : "";
                        SS1.ActiveSheet.Cells[nRow - 1, 10].Text = nPOL8G > 0 ? "True" : "0";
                        SS1.ActiveSheet.Cells[nRow - 1, 11].Text = nPOL8G > 0 ? string.Format("{0:#0.00}", nPOL8G) : "";

                        SS1.ActiveSheet.Cells[nRow - 1, 12].Text = nBASCA > 0 ? "True" : "0";
                        SS1.ActiveSheet.Cells[nRow - 1, 13].Text = nBASCA > 0 ? string.Format("{0:#0.00}", nBASCA) : "";

                        if (nBASCAM != 0)
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 12].Text = nBASCAM > 0 ? "True" : "0";
                            SS1.ActiveSheet.Cells[nRow - 1, 13].Text = nBASCAM > 0 ? string.Format("{0:#0.00}", nBASCAM) : "";
                        }
                        SS1.ActiveSheet.Cells[nRow - 1, 14].Text = nPTD25 > 0 ? "True" : "";
                        SS1.ActiveSheet.Cells[nRow - 1, 15].Text = nPTD25 > 0 ? string.Format("{0:#0.00}", nPTD25) : "";
                        SS1.ActiveSheet.Cells[nRow - 1, 16].Text = ""; //처방의사명
                        SS1.ActiveSheet.Cells[nRow - 1, 17].Text = VB.Left(strJumin.Trim(), 6) + "-" + VB.Mid(strJumin.Trim(), 7, 1) + "******";
                        SS1.ActiveSheet.Cells[nRow - 1, 18].Text = list3[i].PTNO.Trim();
                        SS1.ActiveSheet.Cells[nRow - 1, 19].Text = "검사용";
                        SS1.ActiveSheet.Cells[nRow - 1, 20].Text = "Pain";
                        SS1.ActiveSheet.Cells[nRow - 1, 21].Text = list3[i].JUSO.Trim();
                        SS1.ActiveSheet.Cells[nRow - 1, 22].Text = list3[i].SDATE.ToString();

                        SS1.ActiveSheet.Cells[nRow - 1, 24].Text = strPrint;
                        SS1.ActiveSheet.Cells[nRow - 1, 25].Text = "";
                        SS1.ActiveSheet.Cells[nRow - 1, 31].Text = bTX32 == true ? " " : "◎";
                        SS1.ActiveSheet.Cells[nRow - 1, 32].Text = VB.Left(strJumin.Trim(), 6) + "-" + VB.Mid(strJumin.Trim(), 7, 7);
                        if (strGbSite == "1")
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 33].Text = "1";
                            if (list3[i].AMPM2 == "2" || list3[i].AMPM2 == "P")
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 33].Text = "2";
                            }
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 33].Text = "";
                        }

                        SS1.ActiveSheet.Cells[nRow - 1, 28].Text = " ";
                        if (strGbSite == "2")
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 28].Text = "◎";    //본관내시경실
                            SS1.ActiveSheet.Cells[nRow - 1, 3].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H80FFFF"));
                        }

                        if (strHic == "OK")
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 5].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H80C0FF"));
                            SS1.ActiveSheet.Cells[nRow - 1, 26].Text = "HR";
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 5].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                            SS1.ActiveSheet.Cells[nRow - 1, 26].Text = "TO";
                            //일반건진 암검진 접수
                            if (fn_HIC_Jepsu_Check(strPtNo, dtpBDate.Text) == true)
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 5].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFEEFD6"));
                            }
                        }
                    }
                }
            }
            
            bFlag = true;

            //sp.setSpdSort(SS1, 28, true);
            //sp.setSpdSort(SS1, 2, true);
            //sp.setSpdSort(SS1, 3, true);

            //SS1.ActiveSheet.SortRows(28, true, true);
            //SS1.ActiveSheet.SortRows(2, true, true);
            //SS1.ActiveSheet.SortRows(3, true, true);

            //3개의 컬럼에 대해서 정렬 배열선언

            FarPoint.Win.Spread.SortInfo[] sort = new FarPoint.Win.Spread.SortInfo[3];

            //첫번째 컬럼은 오름차순 정렬 **1순위
            sort[0] = new FarPoint.Win.Spread.SortInfo(28, true);
            //두번째 컬럼은 내림차순 정렬 **2순위
            sort[1] = new FarPoint.Win.Spread.SortInfo(2, true);
            //세번째 컬럼은 오름차순 정렬 **3순위
            sort[2] = new FarPoint.Win.Spread.SortInfo(3, true);

            //첫번째 열부터 마지막 열까지
            // 1순위로 첫번째 컬럼 오름차순, 2순위로 두번째 컬럼 내림차순, 3순위로 세번째 컬럼 오름차순 정렬한다. (=멀티정렬)
            SS1.ActiveSheet.SortRows(0, SS1.ActiveSheet.RowCount, sort);


        }

        /// <summary>
        /// 종검환자가 암검진으로 수면내시경을 하였는지 점검 (HIC_수면내시경_Check())
        /// </summary>
        /// <param name="argPtNo"></param>
        /// <param name="argBDate"></param>
        /// <returns></returns>
        bool fn_Hic_SleepEndoScope_Check(string argPtNo, string argBDate)
        {
            bool rtnval = false;
            long nPano = 0;
            long nWRTNO = 0;

            //일반건진 접수번호를 찾음
            nPano = hicPatientService.GetPanobyPtno(argPtNo);

            if (nPano == 0)
            {
                rtnval = false;
                return rtnval;
            }

            //수면내시경이 있는지 점검
            nWRTNO = hicJepsuService.GetWrtnoByPanoJepDate(nPano, argBDate);

            if (nWRTNO == 0)
            {
                rtnval = false;
                return rtnval;
            }

            if (hicResultExCodeService.GetCountbyWrtNo(nWRTNO) > 0)
            {
                rtnval = true;
            }
            else
            {
                rtnval = false;
            }

            return rtnval;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argPtNo"></param>
        /// <returns>Return 내용 D: 대장내시경, S: 수면내시경, E: 일반내시경, Y: 가접수있음</returns>
        string fn_Read_Hic_GaJepsu(string argPtNo)
        {
            string rtnVal = "";

            int nREAD = 0;
            string SSQL = "";

            List<HIC_JEPSU_SUNAP> list = hicJepsuSunapService.GetCodebyPtNo(argPtNo);

            nREAD = list.Count;
            if (nREAD > 0)
            {
                rtnVal = "Y";
                for (int i = 0; i < nREAD; i++)
                {
                    List<HIC_GROUPEXAM_EXCODE> list2 = hicGroupexamExcodeService.GetEndoGubunbyGroupCode(list[i].CODE.Trim());
                    if (list2.Count > 0)
                    {
                        for (int j = 0; j < list2.Count; j++)
                        {

                            if(!list2[j].ENDOGUBUN4.IsNullOrEmpty())
                            {
                                if(list2[j].ENDOGUBUN4.Trim() == "Y")
                                {
                                    rtnVal = "D";
                                    break;
                                }
                            }

                            if (!list2[j].ENDOGUBUN5.IsNullOrEmpty())
                            {
                                if (list2[j].ENDOGUBUN5.Trim() == "Y")
                                {
                                    rtnVal = "D";
                                    break;
                                }
                            }

                            if (!list2[j].ENDOGUBUN3.IsNullOrEmpty())
                            {
                                if (list2[j].ENDOGUBUN3.Trim() == "Y")
                                {
                                    rtnVal = "S";
                                    break;
                                }
                            }

                            if (!list2[j].ENDOGUBUN2.IsNullOrEmpty())
                            {
                                if (list2[j].ENDOGUBUN2.Trim() == "Y")
                                {
                                    rtnVal = "E";
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            List<HIC_JEPSU_SUNAP> list3 = hicJepsuSunapService.GetWrtNoCodebyPtNo(argPtNo);

            nREAD = list3.Count;

            rtnVal = "Y";

            if (nREAD > 0)
            {
                for (int i = 0; i < nREAD; i++)
                {
                    List<HIC_GROUPEXAM_EXCODE> list2 = hicGroupexamExcodeService.GetEndoGubunbyGroupCode(list3[i].CODE.Trim());
                    if (list2.Count > 0)
                    {
                        for (int j = 0; j < list2.Count; j++)
                        {
                            if (!list2[j].ENDOGUBUN4.IsNullOrEmpty() && !list2[j].ENDOGUBUN5.IsNullOrEmpty())
                            {
                                if (list2[j].ENDOGUBUN4.IsNullOrEmpty() && list2[j].ENDOGUBUN4.Trim() == "Y" || list2[j].ENDOGUBUN5.Trim() == "Y")
                                {
                                    rtnVal = "D";
                                    break;
                                }
                            }

                            if (!list2[j].ENDOGUBUN3.IsNullOrEmpty() && list2[j].ENDOGUBUN3.Trim() == "Y")
                            {
                                rtnVal = "S";
                                break;
                            }

                            if (!list2[j].ENDOGUBUN2.IsNullOrEmpty() &&  list2[j].ENDOGUBUN2.Trim() == "Y")
                            {
                                rtnVal = "E";
                                break;
                            }
                        }
                    }
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 종검환자가 암검진으로 수면내시경을 하였는지 점검
        /// </summary>
        /// <param name="strPtNo"></param>
        /// <param name="strBDate"></param>
        /// <returns></returns>
        bool fn_HIC_Jepsu_Check(string strPtNo, string strBDate)
        {
            bool rtnVal = false;
            long nPano = 0;
            long nWRTNO = 0;

            //일반건진 접수번호를 찾음
            nPano = hicPatientService.GetPanobyPtno(strPtNo);

            if (nPano == 0)
            {
                rtnVal = false;
                return rtnVal;
            }

            //암검진이 있는지 점검
            nWRTNO = hicJepsuService.GetWrtnoByPanoJepDate(nPano, strBDate);
            rtnVal = true;

            if (nWRTNO == 0)
            {
                rtnVal = false;
            }

            return rtnVal;
        }

        /// <summary>
        /// 마감 여부를 점검함btnMenuFinishOk
        /// </summary>
        /// <returns></returns>
        bool fn_Magam_Check()
        {
            bool rtnVal = false;
            string strBDate = "";

            strBDate = dtpBDate.Text;

            if (hicHyangApproveService.GetCountbyBDate(strBDate) > 0)
            {
                rtnVal = true;
            }

            return rtnVal;
        }

        /// <summary>
        /// 처방작성용 자료를 View
        /// </summary>
        void fn_Display_Slip_Print()
        {
            int nREAD = 0;
            int nRead2 = 0;
            int nRow = 0;
            long nAge = 0;
            int nCNT11 = 0;
            int nCNT12 = 0;
            int nCNT21 = 0;
            int nCNT22 = 0;
            int nCNT31 = 0;
            int nCNT32 = 0;
            long nWRTNO = 0;
            long nPano = 0;
            string strOK = "";
            string strOK2 = "";
            string strPtNo = "";
            string strJEPDATE = "";
            string strSDate = "";
            string strBDATE = "";
            string strDeptCode = "";
            string strROWID = "";
            string strDrno = "";
            string strDRSABUN = "";
            string strGbSite = "";
            string strSname = "";
            string strNRSABUN = "";
            string strSEX = "";
            string strJumin = "";
            string strJong = "";
            string strPrint="";
            double nPOL2 = 0;
            double nPOL12G = 0;
            double nPOL8G = 0;
            double nBASCA = 0;
            double nPTDHA = 0;
            double nPTD25 = 0;
            double nPOL2_E = 0;
            double nPOL12G_E = 0;
            double nPOL8G_E = 0;
            double nBASCA_E = 0;
            double nPTDHA_E = 0;
            double nPTD25_E = 0;
            double[,] nTOT = new double[4, 1];
            string strHic = "";
            string strSlipList = ""; //이미 발급한 목록 형식: "{}Pano,SName{}...."
            string strGbApprove = "";
            string strGBPrint = "";
            string strGbSend = "";
            string strJuso = "";

            string strBDate = "";

            bFlag = false;
            nRow = 0;
            sp.Spread_All_Clear(SS1);

            strBDate = dtpBDate.Text;

            for (int i = 0; i <= 4; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    nTOT[i, j] = 0;
                }
            }

            //=================================
            // 승인된 의뢰된 내역을 Display
            //=================================
            List<HIC_HYANG_APPROVE> list = hicHyangApproveService.GetItembyBDate(dtpBDate.Text);

            //SS1.ActiveSheet.RowCount =  list.Count;

            strSlipList = "{}";
            for (int i = 0; i < nREAD; i++)
            {
                nWRTNO = list[i].WRTNO;
                nPano = list[i].PANO;
                strSname = list[i].SNAME;
                strSlipList += list[i].PTNO + "," + strSname + "{}";

                if (list[i].PRINT.Trim() == "Y")
                {
                    SS1.ActiveSheet.Cells[i, 0].Text = "";
                    SS1.ActiveSheet.Cells[i, 24].Text = "Y";
                }
                else
                {
                    SS1.ActiveSheet.Cells[i, 0].Text = "True";
                    SS1.ActiveSheet.Cells[i, 24].Text = "";
                }
                SS1.ActiveSheet.Cells[i, 1].Text = nPano.ToString();
                SS1.ActiveSheet.Cells[i, 3].Text = strSname;
                SS1.ActiveSheet.Cells[i, 5].Text = nWRTNO.ToString();

                //처방발급 상세내역을 Display
                List<HIC_HYANG_APPROVE> list2 = hicHyangApproveService.GetItembyWrtNo(nWRTNO, strBDate, 0);

                nRead2 = list2.Count;
                nPOL2 = 0;
                nPOL12G = 0;
                nPOL8G = 0;
                nBASCA = 0;
                nPTDHA = 0;
                nPTD25 = 0;
                nPOL2_E = 0;
                nPOL12G_E = 0;
                nPOL8G_E = 0;
                nBASCA_E = 0;
                nPTDHA_E = 0;
                nPTD25_E = 0;

                for (int j = 0; j < nRead2; j++)
                {   
                    //약품별 처방수량을 표시함
                    switch (list2[j].SUCODE.Trim())
                    {
                        case "A-POL2":
                            nPOL2 = list2[j].ENTQTY;
                            nPOL2_E = list2[j].ENTQTY2;
                            break;
                        //case "A-PO12GA":
                        //    nPOL12G = list2[j].ENTQTY;
                        //    nPOL12G_E = list2[j].ENTQTY2;
                        //    break;
                        case "A-ANE12G":
                            nPOL12G = list2[j].ENTQTY;
                            nPOL12G_E = list2[j].ENTQTY2;
                            break;
                        case "A-POL8G":
                            nPOL8G = list2[j].ENTQTY;
                            nPOL8G_E = list2[j].ENTQTY2;
                            break;
                        case "A-BASCA":
                            nBASCA = list2[j].ENTQTY;
                            nBASCA_E = list2[j].ENTQTY2;
                            break;
                        case "N-PTD-HA":
                            nPTDHA = list2[j].ENTQTY;
                            nPTDHA_E = list2[j].ENTQTY2;
                            break;
                        case "N-PTD25":
                            nPTD25 = list2[j].ENTQTY;
                            nPTD25_E = list2[j].ENTQTY2;
                            break;
                        default:
                            break;
                    }

                    if (j == 0)
                    {
                        strDRSABUN = list2[i].DRSABUN.Trim();
                        strPtNo = list2[i].PTNO.Trim();
                        strJumin = clsAES.DeAES(list2[j].JUMIN2.Trim());
                        strSEX = list2[j].SEX;
                        nAge = list2[j].AGE;
                        if (strNRSABUN == "")
                        {
                            strNRSABUN = list2[j].NRSABUN.Trim();
                        }
                        SS1.ActiveSheet.Cells[i, 2].Text = list2[j].AMPM == "2" ? "오후" : " ";
                        SS1.ActiveSheet.Cells[i, 4].Text = nAge + "/" + strSEX;
                        SS1.ActiveSheet.Cells[i, 16].Text = hb.READ_Sabun_Name(list2[j].DRSABUN).Trim(); ;
                        SS1.ActiveSheet.Cells[i, 17].Text = VB.Left(strJumin, 6) + "-" + VB.Mid(strJumin, 7, 1) + "******";
                        SS1.ActiveSheet.Cells[i, 18].Text = strPtNo;
                        SS1.ActiveSheet.Cells[i, 27].Text = "Y";
                        SS1.ActiveSheet.Cells[i, 29].Text = list2[j].BDATE.ToString();
                        SS1.ActiveSheet.Cells[i, 30].Text = list2[j].DRSABUN.Trim();
                        SS1.ActiveSheet.Cells[i, 31].Text = list2[j].GBSLEEP == "Y" ? "◎" : " "; //수면
                        
                        if (list2[j].DEPTCODE.Trim() == "HR")
                        {
                            SS1.ActiveSheet.Cells[i, 5].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H80C0FF"));
                            SS1.ActiveSheet.Cells[i, 26].Text = "HR";
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[i, 5].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                            SS1.ActiveSheet.Cells[i, 26].Text = "TO";
                            //일반건진 암검진 접수
                            if (fn_HIC_Jepsu_Check(strPtNo, strBDate) == true)
                            {
                                SS1.ActiveSheet.Cells[i, 5].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFEEFD6"));
                            }
                        }
                        
                        SS1.ActiveSheet.Cells[i, 32].Text = VB.Left(strJumin, 6) + "-" + VB.Right(strJumin, 7);
                    }
                }

                nTOT[0, 0] += nPOL2;
                nTOT[0, 1] += nPOL2_E;
                nTOT[1, 0] += nPOL12G;
                nTOT[1, 1] += nPOL12G_E;
                nTOT[2, 0] += nPOL8G;
                nTOT[2, 1] += nPOL8G_E;
                nTOT[3, 0] += nBASCA;
                nTOT[3, 1] += nBASCA_E;
                nTOT[4, 0] += nPTD25;
                nTOT[4, 1] += nPTD25_E;

                SS1.ActiveSheet.Cells[i, 4].Text = nAge + "/" + strSEX;
                SS1.ActiveSheet.Cells[i, 6].Text = nPOL2 > 0 ? "True" : "";
                SS1.ActiveSheet.Cells[i, 7].Text = nPOL2_E > 0 ? string.Format("{0:#0.00}", nPOL2_E) : "";
                SS1.ActiveSheet.Cells[i, 8].Text = nPOL12G > 0 ? "True" : "";
                SS1.ActiveSheet.Cells[i, 9].Text = nPOL12G_E > 0 ? string.Format("{0:#0.00}", nPOL12G_E) : "";
                SS1.ActiveSheet.Cells[i, 10].Text = nPOL8G > 0 ? "True" : "";
                SS1.ActiveSheet.Cells[i, 11].Text = nPOL8G_E > 0 ? string.Format("{0:#0.00}", nPOL8G_E) : "";
                SS1.ActiveSheet.Cells[i, 12].Text = nBASCA > 0 ? "True" : "";
                SS1.ActiveSheet.Cells[i, 13].Text = nBASCA_E > 0 ? string.Format("{0:#0.00}", nBASCA_E) : "";
                SS1.ActiveSheet.Cells[i, 14].Text = nPTD25 > 0 ? "True" : "";
                SS1.ActiveSheet.Cells[i, 15].Text = nPTD25_E > 0 ? string.Format("{0:#0.00}", nPTD25_E) : "";

                SS1.ActiveSheet.Cells[i, 19].Text = "검사용";
                SS1.ActiveSheet.Cells[i, 20].Text = "Pain";
                SS1.ActiveSheet.Cells[i, 21].Text = "";
                SS1.ActiveSheet.Cells[i, 22].Text = "";
                SS1.ActiveSheet.Cells[i, 23].Text = "";
                SS1.ActiveSheet.Cells[i, 25].Text = "";
            }

            sp.setSpdSort(SS1, 28, true);

        }

        /// <summary>
        /// 사용량 전송용 자료를 View
        /// </summary>
        void fn_Display_Slip_Send()
        {
            int nREAD = 0;
            int nRead2 = 0;
            int nRow = 0;
            long nAge = 0;
            long nWRTNO = 0;
            long nPano = 0;
            string strOK = "";
            string strOK2 = "";
            string strPtNo = "";
            string strJEPDATE = "";
            string strSDate = "";
            string strROWID = "";
            string strDrno = "";
            string strDRSABUN = "";
            string strSEX = "";
            string strJuso = "";
            string strSname = "";
            string strJumin = "";
            string strJong = "";
            string strPrint = "";
            double nPOL2 = 0;
            double nPOL12G = 0;
            double nPOL8G = 0;
            double nBASCA = 0;
            double nBASCAM = 0;
            double nPTDHA = 0;
            double nPTD25 = 0;
            double nPOL2_E = 0;
            double nPOL12G_E = 0;
            double nPOL8G_E = 0;
            double nBASCA_E = 0;
            double nBASCAM_E = 0;
            double nPTDHA_E = 0;
            double nPTD25_E = 0;
            double[,] nTOT = new double[5, 2];
            long nBanQty = 0;
            string strHic = "";
            string strDept = "";

            bFlag = false;

            nRow = 0;
            sp.Spread_All_Clear(SS1);

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    nTOT[i, j] = 0;
                }
            }
            FstrBDate = dtpBDate.Text;
            //=============================================
            // 승인된 의뢰된 내역을 Display
            //=============================================
            List<HIC_HYANG_APPROVE> list = hicHyangApproveService.GetItembyBDate(FstrBDate);

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                nWRTNO = list[i].WRTNO;
                nPano = list[i].PANO;
                strSname = list[i].SNAME.Trim();

                SS1.ActiveSheet.RowCount = list.Count;

                SS1.ActiveSheet.Cells[i, 0].Text = "";
                SS1.ActiveSheet.Cells[i, 1].Text = nPano.ToString();
                SS1.ActiveSheet.Cells[i, 3].Text = strSname;
                SS1.ActiveSheet.Cells[i, 5].Text = nWRTNO.ToString();
                SS1.ActiveSheet.Cells[i, 29].Text = dtpBDate.Text;

                //수면여부 표시
                HIC_HYANG_APPROVE list2 = hicHyangApproveService.GetItembyWrtnoBDate(nWRTNO, dtpBDate.Text);

                strDept = "";
                if (list2 != null)
                {
                    SS1.ActiveSheet.Cells[i, 2].Text = list2.AMPM.Trim() == "2" ? "오후" : " ";
                    if (list2.PRINT.IsNullOrEmpty())
                    {
                        SS1.ActiveSheet.Cells[i, 24].Text = "";
                    }
                    else if (list2.PRINT.Trim() == "Y")
                    {
                        SS1.ActiveSheet.Cells[i, 24].Text = "Y";
                    }
                    //SS1.ActiveSheet.Cells[i, 24].Text = list2.PRINT.Trim() == "Y" ? "Y" : "";
                    SS1.ActiveSheet.Cells[i, 31].Text = list2.GBSLEEP == "Y" ? "◎" : "";
                    strPtNo = list2.PTNO.Trim();
                    strJumin = clsAES.DeAES(list2.JUMIN2);
                    strSEX = list2.SEX;
                    nAge = list2.AGE;
                    strDept = list2.DEPTCODE.Trim();
                    if (!list2.DRSABUN.IsNullOrEmpty())
                    {
                        strDRSABUN = list2.DRSABUN.Trim();
                    }
                   
                }

                //처방발급 상세내역을 Display
                List<HIC_HYANG_APPROVE> list3 = hicHyangApproveService.GetItembyWrtNoBDate(nWRTNO, dtpBDate.Text, "", "");

                nRead2 = list3.Count;

                nPOL2 = 0;
                nPOL12G = 0;
                nPOL8G = 0;
                nBASCA = 0;
                nPTDHA = 0;
                nPTD25 = 0;
                nPOL2_E = 0;
                nPOL12G_E = 0;
                nPOL8G_E = 0;
                nBASCA_E = 0;
                nBASCAM_E = 0;
                nPTDHA_E = 0;
                nPTD25_E = 0;

                for (int j = 0; j < nRead2; j++)
                {
                    switch (list3[j].SUCODE.Trim())
                    {
                        case "A-POL2":
                            nPOL2 = list3[j].ENTQTY;
                            nPOL2_E = list3[j].ENTQTY2;
                            break;
                        //case "A-PO12GA":
                        //    nPOL12G = list3[j].ENTQTY;
                        //    nPOL12G_E = list3[j].ENTQTY2;
                        //    break;
                        case "A-ANE12G":
                            nPOL12G = list3[j].ENTQTY;
                            nPOL12G_E = list3[j].ENTQTY2;
                            break;
                        case "A-POL8G":
                            nPOL8G = list3[j].ENTQTY;
                            nPOL8G_E = list3[j].ENTQTY2;
                            break;
                        case "A-BASCA":
                            nPOL8G = list3[j].ENTQTY;
                            nPOL8G_E = list3[j].ENTQTY2;
                            break;
                        case "A-BASCAM":
                            nBASCAM = list3[j].ENTQTY;
                            nBASCAM_E = list3[j].ENTQTY2;
                            break;
                        case "N-PTD-HA":
                            nPTDHA = list3[j].ENTQTY;
                            nPTDHA_E = list3[j].ENTQTY2;
                            break;
                        case "N-PTD25":
                            nPTD25 = list3[j].ENTQTY;
                            nPTD25_E = list3[j].ENTQTY2;
                            break;
                        default:
                            break;
                    }

                    if (j == 0)
                    {
                        if(!list3[j].DRSABUN.IsNullOrEmpty())
                        {
                            strDRSABUN = list3[j].DRSABUN.Trim();
                        }
                        strJuso = list3[j].JUSO.Trim();
                    }
                }

                nTOT[0, 0] += nPOL2;
                nTOT[0, 1] += nPOL2_E;
                nTOT[1, 0] += nPOL12G;
                nTOT[1, 1] += nPOL12G_E;
                nTOT[2, 0] += nPOL8G;
                nTOT[2, 1] += nPOL8G_E;
                nTOT[3, 0] += nBASCA;
                nTOT[3, 1] += nBASCA_E;
                nTOT[4, 0] += nPTD25;
                nTOT[4, 1] += nPTD25_E;

                SS1.ActiveSheet.Cells[i, 4].Text = nAge + "/" + strSEX;
                SS1.ActiveSheet.Cells[i, 6].Text = nPOL2 > 0 ? "True" : "";
                SS1.ActiveSheet.Cells[i, 7].Text = nPOL2 > 0 ? string.Format("{0:#0.00}", nPOL2_E) : "";
                SS1.ActiveSheet.Cells[i, 8].Text = nPOL12G > 0 ? "True" : "";
                SS1.ActiveSheet.Cells[i, 9].Text = nPOL12G > 0 ? string.Format("{0:#0.00}", nPOL12G_E) : "";
                SS1.ActiveSheet.Cells[i, 10].Text = nPOL8G > 0 ? "True" : "";
                SS1.ActiveSheet.Cells[i, 11].Text = nPOL8G > 0 ? string.Format("{0:#0.00}", nPOL8G_E) : "";
                SS1.ActiveSheet.Cells[i, 12].Text = nBASCAM > 0 ? "True" : "";
                SS1.ActiveSheet.Cells[i, 13].Text = nBASCAM > 0 ? string.Format("{0:#0.00}", nBASCAM_E) : "";
                SS1.ActiveSheet.Cells[i, 14].Text = nPTD25 > 0 ? "True" : "";
                SS1.ActiveSheet.Cells[i, 15].Text = nPTD25 > 0 ? string.Format("{0:#0.00}", nPTD25_E) : "";

                if (strDRSABUN != "")
                {
                    SS1.ActiveSheet.Cells[i, 16].Text = hb.READ_Sabun_Name(strDRSABUN).Trim();
                }
                else
                {
                    SS1.ActiveSheet.Cells[i, 16].Text = "";
                }

                SS1.ActiveSheet.Cells[i, 17].Text = VB.Left(strJumin.Trim(), 6) + "-" + VB.Mid(strJumin.Trim(), 7, 7);
                SS1.ActiveSheet.Cells[i, 18].Text = strPtNo;
                SS1.ActiveSheet.Cells[i, 19].Text = "검사용";
                SS1.ActiveSheet.Cells[i, 20].Text = "Pain";
                SS1.ActiveSheet.Cells[i, 22].Text = "";
                SS1.ActiveSheet.Cells[i, 26].Text = strDept;
                SS1.ActiveSheet.Cells[i, 27].Text = "Y";
                SS1.ActiveSheet.Cells[i, 28].Text = "";
                SS1.ActiveSheet.Cells[i, 30].Text = strDRSABUN;

                if (strDept == "HR")
                {
                    SS1.ActiveSheet.Cells[i, 3].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H80C0FF"));
                }
                else if (strHic == "OK")
                {
                    SS1.ActiveSheet.Cells[i, 3].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H80C0FF"));
                }
                else
                {
                    SS1.ActiveSheet.Cells[i, 3].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                    //일반건진 암검진 접수
                    if (fn_HIC_Jepsu_Check(strPtNo, dtpBDate.Text) == true)
                    {
                        SS1.ActiveSheet.Cells[i, 3].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFEEFD6"));
                    }
                }
                //오전/오후 표시
                if (heaJepsuService.GetAmPm2byWrtNo(nWRTNO) == "2")
                {
                    SS1.ActiveSheet.Cells[i, 2].Text = "오후";
                }
            }

            bFlag = true;

            FnRow_S = SS1.ActiveSheet.RowCount;
            FnRow_T = SS1.ActiveSheet.RowCount + 1;
            SS1.ActiveSheet.RowCount = FnRow_T;

            sp.CellSpan(SS1, FnRow_T-1, 4, 1, 2);
            SS1.ActiveSheet.Cells[FnRow_T-1, 4].Text = "합 계";

            SS1.ActiveSheet.Cells[FnRow_T-1, 7].Text = nTOT[0, 1] > 0 ? string.Format("{0:#0.00}", nTOT[0, 1]) : "";
            SS1.ActiveSheet.Cells[FnRow_T-1, 9].Text = nTOT[1, 1] > 0 ? string.Format("{0:#0.00}", nTOT[1, 1]) : "";
            SS1.ActiveSheet.Cells[FnRow_T-1, 11].Text = nTOT[2, 1] > 0 ? string.Format("{0:#0.00}", nTOT[2, 1]) : "";
            SS1.ActiveSheet.Cells[FnRow_T-1, 13].Text = nTOT[3, 1] > 0 ? string.Format("{0:#0.00}", nTOT[3, 1]) : "";
            SS1.ActiveSheet.Cells[FnRow_T-1, 15].Text = nTOT[4, 1] > 0 ? string.Format("{0:#0.00}", nTOT[4, 1]) : "";

            ufn_Spread_CellType_Set(SS1, FnRow_T-1, 0);
            ufn_Spread_CellType_Set(SS1, FnRow_T-1, 6);
            ufn_Spread_CellType_Set(SS1, FnRow_T-1, 8);
            ufn_Spread_CellType_Set(SS1, FnRow_T-1, 10);
            ufn_Spread_CellType_Set(SS1, FnRow_T-1, 12);
            ufn_Spread_CellType_Set(SS1, FnRow_T-1, 14);

            SS1_Sheet1.Columns.Get(-1).BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HE0E0E0"));

            FnRow_B = SS1.ActiveSheet.RowCount + 1;
            SS1.ActiveSheet.RowCount = FnRow_B;

            sp.CellSpan(SS1, FnRow_B-1, 4, 1, 2);
            SS1.ActiveSheet.Cells[FnRow_B-1, 4].Text = "반납수량";

            ufn_Spread_CellType_Set(SS1, FnRow_T-1, 0);
            ufn_Spread_CellType_Set(SS1, FnRow_T-1, 6);
            ufn_Spread_CellType_Set(SS1, FnRow_T-1, 9);
            ufn_Spread_CellType_Set(SS1, FnRow_T-1, 12);
            ufn_Spread_CellType_Set(SS1, FnRow_T-1, 15);
            ufn_Spread_CellType_Set(SS1, FnRow_T-1, 20);

            SS1.ActiveSheet.Cells[FnRow_B-1, 7].Text = ufn_BannapQty_Check(nTOT[0, 1], 20);   //A-POL2
            SS1.ActiveSheet.Cells[FnRow_B-1, 9].Text = ufn_BannapQty_Check(nTOT[1, 1], 12);   //A-ANE12G
            SS1.ActiveSheet.Cells[FnRow_B-1, 11].Text = ufn_BannapQty_Check(nTOT[2, 1], 8);   //A-POL8G
            SS1.ActiveSheet.Cells[FnRow_B-1, 13].Text = ufn_BannapQty_Check(nTOT[3, 1], 5);   //A-BASCAM
            SS1.ActiveSheet.Cells[FnRow_B-1, 15].Text = ufn_BannapQty_Check(nTOT[4, 1], 25);  //N-NPT25

            SS1_Sheet1.Columns.Get(-1).BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HE0F3FE"));

            //마감이 되었으면 처방작성은 불가능
            if (fn_Magam_Check() == true)
            {
                SS1.Enabled = false;
                btnHyang.Enabled = false;
                btnMenuDelete.Enabled = false;
                btnMenuSave.Enabled = false;
                btnMenuFinishOk.Enabled = false;
                btnMenuFinishCancel.Enabled = false;
                //전산실,최종숙과장만 마감취소가 가능함
                if (long.Parse(clsType.User.IdNumber) == 36540 || long.Parse(clsType.User.IdNumber) == 31197 || long.Parse(clsType.User.IdNumber) == 29822 || long.Parse(clsType.User.IdNumber) == 26080)
                {
                    btnMenuFinishCancel.Enabled = true;
                }
            }
            else
            {
                SS1.Enabled = true;
                btnHyang.Enabled = true;
                btnMenuDelete.Enabled = true;
                btnMenuSave.Enabled = true;
                btnMenuFinishOk.Enabled = true;
                btnMenuFinishCancel.Enabled = false;
            }
            
        }

        string ufn_BannapQty_Check(double argSomo, long argUnitQty)
        {
            string rtnVal = "";
            double nQty = 0;

            if (argSomo == 0 || argUnitQty == 0)
            {
                rtnVal = "";
                return rtnVal;
            }

            nQty = VB.FixDbl(argSomo / argUnitQty);

            if (argSomo > (nQty * argUnitQty))
            {
                nQty += 1;
            }

            if (nQty == 0)
            {
                rtnVal = "";
            }
            else
            {
                rtnVal = string.Format("{0:#0.00}", (nQty * argUnitQty) - argSomo);
            }

            return rtnVal;
        }

        void ufn_Spread_CellType_Set(FarPoint.Win.Spread.FpSpread spdNm, int nRow, int nCol)
        {
            spdNm.ActiveSheet.Cells[nRow, 0].CellType = txt;
            spdNm.ActiveSheet.Cells[nRow, 0].Text = "";
            spdNm.ActiveSheet.Cells[nRow, 0].Locked = true;
        }

        void eSpdClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                if (e.ColumnHeader == true)
                {
                    sp.setSpdSort(SS1, e.Column, true);
                }
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            long nQty = 0;

            string strSname = "";
            string strPtno = "";
            string strSite = "";
            string strMsg = "";
            string strNewSite = "";
            string strSucode = "";


            if (e.Column != 28) { return; }

            strSname = SS1.ActiveSheet.Cells[e.Row, 3].Text;
            strPtno = SS1.ActiveSheet.Cells[e.Row, 18].Text;
            strSite = SS1.ActiveSheet.Cells[e.Row, 28].Text;

            strMsg = strPtno + " " + strSname + "님 내시경 장소를 ";
            if(strSite == "◎")
            {
                strMsg += "종검내시경실";
            }
            else
            {
                strMsg += "본관내시경실";
            }
            strMsg += " 로 변경을 하시겠습니까?";


            if (MessageBox.Show(strMsg, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }


            strNewSite = "2";
            if(strSite == "◎") { strNewSite = "1"; }

            for (int i = 1; i < 4; i++)
            {
                if (strNewSite == "1")
                {

                    switch (i)
                    {
                        case 1:
                            strSucode = "A-ANE12G";
                            nQty = 12;
                            break;
                        case 2:
                            strSucode = "A-BASCA";
                            nQty = 5;
                            break;
                        case 3:
                            strSucode = "N-PTD25";
                            nQty = 25;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (i)
                    {
                        case 1:
                            strSucode = "A-ANE12G";
                            nQty = 1;
                            break;
                        case 2:
                            strSucode = "A-BASCA";
                            nQty = 1;
                            break;
                        case 3:
                            strSucode = "N-PTD25";
                            nQty = 1;
                            break;
                        default:
                            break;
                    }
                }

                int result = hicHyangApproveService.UpdateGbSiteEntQtyByBdatePtnoSucode(strNewSite, nQty, dtpBDate.Text, strPtno, strSucode);
                if (result < 0)
                {
                    MessageBox.Show("내시경 장소변경 오류!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (strSite == "◎")
            {
                SS1.ActiveSheet.Cells[e.Row, 28].Text = "";
            }
            else
            {
                SS1.ActiveSheet.Cells[e.Row, 28].Text = "◎";
            }

            MessageBox.Show("저장완료!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
