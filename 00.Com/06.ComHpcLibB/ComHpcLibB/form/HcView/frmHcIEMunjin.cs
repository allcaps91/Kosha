using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcIEMunjin.cs
/// Description     : 인터넷문진표 Data View
/// Author          : 이상훈
/// Create Date     : 2020-06-23
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmIEMunjin.frm(FrmIEMunjin)" />

namespace ComHpcLibB
{
    public partial class frmHcIEMunjin : Form
    {
        HicIeMunjinNewService hicIeMunjinNewService = null;
        ComHpcLibBService comHpcLibBService = null;
        HicJepsuService hicJepsuService = null;
        HicIeMunjinViewService hicIeMunjinViewService = null;
        BasPatientService basPatientService = null;
        frmHcPanInternetMunjin_New FrmHcPanInternetMunjin_New = null;

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        //
        public delegate void SetJepsuGstrValue(string GstrClass, string GstrBan, string GstrBun, string GstrIEMunjin);
        public event SetJepsuGstrValue rSetJepsuGstrValue;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();

        long FnCnt;
        long FnMunID;
        string FstrOSVer;
        string FstrForm;
        string FstrPtno;
        string FstrGjJong;
        long FnWrtNo;
        long FnJepsuNo;

        string FstrClass;
        string FstrBan;
        string FstrBun;

        string FstrJumin1;
        string FstrSName;
        long FnIEMunNo;
        string FstrPtNo;
        string FstrJong;

        string FstrViewKey;

        public frmHcIEMunjin()
        {
            InitializeComponent();            
            SetEvents();
        }

        public frmHcIEMunjin(string strJumin1 = "", string strSName = "", long nWrtNo = 0, long nIEMunNo = 0, string strPtNo = "", string strJong = "", string strViewKey = "")
        {
            InitializeComponent();

            FstrJumin1 = strJumin1;
            FstrSName = strSName;
            FnWrtNo = nWrtNo;
            FnIEMunNo = nIEMunNo;
            FstrPtNo = strPtNo;
            FstrJong = strJong;

            FstrViewKey = strViewKey;

            SetEvents();
        }

        void SetEvents()
        {
            hicIeMunjinNewService = new HicIeMunjinNewService();
            comHpcLibBService = new ComHpcLibBService();
            hicJepsuService = new HicJepsuService();
            hicIeMunjinViewService = new HicIeMunjinViewService();
            basPatientService = new BasPatientService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);            
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnSelect.Click += new EventHandler(eBtnClick);
            this.btnLink.Click += new EventHandler(eBtnClick);
            this.btnDelete.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.timer1.Tick += new EventHandler(eTimerTick);
            this.timer2.Tick += new EventHandler(eTimerTick);

            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            clsQuery.READ_PC_CONFIG(clsDB.DbCon);
            ComFunc.ReadSysDate(clsDB.DbCon);
            sp.Spread_All_Clear(SS1);

            dtpFrDate.Text = VB.Left(clsPublic.GstrSysDate, 4) + "-01-01";
            dtpToDate.Text = clsPublic.GstrSysDate;

            txtSName.Text = "";
            txtLtdCode.Text = "";

            if (FnWrtNo == 0)
            {
                if (FnIEMunNo > 0)
                {
                    FnWrtNo = FnIEMunNo;
                }
            }

            txtSName.Text = FstrSName;

            if (clsHcVariable.GbHicAdminSabun == true)
            {
                btnDelete.Visible = true;
                btnLink.Visible = true;
            }
            if (clsType.User.IdNumber == "31197" || clsType.User.IdNumber == "26080")
            {
                btnDelete.Visible = true;
                btnLink.Visible = true; ;
            }
            if (clsType.User.IdNumber == "18551" || clsType.User.IdNumber == "20184")
            {
                btnDelete.Visible = true;
                btnLink.Visible = true;
            }

            FstrOSVer = hf.fn_Find_OS_version().ToUpper();

