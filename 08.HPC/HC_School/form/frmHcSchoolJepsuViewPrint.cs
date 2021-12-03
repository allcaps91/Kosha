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
/// Class Name      : Hc_School
/// File Name       : frmHcSchoolJepsuViewPrint.cs
/// Description     : 학생검진 접수자조회 및 출력
/// Author          : 이상훈
/// Create Date     : 2020-01-31
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmSchool9.frm(HcSchool09)" />

namespace HC_School
{
    public partial class frmHcSchoolJepsuViewPrint : Form
    {
        HicSchoolNewService hicSchoolNewService = null;
        HicJepsuSchoolNewService hicJepsuSchoolNewService = null;
        HicJepsuPatientSchoolService hicJepsuPatientSchoolService = null;
        HicBcodeService hicBcodeService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        long FnDrNo;

        public frmHcSchoolJepsuViewPrint()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void SetEvent()
        {
            hicSchoolNewService = new HicSchoolNewService();
            hicJepsuSchoolNewService = new HicJepsuSchoolNewService();
            hicJepsuPatientSchoolService = new HicJepsuPatientSchoolService();
            hicBcodeService = new HicBcodeService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.txtLtdCode.Click += new EventHandler(eTxtClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFrDate.Text = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-30).ToShortDateString();
            dtpToDate.Text = clsPublic.GstrSysDate;
            txtLtdCode.Text = "";
            txtSName.Text = "";
            txtBun.Text = "";

            cboClass.Items.Clear();
            cboClass.Items.Add("*.전체");
            cboClass.Items.Add("1");
            cboClass.Items.Add("2");
            cboClass.Items.Add("3");
            cboClass.Items.Add("4");
            cboClass.Items.Add("5");
            cboClass.Items.Add("6");
            cboClass.SelectedIndex = 0;

            cboBan.Items.Clear();
            cboBan.Items.Add("*.전체");
            cboBan.Items.Add("1");
            cboBan.Items.Add("2");
            cboBan.Items.Add("3");
            cboBan.Items.Add("4");
            cboBan.Items.Add("5");
            cboBan.Items.Add("6");
            cboBan.Items.Add("7");
            cboBan.Items.Add("8");
            cboBan.Items.Add("9");
            cboBan.Items.Add("10");
            cboBan.Items.Add("11");
            cboBan.Items.Add("12");
            cboBan.Items.Add("13");
            cboBan.Items.Add("14");
            cboBan.Items.Add("15");
            cboBan.Items.Add("16");
            cboBan.Items.Add("17");
            cboBan.Items.Add("18");
            cboBan.SelectedIndex = 0;

            btnPrint.Enabled = false;
        }

