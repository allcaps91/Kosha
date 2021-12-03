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
using System.Drawing.Printing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Bill
/// File Name       : frmHcBillChkList.cs
/// Description     : 청구자 제외 리스트 관리
/// Author          : 이상훈
/// Create Date     : 2021-01-26
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "HcBill24.frm(FrmChkList)" />

namespace HC_Bill
{

    public partial class frmHcBillChkList : Form
    {
        HicJepsuService hicJepsuService = null;
        HicJepsuGundateService hicJepsuGundateService = null;
        HicExjongService hicExjongService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();
        clsHcBill chb = new clsHcBill();
        clsHcType hct = new clsHcType();
        clsPrint CP = new clsPrint();

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        frmHcCodeHelp FrmHcCodeHelp = null;

        PrintDocument pd;

        string FstrCode = "";
        string FstrName = "";

        public frmHcBillChkList()
        {
            InitializeComponent();

            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicJepsuService = new HicJepsuService();
            hicJepsuGundateService = new HicJepsuGundateService();
            hicExjongService = new HicExjongService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnDel.Click += new EventHandler(eBtnClick);
            this.btnMenuFind.Click += new EventHandler(eBtnClick);
            this.btnRoll.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.btnBogunHelp.Click += new EventHandler(eBtnClick);            

            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtBogen.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            
        }

        void SetControl()
        {

        }

        void eFormLoad(object sender, EventArgs e)
        {
            int nYY = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);

            nYY = VB.Left(clsPublic.GstrSysDate, 4).To<int>();

            cboYear.Items.Clear();
            for (int i = 1; i <= 5; i++)
            {
                cboYear.Items.Add(nYY);
                nYY -= 1;
            }

            cboYear.SelectedIndex = 0;
            txtLtdCode.Text = "";
            txtWrtNo.Text = "";
            dtpFDate.Text = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-90).ToShortDateString();
            dtpTDate.Text = clsPublic.GstrSysDate;

            cboJong.Items.Clear();
            cboJong.Items.Add("1.건강검진");
            cboJong.Items.Add("3.구강검진");
            cboJong.Items.Add("4.공단암");
            cboJong.Items.Add("E.의료급여");
            cboJong.SelectedIndex = 0;

            cboJohap.Items.Clear();
            cboJohap.Items.Add("");
            cboJohap.Items.Add("직장");
            cboJohap.Items.Add("공교");
            cboJohap.Items.Add("지역");
            cboJohap.SelectedIndex = 0;

            List<HIC_EXJONG> list = hicExjongService.Read_ExJong_Add(true);

            cboJong1.Items.Clear();
            cboJong1.Items.Add("**.전체");
            for (int i = 0; i < list.Count; i++)
            {
                cboJong1.Items.Add(list[i].CODE + "." + list[i].NAME);
            }
            cboJong1.SelectedIndex = 0;

            SS2.Visible = false;
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
            else if (sender == btnBogunHelp)
            {
                clsPublic.GstrRetValue = "25"; //보건소기호
                FrmHcCodeHelp = new frmHcCodeHelp(clsPublic.GstrRetValue);
                FrmHcCodeHelp.rSetGstrValue += new frmHcCodeHelp.SetGstrValue(eCode_value);
                FrmHcCodeHelp.ShowDialog();

                if (!FstrCode.IsNullOrEmpty())
                {
                    txtBogen.Text = FstrCode.Trim() + "." + FstrName.Trim();
                }
                else
                {
                    txtBogen.Text = "";
                }
                FrmHcCodeHelp.rSetGstrValue -= new frmHcCodeHelp.SetGstrValue(eCode_value);
            }
            else if (sender == btnPrint)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;
                string strChung = "";

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                if (rdoDel0.Checked == true)
                {
                    strChung = "제외";
                }
                else
                {
                    strChung = "미제외";
                }

