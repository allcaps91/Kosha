using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Pan
/// File Name       : frmHcPanJindanSet.cs
/// Description     : 진단서 구분설정 및 판정의사설정
/// Author          : 이상훈
/// Create Date     : 2019-12-02
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmJindanSet.frm(HcPan111)" />

namespace ComHpcLibB
{
    public partial class frmHcPanJindanSet : Form
    {
        HicJepsuService hicJepsuService = null;
        HicResDentalService hicResDentalService = null;
        HicCodeService hicCodeService = null;
        HicJinGbnService hicJinGbnService = null;
        HicJepsuJinGbnService hicJepsuJinGbnService = null;
        HicBcodeService hicBcodeService = null;
        HicSunapdtlService hicSunapdtlService = null;

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        List<string> lstJindan = new List<string>();        //진단서

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        long FnWRTNO;
        long FnWRTNO1;

        public frmHcPanJindanSet()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmHcPanJindanSet(long argWrtno)
        {
            InitializeComponent();
            SetEvent();
            SetControl();

            FnWRTNO1 = argWrtno;
        }


        void SetEvent()
        {
            hicJepsuService = new HicJepsuService();
            hicResDentalService = new HicResDentalService();
            hicCodeService = new HicCodeService();
            hicJinGbnService = new HicJinGbnService();
            hicJepsuJinGbnService = new HicJepsuJinGbnService();
            hicBcodeService = new HicBcodeService();
            hicSunapdtlService = new HicSunapdtlService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);

            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);

