using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHcDateChange.cs
/// Description     : 검진접수자 상담자조회 및 상담일자  변경
/// Author          : 이상훈
/// Create Date     : 2019-09-27
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "HaMain06.frm(FrmIDateChange)" />

namespace HC_Main
{
    public partial class frmHcDateChange : Form
    {
        HeaJepsuService heaJepsuService = null;
        HeaJepsuPatientService heaJepsuPatientService = null;

        frmHcLtdHelp frmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();

        string FstrFlag;

        public frmHcDateChange()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            heaJepsuService = new HeaJepsuService();
            heaJepsuPatientService = new HeaJepsuPatientService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.txtName.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SS1.Change += new ChangeEventHandler(eSpdChange);
            this.txtLtdCode.LostFocus += new EventHandler(etxtLostFocus);
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            FstrFlag = "";
            txtName.Text = "";
            txtLtdCode.Text = "";

            SS1.ActiveSheet.Columns[14].Visible = false;    //수정
            SS1.ActiveSheet.Columns[15].Visible = false;    //ROWID

            dtpFrDate.Text = clsPublic.GstrSysDate;
            dtpToDate.Text = clsPublic.GstrSysDate;

            //검진종류 SET
            cboJong.Items.Clear();
            hb.ComboJong_AddItem(cboJong);
            eBtnClick(btnSearch, new EventArgs());
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
            }
            else if (sender == btnSearch)
            {
                int nREAD = 0;
                int nRow = 0;
                string strOldData = "";
                string strNewData = "";
                string strJumin = "";

                string strFrDate = "";
                string strToDate = "";
                string strName = "";
                string strGbn = "";
                string strLtdCode = "";
                string strJong = "";

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;
                strName = txtName.Text.Trim();
                if (rdoGbn1.Checked == true)
                {
                    strGbn = "1";
                }
                else if (rdoGbn2.Checked == true)
                {
                    strGbn = "2";
                }
                else if (rdoGbn3.Checked == true)
                {
                    strGbn = "3";
                }
                strLtdCode = VB.Pstr(txtLtdCode.Text.Trim(), ".", 1);
                strJong = VB.Left(cboJong.Text, 2);

                sp.Spread_All_Clear(SS1);
                SS1.ActiveSheet.RowCount = 30;

                //자료를 SELECT
                List<HEA_JEPSU_PATIENT> list = heaJepsuPatientService.GetItembyIDate(strFrDate, strToDate, strName, strGbn, strLtdCode, strJong);

                nREAD = list.Count;
                for (int i = 0; i < nREAD; i++)
                {
                    strJumin = clsAES.DeAES(list[i].JUMIN2);

                    nRow += 1;
                    if (nRow > SS1.ActiveSheet.RowCount)
                    {
                        SS1.ActiveSheet.RowCount = nRow;
                    }
                    SS1.ActiveSheet.Cells[i, 1].Text = list[i].ARRIVE.Trim();
                    if (list[i].ARRIVE.Trim() == "OK")
                    {
                        if (list[i].GBSANGDAM.Trim() != "C")
                        {
                            SS1.ActiveSheet.Cells[i, 2].Text = list[i].ENTTIME.ToString();
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[i, 2].Text = "상담";
                        }
                    }
                    SS1.ActiveSheet.Cells[i, 3].Text = hb.READ_GjJong_HeaName(list[i].GJJONG);
                    SS1.ActiveSheet.Cells[i, 4].Text = list[i].PANO.ToString();
                    SS1.ActiveSheet.Cells[i, 5].Text = list[i].SDATE;
                    SS1.ActiveSheet.Cells[i, 6].Text = list[i].SNAME;
                    SS1.ActiveSheet.Cells[i, 7].Text = hb.READ_Ltd_Name(list[i].LTDCODE.To<string>());
                    SS1.ActiveSheet.Cells[i, 8].Text = list[i].AGE + "/" + list[i].SEX;
                    SS1.ActiveSheet.Cells[i, 9].Text = VB.Left(strJumin, 6) + "-" + VB.Right(strJumin, 7);
                    SS1.ActiveSheet.Cells[i, 10].Text = list[i].IDATE.ToString();
                    if (list[i].GBSANGDAM.Trim() == "Y")
                    {
                        SS1.ActiveSheet.Cells[i, 11].Text = list[i].IDATE.ToString();
                    }
                    SS1.ActiveSheet.Cells[i, 12].Text = list[i].MAILDATE;
                    SS1.ActiveSheet.Cells[i, 13].Text = list[i].PTNO;
                    SS1.ActiveSheet.Cells[i, 15].Text = list[i].RID;
                }
                SS1.ActiveSheet.RowCount = nRow;
            }
            else if (sender == btnPrint)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "종검 상담자  명 단";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("작업기간:" + dtpFrDate.Text + " ~ " + dtpToDate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                strHeader += sp.setSpdPrint_String("검진종류:" + cboJong.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                strHeader += sp.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
            else if (sender == btnSave)
            {
                string strROWID = "";
                string strFrDate = "";
                string strToDate = "";
                string strOK = "";
                string strChk = "";

                string strOldDate = "";
                string strNewDate = "";
                string strMailDate = "";
                string strSangDam = "";

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strOldDate = "";
                    strNewDate = "";
                    strSangDam = "";
                    strOK = "";
                    strChk = "";

                    strChk = SS1.ActiveSheet.Cells[i, 1].Text.Trim();
                    //도착처리된거
                    if (strChk == "OK")
                    {
                        if (SS1.ActiveSheet.Cells[i, 1].Text == "True")
                        {
                            strSangDam = "Y";
                        }
                        else
                        {
                            strSangDam = "N";
                        }
                    }
                    else
                    {
                        strSangDam = "";
                    }

                    strOldDate = SS1.ActiveSheet.Cells[i, 10].Text.Trim();
                    strNewDate = SS1.ActiveSheet.Cells[i, 11].Text.Trim();
                    strMailDate = SS1.ActiveSheet.Cells[i, 12].Text.Trim();
                    strOK = SS1.ActiveSheet.Cells[i, 14].Text.Trim();
                    strROWID = SS1.ActiveSheet.Cells[i, 15].Text.Trim();
                    //도착처리 된것을 최종 상담일자 저장하기
                    if (strSangDam == "Y" && strOK == "Y")
                    {
                        if (string.Compare(strOldDate, strNewDate) > 0)
                        {
                            MessageBox.Show("최종상담일자를 확인하세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        if (strNewDate == "")
                        {
                            MessageBox.Show("최종 상담일자를 확인하십시오", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        HEA_JEPSU item = new HEA_JEPSU();

                        item.GBSANGDAM = strSangDam;
                        item.IDATE = strNewDate;
                        item.MAILDATE = strMailDate;
                        item.RID = strROWID;

                        int result = heaJepsuService.UpdatebyRowId(item);

                        if (result < 0)
                        {
                            string sMsg = "";
                            sMsg = i + "번줄 검진 상담일자 변경시 오류가 발생함" + "\r\n";
                            sMsg += "오류자료를 수정후 다시 저장버튼을" + "\r\n";
                            sMsg += "클릭하세요.";
                            MessageBox.Show(sMsg, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else if (strSangDam == "" && strOK == "Y")
                    {
                        if (string.Compare(strOldDate, strNewDate) > 0)
                        {
                            MessageBox.Show("최종상담일자를 확인하세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        if (strNewDate == "")
                        {
                            MessageBox.Show("최종 상담일자를 확인하십시오", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        HEA_JEPSU item = new HEA_JEPSU();

                        item.GBSANGDAM = strSangDam;
                        item.IDATE = strNewDate;
                        item.MAILDATE = strMailDate;
                        item.RID = strROWID;

                        int result = heaJepsuService.UpdatebyRowId(item);

                        if (result < 0)
                        {
                            string sMsg = "";
                            sMsg = i + "번줄 검진 상담일자 변경시 오류가 발생함" + "\r\n";
                            sMsg += "오류자료를 수정후 다시 저장버튼을" + "\r\n";
                            sMsg += "클릭하세요.";
                            MessageBox.Show(sMsg, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                sp.Spread_All_Clear(SS1);
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
            if (sender == txtName)
            {
                if (e.KeyChar == 13)
                {
                    SendKeys.Send("{TAB}");
                    eBtnClick(btnSearch, new EventArgs());
                }
            }
            else if (sender == txtLtdCode)
            {
                if (e.KeyChar == 13)
                {
                    SendKeys.Send("{TAB}");
                    eBtnClick(btnLtdCode, new EventArgs());
                }
            }
        }

        void etxtLostFocus(object sender, EventArgs e)
        {
            string strLtdName = "";

            if (sender == txtLtdCode)
            {
                if (txtLtdCode.Text == "")
                {
                    txtLtdCode.Text = "전체";
                    return;
                }

                strLtdName = hb.READ_Ltd_Name(txtLtdCode.Text);

                if (strLtdName != "")
                {
                    txtLtdCode.Text += "." + strLtdName;
                }
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                long nWRTNO = 0;
                string strROWID = "";
                string strSecond = "";

                string strEntTime = "";
                string strGbSangdam = "";
                int result = 0;

                ComFunc.ReadSysDate(clsDB.DbCon);

                //초를 구함
                strSecond = DateTime.Now.ToString("SS");

                nWRTNO = long.Parse(SS1.ActiveSheet.Cells[e.Row, 5].Text.Trim());
                strROWID = SS1.ActiveSheet.Cells[e.Row, 15].Text.Trim();

                if (nWRTNO > 0 && strROWID != "")
                {
                    if (e.Column > 8)
                    {
                        return;
                    }

                    if (SS1.ActiveSheet.Cells[e.Row, 1].Text.Trim() == "")
                    {
                        strEntTime = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + ":" + strSecond;
                        strGbSangdam = "A";
                        SS1.ActiveSheet.Cells[e.Row, 1].Text = "OK";
                        result = heaJepsuService.UpdateGbSangdambyRowId(strEntTime, strROWID, strGbSangdam);
                    }
                    else
                    {
                        strEntTime = clsPublic.GstrSysDate + " " + "00:00";
                        strGbSangdam = "B";
                        SS1.ActiveSheet.Cells[e.Row, 1].Text = "";
                        result = heaJepsuService.UpdateGbSangdambyRowId(strEntTime, strROWID, strGbSangdam);
                    }

                    if (result < 0)
                    {
                        MessageBox.Show("자료 갱신중 오류가 발생했습니다", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                eBtnClick(btnSearch, new EventArgs());
            }
        }

        void eSpdChange(object sender, ChangeEventArgs e)
        {
            if (sender == SS1)
            {
                SS1.ActiveSheet.Cells[e.Row, 0].Text = "True";
                SS1.ActiveSheet.Cells[e.Row, 14].Text = "Y";
            }
        }
    }
}
