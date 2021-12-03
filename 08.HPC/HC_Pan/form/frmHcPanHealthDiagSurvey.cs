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
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Pan
/// File Name       : frmHcPanHealthDiagSurvey.cs
/// Description     : 건강진단 사전조사표
/// Author          : 이상훈
/// Create Date     : 2019-11-11
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm건강진단사전조사표.frm(Frm건강진단사전조사표)" />

namespace HC_Pan
{
    public partial class frmHcPanHealthDiagSurvey : Form
    {
        HicSjPanoService hicSjPanoService = null;
        HicJepsuService hicJepsuService = null;
        HicSpcPanjengJepsuService hicSpcPanjengJepsuService = null;
        HicSpcPanjengService hicSpcPanjengService = null;
        ComHpcLibBService comHpcLibBService = null;
        HicLtdService hicLtdService = null;
        HicSjMstService hicSjMstService = null;
        HicSjJindanService hicSjJindanService = null;
        HicChukmstService hicChukmstService = null;

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        string FstrROWID;

        public frmHcPanHealthDiagSurvey()
        {
            InitializeComponent();

            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicSjPanoService = new HicSjPanoService();
            hicJepsuService = new HicJepsuService();
            hicSpcPanjengJepsuService = new HicSpcPanjengJepsuService();
            hicSpcPanjengService = new HicSpcPanjengService();
            comHpcLibBService = new ComHpcLibBService();
            hicLtdService = new HicLtdService();
            hicSjMstService = new HicSjMstService();
            hicSjJindanService = new HicSjJindanService();
            hicChukmstService = new HicChukmstService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnMissCompanyView.Click += new EventHandler(eBtnClick);
            this.btnListView.Click += new EventHandler(eBtnClick);
            this.btnHelp.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnPrint2.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.btnDelete.Click += new EventHandler(eBtnClick);
            this.btnPrint3.Click += new EventHandler(eBtnClick);
            this.btnExamResultCreate.Click += new EventHandler(eBtnClick);
            this.btnSave2.Click += new EventHandler(eBtnClick);
            this.btnPrint4.Click += new EventHandler(eBtnClick);
            this.btnBuild.Click += new EventHandler(eBtnClick);
            this.btnSave5.Click += new EventHandler(eBtnClick);
            this.btnPrint5.Click += new EventHandler(eBtnClick);
            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SS4.Change += new ChangeEventHandler(eSpdChange);
            this.SS5.Change += new ChangeEventHandler(eSpdChange);
            this.rdoRecentlyMeasure6.Click += new EventHandler(eRdoClick);
            this.txtJepDate.DoubleClick += new EventHandler(etxtDateDblClick);
            this.txtRFDate.DoubleClick += new EventHandler(etxtDateDblClick);
            this.txtRTDate.DoubleClick += new EventHandler(etxtDateDblClick);
            this.txtMDate.DoubleClick += new EventHandler(etxtDateDblClick);
            this.txtOldFDate.DoubleClick += new EventHandler(etxtDateDblClick);
            this.txtOldTDate.DoubleClick += new EventHandler(etxtDateDblClick);
            this.txtChukDate1.DoubleClick += new EventHandler(etxtDateDblClick);
            this.txtChukDate2.DoubleClick += new EventHandler(etxtDateDblClick);
            this.txtChukChogwa.KeyPress += new KeyPressEventHandler(etxtKeyPress);

            this.txtChukDate1.KeyPress += new KeyPressEventHandler(etxtKeyPress);
            this.txtChukDate2.KeyPress += new KeyPressEventHandler(etxtKeyPress);
            this.txtChukDate2.KeyPress += new KeyPressEventHandler(etxtKeyPress);
            this.txtChukNochul.KeyPress += new KeyPressEventHandler(etxtKeyPress);
            this.txtChukYuhe.KeyPress += new KeyPressEventHandler(etxtKeyPress);
            this.txtCode.KeyPress += new KeyPressEventHandler(etxtKeyPress);
            this.txtDamName.KeyPress += new KeyPressEventHandler(etxtKeyPress);
            this.txtInwon1.KeyPress += new KeyPressEventHandler(etxtKeyPress);
            this.txtInwon2.KeyPress += new KeyPressEventHandler(etxtKeyPress);
            this.txtInwon3.KeyPress += new KeyPressEventHandler(etxtKeyPress);
            this.txtJepDate.KeyPress += new KeyPressEventHandler(etxtKeyPress);
            this.txtJepDate.KeyPress += new KeyPressEventHandler(etxtKeyPress);
            this.txtJepName.KeyPress += new KeyPressEventHandler(etxtKeyPress);
            this.txtJosaRemark.KeyPress += new KeyPressEventHandler(etxtKeyPress);
            this.txtMDate.KeyPress += new KeyPressEventHandler(etxtKeyPress);
            this.txtOldFDate.KeyPress += new KeyPressEventHandler(etxtKeyPress);
            this.txtOldInwon1.KeyPress += new KeyPressEventHandler(etxtKeyPress);
            this.txtOldInwon2.KeyPress += new KeyPressEventHandler(etxtKeyPress);
            this.txtOldInwon3.KeyPress += new KeyPressEventHandler(etxtKeyPress);
            this.txtOldTDate.KeyPress += new KeyPressEventHandler(etxtKeyPress);
            this.txtRemark1.KeyPress += new KeyPressEventHandler(etxtKeyPress);
            this.txtRemark2.KeyPress += new KeyPressEventHandler(etxtKeyPress);
            this.txtRFDate.KeyPress += new KeyPressEventHandler(etxtKeyPress);
            this.txtRTDate.KeyPress += new KeyPressEventHandler(etxtKeyPress);
            this.txtViewLtd.KeyPress += new KeyPressEventHandler(etxtKeyPress);
            this.txtPremedicalExamAgency.KeyPress += new KeyPressEventHandler(etxtKeyPress);
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            long nYear = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);

            nYear = long.Parse(VB.Left(clsPublic.GstrSysDate, 4));

            cboYear.Items.Clear();
            for (int i = 0; i < 10; i++)
            {
                cboYear.Items.Add(nYear);
                nYear -= 1;
            }
            cboYear.SelectedIndex = 0;

            dtpFrDate.Text = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-30).ToShortDateString();
            dtpToDate.Text = clsPublic.GstrSysDate;
            txtViewLtd.Text = "";

