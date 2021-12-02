using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHcGaJepsuIntMunjinYN.cs
/// Description     : 가접수대상자 인터넷문진표 작성여부 조회
/// Author          : 이상훈
/// Create Date     : 2020-07-23
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm가접수문진작성조회.frm(Frm가접수문진작성조회)" />

namespace HC_Main
{
    public partial class frmHcGaJepsuIntMunjinYN : Form
    {
        HicJepsuWorkService hicJepsuWorkService = null;
        HicSunapdtlWorkService hicSunapdtlWorkService = null;
        HicIeMunjinNewService hicIeMunjinNewService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hc = new clsHcFunc();
        ComFunc cf = new ComFunc();
        clsAlimTalk Alim = new clsAlimTalk();

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        string fstrErr; //루틴에러 체크

        public frmHcGaJepsuIntMunjinYN()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicJepsuWorkService = new HicJepsuWorkService();
            hicSunapdtlWorkService = new HicSunapdtlWorkService();
            hicIeMunjinNewService = new HicIeMunjinNewService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnTalkSend.Click += new EventHandler(eBtnClick); 
            this.chkAll.Click += new EventHandler(eChkClick);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            txtLtdCode.Text = "";
            dtpFrDate.Text = Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-7).ToShortDateString();
            dtpToDate.Text = clsPublic.GstrSysDate;
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
                int nRead1 = 0;
                int nRow = 0;
                string strOK = "";
                string strMunDate = "";
                string strRecvForm = "";
                string strMList = "";
                string strCHK20003 = "";
                string strMunDate1 = "";
                string strFrDate = "";
                string strToDate = "";
                long nLtdCode = 0;

                string strMListName = "";

                Cursor.Current = Cursors.WaitCursor;

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;
                if (txtLtdCode.Text.IsNullOrEmpty())
                {
                    nLtdCode = 0;
                }
                else
                {
                    nLtdCode = long.Parse(VB.Pstr(txtLtdCode.Text, ".", 1));
                }

                sp.Spread_All_Clear(SS1);
                Application.DoEvents();
                //SS1.ActiveSheet.RowCount = 30;
                strOK = "";

                strMunDate = VB.Left(dtpFrDate.Text, 4) + "-01-01";

                //자료를 SELECT
                List<HIC_JEPSU_WORK> list = hicJepsuWorkService.GetItembyJepDateGjJongLtdCode2(strFrDate, strToDate, nLtdCode);

