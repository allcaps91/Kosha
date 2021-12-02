using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Act
/// File Name       : frmHaEndoSeqReg.cs
/// Description     : 내시경검사 대기순번 등록
/// Author          : 이상훈
/// Create Date     : 2019-08-26
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmAct05.frm(Frm내시경실검사순번등록)" />

namespace HC_Act
{
    public partial class frmHaEndoSeqReg : Form
    {
        HicJepsuResultService hicJepsuResultService = null;
        HeaSangdamWaitService heaSangdamWaitService = null;
        HicResultService hicResultService = null;
        HicXrayResultService hicXrayResultService = null;
        HeaJepsuService heaJepsuService = null;
        HeaSangdamHisService heaSangdamHisService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();

        long FnWRTNO;
        const string ENDO_ROOM = "09";  //대기순번 내시경실 번호

        public frmHaEndoSeqReg()
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

            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.SS1.CellClick += new CellClickEventHandler(eSpreadClick);
            this.SS2.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
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

            SS1_Sheet1.Columns.Get(1).Visible = false;

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

                sp.Spread_All_Clear(SS1);
                sp.Spread_All_Clear(SS2);
                sp.Spread_All_Clear(SS3);

                //접수순번 명단 표시
                List<HIC_JEPSU_RESULT> lst = hicJepsuResultService.GetWrtNobySysDate("ENTTIME");

                nREAD = lst.Count;
                for (int i = 0; i < nREAD; i++)
                {
                    strOK = "OK";
                    nWrtNo = lst[i].WRTNO;
                    string[] strRoom = { "09" };
                    if (heaSangdamWaitService.GetRowIdbyWrtNo(nWrtNo, clsPublic.GstrSysDate, Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(1).ToShortDateString(), strRoom) > 0)
                    {
                        strOK = "";
                    }

                    if (strOK == "OK")
                    {
                        nRow += 1;
                        if (SS1.ActiveSheet.RowCount < nRow)
                        {
                            SS1.ActiveSheet.RowCount = nRow;
                        }
                        SS1.ActiveSheet.Cells[nRow, 0].Text = lst[i].PANO.ToString();
                        SS1.ActiveSheet.Cells[nRow, 1].Text = lst[i].WRTNO.ToString();
                        SS1.ActiveSheet.Cells[nRow, 2].Text = lst[i].SNAME;
                        SS1.ActiveSheet.Cells[nRow, 3].Text = lst[i].AMPM2 == "2" ? "오후" : "";
                        SS1.ActiveSheet.Cells[nRow, 4].Text = lst[i].ENTTIME;

                        if (lst[i].LTDCODE != 0)
                        {
                            SS1.ActiveSheet.Cells[nRow, 5].Text = hb.READ_Ltd_Name(lst[i].LTDCODE.ToString());
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[nRow, 5].Text = "개인종검";
                        }
                        SS1.ActiveSheet.Cells[nRow, 6].Text = lst[i].EXAMCHANGE.Trim();

                        if (nWrtNo > 0)
                        {
                            //수면내시경 노란색
                            string[] strExCodes = { "TX20" };
                            if (hicResultService.GetCountbyWrtNo(nWrtNo, strExCodes) > 0)
                            {
                                SS1.ActiveSheet.Cells[nRow, 2].BackColor = Color.Yellow;
                            }
                            else
                            {
                                SS1.ActiveSheet.Cells[nRow, 2].BackColor = Color.White;
                            }
                        }
                        //내시경검사 여부
                        if (!hicXrayResultService.GetEndoResultDateByPtno(lst[i].PTNO).IsNullOrEmpty())
                        {
                            SS1.ActiveSheet.Cells[nRow, 8].Text = "완료";
                        }
                        else
                        {
                            if (hicResultService.GetResultByWrtNo(nWrtNo) == "01")
                            {
                                SS1.ActiveSheet.Cells[nRow, 8].Text = "완료";
                            }
                            else
                            {
                                SS1.ActiveSheet.Cells[nRow, 8].Text = "";
                            }
                        }
                    }
                }

                nRow = 0;
                //해당방에 등록되어 있는지 확인
                List<HEA_SANGDAM_WAIT> lstSdWait = heaSangdamWaitService.GetSangdamWaitInfobyEndoRoom(ENDO_ROOM, clsPublic.GstrSysDate);

