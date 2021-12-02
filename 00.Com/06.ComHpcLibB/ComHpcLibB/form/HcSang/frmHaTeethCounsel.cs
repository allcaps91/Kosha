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
/// Class Name      : ComHpcLibB
/// File Name       : frmHaTeethCounsel.cs
/// Description     : 종합검진 구강상담
/// Author          : 이상훈
/// Create Date     : 2019-09-16
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm종검구강상담_new.frm(Frm종검구강상담_new)" />

namespace ComHpcLibB
{
    public partial class frmHaTeethCounsel : Form
    {
        HeaDentalService heaDentalService = null;
        HicResultService hicResultService = null;
        HeaSangdamWaitService heaSangdamWaitService = null;
        HeaJepsuSangdamDentalService heaJepsuSangdamDentalService = null;
        HeaJepsuService heaJepsuService = null;
        HeaResultService heaResultService = null;

        frmHcSangInternetMunjinView FrmHcSangInternetMunjinView = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();

        long FnWRTNO;
        long FnPano;
        string FstrROWID;
        string FstrPtno;
        string FstrSdate;
        string FstrJepDate;
        string FstrPano;
        string FstrGjJong;

        frmHcLtdHelp frmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        FarPoint.Win.Spread.CellType.CheckBoxCellType chk = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
        FarPoint.Win.Spread.CellType.TextCellType txt = new FarPoint.Win.Spread.CellType.TextCellType();

        public frmHaTeethCounsel()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            heaDentalService = new HeaDentalService();
            hicResultService = new HicResultService();
            heaSangdamWaitService = new HeaSangdamWaitService();
            heaJepsuSangdamDentalService = new HeaJepsuSangdamDentalService();
            heaJepsuService = new HeaJepsuService();
            heaResultService = new HeaResultService();

            this.Load += new EventHandler(eFormLoad);
            this.FormClosing += new FormClosingEventHandler(eFromClosing);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SSList.CellClick += new CellClickEventHandler(eSpdClick);
            this.txtWrtNo.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.rdoSts1.Click += new EventHandler(eRdoClick);
            this.rdoSts2.Click += new EventHandler(eRdoClick);
            this.txtDt0.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtDt1.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtDt2.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtDt3.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtDt4.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtDt5.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtDt6.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtDt7.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtDt8.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtDt9.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtDt10.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtDt11.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtDt12.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtDt13.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtDt14.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtDt15.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtDt16.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtDt17.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtDt18.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtDt19.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtDt20.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtDt21.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtDt22.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtDt23.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtDt24.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtDt25.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtDt26.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtDt27.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtDt28.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtDt29.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtDt30.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtDt31.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtDt32.DoubleClick += new EventHandler(eTxtDblClick);

            this.txtUdt0.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtUdt1.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtUdt2.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtUdt3.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtUdt4.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtUdt5.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtUdt6.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtUdt7.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtUdt8.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtUdt9.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtUdt10.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtUdt11.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtUdt12.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtUdt13.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtUdt14.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtUdt15.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtUdt16.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtUdt17.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtUdt18.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtUdt19.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtUdt20.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtUdt21.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtUdt22.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtUdt23.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtUdt24.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtUdt25.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtUdt26.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtUdt27.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtUdt28.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtUdt29.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtUdt30.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtUdt31.DoubleClick += new EventHandler(eTxtDblClick);
            this.txtUdt32.DoubleClick += new EventHandler(eTxtDblClick);

            this.rdoJob1.Click += new EventHandler(eRdoClick);
            this.rdoJob2.Click += new EventHandler(eRdoClick);
            this.rdoSort1.Click += new EventHandler(eRdoClick);
            this.rdoSort2.Click += new EventHandler(eRdoClick);
            this.rdoSort3.Click += new EventHandler(eRdoClick);
            this.rdoSort4.Click += new EventHandler(eRdoClick);

            this.txtPanDrNo.KeyPress += new KeyPressEventHandler(eTxtKeyPress);

            this.SS_SEL.ButtonClicked += new EditorNotifyEventHandler(eSpdBtnClick);
        }

        private void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFrDate.Text = clsPublic.GstrSysDate;
            dtpToDate.Text = clsPublic.GstrSysDate;

            txtLtdCode.Text = "";
            txtSName.Text = "";

            sp.Spread_All_Clear(SSList);
            sp.Spread_All_Clear(SS1);

            fn_Screen_Clear();

            rdoSort3.Checked = true;
            txtWrtNo.Text = "";

