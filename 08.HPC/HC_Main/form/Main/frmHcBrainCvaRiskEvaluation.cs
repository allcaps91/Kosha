using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHcBrainCvaRiskEvaluation.cs
/// Description     : 뇌ㆍ심혈관질환 발병위험도평가 입력
/// Author          : 이상훈
/// Create Date     : 2019-09-06
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm뇌심혈관발병위험평가.frm(Frm뇌심혈관발병위험평가)" />

namespace HC_Main
{
    public partial class frmHcBrainCvaRiskEvaluation : Form
    {
        HeaJepsuService heaJepsuService = null;
        HicResultService hicResultService = null;
        HeaValuationService heaValuationService = null;
        HeaJepsuValuationService heaJepsuValuationService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();

        frmHcLtdHelp frmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        string FstrROWID = "";
        long FnWRTNO = 0;
        string FstrSex = "";

        long FWRTNO;

        public frmHcBrainCvaRiskEvaluation(long WrtNo)
        {
            InitializeComponent();
            FWRTNO = WrtNo;
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            heaJepsuService = new HeaJepsuService();
            hicResultService = new HicResultService();
            heaValuationService = new HeaValuationService();
            heaJepsuValuationService = new HeaJepsuValuationService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnDelete.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtGajok1.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtGajok2.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.chkJilhwan1.KeyPress += new KeyPressEventHandler(eKeyPress); 
            this.txtSmoke.GotFocus += new EventHandler(etxtGotFocus);
            this.txtSmoke.LostFocus += new EventHandler(etxtLostFocus);
            this.txtActive.GotFocus += new EventHandler(etxtGotFocus);
            this.txtActive.LostFocus += new EventHandler(etxtLostFocus);
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            txtLtdCode.Text = "";
            dtpFrDate.Text = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-20).ToShortDateString();
            dtpToDate.Text = clsPublic.GstrSysDate;

            sp.Spread_All_Clear(SSList);
            SSList.ActiveSheet.RowCount = 50;

            fn_Screen_Clear();

            eBtnClick(btnSearch, new EventArgs());

            if (FWRTNO > 0)
            {
                FnWRTNO = FWRTNO;
                fn_Screen_Display();
            }
        }