                if (lstSdWait.Count > 0)
                {
                    nRow = lstSdWait.Count;
                    for (int i = 0; i < nRow; i++)
                    {
                        nWrtNo = lstSdWait[i].WRTNO;
                        string[] strExCodes = { "TX20" };
                        if (hicResultService.GetCountbyWrtNo(nWrtNo, strExCodes) > 0)
                        {
                            nRow += 1;
                            if (SS2.ActiveSheet.RowCount < nRow)
                            {
                                SS2.ActiveSheet.RowCount = nRow;
                            }
                            SS2.ActiveSheet.Cells[nRow, 0].Text = lstSdWait[i].SNAME.Trim();
                            SS2.ActiveSheet.Cells[nRow, 1].Text = lstSdWait[i].ENTTIME.Trim();
                            SS2.ActiveSheet.Cells[nRow, 2].Text = lstSdWait[i].WRTNO.ToString();
                        }

                        if (nWrtNo > 0)
                        {
                            //수면내시경 노란색
                            string[] strExCodes1 = { "TX20" };
                            if (hicResultService.GetCountbyWrtNo(nWrtNo, strExCodes1) > 0)
                            {
                                SS2.ActiveSheet.Cells[nRow, 0].BackColor = Color.Yellow;
                            }
                            else
                            {
                                SS2.ActiveSheet.Cells[nRow, 0].BackColor = Color.White;
                            }
                        }
                    }
                    pnlJepsu.Visible = false;
                }
            }
            else if (sender == btnPrint)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                if (int.Parse(txtCount.Text) <= 0)
                {
                    return;
                }

                ComFunc.ReadSysDate(clsDB.DbCon);

                SS1.ActiveSheet.Columns.Get(5).Width = 20;

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;                

