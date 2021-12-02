using ComBase;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ComHpcLibB.form.HcView;
using ComBase.Controls;
using System.ComponentModel;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmRegisteredMailFileCreate.cs
/// Description     : 등기우편파일생성
/// Author          : 이상훈
/// Create Date     : 2019-09-09
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm등기우편파일생성.frm(Frm등기우편파일생성)" />

namespace ComHpcLibB
{
    public partial class frmRegisteredMailFileCreate : Form
    {
        HeaJepsuService heaJepsuService = null;
        ComHpcLibBService comHpcLibBService = null;
        HeaMailSendJepsuService heaMailSendJepsuService = null;
        HeaJepsuPatientService heaJepsuPatientService = null;
        HeaMailsendService heaMailsendService = null;

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();

        public frmRegisteredMailFileCreate()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            heaJepsuService = new HeaJepsuService();
            comHpcLibBService = new ComHpcLibBService();
            heaMailSendJepsuService = new HeaMailSendJepsuService();
            heaJepsuPatientService = new HeaJepsuPatientService();
            heaMailsendService = new HeaMailsendService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.btnImport.Click += new EventHandler(eBtnClick);
            this.btnExport.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.btnSearch2.Click += new EventHandler(eBtnClick);

            this.btnExcel.Click += new EventHandler(eBtnClick);
            this.btnFileMake.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.btnSendDateDelete.Click += new EventHandler(eBtnClick);
            this.btnRecvDateDelete.Click += new EventHandler(eBtnClick);
            this.btnMailWeightSave.Click += new EventHandler(eBtnClick);

