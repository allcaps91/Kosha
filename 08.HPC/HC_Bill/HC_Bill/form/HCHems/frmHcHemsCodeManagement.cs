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
/// File Name       : frmHcHemsCodeManagement.cs
/// Description     : HEMS 전용 코드 등록 및 관리 작업
/// Author          : 이상훈
/// Create Date     : 2021-02-05
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmHems_코드등록.frm(FrmHems_코드등록)" />

namespace HC_Bill
{
    public partial class frmHcHemsCodeManagement : Form
    {
        HicHemsCodeService hicHemsCodeService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();
        clsHcBill chb = new clsHcBill();
        clsHcType hct = new clsHcType();
        clsPrint CP = new clsPrint();

        public frmHcHemsCodeManagement()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            hicHemsCodeService = new HicHemsCodeService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnView.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.btnOK.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);

            this.txtName.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.SS1.Change += new ChangeEventHandler(eSpdChange);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            int nRow = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);

            SS1_Sheet1.Columns[6].Visible = false;
            SS1_Sheet1.Columns[7].Visible = false;
            SS1_Sheet1.Columns[8].Visible = false;
            SS1_Sheet1.Columns[9].Visible = false;

            SS1_Sheet1.Rows[-1].Height = 20;

            cboJong.Items.Clear();
            List<HIC_HEMS_CODE> list = hicHemsCodeService.GetItembyGubunName("00", "");
            if (list.Count > 0)
            {
                cboJong.Items.Add("00.HEMS 기초코드");
                for (int i = 0; i < list.Count; i++)
                {
                    cboJong.Items.Add(list[i].CODE1 + "." + list[i].NAME);
                }
                cboJong.SelectedIndex = 0;
            }

            fn_Screen_Clear();
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
                string strJong = "";
                string strName = "";

                fn_Screen_Clear();
                cboJong.Focus();
            }
            else if (sender == btnView)
            {
                int nRead = 0;
                string strJong = "";
                string strName = "";

                pnlSearch.Enabled = false;
                SS1.Enabled = true;

                strJong = VB.Left(cboJong.Text, 2);
                strName = txtName.Text.Trim();

                List<HIC_HEMS_CODE> list = hicHemsCodeService.GetItembyGubunName(strJong, strName);

                SS1.ActiveSheet.RowCount = list.Count + txtRow.Text.To<int>();

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    SS1.ActiveSheet.Cells[i, 0].Text = "";
                    SS1.ActiveSheet.Cells[i, 1].Text = list[i].CODE1;
                    SS1.ActiveSheet.Cells[i, 2].Text = list[i].CODE2;
                    SS1.ActiveSheet.Cells[i, 3].Text = list[i].CODE3;
                    SS1.ActiveSheet.Cells[i, 4].Text = list[i].CODE4;
                    SS1.ActiveSheet.Cells[i, 5].Text = list[i].NAME;
                    SS1.ActiveSheet.Cells[i, 8].Text = list[i].RID;
                }

                btnOK.Enabled = true;
                btnCancel.Enabled = true;
                btnPrint.Enabled = true;

            }
            else if (sender == btnOK)
            {
                string strDel = "";
                string strCode1 = "";
                string strCode2 = "";
                string strCode3 = "";
                string strCode4 = "";

                string strTemp1 = "";
                string strTemp2 = "";

                string strName = "";
                string strGbDel = "";
                string strROWID = "";
                string strChange = "";

                int result = 0;

                //자료에 오류가 있는지 Check
                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strDel = SS1.ActiveSheet.Cells[i, 0].Text;

                    strCode1 = SS1.ActiveSheet.Cells[i, 1].Text;
                    strCode2 = SS1.ActiveSheet.Cells[i, 2].Text;
                    strCode3 = SS1.ActiveSheet.Cells[i, 3].Text;
                    strCode4 = SS1.ActiveSheet.Cells[i, 4].Text;

                    strName = SS1.ActiveSheet.Cells[i, 5].Text;
                    strTemp1 = SS1.ActiveSheet.Cells[i, 6].Text;

                    strGbDel = SS1.ActiveSheet.Cells[i, 7].Text;

                    if (strCode1.IsNullOrEmpty())
                    {
                        MessageBox.Show("코드가 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (strGbDel != "Y" && strGbDel != "")
                    {
                        MessageBox.Show("삭제여부 오류(삭제여부 Y=삭제 공란=사용중)", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                btnOK.Enabled = false;
                btnCancel.Enabled = false;
                btnPrint.Enabled = false;
                SS1.Enabled = false;

                clsDB.setBeginTran(clsDB.DbCon);

                for (int i = 0; i < SS1.ActiveSheet.NonEmptyRowCount; i++)
                {
                    strDel = SS1.ActiveSheet.Cells[i, 0].Text;
                    strCode1 = SS1.ActiveSheet.Cells[i, 1].Text;
                    strCode2 = SS1.ActiveSheet.Cells[i, 2].Text;
                    strCode3 = SS1.ActiveSheet.Cells[i, 3].Text;
                    strCode4 = SS1.ActiveSheet.Cells[i, 4].Text;

                    strName = SS1.ActiveSheet.Cells[i, 5].Text;
                    strTemp1 = SS1.ActiveSheet.Cells[i, 6].Text;

                    strGbDel = SS1.ActiveSheet.Cells[i, 7].Text;
                    strROWID = SS1.ActiveSheet.Cells[i, 8].Text;
                    strChange = SS1.ActiveSheet.Cells[i, 9].Text;

                    if (strCode2.Length > 6)
                    {
                        strCode2 = "";
                    }

                    HIC_HEMS_CODE item = new HIC_HEMS_CODE();

                    item.GUBUN = VB.Left(cboJong.Text, 2);
                    item.CODE1 = strCode1;
                    item.CODE2 = strCode2;
                    item.CODE3 = strCode3;
                    item.CODE4 = strCode4;
                    item.NAME = strName;

                    if (strROWID.IsNullOrEmpty())
                    {
                        if (strDel != "True")
                        {
                            result = hicHemsCodeService.Insert(item);
                        }
                        else
                        {
                            if (strDel == "True")
                            {
                                result = hicHemsCodeService.Delete(item);
                            }
                            else if (strChange == "Y")
                            {
                                result = hicHemsCodeService.Update(item);
                            }
                        }

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("DB에 자료를 등록시 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            btnOK.Enabled = true;
                            btnCancel.Enabled = true;
                            btnPrint.Enabled = true;
                            SS1.Enabled = true;
                            return;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                fn_Screen_Clear();
                cboJong.Focus();
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

                strTitle = "HEMS 코드집";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("출력일시: " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + "  PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);
                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            string strName = "";

            if (sender == txtName)
            {
                if (e.KeyChar == 13)
                {
                    eBtnClick(btnView, new EventArgs());
                }
            }
        }

        void eSpdChange(object sender, ChangeEventArgs e)
        {
            SS1.ActiveSheet.Cells[e.Row, 9].Text = "Y";
        }

        void fn_Screen_Clear()
        {
            sp.Spread_All_Clear(SS1);
            pnlSearch.Enabled = true;
            btnOK.Enabled = false;
            btnCancel.Enabled = false;
            btnPrint.Enabled = false;
            SS1.Enabled = false;
        }
    }
}

