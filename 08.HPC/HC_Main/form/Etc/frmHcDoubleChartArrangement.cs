using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHcDoubleChartArrangement.cs
/// Description     : 일반건진 이중챠트 정리작업
/// Author          : 이상훈
/// Create Date     : 2020-06-26
/// Update History  : 
/// </summary>
/// <seealso cref= "Frm일반건진이중챠트정리(Frm일반건진이중챠트정리.frm)" />

namespace HC_Main
{
    public partial class frmHcDoubleChartArrangement : Form
    {
        HicPatientService hicPatientService = null;
        HicJepsuService hicJepsuService = null;
        HicJepsuWorkService hicJepsuWorkService = null;
        HicSunapWorkService hicSunapWorkService = null;
        HicSunapdtlWorkService hicSunapdtlWorkService = null;
        HicSunapService hicSunapService = null;
        HicSangdamNewService hicSangdamNewService = null;
        HicTitemService hicTitemService = null;
        HicXrayResultService hicXrayResultService = null;
        ComHpcLibBService comHpcLibBService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();

        string FstrJumin;
        int result;
        string strOK;

        public frmHcDoubleChartArrangement()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            hicPatientService = new HicPatientService();
            hicJepsuService = new HicJepsuService();
            hicJepsuWorkService = new HicJepsuWorkService();
            hicSunapWorkService = new HicSunapWorkService();
            hicSunapdtlWorkService = new HicSunapdtlWorkService();
            hicSunapService = new HicSunapService();
            hicSangdamNewService = new HicSangdamNewService();
            hicTitemService = new HicTitemService();
            hicXrayResultService = new HicXrayResultService();
            comHpcLibBService = new ComHpcLibBService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnOK.Click += new EventHandler(eBtnClick);
            this.SS1.CellClick += new CellClickEventHandler(eSpdClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            sp.Spread_All_Clear(ssList);
            sp.Spread_All_Clear(SS1);
            sp.Spread_All_Clear(SS2);

            btnJob1.Enabled = false;

            for (int i = 0; i <= 8; i++)
            {
                CheckBox chkJob = (Controls.Find("chkJob" + (i).ToString(), true)[0] as CheckBox);
                chkJob.Checked = false;
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnMenu1)
            {
                int nREAD = 0;
                long nPano = 0;

                List<HIC_PATIENT> list = hicPatientService.GetDoubleChartSearch();

                nREAD = list.Count;

                if (nREAD == 0)
                {
                    MessageBox.Show("이중차트가 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                progressBar1.Maximum = nREAD;

                if (nREAD > 0)
                {
                    clsDB.setBeginTran(clsDB.DbCon);
                    for (int i = 0; i < nREAD; i++)
                    {
                        nPano = list[i].PANO;
                        //접수마스타가 없으면 이중차트 별도 보관함
                        if (hicJepsuService.GetCountbyPaNo(nPano) == 0)
                        {
                            result = hicPatientService.InsertHicPatientOverLap(nPano);

                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                MessageBox.Show("이중챠트 별도 보관 작업 중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            result = hicPatientService.DeletebyPano(nPano);

                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                MessageBox.Show("이중챠트 삭제 작업 중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        progressBar1.Value = i + 1;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);

                    MessageBox.Show("작업 완료!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (sender == btnJob1)
            {
                long nPano = 0;
                string strJumin = "";

                for (int i = 0; i < ssList.ActiveSheet.RowCount; i++)
                {
                    strJumin = ssList.ActiveSheet.Cells[i, 0].Text.Replace("-", "");

                    List<HIC_PATIENT> list = hicPatientService.GetDblChartbyJumin2(clsAES.AES(strJumin));
                    SS1.ActiveSheet.RowCount = list.Count;
                    if (list.Count > 0)
                    {
                        for (int j = 0; j < list.Count; j++)
                        {
                            nPano = list[j].PANO;
                            if (hicJepsuService.GetCountbyPaNo(nPano) == 0)
                            {
                                clsDB.setBeginTran(clsDB.DbCon);
                                result = hicPatientService.UpdatebySNamePaNo("이중챠트", nPano);

                                if (result < 0)
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    MessageBox.Show("HIC_PATIENT 마스터 정리중에 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                                clsDB.setCommitTran(clsDB.DbCon);
                            }
                        }
                    }
                }
                btnJob1.Enabled = false;
            }
            else if (sender == btnOK)
            {
                string strName = "";
                string strPANO = "";
                string strTemp = "";

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    if (SS1.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        strPANO = SS1.ActiveSheet.Cells[i, 1].Text;
                        strName = SS1.ActiveSheet.Cells[i, 2].Text;
                        break;
                    }
                }

                if (strPANO.IsNullOrEmpty() || FstrJumin.IsNullOrEmpty())
                {
                    return;
                }

                strTemp = "성명" + strName + "\r\n";
                if (MessageBox.Show(strTemp + strPANO + " 번호로 확정하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                for (int i = 0; i <= 8; i++)
                {
                    CheckBox chkJob = (Controls.Find("chkJob" + (i).ToString(), true)[0] as CheckBox);
                    chkJob.Checked = false;
                }

                clsDB.setBeginTran(clsDB.DbCon);

                for (int i = 0; i <= 8; i++)
                {
                    switch (i)
                    {
                        case 0:
                            fn_UPDATE_HIC_JEPSU_WORK(strPANO, clsAES.AES(FstrJumin));
                            break;
                        case 1:
                            fn_UPDATE_HIC_SUNAP_WORK(strPANO, clsAES.AES(FstrJumin));
                            break;
                        case 2:
                            fn_UPDATE_HIC_SUNAPDTL_WORK(strPANO, clsAES.AES(FstrJumin));
                            break;
                        case 3:
                            fn_UPDATE_HIC_JEPSU(strPANO, clsAES.AES(FstrJumin));
                            break;
                        case 4:
                            fn_UPDATE_HIC_SUNAP(strPANO, clsAES.AES(FstrJumin));
                            break;
                        case 5:
                            fn_UPDATE_HIC_SANGDAM_NEW(strPANO, clsAES.AES(FstrJumin));
                            break;
                        case 6:
                            fn_UPDATE_HIC_TITEM(strPANO, clsAES.AES(FstrJumin));
                            break;
                        case 7:
                            fn_UPDATE_HIC_XRAY_RESULT(strPANO, clsAES.AES(FstrJumin));
                            break;
                        case 8:
                            fn_UPDATE_HIC_PATIENT(strPANO, clsAES.AES(FstrJumin));
                            break;
                        default:
                            break;
                    }

                    CheckBox chkJob = (Controls.Find("chkJob" + (i).ToString(), true)[0] as CheckBox);
                    chkJob.Checked = true;

                    if (strOK == "NO")
                    {
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                MessageBox.Show("이중챠트 정리 완료!! 환자마스터 및 접수내역을 확인해보시길 바랍니다.", "작업완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (sender == btnSearch)
            {
                int nREAD = 0;
                int nRow = 0;
                string strJumin = "";

                sp.Spread_All_Clear(ssList);
                sp.Spread_All_Clear(SS1);
                sp.Spread_All_Clear(SS2);

                List<HIC_PATIENT> list = hicPatientService.GetDblCharSearch();

                nREAD = list.Count;

                if (nREAD == 0)
                {
                    MessageBox.Show("이중차트가 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (nREAD > 300)
                {
                    if (MessageBox.Show("조회건수가 " + nREAD + "건 입니다. 300건만 조회하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        nREAD = 300;
                    }
                }

                ssList.ActiveSheet.RowCount = nREAD;

                progressBar1.Maximum = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    strJumin = clsAES.DeAES(list[i].JUMIN2);
                    ssList.ActiveSheet.Cells[i, 0].Text = VB.Left(strJumin, 6) + "-" + VB.Mid(strJumin, 7, 7);
                    ssList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                    ssList.ActiveSheet.Cells[i, 2].Text = list[i].CNT.ToString();
                    progressBar1.Value = i + 1;
                }

                btnJob1.Enabled = true;
            }
        }

        void fn_UPDATE_HIC_JEPSU_WORK(string argPaNo, string strJumin2)
        {
            result = hicJepsuWorkService.UpdatePaNobyPaNo(argPaNo, strJumin2);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("HIC_JEPSU_WORK 정리중에 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                strOK = "NO";
            }
        }

        void fn_UPDATE_HIC_SUNAP_WORK(string argPaNo, string strJumin2)
        {
            result = hicSunapWorkService.UpdatePaNobyPaNo(argPaNo, strJumin2);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("HIC_SUNAP_WORK 정리중에 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                strOK = "NO";
            }
        }

        void fn_UPDATE_HIC_SUNAPDTL_WORK(string argPaNo, string strJumin2)
        {
            result = hicSunapdtlWorkService.UpdatePaNobyPaNo(argPaNo, strJumin2);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("HIC_SUNAPDTL_WORK 정리중에 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                strOK = "NO";
            }
        }

        void fn_UPDATE_HIC_JEPSU(string argPaNo, string strJumin2)
        {
            result = hicJepsuService.UpdatePaNobyPaNo(argPaNo, strJumin2);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("HIC_JEPSU 정리중에 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                strOK = "NO";
            }
        }
        
        void fn_UPDATE_HIC_SUNAP(string argPaNo, string strJumin2)
        {
            result = hicSunapService.UpdatePaNobyPaNo(argPaNo, strJumin2);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("HIC_SUNAP 정리중에 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                strOK = "NO";
            }
        }

        void fn_UPDATE_HIC_SANGDAM_NEW(string argPaNo, string strJumin2)
        {
            result = hicSangdamNewService.UpdatePaNobyPaNo(argPaNo, strJumin2);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("HIC_SANGDAM_NEW 정리중에 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                strOK = "NO";
            }
        }

        void fn_UPDATE_HIC_TITEM(string argPaNo, string strJumin2)
        {
            result = hicTitemService.UpdatePaNobyPaNo(argPaNo, strJumin2);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("HIC_TITEM 정리중에 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                strOK = "NO";
            }
        }

        void fn_UPDATE_HIC_XRAY_RESULT(string argPaNo, string strJumin2)
        {
            result = hicXrayResultService.UpdatePaNobyPaNo(argPaNo, strJumin2);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("HIC_XRAY_RESULT 정리중에 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                strOK = "NO";
            }
        }

        void fn_UPDATE_HIC_PATIENT(string argPaNo, string strJumin2)
        {
            result = hicPatientService.UpdatePaNobyPaNo(argPaNo, strJumin2);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("HIC_PATIENT 정리중에 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                strOK = "NO";
            }
        }

        void eSpdClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                SS1.ActiveSheet.Cells[0, 0, SS1.ActiveSheet.RowCount - 1, SS1.ActiveSheet.ColumnCount - 1].BackColor = Color.White;
                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    if (e.Row != i)
                    {
                        SS1.ActiveSheet.Cells[i, 0].Text = "";
                    }
                }

                if (SS1.ActiveSheet.Cells[e.Row, 0].Text == "True")
                {
                    SS1.ActiveSheet.Cells[e.Row, 0].Text = "";
                    SS1.ActiveSheet.Cells[e.Row, 0, e.Row, SS1.ActiveSheet.ColumnCount - 1].BackColor = Color.White;
                }
                else
                {
                    SS1.ActiveSheet.Cells[e.Row, 0].Text = "True";
                    SS1.ActiveSheet.Cells[e.Row, 0, e.Row, SS1.ActiveSheet.ColumnCount - 1].BackColor = Color.Yellow;
                }
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                long nPano = 0;
                int nRead = 0;

                sp.Spread_All_Clear(SS2);

                nPano = long.Parse(SS1.ActiveSheet.Cells[e.Row, 1].Text);

                List<HIC_JEPSU> list = hicJepsuService.GetDetailbyPaNo(nPano);

                nRead = list.Count;
                SS2.ActiveSheet.RowCount = nRead;
                if (nRead > 0)
                {
                    for (int i = 0; i < nRead; i++)
                    {
                        SS2.ActiveSheet.Cells[i, 0].Text = list[i].JEPDATE;
                        SS2.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                        SS2.ActiveSheet.Cells[i, 2].Text = list[i].AGE + "/" + list[i].SEX;
                        SS2.ActiveSheet.Cells[i, 3].Text = list[i].GJJONG;
                        switch (list[i].GBSTS)
                        {
                            case "1":
                                SS2.ActiveSheet.Cells[i, 4].Text = "수검중";
                                break;
                            case "2":
                                SS2.ActiveSheet.Cells[i, 4].Text = "수검완료";
                                break;
                            case "D":
                                SS2.ActiveSheet.Cells[i, 4].Text = "취소";
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            else if (sender == ssList)
            {
                string strJumin = "";
                long nPano = 0;
                int nRead = 0;

                ssList.ActiveSheet.Cells[0, 0, ssList.ActiveSheet.RowCount - 1, ssList.ActiveSheet.ColumnCount - 1].BackColor = Color.White;

                if (ssList.ActiveSheet.Cells[e.Row, 0, e.Row, ssList.ActiveSheet.ColumnCount - 1].BackColor == Color.Yellow)
                {
                    ssList.ActiveSheet.Cells[e.Row, 0, e.Row, ssList.ActiveSheet.ColumnCount - 1].BackColor = Color.White;
                }
                else
                {
                    ssList.ActiveSheet.Cells[e.Row, 0, e.Row, ssList.ActiveSheet.ColumnCount - 1].BackColor = Color.Yellow;
                }

                sp.Spread_All_Clear(SS1);
                sp.Spread_All_Clear(SS2);

                strJumin = ssList.ActiveSheet.Cells[e.Row, 0].Text.Replace("-", "");
                FstrJumin = strJumin;

                List<HIC_PATIENT> list = hicPatientService.GetDblChartbyJumin2(clsAES.AES(strJumin));

                nRead = list.Count;
                SS1.ActiveSheet.RowCount = nRead;
                if (nRead > 0)
                {
                    for (int i = 0; i < nRead; i++)
                    {
                        SS1.ActiveSheet.Cells[i, 0].Text = "";
                        SS1.ActiveSheet.Cells[i, 1].Text = list[i].PANO.ToString();
                        SS1.ActiveSheet.Cells[i, 2].Text = list[i].SNAME;
                        SS1.ActiveSheet.Cells[i, 3].Text = list[i].PTNO;
                    }
                }

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    nPano = long.Parse(SS1.ActiveSheet.Cells[i, 1].Text);
                    SS1.ActiveSheet.Cells[i, 4].Text = hicJepsuService.GetCountbyPaNo(nPano).ToString();
                }
            }
        }
    }
}