            fn_Screen_Clear();
        }

        void eBtnClick(object sender, EventArgs e)
        {
            int result = 0;

            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnCancel)
            {
                fn_Screen_Clear();
            }
            else if (sender == btnListView)
            {
                int nREAD = 0;
                int nRow = 0;
                long nLtdCode = 0;
                string strGjYear = "";
                string strFrDate = "";
                string strToDate = "";
                string strViewLtd = "";

                Cursor.Current = Cursors.WaitCursor;

                strGjYear = cboYear.Text;
                if (!txtCode.Text.IsNullOrEmpty())
                {
                    nLtdCode = long.Parse(VB.Pstr(txtCode.Text, ".", 1));
                }
                else
                {
                    nLtdCode = 0;
                }

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;

                strViewLtd = txtViewLtd.Text.Trim();

                sp.Spread_All_Clear(ssList);
                ssList.ActiveSheet.RowCount = 50;

                List<COMHPC> list = comHpcLibBService.GetHicSjMstLtdbyGjYear(strGjYear, strFrDate, strToDate, strViewLtd);

                nREAD = list.Count;
                ssList.ActiveSheet.RowCount = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    ssList.ActiveSheet.Cells[i, 0].Text = list[i].JEPDATE;
                    ssList.ActiveSheet.Cells[i, 1].Text = list[i].LTDCODE.ToString();
                    ssList.ActiveSheet.Cells[i, 2].Text = list[i].NAME;
                }

                Cursor.Current = Cursors.Default;
            }
            else if (sender == btnBuild)
            {
                int nREAD = 0;
                int nREAD2 = 0;
                long nWRTNO = 0;
                long nWrtno2 = 0;
                string strYear = "";
                string strPan = "";
                string strPanjeng = "";
                long nLtdCode = 0;
                string strJepDate = "";

                strYear = cboYear.Text;
                nLtdCode = VB.Pstr(txtCode.Text, ".", 1).To<long>();
                strJepDate = strYear + "-01-01";

                clsDB.setBeginTran(clsDB.DbCon);

                //기존 자료에 삭제 표시
                result = hicSjPanoService.UPdateGbDelbyLtdCode(strYear, nLtdCode);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("기존자료 삭제 Falg 표시중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //접수자료를 읽어 DB에 업데이트
                List<HIC_JEPSU> list = hicJepsuService.GetIetmbyJepDate(strJepDate, strYear, nLtdCode);

                nREAD = list.Count;
                for (int i = 0; i < nREAD; i++)
                {
                    nWRTNO = list[i].WRTNO;

                    //전년도 접수번호를 찾기
                    nWrtno2 = hicJepsuService.GetWrtNo2byPano(list[i].PANO, strYear, nLtdCode);

                    //전년도 판정결과를 읽음
                    if (nWrtno2 > 0)
                    {
                        List<HIC_SPC_PANJENG> list2 = hicSpcPanjengService.GetItembyWrtNo2(nWrtno2);

                        nREAD2 = list2.Count;
                        for (int j = 0; j < nREAD2; j++)
                        {
                            switch (list2[j].PANJENG)
                            {
                                case "1":
                                case "2":
                                    strPan = "A";
                                    break;
                                case "3":
                                    strPan = "C1";
                                    break;
                                case "4":
                                    strPan = "C2";
                                    break;
                                case "5":
                                    strPan = "D1";
                                    break;
                                case "6":
                                    strPan = "D2";
                                    break;
                                case "7":
                                case "8":
                                    strPan = "U";
                                    break;
                                case "9":
                                    strPan = "CN";
                                    break;
                                case "A":
                                    strPan = "DN";
                                    break;
                                default:
                                    break;
                            }

                            if (VB.InStr(strPanjeng, strPan + ",") == 0)
                            {
                                if (strPanjeng == "A,")
                                {
                                    strPanjeng = strPan + ",";
                                }
                                else
                                {
                                    strPanjeng += strPan + ",";
                                }
                            }
                        }
                        if (!strPanjeng.IsNullOrEmpty())
                        {
                            strPanjeng = VB.Left(strPanjeng, strPanjeng.Length - 1);
                        }
                    }

                    //대상자 명단에 업데이트
                    HIC_SJ_PANO list3 = hicSjPanoService.GetItembyGjYearLtdCodeWrtNo(strYear, nLtdCode, nWRTNO);

                    HIC_SJ_PANO item = new HIC_SJ_PANO();

                    item.GJYEAR = strYear;
                    item.LTDCODE = nLtdCode;
                    item.WRTNO = nWRTNO;
                    item.BUSE = list[i].BUSENAME;
                    item.CHUKRESULT = "노출기준미만";
                    item.PANJENG = strPanjeng;
                    item.GBDEL = "";
                    item.RID = list3.RID;

                    if (list3.IsNullOrEmpty())
                    {
                        result = hicSjPanoService.Insert(item);
                    }
                    else
                    {
                        result = hicSjPanoService.UpdatePanjengGbDelbyRowId(item);
                    }

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("대상자 명단 저장중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                fn_Screen_Display5();
                fn_Screen_Display3();
                
                MessageBox.Show("작업 완료!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (sender == btnDelete)
            {
                string strGjYear = "";
                long nLtdCode = 0;

                strGjYear = cboYear.Text;
                nLtdCode = long.Parse(VB.Pstr(txtCode.Text, ".", 1));

                if (MessageBox.Show("정말로 삭제를 하시겠습니까?", "확인", MessageBoxButtons.OK, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                clsDB.setBeginTran(clsDB.DbCon);

                result = comHpcLibBService.DeleteHicSjMstbyRowId(FstrROWID);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("건강진단 사전조사표 마스타 삭제중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                result = comHpcLibBService.DeleteHicSjJindanbyGjYearLtdCode(strGjYear, nLtdCode);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("건강진단 사전조사표(과년도 특수건강진단결과 요약) 삭제중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                result = comHpcLibBService.DeleteHicSjPanobyGjYearLtdCode(strGjYear, nLtdCode);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("건강진단 사전조사 대상자명단 삭제중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                fn_Screen_Clear();
                eBtnClick(btnListView, new EventArgs());

                clsDB.setCommitTran(clsDB.DbCon);

                MessageBox.Show("삭제 완료!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (sender == btnSave)
            {
                string strNewLtd = "";
                string strTel = "";
                string strIn = "";
                string strOut = "";
                string strOutPlace = "";
                string strMaterial1 = "";
                string strMaterial2 = "";
                string strMsds1 = "";
                string strMsds2 = "";
                string strMsds3 = "";
                string strMsds4 = "";
                string strPremedicalExamAgency = "";    //이전검진기관
                string strRecentlyMeasure = "";         //측정기관

                if (rdoNewLtd1.Checked == true)
                {
                    strNewLtd = "1";
                }
                else if (rdoNewLtd2.Checked == true)
                {
                    strNewLtd = "2";
                }
                else
                {
                    strNewLtd = "";
                }

                if (rdoTel1.Checked == true)
                {
                    strTel = "1";
                }
                else if (rdoTel2.Checked == true)
                {
                    strTel = "2";
                }
                else
                {
                    strTel = "";
                }

                if (chkInHosp.Checked == true)
                {
                    strIn = "Y";
                }
                else
                {
                    strIn = "N";
                }

                if (chkOut.Checked == true)
                {
                    strOut = "Y";
                }
                else
                {
                    strOut = "N";
                }

                if (rdoOut1.Checked == true)
                {
                    strOutPlace = "1";
                }
                else if (rdoOut2.Checked == true)
                {
                    strOutPlace = "2";
                }
                else if (rdoOut3.Checked == true)
                {
                    strOutPlace = "3";
                }
                else
                {
                    strOutPlace = "";
                }

                if (chkMaterial1.Checked == true)
                {
                    strMaterial1 = "Y";
                }
                else
                {
                    strMaterial1 = "N";
                }

                if (chkMaterial2.Checked == true)
                {
                    strMaterial2 = "Y";
                }
                else
                {
                    strMaterial2 = "N";
                }

                if (chkMsds1.Checked == true)
                {
                    strMsds1 = "Y";
                }
                else
                {
                    strMsds1 = "N";
                }

                if (chkMsds2.Checked == true)
                {
                    strMsds2 = "Y";
                }
                else
                {
                    strMsds2 = "N";
                }

                if (chkMsds3.Checked == true)
                {
                    strMsds3 = "Y";
                }
                else
                {
                    strMsds3 = "N";
                }

                if (chkMsds4.Checked == true)
                {
                    strMsds4 = "Y";
                }
                else
                {
                    strMsds4 = "N";
                }

                if (rdoPremedicalExamAgency1.Checked == true)
                {
                    strPremedicalExamAgency = "포항성모";
                }
                else if (rdoPremedicalExamAgency2.Checked == true)
                {
                    strPremedicalExamAgency = "동국대";
                }
                else if (rdoPremedicalExamAgency3.Checked == true)
                {
                    strPremedicalExamAgency = "세명기독";
                }
                else if (rdoPremedicalExamAgency4.Checked == true)
                {
                    strPremedicalExamAgency = "보건협회";
                }
                else if (rdoPremedicalExamAgency5.Checked == true)
                {
                    strPremedicalExamAgency = txtPremedicalExamAgency.Text.Trim();
                }
                else
                {
                    strPremedicalExamAgency = "";
                }

                if (rdoRecentlyMeasure1.Checked == true)
                {
                    strRecentlyMeasure = "포항성모";
                }
                else if (rdoRecentlyMeasure2.Checked == true)
                {
                    strRecentlyMeasure = "동국대";
                }
                else if (rdoRecentlyMeasure3.Checked == true)
                {
                    strRecentlyMeasure = "세명";
                }
                else if (rdoRecentlyMeasure4.Checked == true)
                {
                    strRecentlyMeasure = "유성";
                }
                else if (rdoRecentlyMeasure5.Checked == true)
                {
                    strRecentlyMeasure = "해당없음";
                }
                if (rdoRecentlyMeasure6.Checked == true)
                {
                    strRecentlyMeasure = txtRecentlyMeasureEtc.Text.Trim();
                }
                else
                {
                    strRecentlyMeasure = "";
                }

                HIC_SJ_MST item = new HIC_SJ_MST();
                item.GJYEAR = cboYear.Text;      
                item.JEPDATE = txtJepDate.Text;
                item.INWON1 = txtInwon1.Text.To<long>();
                item.INWON2 = txtInwon2.Text.To<long>();
                item.INWON3 = txtInwon3.Text.To<long>();
                item.JEPNAME = txtJepName.Text;
                item.DAMNAME = txtDamName.Text;
                item.JOSAREMARK = txtJosaRemark.Text;
                item.RFDATE = dtpFrDate.Text;
                item.RTDATE = dtpToDate.Text;
                item.MDATE = txtMDate.Text;
                item.OLDFDATE = txtOldFDate.Text;
                item.OLDTDATE = txtOldTDate.Text;
                item.OLDINWON1 = txtOldInwon1.Text.To<long>();
                item.OLDINWON2 = txtOldInwon2.Text.To<long>();
                item.OLDINWON3 = txtOldInwon3.Text.To<long>();
                item.CHUKDATE1 = txtChukDate1.Text;
                item.CHUKDATE2 = txtChukDate2.Text;
                item.CHUKNOCHUL = txtChukNochul.Text;
                item.CHUKCHOGWA = txtChukChogwa.Text;
                item.CHUKYUHE = txtChukYuhe.Text;
                item.REMARK1 = txtRemark1.Text;
                item.REMARK2 = txtRemark2.Text;
                item.ENTSABUN = clsType.User.IdNumber.To<long>(); 
                if (txtCode.Text.Trim() != "")
                {
                    item.LTDCODE = VB.Pstr(txtCode.Text, ".", 1).To<long>(); 
                }
                else
                {
                    item.LTDCODE = 0;
                }
                item.GBNEWLTD = strNewLtd;
                item.GBTEL = strTel;
                item.PLACE1 = strIn;
                item.PLACE2 = strOut;
                item.PLACE3 = strOutPlace;
                item.GBMAIL1 = strMaterial1;
                item.GBMAIL2 = strMaterial2;
                item.GBJUNBI1 = strMsds1;
                item.GBJUNBI2 = strMsds2;
                item.GBJUNBI3 = strMsds3;
                item.GBJUNBI4 = strMsds4;
                item.OLDHOSPITAL = strPremedicalExamAgency;
                item.CHUKHOSPITAL = strRecentlyMeasure;

                //오류 점검
                if (txtJepDate.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("사전접수일이 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (txtJepName.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("사전접수자가 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (txtDamName.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("검진의뢰자가 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (txtJosaRemark.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("주요내용이 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (txtRFDate.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("검진시작 예정일자가 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (txtRTDate.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("검진종료 예정일자가 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsDB.setBeginTran(clsDB.DbCon);

                if (FstrROWID.IsNullOrEmpty())
                {
                    result = hicSjMstService.Insert(item);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("사전조사표 저장시 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                result = hicSjMstService.Update(item, strOut);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("사전조사표 저장시 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                fn_Screen_Clear();
            }
            else if (sender == btnSave2)
            {
                string strPan = "";
                string strBuse = "";
                string strYuhe = "";
                string strJanggi = "";
                long nInwon = 0;
                string strROWID = "";
                string strChk = "";
                string strChange = "";
                string strGjYear = "";
                long nLtdCode = 0;

                strGjYear = cboYear.Text;
                nLtdCode = VB.Pstr(txtCode.Text, ".", 1).To<long>();

                clsDB.setBeginTran(clsDB.DbCon);

                for (int i = 0; i < SS4.ActiveSheet.NonEmptyRowCount; i++)
                {
                    strChk = SS4.ActiveSheet.Cells[i, 0].Text.Trim();
                    strPan = SS4.ActiveSheet.Cells[i, 1].Text.Trim();
                    strBuse = SS4.ActiveSheet.Cells[i, 2].Text.Trim();
                    strYuhe = SS4.ActiveSheet.Cells[i, 3].Text.Trim();
                    strJanggi = SS4.ActiveSheet.Cells[i, 4].Text.Trim();
                    nInwon = SS4.ActiveSheet.Cells[i, 5].Text.Trim().To<long>(); 
                    strROWID = SS4.ActiveSheet.Cells[i, 6].Text.Trim();
                    strChange = SS4.ActiveSheet.Cells[i, 7].Text.Trim();

                    if (strROWID.IsNullOrEmpty())
                    {
                        if (!strPan.IsNullOrEmpty() || !strBuse.IsNullOrEmpty() || !strYuhe.IsNullOrEmpty())
                        {
                            result = hicSjJindanService.Insert(strGjYear, nLtdCode, strPan, strBuse, strYuhe, strJanggi, nInwon);
                        }
                    }
                    else
                    {
                        if (strChk == "True")
                        {
                            result = hicSjJindanService.Delete(strROWID);
                        }
                        else if (strChange == "Y")
                        {
                            result = hicSjJindanService.Update(strPan, strBuse, strYuhe, strJanggi, nInwon, strROWID);
                        }
                    }

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show(i + " 번줄 저장 오류!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                MessageBox.Show("저장 완료", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);

                fn_Screen_Display4();
            }
            else if (sender == btnSave5)
            {
                string strROWID = "";
                string strChange = "";
                string strBuse = "";
                string strChukResult = "";
                string strPanjeng = "";

                clsDB.setBeginTran(clsDB.DbCon);

                for (int i = 0; i < SS5.ActiveSheet.RowCount; i++)
                {
                    strChange = SS5.ActiveSheet.Cells[i, 7].Text.Trim();
                    if (strChange == "Y")
                    {
                        strBuse = SS5.ActiveSheet.Cells[i, 0].Text.Trim();
                        strChukResult = SS5.ActiveSheet.Cells[i, 4].Text.Trim();
                        strPanjeng = SS5.ActiveSheet.Cells[i, 5].Text.Trim();
                        strROWID = SS5.ActiveSheet.Cells[i, 6].Text.Trim();

                        result = hicSjPanoService.UpdatebyRowId(strBuse, strChukResult, strPanjeng, strROWID);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show(i + " 번줄 저장 오류", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                MessageBox.Show("저장 완료", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);

                fn_Screen_Display3();
                fn_Screen_Display5();
            }
            else if (sender == btnSearch)
            {
                int nREAD = 0;
                string strGjYear = "";
                string strBoName = "";
                long nCNT1 = 0;
                long nCNT2 = 0;
                long nCNT3 = 0;
                string strCode = "";
                long nLtdCode = 0;

                string strFrDate = "";
                string strToDate = "";

                txtCode.Text = txtCode.Text.Trim();
                strCode = txtCode.Text;
                strGjYear = cboYear.Text;


                if (txtCode.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("회사코드가 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                HIC_LTD list = hicLtdService.GetItembyCode(VB.Pstr(txtCode.Text, ".", 1));

                if (list.IsNullOrEmpty())
                {
                    MessageBox.Show("회사코드가 등록 않됨", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                SSLtd.ActiveSheet.Cells[0, 1].Text = list.SANGHO;
                SSLtd.ActiveSheet.Cells[0, 3].Text = list.DAEPYO;
                SSLtd.ActiveSheet.Cells[0, 5].Text = list.BONAME;
                if (!list.BOJIK.IsNullOrEmpty())
                {
                    SSLtd.ActiveSheet.Cells[0, 5].Text += " " + list.BOJIK;
                }
                SSLtd.ActiveSheet.Cells[1, 1].Text = list.JUSO + " " + list.JUSODETAIL;
                SSLtd.ActiveSheet.Cells[1, 5].Text = list.HTEL;
                SSLtd.ActiveSheet.Cells[2, 1].Text = list.JEPUMLIST;
                SSLtd.ActiveSheet.Cells[2, 3].Text = list.TEL;
                SSLtd.ActiveSheet.Cells[2, 5].Text = list.FAX;

                SSLtd.ActiveSheet.Cells[3, 1].Text = list.REMARK;
                strBoName = list.BONAME;

                // 사전조사 내용을 읽음
                HIC_SJ_MST list2 = hicSjMstService.GetItembyGjYearLtdCode(strGjYear, VB.Pstr(strCode, ".", 1));

                if (list2.IsNullOrEmpty())
                {
                    rdoNewLtd2.Checked = true;  //기존사업장
                    rdoTel1.Checked = true;     //전화조사
                    txtJepDate.Text = clsPublic.GstrSysDate;
                    txtJepName.Text = clsPublic.GstrJobName;
                    txtDamName.Text = strBoName;
                    txtJosaRemark.Text = "건강진단업무 및 일정 협의";
                    txtMDate.Text = clsPublic.GstrSysDate;
                    chkInHosp.Checked = true;
                    chkMsds1.Checked = true;
                    chkMsds2.Checked = true;
                    chkMsds3.Checked = true;
                    chkMsds4.Checked = false;
                    txtChukNochul.Text = "해당없음";
                    txtChukChogwa.Text = "해당없음";
                    txtChukYuhe.Text = "해당없음";
                    FstrROWID = "";

                    btnSave.Enabled = true;
                    btnDelete.Enabled = false;
                    btnPrint.Enabled = false;
                    btnPrint2.Enabled = false;
                }
                else
                {
                    if (list2.GBNEWLTD == "1")
                    {
                        rdoNewLtd1.Checked = true;  //신규사업장
                    }
                    else
                    {
                        rdoNewLtd2.Checked = true;  //기존사업장
                    }

                    if (list2.GBTEL == "1")
                    {
                        rdoTel1.Checked = true;  //전화
                    }
                    else
                    {
                        rdoTel2.Checked = true;  //방문
                    }

                    txtInwon1.Text = list2.INWON1.To<string>();
                    txtInwon2.Text = list2.INWON2.To<string>();
                    txtInwon3.Text = list2.INWON3.To<string>();
                    txtJepDate.Text = list2.JEPDATE;
                    txtJepName.Text = list2.JEPNAME;
                    txtDamName.Text = list2.DAMNAME;
                    txtJosaRemark.Text = list2.JOSAREMARK;
                    txtRFDate.Text = list2.RFDATE;
                    txtRTDate.Text = list2.RTDATE;
                    if (list2.PLACE1 == "Y") chkInHosp.Checked = true;
                    if (list2.PLACE2 == "Y")
                    {
                        chkOut.Checked = true;
                        if (list2.PLACE3.Trim() == "1")
                        {
                            rdoOut1.Checked = true; //회의실
                        }
                        else if (list2.PLACE3.Trim() == "2")
                        {
                            rdoOut2.Checked = true; //식당
                        }
                        else
                        {
                            rdoOut3.Checked = true; //기타
                        }
                    }

                    txtMDate.Text = list2.MDATE;
                    if (list2.GBMAIL1 == "Y") chkMaterial1.Checked = true;
                    if (list2.GBMAIL2 == "Y") chkMaterial2.Checked = true;
                    if (list2.GBJUNBI1 == "Y") chkMsds1.Checked = true;
                    if (list2.GBJUNBI2 == "Y") chkMsds2.Checked = true;
                    if (list2.GBJUNBI3 == "Y") chkMsds3.Checked = true;
                    if (list2.GBJUNBI4 == "Y") chkMsds4.Checked = true;

                    //이전 특수검진
                    switch (list2.OLDHOSPITAL)
                    {
                        case "포항성모":
                            rdoPremedicalExamAgency1.Checked = true;
                            break;
                        case "동국대":
                            rdoPremedicalExamAgency2.Checked = true;
                            break;
                        case "세명기독":
                            rdoPremedicalExamAgency3.Checked = true;
                            break;
                        case "보건협회":
                            rdoPremedicalExamAgency4.Checked = true;
                            break;
                        default:
                            rdoPremedicalExamAgency5.Checked = true;
                            txtPremedicalExamAgency.Text = list2.OLDHOSPITAL;
                            break;
                    }
                    txtOldFDate.Text = list2.OLDFDATE;
                    txtOldTDate.Text = list2.OLDTDATE;
                    txtOldInwon1.Text = list2.OLDINWON1.ToString();
                    txtOldInwon2.Text = list2.OLDINWON2.ToString();
                    txtOldInwon3.Text = list2.OLDINWON3.ToString();

                    //작업환경측정
                    switch (list2.CHUKHOSPITAL)
                    {
                        case "포항성모":
                            rdoRecentlyMeasure1.Checked = true;
                            break;
                        case "동국대":
                            rdoRecentlyMeasure2.Checked = true;
                            break;
                        case "세명":
                            rdoRecentlyMeasure3.Checked = true;
                            break;
                        case "유성":
                            rdoRecentlyMeasure4.Checked = true;
                            break;
                        case "해당없음":
                            rdoRecentlyMeasure6.Checked = true;
                            break;
                        default:
                            rdoRecentlyMeasure5.Checked = true;
                            txtRecentlyMeasureEtc.Text = list2.CHUKHOSPITAL;
                            break;
                    }

                    txtChukDate1.Text = list2.CHUKDATE1;
                    txtChukDate2.Text = list2.CHUKDATE2;
                    txtChukNochul.Text = list2.CHUKNOCHUL;
                    txtChukChogwa.Text = list2.CHUKCHOGWA;
                    txtChukYuhe.Text = list2.CHUKYUHE;
                    txtRemark1.Text = list2.REMARK1;
                    txtRemark2.Text = list2.REMARK2;
                    FstrROWID = list2.RID;

                    btnSave.Enabled = true;
                    btnDelete.Enabled = true;
                    btnPrint.Enabled = true;
                    btnPrint2.Enabled = true;
                }

                //신규등록이면 전년도 특검 인원수를 불러옴
                if (FstrROWID.IsNullOrEmpty())
                {
                    strGjYear = cboYear.Text;
                    strFrDate = strGjYear + "-01-01";
                    strToDate = strGjYear + "-12-31";

                    HIC_JEPSU list3 = hicJepsuService.GetJepDatebyGjYear(strFrDate, strToDate, strGjYear, nLtdCode);

                    if (!list3.STARTDATE.IsNullOrEmpty())
                    {
                        rdoPremedicalExamAgency1.Checked = true;
                        txtOldFDate.Text = list3.STARTDATE;
                        txtOldTDate.Text = list3.ENDDATE;

                        List<HIC_JEPSU> list4 = hicJepsuService.GetItembyJepDateGjYearLtdCode(strFrDate, strToDate, strGjYear, nLtdCode, "");

                        nREAD = list4.Count;
                        nCNT1 = 0;
                        nCNT2 = 0;
                        nCNT3 = 0;

                        for (int i = 0; i < nREAD; i++)
                        {
                            if (list4[i].GJJONG == "23")
                            {
                                nCNT2 += 1;
                            }
                            else
                            {
                                if (list4[i].UCODES.IsNullOrEmpty())
                                {
                                    nCNT3 += 1;
                                }
                                else
                                {
                                    nCNT1 += 1;
                                }
                            }
                        }

                        txtOldInwon1.Text = nCNT1.To<string>();
                        txtOldInwon2.Text = nCNT2.To<string>();
                        txtOldInwon3.Text = nCNT3.To<string>();
                    }
                }

                //작업환경측정 정보 읽기
                if (FstrROWID.IsNullOrEmpty())
                {
                    strGjYear = string.Format("{0:0000}", long.Parse(cboYear.Text) - 1);

                    List<HIC_CHUKMST> list4 = hicChukmstService.GetSDatebySDateLtdCode(strFrDate, nLtdCode);

                    nREAD = list4.Count;
                    if (nREAD >= 1)
                    {
                        rdoPremedicalExamAgency1.Checked = true;
                        txtChukDate1.Text = list4[0].SDATE;
                        if (nREAD >= 2)
                        {
                            txtChukDate2.Text = list4[1].SDATE;
                        }
                    }
                }

                fn_Screen_Display3();
                fn_Screen_Display4();
                fn_Screen_Display5();

                tabControl1.TabIndex = 0;
                tabControl1.SelectedTab = tab1;
            }
            else if (sender == btnExamResultCreate)
            {
                int nREAD = 0;
                int nREAD2 = 0;
                long nWRTNO = 0;
                long nWrtno2 = 0;
                string strYear = "";
                string strPan = "";
                string strPanjeng = "";
                string strBuseName = "";
                string strUCode = "";
                string strYuhe = "";
                long nInwon = 0;

                string strGjYear = "";
                long nLtdCode = 0;
                string strJepDate = "";

                strGjYear = cboYear.Text;
                nLtdCode = VB.Pstr(txtCode.Text, ".", 1).To<long>();
                strJepDate = strGjYear + "-01-01";

                if (MessageBox.Show("기존 자료를 삭제 후 다시 형성을 하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                //기존 자료를 삭제
                result = hicSjJindanService.DeletebyGjYearLtdCode(strGjYear, nLtdCode);

                strYear = string.Format("{0:0000}", cboYear.Text);

                //과년도 접수자료를 읽어 진단결과를 업데이트
                List<HIC_JEPSU> list = hicJepsuService.GetItembyJepDateGjYearLtdCodeGjJong(strJepDate, strGjYear, nLtdCode);

                nREAD = list.Count;
                for (int i = 0; i < nREAD; i++)
                {
                    nWRTNO = list[i].WRTNO;
                    strBuseName = list[i].BUSENAME;
                    if (strBuseName.IsNullOrEmpty())
                    {
                        strBuseName = "기타";
                        strPanjeng = "";
                        strUCode = "";

                        //전년도 판정결과를 읽음
                        List<HIC_SPC_PANJENG> list2 = hicSpcPanjengService.GetItembyWrtNo(nWRTNO);

                        nREAD2 = list2.Count;
                        for (int j = 0; j < nREAD2; j++)
                        {
                            switch (list2[j].PANJENG)
                            {
                                case "1":
                                case "2":
                                    strPan = "A";
                                    break;
                                case "3":
                                    strPan = "C1";
                                    break;
                                case "4":
                                    strPan = "C2";
                                    break;
                                case "5":
                                    strPan = "D1";
                                    break;
                                case "6":
                                    strPan = "D2";
                                    break;
                                case "7":
                                case "8":
                                    strPan = "U";
                                    break;
                                case "9":
                                    strPan = "CN";
                                    break;
                                case "A":
                                    strPan = "DN";
                                    break;
                                default:
                                    break;
                            }

                            if (VB.InStr(strPanjeng, strPan + ",") == 0)
                            {
                                strPanjeng += strPan + ",";
                            }
                            strUCode += list2[j].MCODE + ",";
                        }
                        if (!strPanjeng.IsNullOrEmpty())
                        {
                            strPanjeng = VB.Left(strPanjeng, strPanjeng.Length - 1);
                        }

                        if (!strPanjeng.IsNullOrEmpty())
                        {
                            strYuhe = hm.UCode_Names_Display(strUCode);

                            //대상자 명단에 업데이트
                            HIC_SJ_JINDAN list3 = hicSjJindanService.GetInwonRowIdbyGjYear(strGjYear, nLtdCode, strPanjeng, strBuseName, strYuhe);

                            if (list3 == null)
                            {
                                result = hicSjJindanService.Insert(strGjYear, nLtdCode, strPanjeng, strBuseName, strYuhe, "", 1);
                            }
                            else
                            {
                                nInwon = list3.INWON + 1;

                                result = hicSjJindanService.UpdateInwon(nInwon, list3.RID);
                            }
                        }
                    }
                }
                fn_Screen_Display4();

                MessageBox.Show("작업 완료", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (sender == btnMissCompanyView)
            {
                int nREAD = 0;
                int nRow = 0;
                long nLtdCode = 0;
                string strYear = "";
                string strViewLtd = "";

                strYear = cboYear.Text;
                strViewLtd = txtViewLtd.Text;

                sp.Spread_All_Clear(ssList);
                //ssList.ActiveSheet.RowCount = 50;

                List<COMHPC> list = comHpcLibBService.GetItembyHicMirHemsLtd(strYear, strViewLtd);

                nREAD = list.Count;
                //ssList.ActiveSheet.RowCount = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    nLtdCode = list[i].LTDCODE;

                    if (hicSjMstService.GetCountbyGjYearLtdCode(strYear, nLtdCode) == 0)
                    {
                        nRow += 1;
                        if (ssList.ActiveSheet.RowCount < nRow)
                        {
                            ssList.ActiveSheet.RowCount = nRow;
                        }

                        ssList.ActiveSheet.Cells[nRow - 1, 0].Text = "";
                        ssList.ActiveSheet.Cells[nRow - 1, 1].Text = nLtdCode.ToString();
                        ssList.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].NAME.Trim();
                    }
                }

            }
            else if (sender == btnHelp)
            {
                string strLtdCode = "";

                if (txtCode.Text.IndexOf(".") > 0)
                {
                    strLtdCode = VB.Pstr(txtCode.Text, ".", 2);
                }
                else
                {
                    strLtdCode = txtCode.Text;
                }

                FrmHcLtdHelp = new frmHcLtdHelp(strLtdCode);
                FrmHcLtdHelp.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(LtdCd_value);
                FrmHcLtdHelp.ShowDialog();
                FrmHcLtdHelp.rSetGstrValue -= new frmHcLtdHelp.SetGstrValue(LtdCd_value);

                if (!LtdHelpItem.IsNullOrEmpty())
                {
                    txtCode.Text = LtdHelpItem.CODE.ToString() + "." + LtdHelpItem.SANGHO;
                }
                else
                {
                    txtCode.Text = "";
                }
            }
            else if (sender == btnPrint)
            {
                int nREAD = 0;
                string strGjYear = "";
                long nCNT1 = 0;
                long nCNT2 = 0;
                long nCNT3 = 0;
                string strCode = "";

                txtCode.Text = txtCode.Text.Trim();
                strGjYear = cboYear.Text;

                if (txtCode.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("회사코드가 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                strCode = VB.Pstr(txtCode.Text, ".", 1);
                HIC_LTD list = hicLtdService.GetItembyCode(strCode);
                
                if (list == null)
                {
                    MessageBox.Show("회사코드가 등록 않됨!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;

                SS1.ActiveSheet.Cells[1, 0].Text = "  " + cboYear.Text + "년 건강진단 사전조사표";
                SS1.ActiveSheet.Cells[5, 1].Text = list.SANGHO;

                SS1.ActiveSheet.Cells[5, 5].Text = list.DAEPYO;
                SS1.ActiveSheet.Cells[6, 1].Text = list.JUSO + " " + list.JUSODETAIL;
                SS1.ActiveSheet.Cells[6, 8].Text = list.BONAME;
                if (!list.BOJIK.IsNullOrEmpty())
                {
                    SS1.ActiveSheet.Cells[6, 8].Text += " " + list.BOJIK;
                }
                SS1.ActiveSheet.Cells[7, 8].Text = list.HTEL;
                SS1.ActiveSheet.Cells[8, 1].Text = list.JEPUMLIST;
                SS1.ActiveSheet.Cells[8, 5].Text = list.TEL;
                SS1.ActiveSheet.Cells[8, 8].Text = list.FAX;

                //사전조사 내용을 읽음
                HIC_SJ_MST list2 = hicSjMstService.GetItembyGjYearLtdCode(strGjYear, strCode);

                if (list2.IsNullOrEmpty())
                {
                    MessageBox.Show("사전조사 마스타에 등록이 안됨", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (list2.GBNEWLTD == "1" && list2.GBTEL == "1")
                {
                    SS1.ActiveSheet.Cells[1, 7].Text = "■신규사업장 ■전화상담 ";
                    SS1.ActiveSheet.Cells[2, 7].Text = "□기존사업장 □방문조사 ";
                }
                else if (list2.GBNEWLTD == "2" && list2.GBTEL == "1")
                {
                    SS1.ActiveSheet.Cells[1, 7].Text = "□신규사업장 ■전화상담 ";
                    SS1.ActiveSheet.Cells[2, 7].Text = "■기존사업장 □방문조사 ";
                }
                else if (list2.GBNEWLTD == "2" && list2.GBTEL == "1")
                {
                    SS1.ActiveSheet.Cells[1, 7].Text = "□신규사업장 □전화상담 ";
                    SS1.ActiveSheet.Cells[2, 7].Text = "■기존사업장 ■방문조사 ";
                }
                else
                {
                    SS1.ActiveSheet.Cells[1, 7].Text = "■신규사업장 □전화상담 ";
                    SS1.ActiveSheet.Cells[2, 7].Text = "□기존사업장 ■방문조사 ";
                }

                nCNT1 = list2.INWON1; //일특
                nCNT2 = list2.INWON2; //특수
                nCNT3 = list2.INWON3; //일반

                SS1.ActiveSheet.Cells[5, 8].Text = (nCNT1 + nCNT2 + nCNT3) + "명";

                SS1.ActiveSheet.Cells[11, 2].Text = " 접수(협의)일자: " + list2.JEPDATE;
                SS1.ActiveSheet.Cells[12, 2].Text = " 사전조사 접수(협의)자: " + list2.JEPNAME;
                SS1.ActiveSheet.Cells[12, 6].Text = " 검진의뢰(협의)자: " + list2.DAMNAME;
                SS1.ActiveSheet.Cells[13, 2].Text = " 주요내용: " + list2.JOSAREMARK;
                SS1.ActiveSheet.Cells[14, 2].Text = " 검진예정일: " + list2.RFDATE + " ~ " + list2.RTDATE;
                SS1.ActiveSheet.Cells[15, 2].Text = " 검진예정인원: " + (nCNT1 + nCNT2 + nCNT3) + "명(일반특수:";
                SS1.ActiveSheet.Cells[15, 2].Text += nCNT1 + "명, 특수:" + nCNT2 + "명, 일반:" + nCNT3 + "명)";
                SS1.ActiveSheet.Cells[16, 2].Text = " 검진실시 장소: ";

                if (list2.PLACE1 == "Y" && list2.PLACE2 == "Y")
                {
                    SS1.ActiveSheet.Cells[16, 2].Text += "■내원(건강증진센터) ■출장";
                    if (list2.PLACE2 == "Y")
                    {
                        if (list2.PLACE3 == "1")
                        {
                            SS1.ActiveSheet.Cells[16, 2].Text += "(회의실)";
                        }
                        else if (list2.PLACE3 == "2")
                        {
                            SS1.ActiveSheet.Cells[16, 2].Text += "(식당)";
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[16, 2].Text += "(기타)";
                        }
                    }
                }
                else if (list2.PLACE1 == "Y")
                {
                    SS1.ActiveSheet.Cells[16, 2].Text += "■내원(건강증진센터)";
                }
                else if (list2.PLACE2 == "Y")
                {
                    SS1.ActiveSheet.Cells[16, 2].Text += "■출장";
                    if (list2.PLACE3 == "1")
                    {
                        SS1.ActiveSheet.Cells[16, 2].Text += "(회의실)";
                    }
                    else if (list2.PLACE3 == "2")
                    {
                        SS1.ActiveSheet.Cells[16, 2].Text += "(식당)";
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[16, 2].Text += "(기타)";
                    }
                }

                SS1.ActiveSheet.Cells[17, 2].Text = " 검진명단 수령예정시기: " + list2.MDATE;
                SS1.ActiveSheet.Cells[18, 3].Text = " ";

                if (list2.GBJUNBI1 == "Y") SS1.ActiveSheet.Cells[18, 3].Text += "MSDS / ";
                if (list2.GBJUNBI2 == "Y") SS1.ActiveSheet.Cells[18, 3].Text += "작업환경측정결과 / ";
                if (list2.GBJUNBI3 == "Y") SS1.ActiveSheet.Cells[18, 3].Text += "특수건강진단결과 / ";
                if (list2.GBJUNBI4 == "Y") SS1.ActiveSheet.Cells[18, 3].Text += "근무조직도 / ";
                if (VB.Right(SS1.ActiveSheet.Cells[18, 3].Text, 3) == " / ")
                {
                    SS1.ActiveSheet.Cells[18, 3].Text = VB.Left(SS1.ActiveSheet.Cells[18, 3].Text, SS1.ActiveSheet.Cells[18, 3].Text.Length - 3);
                }
                SS1.ActiveSheet.Cells[19, 3].Text = " 검진준비물(문진표,객담통,소변컵)수령방법:";
                if (list2.GBMAIL1 == "Y") SS1.ActiveSheet.Cells[19, 3].Text += "■우편 ";
                if (list2.GBMAIL2 == "Y") SS1.ActiveSheet.Cells[19, 3].Text += "■내원 ";
                //이전 특수검진 기관
                SS1.ActiveSheet.Cells[20, 2].Text = " 이전 특수건강진단기관: " + list2.OLDHOSPITAL.Trim();
                SS1.ActiveSheet.Cells[21, 2].Text = " 이전 검진일자: " + list2.OLDFDATE + " ~ " + list2.OLDTDATE;

                nCNT1 = list2.OLDINWON1; //일특
                nCNT2 = list2.OLDINWON2; //특수
                nCNT3 = list2.OLDINWON3; //일반

                SS1.ActiveSheet.Cells[22, 2].Text = " 이전 검진실시인원: " + (nCNT1 + nCNT2 + nCNT3) + "명(일반특수:";
                SS1.ActiveSheet.Cells[22, 2].Text += nCNT1 + "명, 특수:" + nCNT2 + "명, 일반:" + nCNT3 + "명)";
                SS1.ActiveSheet.Cells[23, 2].Text = " 이전 유소견자 및 요관찰자 : 건강진단 사후관리 소견서 첨부";

                //작업환경측정
                SS1.ActiveSheet.Cells[24, 2].Text = " 최근 작업환경측정기관: " + list2.CHUKHOSPITAL;
                SS1.ActiveSheet.Cells[25, 2].Text = " 최근 측정일자: " + list2.CHUKDATE1;
                SS1.ActiveSheet.Cells[25, 6].Text = " 이전 측정일자: " + list2.CHUKDATE2;
                SS1.ActiveSheet.Cells[26, 2].Text = " 노출기준 초과여부: " + list2.CHUKNOCHUL;
                SS1.ActiveSheet.Cells[26, 6].Text = " 초과공정: " + list2.CHUKCHOGWA;
                SS1.ActiveSheet.Cells[27, 2].Text = " 주요유해인자: " + list2.CHUKYUHE;
                SS1.ActiveSheet.Cells[28, 2].Text = " " + list2.REMARK1;
                SS1.ActiveSheet.Cells[29, 2].Text = " " + list2.REMARK2;

                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;
                string strPrintName = "";

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strPrintName = clsPrint.gGetDefaultPrinter();

                strTitle = "";
                strHeader = "";
                strFooter = "";

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);

                Cursor.Current = Cursors.Default;
            }
            else if (sender == btnPrint2)
            {
                string strRemark = "";
                string strCode = "";
                string strGjYear = "";
                string strLtdCode = "";

                strGjYear = cboYear.Text;
                strLtdCode = VB.Pstr(txtCode.Text, ".", 1);

                txtCode.Text = txtCode.Text.Trim();
                if (txtCode.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("회사코드가 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                strCode = VB.Pstr(txtCode.Text, ".", 1);
                HIC_LTD list = hicLtdService.GetItembyCode(strCode);

                if (list.IsNullOrEmpty())
                {
                    MessageBox.Show("회사코드가 등록 않됨!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;

                SS2.ActiveSheet.Cells[4, 4].Text = " " + list.SANGHO;
                SS2.ActiveSheet.Cells[4, 13].Text = " " + list.JEPUMLIST;
                SS2.ActiveSheet.Cells[5, 4].Text = " " + list.JUSO + " " + list.JUSODETAIL;
                SS2.ActiveSheet.Cells[6, 4].Text = " " + list.TEL;
                SS2.ActiveSheet.Cells[6, 13].Text = " " + list.FAX;

                //사전조사 내용을 읽음
                HIC_SJ_MST list2 = hicSjMstService.GetItembyGjYearLtdCode(strGjYear, strLtdCode);

                if (list2 == null)
                {
                    MessageBox.Show("사전조사 마스타에 등록이 안됨", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                SS2.ActiveSheet.Cells[9, 4].Text = " " + list2.RFDATE + " ~ " + list2.RTDATE;

                if (list2.PLACE1 == "Y" && list2.PLACE2 == "Y")
                {
                    strRemark = "포항성모병원 건강증진센타 및 출장검진";
                    if (list2.PLACE2 == "Y")
                    {
                        if (list2.PLACE3 == "1")
                        {
                            strRemark += "(회의실)";
                        }
                        else if (list2.PLACE3 == "2")
                        {
                            strRemark += "(식당)";
                        }
                        else
                        {
                            strRemark += "(기타)";
                        }
                    }
                }
                else if (list2.PLACE1 == "Y")
                {
                    strRemark = "포항성모병원 건강증진센타";
                }
                else if (list2.PLACE2 == "Y")
                {
                    strRemark = "출장검진";
                    if (list2.PLACE2 == "Y")
                    {
                        if (list2.PLACE3 == "1")
                        {
                            strRemark += "(회의실)";
                        }
                        else if (list2.PLACE3 == "2")
                        {
                            strRemark += "(식당)";
                        }
                        else
                        {
                            strRemark += "(기타)";
                        }
                    }
                }

                SS2.ActiveSheet.Cells[10, 4].Text = " " + strRemark;
                SS2.ActiveSheet.Cells[27, 1].Text = list2.JEPNAME + "  (서명)";
                SS2.ActiveSheet.Cells[27, 1].Text = "남   복   동    (서명)";
                SS2.ActiveSheet.Cells[28, 1].Text = "신   충   곤    (서명)";
                SS2.ActiveSheet.Cells[29, 1].Text = "전   현   준    (서명)";

                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "";
                strHeader = "";
                strFooter = "";

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS2, PrePrint, setMargin, setOption, strHeader, strFooter);

                Cursor.Current = Cursors.Default;
            }
            else if (sender == btnPrint3)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ComFunc.ReadSysDate(clsDB.DbCon);

                Cursor.Current = Cursors.WaitCursor;

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "3. 건강진단 대상업무(유해인자) 요약";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("맑은 고딕", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS3, PrePrint, setMargin, setOption, strHeader, strFooter);

                Cursor.Current = Cursors.Default;
            }
            else if (sender == btnPrint4)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ComFunc.ReadSysDate(clsDB.DbCon);

                SS4_Sheet1.Columns.Get(2).Width = 140;
                SS4_Sheet1.Columns.Get(0).Visible = false;

                Cursor.Current = Cursors.WaitCursor;

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "4. 과년도 특수건강진단결과 요약";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("맑은 고딕", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS4, PrePrint, setMargin, setOption, strHeader, strFooter);

                SS4_Sheet1.Columns.Get(2).Width = 107;
                SS4_Sheet1.Columns.Get(0).Visible = true;

                Cursor.Current = Cursors.Default;
            }
            else if (sender == btnPrint5)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ComFunc.ReadSysDate(clsDB.DbCon);

                Cursor.Current = Cursors.WaitCursor;

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "5. 건강진단 대상자명단";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("맑은 고딕", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS5, PrePrint, setMargin, setOption, strHeader, strFooter);

                Cursor.Current = Cursors.Default;
            }
        }

        void etxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (txtCode.Text.Length >= 2)
                {
                    eBtnClick(btnHelp, new EventArgs());
                }
            }
        }

        /// <summary>
        /// 거래처코드 찾기 화면 스프레드더블클릭이벤트
        /// </summary>
        /// <param name="item"></param>
        private void LtdCd_value(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        void fn_Screen_Display3()
        {
            int nREAD = 0;
            int nRow = 0;
            string strUName = "";
            long nHeight = 0;
            long nTOT = 0;

            string strGjYear = "";
            long nLtdCode = 0;

            strGjYear = cboYear.Text;
            if (txtCode.Text.Trim() != "")
            {
                nLtdCode = long.Parse(VB.Pstr(txtCode.Text, ".", 1));
            }
            else
            {
                nLtdCode = 0;
            }

            sp.Spread_All_Clear(SS3);
            SS3.ActiveSheet.RowCount = 30;

            List<COMHPC> list = comHpcLibBService.GetHICsJPanoJepsubyGjYearLtdCode(strGjYear, nLtdCode);

            nREAD = list.Count;
            nRow = 0;
            nTOT = 0;
            SS3.ActiveSheet.RowCount = nREAD;
            for (int i = 0; i < nREAD; i++)
            {
                strUName = hm.UCode_Names_Display(list[i].UCODES);

                SS3.ActiveSheet.Cells[i, 0].Text = list[i].BUSE;
                SS3.ActiveSheet.Cells[i, 1].Text = strUName;
                SS3.ActiveSheet.Cells[i, 2].Text = list[i].CHUKRESULT;
                SS3.ActiveSheet.Cells[i, 3].Text = list[i].CNT.ToString();

                nTOT += list[i].CNT;
            }

            SS3.ActiveSheet.RowCount += 1;

            SS3.ActiveSheet.Cells[SS3.ActiveSheet.RowCount - 1, 2].Text = "합  계";
            SS3.ActiveSheet.Cells[SS3.ActiveSheet.RowCount - 1, 3].Text = string.Format("{0:N0}", nTOT.ToString());

            for (int i = 0; i < SS3.ActiveSheet.RowCount; i++)
            {
                if (SS3.ActiveSheet.Cells[i, 1].Text.Trim() == "")
                {
                    SS3.ActiveSheet.Rows[i].Height = 20;
                }
                else
                {
                    Size size = SS3.ActiveSheet.GetPreferredCellSize(i, 1);
                    SS3.ActiveSheet.Rows[i].Height = size.Height;
                }
            }
        }

        void fn_Screen_Display4()
        {
            int nREAD = 0;
            int nRow = 0;
            string strUName = "";
            long nHeight = 0;

            string strGjYear = "";
            long nLtdCode = 0;

            strGjYear = cboYear.Text;
            if (txtCode.Text.Trim() != "")
            {
                nLtdCode = long.Parse(VB.Pstr(txtCode.Text, ".", 1));
            }
            else
            {
                nLtdCode = 0;
            }

            sp.Spread_All_Clear(SS4);
            SS4.ActiveSheet.RowCount = 50;

            List<HIC_SJ_JINDAN> list = hicSjJindanService.GetItembyGjYearLtdCode(strGjYear, nLtdCode);

            nREAD = list.Count;
            SS4.ActiveSheet.RowCount = nREAD;
            for (int i = 0; i < nREAD; i++)
            {
                strUName = list[i].YUHE;
                SS4.ActiveSheet.Cells[i, 0].Text = "";
                SS4.ActiveSheet.Cells[i, 1].Text = list[i].PANJENG;
                SS4.ActiveSheet.Cells[i, 2].Text = list[i].BUSE;
                SS4.ActiveSheet.Cells[i, 3].Text = list[i].YUHE;
                SS4.ActiveSheet.Cells[i, 4].Text = list[i].JANGGI;
                SS4.ActiveSheet.Cells[i, 5].Text = list[i].INWON.ToString();
                SS4.ActiveSheet.Cells[i, 6].Text = list[i].RID;
                SS4.ActiveSheet.Cells[i, 7].Text = "";
            }

            Application.DoEvents();

            for (int i = 0; i < SS4.ActiveSheet.RowCount; i++)
            {
                if (SS4.ActiveSheet.Cells[i, 3].Text.Trim() == "")
                {
                    SS4.ActiveSheet.Rows[i].Height = 20;
                }
                else
                {
                    Size size = SS4.ActiveSheet.GetPreferredCellSize(i, 3);
                    SS4.ActiveSheet.Rows[i].Height = size.Height;
                }
                Application.DoEvents();
            }
            SS4.ActiveSheet.RowCount += 50;
            Application.DoEvents();
        }

        void fn_Screen_Display5()
        {
            int nREAD = 0;
            int nRow = 0;
            string strUName = "";
            long nHeight = 0;

            string strGjYear = "";
            long nLtdCode = 0;

            sp.Spread_All_Clear(SS5);
            SS5.ActiveSheet.RowCount = 50;

            strGjYear = cboYear.Text;
            if (txtCode.Text.Trim() != "")
            {
                nLtdCode = long.Parse(VB.Pstr(txtCode.Text, ".", 1));
            }
            else
            {
                nLtdCode = 0;
            }

            List<COMHPC> list = comHpcLibBService.GetItembyHicSjPanoJepsuPatient(strGjYear, nLtdCode);

            nREAD = list.Count;
            SS5.ActiveSheet.RowCount = nREAD;
            for (int i = 0; i < nREAD; i++)
            {
                strUName = hm.UCode_Names_Display(list[i].UCODES);
                SS5.ActiveSheet.Cells[i, 0].Text = list[i].BUSE;
                SS5.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                SS5.ActiveSheet.Cells[i, 2].Text = VB.Left(list[i].JUMIN, 6);
                SS5.ActiveSheet.Cells[i, 3].Text = strUName;
                SS5.ActiveSheet.Cells[i, 4].Text = list[i].CHUKRESULT;
                SS5.ActiveSheet.Cells[i, 5].Text = list[i].PANJENG;
                SS5.ActiveSheet.Cells[i, 6].Text = list[i].RID;
                SS5.ActiveSheet.Cells[i, 7].Text = "";
                Application.DoEvents();
            }

            for (int i = 0; i < SS5.ActiveSheet.RowCount; i++)
            {
                if (SS5.ActiveSheet.Cells[i, 3].Text.Trim() == "")
                {
                    SS5.ActiveSheet.Rows[i].Height = 20;
                }
                else
                {
                    Size size = SS5.ActiveSheet.GetPreferredCellSize(i, 3);
                    SS5.ActiveSheet.Rows[i].Height = size.Height;
                }
            }
        }

        void fn_Screen_Clear()
        {
            txtCode.Text = "";
            SSLtd.ActiveSheet.Cells[0, 1].Text = "";
            SSLtd.ActiveSheet.Cells[0, 3].Text = "";
            SSLtd.ActiveSheet.Cells[0, 5].Text = "";

            SSLtd.ActiveSheet.Cells[1, 1].Text = "";
            SSLtd.ActiveSheet.Cells[1, 5].Text = "";

            SSLtd.ActiveSheet.Cells[2, 1].Text = "";
            SSLtd.ActiveSheet.Cells[2, 3].Text = "";
            SSLtd.ActiveSheet.Cells[2, 5].Text = "";

            SSLtd.ActiveSheet.Cells[3, 1].Text = "";

            //사전조사 협의 내용
            rdoNewLtd1.Checked = false;
            rdoNewLtd2.Checked = false;
            rdoTel1.Checked = false;
            rdoTel2.Checked = false;

            txtInwon1.Text = "";
            txtInwon2.Text = "";
            txtInwon3.Text = "";
            txtJepDate.Text = clsPublic.GstrSysDate;
            txtJepName.Text = "";
            txtDamName.Text = "";
            txtJosaRemark.Text = "";
            txtRFDate.Text = clsPublic.GstrSysDate;
            txtRTDate.Text = clsPublic.GstrSysDate;

            chkInHosp.Checked = false;
            chkOut.Checked = false;
            rdoOut1.Checked = false;
            rdoOut2.Checked = false;
            rdoOut3.Checked = false;
            txtMDate.Text = clsPublic.GstrSysDate;
            chkMaterial1.Checked = false;
            chkMaterial2.Checked = false;
            chkMsds1.Checked = false;
            chkMsds2.Checked = false;
            chkMsds3.Checked = false;
            chkMsds4.Checked = false;
            rdoPremedicalExamAgency1.Checked = false;
            rdoPremedicalExamAgency2.Checked = false;
            rdoPremedicalExamAgency3.Checked = false;
            rdoPremedicalExamAgency4.Checked = false;
            rdoPremedicalExamAgency5.Checked = false;
            txtPremedicalExamAgency.Text = "";
            txtOldFDate.Text = clsPublic.GstrSysDate;
            txtOldTDate.Text = clsPublic.GstrSysDate;

            txtOldInwon1.Text = "";
            txtOldInwon2.Text = "";
            txtOldInwon3.Text = "";
            rdoRecentlyMeasure1.Checked = false;
            rdoRecentlyMeasure2.Checked = false;
            rdoRecentlyMeasure3.Checked = false;
            rdoRecentlyMeasure4.Checked = false;
            rdoRecentlyMeasure5.Checked = false;
            txtRecentlyMeasureEtc.Text = "";
            txtChukDate1.Text = clsPublic.GstrSysDate;
            txtChukDate2.Text = clsPublic.GstrSysDate;
            txtChukNochul.Text = "";
            txtChukChogwa.Text = "";
            txtChukYuhe.Text = "";
            txtRemark1.Text = "";
            txtRemark2.Text = "";

            btnListView.Enabled = true;
            btnSave.Enabled = false;
            btnDelete.Enabled = false;
            btnPrint.Enabled = false;
            btnPrint2.Enabled = false;
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == ssList)
            {
                fn_Screen_Clear();
                txtCode.Text = ssList.ActiveSheet.Cells[e.Row, 1].Text.Trim() + "." + hb.READ_Ltd_Name(ssList.ActiveSheet.Cells[e.Row, 1].Text.Trim());                 
                eBtnClick(btnSearch, new EventArgs());
            }
        }

        void eSpdChange(object sender, ChangeEventArgs e)
        {
            if (sender == SS4)
            {
                SS4.ActiveSheet.Cells[e.Row, 7].Text = "Y";
            }
            else if (sender == SS5)
            {
                SS5.ActiveSheet.Cells[e.Row, 7].Text = "Y";
            }
        }

        void eRdoClick(object sender, EventArgs e)
        {
            if (rdoRecentlyMeasure6.Checked == true)
            {
                txtChukDate1.Text = "";
                txtChukDate2.Text = "";
                txtChukNochul.Text = "";
                txtChukChogwa.Text = "";
                txtChukYuhe.Text = "";
            }
        }

        void etxtDateDblClick(object sender, EventArgs e)
        {
            if (!((TextBox)sender).Text.IsNullOrEmpty())
            {
                clsPublic.GstrCalDate = ((TextBox)sender).Text;
            }

            frmCalendar frmCalendarX = new frmCalendar();
            frmCalendarX.StartPosition = FormStartPosition.CenterParent;
            frmCalendarX.ShowDialog();

            ((TextBox)sender).Text = clsPublic.GstrCalDate;
        }
    }
}
