using ComBase;
using ComBase.Controls;
using ComEmrBase;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHaResultEKG.cs
/// Description     : 종검EKG 판독 등록
/// Author          : 이상훈
/// Create Date     : 2019-09-30
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "HaMain07.frm(FrmResultEKG)" />

namespace ComHpcLibB
{
    public partial class frmHaResultEKG : Form
    {
        BasBcodeEcgService basBcodeEcgService = null;
        ComHpcLibBService comHpcLibBService = null;
        EtcJupmstService etcJupmstService = null;
        HeaEkgResultService heaEkgResultService = null;
        HeaJepsuService heaJepsuService = null;
        HicResultService hicResultService = null;
        HeaJepsuEkgResultService heaJepsuEkgResultService = null;
        HeaResultService heaResultService = null;

        frmHaEcgCode fHaEcgCode = null;
        frmViewResult fViewResult = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hc = new clsHcFunc();

        long FnWRTNO;
        long FnWRTNO2;
        long FnClickRow;                        //Help를 Click한 Row
        string[] FstrResult = new string[300];  //문장 결과를 보관할 배열
        long FnResultRow;
        long FnPano;
        string FstrPtno;                        // 외래번호
        string FstrSdate;                       // 수진일자

        public frmHaResultEKG()
        {
            InitializeComponent();

            SetEvent();
            SetControl();
        }

