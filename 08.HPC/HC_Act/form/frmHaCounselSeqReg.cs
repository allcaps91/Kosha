using ComBase;
using ComBase.Mvc;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;
using ComBase.Controls;
using FarPoint.Win.Spread;
using System.Drawing;
using System.ComponentModel;

/// <summary>
/// Class Name      : HC_Act
/// File Name       : frmHaCounselSeqReg.cs
/// Description     : 종검상담 대기순번 등록
/// Author          : 이상훈
/// Create Date     : 2019-07
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmAct06.frm(Frm종검상담순번등록)" />

namespace HC_Act
{
    public partial class frmHaCounselSeqReg : Form
    {
        HicJepsuResultService hicJepsuResultService = null;
        HeaSangdamWaitService heaSangdamWaitService = null;
        HicResultService hicResultService = null;
        HicXrayResultService hicXrayResultService = null;
        HeaJepsuService heaJepsuService = null;
        HeaSangdamHisService heaSangdamHisService = null;
        HeaJepsuResultService heaJepsuResultService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();

        long FnWRTNO;
        const string ENDO_ROOM = "12";  //대기순번 내시경실 번호

        public frmHaCounselSeqReg()
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
            heaJepsuResultService = new HeaJepsuResultService();

            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.SS1.CellClick += new CellClickEventHandler(eSpreadClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.SS2.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.SS3.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.SS4.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.SS5.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            fn_Form_Load();

            clsCompuInfo.SetComputerInfo();
        }