                nREAD = list.Count;
                progressBar1.Maximum = nREAD;
                if (nREAD > 0)
                {
                    for (int i = 0; i < nREAD; i++)
                    {
                        strMList = "";
                        //작성대상문진표 확인
                        switch (list[i].GJJONG)
                        {
                            case "11":
                            case "12":
                            case "13":
                            case "14":
                                strMList = fn_MList_Add(strMList, "12001"); //건강검진
                                break;
                            case "41":
                            case "42":
                            case "43":
                                strMList = fn_MList_Add(strMList, "12001"); //건강검진
                                break;
                            case "31":
                            case "35":
                                strMList = fn_MList_Add(strMList, "12003"); //암검진
                                break;
                            default:
                                break;
                        }

                        if (!list[i].UCODES.IsNullOrEmpty())
                        {
                            strMList = fn_MList_Add(strMList, "12001"); //공통문진표
                            strMList = fn_MList_Add(strMList, "20002"); //특수검진
                            //분진
                            strCHK20003 = "";
                            strCHK20003 = fn_READ_UCODES_WORK(list[i].PANO.ToString());
                            //야간작업
                            if (VB.InStr(list[i].UCODES, "V01") > 0)
                            {
                                strMList = fn_MList_Add(strMList, "30001");
                            }
                            if (VB.InStr(list[i].UCODES, "V02") > 0)
                            {
                                strMList = fn_MList_Add(strMList, "30001");
                            }
                        }

                        if (list[i].GJJONG == "11")
                        {
                            if (list[i].AGE == 40 || list[i].AGE == 50 || list[i].AGE == 60 || list[i].AGE == 70)
                            {
                                strMList = fn_MList_Add(strMList, "20004,20005,20006,20007,20008");
                            }

                            if (list[i].AGE >= 66 || list[i].AGE % 2 == 0)
                            {
                                strMList = fn_MList_Add(strMList, "20011");
                            }

                            if (list[i].AGE == 20 || list[i].AGE == 30 || list[i].AGE == 40 || list[i].AGE == 50 || list[i].AGE == 60 || list[i].AGE == 70)
                            {
                                strMList = fn_MList_Add(strMList, "20012");
                            }
                        }

                        strOK = "";
                        strRecvForm = "";
                        strMunDate1 = "";

                        HIC_IE_MUNJIN_NEW list2 = hicIeMunjinNewService.GetRecvFormbyMunDatePtNo(strMunDate, list[i].PTNO);

                        if (!list2.IsNullOrEmpty())
                        {
                            strRecvForm = hb.IEMunjin_Name_Display(list2.RECVFORM);
                            if (strRecvForm.Length > 1)
                            {
                                strRecvForm = VB.Mid(strRecvForm, 1, strRecvForm.Length - 1);
                            }
                            strMunDate1 = list2.MUNDATE;
                            strOK = "OK";
                        }

                        if (rdoGubun0.Checked == true)
                        {
                            nRow += 1;
                            if (nRow > SS1.ActiveSheet.RowCount)
                            {
                                SS1.ActiveSheet.RowCount = nRow;
                            }
                            SS1.ActiveSheet.Cells[nRow - 1, 0].Text = "";
                            SS1.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].SNAME;
                            SS1.ActiveSheet.Cells[nRow - 1, 2].Text = hb.READ_GjJong_Name(list[i].GJJONG);
                            SS1.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].BUSENAME;
                            SS1.ActiveSheet.Cells[nRow - 1, 4].Text = VB.Left(list[i].JUMINNO, 6) + "-" + VB.Mid(list[i].JUMINNO, 7, 7);
                            SS1.ActiveSheet.Cells[nRow - 1, 5].Text = list[i].AGE + "/" + list[i].SEX;
                            SS1.ActiveSheet.Cells[nRow - 1, 6].Text = list[i].HPHONE;
                            SS1.ActiveSheet.Cells[nRow - 1, 7].Text = list[i].JEPDATE;
                            if (strMList.Length > 0)
                            {
                                strMListName = hb.IEMunjin_Name_Display(strMList);
                                SS1.ActiveSheet.Cells[nRow - 1, 8].Text = VB.Mid(strMListName, 1, strMListName.Length - 1);
                            }
                            else
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 8].Text ="";
                            }
                            SS1.ActiveSheet.Cells[nRow - 1, 9].Text = strRecvForm;
                            SS1.ActiveSheet.Cells[nRow - 1, 10].Text = strMunDate1;
                            SS1.ActiveSheet.Cells[nRow - 1, 11].Text = hb.READ_Ltd_Name(list[i].LTDCODE.ToString());
                            //SS1.ActiveSheet.Cells[nRow - 1, 12].Text = "";
                        }
                        else if (rdoGubun1.Checked == true) //문진표 작성대상자
                        {
                            if (strOK == "OK")
                            {
                                nRow += 1;
                                if (nRow > SS1.ActiveSheet.RowCount)
                                {
                                    SS1.ActiveSheet.RowCount = nRow;
                                }
                                SS1.ActiveSheet.Cells[nRow - 1, 0].Text = "";
                                SS1.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].SNAME;
                                SS1.ActiveSheet.Cells[nRow - 1, 2].Text = hb.READ_GjJong_Name(list[i].GJJONG);
                                SS1.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].BUSENAME;
                                SS1.ActiveSheet.Cells[nRow - 1, 4].Text = VB.Left(list[i].JUMINNO, 6) + "-" + VB.Mid(list[i].JUMINNO, 7, 7);
                                SS1.ActiveSheet.Cells[nRow - 1, 5].Text = list[i].AGE + "/" + list[i].SEX;
                                SS1.ActiveSheet.Cells[nRow - 1, 6].Text = list[i].HPHONE;
                                SS1.ActiveSheet.Cells[nRow - 1, 7].Text = list[i].JEPDATE;
                                if (strMList.Length > 0)
                                {
                                    strMListName = hb.IEMunjin_Name_Display(strMList);
                                    SS1.ActiveSheet.Cells[nRow - 1, 8].Text = VB.Mid(strMListName, 1, strMListName.Length - 1);
                                }
                                else
                                {
                                    SS1.ActiveSheet.Cells[nRow - 1, 8].Text = "";
                                }
                                SS1.ActiveSheet.Cells[nRow - 1, 9].Text = strRecvForm;
                                SS1.ActiveSheet.Cells[nRow - 1, 10].Text = "";
                                SS1.ActiveSheet.Cells[nRow - 1, 11].Text = hb.READ_Ltd_Name(list[i].LTDCODE.ToString());
                                //SS1.ActiveSheet.Cells[nRow - 1, 12].Text = "";
                            }
                        }
                        else if (rdoGubun2.Checked == true) //문진표 미작성대상자
                        {
                            if (strOK == "")
                            {
                                nRow += 1;
                                if (nRow > SS1.ActiveSheet.RowCount)
                                {
                                    SS1.ActiveSheet.RowCount = nRow;
                                }
                                SS1.ActiveSheet.Cells[nRow - 1, 0].Text = "";
                                SS1.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].SNAME;
                                SS1.ActiveSheet.Cells[nRow - 1, 2].Text = hb.READ_GjJong_Name(list[i].GJJONG);
                                SS1.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].BUSENAME;
                                SS1.ActiveSheet.Cells[nRow - 1, 4].Text = VB.Left(list[i].JUMINNO, 6) + "-" + VB.Mid(list[i].JUMINNO, 7, 7);
                                SS1.ActiveSheet.Cells[nRow - 1, 5].Text = list[i].AGE + "/" + list[i].SEX;
                                SS1.ActiveSheet.Cells[nRow - 1, 6].Text = list[i].HPHONE;
                                SS1.ActiveSheet.Cells[nRow - 1, 7].Text = list[i].JEPDATE;
                                if (strMList.Length > 0)
                                {
                                    strMListName = hb.IEMunjin_Name_Display(strMList);
                                    SS1.ActiveSheet.Cells[nRow - 1, 8].Text = VB.Mid(strMListName, 1, strMListName.Length - 1);
                                }
                                else
                                {
                                    SS1.ActiveSheet.Cells[nRow - 1, 8].Text = "";
                                }
                                SS1.ActiveSheet.Cells[nRow - 1, 9].Text = strRecvForm;
                                SS1.ActiveSheet.Cells[nRow - 1, 10].Text = "";
                                SS1.ActiveSheet.Cells[nRow - 1, 11].Text = hb.READ_Ltd_Name(list[i].LTDCODE.ToString());
                                //SS1.ActiveSheet.Cells[nRow - 1, 12].Text = "";
                            }
                        }
                        progressBar1.Value = i + 1;
                    }
                }

                SS1.ActiveSheet.RowCount = nRow;

                for (int i = 0; i < nRow; i++)
                {
                    Size size = SS1.ActiveSheet.GetPreferredCellSize(i, 8);
                    Size size1 = SS1.ActiveSheet.GetPreferredCellSize(i, 9);

                    if (size.Height >= size1.Height)
                    {
                        SS1.ActiveSheet.Rows[i].Height = size.Height;
                    }
                    else if (size1.Height >= size.Height)
                    {
                        SS1.ActiveSheet.Rows[i].Height = size1.Height;
                    }
                }

                Cursor.Current = Cursors.Default;
            }
            else if (sender == btnPrint)
            {
                bool PrePrint = true;

                SS1_Sheet1.Columns.Get(0).Visible = false;

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 10, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, true, true, true, false, false, false);

                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, "", "");

                SS1_Sheet1.Columns.Get(0).Visible = true;
            }
            else if (sender == btnTalkSend)
            {
                string strSname = "";
                string strGjjong = "";
                string strHtel = "";
                string strJepDate  = "";
                string strLtdName = "";
                string strTempCD = "";

                int nCount = 0;

                ComFunc.ReadSysDate(clsDB.DbCon);

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    if (SS1.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        //====================== 알림톡 전환시 사용 ========================
                        Alim.Clear_ATK_Varient();

                        strSname = SS1.ActiveSheet.Cells[i, 1].Text;
                        strGjjong = SS1.ActiveSheet.Cells[i, 2].Text;
                        strHtel = SS1.ActiveSheet.Cells[i, 6].Text;
                        strJepDate = SS1.ActiveSheet.Cells[i, 7].Text + " " + clsPublic.GstrSysTime;
                        strLtdName = SS1.ActiveSheet.Cells[i, 11].Text;

                        //------------( 자료를 DB에 INSERT )---------------------
                        strTempCD = "C_MJ_001_02_19072";

                        clsHcType.ATK.RDate = strJepDate;
                        clsHcType.ATK.SendUID = strHtel + clsPublic.GstrSysDate + DateTime.Now.ToString("HH:mm:ss");
                        clsHcType.ATK.SendUID = clsHcType.ATK.SendUID.Replace(":", "").Replace("-", "").Replace(" ", "").Replace(".", "");
                        clsHcType.ATK.Pano = "";
                        clsHcType.ATK.sName = strSname;
                        clsHcType.ATK.HPhone = strHtel;
                        clsHcType.ATK.RetTel = "054-260-8188";
                        clsHcType.ATK.SendType = "L";
                        clsHcType.ATK.TempCD = strTempCD;
                        clsHcType.ATK.Dept = "HR";
                        clsHcType.ATK.DrName = "";
                        clsHcType.ATK.LtdName = strLtdName;
                        clsHcType.ATK.JobSabun = long.Parse(clsType.User.IdNumber);
                        clsHcType.ATK.GJNAME = strGjjong;

                        clsHcType.ATK.ATMsg = Alim.READ_TEMPLATE_MESSAGE(strTempCD);
                        clsHcType.ATK.SmsMsg = Alim.READ_TEMPLATE_SMS_MESSAGE(strTempCD);
                        clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{사업장명}", strLtdName);
                        clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{수검자명}", strSname);
                        clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{검진종류}", strGjjong);

                        clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{사업장명}", strLtdName);
                        clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{수검자명}", strSname);
                        clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{검진종류}", strGjjong);

                        Alim.INSERT_ALIMTALK_MESSAGE();

                        nCount += 1;
                    }
                }

                MessageBox.Show(nCount + " 건 알림톡 전송완료하였습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        string fn_READ_UCODES_WORK(string argPaNo)
        {
            string rtnVal = "";

            string strCode = "";

            if (hicSunapdtlWorkService.GetCountbyPaNo(argPaNo) > 0)
            {
                rtnVal = "OK";
            }

            return rtnVal;
        }

        /// <summary>
        /// 거래처코드 찾기 화면 스프레드더블클릭이벤트
        /// </summary>
        /// <param name="item"></param>
        private void LtdCd_value(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        void eChkClick(object sender, EventArgs e)
        {
            if (chkAll.Checked == true)
            {
                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    SS1.ActiveSheet.Cells[i, 0].Text = "True";
                }
            }
            else
            {
                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    SS1.ActiveSheet.Cells[i, 0].Text = "False";
                }
            }
        }

        string fn_MList_Add(string argMList, string argJong)
        {
            string rtnVal = "";

            if (VB.InStr(argMList, argJong + ",") == 0)
            {
                rtnVal = argMList + argJong + ",";
            }
            else
            {
                rtnVal = argMList;
            }

            return rtnVal;
        }
    }
}