            SS_SEL_Sheet1.Columns[3].Visible = false;
            SS_SEL_Sheet1.Columns[4].Visible = false;
        }

        void eFromClosing(object sender, FormClosingEventArgs e)
        {
            if (hf.OpenForm_Check("frmHcSangInternetMunjinView") == true)
            {
                //FrmHcSangInternetMunjinView.Close();
                FrmHcSangInternetMunjinView.Dispose();
                FrmHcSangInternetMunjinView = null;
            }

            ComFunc.KillProc("friendly omr.exe");
            ComFunc.KillProc("hcscript.exe");
        }

        void fn_Screen_Clear()
        {
            txtWrtNo.Text = "";
            sp.Spread_All_Clear(SS1);

            for (int i = 0; i <= 32; i++)
            {
                TextBox txtUDt = (Controls.Find("txtUDt" + i.ToString(), true)[0] as TextBox);
                TextBox txtDt = (Controls.Find("txtDt" + i.ToString(), true)[0] as TextBox);
                txtDt.Text = "";
                txtUDt.Text = "";
            }

            rdoSts1.Checked = true;

            lblToothdecay.Text = " ";
            lblToothDefect.Text = " ";

            rdoGingivitis2.Checked = true;
            rdoPeriodontitis2.Checked = true;
            rdoToothBrush2.Checked = true;
            rdoArtificialTeeth1.Checked = true;
            txtArtificialTeeth.Text = "";

            for (int i = 0; i <= 13; i++)
            {
                CheckBox chkPanjeng = (Controls.Find("chkPanjeng" + i.ToString(), true)[0] as CheckBox);
                chkPanjeng.Checked = false;
            }

            btnSave.Enabled = false;

            txtPanjengEtc.Text = "";
            txtPanDrNo.Text = "";
            lblDrName.Text = "";
            pnlMain.Enabled = false;

            eBtnClick(btnSearch, new EventArgs());
        }

        void eRdoClick(object sender, EventArgs e)
        {
            if (sender == rdoSts1)  //우식치
            {
                rdoSts1.Checked = true;
                rdoSts2.Checked = false;
                pnlUsik.Visible = true;
                pnlGyeol.Visible = false;
                pnlUsik.BringToFront();
            }
            else if (sender == rdoSts2) //결손치
            {
                rdoSts1.Checked = false;
                rdoSts2.Checked = true;
                pnlUsik.Visible = false;
                pnlGyeol.Visible = true;
                pnlGyeol.BringToFront();
            }
            else if (sender == rdoJob1 || sender == rdoJob2 || sender == rdoSort1 || sender == rdoSort2 || sender == rdoSort3 || sender == rdoSort4)
            {
                eBtnClick(btnSearch, new EventArgs());
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtPanDrNo)
            {
                lblDrName.Text = hb.READ_License_DrName(txtPanDrNo.Text.To<long>());
            }
        }

        void eSpdBtnClick(object sender, EditorNotifyEventArgs e)
        {
            int nCnt = 0;

            for (int i = 0; i < SS_SEL.ActiveSheet.NonEmptyRowCount; i++)
            {
                if (SS_SEL.ActiveSheet.Cells[i, 2].Text == "True")
                {
                    nCnt += 1;
                }

                if (nCnt > 1)
                {
                    MessageBox.Show("동시에 2개를 선택할수 없습니다...한개만 선택후 하십시오", "확인", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    SS_SEL.ActiveSheet.Cells[e.Row, 2].Text = "";
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
            else if (sender == btnCancel)
            {
                fn_Screen_Clear();
                fn_SS_SEL_Clear();
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
                string strLtdCode = "";
                string strFrDate = "";
                string strToDate = "";
                string strJob = "";
                string strSort = "";

                if (rdoJob1.Checked == true)
                {
                    strJob = "1";
                }
                else
                {
                    strJob = "2";
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
                fn_SS_SEL_Clear();

                strLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1);

                strFrDate = dtpFrDate.Text;
                strToDate = DateTime.Parse(dtpToDate.Text).AddDays(1).ToShortDateString();

                //자료를 SELECT
                List<HEA_JEPSU_SANGDAM_DENTAL> list = heaJepsuSangdamDentalService.GetItembyDentRoom(strFrDate, strToDate, clsHcVariable.DENT_ROOM, strLtdCode, txtSName.Text.Trim(), strJob, strSort);

                SSList.ActiveSheet.RowCount = list.Count;

                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        SSList.ActiveSheet.Cells[i, 0].Text = list[i].WRTNO.To<string>();
                        SSList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                        SSList.ActiveSheet.Cells[i, 2].Text = list[i].SDATE;
                        SSList.ActiveSheet.Cells[i, 3].Text = list[i].GJJONG;
                        SSList.ActiveSheet.Cells[i, 4].Text = list[i].SDATE2;
                    }
                }
            }
            else if (sender == btnSave)
            {
                long nAge = 0;
                string strOK = "";
                string strUsik = "";
                string strGyeol = "";
                string strChiEun = "";
                string strChiJu = "";
                string strChiGun = "";
                string strDenture = "";
                string strDentureEtc = "";
                string strTPanjengEtc = "";
                string[] strTPanjeng = new string[15];
                string strMsg = "";
                string strSname = "";
                string strSEX = "";
                string strGJJONG = "";

                int result = 0;

                int nSCnt = 0;
                string strRoom = "";

                //검사실 지정없으면 상담저장안됨
                for (int i = 0; i < SS_SEL.ActiveSheet.NonEmptyRowCount; i++)
                {
                    if (SS_SEL.ActiveSheet.Cells[i, 2].Text == "True")
                    {
                        nSCnt += 1;
                        strRoom = SS_SEL.ActiveSheet.Cells[i, 3].Text;
                    }
                }

                if (nSCnt == 0)
                {
                    MessageBox.Show("다음 검사실을 지정해주세요!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else if (nSCnt > 1)
                {
                    MessageBox.Show("검사실을 여러개 지정할수 없습니다!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                strSname = SS1.ActiveSheet.Cells[0, 1].Text.Trim();
                nAge = VB.Pstr(SS1.ActiveSheet.Cells[0, 2].Text, "/", 1).To<long>();
                strSEX = VB.Pstr(SS1.ActiveSheet.Cells[0, 2].Text, "/", 2);
                strGJJONG = VB.Pstr(SS1.ActiveSheet.Cells[0, 5].Text.Trim(), ".", 1);

                if (txtPanDrNo.Text.To<long>() == 0)
                {
                    MessageBox.Show("판정의사 면허번호 누락", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                strOK = "";
                for (int i = 0; i <= 13; i++)
                {
                    CheckBox chkPanjeng = (Controls.Find("chkPanjeng" + i.ToString(), true)[0] as CheckBox);
                    
                    if (chkPanjeng.Checked == true)
                    {
                        strOK = "OK";
                        break;
                    }
                }

                if (strOK == "")
                {
                    MessageBox.Show("종합소견이 없습니다.", "저장불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                strUsik = "";
                strGyeol = "";
                for (int i = 0; i <= 32; i++)
                {
                    TextBox txtUDt = (Controls.Find("txtUDt" + i.ToString(), true)[0] as TextBox);
                    TextBox txtDt = (Controls.Find("txtDt" + i.ToString(), true)[0] as TextBox);
                    if (txtUDt.Text.Trim() != "")
                    {
                        strUsik += i + ",";
                    }
                    if (txtDt.Text.Trim() != "")
                    {
                        strGyeol += i + ",";
                    }
                }

                if (rdoGingivitis1.Checked == true) //치은염
                {
                    strChiEun = "0";
                }
                else
                {
                    strChiEun = "1";
                }

                if (rdoPeriodontitis1.Checked == true)  //치주염
                {
                    strChiJu = "0";
                }
                else
                {
                    strChiJu = "1";
                }

                if (rdoToothBrush1.Checked == true)  //치근막염
                {
                    strChiGun = "0";
                }
                else
                {
                    strChiGun = "1";
                }

                if (rdoArtificialTeeth1.Checked == true)    //의치
                {
                    strDenture = "0";
                }
                else if (rdoArtificialTeeth2.Checked == true)
                {
                    strDenture = "1";
                }
                else if (rdoArtificialTeeth3.Checked == true)
                {
                    strDenture = "2";
                }
                else if (rdoArtificialTeeth4.Checked == true)
                {
                    strDenture = "3";
                }
                strDentureEtc = txtArtificialTeeth.Text.Trim();

                //종합소견
                if (chkPanjeng0.Checked == true) { strTPanjeng[0] = "1"; }
                if (chkPanjeng1.Checked == true) { strTPanjeng[1] = "1"; }
                if (chkPanjeng2.Checked == true) { strTPanjeng[2] = "1"; }
                if (chkPanjeng3.Checked == true) { strTPanjeng[3] = "1"; }
                if (chkPanjeng4.Checked == true) { strTPanjeng[4] = "1"; }
                if (chkPanjeng5.Checked == true) { strTPanjeng[5] = "1"; }
                if (chkPanjeng6.Checked == true) { strTPanjeng[6] = "1"; }
                if (chkPanjeng7.Checked == true) { strTPanjeng[7] = "1"; }
                if (chkPanjeng8.Checked == true) { strTPanjeng[8] = "1"; }
                if (chkPanjeng9.Checked == true) { strTPanjeng[9] = "1"; }
                if (chkPanjeng10.Checked == true) { strTPanjeng[10] = "1"; }
                if (chkPanjeng11.Checked == true) { strTPanjeng[11] = "1"; }
                if (chkPanjeng12.Checked == true) { strTPanjeng[12] = "1"; }
                if (chkPanjeng13.Checked == true) { strTPanjeng[13] = "1"; }
                if (chkPanjeng8.Checked == true)
                {
                    strTPanjengEtc = txtPanjengEtc.Text.Trim();
                }

                HEA_DENTAL item = new HEA_DENTAL();

                item.GBSTS = "Y";
                item.PANJENGDRNO = txtPanDrNo.Text.Trim().To<long>();
                item.USIK = strUsik;
                item.GYEOLSON = strGyeol;
                item.CHIEUN = strChiEun;
                item.CHIJU = strChiJu;
                item.CHIGUNMAK = strChiGun;
                item.DENTURE = strDenture;
                item.DENTURE_ETC = strDentureEtc;
                item.PANJENG1 = strTPanjeng[0];
                item.PANJENG2 = strTPanjeng[1];
                item.PANJENG3 = strTPanjeng[2];
                item.PANJENG4 = strTPanjeng[3];
                item.PANJENG5 = strTPanjeng[4];
                item.PANJENG6 = strTPanjeng[5];
                item.PANJENG7 = strTPanjeng[6];
                item.PANJENG8 = strTPanjeng[7];
                item.PANJENG9 = strTPanjeng[8];
                item.PANJENG11 = strTPanjeng[9];
                item.PANJENG12 = strTPanjeng[10];
                item.PANJENG13 = strTPanjeng[11];
                item.PANJENG14 = strTPanjeng[12];
                item.PANJENG15 = strTPanjeng[13];
                item.PANJENG10 = strTPanjengEtc;
                item.WRTNO = FnWRTNO;

                result = heaDentalService.Update(item);

                if (result < 0)
                {
                    MessageBox.Show("상담Flag 저장시 오류 발생(HEA_DENTAL)", "");
                    return;
                }

                strMsg = "";
                //종검결과 Data에 요약정리 및 전송
                //우식증 여부 확인
                if (strUsik != "")
                {
                    strMsg = "▶우식증: " + (VB.L(strUsik, ",") - 1).ToString() + "개";                    
                }

                //결손치 여부 확인
                if (strGyeol != "")
                {
                    strMsg += "▶결손치: " + (VB.L(strGyeol, ",") - 1).ToString() + "개";
                }

                //치은염, 치주염, 치근막염
                if (strChiEun == "0" || strChiJu == "0" || strChiGun == "0")
                {
                    strMsg += "▶치주질환: ";
                    if (strChiEun == "0") strMsg += "치은염,";
                    if (strChiJu == "0") strMsg += "치주염,";
                    if (strChiGun == "0") strMsg += "치주농루증(풍치),";
                    if (VB.Right(strMsg, 1) == ",") strMsg = VB.Left(strMsg, strMsg.Length - 1) + " ";
                }

                //의치
                if (strDenture != "0")
                {
                    switch (strDenture)
                    {
                        case "1":
                            strMsg += "(의치양호)";
                            break;
                        case "2":
                            strMsg += "(의치불량)";
                            break;
                        case "3":
                            strMsg += "(의치필요)";
                            break;
                        default:
                            break;
                    }

                    strMsg += "\r\n";
                }

                //종합소견
                strMsg += "[종합소견] ";
                for (int i = 0; i <= 13; i++)
                {
                    if (i != 8)
                    {   
                        CheckBox chkPanjeng = (Controls.Find("chkPanjeng" + i.ToString(), true)[0] as CheckBox);
                        if (chkPanjeng.Checked == true)
                        {
                            strMsg += chkPanjeng.Text + ",";
                        }                        
                    }
                    else
                    {
                        CheckBox chkPanjeng = (Controls.Find("chkPanjeng" + i.ToString(), true)[0] as CheckBox);
                        if (chkPanjeng.Checked == true)
                        {
                            strMsg += "기타:" + strTPanjengEtc + "(이)가 필요합니다." + ",";
                        }
                    }
                }

                if (VB.Right(strMsg, 1) == ".")
                {
                    strMsg = VB.Left(strMsg, strMsg.Length - 1) + " ";
                }

                string[] strCodes = new string[] { "ZD00" };

                HIC_RESULT item2 = new HIC_RESULT();

                item2.RESULT = strMsg;
                item2.ACTIVE = "Y";
                item2.ENTSABUN = clsType.User.IdNumber.To<long>();
                item2.WRTNO = FnWRTNO;

                result = hicResultService.Update_Result_ChulAutoFlag(item2, strCodes);

                if (result < 0)
                {
                    MessageBox.Show("구강상담결과 Data저장시 오류 발생(HEA_RESULT)", "전산실 연락요망", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (heaSangdamWaitService.GetRowIdbyEndoRoom(FnWRTNO, clsHcVariable.DENT_ROOM) > 0)
                {
                    string[] strJongSQL;

                    strJongSQL = new string[] { clsHcVariable.DENT_ROOM };

                    result = heaSangdamWaitService.Update_Sangdam_GbCall(FnWRTNO, strJongSQL);

                    if (result < 0)
                    {
                        MessageBox.Show("구강상담결과 Data저장시 오류 발생(HEA_SANGDAM_WAIT)", "전산실 연락요망", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    HEA_SANGDAM_WAIT item3 = new HEA_SANGDAM_WAIT();

                    item3.WRTNO = FnWRTNO;
                    item3.SNAME = strSname;
                    item3.SEX = strSEX;
                    item3.AGE = nAge;
                    item3.GJJONG = strGJJONG;
                    item3.GBCALL = "Y";
                    item3.GUBUN = clsHcVariable.DENT_ROOM;
                    item3.WAITNO = 0;

                    result = heaSangdamWaitService.Insert_Sangdam_Wait(item3);

                    if (result < 0)
                    {
                        MessageBox.Show("상담대기 순번등록 중 오류 발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                fn_Screen_Clear();
            }
        }

        void fn_Screen_Display()
        {
            string strSex = "";
            string strTemp = "";
            //string strExCode = "";
            int ii = 0;
            int result = 0;

            fn_Screen_Clear();

            btnSave.Enabled = true;
            pnlMain.Enabled = true;

            fn_Screen_Injek_display();  //인적사항을 Display            

            FstrROWID = "";
            FstrROWID = heaDentalService.GetRowIdbyWrtNo(FnWRTNO);

            string[] strExCode = new string[] { "ZD00" };            

            //상담테이블 없을 시 상담테이블 생성함
            if (FstrROWID == "")
            {
                HEA_DENTAL item = new HEA_DENTAL();

                item.WRTNO = FnWRTNO;
                item.JEPDATE = FstrSdate;

                if (hicResultService.GetCountbyWrtNo(FnWRTNO, strExCode) > 0)
                {
                    result = heaDentalService.Insert(item);

                    if (result < 0)
                    {
                        MessageBox.Show("구강상담 테이블 생성 도중 ERROR 발생!!", "전산실연락요망!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            if (clsHcVariable.GbHeaAdminSabun == true)
            {
                fn_Update_Patient_Call();
            }

            //상담 및 판정내역 DISPLAY
            if (FstrROWID != "")
            {
                //HEA_DENTAL list = heaDentalService.GetItemAllbyWrtNo(FnWRTNO, txtPanDrNo.Text.Trim());
                HEA_DENTAL list = heaDentalService.GetItemAllbyWrtNo(FnWRTNO, clsHcVariable.GnHicLicense.To<string>());
                if (!list.IsNullOrEmpty())
                {
                    //우식증
                    strTemp = "";
                    pnlUsik.Visible = true;
                    pnlUsik.BringToFront();
                    pnlGyeol.Visible = false;
                    strTemp = list.USIK;
                    lblToothdecay.Text = list.USIK;
                    if (VB.L(strTemp, ",") > 1)
                    {
                        for (int i = 1; i < VB.L(strTemp, ","); i++)
                        {
                            ii = VB.Pstr(strTemp, ",", i).To<int>();
                            TextBox txtUDt = (Controls.Find("txtUDt" + ii.ToString(), true)[0] as TextBox);
                            txtUDt.Text = "Ｘ";
                        }
                    }

                    if (list.CHIEUN == "0")
                    {
                        rdoGingivitis1.Checked = true;
                    }
                    if (list.CHIJU == "0")
                    {
                        rdoPeriodontitis1.Checked = true;
                    }
                    if (list.CHIGUNMAK == "0")
                    {
                        rdoToothBrush1.Checked = true;
                    }

                    if (list.DENTURE == "0")
                    {
                        rdoArtificialTeeth1.Checked = true;
                    }
                    else if (list.DENTURE == "1")
                    {
                        rdoArtificialTeeth2.Checked = true;
                    }
                    else if (list.DENTURE == "2")
                    {
                        rdoArtificialTeeth3.Checked = true;
                    }

                    if (!list.DENTURE_ETC.IsNullOrEmpty())
                    {
                        txtArtificialTeeth.Text = list.DENTURE_ETC;
                    }

                    if (list.PANJENG1 == "1") chkPanjeng0.Checked = true;
                    if (list.PANJENG2 == "1") chkPanjeng1.Checked = true;
                    if (list.PANJENG3 == "1") chkPanjeng2.Checked = true;
                    if (list.PANJENG4 == "1") chkPanjeng3.Checked = true;
                    if (list.PANJENG5 == "1") chkPanjeng4.Checked = true;
                    if (list.PANJENG6 == "1") chkPanjeng5.Checked = true;
                    if (list.PANJENG7 == "1") chkPanjeng6.Checked = true;
                    if (list.PANJENG8 == "1") chkPanjeng7.Checked = true;
                    if (list.PANJENG9 == "1") chkPanjeng8.Checked = true;
                    if (list.PANJENG11 == "1") chkPanjeng9.Checked = true;
                    if (list.PANJENG12 == "1") chkPanjeng10.Checked = true;

                    if (list.PANJENG13 == "1") chkPanjeng11.Checked = true;
                    if (list.PANJENG14 == "1") chkPanjeng12.Checked = true;
                    if (list.PANJENG15 == "1") chkPanjeng13.Checked = true;

                    if (list.PANJENG10 != "")
                    {
                        txtPanjengEtc.Text = list.PANJENG10;
                    }

                    txtPanDrNo.Text = list.PANJENGDRNO.To<string>();
                    lblDrName.Text = hb.READ_License_DrName(list.PANJENGDRNO);

                    if (txtPanDrNo.Text.To<long>() > 0 && txtPanDrNo.Text.To<long>() != clsHcVariable.GnHicLicense)
                    {
                        btnSave.Enabled = false;
                    }
                }
            }

            if (txtPanDrNo.Text == "")
            {
                txtPanDrNo.Text = clsHcVariable.GnHicLicense.To<string>();
                lblDrName.Text = hb.READ_License_DrName(txtPanDrNo.Text.To<long>());
                btnSave.Enabled = true;
            }
        }

        void fn_Update_Patient_Call()
        {
            int result = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);

            List<HEA_SANGDAM_WAIT> list = heaSangdamWaitService.GetcountbyWrtNoRoom(FnWRTNO, clsHcVariable.DENT_ROOM);

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    result = heaSangdamWaitService.Update_Sangdam_CallTime(list[i].WRTNO, "");

                    if (result < 0)
                    {
                        MessageBox.Show("상담 호출시간 갱신중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            result = heaSangdamWaitService.Update_Patient_Call(FnWRTNO, null);

            if (result < 0)
            {
                MessageBox.Show("상담 호출시간 갱신중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        void fn_Screen_Injek_display()
        {
            string strSex = "";

            //인적사항을 Read
            HEA_JEPSU list = heaJepsuService.GetHeaJepsubyWrtNo(FnWRTNO);

            if (list.IsNullOrEmpty())
            {
                MessageBox.Show("접수번호가 등록 안됨", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            FstrPtno = list.PTNO;
            FnPano = list.PANO;
            FstrSdate = list.SDATE;

            SS1.ActiveSheet.RowCount = 1;
            SS1_Sheet1.SetRowHeight(-1, 28);

            SS1.ActiveSheet.Cells[0, 0].Text = list.PTNO;
            SS1.ActiveSheet.Cells[0, 1].Text = list.SNAME;
            SS1.ActiveSheet.Cells[0, 2].Text = list.SEXAGE;
            SS1.ActiveSheet.Cells[0, 3].Text = hb.READ_Ltd_Name(list.LTDCODE.To<string>());
            SS1.ActiveSheet.Cells[0, 4].Text = list.SDATE;
            SS1.ActiveSheet.Cells[0, 5].Text = list.GJJONG + "." + hb.READ_GjJong_HeaName(list.GJJONG);
        }

        /// <summary>
        /// 거래처코드 찾기 화면 스프레드더블클릭이벤트
        /// </summary>
        /// <param name="item"></param>
        private void LtdCd_value(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        void eSpdClick(object sender, CellClickEventArgs e)
        {
            if (sender == SSList)
            {
                if (e.ColumnHeader == true)
                {
                    sp.setSpdSort(SS1, e.Column, true);
                }
            }
        }

        void fn_SS_SEL_Clear()
        {
            SS_SEL.ActiveSheet.Cells[0, 2, SS_SEL.ActiveSheet.RowCount - 1, 2].CellType = chk;
            SS_SEL.ActiveSheet.Cells[0, 2, SS_SEL.ActiveSheet.RowCount - 1, 2].Text = "";
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SSList)
            {
                FnWRTNO = SSList.ActiveSheet.Cells[e.Row, 0].Text.To<long>();
                txtWrtNo.Text = SSList.ActiveSheet.Cells[e.Row, 0].Text;
                FstrJepDate = "20" + SSList.ActiveSheet.Cells[e.Row, 2].Text;
                FstrROWID = SSList.ActiveSheet.Cells[e.Row, 5].Text;

                fn_Screen_Display();

                //sp.Spread_All_Clear(SS_SEL);

                fn_SS_SEL_Clear();
                fn_READ_SS_SEL_SET(FnWRTNO, FstrSdate);

                if (chkMunjin.Checked == true)
                {
                    if (hf.OpenForm_Check("frmHcSangInternetMunjinView") == true)
                    {
                        FrmHcSangInternetMunjinView.Dispose();
                        FrmHcSangInternetMunjinView = null;
                    }
                    
                    FrmHcSangInternetMunjinView = new frmHcSangInternetMunjinView(FnWRTNO, FstrJepDate, FstrPtno, FstrGjJong, FstrROWID);
                    FrmHcSangInternetMunjinView.StartPosition = FormStartPosition.CenterParent;
                    FrmHcSangInternetMunjinView.ShowDialog(this);
                }
            }
        }

        void fn_READ_SS_SEL_SET(long argWrtNo, string argSDate)
        {
            int nRead = 0;
            int nCnt = 0;

            string strPart = "";
            string strRoom = "";

            string strOK1 = "";
            string strOK2 = "";
            string strOK3 = "";
            string strOK4 = "";

            string strFrDate = "";
            string strToDate = "";

            strFrDate = argSDate;
            strToDate = DateTime.Parse(argSDate).AddDays(1).ToShortDateString();

            for (int i = 0; i < SS_SEL.ActiveSheet.NonEmptyRowCount; i++)
            {
                strRoom = SS_SEL.ActiveSheet.Cells[i, 3].Text.Trim();

                strOK1 = "";
                strOK2 = "";
                strOK3 = "";
                strOK4 = "";

                //검사실 대기상황
                nCnt = heaSangdamWaitService.GetCountbyEntTimeGubun(strFrDate, strToDate, strRoom);

                //검사실 이미 등록여부
                HEA_SANGDAM_WAIT list = heaSangdamWaitService.GetGbCallGbDentbyEntTimeGubun(strFrDate, strToDate, strRoom, argWrtNo);

                if (!list.IsNullOrEmpty())
                {
                    strOK1 = "OK";
                    if (list.GBCALL == "Y")
                    {
                        strOK1 = "OK2";
                    }

                    if (list.GBDENT == "Y")
                    {
                        strOK3 = "OK";
                    }
                }

                //액팅 상태점검
                if (heaResultService.GetCountbyWrtNo(argWrtNo, strRoom) > 0)
                {
                    strOK2 = "OK";  //액팅한개라도 있으면
                }

                //접수방 선택시 체크
                if (heaDentalService.GetCountbyWrtNo(argWrtNo) > 0)
                {
                    strOK4 = "OK";
                }

                SS_SEL.ActiveSheet.Cells[i, 4].Text = nCnt.To<string>();
                SS_SEL.ActiveSheet.Cells[i, 0].BackColor = Color.FromArgb(255, 255, 255);



                if (strOK1 == "OK" || strOK1 == "OK2")
                {
                    //검사방에 이미 등록된경우
                    SS_SEL.ActiveSheet.Cells[i, 0].BackColor = Color.FromArgb(255, 255, 200);

                    SS_SEL.ActiveSheet.Cells[i, 2].CellType = txt;
                    SS_SEL.ActiveSheet.Cells[i, 2].Text = "";
                }

                if (strOK2 == "OK")
                {
                    //액팅이 한개라도 있으면
                    SS_SEL.ActiveSheet.Cells[i, 2].CellType = txt;
                    SS_SEL.ActiveSheet.Cells[i, 2].Text = "";
                }

                if (strOK3 == "OK")
                {
                    SS_SEL.ActiveSheet.Cells[i, 0].BackColor = Color.FromArgb(147, 201, 255);
                }

                //해당방의 액팅이 없으면 체크 못하게 비활성화
                if (heaResultService.GetCountbyWrtNoHaRoom(argWrtNo, strRoom) == 0)  //액팅한개라도 없으면
                {
                    SS_SEL.ActiveSheet.Cells[i, 2].CellType = txt;
                    SS_SEL.ActiveSheet.Cells[i, 2].Text = "";
                }
            }

            if (strOK4 == "OK")
            {
                SS_SEL.ActiveSheet.Cells[SS_SEL.ActiveSheet.RowCount - 1, 0].BackColor = Color.FromArgb(147, 201, 255);
            }
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtLtdCode)
            {
                if (e.KeyChar == (char)13)
                {
                    eBtnClick(btnLtdCode, new EventArgs());
                }
            }
            else if (sender == txtWrtNo)
            {
                string strTemp = "";
                string strPtNo = "";

                if (txtWrtNo.Text.To<long>() == 0)
                {
                    return;
                }

                if (e.KeyChar == 13)
                {
                    strTemp = txtWrtNo.Text;
                    fn_Screen_Clear();
                    txtWrtNo.Text = strTemp;

                    if (txtWrtNo.Text.Trim().Length > 6)
                    {
                        strPtNo = VB.Pstr(txtWrtNo.Text, " ", 1).Trim();
                        txtWrtNo.Text = "";
                        //외래번호로 접수번호 찾기
                        HEA_JEPSU list2 = heaJepsuService.GetWrtNobyPtNo(strPtNo);

                        if (list2 != null)
                        {
                            txtWrtNo.Text = list2.WRTNO.To<string>();
                            FnWRTNO = txtWrtNo.Text.To<long>();
                        }
                        else
                        {
                            MessageBox.Show("(" + txtWrtNo.Text.Trim() + ") 금일 접수된 번호가 아닙니다. 외래번호(OPD) 확인요망!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtWrtNo.Text = "";
                            txtWrtNo.Focus();
                            return;
                        }
                    }

                    fn_Screen_Display();
                    fn_READ_SS_SEL_SET(FnWRTNO, FstrSdate);

                    txtWrtNo.SelectionStart = 0;
                    txtWrtNo.SelectionLength = txtWrtNo.Text.Length;

                    if (chkMunjin.Checked == true)
                    {
                        if (hf.OpenForm_Check("frmHcSangInternetMunjinView") == true)
                        {
                            FrmHcSangInternetMunjinView.Dispose();
                            FrmHcSangInternetMunjinView = null;
                        }

                        FrmHcSangInternetMunjinView = new frmHcSangInternetMunjinView(FnWRTNO, FstrJepDate, FstrPtno, FstrGjJong, FstrROWID);
                        FrmHcSangInternetMunjinView.StartPosition = FormStartPosition.CenterParent;
                        FrmHcSangInternetMunjinView.ShowDialog(this);
                    }
                }
            }
        }

        void eTxtDblClick(object sender, EventArgs e)
        {
            if (sender == txtUdt0  || sender == txtUdt1  || sender == txtUdt2  || sender == txtUdt3  || sender == txtUdt4  || sender == txtUdt5  ||
                sender == txtUdt6  || sender == txtUdt7  || sender == txtUdt8  || sender == txtUdt9  || sender == txtUdt10 || sender == txtUdt11 ||
                sender == txtUdt12 || sender == txtUdt13 || sender == txtUdt14 || sender == txtUdt15 || sender == txtUdt16 || sender == txtUdt17 ||
                sender == txtUdt18 || sender == txtUdt19 || sender == txtUdt20 || sender == txtUdt21 || sender == txtUdt22 || sender == txtUdt23 ||
                sender == txtUdt24 || sender == txtUdt25 || sender == txtUdt26 || sender == txtUdt27 || sender == txtUdt28 || sender == txtUdt29 ||
                sender == txtUdt30 || sender == txtUdt31 || sender == txtUdt32)
            {
                lblToothdecay.Text = "";                
                for (int i = 0; i <= 32; i++)
                {
                    TextBox txtUdt = (Controls.Find("txtUdt" + i.ToString(), true)[0] as TextBox);

                    if (sender == txtUdt)
                    {
                        if (txtUdt.Text == "")
                        {
                            txtUdt.Text = "Ｘ";
                        }
                        else
                        {
                            txtUdt.Text = "";
                        }
                    }

                    if (txtUdt.Text != "")
                    {
                        lblToothdecay.Text += string.Format("{0:#0}", i) + ",";
                    }                    
                }
            }
            else if (sender == txtDt0  || sender == txtDt1  || sender == txtDt2  || sender == txtDt3  || sender == txtDt4  || sender == txtDt5  ||
                     sender == txtDt6  || sender == txtDt7  || sender == txtDt8  || sender == txtDt9  || sender == txtDt10 || sender == txtDt11 ||
                     sender == txtDt12 || sender == txtDt13 || sender == txtDt14 || sender == txtDt15 || sender == txtDt16 || sender == txtDt17 ||
                     sender == txtDt18 || sender == txtDt19 || sender == txtDt20 || sender == txtDt21 || sender == txtDt22 || sender == txtDt23 ||
                     sender == txtDt24 || sender == txtDt25 || sender == txtDt26 || sender == txtDt27 || sender == txtDt28 || sender == txtDt29 ||
                     sender == txtDt30 || sender == txtDt31 || sender == txtDt32)
            {
                lblToothDefect.Text = "";
                for (int i = 0; i <= 32; i++)
                {
                    TextBox txtDt = (Controls.Find("txtDt" + i.ToString(), true)[0] as TextBox);
                    if (sender == txtDt)
                    {
                        if (txtDt.Text == "")
                        {
                            txtDt.Text = "Ｘ";
                        }
                        else
                        {
                            txtDt.Text = "";
                        }
                    }
                    
                    if (txtDt.Text != "")
                    {
                        lblToothDefect.Text += string.Format("{0:#0}", i) + ",";
                    }                  
                }
            }
        }
    }
}
