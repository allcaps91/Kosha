using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Pan
/// File Name       : frmHcPanVivisectionsampleMgmt.cs
/// Description     : 생체시료(혈액,소변) 관리
/// Author          : 이상훈
/// Create Date     : 2019-12-24
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmH827.frm(FrmH827)" />

namespace HC_Pan
{
    public partial class frmHcPanVivisectionsampleMgmt : Form
    {
        HicJepsuResultService hicJepsuResultService = null;
        HicResultH827Service hicResultH827Service = null;
        ComHpcLibBService comHpcLibBService = null;
        HicBcodeService hicBcodeService = null;

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        public frmHcPanVivisectionsampleMgmt()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicJepsuResultService = new HicJepsuResultService();
            hicResultH827Service = new HicResultH827Service();
            comHpcLibBService = new ComHpcLibBService();
            hicBcodeService = new HicBcodeService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eKeyPress);
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFrDate.Text = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-90).ToShortDateString();
            dtpToDate.Text = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-1).ToShortDateString();
            txtLtdCode.Text = "";
            btnSave.Enabled = false;

            List<HIC_BCODE> list = hicBcodeService.GetCodeNamebyGubun("HIC_생체시료검사코드");

            cboExamMeterial.Items.Clear();
            cboExamMeterial.Items.Add("전체");
            for (int i = 0; i < list.Count; i++)
            {
                cboExamMeterial.Items.Add(list[i].CODE.Trim());
            }
            cboExamMeterial.SelectedIndex = 0;
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
            else if (sender == btnBuild)
            {
                int nREAD = 0;
                int nRow = 0;
                long nWRTNO = 0;
                string strJepDate = "";
                string strExCode = "";
                string strResult = "";
                string strOK = "";
                string strBloodDate = "";
                string strFrDate = "";
                string strToDate = "";
                int result = 0;

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;

                sp.Spread_All_Clear(SS1);
                SS1.ActiveSheet.RowCount = 30;

                List<HIC_JEPSU_RESULT> list = hicJepsuResultService.GetItembyJepDateExCode(strFrDate, strToDate);

                nREAD = list.Count;
                SS1.ActiveSheet.RowCount = nREAD;
                nRow = 0;

                clsDB.setBeginTran(clsDB.DbCon);

                for (int i = 0; i < nREAD; i++)
                {
                    nWRTNO = list[i].WRTNO;
                    strJepDate = list[i].JEPDATE;
                    strExCode =  list[i].EXCODE;
                    strResult = list[i].RESULT;
                    strBloodDate = "";
                    if (!strResult.IsNullOrEmpty() && strResult != "미실시")
                    {
                        strBloodDate = clsPublic.GstrSysDate;
                    }

                    HIC_RESULT_H827 item = new HIC_RESULT_H827();

                    item.WRTNO = nWRTNO;
                    item.PANO = list[i].PANO;
                    item.JEPDATE = strJepDate;
                    item.EXCODE = strExCode;
                    item.BLOODDATE = strBloodDate;
                    item.REMARK = "";
                    if (strBloodDate.IsNullOrEmpty())
                    {
                        item.ENTSABUN = long.Parse("");
                    }
                    else
                    {
                        item.ENTSABUN = long.Parse(clsType.User.IdNumber);
                    }

                    HIC_RESULT_H827 list2 = hicResultH827Service.GetBloodDatebyWrtNoExCode(nWRTNO, strExCode);

                    if (list2.IsNullOrEmpty())
                    {
                        if (strBloodDate.IsNullOrEmpty())
                        {
                            result = hicResultH827Service.Insert(item);
                        }
                    }
                    else
                    {
                        strOK = "";
                        if (list2.BLOODDATE.ToString().IsNullOrEmpty() && !strBloodDate.IsNullOrEmpty())
                        {
                            strOK = "OK";
                        }

                        if (strOK == "OK")
                        {
                            result = hicResultH827Service.Update(item, strBloodDate);
                        }
                    }

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("채혈대상 자동등록중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    SS1.ActiveSheet.Cells[i, 0].Text = strJepDate;
                    SS1.ActiveSheet.Cells[i, 5].Text = nWRTNO.To<string>();
                    SS1.ActiveSheet.Cells[i, 3].Text = list[i].SNAME;
                    SS1.ActiveSheet.Cells[i, 6].Text = " " + hb.READ_Ltd_Name(list[i].LTDCODE.To<string>());
                    SS1.ActiveSheet.Cells[i, 9].Text = list[i].EXCODE;
                    SS1.ActiveSheet.Cells[i, 10].Text = strResult;
                }
                //접수취소자 삭제
                result = hicResultH827Service.Delete(strFrDate, strToDate);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("접수취소자 삭제중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                if (SS1.ActiveSheet.RowCount == 0)
                {
                    MessageBox.Show("신규 자료가 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(SS1.ActiveSheet.RowCount + "건 처리를 하였습니다", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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

                strTitle = "생체시료(혈액,소변) 관리";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 14, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("회사명:" + VB.Pstr(txtLtdCode.Text, ".", 2), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
            else if (sender == btnSave)
            {
                long nWRTNO = 0;
                string strBloodDate = "";
                string strRemark = "";
                int result = 0;

                if (MessageBox.Show("참고사항을 저장 하시겠습니까?", "확인", MessageBoxButtons.OK, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                clsDB.setBeginTran(clsDB.DbCon);

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    nWRTNO = long.Parse(SS1.ActiveSheet.Cells[i, 1].Text);
                    strRemark = SS1.ActiveSheet.Cells[i, 5].Text;

                    if (strRemark.Length > 100)
                    {
                        MessageBox.Show(i + "번줄 참고사항이 100자를 초과하여 저장 불가", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        result = hicResultH827Service.UpdateRemarkbyWrtNo(strRemark, nWRTNO);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("참고사항 저장시 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                MessageBox.Show("저장 완료!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (sender == btnSearch)
            {
                int nREAD = 0;
                int nRow = 0;
                long nWRTNO = 0;
                string strJepDate = "";
                string strResult = "";
                string strOK = "";
                string strBloodDate = "";
                string strFrDate = "";
                string strToDate = "";
                string strJob = "";
                string strExamMeterial = "";
                long nLtdCode = 0;

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;
                strExamMeterial = cboExamMeterial.Text;
                if (rdoJob1.Checked == true)
                {
                    strJob = "1";
                }
                else if (rdoJob2.Checked == true)
                {
                    strJob = "2";
                }
                else if (rdoJob3.Checked == true)
                {
                    strJob = "3";
                }

                txtLtdCode.Text = txtLtdCode.Text.Trim();
                nLtdCode = txtLtdCode.Text.To<long>();

                sp.Spread_All_Clear(SS1);
                SS1.ActiveSheet.RowCount = 30;

                List<COMHPC> list = comHpcLibBService.GetItembyJepDate(strFrDate, strToDate, strJob, strExamMeterial, nLtdCode);

                nREAD = list.Count;
                SS1.ActiveSheet.RowCount = nREAD;
                nRow = 0;
                for (int i = 0; i < nREAD; i++)
                {
                    nWRTNO = list[i].WRTNO;
                    strJepDate = list[i].JEPDATE;
                    strBloodDate = list[i].BLOODDATE;
                    strResult = list[i].RESULT.Trim();
                    SS1.ActiveSheet.Cells[i, 0].Text = strJepDate;
                    SS1.ActiveSheet.Cells[i, 1].Text = list[i].GJJONG;
                    SS1.ActiveSheet.Cells[i, 2].Text = list[i].PANO.To<string>();
                    SS1.ActiveSheet.Cells[i, 3].Text = list[i].SNAME;
                    SS1.ActiveSheet.Cells[i, 4].Text = list[i].AGE + "/" + list[i].SEX;
                    SS1.ActiveSheet.Cells[i, 5].Text = nWRTNO.To<string>();
                    SS1.ActiveSheet.Cells[i, 6].Text = hb.READ_Ltd_Name(list[i].LTDCODE.To<string>());
                    SS1.ActiveSheet.Cells[i, 7].Text = list[i].BUSENAME;
                    SS1.ActiveSheet.Cells[i, 8].Text = list[i].JUMIN;
                    SS1.ActiveSheet.Cells[i, 9].Text = list[i].EXCODE;
                    SS1.ActiveSheet.Cells[i, 10].Text = list[i].HNAME;
                    SS1.ActiveSheet.Cells[i, 11].Text = strResult;
                    SS1.ActiveSheet.Cells[i, 12].Text = " " + list[i].REMARK;
                }

                btnSave.Enabled = false;
                if (rdoJob1.Checked == true)
                {
                    btnSave.Enabled = true;
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