            if (!txtSName.Text.IsNullOrEmpty() || FnWrtNo > 0)
            {
                eBtnClick(btnSearch, new EventArgs());
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
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
            else if (sender == btnSearch)
            {
                int nREAD = 0;
                int nRow = 0;
                string strJong = "";
                string strPtNo = "";
                string strSname = "";
                string strBirth = "";
                string strHPhone = "";
                string strFrDate = "";
                string strToDate = "";
                string strLtdName = "";
                int result = 0;

                sp.Spread_All_Clear(SS1);
                nRow = 0;

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;
                strSname = txtSName.Text.Trim();
                strLtdName = VB.Pstr(txtLtdCode.Text, ".", 2);

                List<HIC_IE_MUNJIN_NEW> list = hicIeMunjinNewService.GetItembyWrtNoMunDate(FnWrtNo, strFrDate, strToDate, FstrPtno, strSname, strLtdName, FstrGjJong);

                nREAD = list.Count;
                SS1.ActiveSheet.RowCount = nREAD;
                if (nREAD >= 500)
                {
                    MessageBox.Show("자료가 500건을 초과하여 500개만 표시됩니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    nREAD = 500;
                    SS1.ActiveSheet.RowCount = 500;
                }

                for (int i = 0; i < nREAD; i++)
                {
                    nRow += 1;
                    strJong = "";
                    if (list[i].GBMUN4 == "Y")
                    {
                        strJong = "학생";
                    }
                    else
                    {
                        if (list[i].GBMUN1 == "Y")
                        {
                            strJong += "1차" + ",";
                        }
                        if (list[i].GBMUN2 == "Y")
                        {
                            strJong += "암" + ",";
                        }
                        if (list[i].GBMUN3 == "Y")
                        {
                            strJong += "특수" + ",";
                        }
                    }

                    SS1.ActiveSheet.Cells[i, 1].Text = strJong;
                    SS1.ActiveSheet.Cells[i, 2].Text = list[i].SNAME;
                    SS1.ActiveSheet.Cells[i, 3].Text = list[i].BIRTH;
                    SS1.ActiveSheet.Cells[i, 4].Text = list[i].AGE + "/" + list[i].SEX;
                    SS1.ActiveSheet.Cells[i, 5].Text = list[i].LTDNAME;
                    SS1.ActiveSheet.Cells[i, 6].Text = list[i].TEL;
                    SS1.ActiveSheet.Cells[i, 7].Text = list[i].HPHONE;
                    SS1.ActiveSheet.Cells[i, 8].Text = list[i].CLASS.ToString();
                    SS1.ActiveSheet.Cells[i, 9].Text = list[i].BAN.ToString();
                    SS1.ActiveSheet.Cells[i, 10].Text = list[i].BUN.ToString();
                    SS1.ActiveSheet.Cells[i, 11].Text = list[i].RECVFORM;
                    if (VB.InStr(SS1.ActiveSheet.Cells[i, 11].Text, "12001") > 0)
                    {
                        SS1.ActiveSheet.Cells[i, 11].Text = SS1.ActiveSheet.Cells[i, 11].Text.Replace("12001", "12001,12005");
                    }
                    SS1.ActiveSheet.Cells[i, 13].Text = list[i].ROWID;
                    SS1.ActiveSheet.Cells[i, 14].Text = "NEW";
                    SS1.ActiveSheet.Cells[i, 15].Text = list[i].PTNO;

                    if (list[i].PTNO.IsNullOrEmpty())
                    {
                        strSname = list[i].SNAME;
                        strBirth = list[i].BIRTH;
                        strHPhone = list[i].HPHONE;
                        strPtNo = fn_GET_Mujin_Ptno(strSname, strBirth, strHPhone);
                        if (!strPtNo.IsNullOrEmpty())
                        {
                            clsDB.setBeginTran(clsDB.DbCon);

                            result = hicIeMunjinNewService.UpdatePtNobyRowId(strPtNo, list[i].ROWID);

                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }
                            clsDB.setCommitTran(clsDB.DbCon);
                            SS1.ActiveSheet.Cells[i, 15].Text = strPtNo;
                        }
                    }
                }

                if (FnWrtNo > 0 && nRow == 1)
                {
                    eSpdDClick(SS1, new CellClickEventArgs(new SpreadView(), 0, 0, 0, 0, new MouseButtons(), false, false));
                }
            }
            else if (sender == btnSelect)
            {
                clsHcVariable.GstrIEMunjin = "";

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    if (SS1.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        FstrClass = SS1.ActiveSheet.Cells[i, 8].Text;
                        FstrBan = SS1.ActiveSheet.Cells[i, 9].Text;
                        FstrBun = SS1.ActiveSheet.Cells[i, 10].Text;
                        clsHcVariable.GstrIEMunjin = SS1.ActiveSheet.Cells[i, 13].Text;

                        rSetJepsuGstrValue(FstrClass, FstrBan, FstrBun, clsHcVariable.GstrIEMunjin);

                        break;
                    }
                }
                this.Close();
            }
            else if (sender == btnLink)
            {
                int nCNT = 0;
                string strROWID = "";
                string strPtNo = "";
                string strGjjong = "";
                string strRecvForm = "";
                long nMunNo = 0;
                string strGbn = "";
                string strClass = "";
                int result = 0;

                if (FnJepsuNo == 0)
                {
                    return;
                }

                nCNT = 0;
                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    if (SS1.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        nCNT += 1;
                        strROWID = SS1.ActiveSheet.Cells[i, 13].Text;
                    }
                }