                strTitle = " 청구대상자 " + strChung + "명단";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("출력일시: " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + "  PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);
                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
            else if (sender == btnSearch)
            {
                int nREAD = 0;
                string strFrDate = "";
                string strToDate = "";
                string strJong = "";
                string strJong1 = "";
                string strJep = "";
                string strJohap = "";
                string strSort = "";
                string strBogunso = "";
                string strDel = "";
                long nLtdCode = 0;
                long nWrtNo = 0;
                string strYear = "";

                sp.Spread_All_Clear(SS1);
                SS1_Sheet1.Rows[-1].Height = 20;

                if (rdoJep1.Checked == true)
                {
                    strJep = "Y";
                }
                else
                {
                    strJep = "";
                }
                if (rdoDel1.Checked == true)
                {
                    strDel = "1";
                }
                else
                {
                    strDel = "0";
                }
                if (rdoSort0.Checked == true)
                {
                    strSort = "0";
                }
                else if (rdoSort1.Checked == true)
                {
                    strSort = "1";
                }
                else if (rdoSort2.Checked == true)
                {
                    strSort = "2";
                }
                strFrDate = dtpFDate.Text;
                strToDate = dtpTDate.Text;
                strJong = VB.Left(cboJong.Text, 1);
                strJong1 = VB.Left(cboJong1.Text, 2);
                strJohap = cboJohap.Text;
                nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>();
                nWrtNo = txtWrtNo.Text.To<long>();
                strBogunso = VB.Pstr(txtBogen.Text.Trim(), ".", 1);
                strYear = cboYear.Text;

                if (rdoDel1.Checked == true)
                {
                    btnDel.Enabled = true;
                    btnRoll.Enabled = false;
                }
                else
                {
                    btnDel.Enabled = false;
                    btnRoll.Enabled = true;
                }

                List<HIC_JEPSU_GUNDATE> list = hicJepsuGundateService.GetItembyJepDateGjYearBogunso(strFrDate, strToDate, strYear, nLtdCode, nWrtNo, strJep, strDel, strJong, strJong1, strJohap, strBogunso, strSort);

                nREAD = list.Count;
                if (nREAD >= 200)
                {
                    if (MessageBox.Show("조회건수" + nREAD + "입니다." + "\r\n\r\n" + " 아니요를 선택하면 200건만 조회합니다.", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        nREAD = 200;
                    }
                }

                SS1.ActiveSheet.RowCount = nREAD;
                progressBar1.Maximum = nREAD;

                for (int i = 0; i < nREAD; i++)
                {
                    SS1.ActiveSheet.Cells[i, 1].Text = list[i].JEPDATE;
                    if (rdoJep1.Checked == true)
                    {
                        SS1.ActiveSheet.Cells[i, 1].Text = list[i].GUNDATE;
                    }
                    SS1.ActiveSheet.Cells[i, 2].Text = list[i].WRTNO.To<string>();
                    SS1.ActiveSheet.Cells[i, 3].Text = list[i].SNAME;
                    SS1.ActiveSheet.Cells[i, 4].Text = list[i].AGE.To<string>();
                    SS1.ActiveSheet.Cells[i, 5].Text = hb.READ_GjJong_Name(list[i].GJJONG);
                    SS1.ActiveSheet.Cells[i, 6].Text = list[i].MIRSAYU;
                    SS1.ActiveSheet.Cells[i, 7].Text = hb.READ_Ltd_Name(list[i].LTDCODE);
                    SS1.ActiveSheet.Cells[i, 8].Text = list[i].BOGUNSO;

                    progressBar1.Value = i + 1;
                }
            }
            else if (sender == btnDel)
            {
                string strChk = "";
                long nWRTNO = 0;
                string strGubun = "";
                string strSayu = "";
                string strBogenso = "";
                int result = 0;
                string strJong = "";

                strJong = VB.Left(cboJohap.Text, 1);

                //데이타 점검
                for (int i = 0; i < SS1.ActiveSheet.NonEmptyRowCount; i++)
                {
                    strChk = SS1.ActiveSheet.Cells[i, 0].Text;
                    strSayu = SS1.ActiveSheet.Cells[i, 6].Text;

                    if (strChk == "True")
                    {
                        if (strSayu.IsNullOrEmpty())
                        {
                            MessageBox.Show(i + 1 + "번째줄 사유미입력", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }

                if (MessageBox.Show("작업을 정말로 하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                clsDB.setBeginTran(clsDB.DbCon);

                for (int i = 0; i < SS1.ActiveSheet.NonEmptyRowCount; i++)
                {
                    strChk = SS1.ActiveSheet.Cells[i, 0].Text;
                    nWRTNO = SS1.ActiveSheet.Cells[i, 2].Text.To<long>();
                    strSayu = SS1.ActiveSheet.Cells[i, 6].Text + " " + clsType.User.IdNumber;
                    strBogenso = SS1.ActiveSheet.Cells[i, 8].Text;
                    if (strChk == "True")
                    {
                        result = hicJepsuService.UpdatebyMirSayuMirNobyWrtNo(strSayu, nWRTNO, strJong, strBogenso);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("HIC_JEPSU에 UPDATE시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    SS1.ActiveSheet.Cells[i, 0].Text = "";
                }

                clsDB.setCommitTran(clsDB.DbCon);

                eBtnClick(btnSearch, new EventArgs());
            }
            else if (sender == btnMenuFind)
            {
                string strTemp = "";
                string strTemp1 = "";

                if (txtWrtNo.Text.Trim() == "")
                {
                    MessageBox.Show("접수번호를 입력하세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                List<HIC_JEPSU> list = hicJepsuService.GetItemMirNobyWrtNo(txtWrtNo.Text.To<long>());

                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        SS2.ActiveSheet.Cells[i, 0].Text = list[i].WRTNO.To<string>();
                        SS2.ActiveSheet.Cells[i, 0].Text = list[i].SNAME;
                        if (list[i].MIRNO1 > 1)
                        {
                            strTemp = "사업장,";
                            strTemp1 = list[i].MIRNO1.To<string>();
                        }
                        if (list[i].MIRNO2 > 1)
                        {
                            strTemp += "구강,";
                            strTemp1 += list[i].MIRNO2.To<string>();
                        }
                        if (list[i].MIRNO3 > 1)
                        {
                            strTemp += "공단암,";
                            strTemp1 += list[i].MIRNO3.To<string>();
                        }
                        if (list[i].MIRNO4 > 1)
                        {
                            strTemp += "보건소암,";
                            strTemp1 += list[i].MIRNO4.To<string>();
                        }
                        if (list[i].MIRNO5 > 1)
                        {
                            strTemp += "의료급여암,";
                            strTemp1 += list[i].MIRNO5.To<string>();
                        }

                        if (!strTemp.IsNullOrEmpty())
                        {
                            strTemp = VB.Mid(strTemp, 1, strTemp.Length - 1);
                            strTemp1 = VB.Mid(strTemp1, 1, strTemp1.Length - 1);
                        }

                        SS2.ActiveSheet.Cells[i, 2].Text = strTemp1;
                        SS2.ActiveSheet.Cells[i, 3].Text = hb.READ_GjJong_Name(list[i].GJJONG);
                        SS2.ActiveSheet.Cells[i, 4].Text = strTemp;
                    }
                }

                SS2.Visible = true;
            }
            else if (sender == btnRoll)
            {
                string strChk = "";
                long nWRTNO = 0;
                string strGubun = "";
                string strBogenso = "";
                int result = 0;
                string strJong = "";

                strJong = VB.Left(cboJohap.Text, 1);

                if (MessageBox.Show("작업을 정말로 하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                clsDB.setBeginTran(clsDB.DbCon);

                for (int i = 0; i < SS1.ActiveSheet.NonEmptyRowCount; i++)
                {
                    strChk = SS1.ActiveSheet.Cells[i, 0].Text;
                    nWRTNO = SS1.ActiveSheet.Cells[i, 2].Text.To<long>();
                    strBogenso = SS1.ActiveSheet.Cells[i, 8].Text;
                    if (strChk == "True")
                    {
                        result = hicJepsuService.UpdatebyMirSayuMirNobyWrtNo("", nWRTNO, strJong, strBogenso);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("HIC_JEPSU에 UPDATE시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    SS1.ActiveSheet.Cells[i, 0].Text = "";
                }

                clsDB.setCommitTran(clsDB.DbCon);

                eBtnClick(btnSearch, new EventArgs());
            }
        }

        void eCode_value(string strCode, string strName)
        {
            FstrCode = strCode;
            FstrName = strName;
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            string strName = "";

            if (sender == txtLtdCode)
            {
                if (e.KeyChar == (char)13)
                {
                    if (txtLtdCode.Text.Length >= 2)
                    {
                        if (txtLtdCode.Text.Trim().IndexOf(".") > 0)
                        {
                            strName = hb.READ_Ltd_Name(VB.Pstr(txtLtdCode.Text.Trim(), ".", 1));
                        }
                        else
                        {
                            strName = hb.READ_Ltd_Name(txtLtdCode.Text.Trim());
                        }

                        if (strName.IsNullOrEmpty())
                        {
                            eBtnClick(btnLtdCode, new EventArgs());
                        }
                        else
                        {
                            txtLtdCode.Text += "." + strName;
                        }
                    }
                    SendKeys.Send("{Tab}");
                }
            }
            else if (sender == txtBogen)
            {
                if (e.KeyChar == (char)13)
                {
                    if (txtBogen.Text.Length >= 2)
                    {
                        if (txtBogen.Text.Trim().IndexOf(".") > 0)
                        {
                            strName = cf.Read_Bcode_Name(clsDB.DbCon, "25", VB.Pstr(txtBogen.Text.Trim(), ".", 1));
                        }
                        else
                        {
                            strName = cf.Read_Bcode_Name(clsDB.DbCon, "25", txtBogen.Text.Trim());
                        }

                        if (strName.IsNullOrEmpty())
                        {
                            eBtnClick(btnBogunHelp, new EventArgs());
                        }
                        else
                        {
                            txtBogen.Text += "." + strName;
                        }
                    }
                    SendKeys.Send("{Tab}");
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
    }
}