        void fn_Form_Load()
        {
            sp.Spread_All_Clear(SS1);
            sp.Spread_All_Clear(SS2);
            sp.Spread_All_Clear(SS3);
            sp.Spread_All_Clear(SS4);
            sp.Spread_All_Clear(SS5);

            SS1_Sheet1.Columns.Get(1).Visible = false;

            dtpSDate.Text = clsPublic.GstrSysDate;

            eBtnClick(btnSearch, new EventArgs());

            pnlJepsu.Visible = false;
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
                long nWrtNo = 0;
                string strTemp = "";
                string strPtno = "";
                string strOK = "";
                string strWrtNo = "";
                int nCurRow = 0;
                string strJepDate = "";
                string strFrDate = "";
                string strToDate = "";

                sp.Spread_All_Clear(SS1);
                sp.Spread_All_Clear(SS2);
                sp.Spread_All_Clear(SS3);
                sp.Spread_All_Clear(SS4);
                sp.Spread_All_Clear(SS5);

                strJepDate = dtpSDate.Text;
                strFrDate = clsPublic.GstrSysDate;
                strToDate = DateTime.Parse(clsPublic.GstrSysDate).AddDays(1).ToShortDateString();

                //접수순번 명단 표시
                List<HIC_JEPSU_RESULT> lst = hicJepsuResultService.GetHeaItembySDate(strJepDate);

                nRow = 0;

                nREAD = lst.Count;
                for (int i = 0; i < nREAD; i++)
                {
                    strOK = "OK";
                    nWrtNo = lst[i].WRTNO;

                    //HEA_SANGDAM_WAIT wait = heaSangdamWaitService.GetGBCallbyWrtNo(nWrtNo, clsPublic.GstrSysDate, Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(1).ToShortDateString(), ENDO_ROOM);
                    HEA_SANGDAM_WAIT wait = heaSangdamWaitService.GetGbCallbyWrtNoEntTime(nWrtNo, strFrDate, strToDate);

                    if (!wait.IsNullOrEmpty())
                    {
                        if (wait.GBCALL == "Y")
                        {
                            strOK = "OK";
                        }
                        else
                        {
                            strOK = "";
                        }
                    }

                    if (strOK == "OK")
                    {
                        nRow += 1;
                        if (SS1.ActiveSheet.RowCount < nRow)
                        {
                            SS1.ActiveSheet.RowCount = nRow;
                        }

                        if (!wait.IsNullOrEmpty())
                        {
                            if (wait.GBCALL == "Y")
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 0].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFC0C0"));
                            }
                        }

                        SS1.ActiveSheet.Cells[nRow - 1, 0].Text = lst[i].PANO.To<string>();
                        SS1.ActiveSheet.Cells[nRow - 1, 1].Text = lst[i].WRTNO.To<string>();
                        SS1.ActiveSheet.Cells[nRow - 1, 2].Text = lst[i].SNAME;
                        SS1.ActiveSheet.Cells[nRow - 1, 3].Text = lst[i].AMPM2 == "2" ? "오후" : "";
                        SS1.ActiveSheet.Cells[nRow - 1, 4].Text = lst[i].ENTTIME;

                        if (lst[i].LTDCODE != 0)
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 5].Text = hb.READ_Ltd_Name(lst[i].LTDCODE.To<string>());
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 5].Text = "개인종검";
                        }
                        SS1.ActiveSheet.Cells[nRow - 1, 6].Text = lst[i].GBDAILY == "Y" ? "Y" : "";
                        SS1.ActiveSheet.Cells[nRow - 1, 7].Text = lst[i].EXAMCHANGE;
                        SS1.ActiveSheet.Cells[nRow - 1, 9].Text = lst[i].FAMILLY;
                        SS1.ActiveSheet.Cells[nRow - 1, 10].Text = "";
                    }
                }

                nRow = 0;
                //해당방에 등록되어 있는지 확인
                List<HEA_SANGDAM_WAIT> lstSdWait = heaSangdamWaitService.GetSangdamWaitInfobyEndoRoom("12", clsPublic.GstrSysDate);

                if (lstSdWait.Count > 0)
                {
                    for (int i = 0; i < lstSdWait.Count; i++)
                    {
                        nWrtNo = lstSdWait[i].WRTNO;
                        if (heaJepsuService.GetCountbyWrtNo(nWrtNo) > 0)
                        {
                            nRow += 1;
                            if (SS2.ActiveSheet.RowCount < nRow)
                            {
                                SS2.ActiveSheet.RowCount = nRow;
                            }
                            SS2.ActiveSheet.Cells[nRow - 1, 0].Text = lstSdWait[i].SNAME;
                            SS2.ActiveSheet.Cells[nRow - 1, 1].Text = lstSdWait[i].ENTTIME;
                            SS2.ActiveSheet.Cells[nRow - 1, 2].Text = lstSdWait[i].WRTNO.To<string>();
                        }
                    }
                }

                List<HEA_SANGDAM_WAIT> lstSdWait2 = heaSangdamWaitService.GetSangdamWaitInfobyEndoRoom("13", clsPublic.GstrSysDate);

                nRow = 0;
                if (lstSdWait2.Count > 0)
                {   
                    for (int i = 0; i < lstSdWait2.Count; i++)
                    {
                        nWrtNo = lstSdWait2[i].WRTNO;
                        if (heaJepsuService.GetCountbyWrtNo(nWrtNo) > 0)
                        {
                            nRow += 1;
                            if (SS4.ActiveSheet.RowCount < nRow)
                            {
                                SS4.ActiveSheet.RowCount = nRow;
                            }
                            SS4.ActiveSheet.Cells[nRow - 1, 0].Text = lstSdWait2[i].SNAME;
                            SS4.ActiveSheet.Cells[nRow - 1, 1].Text = lstSdWait2[i].ENTTIME;
                            SS4.ActiveSheet.Cells[nRow - 1, 2].Text = lstSdWait2[i].WRTNO.To<string>();
                        }
                    }                    
                }



                List<HEA_SANGDAM_WAIT> lstSdWait3 = heaSangdamWaitService.GetSangdamWaitInfobyEndoRoom("'14'", clsPublic.GstrSysDate);

                nRow = 0;
                if (lstSdWait3.Count > 0)
                {   
                    for (int i = 0; i < lstSdWait3.Count; i++)
                    {
                        nWrtNo = lstSdWait3[i].WRTNO;
                        if (heaJepsuService.GetCountbyWrtNo(nWrtNo) > 0)
                        {
                            nRow += 1;
                            if (SS5.ActiveSheet.RowCount < nRow)
                            {
                                SS5.ActiveSheet.RowCount = nRow;
                            }
                            SS5.ActiveSheet.Cells[nRow - 1, 0].Text = lstSdWait3[i].SNAME;
                            SS5.ActiveSheet.Cells[nRow - 1, 1].Text = lstSdWait3[i].ENTTIME;
                            SS5.ActiveSheet.Cells[nRow - 1, 2].Text = lstSdWait3[i].WRTNO.To<string>();
                        }
                    }
                }
                pnlJepsu.Visible = false;

                Application.DoEvents();

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strWrtNo = "";
                    strOK = "";
                    nCurRow = i;
                    strWrtNo = SS1.ActiveSheet.Cells[nCurRow, 9].Text.Trim();
                    strOK = SS1.ActiveSheet.Cells[nCurRow, 10].Text.Trim();

                    if (!strWrtNo.IsNullOrEmpty() && strOK.IsNullOrEmpty())
                    {
                        for (int j = 0; j < SS1.ActiveSheet.RowCount; j++)
                        {
                            nCurRow = j;
                            if (strWrtNo == SS1.ActiveSheet.Cells[nCurRow, 0].Text.Trim())
                            {
                                SS1.ActiveSheet.Cells[nCurRow, 10].Text = "Y";
                                nCurRow = i;
                                SS1.ActiveSheet.Cells[nCurRow, 10].Text = "Y";
                                if (i != (j + 1) && i != (j - 1))
                                {
                                    SS1.ActiveSheet.RowCount += 1;
                                    SS1.ActiveSheet.AddRows(i + 1, 1);
                                    if (i > j)
                                    {
                                        SS1.ActiveSheet.CopyRange(j, 0, i + 1, 0, 1, SS1.ActiveSheet.ColumnCount, false);
                                        SS1.ActiveSheet.Cells[i, 2].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0E0FF"));
                                        SS1.ActiveSheet.Cells[i + 1, 2].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0E0FF"));
                                        SS1.ActiveSheet.Rows[j].Remove();
                                    }
                                    if (i < j)
                                    {
                                        SS1.ActiveSheet.CopyRange(j + 1, 0, i + 1, 0, 1, SS1.ActiveSheet.ColumnCount, false);                                     
                                        SS1.ActiveSheet.Cells[i, 2].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0E0FF"));
                                        SS1.ActiveSheet.Cells[i + 1, 2].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0E0FF"));
                                        SS1.ActiveSheet.Rows[j + 1].Remove();
                                    }

                                    SS1.ActiveSheet.RowCount -= 1;
                                    if (i > j) nCurRow = i;
                                    if (i < j)
                                    {
                                        nCurRow = i + 1;
                                        i += 1;
                                    }
                                }
                                else
                                {
                                    if (i == j + 1)
                                    {
                                        SS1.ActiveSheet.Cells[i - 1, 2].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0E0FF"));
                                        SS1.ActiveSheet.Cells[i, 2].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0E0FF"));
                                    }
                                    if (i == j - 1)
                                    {
                                        SS1.ActiveSheet.Cells[i, 2].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0E0FF"));
                                        SS1.ActiveSheet.Cells[j, 2].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0E0FF"));
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }            
        }

        void eSpreadClick(object sender, CellClickEventArgs e)
        {
            int nRow = 0;
            FpSpread s = (FpSpread)sender;

            if (sender == this.SS1)
            {
                if (e.Row == 0 || e.Column != 7) return;

                pnlJepsu.Visible = true;

                FnWRTNO = SS1.ActiveSheet.Cells[nRow, 1].Text.To<long>();

                SS3.ActiveSheet.RowCount = 0;

                nRow = 0;
                //해당방에 등록되어 있는지 확인
                List<HEA_SANGDAM_WAIT> lst = heaSangdamWaitService.GetSangdamWaitInfobyEndoRoom(ENDO_ROOM, clsPublic.GstrSysDate);

                if (lst.Count > 0)
                {
                    for (int i = 0; i < lst.Count; i++)
                    {
                        if (heaJepsuService.GetCountbyWrtNo(lst[i].WRTNO) > 0)
                        {
                            nRow += 1;
                            if (SS3.ActiveSheet.RowCount < nRow)
                            {
                                SS3.ActiveSheet.RowCount = nRow;
                                SS3.ActiveSheet.Cells[nRow - 1, 0].Text = lst[i].SNAME;
                                SS3.ActiveSheet.Cells[nRow - 1, 1].Text = lst[i].ENTTIME;
                                SS3.ActiveSheet.Cells[nRow - 1, 2].Text = lst[i].WRTNO.To<string>();
                                SS3.ActiveSheet.Cells[nRow - 1, 3].Text = lst[i].WAITNO.To<string>();
                            }
                        }
                    }
                }
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
            

            FpSpread o = (FpSpread)sender;
            ComFunc.ReadSysDate(clsDB.DbCon);

            if (sender == this.SS1)
            {
                string[] strRooms = { "12", "13", "14" };

                if (e.RowHeader == true || e.ColumnHeader == true) return;

                //if (e.Row == 0 || e.Column == 0) return;
                strRoom = "";
                if (e.Column == 0) strRoom = "12";
                if (e.Column == 2) strRoom = "13";
                if (e.Column == 3) strRoom = "14";
                if (strRoom.IsNullOrEmpty()) return;

                nWRTNO = SS1.ActiveSheet.Cells[e.Row, 1].Text.To<long>();
                // 해당방에 등록
                HEA_JEPSU lst = heaJepsuService.GetItembyWrtNo(nWRTNO);
                if (!lst.IsNullOrEmpty())
                {
                    strSName = lst.SNAME;
                    strSex = lst.SEX;
                    nAge = lst.AGE;
                    strGjJong = lst.GJJONG;

                    //기존순번 등록내역 있는지 확인
                    if (heaSangdamWaitService.GetRowIdbyWrtNo(nWRTNO, clsPublic.GstrSysDate, Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(1).ToShortDateString(), strRooms) > 0)
                    {
                        MessageBox.Show("이미 상담대기 순번이 등록되었습니다!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    clsDB.setBeginTran(clsDB.DbCon);

                    nWait = heaSangdamWaitService.GetWaitNobyEndoRoom(strRoom, clsPublic.GstrSysDate);

                    HEA_SANGDAM_HIS item2 = new HEA_SANGDAM_HIS();

                    item2.WRTNO = nWRTNO;
                    item2.SNAME = strSName;
                    item2.SEX = strSex;
                    item2.AGE = nAge;
                    item2.GJJONG = strGjJong;
                    item2.GUBUN = strRoom;
                    item2.WAITNO = nWait;
                    item2.ENTGUBUN = "";

                    int result1 = heaSangdamHisService.InsertSangdam(item2);

                    if (result1 < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("상담대기_HIS 순번등록 중 오류 발생!", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);

                    fn_Screen_Display();
                }
                else
                {
                    MessageBox.Show("접수가 취소된 번호입니다!", "등록불가", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else if (sender == this.SS2)
            {
                if (e.RowHeader == true || e.ColumnHeader == true) return;

                nWRTNO = SS2.ActiveSheet.Cells[e.Row, 2].Text.To<long>();

                clsDB.setBeginTran(clsDB.DbCon);

                //기존의 자료가 있으면 삭제함
                int result = heaSangdamWaitService.DeleteSangdamWait(nWRTNO);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("대기순번 삭제시 오류 발생!", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                HEA_SANGDAM_WAIT lst = heaSangdamWaitService.GetItembyWrtNo(nWRTNO);

                nAge = lst.AGE;
                nWait = lst.WAITNO;
                strSName = lst.SNAME;
                strSex = lst.SEX;
                strGjJong = lst.GJJONG;

                HEA_SANGDAM_HIS His = new HEA_SANGDAM_HIS();

                His.AGE = nAge;
                His.WAITNO = nWait;
                His.SNAME = strSName;
                His.SEX = lst.SEX;
                His.GJJONG = lst.GJJONG;
                His.ENTGUBUN = "2";

                int result2 = heaSangdamHisService.InsertSangdamHis(His);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("상담대기_HIS 순번등록 중 오류 발생!", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                clsDB.setCommitTran(clsDB.DbCon);

                fn_Screen_Display();
            }
            else if (sender == this.SS3)
            {
                if (e.RowHeader == true || e.ColumnHeader == true) return;

                nWait = SS3.ActiveSheet.Cells[e.Row, 3].Text.To<long>();

                //해당방에 등록
                HEA_JEPSU lst = heaJepsuService.GetItembyWrtNo(nWRTNO);

                if (!lst.IsNullOrEmpty())
                {
                    strSName = lst.SNAME;
                    strSex = lst.SEX;
                    nAge = lst.AGE;
                    strGjJong = lst.GJJONG;

                    clsDB.setBeginTran(clsDB.DbCon);

                    HEA_SANGDAM_WAIT item = new HEA_SANGDAM_WAIT();

                    item.WRTNO = nWRTNO;
                    item.SNAME = strSName;
                    item.SEX = strSex;
                    item.AGE = nAge;
                    item.GJJONG = strGjJong;
                    item.GUBUN = "12";
                    item.WAITNO = nWait;

                    int result = heaSangdamWaitService.InsertSangdamWait(item);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("상담대기 순번등록 중 오류 발생!", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    for (int i = 0; i < SS3.ActiveSheet.RowCount; i++)
                    {
                        nWait += 1;
                        nWRTNO = SS3.ActiveSheet.Cells[i, 2].Text.To<long>();

                        int result3 = heaSangdamWaitService.UpdateSangdamWaitNo(nWait, nWRTNO, "12");

                        if (result3 < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("상담대기 순번등록 중 오류 발생!", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    fn_Screen_Display();
                }
                else
                {
                    MessageBox.Show("접수가 취소된 번호입니다!", "등록불가", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else if (sender == this.SS4)
            {
                //기존의 자료가 있으면 삭제함
                nWRTNO = SS4.ActiveSheet.Cells[e.Row, 2].Text.To<long>();

                int result = heaSangdamWaitService.DeleteSangdamWait(nWRTNO);

                if (result < 0)
                {
                    MessageBox.Show("대기순번 삭제시 오류 발생!", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                fn_Screen_Display();
            }
            else if (sender == this.SS5)
            {
                //기존의 자료가 있으면 삭제함
                nWRTNO = SS5.ActiveSheet.Cells[e.Row, 2].Text.To<long>();

                int result = heaSangdamWaitService.DeleteSangdamWait(nWRTNO);

                if (result < 0)
                {
                    MessageBox.Show("대기순번 삭제시 오류 발생!", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                fn_Screen_Display();
            }
        }

        private void fn_Screen_Display()
        {
            long nREAD = 0;
            int nRow = 0;
            long nWrtNo = 0;
            long nHeaPano = 0;
            string strTemp = "";
            string strPtno = "";
            string strOK = "";
            List<string> strEndo_Room = new List<string>();

            sp.Spread_All_Clear(SS1);
            sp.Spread_All_Clear(SS2);
            sp.Spread_All_Clear(SS3);
            sp.Spread_All_Clear(SS4);
            sp.Spread_All_Clear(SS5);

            strEndo_Room.Clear();
            strEndo_Room.Add("12");
            strEndo_Room.Add("13");

            nRow = 0;

            //접수순번 명단 표시
            List<HEA_JEPSU_RESULT> lst = heaJepsuResultService.GetItembySDate(dtpSDate.Text);

            nREAD = lst.Count;
            for (int i = 0; i < nREAD; i++)
            {
                strOK = "OK";
                nWrtNo = lst[i].WRTNO;
                HEA_SANGDAM_WAIT wait = heaSangdamWaitService.GetGBCallbyWrtNo(nWrtNo, clsPublic.GstrSysDate, Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(1).ToShortDateString(), strEndo_Room);

                if (!wait.IsNullOrEmpty())
                {
                    if (wait.GBCALL == "Y")
                    {
                        strOK = "OK";
                    }

                    if (wait.GBCALL.IsNullOrEmpty())
                    {
                        strOK = "";
                    }
                }

                if (strOK == "OK")
                {
                    nRow += 1;
                    if (SS1.ActiveSheet.RowCount < nRow)
                    {
                        SS1.ActiveSheet.RowCount = nRow;
                    }

                    if (!wait.IsNullOrEmpty())
                    {
                        if (wait.GBCALL == "Y")
                        {
                            SS1.ActiveSheet.Cells[nRow, 0].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFC0C0"));
                        }
                    }

                    SS1.ActiveSheet.Cells[nRow - 1, 0].Text = lst[i].PANO.To<string>();
                    SS1.ActiveSheet.Cells[nRow - 1, 1].Text = lst[i].WRTNO.To<string>();
                    SS1.ActiveSheet.Cells[nRow - 1, 2].Text = lst[i].SNAME;
                    SS1.ActiveSheet.Cells[nRow - 1, 3].Text = lst[i].AMPM2 == "2" ? "오후" : "";
                    SS1.ActiveSheet.Cells[nRow - 1, 4].Text = lst[i].ENTTIME;

                    if (lst[i].LTDCODE != 0)
                    {
                        SS1.ActiveSheet.Cells[nRow - 1, 5].Text = hb.READ_Ltd_Name(lst[i].LTDCODE.To<string>());
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[nRow - 1, 5].Text = "개인종검";
                    }
                    SS1.ActiveSheet.Cells[nRow - 1, 6].Text = lst[i].GBDAILY == "Y" ? "Y" : "";
                    SS1.ActiveSheet.Cells[nRow - 1, 7].Text = lst[i].EXAMCHANGE;
                }                
            }

            nRow = 0;
            //해당방에 등록되어 있는지 확인
            List<HEA_SANGDAM_WAIT> lstSdWait = heaSangdamWaitService.GetSangdamWaitInfobyEndoRoom("12", clsPublic.GstrSysDate);

            if (lstSdWait.Count > 0)
            {
                nREAD = lstSdWait.Count;
                for (int i = 0; i < nREAD; i++)
                {
                    nWrtNo = lstSdWait[i].WRTNO;
                    if (heaJepsuService.GetCountbyWrtNo(nWrtNo) > 0)
                    {
                        nRow += 1;
                        if (SS2.ActiveSheet.RowCount < nRow)
                        {
                            SS2.ActiveSheet.RowCount = nRow;
                        }
                        SS2.ActiveSheet.Cells[nRow - 1, 0].Text = lstSdWait[i].SNAME;
                        SS2.ActiveSheet.Cells[nRow - 1, 1].Text = lstSdWait[i].ENTTIME;
                        SS2.ActiveSheet.Cells[nRow - 1, 2].Text = lstSdWait[i].WRTNO.To<string>();
                    }
                }
            }

            nRow = 0;
            List<HEA_SANGDAM_WAIT> lstSdWait2 = heaSangdamWaitService.GetSangdamWaitInfobyEndoRoom("13", clsPublic.GstrSysDate);

            if (lstSdWait2.Count > 0)
            {
                nREAD = lstSdWait2.Count;
                for (int i = 0; i < nREAD; i++)
                {
                    nWrtNo = lstSdWait2[i].WRTNO;
                    if (heaJepsuService.GetCountbyWrtNo(nWrtNo) > 0)
                    {
                        nRow += 1;
                        if (SS4.ActiveSheet.RowCount < nRow)
                        {
                            SS4.ActiveSheet.RowCount = nRow;
                        }
                        SS4.ActiveSheet.Cells[nRow - 1, 0].Text = lstSdWait2[i].SNAME;
                        SS4.ActiveSheet.Cells[nRow - 1, 1].Text = lstSdWait2[i].ENTTIME;
                        SS4.ActiveSheet.Cells[nRow - 1, 2].Text = lstSdWait2[i].WRTNO.To<string>();
                    }
                }
            }

            nRow = 0;
            List<HEA_SANGDAM_WAIT> lstSdWait3 = heaSangdamWaitService.GetSangdamWaitInfobyEndoRoom("14", clsPublic.GstrSysDate);

            if (lstSdWait3.Count > 0)
            {
                nREAD = lstSdWait3.Count;
                for (int i = 0; i < nREAD; i++)
                {
                    nWrtNo = lstSdWait3[i].WRTNO;
                    if (heaJepsuService.GetCountbyWrtNo(nWrtNo) > 0)
                    {
                        nRow += 1;
                        if (SS5.ActiveSheet.RowCount < nRow)
                        {
                            SS5.ActiveSheet.RowCount = nRow;
                        }
                        SS5.ActiveSheet.Cells[nRow - 1, 0].Text = lstSdWait3[i].SNAME;
                        SS5.ActiveSheet.Cells[nRow - 1, 1].Text = lstSdWait3[i].ENTTIME;
                        SS5.ActiveSheet.Cells[nRow - 1, 2].Text = lstSdWait3[i].WRTNO.To<string>();
                    }
                }
            }

            pnlJepsu.Visible = false;
        }
    }
}