        void fn_Screen_Clear()
        {
            FstrROWID = "";
            FstrSex = "";
            FnWRTNO = 0;

            txtSmoke.Text = "";
            lblSmoke.Text = "";
            txtActive.Text = "";
            lblActive.Text = "";

            //가족력
            txtGajok1.Text = "";
            txtGajok2.Text = "";
            chkGajok1.Checked = false;
            chkGajok2.Checked = false;
            chkGajok3.Checked = false;

            //표면장기
            chkJangi1.Checked = false;
            chkJangi2.Checked = false;
            chkJangi3.Checked = false;
            chkJangi4.Checked = false;

            //질병상태
            chkJilhwan1.Checked = false;
            chkJilhwan2.Checked = false;
            chkJilhwan3.Checked = false;
            chkJilhwan4.Checked = false;
            chkJilhwan5.Checked = false;
            chkJilhwan6.Checked = false;
            chkJilhwan7.Checked = false;

            //기본검사정보
            for (int i = 0; i < 6; i++)
            {
                SS1.ActiveSheet.Cells[i, 1].Text = "";
            }
            lblMsg.Text = "";
            lblInfo.Text = "";
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            string strTemp = "";

            if (sender == SSList)
            {
                fn_Screen_Clear();

                FnWRTNO = long.Parse(SSList.ActiveSheet.Cells[e.Row, 0].Text);

                strTemp = "" + SSList.ActiveSheet.Cells[e.Row, 1].Text.Trim();
                strTemp += " 성별 : " + SSList.ActiveSheet.Cells[e.Row, 4].Text.Trim();
                strTemp += " 나이 : " + SSList.ActiveSheet.Cells[e.Row, 5].Text.Trim();

                lblInfo.Text = strTemp;

                fn_Screen_Display();
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
                int nRead = 0;
                string strList = "";
                string strTemp = "";

                sp.Spread_All_Clear(SSList);
                SSList.ActiveSheet.RowCount = 50;
                //txtLtdCode.Text = txtLtdCode.Text + "." + hb.READ_Ltd_Name(txtLtdCode.Text.Trim());

                if (rdoGubun1.Checked == true)
                {
                    strTemp = "1";
                }
                else if (rdoGubun2.Checked == true)
                {
                    strTemp = "2";
                }

                //자료를 Select
                List<HEA_JEPSU_VALUATION> list = heaJepsuValuationService.GetItembyLtdCode(strTemp, dtpFrDate.Text, dtpToDate.Text, VB.Pstr(txtLtdCode.Text, ".", 1).Trim());

                nRead = list.Count;
                SSList.ActiveSheet.RowCount = nRead;
                //SSList.DataSource = list;
                for (int i = 0; i < list.Count; i++)
                {
                    SSList.ActiveSheet.Cells[i, 0].Text = list[i].WRTNO.ToString();
                    SSList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                    SSList.ActiveSheet.Cells[i, 2].Text = list[i].JEPDATE;
                    SSList.ActiveSheet.Cells[i, 3].Text = list[i].GJJONG;
                    SSList.ActiveSheet.Cells[i, 4].Text = list[i].SEX;
                    SSList.ActiveSheet.Cells[i, 5].Text = list[i].AGE.ToString();
                }
            }
            else if (sender == btnLtdCode)
            {
                frmHcLtdHelp frm = new frmHcLtdHelp();
                frm.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(LtdCd_value);
                frm.ShowDialog();
                frm.rSetGstrValue -= new frmHcLtdHelp.SetGstrValue(LtdCd_value);

                if (!LtdHelpItem.IsNullOrEmpty())
                {
                    txtLtdCode.Text = LtdHelpItem.CODE.ToString() + "." + LtdHelpItem.SANGHO;
                }
                else
                {
                    txtLtdCode.Text = "";
                }

                //If GstrRetValue<> "" Then
                //    TxtLtdCode.Text = GstrRetValue
                //    '사업장코드의 지사,사업장기호를 Display
                //    SQL = "SELECT Name,Jisa,Kiho FROM HIC_LTD "
                //    SQL = SQL & "WHERE Code=" & Val(TxtLtdCode.Text) & " "
                //    Call AdoOpenSet(rs1, SQL)
                //    If RowIndicator = 0 Then
                //        AdoCloseSet(rs1)
                //        TxtLtdCode.Text = ""
                //        PanelLtdName.Caption = ""
                //        Exit Sub
                //    End If
                //    PanelLtdName.Caption = Trim(AdoGetString(rs1, "Name", 0))
                //End If
            }
            else if (sender == btnCancel)
            {
                fn_Screen_Clear();
            }
            else if (sender == btnDelete)
            {
                string strMsg = "";

                if (FstrROWID == "")
                {
                    return;
                }

                strMsg = "정말로 문진표 내역을" + "\r\n";
                strMsg += "삭제 하시겠습니까?" + "\r\n";
                if (MessageBox.Show(strMsg, "선택", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                //문진내역을 삭제
                int result = heaValuationService.Delete(FnWRTNO);

                if (result < 0)
                {
                    MessageBox.Show("문진내역을 삭제시 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }

                eBtnClick(btnSearch, new EventArgs());
                fn_Screen_Clear();
            }
            else if (sender == btnSave)
            {
                string strSmoke = "";
                string strActive = "";
                string strGajok1 = "";
                string strGajok2 = "";
                string strGajok3 = "";
                string strJangi = "";
                string strJilhwan = "";

                //변수에 입력값을 저장
                //흡연     
                strSmoke = txtSmoke.Text.Trim();
                //신체활동
                strActive = txtActive.Text.Trim();
                //가족력
                strGajok1 = txtGajok1.Text.Trim();
                strGajok2 = txtGajok2.Text.Trim();
                strGajok3 = "";
                
                if (chkGajok1.Checked == true)
                {
                    strGajok3 += "1";   //흡연-예
                }
                else
                {
                    strGajok3 += "2";   //아니오
                }

                if (chkGajok2.Checked == true)
                {
                    strGajok3 += "1";   //흡연-예
                }
                else
                {
                    strGajok3 += "2";   //아니오
                }

                if (chkGajok3.Checked == true)
                {
                    strGajok3 += "1";   //흡연-예
                }
                else
                {
                    strGajok3 += "2";   //아니오
                }

                //표면장기
                strJangi = "";
                if (chkJangi1.Checked == true)
                {
                    strJangi += "1";    //예
                }
                else
                {
                    strJangi += "0";    //아니오
                }

                if (chkJangi2.Checked == true)
                {
                    strJangi += "1";    //예
                }
                else
                {
                    strJangi += "0";    //아니오
                }

                if (chkJangi3.Checked == true)
                {
                    strJangi += "1";    //예
                }
                else
                {
                    strJangi += "0";    //아니오
                }

                if (chkJangi4.Checked == true)
                {
                    strJangi += "1";    //예
                }
                else
                {
                    strJangi += "0";    //아니오
                }

                //질병상태
                if (chkJilhwan1.Checked == true)
                {
                    strJilhwan += "1";  //예
                }
                else
                {
                    strJilhwan += "0";  //아니오
                }

                if (chkJilhwan2.Checked == true)
                {
                    strJilhwan += "1";  //예
                }
                else
                {
                    strJilhwan += "0";  //아니오
                }

                if (chkJilhwan3.Checked == true)
                {
                    strJilhwan += "1";  //예
                }
                else
                {
                    strJilhwan += "0";  //아니오
                }

                if (chkJilhwan4.Checked == true)
                {
                    strJilhwan += "1";  //예
                }
                else
                {
                    strJilhwan += "0";  //아니오
                }

                if (chkJilhwan5.Checked == true)
                {
                    strJilhwan += "1";  //예
                }
                else
                {
                    strJilhwan += "0";  //아니오
                }

                if (chkJilhwan6.Checked == true)
                {
                    strJilhwan += "1";  //예
                }
                else
                {
                    strJilhwan += "0";  //아니오
                }

                if (chkJilhwan7.Checked == true)
                {
                    strJilhwan += "1";  //예
                }
                else
                {
                    strJilhwan += "0";  //아니오
                }

                if (FstrROWID == "")
                {
                    HEA_VALUATION item = new HEA_VALUATION();

                    item.WRTNO = FnWRTNO;
                    item.SMOKE = strSmoke;
                    item.ACTIVE = strActive;
                    item.GAJOK1 = strGajok1;
                    item.GAJOK2 = strGajok2;
                    item.GAJOK3 = strGajok3;
                    item.JANGI = strJangi;
                    item.JIHWAN = strJilhwan;
                    item.ENTSABUN = long.Parse(clsType.User.IdNumber);

                    int result = heaValuationService.Insert(item);

                    if (result < 0)
                    {
                        MessageBox.Show("자료를 등록중 오류가 발생함!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    HEA_VALUATION item = new HEA_VALUATION();

                    item.WRTNO = FnWRTNO;
                    item.SMOKE = strSmoke;
                    item.ACTIVE = strActive;
                    item.GAJOK1 = strGajok1;
                    item.GAJOK2 = strGajok2;
                    item.GAJOK3 = strGajok3;
                    item.JANGI = strJangi;
                    item.JIHWAN = strJilhwan;
                    item.ENTSABUN = long.Parse(clsType.User.IdNumber);

                    int result = heaValuationService.Update(item);

                    if (result < 0)
                    {
                        MessageBox.Show("자료를 등록중 오류가 발생함!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                fn_Screen_Clear();
                eBtnClick(btnSearch, new EventArgs());
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

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtLtdCode)
            {
                if (e.KeyChar == (char)13)
                {
                    txtLtdCode.Text = hb.READ_Ltd_Name(txtLtdCode.Text.Trim());
                    SendKeys.Send("{Tab}");
                }
            }
            else if (sender == txtActive)
            {
                if (e.KeyChar == 13)
                {
                    switch (txtActive.Text.Trim())
                    {
                        case "1":
                            lblActive.Text = "규칙적으로한다";
                            break;
                        case "2":
                            lblActive.Text = "운동부족이다";
                            break;
                        default:
                            break;
                    }
                }
            }
            else if (sender == txtSmoke)
            {
                if (e.KeyChar == 13)
                {
                    switch (txtSmoke.Text.Trim())
                    {
                        case "1":
                            lblSmoke.Text = "현재하고있다";
                            break;
                        case "2":
                            lblSmoke.Text = "안한다";
                            break;
                        default:
                            break;
                    }

                    SendKeys.Send("{Tab}");
                }
            }
            else if (sender == txtGajok1)
            {
                SendKeys.Send("{Tab}");
            }
            else if (sender == txtGajok2)
            {
                SendKeys.Send("{Tab}");
            }
            else if (sender == chkGajok1)
            {
                SendKeys.Send("{Tab}");
            }
            else if (sender == chkGajok2)
            {
                SendKeys.Send("{Tab}");
            }
            else if (sender == chkGajok3)
            {
                SendKeys.Send("{Tab}");
            }
            else if (sender == chkJilhwan1)
            {
                SendKeys.Send("{Tab}");
            }
            else if (sender == chkJilhwan2)
            {
                SendKeys.Send("{Tab}");
            }
            else if (sender == chkJilhwan3)
            {
                SendKeys.Send("{Tab}");
            }
            else if (sender == chkJilhwan4)
            {
                SendKeys.Send("{Tab}");
            }
            else if (sender == chkJilhwan5)
            {
                SendKeys.Send("{Tab}");
            }
            else if (sender == chkJilhwan6)
            {
                SendKeys.Send("{Tab}");
            }
            else if (sender == chkJilhwan7)
            {
                SendKeys.Send("{Tab}");
            }
            else if (sender == chkJangi1)
            {
                SendKeys.Send("{Tab}");
            }
            else if (sender == chkJangi2)
            {
                SendKeys.Send("{Tab}");
            }
            else if (sender == chkJangi3)
            {
                SendKeys.Send("{Tab}");
            }
            else if (sender == chkJangi4)
            {
                SendKeys.Send("{Tab}");
            }
        }

        void etxtGotFocus(object sender, EventArgs e)
        {
            if (sender == txtSmoke)
            {
                lblMsg.Text = "1.현재하고있다  2.안한다";
            }
            else if (sender == txtActive)
            {
                lblMsg.Text = "1.규칙적으로한다  2.운동부족이다";
            }
        }

        void etxtLostFocus(object sender, EventArgs e)
        {
            lblMsg.Text = "";
        }

        void fn_Screen_Display()
        {
            string strData = "";
            string strH = "";
            string strL = "";
            long nHeight = 0;
            long nWeight = 0;
            long nBMI = 0;

            if (FnWRTNO == 0)
            {
                return;
            }

            //인적사항을 Display
            HEA_JEPSU list = heaJepsuService.GetItembyWrtNo(FnWRTNO);

            if (list == null)
            {
                MessageBox.Show("접수번호 " + FnWRTNO + "번이 자료가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            FstrSex = list.SEX.Trim();

            //기본검사정보
            List<HIC_RESULT> list2 = hicResultService.GetAllByWrtNo(FnWRTNO);

            if (list2.Count == 0)
            {
                MessageBox.Show("접수번호 " + FnWRTNO + "번이 자료가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            for (int i = 0; i < list2.Count; i++)
            {
                //기본검사정보
                switch (list2[i].EXCODE)
                {
                    case "A101":
                        SS1.ActiveSheet.Cells[0, 1].Text = list2[i].RESULT.Trim();
                        nHeight = long.Parse(SS1.ActiveSheet.Cells[0, 1].Text); //신장
                        break;
                    case "A102":
                        SS1.ActiveSheet.Cells[1, 1].Text = list2[i].RESULT.Trim();
                        nWeight = long.Parse(SS1.ActiveSheet.Cells[i, 1].Text); //체중
                        break;
                    case "A108":                        
                        strH = list2[i].RESULT.Trim();
                        break;
                    case "A109":
                        strL = list2[i].RESULT.Trim();
                        break;
                    case "A123":
                        SS1.ActiveSheet.Cells[4, 1].Text = list2[i].RESULT.Trim();  //총콜레스테롤
                        break;
                    case "A122":
                        SS1.ActiveSheet.Cells[5, 1].Text = list2[i].RESULT.Trim();  //혈당
                        break;
                    default:
                        break;
                }
            }

            //BMI
            nHeight = nHeight / 100;
            SS1.ActiveSheet.Cells[2, 1].Text = string.Format("{0:#.0}", nBMI);
            //혈압
            SS1.ActiveSheet.Cells[3, 1].Text = strH + "/" + strL;

            //문진내역을 READ
            FstrROWID = "";

            HEA_VALUATION list3 = heaValuationService.GetAllbyWrtNo(FnWRTNO);

            if (list3 == null)
            {
                txtSmoke.Focus();
                return;
            }

            FstrROWID = list3.ROWID;

            txtSmoke.Text = list3.SMOKE.Trim();
            switch (list3.SMOKE)
            {
                case "1":
                    lblSmoke.Text = "현재하고있다";
                    break;
                case "2":
                    lblSmoke.Text = "안있다";
                    break;
                default:
                    break;
            }

            txtActive.Text = list3.ACTIVE.Trim();
            switch (list3.ACTIVE)
            {
                case "1":
                    lblActive.Text = "규칙적으로한다";
                    break;
                case "2":
                    lblActive.Text = "운동부족이다";
                    break;
                default:
                    break;
            }

            txtGajok1.Text = list3.GAJOK1.Trim();
            txtGajok2.Text = list3.GAJOK2.Trim();
            
            if (VB.Mid(list3.GAJOK3.Trim(), 1, 1) == "1")
            {
                chkGajok1.Checked = true;
            }
            else
            {
                chkGajok1.Checked = false;
            }

            if (VB.Mid(list3.GAJOK3.Trim(), 2, 1) == "1")
            {
                chkGajok2.Checked = true;
            }
            else
            {
                chkGajok2.Checked = false;
            }

            if (VB.Mid(list3.GAJOK3.Trim(), 3, 1) == "1")
            {
                chkGajok3.Checked = true;
            }
            else
            {
                chkGajok3.Checked = false;
            }

            //표면장기
            if (VB.Mid(list3.JANGI.Trim(), 1, 1) == "1")
            {
                chkJangi1.Checked = true;
            }
            else
            {
                chkJangi1.Checked = false;
            }

            if (VB.Mid(list3.JANGI.Trim(), 2, 1) == "1")
            {
                chkJangi2.Checked = true;
            }
            else
            {
                chkJangi2.Checked = false;
            }

            if (VB.Mid(list3.JANGI.Trim(), 3, 1) == "1")
            {
                chkJangi3.Checked = true;
            }
            else
            {
                chkJangi3.Checked = false;
            }

            if (VB.Mid(list3.JANGI.Trim(), 4, 1) == "1")
            {
                chkJangi4.Checked = true;
            }
            else
            {
                chkJangi4.Checked = false;
            }

            //질병상태
            if (VB.Mid(list3.JIHWAN.Trim(), 1, 1) == "1")
            {
                chkJilhwan1.Checked = true;
            }
            else
            {
                chkJilhwan1.Checked = false;
            }

            if (VB.Mid(list3.JIHWAN.Trim(), 2, 1) == "1")
            {
                chkJilhwan2.Checked = true;
            }
            else
            {
                chkJilhwan2.Checked = false;
            }

            if (VB.Mid(list3.JIHWAN.Trim(), 3, 1) == "1")
            {
                chkJilhwan3.Checked = true;
            }
            else
            {
                chkJilhwan3.Checked = false;
            }

            if (VB.Mid(list3.JIHWAN.Trim(), 4, 1) == "1")
            {
                chkJilhwan4.Checked = true;
            }
            else
            {
                chkJilhwan4.Checked = false;
            }

            if (VB.Mid(list3.JIHWAN.Trim(), 5, 1) == "1")
            {
                chkJilhwan5.Checked = true;
            }
            else
            {
                chkJilhwan5.Checked = false;
            }

            if (VB.Mid(list3.JIHWAN.Trim(), 6, 1) == "1")
            {
                chkJilhwan6.Checked = true;
            }
            else
            {
                chkJilhwan6.Checked = false;
            }

            if (VB.Mid(list3.JIHWAN.Trim(), 7, 1) == "1")
            {
                chkJilhwan7.Checked = true;
            }
            else
            {
                chkJilhwan7.Checked = false;
            }
        }
    }
}