        void eTxtClick(object sender, EventArgs e)
        {
            if (sender == txtLtdCode)
            {
                eBtnClick(btnLtdCode, new EventArgs());
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
                int nRead = 0;
                int nRow = 0;
                string strJumin = "";
                string strSex = "";
                string strTemp1 = "";
                string strTemp2 = "";

                string strFrDate = "";
                string strToDate = "";
                long nLtdCode = 0;
                string strSName = "";
                string strClass = "";
                string strBan = "";
                string strBun = "";
                string strSort = "";

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;
                nLtdCode = long.Parse(VB.Pstr(txtLtdCode.Text, ".", 1));
                strSName = txtSName.Text.Trim();
                strClass = cboClass.Text;
                strBan = cboBan.Text;
                strBun = txtBun.Text.Trim();
                if (rdoSort1.Checked == true)
                {
                    strSort = "1";
                }
                else if (rdoSort2.Checked == true)
                {
                    strSort = "2";
                }

                sp.Spread_All_Clear(SS1);

                List<HIC_JEPSU_SCHOOL_NEW> list = hicJepsuSchoolNewService.GetItembyJepDateClassBanBun(strFrDate, strToDate, strSName, nLtdCode, strClass, strBan, strBun, strSort);

                nRead = list.Count;

                SS1.ActiveSheet.RowCount = nRead;
                for (int i = 0; i < nRead; i++)
                {
                    strJumin = clsAES.DeAES(list[i].JUMIN2.Trim());
                    SS1.ActiveSheet.Cells[i, 0].Text = list[i].SNAME.Trim();
                    SS1.ActiveSheet.Cells[i, 1].Text = hb.READ_Ltd_Name(list[i].LTDCODE.ToString());
                    SS1.ActiveSheet.Cells[i, 2].Text = list[i].CLASS.Trim();
                    SS1.ActiveSheet.Cells[i, 3].Text = list[i].BAN.Trim();
                    SS1.ActiveSheet.Cells[i, 4].Text = list[i].BUN.Trim();
                    if (list[i].SEX == "M")
                    {
                        strSex = "남";
                    }
                    else
                    {
                        strSex = "여";
                    }
                    SS1.ActiveSheet.Cells[i, 5].Text = strSex + "/" + list[i].AGE.ToString();
                    SS1.ActiveSheet.Cells[i, 6].Text = list[i].JEPDATE.ToString();
                    SS1.ActiveSheet.Cells[i, 7].Text = VB.Left(strJumin, 6) + "-" + VB.Mid(strJumin, 7, 1);
                    SS1.ActiveSheet.Cells[i, 8].Text = list[i].PPANA1.Trim();
                    strTemp1 = list[i].PPANA1.Trim();
                    SS1.ActiveSheet.Cells[i, 9].Text = list[i].PPANA2.Trim();
                    strTemp2 = list[i].PPANA2.Trim();
                    SS1.ActiveSheet.Cells[i, 10].Text = fn_ObesityLevelAutoPan(long.Parse(strTemp1), long.Parse(strTemp2), list[i].SEX, list[i].AGE);
                    strTemp1 = list[i].PPANA3.Trim();
                    switch (strTemp1)   //비만도
                    {
                        case "1":
                            SS1.ActiveSheet.Cells[i, 11].Text = "정상";
                            break;
                        case "2":
                            SS1.ActiveSheet.Cells[i, 11].Text = "과체중";
                            break;
                        case "3":
                            SS1.ActiveSheet.Cells[i, 11].Text = "비만";
                            break;
                        case "4":
                            SS1.ActiveSheet.Cells[i, 11].Text = "저체중";
                            break;
                        default:
                            break;
                    }

                    switch (list[i].PPANA4)
                    {
                        case "1":
                            SS1.ActiveSheet.Cells[i, 11].Text = "정상";
                            break;
                        case "2":
                            SS1.ActiveSheet.Cells[i, 11].Text = "경도비만";
                            break;
                        case "3":
                            SS1.ActiveSheet.Cells[i, 11].Text = "중증도비만";
                            break;
                        case "4":
                            SS1.ActiveSheet.Cells[i, 11].Text = "고도비만";
                            break;
                        default:
                            break;
                    }
                }
                btnPrint.Enabled = true;
            }
            else if (sender == btnPrint)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ComFunc.ReadSysDate(clsDB.DbCon);

                if (SS1.ActiveSheet.NonEmptyRowCount == 0)
                {
                    MessageBox.Show("자료가 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "학생검진 접수명단";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("맑은 고딕", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);

                sp.Spread_All_Clear(SS1);
            }
        }

        string fn_ObesityLevelAutoPan(long argHeight, long argWeight, string argSex, long argAge)
        {
            string rtnVal = "";
            string strPan = "";
            string strBiman = "";
            double nBMI = 0;
            double nHEIGHT = 0;

            if (argHeight == 0 || argWeight == 0)
            {
                return rtnVal;
            }
            else
            {
                nHEIGHT = argHeight / 100;
                nBMI = argWeight / (nHEIGHT * nHEIGHT);
                nBMI = Math.Round(nBMI, 2);
                if (argSex == "M")
                {
                    for (int i = 1; i < SS5.ActiveSheet.RowCount; i++)
                    {
                        if (long.Parse(SS5.ActiveSheet.Cells[i, 0].Text) == argAge)
                        {
                            for (int j = 0; 1 < SS5.ActiveSheet.ColumnCount; j++)
                            {
                                if (nBMI < double.Parse(SS5.ActiveSheet.Cells[0, j].Text))
                                {
                                    if (j == 1)
                                    {
                                        j += 1;
                                    }
                                    strPan = SS5.ActiveSheet.Cells[0, j - 1].Text.Trim();
                                    break;
                                }
                            }
                            if (strPan == "")
                            {
                                strPan = "97";
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 1; i < SS6.ActiveSheet.RowCount; i++)
                    {
                        if (long.Parse(SS6.ActiveSheet.Cells[i, 0].Text) == argAge)
                        {
                            for (int j = 1; j < SS6.ActiveSheet.ColumnCount; j++)
                            {
                                if (j == 1)
                                {
                                    j += 1;
                                }
                                strPan = SS6.ActiveSheet.Cells[i, j - 1].Text.Trim();
                                break;
                            }
                            if (strPan == "")
                            {
                                strPan = "97";
                            }
                        }
                    }
                }
            }

            rtnVal = nBMI.ToString();

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
    }
}