                strTitle = "내시경 접수자 명단";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);

                ///TODO : 이상훈(2019.08.26) 출력 Log 여부 확인
                //SQL_LOG("", SS1.PrintHeader);
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

                FnWRTNO = long.Parse(SS1.ActiveSheet.Cells[nRow, 1].Text);

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
                                SS3.ActiveSheet.Cells[nRow, 0].Text = lst[i].SNAME.Trim();
                                SS3.ActiveSheet.Cells[nRow, 1].Text = lst[i].ENTTIME.Trim();
                                SS3.ActiveSheet.Cells[nRow, 2].Text = lst[i].WRTNO.ToString();
                                SS3.ActiveSheet.Cells[nRow, 3].Text = lst[i].WAITNO.ToString();
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
                if (e.RowHeader == true || e.ColumnHeader == true) return;

                nWRTNO = long.Parse(SS1.ActiveSheet.Cells[e.Row, 1].Text);
                // 해당방에 등록
                HEA_JEPSU lst = heaJepsuService.GetItembyWrtNo(nWRTNO);
                if (lst != null)
                {
                    strSName = lst.SNAME.Trim();
                    strSex = lst.SEX;
                    nAge = lst.AGE;
                    strGjJong = lst.GJJONG.Trim();

                    nWait = heaSangdamWaitService.GetWaitNobyEndoRoom(ENDO_ROOM, clsPublic.GstrSysDate);

                    HEA_SANGDAM_WAIT item = new HEA_SANGDAM_WAIT();

                    item.WRTNO = nWRTNO;
                    item.SNAME = strSName;
                    item.SEX = strSex;
                    item.AGE = nAge;
                    item.GJJONG = strGjJong;
                    item.GUBUN = ENDO_ROOM;
                    item.WAITNO = nWait;

                    int result = heaSangdamWaitService.InsertSangdamWait(item);

                    if (result < 0)
                    {
                        MessageBox.Show("상담대기 순번등록 중 오류 발생!", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    HEA_SANGDAM_HIS item2 = new HEA_SANGDAM_HIS();

                    item2.WRTNO = nWRTNO;
                    item2.SNAME = strSName;
                    item2.SEX = strSex;
                    item2.AGE = nAge;
                    item2.GJJONG = strGjJong;
                    item2.GUBUN = ENDO_ROOM;
                    item2.WAITNO = nWait;
                    item2.ENTGUBUN = "1";

                    int result1 = heaSangdamHisService.InsertSangdamHis(item2);

                    if (result1 < 0)
                    {
                        MessageBox.Show("상담대기_HIS 순번등록 중 오류 발생!", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

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

                nWRTNO = long.Parse(SS1.ActiveSheet.Cells[e.Row, 2].Text);

                //기존의 자료가 있으면 삭제함
                int result = heaSangdamWaitService.DeleteSangdamWait(nWRTNO);

                if (result < 0)
                {
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
                    MessageBox.Show("상담대기_HIS 순번등록 중 오류 발생!", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                fn_Screen_Display();
            }
            else if (sender == this.SS3)
            {
                if (e.RowHeader == true || e.ColumnHeader == true) return;

                nWait = long.Parse(SS3.ActiveSheet.Cells[e.Row, 3].Text);

                //해당방에 등록
                HEA_JEPSU lst = heaJepsuService.GetItembyWrtNo(nWRTNO);
                if (lst != null)
                {
                    strSName = lst.SNAME.Trim();
                    strSex = lst.SEX;
                    nAge = lst.AGE;
                    strGjJong = lst.GJJONG.Trim();

                    HEA_SANGDAM_WAIT item = new HEA_SANGDAM_WAIT();

                    item.WRTNO = nWRTNO;
                    item.SNAME = strSName;
                    item.SEX = strSex;
                    item.AGE = nAge;
                    item.GJJONG = strGjJong;
                    item.GUBUN = ENDO_ROOM;
                    item.WAITNO = nWait;

                    int result = heaSangdamWaitService.InsertSangdamWait(item);

                    if (result < 0)
                    {
                        MessageBox.Show("상담대기 순번등록 중 오류 발생!", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    for (int i = 0; i < SS3.ActiveSheet.RowCount; i++)
                    {
                        nWait += 1;
                        nWRTNO = long.Parse(SS3.ActiveSheet.Cells[i, 2].Text);

                        int result3 = heaSangdamWaitService.UpdateSangdamWaitNo(nWait, nWRTNO, ENDO_ROOM);

                        if (result3 < 0)
                        {
                            MessageBox.Show("상담대기 순번등록 중 오류 발생!", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

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
                        MessageBox.Show("상담대기_HIS 순번등록 중 오류 발생!", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    fn_Screen_Display();
                }
                else
                {
                    MessageBox.Show("접수가 취소된 번호입니다!", "등록불가", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
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
            string[] strExCode = { "TX20" }; 
            

            sp.Spread_All_Clear(SS1);
            sp.Spread_All_Clear(SS2);
            sp.Spread_All_Clear(SS3);

            nRow = 0;

            string[] strRoom = { "09" };

            //접수순번 명단 표시
            List<HIC_JEPSU_RESULT> lst = hicJepsuResultService.GetWrtNobySysDate("ENTTIME");

            nREAD = lst.Count;
            for (int i = 0; i < nREAD; i++)
            {
                strOK = "OK";
                nWrtNo = lst[i].WRTNO;

                if (heaSangdamWaitService.GetRowIdbyWrtNo(nWrtNo, clsPublic.GstrSysDate, Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(1).ToShortDateString(), strRoom) > 0)
                {
                    strOK = "";
                }

                if (strOK == "OK")
                {
                    nRow += 1;
                    if (SS1.ActiveSheet.RowCount < nRow)
                    {
                        SS1.ActiveSheet.RowCount = nRow;
                    }
                    SS1.ActiveSheet.Cells[nRow, 0].Text = lst[i].PANO.ToString();
                    SS1.ActiveSheet.Cells[nRow, 1].Text = lst[i].WRTNO.ToString();
                    SS1.ActiveSheet.Cells[nRow, 2].Text = lst[i].SNAME;
                    SS1.ActiveSheet.Cells[nRow, 3].Text = lst[i].AMPM2 == "2" ? "오후" : "";
                    SS1.ActiveSheet.Cells[nRow, 4].Text = lst[i].ENTTIME;

                    if (lst[i].LTDCODE != 0)
                    {
                        SS1.ActiveSheet.Cells[nRow, 5].Text = hb.READ_Ltd_Name(lst[i].LTDCODE.ToString());
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[nRow, 5].Text = "개인종검";
                    }
                    SS1.ActiveSheet.Cells[nRow, 6].Text = lst[i].EXAMCHANGE.Trim();

                    if (nWrtNo > 0)
                    {
                        //수면내시경 노란색
                        if (hicResultService.GetCountbyWrtNo(nWrtNo, strExCode) > 0)
                        {
                            SS1.ActiveSheet.Cells[nRow, 2].BackColor = Color.Yellow;
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[nRow, 2].BackColor = Color.White;
                        }
                    }                    
                }
            }

            nRow = 0;
            //해당방에 등록되어 있는지 확인
            List<HEA_SANGDAM_WAIT> lstSdWait = heaSangdamWaitService.GetSangdamWaitInfobyEndoRoom(ENDO_ROOM, clsPublic.GstrSysDate);

            if (lstSdWait.Count > 0)
            {
                nRow = lstSdWait.Count;
                for (int i = 0; i < nRow; i++)
                {
                    nWrtNo = lstSdWait[i].WRTNO;
                    if (hicResultService.GetCountbyWrtNo(nWrtNo, strExCode) > 0)
                    {
                        nRow += 1;
                        if (SS2.ActiveSheet.RowCount < nRow)
                        {
                            SS2.ActiveSheet.RowCount = nRow;
                        }
                        SS2.ActiveSheet.Cells[nRow, 0].Text = lstSdWait[i].SNAME.Trim();
                        SS2.ActiveSheet.Cells[nRow, 1].Text = lstSdWait[i].ENTTIME.Trim();
                        SS2.ActiveSheet.Cells[nRow, 2].Text = lstSdWait[i].WRTNO.ToString();
                    }

                    if (nWrtNo > 0)
                    {
                        //수면내시경 노란색
                        if (hicResultService.GetCountbyWrtNo(nWrtNo, strExCode) > 0)
                        {
                            SS2.ActiveSheet.Cells[nRow, 0].BackColor = Color.Yellow;
                        }
                        else
                        {
                            SS2.ActiveSheet.Cells[nRow, 0].BackColor = Color.White;
                        }
                    }
                }
                pnlJepsu.Visible = false;
            }
        }
    }
}