            this.txtPanDrNo.GotFocus += new EventHandler(etxtGotFocus);
            this.txtPanDrNo.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtWrtNo.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtPanDrNo.KeyUp += new KeyEventHandler(eTxtKeyUp);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eKeyPress);
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            hf.SET_자료사전_VALUE();

            List<HIC_BCODE> list = hicBcodeService.GetCodebyGubun("HIC_진단서구분등록");
            for (int j = 0; j < list.Count; j++)
            {
                lstJindan.Add(list[j].CODE.Trim());
            }


            txtLtdCode.Text = "";
            dtpFrDate.Text = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-20).ToShortDateString();
            dtpToDate.Text = clsPublic.GstrSysDate;
            SSList.ActiveSheet.RowCount = 50;

            sp.Spread_All_Clear(SSList);
            sp.Spread_Clear_Range(ssPatInfo, 0, 0, 0, ssPatInfo.ActiveSheet.ColumnCount - 1);

            fn_Read_Jin();
            fn_Screen_Clear();

            txtWrtNo.Text = FnWRTNO.ToString();
            
            pnlList.Enabled = true;
            SSList_Sheet1.Columns.Get(6).Visible = false;


            if (FnWRTNO1 > 0)
            {
                txtWrtNo.Text = FnWRTNO1.ToString();
                eTxtKeyPress(txtWrtNo, new KeyPressEventArgs((char)13));
                eTxtKeyUp(txtPanDrNo, new KeyEventArgs(Keys.F7));
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
            else if (sender == btnSave)
            {
                string strRowId = "";
                string strGbn = "";
                int result = 0;

                strGbn = VB.Left(cboGbn.Text, 2);

                if (txtWrtNo.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("저장할 대상을 먼저 선택하세요!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                strRowId = hicJinGbnService.GetItembyWrtNo(long.Parse(txtWrtNo.Text));

                clsDB.setBeginTran(clsDB.DbCon);

                HIC_JIN_GBN item = new HIC_JIN_GBN();

                item.WRTNO = txtWrtNo.Text.To<long>();
                item.GUBUN = strGbn;
                item.PANJENGDRNO = txtPanDrNo.Text.To<long>();
                item.ENTSABUN = clsType.User.IdNumber.To<long>();
                item.ROWID = strRowId;

                if (strRowId.IsNullOrEmpty())
                {
                    result = hicJinGbnService.Insert(item);
                }
                else
                {
                    result = hicJinGbnService.Update(item);
                }

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("진단서 구분 설정시 오류발생!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                fn_Screen_Clear();
                eBtnClick(btnSearch, new EventArgs());
            }
            else if (sender == btnSearch)
            {
                int nREAD = 0;
                string strList = "";
                int nRow = 0;
                string strOK = "";
                string strFrDate = "";
                string strToDate = "";
                long nLtdCode = 0;

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;
                if (!txtLtdCode.Text.IsNullOrEmpty())
                {
                    nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>();
                }
                else
                {
                    nLtdCode = 0;
                }

                cboGbn.SelectedIndex = 0;

                sp.Spread_All_Clear(SSList);
                SSList.ActiveSheet.RowCount = 50;

                SSList_Sheet1.Rows.Get(-1).Height = 24;

                //자료를 SELECT
                List<HIC_JEPSU_JIN_GBN> list = hicJepsuJinGbnService.GetItembyJepDate(strFrDate, strToDate, nLtdCode);

                nREAD = list.Count;
                //SSList.ActiveSheet.RowCount = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    strOK = "";
                    if (rdoJob1.Checked == true)    //신규
                    {
                        if (list[i].RID.IsNullOrEmpty())
                        {
                            strOK = "OK";
                        }
                    }
                    else
                    {
                        if (!list[i].RID.IsNullOrEmpty())
                        {
                            strOK = "OK";
                        }
                    }

                    if (hicSunapdtlService.GetCountbyWrtNOInCode(list[i].WRTNO, lstJindan) == 0) { strOK = ""; } ;

                    if (strOK == "OK")
                    {
                        nRow += 1;
                        if (SSList.ActiveSheet.RowCount < nRow)
                        {
                            SSList.ActiveSheet.RowCount = nRow;
                        }
                        SSList.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].WRTNO.To<string>();
                        SSList.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].SNAME;
                        SSList.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].JEPDATE;
                        SSList.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].GJJONG;
                        SSList.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].GUBUN;
                        SSList.ActiveSheet.Cells[nRow - 1, 5].Text = list[i].GUBUNNAME;
                        //switch (list[i].GUBUN)
                        //{
                        //    case "01":
                        //        SSList.ActiveSheet.Cells[nRow - 1, 5].Text = "면허발급용";
                        //        break;
                        //    case "02":
                        //        SSList.ActiveSheet.Cells[nRow - 1, 5].Text = "응급구조사";
                        //        break;
                        //    case "03":
                        //        SSList.ActiveSheet.Cells[nRow - 1, 5].Text = "의무기록사";
                        //        break;
                        //    case "04":
                        //        SSList.ActiveSheet.Cells[nRow - 1, 5].Text = "조리사";
                        //        break;
                        //    case "05":
                        //        SSList.ActiveSheet.Cells[nRow - 1, 5].Text = "영양사";
                        //        break;
                        //    case "06":
                        //        SSList.ActiveSheet.Cells[nRow - 1, 5].Text = "의사";
                        //        break;
                        //    case "07":
                        //        SSList.ActiveSheet.Cells[nRow - 1, 5].Text = "치과의사";
                        //        break;
                        //    case "08":
                        //        SSList.ActiveSheet.Cells[nRow - 1, 5].Text = "안경사";
                        //        break;
                        //    case "09":
                        //        SSList.ActiveSheet.Cells[nRow - 1, 5].Text = "조산사";
                        //        break;
                        //    case "10":
                        //        SSList.ActiveSheet.Cells[nRow - 1, 5].Text = "간호사";
                        //        break;
                        //    case "11":
                        //        SSList.ActiveSheet.Cells[nRow - 1, 5].Text = "간호조무사";
                        //        break;
                        //    case "12":
                        //        SSList.ActiveSheet.Cells[nRow - 1, 5].Text = "의료기사";
                        //        break;
                        //    case "13":
                        //        SSList.ActiveSheet.Cells[nRow - 1, 5].Text = "한의사";
                        //        break;
                        //    case "14":
                        //        SSList.ActiveSheet.Cells[nRow - 1, 5].Text = "위생사";
                        //        break;
                        //    case "15":
                        //        SSList.ActiveSheet.Cells[nRow - 1, 5].Text = "치위생사";
                        //        break;
                        //    case "16":
                        //        SSList.ActiveSheet.Cells[nRow - 1, 5].Text = "의지보조기사";
                        //        break;
                        //    case "17":
                        //        SSList.ActiveSheet.Cells[nRow - 1, 5].Text = "미용사";
                        //        break;
                        //    default:
                        //        break;
                        //}
                        SSList.ActiveSheet.Cells[nRow - 1, 6].Text = list[i].RID;
                    }
                }

                SSList.ActiveSheet.RowCount = nRow;

                btnSave.Enabled = false;
            }
        }

        void fn_Screen_Clear()
        {
            txtWrtNo.Text = "";
            cboGbn.SelectedIndex = 0;
            btnSave.Enabled = false;

            txtPanDrNo.Text = "";
            lblDrName.Text = "";
        }

        void fn_Screen_Display()
        {
            string strData = "";

            sp.Spread_Clear_Range(ssPatInfo, 0, 0, 0, ssPatInfo.ActiveSheet.ColumnCount - 1);

            FnWRTNO = long.Parse(txtWrtNo.Text);

            if (FnWRTNO == 0) return;

            //삭제된것 체크
            if (hb.READ_JepsuSTS(FnWRTNO) == "D")
            {
                MessageBox.Show(FnWRTNO + " 접수번호는 삭제된것 입니다. 확인하십시오", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //인적사항을 Display
            HIC_JEPSU_JIN_GBN list = hicJepsuJinGbnService.GetItembyWrtNo(FnWRTNO);

            if (list.IsNullOrEmpty())
            {
                MessageBox.Show("접수번호 " + FnWRTNO + " 번이 자료가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ssPatInfo.ActiveSheet.Cells[0, 0].Text = list.PANO.To<string>();
            ssPatInfo.ActiveSheet.Cells[0, 1].Text = list.SNAME;
            ssPatInfo.ActiveSheet.Cells[0, 2].Text = list.AGE + "/" + list.SEX;
            ssPatInfo.ActiveSheet.Cells[0, 3].Text = hb.READ_Ltd_Name(list.LTDCODE.To<string>());
            ssPatInfo.ActiveSheet.Cells[0, 4].Text = list.JEPDATE;
            ssPatInfo.ActiveSheet.Cells[0, 5].Text = hb.READ_GjJong_Name(list.GJJONG);

            if (list.PANJENGDRNO > 0)
            {
                txtPanDrNo.Text = list.PANJENGDRNO.To<string>();
                lblDrName.Text = hb.READ_License_DrName(txtPanDrNo.Text.To<long>());
            }
            btnSave.Enabled = true;
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SSList)
            {
                fn_Screen_Clear();
                txtWrtNo.Text = SSList.ActiveSheet.Cells[e.Row, 0].Text;
                cboGbn.SelectedIndex = SSList.ActiveSheet.Cells[e.Row, 4].Text.To<int>();
                fn_Screen_Display();
            }
        }

        void etxtGotFocus(object sender, EventArgs e)
        {
            if (sender == txtPanDrNo)
            {
                lblMsg.Text = clsHcVariable.B01_SANGDAM_DRLIST;
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (sender == txtPanDrNo)
                {
                    lblDrName.Text = hb.READ_License_DrName(txtPanDrNo.Text.To<long>());
                    SendKeys.Send("{TAB}");
                }

                if (sender == txtWrtNo)
                {
                 
                    if (FnWRTNO1 > 0)
                    {
                        fn_Screen_Clear();
                        txtWrtNo.Text = FnWRTNO1.ToString();
                        fn_Screen_Display();
                    }
                }
            }
        }

        void eTxtKeyUp(object sender, KeyEventArgs e)
        {
            long nDrNO = 0;

            if (sender == txtPanDrNo)
            {
                nDrNO = hf.B01_GET_Sangdam_DrNo(e.KeyCode);

                if (nDrNO > 0)
                {
                    txtPanDrNo.Text = nDrNO.To<string>();
                    lblDrName.Text = hb.READ_License_DrName(txtPanDrNo.Text.To<long>());
                    SendKeys.Send("{TAB}");
                }
            }
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
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

        void fn_Read_Jin()
        {
            List<HIC_CODE> list = hicCodeService.GetCodeNamebyGubun("J1");

            cboGbn.Items.Clear();
            cboGbn.Items.Add(" ");
            for (int i = 0; i < list.Count; i++)
            {
                cboGbn.Items.Add(list[i].CODE.Trim() + "." + list[i].NAME.Trim());
            }

            cboGbn.SelectedIndex = 0;
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
