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
/// File Name       : frmHaEndoWaitSeq.cs
/// Description     : 종검내시경실대기순번
/// Author          : 이상훈
/// Create Date     : 2019-08-27
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm종검내시경실대기순번.frm(Frm종검내시경실대기순번)" />

namespace HC_Act
{
    public partial class frmHaEndoWaitSeq : Form
    {
        HicJepsuResultService hicJepsuResultService = null;
        HeaSangdamWaitService heaSangdamWaitService = null;
        HicResultService hicResultService = null;
        HicXrayResultService hicXrayResultService = null;
        HeaJepsuService heaJepsuService = null;
        HeaSangdamHisService heaSangdamHisService = null;
        HicJepsuService hicJepsuService = null;
        HeaResultService heaResultService = null;
        HicDoctorService hicDoctorService = null;

        ComHpcLibBService comHpcLibBService = null;
        EndoJupmstService endoJupmstService = null;
        HeaResvExamPatientService heaResvExamPatientService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();

        long FnMAX_WaitNo;
        const string ENDO_ROOM = "09";  //대기순번 내시경실 번호

        public frmHaEndoWaitSeq()
        {
            InitializeComponent();

            SetEvent();
        }

        void SetEvent()
        {
            hicJepsuResultService = new HicJepsuResultService();
            heaSangdamWaitService = new HeaSangdamWaitService();
            hicResultService = new HicResultService();
            hicXrayResultService = new HicXrayResultService();
            heaJepsuService = new HeaJepsuService();
            heaSangdamHisService = new HeaSangdamHisService();
            hicJepsuService = new HicJepsuService();
            heaResultService = new HeaResultService();
            heaResvExamPatientService = new HeaResvExamPatientService();
            hicDoctorService = new HicDoctorService();

            comHpcLibBService = new ComHpcLibBService();
            endoJupmstService = new EndoJupmstService();

            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSeqChange.Click += new EventHandler(eBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.SS2.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.chkAutoCompletion.Click += new EventHandler(eCheckBoxClick);
            this.timerAutoCompletion.Tick += new EventHandler(etimerAutoCompletionTick);
            this.chkNoResultInput.CheckedChanged += new EventHandler(eChkChanged);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            fn_Form_Load();

            clsCompuInfo.SetComputerInfo();

            if (chkAutoCompletion.Checked == true)
            {
                timerAutoCompletion.Enabled = true;
            }
        }

        void fn_Form_Load()
        {
            sp.Spread_All_Clear(SS1);
            sp.Spread_All_Clear(SS2);

            SS1.ActiveSheet.ColumnHeader.Cells[0, 0, 0, SS1.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;

            eBtnClick(btnSearch, new EventArgs());
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
                int nREAD = 0;
                int nRow = 0;
                string strList = "";
                List<string> lstInstr = new List<string>();
                string strOK = "";
                string strTemp = "";

                fn_Display_Wait_List();

                strList = "";
                for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
                {
                    lstInstr.Add(SS2.ActiveSheet.Cells[i, 3].Text);
                }

                strTemp = chkNoResultInput.Checked == true ? "Y" : "N";

                List<ENDO_JUPMST> list = endoJupmstService.GetItembyPtNo(lstInstr, strTemp);

                nREAD = list.Count;
                SS1.ActiveSheet.RowCount = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    strOK = "OK";
                    if (list[i].DEPTCODE.Trim() == "TO")
                    {
                        if (heaJepsuService.GetEndoGbnbyPtNo(list[i].PTNO) == "2")
                        {
                            strOK = "";
                        }
                    }

                    //2020-04-01(명단오류관련 추가)
                    if (list[i].DEPTCODE.Trim() == "TO")
                    {
                        HEA_RESV_EXAM_PATIENT item = heaResvExamPatientService.GetItembyPtnoExam(list[i].PTNO, "02");

                        if (!item.IsNullOrEmpty())
                        {
                            strOK = "";
                        }
                    }

                    if (strOK == "OK")
                    {
                        nRow += 1;
                        SS1.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].SNAME.Trim();
                        SS1.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].PTNO.Trim();
                        SS1.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].DEPTCODE.Trim();

                        if (list[i].DEPTCODE.Trim() == "TO")
                        {
                            if (heaResultService.GetCountbyPtNo(list[i].PTNO.Trim()) > 0)
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 3].Text = "수면";
                                //SS1.ActiveSheet.Cells[nRow - 1, 0].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFC0C0"));
                            }