            this.rdoSend3.Click += new EventHandler(eRodClick);
            this.rdoSend2.Click += new EventHandler(eRodClick);
            this.rdoSend1.Click += new EventHandler(eRodClick);
            this.rdoSend4.Click += new EventHandler(eRodClick);
            this.chkAll.Click += new EventHandler(eChkBoxCheck);
            this.chkAll2.Click += new EventHandler(eChkBoxCheck);
            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);

            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eKeyPress);
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtLtdCode)
            {
                if (e.KeyChar == 13)
                {
                    eBtnClick(btnLtdCode, new EventArgs());
                }
            }
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFrDate.Text = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-10).ToShortDateString();
            dtpToDate.Text = clsPublic.GstrSysDate;

            dtpSendDate.Text = clsPublic.GstrSysDate;
            txtSName.Text = "";
            txtLtdCode.Text = "";

            sp.Spread_All_Clear(SSList);
            sp.Spread_All_Clear(SS1);

            SS1.ActiveSheet.RowCount = 0;
        }

        void eRodClick(object sender, EventArgs e)
        {
            if (sender == rdoSend3 || sender == rdoSend2 || sender == rdoSend1 || sender == rdoSend4)
            {
                eBtnClick(btnSearch, new EventArgs());
            }
        }

        void eChkBoxCheck(object sender, EventArgs e)
        {
            if (sender == chkAll)
            {
                if (chkAll.Checked == true)
                {   
                    SSList.ActiveSheet.Cells[0, 0, SSList.ActiveSheet.RowCount - 1, 0].Text = "True";
                }
                else
                {
                    SSList.ActiveSheet.Cells[0, 0, SSList.ActiveSheet.RowCount - 1, 0].Text = "";
                }
            }
            else if (sender == chkAll2)
            {
                if (chkAll2.Checked == true)
                {
                    SS1.ActiveSheet.Cells[0, 0, SS1.ActiveSheet.RowCount - 1, 0].Text = "True";
                }
                else
                {
                    SS1.ActiveSheet.Cells[0, 0, SS1.ActiveSheet.RowCount - 1, 0].Text = "";
                }
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
                int nRow = 0;
                int nCurRow = 0;
                int nRead = 0;
                string strWrtNo = "";
                string strOK = "";
                string strSend = "";
                string strSort = "";
                long nLtdCode = 0;

                if (rdoSend1.Checked == true)
                {
                    strSend = "1";
                }
                else if (rdoSend2.Checked == true)
                {
                    strSend = "2";
                }
                else if (rdoSend3.Checked == true)
                {
                    strSend = "3";
                }
                else if (rdoSend4.Checked == true)
                {
                    strSend = "4";
                }

                if (rdoSort1.Checked == true)
                {
                    strSort = "1";
                }
                else if (rdoSort2.Checked == true)
                {
                    strSort = "2";
                }
                else if (rdoSort3.Checked == true)
                {
                    strSort = "3";
                }
                else if (rdoSort4.Checked == true)
                {
                    strSort = "4";
                }

                sp.Spread_All_Clear(SSList);

                if (txtLtdCode.Text.Trim() == "")
                {
                    nLtdCode = 0;
                }
                else
                {
                    nLtdCode = VB.Pstr(txtLtdCode.Text.Trim(), ".", 1).To<long>();
                }

                List<HEA_JEPSU_PATIENT> list = heaJepsuPatientService.GetItembySName(dtpFrDate.Text, dtpToDate.Text, nLtdCode, txtSName.Text.Trim(), strSend, strSort);

                nRow = 0;
                for (int i = 0; i < list.Count; i++)
                {
                    nRow += 1;
                    if (nRow > SSList.ActiveSheet.RowCount)
                    {
                        SSList.ActiveSheet.RowCount = nRow;
                    }

                    SSList.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].PTNO;
                    SSList.ActiveSheet.Cells[nRow - 1, 12].Text = list[i].WRTNO.To<string>();
                    SSList.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].SNAME;
                    SSList.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].SDATE;
                    SSList.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].LTDNAME;
                    SSList.ActiveSheet.Cells[nRow - 1, 5].Text = list[i].MAILWEIGHT;
                    SSList.ActiveSheet.Cells[nRow - 1, 6].Text = list[i].PRTDATE;
                    SSList.ActiveSheet.Cells[nRow - 1, 7].Text = list[i].RECVDATE;
                    SSList.ActiveSheet.Cells[nRow - 1, 8].Text = list[i].MAILDATE;
                    SSList.ActiveSheet.Cells[nRow - 1, 9].Text = list[i].PANO.To<string>();
                    SSList.ActiveSheet.Cells[nRow - 1, 10].Text = list[i].FAMILLY;
                    if (!list[i].FAMILLY.IsNullOrEmpty())
                    {
                        SSList.ActiveSheet.Cells[i, 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0FFFF"));
                    }
                    SSList.ActiveSheet.Cells[nRow - 1, 11].Text = "";
                    //SSList.ActiveSheet.Cells[nRow - 1, 12].Text = list[i].WRTNO.To<string>();
                }

                SSList.ActiveSheet.RowCount = nRow;

                //for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
                //{
                //    strWrtNo = "";
                //    strOK = "";
                //    nCurRow = i;
                //    strWrtNo = SSList.ActiveSheet.Cells[nCurRow, 10].Text.Trim();
                //    strOK = SSList.ActiveSheet.Cells[nCurRow, 11].Text.Trim();

                //    if (strWrtNo != "" && strOK == "")
                //    {
                //        for (int j = 0; j < SSList.ActiveSheet.RowCount; j++)
                //        {
                //            nCurRow = j;
                //            if (strWrtNo == SSList.ActiveSheet.Cells[nCurRow, 9].Text.Trim())
                //            {
                //                SSList.ActiveSheet.Cells[nCurRow, 10].Text = "Y";
                //                nCurRow = i;
                //                SSList.ActiveSheet.Cells[nCurRow, 10].Text = "Y";
                //                if (i != (j + 1) && i != (j - 1))
                //                {
                //                    SSList.ActiveSheet.RowCount += 1;
                //                    SSList.ActiveSheet.Rows[i + 1].Add();
                //                    if (i > j)
                //                    {
                //                        SSList.ActiveSheet.CopyRange(j, 0, i + 1, 0, 1, SSList.ActiveSheet.ColumnCount, false);
                //                        SSList.ActiveSheet.Cells[i, 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0E0FF"));
                //                        SSList.ActiveSheet.Cells[i + 1, 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0E0FF"));
                //                        SSList.ActiveSheet.Rows[j].Remove();
                //                    }
                //                    if (i < j)
                //                    {
                //                        SSList.ActiveSheet.CopyRange(j + 1, 0, i + 1, 0, 1, SSList.ActiveSheet.ColumnCount, false);
                //                        SSList.ActiveSheet.Cells[i, 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0E0FF"));
                //                        SSList.ActiveSheet.Cells[i + 1, 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0E0FF"));
                //                        SSList.ActiveSheet.Rows[j + 1].Remove();
                //                    }

                //                    SSList.ActiveSheet.RowCount -= 1;
                //                    if (i > j) nCurRow = i;
                //                    if (i < j)
                //                    {
                //                        nCurRow = i + 1;
                //                        i += 1;
                //                    }
                //                }
                //                else
                //                {
                //                    if (i == (j + 1))
                //                    {
                //                        SSList.ActiveSheet.Cells[i - 1, 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0E0FF"));
                //                        SSList.ActiveSheet.Cells[i, 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0E0FF"));
                //                    }
                //                    if (i == (j - 1))
                //                    {
                //                        SSList.ActiveSheet.Cells[i, 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0E0FF"));
                //                        SSList.ActiveSheet.Cells[j, 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0E0FF"));
                //                    }
                //                }
                //                break;
                //            }
                //        }
                //    }
                //}
            }
            else if (sender == btnCancel)
            {
                sp.Spread_All_Clear(SS1);
            }
            else if (sender == btnImport)
            {
                int nRow = 0;
                long nWRTNO = 0;
                string strOK = "";
                string strPrtDate = "";
                string strRecvDate = "";
                string strMailDate = "";
                int nMailWeight = 0;
                string strTemp = "";

                nRow = SS1.ActiveSheet.RowCount;

                for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
                {
                    strTemp = "";
                    strOK = SSList.ActiveSheet.Cells[i, 0].Text;

                    if (strOK == "True")
                    {
                        nWRTNO = SSList.ActiveSheet.Cells[i, 12].Text.To<long>();
                        nMailWeight = SSList.ActiveSheet.Cells[i, 5].Text.To<int>();
                        strPrtDate = SSList.ActiveSheet.Cells[i, 6].Text.Trim();
                        strRecvDate = SSList.ActiveSheet.Cells[i, 7].Text.Trim();
                        strMailDate = SSList.ActiveSheet.Cells[i, 8].Text.Trim();

                        for (int j = 0; j < SS1.ActiveSheet.RowCount; j++)
                        {
                            if (nWRTNO == SS1.ActiveSheet.Cells[j, 6].Text.To<long>())
                            {
                                strTemp = "OK";
                                break;
                            }
                        }

                        //중량 항목 삭제로 인해 주석처리(2020.11.10)
                        //if (!strPrtDate.IsNullOrEmpty() && strMailDate.IsNullOrEmpty() && strRecvDate.IsNullOrEmpty() && strTemp != "OK")
                        //{
                        //    if (nMailWeight == 0)
                        //    {
                        //        MessageBox.Show(i + "번줄 우편물 중량을 입력하세요", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //        return;
                        //    }
                        //}
                    }
                }

                for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
                {
                    strTemp = "";
                    strOK = SSList.ActiveSheet.Cells[i, 0].Text.Trim();

                    if (strOK == "True")
                    {
                        SSList.ActiveSheet.Cells[i, 0].Text = "";
                        nWRTNO = SSList.ActiveSheet.Cells[i, 12].Text.To<long>();
                        nMailWeight = SSList.ActiveSheet.Cells[i, 5].Text.To<int>();
                        strPrtDate = SSList.ActiveSheet.Cells[i, 6].Text.Trim();
                        strRecvDate = SSList.ActiveSheet.Cells[i, 7].Text.Trim();
                        strMailDate = SSList.ActiveSheet.Cells[i, 8].Text.Trim();

                        for (int j = 0; j < SS1.ActiveSheet.RowCount; j++)
                        {
                            if (nWRTNO == SS1.ActiveSheet.Cells[j, 6].Text.To<long>())
                            {
                                strTemp = "OK";
                            }
                        }

                        if (!strPrtDate.IsNullOrEmpty() && strMailDate.IsNullOrEmpty() && strRecvDate.IsNullOrEmpty() && strTemp != "OK")
                        {
                            HEA_JEPSU list = heaJepsuService.GetItembyWrtNo(nWRTNO);

                            if (list != null)
                            {
                                nRow += 1;
                                if (SS1.ActiveSheet.RowCount < nRow)
                                {
                                    SS1.ActiveSheet.RowCount = nRow;
                                    SS1.ActiveSheet.Cells[nRow - 1, 1].Text = list.SNAME;
                                    SS1.ActiveSheet.Cells[nRow - 1, 2].Text = list.RECVDATE;
                                    SS1.ActiveSheet.Cells[nRow - 1, 3].Text = list.MAILDATE.To<string>();
                                    SS1.ActiveSheet.Cells[nRow - 1, 4].Text = list.MAILCODE;
                                    SS1.ActiveSheet.Cells[nRow - 1, 5].Text = list.JUSO;
                                    SS1.ActiveSheet.Cells[nRow - 1, 6].Text = nWRTNO.To<string>();
                                    SS1.ActiveSheet.Cells[nRow - 1, 7].Text = nMailWeight.To<string>();
                                }
                            }
                        }
                    }
                }

                chkAll.Checked = false;
                chkAll2.Checked = false;
            }
            else if (sender == btnExport)
            {
                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    if (SS1.ActiveSheet.Cells[i, 0].Text.Trim() == "True")
                    {
                        if (SS1.ActiveSheet.Cells[i, 3].Text.Trim() == "")
                        {
                            SS1.ActiveSheet.RemoveRows(i, 1);
                            i -= 1;
                        }
                    }
                }

                SS1.ActiveSheet.RowCount = SS1.ActiveSheet.NonEmptyRowCount;
            }
            else if (sender == btnLtdCode)
            {
                string strLtdCode = "";

                if (txtLtdCode.Text.IndexOf(".") > 0)
                {
                    strLtdCode = VB.Pstr(txtLtdCode.Text, ".", 2);
                }
                else
                {
                    strLtdCode = txtLtdCode.Text;
                }

                FrmHcLtdHelp = new frmHcLtdHelp(strLtdCode);
                FrmHcLtdHelp.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(LtdCd_value);
                FrmHcLtdHelp.ShowDialog();
                FrmHcLtdHelp.rSetGstrValue -= new frmHcLtdHelp.SetGstrValue(LtdCd_value);

                if (!LtdHelpItem.IsNullOrEmpty())
                {
                    txtLtdCode.Text = LtdHelpItem.CODE.ToString() + "." + LtdHelpItem.SANGHO;
                }
                else
                {
                    txtLtdCode.Text = "";
                }
            }
            else if (sender == btnSearch2)
            {
                int nRow = 0;
                sp.Spread_All_Clear(SS1);

                List<HEA_MAILSEND_JEPSU> list = heaMailSendJepsuService.GetItembySendDate(dtpSendDate.Text);

                SS1.ActiveSheet.RowCount = list.Count;
                //SS1.DataSource = list;

                for (int i = 0; i < list.Count; i++)
                {
                    SS1.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                    SS1.ActiveSheet.Cells[i, 2].Text = list[i].RECVDATE;
                    SS1.ActiveSheet.Cells[i, 3].Text = list[i].SENDDATE;
                    SS1.ActiveSheet.Cells[i, 4].Text = list[i].MAILCODE;
                    SS1.ActiveSheet.Cells[i, 5].Text = list[i].JUSO;
                    SS1.ActiveSheet.Cells[i, 6].Text = list[i].WRTNO;
                }
            }
            else if (sender == btnExcel)
            {
                bool x = false;

                if (MessageBox.Show("엑셀파일로 만드시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                sp.Spread_All_Clear(SS2);
                SS2.ActiveSheet.RowCount = SS1.ActiveSheet.RowCount;

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    SS2.ActiveSheet.Cells[i, 0].Text = SS1.ActiveSheet.Cells[i, 5].Text;    //주소
                    SS2.ActiveSheet.Cells[i, 1].Text = hb.READ_Ltd_Name(SS1.ActiveSheet.Cells[i, 6].Text.Trim());    //회사명
                    SS2.ActiveSheet.Cells[i, 2].Text = SS1.ActiveSheet.Cells[i, 1].Text;    //수검자명
                    SS2.ActiveSheet.Cells[i, 3].Text = SS1.ActiveSheet.Cells[i, 4].Text;    //우편번호
                }

                x = SS2.SaveExcel("C:\\등기우편대상자라벨.xls", FarPoint.Excel.ExcelSaveFlags.UseOOXMLFormat);

                if (x == true)
                {
                    MessageBox.Show("C:\\등기우편대상자라벨.xls" + " 엑셀파일이 생성되었습니다.", "작업완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("엑셀파일이 생성에 오류가 발생되었습니다.", "작업완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (sender == btnFileMake)
            {
                long nWRTNO = 0;
                int nMailWeight = 0;
                string strSendDate = "";
                string strRecvDate = "";
                string strJuso = "";
                string strMailCode = "";
                string strSname = "";
                string strDel = "";

                if (dtpSendDate.Text == "")
                {
                    MessageBox.Show("발송일자가 공란입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (SS1.ActiveSheet.RowCount == 0)
                {
                    return;
                }


                strSendDate = dtpSendDate.Text;

                clsDB.setBeginTran(clsDB.DbCon);

                List<HEA_MAILSEND> list = heaMailsendService.GetItembySendDate(strSendDate);

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strDel = SS1.ActiveSheet.Cells[i, 0].Text.Trim();
                    strSname = SS1.ActiveSheet.Cells[i, 1].Text.Trim();
                    strRecvDate = SS1.ActiveSheet.Cells[i, 2].Text.Trim();
                    strSendDate = SS1.ActiveSheet.Cells[i, 3].Text.Trim().IsNullOrEmpty() ? dtpSendDate.Text : SS1.ActiveSheet.Cells[i, 3].Text.Trim();
                    strMailCode = SS1.ActiveSheet.Cells[i, 4].Text.Trim();
                    strJuso = SS1.ActiveSheet.Cells[i, 5].Text.Trim();
                    nWRTNO = SS1.ActiveSheet.Cells[i, 6].Text.To<long>();
                    nMailWeight = SS1.ActiveSheet.Cells[i, 7].Text.To<int>();

                    if (strDel == "")
                    {
                        //수령일자가 없을때만 등록..
                        if (strRecvDate.IsNullOrEmpty())
                        {
                            if (heaMailsendService.GetCountbySendDate(nWRTNO, strSendDate) == 0)
                            {
                                HEA_MAILSEND item = new HEA_MAILSEND();

                                item.MAILCODE = strMailCode;
                                item.JUSO = strJuso;
                                item.SNAME = strSname;
                                item.SENDDATE = strSendDate;
                                item.WRTNO = nWRTNO;
                                item.ENTSABUN = clsType.User.IdNumber.To<long>();

                                //발송명단 Data Insert
                                int result = heaMailsendService.Insert(item);

                                if (result < 0)
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    MessageBox.Show("등기우편 발송대장 Data 등록시 오류 발생(HEA_MAILSEND)", "전산실 연락요망", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }

                            //2019-02-08 최종마지막 작업내역이 저장되도록 요청함 - 최종숙 팀장요청
                            //수령일자 및 수령일자 등록사번 저장 쿼리 추가로 함께 요청함
                            //발송일자 등록

                            HEA_JEPSU item1 = new HEA_JEPSU();

                            item1.MAILWEIGHT = nMailWeight;
                            item1.MAILDATE = strSendDate;
                            item1.WRTNO = nWRTNO;

                            int result1 = heaJepsuService.UpdateMailInfo(item1);

                            if (result1 < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                MessageBox.Show("방문수령일자 Data 등록시 오류발생(HEA_JEPSU)", "전산실 연락요망", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                        }
                    }
                    else if (strDel == "True")
                    {
                    }

                    if (!strRecvDate.IsNullOrEmpty())
                    {
                        //2019-02-08 최종마지막 작업내역이 저장되도록 요청함 - 최종숙 팀장요청
                        //수령일자 및 수령일자 등록사번 저장 쿼리 추가로 함께 요청함
                        //내원방문수령일자 등록
                        HEA_JEPSU item2 = new HEA_JEPSU();

                        item2.RECVDATE = strRecvDate;
                        item2.RECVSABUN = clsType.User.IdNumber.To<long>();
                        item2.WRTNO = nWRTNO;

                        int result = heaJepsuService.UpdateRecVDate(item2);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("방문수령일자 Data 등록시 오류발생(HEA_JEPSU)", "전산실 연락요망", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                chkAll2.Checked = false;

                eBtnClick(btnSearch, new EventArgs());
                eBtnClick(btnSearch2, new EventArgs());
            }
            else if (sender == btnCancel)
            {
                sp.Spread_All_Clear(SS1);
            }
            else if (sender == btnSendDateDelete)
            {
                long nWrtNo = 0;

                if (MessageBox.Show("좌측명단의 선택된 수검자 발송일자를 삭제 하시겠습니까?", "확인요망", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
                {
                    if (SSList.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        nWrtNo = SSList.ActiveSheet.Cells[i, 12].Text.To<long>(); 

                        int result = heaJepsuService.UpdateMailDatebyWrtNo(nWrtNo);

                        if (result < 0)
                        {
                            MessageBox.Show("등기발송일자 UPDATE시 오류 발생(HEA_JEPSU)", "전산실 연락요망", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        int result1 = comHpcLibBService.DeletebyWrtNo(nWrtNo);

                        if (result1 < 0)
                        {
                            MessageBox.Show("Data 삭제 중 오류!! 점검요망!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        SSList.ActiveSheet.Cells[i, 6].Text = "";
                    }
                }
                chkAll.Checked = false;
                eBtnClick(btnSearch, new EventArgs());
            }
            else if (sender == btnRecvDateDelete)
            {
                long nWRTNO = 0;

                if (MessageBox.Show("좌측명단의 선택된 수검자 수령일자를 삭제 하시겠습니까?", "확인요망", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
                {
                    if (SSList.ActiveSheet.Cells[i, 0].Text.Trim() == "True")
                    {
                        nWRTNO = SSList.ActiveSheet.Cells[i, 12].Text.To<long>(); 

                        int result = heaJepsuService.UpdateRevcDatebyWrtNo(nWRTNO);

                        if (result < 0)
                        {
                            MessageBox.Show("수령일자 UPDATE시 오류 발생(HEA_JEPSU)", "전산실 연락요망", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        SSList.ActiveSheet.Cells[i, 6].Text = "";
                    }
                }
                chkAll.Checked = false;
                eBtnClick(btnSearch, new EventArgs());
            }
            else if (sender == btnMailWeightSave)
            {
                long nMailWeight = 0;
                long nWRTNO = 0;

                for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
                {
                    nWRTNO = SSList.ActiveSheet.Cells[i, 12].Text.To<long>();
                    nMailWeight = SSList.ActiveSheet.Cells[i, 5].Text.To<int>();

                    int result = heaJepsuService.UpdateMailWeightbyWrtNo(nMailWeight, nWRTNO);

                    if (result < 0)
                    {
                        MessageBox.Show("삭제중 오류 발생!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("저장 완료!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
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

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SSList)
            {
                int nRow = 0;
                long nWRTNO = 0;
                int nMailWeight = 0;
                string strPrtDate = "";
                string strRecvDate = "";
                string strMailDate = "";
                string strMsg = "";
                string strGbReSend = "";

                if (e.RowHeader == false && e.ColumnHeader == false)
                {
                    if (e.Column != 0)
                    {
                        nRow = SS1.ActiveSheet.NonEmptyRowCount;
                        nWRTNO = SSList.ActiveSheet.Cells[e.Row, 12].Text.To<long>();
                        nMailWeight = SSList.ActiveSheet.Cells[e.Row, 5].Text.To<int>();
                        strPrtDate = SSList.ActiveSheet.Cells[e.Row, 6].Text.Trim();
                        strRecvDate = SSList.ActiveSheet.Cells[e.Row, 7].Text.Trim();
                        strMailDate = SSList.ActiveSheet.Cells[e.Row, 8].Text.Trim();

                        strGbReSend = "N";
                        if (strPrtDate.IsNullOrEmpty())
                        {
                            MessageBox.Show("아직 출력작업이 완료되지 안았습니다.", "작성불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        if (!strMailDate.IsNullOrEmpty())
                        {
                            strMsg = "이미 명단작성이 완료되었습니다." + "\r\n";
                            strMsg += "다시 명단작성을 작성하시겠습니까?";
                            if (MessageBox.Show(strMsg, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            {
                                return;
                            }
                            strGbReSend = "Y";
                        }

                        if (!strRecvDate.IsNullOrEmpty())
                        {
                            if (MessageBox.Show("이미 수령일자가 등록되었습니다." + "\r\n" + "다시 명단작성을 작성하시겠습니까?", "작성불가", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            {
                                return;
                            }
                            strGbReSend = "Y";
                        }

                        //이미 명단에 있으면 제외됨
                        for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                        {
                            if (nWRTNO == SS1.ActiveSheet.Cells[i, 6].Text.To<long>())
                            {
                                return;
                            }
                        }

                        HEA_JEPSU lst = heaJepsuService.GetMailCodebyWrtNo(nWRTNO);

                        if (!lst.IsNullOrEmpty())
                        {
                            nRow += 1;
                            if (SS1.ActiveSheet.RowCount < nRow)
                            {
                                SS1.ActiveSheet.RowCount = nRow;
                            }
                            SS1.ActiveSheet.Cells[nRow - 1, 1].Text = lst.SNAME;
                            if (strGbReSend == "Y")
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 2].Text = "";
                                SS1.ActiveSheet.Cells[nRow - 1, 3].Text = dtpSendDate.Text;
                            }
                            else
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 2].Text = lst.RECVDATE;
                                SS1.ActiveSheet.Cells[nRow - 1, 3].Text = lst.MAILDATE.To<string>();
                            }

                            SS1.ActiveSheet.Cells[nRow - 1, 4].Text = lst.MAILCODE.Trim();
                            SS1.ActiveSheet.Cells[nRow - 1, 5].Text = lst.JUSO.Trim();
                            SS1.ActiveSheet.Cells[nRow - 1, 6].Text = nWRTNO.To<string>();
                            SS1.ActiveSheet.Cells[nRow - 1, 7].Text = nMailWeight.To<string>();
                        }
                    }
                }
            }
            else if (sender == SS1)
            {
                if (e.ColumnHeader == false && e.RowHeader == false)
                {
                    if (e.Column != 0 && e.Column != 2 && e.Column != 3)
                    {
                        if (e.Row >= 0)
                        {
                            if (SS1.ActiveSheet.Cells[e.Row, 3].Text.Trim() == "")
                            {
                                SS1.ActiveSheet.Rows[e.Row].Remove();
                                SS1.ActiveSheet.RowCount = SS1.ActiveSheet.NonEmptyRowCount;
                            }
                        }
                    }

                    if (e.Column == 2)
                    {
                        frmCalendar2 frm = new frmCalendar2();
                        frm.StartPosition = FormStartPosition.CenterParent;
                        frm.ShowDialog();

                        ((FarPoint.Win.Spread.FpSpread)sender).ActiveSheet.Cells[e.Row, 2].Text = clsPublic.GstrCalDate;
                        clsPublic.GstrCalDate = "";
                    }
                }
            }
        }
    }
}