                if (nCNT == 0)
                {
                    MessageBox.Show("연계할 인터넷문진을 선택하세요", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (nCNT == 1)
                {
                    MessageBox.Show("연계할 인터넷문진 1개만 선택하세요", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                HIC_IE_MUNJIN_NEW list = hicIeMunjinNewService.GetWrtNoRecvFormbyRowId(strROWID);

                nMunNo = list.WRTNO;
                strRecvForm = list.RECVFORM;

                if (strRecvForm.IsNullOrEmpty())
                {
                    MessageBox.Show("문진항목 분류가 공란이어서 연계가 불가능합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                HIC_JEPSU list2 = hicJepsuService.GetItembyWrtNo(FnJepsuNo);

                strGjjong = list2.GJJONG;
                strPtNo = list2.PTNO;
                strGbn = list2.GBN;
                strClass = list2.CLASS.ToString();

                if (strGjjong != "56")
                {
                    MessageBox.Show("학생검진 인터넷문진표만 연계가 가능합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsDB.setBeginTran(clsDB.DbCon);

                result = hicJepsuService.UpdateIEMunNobyWrtNo(FnJepsuNo);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("인터넷문진 번호 갱신중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; 
                }

                result = hicJepsuService.UpdatePtNoRecvFormbyRowId(strPtNo, strRecvForm, FnJepsuNo, strROWID);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("인터넷문진 번호 갱신중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                MessageBox.Show("저장 완료!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (sender == btnDelete)
            {
                int nREAD = 0;
                int nRow = 0;
                string strROWID = "";
                string strMunDate = "";
                string strInjekData = "";
                string strRecvForm = "";
                string strID_List = "";
                List<string> strID_List_2 = new List<string>();
                int result = 0;
                DataTable dt = null;
                string SqlErr = ""; //에러문 받는 변수
                int intRowAffected = 0; //변경된 Row 받는 변수

                StringBuilder SQL = new StringBuilder();

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    if (SS1.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        if (SS1.ActiveSheet.Cells[i, 14].Text != "NEW")
                        {
                            MessageBox.Show(i + 1 + "번줄 새로운 인터넷문진표가 아닙니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                if (MessageBox.Show("선택한 문진표를 삭제하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                //웹서버 MySql접속
                clsDbMySql.DBConnect("", "", "psmh", "psmh", "psmh2");

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    if (SS1.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        strROWID = SS1.ActiveSheet.Cells[i, 0].Text;

                        HIC_IE_MUNJIN_NEW list = hicIeMunjinNewService.GetItembyRowId(strROWID);

                        if (!list.IsNullOrEmpty())
                        {
                            strMunDate = list.MUNDATE;
                            strInjekData = list.INJEKDATA;
                            strRecvForm = list.RECVFORM;

                            SQL.Clear();
                            SQL.AppendLine("SELECT ID, SENDDATA                        ");
                            SQL.AppendLine("  FROM QUESTION.EXAM_MUNJIN                ");
                            SQL.AppendLine(" WHERE SENDTIME >= :SENDTIME               ");
                            SQL.AppendLine("   AND SENDDATA = :SENDDATA                ");
                            SQL.AppendLine("   AND RECVFORM = :RECVFORM                ");
                            dt = clsDbMySql.GetDataTable(SQL.ToString().Trim());

                            if (dt.IsNullOrEmpty())
                            {
                                ComFunc.MsgBox("조회중 오류가 발생했습니다");
                                return;
                            }

                            if (dt.Rows.Count > 0)
                            {
                                nREAD = dt.Rows.Count;
                                strID_List = "";
                                strID_List_2.Clear();
                                for (int j = 0; j < nREAD; j++)
                                {
                                    strID_List += dt.Rows[i]["ID"].ToString().Trim() + ",";
                                    strID_List_2.Add(dt.Rows[i]["ID"].ToString().Trim());
                                }
                            }

                            if (!strID_List.IsNullOrEmpty())
                            {
                                clsDbMySql.setBeginTran();

                                strID_List = VB.Left(strID_List, strID_List.Length - 1);

                                SQL.Clear();
                                SQL.AppendLine(" DELETE FROM QUESTION.EXAM_MUNJIN WHERE ID IN (" + strID_List + ") ");
                                if (clsDbMySql.ExecuteNonQuery(SQL.ToString().Trim()) == false)
                                {
                                    clsDbMySql.setRollbackTran();
                                }

                                SQL.Clear();
                                SQL.AppendLine(" DELETE FROM QUESTION.TBL_COMMON WHERE ID IN (" + strID_List + ") ");
                                if (clsDbMySql.ExecuteNonQuery(SQL.ToString().Trim()) == false)
                                {
                                    clsDbMySql.setRollbackTran();
                                }

                                SQL.Clear();
                                SQL.AppendLine(" DELETE FROM QUESTION.TBL_CANCER WHERE ID IN (" + strID_List + ") ");
                                if (clsDbMySql.ExecuteNonQuery(SQL.ToString().Trim()) == false)
                                {
                                    clsDbMySql.setRollbackTran();
                                }

                                SQL.Clear();
                                SQL.AppendLine(" DELETE FROM QUESTION.TBL_MOUTH WHERE ID IN (" + strID_List + ") ");
                                if (clsDbMySql.ExecuteNonQuery(SQL.ToString().Trim()) == false)
                                {
                                    clsDbMySql.setRollbackTran();
                                }

                                SQL.Clear();
                                SQL.AppendLine(" DELETE FROM QUESTION.TBL_LIFE WHERE ID IN (" + strID_List + ") ");
                                if (clsDbMySql.ExecuteNonQuery(SQL.ToString().Trim()) == false)
                                {
                                    clsDbMySql.setRollbackTran();
                                }

                                SQL.Clear();
                                SQL.AppendLine(" DELETE FROM QUESTION.TBL_SPECIAL WHERE ID IN (" + strID_List + ") ");
                                if (clsDbMySql.ExecuteNonQuery(SQL.ToString().Trim()) == false)
                                {
                                    clsDbMySql.setRollbackTran();
                                }

                                SQL.Clear();
                                SQL.AppendLine(" DELETE FROM QUESTION.TBL_VITAL WHERE ID IN (" + strID_List + ") ");
                                if (clsDbMySql.ExecuteNonQuery(SQL.ToString().Trim()) == false)
                                {
                                    clsDbMySql.setRollbackTran();
                                }

                                SQL.Clear();
                                SQL.AppendLine(" DELETE FROM QUESTION.TBL_NIGHT WHERE ID IN (" + strID_List + ") ");
                                if (clsDbMySql.ExecuteNonQuery(SQL.ToString().Trim()) == false)
                                {
                                    clsDbMySql.setRollbackTran();
                                }

                                SQL.Clear();
                                SQL.AppendLine(" DELETE FROM QUESTION.TBL_ELEMENTARY WHERE ID IN (" + strID_List + ") ");
                                if (clsDbMySql.ExecuteNonQuery(SQL.ToString().Trim()) == false)
                                {
                                    clsDbMySql.setRollbackTran();
                                }

                                SQL.Clear();
                                SQL.AppendLine(" DELETE FROM QUESTION.TBL_ELEMENTARY_CONTENTS WHERE ID IN (" + strID_List + ") ");
                                if (clsDbMySql.ExecuteNonQuery(SQL.ToString().Trim()) == false)
                                {
                                    clsDbMySql.setRollbackTran();
                                }

                                SQL.Clear();
                                SQL.AppendLine(" DELETE FROM QUESTION.TBL_MIDDLEHIGH WHERE ID IN (" + strID_List + ") ");
                                if (clsDbMySql.ExecuteNonQuery(SQL.ToString().Trim()) == false)
                                {
                                    clsDbMySql.setRollbackTran();
                                }

                                SQL.Clear();
                                SQL.AppendLine(" DELETE FROM QUESTION.TBL_MIDDLEHIGH_CONTENTS WHERE ID IN (" + strID_List + ") ");
                                if (clsDbMySql.ExecuteNonQuery(SQL.ToString().Trim()) == false)
                                {
                                    clsDbMySql.setRollbackTran();
                                }

                                SQL.Clear();
                                SQL.AppendLine(" DELETE FROM QUESTION.TBL_DRINK WHERE ID IN (" + strID_List + ") ");
                                if (clsDbMySql.ExecuteNonQuery(SQL.ToString().Trim()) == false)
                                {
                                    clsDbMySql.setRollbackTran();
                                }

                                SQL.Clear();
                                SQL.AppendLine(" DELETE FROM QUESTION.TBL_SMOKE WHERE ID IN (" + strID_List + ") ");
                                if (clsDbMySql.ExecuteNonQuery(SQL.ToString().Trim()) == false)
                                {
                                    clsDbMySql.setRollbackTran();
                                }

                                SQL.Clear();
                                SQL.AppendLine(" DELETE FROM QUESTION.TBL_EXERCISE WHERE ID IN (" + strID_List + ") ");
                                if (clsDbMySql.ExecuteNonQuery(SQL.ToString().Trim()) == false)
                                {
                                    clsDbMySql.setRollbackTran();
                                }

                                SQL.Clear();
                                SQL.AppendLine(" DELETE FROM QUESTION.TBL_FAT WHERE ID IN (" + strID_List + ") ");
                                if (clsDbMySql.ExecuteNonQuery(SQL.ToString().Trim()) == false)
                                {
                                    clsDbMySql.setRollbackTran();
                                }

                                SQL.Clear();
                                SQL.AppendLine(" DELETE FROM QUESTION.TBL_GLOOM WHERE ID IN (" + strID_List + ") ");
                                if (clsDbMySql.ExecuteNonQuery(SQL.ToString().Trim()) == false)
                                {
                                    clsDbMySql.setRollbackTran();
                                }

                                SQL.Clear();
                                SQL.AppendLine(" DELETE FROM QUESTION.TBL_HEALTH WHERE ID IN (" + strID_List + ") ");
                                if (clsDbMySql.ExecuteNonQuery(SQL.ToString().Trim()) == false)
                                {
                                    clsDbMySql.setRollbackTran();
                                }

                                SQL.Clear();
                                SQL.AppendLine(" DELETE FROM QUESTION.tbl_sickness WHERE ID IN (" + strID_List + ") ");
                                if (clsDbMySql.ExecuteNonQuery(SQL.ToString().Trim()) == false)
                                {
                                    clsDbMySql.setRollbackTran();
                                }

                                SQL.Clear();
                                SQL.AppendLine(" DELETE FROM QUESTION.TBL_BASIC_CONTENTS WHERE ID IN (" + strID_List + ") ");
                                if (clsDbMySql.ExecuteNonQuery(SQL.ToString().Trim()) == false)
                                {
                                    clsDbMySql.setRollbackTran();
                                }

                                clsDbMySql.setCommitTran();
                            }
                        }

                        dt.Dispose();
                        dt = null;

                        clsDB.setBeginTran(clsDB.DbCon);

                        //삭제로그를 저장
                        result = comHpcLibBService.InsertHicIEMunjinDel(strROWID, clsType.User.IdNumber);

                        if (result < 0)
                        {
                            MessageBox.Show("삭제로그 저장시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }

                        result = comHpcLibBService.DeleteHicIEMunjinNewbyRowId(strROWID);

                        if (result < 0)
                        {
                            MessageBox.Show("삭제로그 저장시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }

                        clsDB.setCommitTran(clsDB.DbCon);
                    }
                }

                clsDbMySql.DisDBConnect();

                MessageBox.Show("삭제 완료", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        string fn_GET_Mujin_Ptno(string argSName, string argBirth, string argHPhone)
        {
            string rtnVal = "";

            BAS_PATIENT list = basPatientService.GetPaNobySName(argSName, argBirth, argHPhone);

            if (list.IsNullOrEmpty())
            {
                return rtnVal;
            }
            else
            {
                rtnVal = list.PANO;
            }

            return rtnVal;
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                if (SS1.ActiveSheet.Cells[e.Row, 14].Text == "NEW")
                {
                    FstrViewKey = SS1.ActiveSheet.Cells[e.Row, 13].Text;
                    FstrForm = SS1.ActiveSheet.Cells[e.Row, 11].Text;
                    FstrPtno = SS1.ActiveSheet.Cells[e.Row, 15].Text;
                    timer2.Enabled = false;
                    timer1.Enabled = true;
                }
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtLtdCode)
            {
                if (e.KeyChar == (char)13)
                {
                    if (txtLtdCode.Text.Length >= 2)
                    {
                        eBtnClick(btnLtdCode, new EventArgs());
                    }
                }
            }
        }

        void eTimerTick(object sender, EventArgs e)
        {
            if (sender == timer1)
            {
                string strViewId = "";
                int result = 0;

                FnCnt = 0;
                FnMunID = 0;
                timer1.Enabled = false;

                strViewId = hicIeMunjinViewService.GetViewIdbyViewKey(FstrViewKey, clsPublic.GstrSysDate);

                if (strViewId.IsNullOrEmpty())
                {
                    clsDB.setBeginTran(clsDB.DbCon);

                    result = hicIeMunjinViewService.Insert(clsPublic.GstrSysDate, FstrViewKey);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("HIC_IE_MUNJIN_VIEW  저장 중 오류 발생!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    clsDB.setCommitTran(clsDB.DbCon);
                    Thread.Sleep(1000);
                }

                timer2.Enabled = true;
            }
            else if (sender == timer2)
            {
                string strURL = "";
                string strMunDate = "";
                string strRecvForm = "";
                string strSendData = "";
                string strMunjinRes = "";
                string strJusoData = "";
                string strSite = "";                
                string strYear = "";
                string strIEVer = "";

                timer2.Enabled = false;
                FnCnt += 1;
                if (FnCnt >= 10)
                {
                    MessageBox.Show("인터넷문진 연동 프로그램이 응답이 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string strViewId = hicIeMunjinViewService.GetViewIdbyViewKey(FstrViewKey, clsPublic.GstrSysDate);

                if (!strViewId.IsNullOrEmpty())
                {
                    FnMunID = long.Parse(strViewId);
                }

                Thread.Sleep(500);

                //아직 연동이 되지 않았으면 대기함
                if (FnMunID == 0)
                {
                    timer2.Enabled = true;
                    return;
                }

                var ieVersion = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Internet Explorer").GetValue("Version");

                //XP,VISTA는 IE Old버전으로 설정
                if (VB.InStr(FstrOSVer, "XP") > 0)
                {
                    strIEVer = "8.0";
                }
                if (VB.InStr(FstrOSVer, "VISTA") > 0)
                {
                    strIEVer = "8.0";
                }
                if (VB.InStr(FstrOSVer, "98") > 0)
                {
                    strIEVer = "8.0";
                }
                if (VB.InStr(FstrOSVer, "10") > 0 || VB.InStr(FstrOSVer, "7") > 0)
                {
                    //strIEVer = "8.0";
                    strIEVer = "11.0";
                }

                //종검2층 Windows 7
                if (VB.Left(clsType.PC_CONFIG.IPAddress, 10) == "192.168.41.80")
                {
                    strIEVer = "8.0";
                }

                strYear = VB.Left(hicIeMunjinNewService.GetMunDatebyPtNo(FstrPtno), 4);

                
                //(1) IE 9.0이상 버전
                if (long.Parse(VB.Pstr(strIEVer, ".", 1)) >= 9)
                {
                    //FrmHcPanInternetMunjin_New = new frmHcPanInternetMunjin_New(FnMunID, FstrForm, FstrPtno);
                    //FrmHcPanInternetMunjin_New.ShowDialog();
                    //FrmHcPanInternetMunjin_New = null;

                    FrmHcPanInternetMunjin_New = new frmHcPanInternetMunjin_New(FnMunID, FstrForm, FstrPtno);
                    FrmHcPanInternetMunjin_New.Show();
                }

                // (2) 구글크롬
                else if (Directory.Exists(@"C:\Program Files\Google\Chrome\Application"))
                {
                    if (string.Compare(strYear, "2019") < 0)
                    {
                        VB.Shell(@"C:\Program Files\Google\Chrome\Application\chrome.exe " + "http://www.pohangsmh.co.kr/web_question/general/g_view_2018.html?m_id=" + FnMunID + "&Chk=" + FstrForm, "NormalFocus");
                    }
                    else
                    {
                        VB.Shell(@"C:\Program Files\Google\Chrome\Application\chrome.exe " + "http://www.pohangsmh.co.kr/web_question/general/g_view.html?m_id=" + FnMunID + "&Chk=" + FstrForm, "NormalFocus");
                    }
                }
                else if (Directory.Exists(@"C:\Program Files (x86)\Google\Chrome\Application"))
                {
                    if (string.Compare(strYear, "2019") < 0)
                    {
                        VB.Shell(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe " + "http://www.pohangsmh.co.kr/web_question/general/g_view_2018.html?m_id=" + FnMunID + "&Chk=" + FstrForm, "NormalFocus");
                    }
                    else
                    {
                        VB.Shell(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe " + "http://www.pohangsmh.co.kr/web_question/general/g_view.html?m_id=" + FnMunID + "&Chk=" + FstrForm, "NormalFocus");
                    }
                }
                //(3) 스윙브라우저
                else if (Directory.Exists(@"C:\Users\user\AppData\Local\SwingBrowser\Application"))
                {
                    VB.Shell(@"C:\Users\user\AppData\Local\SwingBrowser\Application\swing.exe " + "http://www.pohangsmh.co.kr/web_question/general/g_view.html?m_id=" + FnMunID + "&Chk=" + FstrForm, "NormalFocus");
                }
                //(4) 기본 브라우저로 표시
                else
                {
                    strURL = @"C:\Program Files\Internet Explorer\iexplore.exe " + "http://www.pohangsmh.co.kr/web_question/general/g_view.html?m_id=" + FnMunID + "&Chk=" + FstrForm;
                    VB.Shell(strURL, "NormalFocus");
                }

                Application.DoEvents();

                if (FnWrtNo > 0)
                {
                    this.Close();
                    return;
                }
            }
        }
    }
}