                            else
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 3].Text = "";
                                //SS1.ActiveSheet.Cells[nRow - 1, 0].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                            }
                        }
                        else
                        {
                            if (hicResultService.GetCountbyPtNo(list[i].PTNO.Trim()) > 0)
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 3].Text = "수면";
                                //SS1.ActiveSheet.Cells[nRow - 1, 0].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFC0C0"));
                            }

                            else
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 3].Text = "";
                                //SS1.ActiveSheet.Cells[nRow - 1, 0].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                            }
                        }

                        if (chkNoResultInput.Checked == false)
                        {
                            ENDO_JUPMST list2 = endoJupmstService.GetResultDatebyPtNo(list[i].PTNO.Trim());

                            if (!list2.IsNullOrEmpty())
                            {
                                if (list2.RESULTDATE.To<string>().IsNullOrEmpty())
                                {
                                    SS1.ActiveSheet.Cells[nRow - 1, 4].Text = "";
                                }
                                else
                                {
                                    //SS1.ActiveSheet.Cells[nRow - 1, 4].Text = "완료";
                                    ENDO_JUPMST list3 = endoJupmstService.GetResultDrcodebyPtNo(list[i].PTNO.Trim());
                                    HIC_DOCTOR item = hicDoctorService.GetIDrNameLicencebyDrSabun(list3.RESULTDRCODE);
                                    if (!item.IsNullOrEmpty())
                                    {
                                        SS1.ActiveSheet.Cells[nRow - 1, 4].Text = item.DRNAME;
                                    }


                                }
                            }
                            else
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 4].Text = "";
                            }
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 4].Text = "";
                        }
                    }
                }
                SS1.ActiveSheet.RowCount = nRow;
            }
            else if (sender == btnSeqChange)
            {
                long nWait = 0;
                string strRowId = "";

                if (MessageBox.Show("순서변경을 하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                clsDB.setBeginTran(clsDB.DbCon);

                for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
                {
                    nWait = SS2.ActiveSheet.Cells[i, 0].Text.To<long>();
                    strRowId = SS2.ActiveSheet.Cells[i, 4].Text;

                    int result = heaSangdamWaitService.UpdateWaitNo(nWait, strRowId);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("상담 대기순번 변경시 오류발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                fn_Display_Wait_List();
            }
        }

        void eChkChanged(object sender, EventArgs e)
        {
            if (sender == chkNoResultInput)
            {
                eBtnClick(btnSearch, new EventArgs());
            }
        }

        void fn_Display_Wait_List()
        {
            int nRead = 0;

            FnMAX_WaitNo = 0;

            List<HEA_SANGDAM_WAIT> list = heaSangdamWaitService.GetItembyRoomCd(ENDO_ROOM);

            nRead = list.Count;
            SS2.ActiveSheet.RowCount = nRead;
            for (int i = 0; i < nRead; i++)
            {
                SS2.ActiveSheet.Cells[i, 0].Text = ((i + 1) * 10).To<string>();
                SS2.ActiveSheet.Cells[i, 1].Text = list[i].SNAME.Trim();
                SS2.ActiveSheet.Cells[i, 2].Text = list[i].ENTTIME;
                SS2.ActiveSheet.Cells[i, 3].Text = string.Format("{0:00000000}", list[i].WRTNO);
                SS2.ActiveSheet.Cells[i, 4].Text = list[i].RID;
                FnMAX_WaitNo = list[i].WAITNO;
            }
        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            long nAge = 0;
            long nWRTNO = 0;
            long nWait = 0;
            string strPart = "";
            string strSName = "";
            string strSex = "";
            string strGjJong = "";
            string strRoom = "";
            string strPtno = "";
            string strGbEndo = "";

            FpSpread o = (FpSpread)sender;
            ComFunc.ReadSysDate(clsDB.DbCon);

            if (sender == this.SS1)
            {
                if (e.ColumnHeader == true || e.RowHeader == true) return;

                strSName = SS1.ActiveSheet.Cells[e.Row, 0].Text.Trim();
                strPtno = SS1.ActiveSheet.Cells[e.Row, 1].Text.Trim();
                strGbEndo = SS1.ActiveSheet.Cells[e.Row, 3].Text.Trim();

                strSex = "";
                nAge = 0;

                HEA_JEPSU lst1 = heaJepsuService.GetSexAgebyPtNo(strPtno);

                if (!lst1.IsNullOrEmpty())
                {
                    strSex = lst1.SEX.Trim();
                    nAge = lst1.AGE;
                }

                if (strSex.IsNullOrEmpty())
                {
                    HIC_JEPSU lst2 = hicJepsuService.GetSexAgebyPtNo(strPtno);

                    if (!lst2.IsNullOrEmpty())
                    {
                        strSex = lst2.SEX.Trim();
                        nAge = lst2.AGE;
                    }
                }

                FnMAX_WaitNo += 1;
                if (!strGbEndo.IsNullOrEmpty())
                {
                    strGbEndo = "Y";
                }

                HEA_SANGDAM_WAIT lstwait = heaSangdamWaitService.GetCountWaitbyEndoRoom(strPtno, ENDO_ROOM);

                HEA_SANGDAM_WAIT item = new HEA_SANGDAM_WAIT();

                item.WRTNO = strPtno.To<long>();
                item.SNAME = strSName;
                item.SEX = strSex;
                item.AGE = nAge;
                item.WAITNO = FnMAX_WaitNo;
                item.GBENDO = strGbEndo;
                item.GBCALL = "";
                item.CALLTIME = "";
                item.GUBUN = ENDO_ROOM;
                item.GJJONG = "";

                if (!lstwait.IsNullOrEmpty())
                {
                    item.RID = lstwait.RID;
                }
                else
                {
                    item.RID = "";
                }

                clsDB.setBeginTran(clsDB.DbCon);

                if (lstwait.IsNullOrEmpty())
                {
                    int result = heaSangdamWaitService.InsertSangdamWait_GbEndo(item);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("상담대기 순번등록 중 오류 발생", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    int result = heaSangdamWaitService.UpdateSangdam_GbEndo(item);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("상담대기 순번등록 중 오류 발생", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                eBtnClick(btnSearch, new EventArgs());
            }
            else if (sender == this.SS2)
            {
                string strRowId = "";

                if (e.ColumnHeader == true || e.Column != 1) return;

                strSName = SS2.ActiveSheet.Cells[e.Row, 1].Text.Trim();
                strRowId = SS2.ActiveSheet.Cells[e.Row, 4].Text.Trim();

                clsDB.setBeginTran(clsDB.DbCon);

                int result = heaSangdamWaitService.Update_CallTimeGbCall(strRowId);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("상담대기 완료 처리중 오류발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    eBtnClick(btnSearch, new EventArgs());
                    return;
                }
                clsDB.setCommitTran(clsDB.DbCon);
                eBtnClick(btnSearch, new EventArgs());
            }
        }

        void eCheckBoxClick(object sender, EventArgs e)
        {
            if (sender == chkAutoCompletion)
            {
                eBtnClick(btnSearch, new EventArgs());

                if (chkAutoCompletion.Checked == true)
                {
                    timerAutoCompletion.Enabled = true;
                }
                else
                {
                    timerAutoCompletion.Enabled = false;
                }
            }
            else if (sender == chkNoResultInput)
            {
                eBtnClick(btnSearch, new EventArgs());
            }
        }

        private void etimerAutoCompletionTick(object sender, EventArgs e)
        {
            ///TODO : 이상훈(2019-08-28) hea_sangdam_wait WRTNO 를 endo_jupmst 의 PTNO 칼럼 대입??? 확인필요

            int nRead = 0;
            string strPtNo = "";
            string strRowId = "";
            string strChange = "";

            if (chkAutoCompletion.Checked == false) return;

            timerAutoCompletion.Enabled = false;

            List<HEA_SANGDAM_WAIT> list = heaSangdamWaitService.GetItembyRoomCd(ENDO_ROOM);

            nRead = list.Count;
            SS2.ActiveSheet.RowCount = nRead;
            for (int i = 0; i < nRead; i++)
            {
                strPtNo = string.Format("{0:00000000}", list[i].WRTNO);
                strRowId = list[i].RID.Trim();

                ENDO_JUPMST lst = endoJupmstService.GetResultDatebyPtNo(strPtNo);

                if (!lst.IsNullOrEmpty())
                {
                    int result = heaSangdamWaitService.Update_CallTimeGbCall(strRowId);

                    if (result < 0)
                    {
                        MessageBox.Show("자동완료 처리중 오류 발생!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    strChange = "Y";
                }
            }

            if (strChange == "Y")
            {
                fn_Display_Wait_List();
            }

            timerAutoCompletion.Enabled = true;
        }
    }
}