        public frmHaResultEKG(long nWrtNo)
        {
            InitializeComponent();

            FnWRTNO = nWrtNo;

            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            basBcodeEcgService = new BasBcodeEcgService();
            comHpcLibBService = new ComHpcLibBService();
            etcJupmstService = new EtcJupmstService();
            heaEkgResultService = new HeaEkgResultService();
            heaJepsuService = new HeaJepsuService();
            hicResultService = new HicResultService();
            heaJepsuEkgResultService = new HeaJepsuEkgResultService();
            heaResultService = new HeaResultService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnMenuECG.Click += new EventHandler(eBtnClick);
            this.btnPacs.Click += new EventHandler(eBtnClick);
            this.btnEMR.Click += new EventHandler(eBtnClick);
            this.btnResultSave.Click += new EventHandler(eBtnClick);
            this.btnResultDelete.Click += new EventHandler(eBtnClick);
            this.btnResultCancel.Click += new EventHandler(eBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.ssCode.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.ssECG.CellDoubleClick += new CellClickEventHandler(eSpdDClick);            
            this.ssList.CellClick += new CellClickEventHandler(eSpdClick);
            this.txtWrtNo.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtWrtNo.LostFocus += new EventHandler(eLostFocus);
        }

        void SetControl()
        {

        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFrDate.Text = clsPublic.GstrSysDate;
            dtpToDate.Text = clsPublic.GstrSysDate;

            ComFunc.KillProc("c:\\ECG_*.ecg");

            //입력상태
            cboJong.Items.Clear();
            cboJong.Items.Add("*.모든검사");
            cboJong.Items.Add("1.입력중");
            cboJong.Items.Add("2.입력완료");
            cboJong.Items.Add("9.판정완료");
            cboJong.SelectedIndex = 1;

            //조회순서

            fn_Screen_clear();

            if (!clsHcVariable.GstrWordView.IsNullOrEmpty())
            {
                txtWrtNo.Text = clsHcVariable.GstrWordView;
            }

            fn_READ_EKG_RESULT();
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnResultDelete)
            {
                string strRowId = "";

                strRowId = heaEkgResultService.GetRowIdbyWrtNo(long.Parse(txtWrtNo.Text));

                if (strRowId != "")
                {
                    int result = heaEkgResultService.DeletebyRowId(strRowId);

                    if (result < 0)
                    {
                        MessageBox.Show("등록 오류 점검 요망", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                txtResult.Text = "";
            }
            else if (sender == btnEMR)
            {
                CallEmrViewNew();
            }
            else if (sender == btnPacs)
            {
                UnloadExamForm();

                fViewResult = new frmViewResult(FstrPtno);
                fViewResult.rEventClosed += new frmViewResult.EventClosed(frmViewResult_EventClosed);
                fViewResult.StartPosition = FormStartPosition.CenterScreen;
                fViewResult.Show(this);
                fViewResult.BringToFront();
            }
            else if (sender == btnResultCancel)
            {
                txtResult.Text = "";
            }
            else if (sender == btnResultSave)
            {
                string strRowId = "";
                string strResult = "";
                int result = 0;

                if (txtResult.Text.Length > 1500)
                {
                    MessageBox.Show("결과가 1500자를 초과함:" + txtResult.Text.Length, "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtResult.Focus();
                    return;
                }

                strRowId = heaEkgResultService.GetRowIdbyWrtNo(long.Parse(txtWrtNo.Text));

                strResult = txtResult.Text.Replace("'", "`");
                if (VB.InStr(strResult, "※판정의사 :") == 0)
                {
                    strResult += "\r\n" + "※판정의사 : " + clsVbfunc.GetInSaName(clsDB.DbCon, clsType.User.IdNumber);
                }

                if (!strRowId.IsNullOrEmpty())
                {
                    result = heaEkgResultService.UpdateResult(strResult, strRowId);
                }
                else
                {
                    result = heaEkgResultService.InsertAll(long.Parse(txtWrtNo.Text), strResult);
                }

                if (result < 0)
                {
                    MessageBox.Show("등록 오류 점검 요망", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //종겁 결과 에  자동으로 등록
                //result = hicResultService.UpdateResCode("007", strResult, FnWRTNO);
                result = heaResultService.UpdateResCode("007", strResult, FnWRTNO);


                if (result < 0)
                {
                    MessageBox.Show("종검결과 자동 등록중 오류발생!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else if (sender == btnSearch)
            {
                int nREAD = 0;
                string strInOut = "";
                string strLtdCode = "";
                string strFrDate = "";
                string strToDate = "";
                List<string> strGbSts = new List<string>();
                string strEkg = "";

                strGbSts.Clear();

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;
                switch (VB.Left(cboJong.Text, 1))
                {
                    case "*":
                        strGbSts.Add("1");
                        strGbSts.Add("2");
                        strGbSts.Add("3");
                        break;
                    case "1":
                        strGbSts.Add("1");
                        strGbSts.Add("2");
                        break;
                    case "2":
                        strGbSts.Add("3");
                        break;
                    case "9":
                        strGbSts.Add("9");
                        break;

                    default:
                        break;
                }
                
                if (chkEKG.Checked == true)
                {
                    strEkg = "Y";
                }

                sp.Spread_All_Clear(ssList);

                //자료를 SELECT
                List<HEA_JEPSU_EKG_RESULT> list = heaJepsuEkgResultService.GetItembySDate(strFrDate, strToDate, strGbSts, strEkg);

                nREAD = list.Count;
                ssList.ActiveSheet.RowCount = nREAD;

                for (int i = 0; i < nREAD; i++)
                {
                    ssList.ActiveSheet.Cells[i, 0].Text = list[i].PTNO;
                    ssList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME.Trim();
                    ssList.ActiveSheet.Cells[i, 2].Text = DateTime.Parse(list[i].SDATE).ToShortDateString();
                    if (!list[i].EKGRESULT.IsNullOrEmpty() && list[i].EKGRESULT.Length > 0)
                    {
                        ssList.ActiveSheet.Cells[i, 3].Text = "◎";
                    }

                    if (list[i].GBEKG == "*")
                    {
                        ssList.ActiveSheet.Cells[i, 0, i, ssList.ActiveSheet.ColumnCount - 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0FFC0"));
                    }
                    else
                    {
                        ssList.ActiveSheet.Cells[i, 0, i, ssList.ActiveSheet.ColumnCount - 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                    }
                    ssList.ActiveSheet.Cells[i, 4].Text = list[i].WRTNO.ToString();
                }
            }
            else if (sender == btnMenuECG)
            {
                UnloadExamForm();

                fHaEcgCode = new frmHaEcgCode();
                fHaEcgCode.rEventClosed += new frmHaEcgCode.EventClosed(frmHaEcgCode_EventClosed);
                fHaEcgCode.StartPosition = FormStartPosition.CenterScreen;
                fHaEcgCode.Show(this);
                fHaEcgCode.BringToFront();
            }
        }

        /// <summary>
        /// 신규 EMR을 호출한다
        /// </summary>
        void CallEmrViewNew()
        {
            //EmrPatient AcpEmr = null;
            //AcpEmr = clsEmrChart.ClearPatient();
            //AcpEmr = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, FstrPtno, "O", FstrSdate.Replace("-", ""), "HR");

            //if (AcpEmr != null)
            //{
            //    #region //Call EMR New
            //    if (clsMedOrderPublic.MedOrderEmr != null)
            //    {
            //        if (clsMedOrderPublic.MedOrderEmr.Visible == false)
            //        {
            //            clsMedOrderPublic.MedOrderEmr.Dispose();
            //            clsMedOrderPublic.MedOrderEmr = null;
            //            clsMedOrderPublic.MedOrderEmr = new frmEmrViewMain(AcpEmr, this);
            //            clsMedOrderPublic.MedOrderEmr.Show();
            //        }
            //        else
            //        {
            //            clsMedOrderPublic.MedOrderEmr.SetNewPatient(AcpEmr, this);
            //        }
            //    }
            //    else
            //    {
            //        clsMedOrderPublic.MedOrderEmr = new frmEmrViewMain(AcpEmr, this);
            //        clsMedOrderPublic.MedOrderEmr.Show();
            //    }
            //    Application.DoEvents();
            //    #endregion //Call EMR New
            //}
            //else
            //{
            //    ComFunc.MsgBox("접수내역을 찾을 수 없습니다.");
            //}


            clsVbEmr.EXECUTE_NewTextEmrView(FstrPtno);

        }

        void eSpdClick(object sender, CellClickEventArgs e)
        {
            if (sender == ssList)
            {
                if (e.ColumnHeader == true)
                {
                    sp.setSpdSort(SS1, e.Column, true);
                }
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == ssList)
            {
                long nWrtNo = 0;

                nWrtNo = ssList.ActiveSheet.Cells[e.Row, 4].Text.To<long>();

                fn_Screen_clear();
                txtWrtNo.Text = nWrtNo.To<string>();
                fn_Screen_Display();

                fn_READ_EKG_RESULT();
            }
            else if (sender == ssCode)
            {
                string strCode = "";

                if (e.RowHeader == true || e.ColumnHeader == true)
                {
                    return;
                }

                //strCode = ssCode.ActiveSheet.Cells[e.Row, 0].Text.PadRight(6);
                //strCode += " " + ssCode.ActiveSheet.Cells[e.Row, 1].Text;

                strCode = ssCode.ActiveSheet.Cells[e.Row, 1].Text;
                txtResult.Text += "\r\n" + strCode;
            }
            else if (sender == ssECG)
            {
                string strRowId = "";
                string strResult = "";
                string strImage_Gbn = "";

                if (e.RowHeader == true || e.ColumnHeader == true)
                {
                    return;
                }

                strResult = ssECG.ActiveSheet.Cells[e.Row, 7].Text;
                strRowId = ssECG.ActiveSheet.Cells[e.Row, 9].Text;
                strImage_Gbn = ssECG.ActiveSheet.Cells[e.Row, 10].Text.Trim();

                if (etcJupmstService.GetCountbyRowId(strRowId) > 0)
                {
                    if (strImage_Gbn.IsNullOrEmpty())
                    {
                        hc.ECGFILE_DBToFile(strRowId, FstrPtno, "1");
                    }
                    else
                    {
                        hc.ETC_FILE_DBToFile(strRowId, FstrPtno, "1");
                    }
                }   
            }
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtWrtNo)
            {
                if (e.KeyChar == (char)13)
                {
                    SendKeys.Send("{TAB}");
                }
            }
        }

        void eLostFocus(object sender, EventArgs e)
        {
            if (sender == txtWrtNo)
            {
                if (txtWrtNo.Text.Trim() == "")
                {
                    return;
                }
                fn_Screen_Display();
            }
        }

        void fn_READ_EKG_RESULT()
        {
            List<BAS_BCODE_ECG> list = basBcodeEcgService.GetCodeAll();

            sp.Spread_All_Clear(ssCode);
            ssCode.ActiveSheet.RowCount = list.Count;

            for (int i = 0; i < list.Count; i++)
            {
                ssCode.ActiveSheet.Cells[i, 0].Text = list[i].CODE;
                ssCode.ActiveSheet.Cells[i, 1].Text = list[i].NAME;
            }
        }

        void fn_READ_ETC_JUPMST_TO()
        {
            sp.Spread_All_Clear(ssECG);
            ssECG_Sheet1.Columns.Get(9).Visible = false;
            ssECG.ActiveSheet.RowCount = 30;

            List<COMHPC> list = comHpcLibBService.GetItembyPtNo(FstrPtno);

            ssECG.ActiveSheet.RowCount = list.Count;
            for (int i = 0; i < list.Count; i++)
            {
                ssECG.ActiveSheet.Cells[i, 0].Text = list[i].BDATE;
                ssECG.ActiveSheet.Cells[i, 1].Text = list[i].RDATE;
                ssECG.ActiveSheet.Cells[i, 2].Text = list[i].SEX;
                ssECG.ActiveSheet.Cells[i, 3].Text = list[i].AGE.ToString();
                ssECG.ActiveSheet.Cells[i, 4].Text = list[i].GBIO == "O" ? "외래" : "입원";
                ssECG.ActiveSheet.Cells[i, 5].Text = list[i].DEPTCODE;
                ssECG.ActiveSheet.Cells[i, 6].Text = list[i].DRNAME;
                if (!list[i].IMAGE.IsNullOrEmpty())
                {
                    ssECG.ActiveSheet.Cells[i, 7].Text = list[i].IMAGE.Length == 0 ? "" : "▦";
                }
                if (ssECG.ActiveSheet.Cells[i, 7].Text == "")
                {
                    ssECG.ActiveSheet.Cells[i, 7].Text = list[i].GbFTP == "Y" ? "▦" : "";
                }
                ssECG.ActiveSheet.Cells[i, 8].Text = list[i].ORDERNAME;
                ssECG.ActiveSheet.Cells[i, 9].Text = list[i].ROWID;
                ssECG.ActiveSheet.Cells[i, 10].Text = list[i].IMAGE_GBN;
            }
        }

        void fn_Screen_clear()
        {
            btnPacs.Enabled = false;
            btnEMR.Enabled = false;
            FstrPtno = "";
            FnWRTNO = 0;
            txtWrtNo.Text = "";
            FnClickRow = 0;
            FnWRTNO2 = 0;
            FnPano = 0;

            for (int i = 1; i < 300; i++)
            {
                FstrResult[i] = "";
            }
            txtWrtNo.Text = "";
            txtResult.Text = "";

            txtResult.Enabled = false;
            btnResultSave.Enabled = false;
            btnResultCancel.Enabled = false;
            txtWrtNo.Enabled = true;

            sp.Spread_All_Clear(SS1);
        }

        void fn_Screen_Display()
        {
            int nRead = 0;
            int nRow = 0;

            string strSEX = "";
            string strHeaSORT = "";
            string strResult = "";
            string strResCode = "";
            string strResultType = "";
            string strGbCodeUse = "";
            string strNomal = "";
            string strTemp = "";
            string strTemp2 = "";
            string strFlag = "";
            string strExcode = "";
            string strYYYY = "";
            string strSDate = "";
            string strImage_Gbn = "";
            string strROWID = "";

            string strSDate_ECG = "";

            txtWrtNo.Enabled = false;
            FnWRTNO = long.Parse(txtWrtNo.Text);
            for (int i = 0; i < 300; i++)
            {
                FstrResult[i] = "";
            }

            //인적사항을 Display
            HEA_JEPSU list = heaJepsuService.GetItembyWrtNo(FnWRTNO);

            if (list.IsNullOrEmpty())
            {
                MessageBox.Show("접수번호가 등록 안됨", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SS1.ActiveSheet.RowCount = 1;
            SS1_Sheet1.Rows[-1].Height = 24;

            FstrPtno = list.PTNO;
            if (FstrPtno != "")
            {
                btnPacs.Enabled = true;
                btnEMR.Enabled = true;
            }

            strSEX = list.SEX.Trim();
            FnPano = list.PANO;
            strSDate = list.SDATE.ToString();
            FstrSdate = list.SDATE.ToString();
            strYYYY = VB.Left(list.SDATE.ToString(), 4);

            SS1.ActiveSheet.Cells[0, 0].Text = FstrPtno.To<string>();
            SS1.ActiveSheet.Cells[0, 1].Text = list.SNAME.Trim();
            SS1.ActiveSheet.Cells[0, 2].Text = list.AGE.ToString() + "/" + list.SEX;
            SS1.ActiveSheet.Cells[0, 3].Text = hb.READ_Ltd_Name(list.LTDCODE.To<string>());
            SS1.ActiveSheet.Cells[0, 4].Text = list.SDATE.ToString();
            SS1.ActiveSheet.Cells[0, 5].Text = hb.READ_GjJong_HeaName(list.GJJONG);

            //EKG 종검 검사 조회
            fn_READ_ETC_JUPMST_TO();

            //EKG 중 검사일자와 동일하면 자동으로 결과 조회
            for (int i = 0; i < ssECG.ActiveSheet.RowCount; i++)
            {
                strSDate_ECG = ssECG.ActiveSheet.Cells[i, 1].Text;
                strResult = ssECG.ActiveSheet.Cells[i, 7].Text;
                strROWID = ssECG.ActiveSheet.Cells[i, 9].Text;
                strImage_Gbn = ssECG.ActiveSheet.Cells[i, 10].Text.Trim();

                //EKG VIEWER 실행
                if (FstrSdate == strSDate_ECG && strResult != "")
                {
                    ETC_JUPMST list2 = etcJupmstService.GetImageGbnbyRowId(strROWID);

                    if (!list2.IsNullOrEmpty())
                    {
                        if (!list2.IMAGE_GBN.IsNullOrEmpty())
                        {
                            strImage_Gbn = list2.IMAGE_GBN.Trim();
                        }
                        //파일 다온로드 '파일 실행
                        if (strImage_Gbn.IsNullOrEmpty())
                        {
                            hc.ECGFILE_DBToFile(strROWID, FstrPtno, "1");
                        }
                        else
                        {
                            hc.ETC_FILE_DBToFile(strROWID, FstrPtno, "1");
                        }
                    }
                }
            }

            //EKG 판독 READ
            txtResult.Text = heaEkgResultService.GetResultbyWrtNo(FnWRTNO);

            grpResult.Enabled = true;
            btnResultSave.Enabled = true;
            txtResult.Enabled = true;
            btnResultCancel.Enabled = true;
        }

        private void frmViewResult_EventClosed()
        {
            fViewResult.Dispose();
            fViewResult = null;
        }

        private void frmHaEcgCode_EventClosed()
        {
            fHaEcgCode.Dispose();
            fHaEcgCode = null;
        }

        void UnloadExamForm()
        {
            try
            {
                if (fViewResult != null)
                {
                    fViewResult.Dispose();
                    fViewResult = null;
                }

                if (fHaEcgCode != null)
                {
                    fHaEcgCode.Dispose();
                    fHaEcgCode = null;
                }
            }
            catch { }
        }


    }
}
