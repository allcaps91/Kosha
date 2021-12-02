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
/// File Name       : frmHcHemsDataExCludeList.cs
/// Description     : HEMS Data 제외 리스트 관리
/// Author          : 이상훈
/// Create Date     : 2021-02-04
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmHEMS_제외관리.frm(FrmHEMS_제외관리)" />
namespace HC_Bill
{
    public partial class frmHcHemsDataExCludeList : Form
    {
        HicJepsuService hicJepsuService = null;
        HicJepsuResSpecialLtdService hicJepsuResSpecialLtdService = null;
        HicSpcPanjengMcodeService hicSpcPanjengMcodeService = null;
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

        public frmHcHemsDataExCludeList()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            hicJepsuService = new HicJepsuService();
            hicJepsuResSpecialLtdService = new HicJepsuResSpecialLtdService();
            hicSpcPanjengMcodeService = new HicSpcPanjengMcodeService();
            hicExjongService = new HicExjongService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.btnDel.Click += new EventHandler(eBtnClick);
            this.btnRoll.Click += new EventHandler(eBtnClick);
            this.btnMenuFind.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);

            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.cboYear.SelectedIndexChanged += new EventHandler(eCboChanged);

            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            int nYY = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);

            nYY = VB.Left(clsPublic.GstrSysDate, 4).To<int>();

            cboYear.Items.Clear();
            for (int i = 1; i <= 5; i++)
            {
                cboYear.Items.Add(string.Format("{0:0000}", nYY));
                nYY -= 1;
            }

            cboYear.SelectedIndex = 0;
            txtLtdCode.Text = "";
            txtWrtNo.Text = "";
            dtpFDate.Text = cboYear.Text + "-01-01";
            dtpTDate.Text = cboYear.Text + "-12-31";

            //hb.ComboJong_Set(cboJong)
            List<HIC_EXJONG> list = hicExjongService.Read_ExJong_Add(true);

            cboJong.Items.Clear();
            cboJong.Items.Add("**.전체");
            for (int i = 0; i < list.Count; i++)
            {
                cboJong.Items.Add(list[i].CODE + "." + list[i].NAME);
            }

            cboJong.SelectedIndex = 0;

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
            else if (sender == btnSearch)
            {
                int nREAD = 0;
                int nRow = 0;
                string strOK = "";
                string strFDate = "";
                string strTDate = "";
                string strJong = "";
                long nLtdCode = 0;
                long nWrtNo = 0;
                string strSort = "";
                string strYear = "";
                string strDel = "";

                sp.Spread_All_Clear(SS1);

                nWrtNo = txtWrtNo.Text.To<long>();
                nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>();
                strFDate = dtpFDate.Text;
                strTDate = dtpTDate.Text;
                strJong = VB.Left(cboJong.Text, 2);
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
                strYear = cboYear.Text;

                if (rdoDel0.Checked == true)
                {
                    strDel = "0";
                }
                else if (rdoDel1.Checked == true)
                {
                    strDel = "1";
                }
                else if (rdoDel2.Checked == true)
                {
                    strDel = "2";
                }

                SS2.Visible = false;
                sp.Spread_All_Clear(SS1);

                List<HIC_JEPSU_RES_SPECIAL_LTD> list = hicJepsuResSpecialLtdService.GetItembyJepDateGjYearGjJongLtdCodeWrtNo(strFDate, strTDate, strYear, nLtdCode, nWrtNo, strJong, strDel, strSort);

                nREAD = list.Count;

                if (nREAD >= 200)
                {
                    if (MessageBox.Show("조회건수" + nREAD + "입니다." + "\r\n" + "아니오를 선택하면 200건만 조회합니다.", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        nREAD = 200;
                    }
                }

                nRow = 0;
                progressBar1.Maximum = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    strOK = "";
                    if (chkNoise.Checked == true)
                    {
                        if (hicSpcPanjengMcodeService.GetCountbyWrtNoUCode(list[i].WRTNO, "L00") > 0)
                        {
                            strOK = "OK";
                        }

                        if (strOK == "OK")
                        {
                            nRow += 1;
                            if (SS1.ActiveSheet.RowCount < nRow)
                            {
                                SS1.ActiveSheet.RowCount = nRow;
                            }

                            SS1.ActiveSheet.Cells[i, 1].Text = list[i].JEPDATE;
                            SS1.ActiveSheet.Cells[i, 2].Text = list[i].WRTNO.To<string>();
                            SS1.ActiveSheet.Cells[i, 3].Text = list[i].SNAME;
                            SS1.ActiveSheet.Cells[i, 4].Text = list[i].AGE.To<string>();
                            SS1.ActiveSheet.Cells[i, 5].Text = hb.READ_GjJong_Name(list[i].GJJONG);
                            SS1.ActiveSheet.Cells[i, 6].Text = list[i].HEMSMIRSAYU;
                            SS1.ActiveSheet.Cells[i, 7].Text = hb.READ_Ltd_Name(list[i].LTDCODE.To<string>());
                            SS1.ActiveSheet.Cells[i, 8].Text = list[i].SUCHUPYN;
                        }
                    }
                    else
                    {
                        if (strOK == "")
                        {
                            nRow += 1;
                            if (SS1.ActiveSheet.RowCount < nRow)
                            {
                                SS1.ActiveSheet.RowCount = nRow;
                            }

                            SS1.ActiveSheet.Cells[i, 1].Text = list[i].JEPDATE;
                            SS1.ActiveSheet.Cells[i, 2].Text = list[i].WRTNO.To<string>();
                            SS1.ActiveSheet.Cells[i, 3].Text = list[i].SNAME;
                            SS1.ActiveSheet.Cells[i, 4].Text = list[i].AGE.To<string>();
                            SS1.ActiveSheet.Cells[i, 5].Text = hb.READ_GjJong_Name(list[i].GJJONG);
                            SS1.ActiveSheet.Cells[i, 6].Text = list[i].HEMSMIRSAYU;
                            SS1.ActiveSheet.Cells[i, 7].Text = hb.READ_Ltd_Name(list[i].LTDCODE.To<string>());
                            SS1.ActiveSheet.Cells[i, 8].Text = list[i].SUCHUPYN;
                        }
                    }

                    progressBar1.Value = i + 1;
                }
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
            else if (sender == btnDel)
            {
                string strChk = null;
                long nWrtNo = 0;
                string strGubun = "";
                string strSayu = "";
                int result = 0;

                //데이타 점검
                for (int i = 0; i < SS1.ActiveSheet.NonEmptyRowCount; i++)
                {
                    strChk = SS1.ActiveSheet.Cells[i, 0].Text;
                    strSayu = SS1.ActiveSheet.Cells[i, 6].Text;
                    if (strChk == "True")
                    {
                        if (strSayu.IsNullOrEmpty())
                        {
                            MessageBox.Show(i + "번째줄 사유미입력", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    nWrtNo = SS1.ActiveSheet.Cells[i, 2].Text.To<long>();
                    strSayu = SS1.ActiveSheet.Cells[i, 6].Text + " " + clsType.User.IdNumber;
                    if (strChk == "True")
                    {
                        result = hicJepsuService.UpdateHemsMirSayubyWrtNo(strSayu, "1", nWrtNo);

                        if (result < 0)
                        {
                            MessageBox.Show("HIC_JEPSU에 UPDATE시 오류 발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }
                    }
                    SS1.ActiveSheet.Cells[i, 0].Text = "";
                }

                clsDB.setCommitTran(clsDB.DbCon);

                eBtnClick(btnSearch, new EventArgs());
            }
            else if (sender == btnRoll)
            {
                string strChk = "";
                long nWrtNo = 0;
                string strGubun = "";
                int result = 0;

                if (MessageBox.Show("작업을 정말로 하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                clsDB.setBeginTran(clsDB.DbCon);

                for (int i = 0; i < SS1.ActiveSheet.NonEmptyRowCount; i++)
                {
                    strChk = SS1.ActiveSheet.Cells[i, 0].Text;
                    nWrtNo = SS1.ActiveSheet.Cells[i, 2].Text.To<long>();
                    if (strChk == "True")
                    {
                        result = hicJepsuService.UpdateHemsMirSayubyWrtNo("", "0", nWrtNo);

                        if (result < 0)
                        {
                            MessageBox.Show("HIC_JEPSU에 UPDATE시 오류 발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }
                    }
                    SS1.ActiveSheet.Cells[i, 0].Text = "";
                }
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

                List<HIC_JEPSU> list = hicJepsuService.GetItembyWrtNoList(txtWrtNo.Text.To<long>());

                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        SS2.ActiveSheet.Cells[i, 0].Text = list[i].WRTNO.To<string>();
                        SS2.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                        if (list[i].HEMSNO > 1)
                        {
                            strTemp = "사업장";
                            strTemp1 = list[i].MIRNO1.To<string>();
                        }
                        SS2.ActiveSheet.Cells[i, 2].Text = strTemp1;
                        SS2.ActiveSheet.Cells[i, 3].Text = hb.READ_GjJong_Name(list[i].GJJONG);
                        SS2.ActiveSheet.Cells[i, 4].Text = strTemp;
                    }
                }

                SS2.Visible = true;                
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                if (e.RowHeader == true && e.Column == 0)
                {
                    SS1.ActiveSheet.Cells[0, 0, SS1.ActiveSheet.RowCount - 1, 0].Text = "True";
                }
            }
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
        }

        void eCboChanged(object sender, EventArgs e)
        {
            dtpFDate.Text = cboYear.Text + "-01-01";
            dtpTDate.Text = cboYear.Text + "-12-31";
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
