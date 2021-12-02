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
/// File Name       : frmHcHemsCorpCodeManagement.cs
/// Description     : 공단코드 등록 - 병원기초코드와 동일함
/// Author          : 이상훈
/// Create Date     : 2021-02-06
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmHems_코드등록.frm(FrmHems_코드등록)" />

namespace HC_Bill
{
    public partial class frmHcHemsCorpCodeManagement : Form
    {
        HicCodeService hicCodeService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();
        clsHcBill chb = new clsHcBill();
        clsHcType hct = new clsHcType();
        clsPrint CP = new clsPrint();

        public frmHcHemsCorpCodeManagement()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            hicCodeService = new HicCodeService();

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

            SS1_Sheet1.Columns[8].Visible = false;
            SS1_Sheet1.Columns[9].Visible = false;

            SS1_Sheet1.Rows[-1].Height = 20;

            cboJong.Items.Clear();
            cboJong.Items.Add("12" + "." + "HEMS용 사후관리코드매칭");
            cboJong.Items.Add("14" + "." + "HEMS용 조치코드매칭");
            cboJong.Items.Add("55" + "." + "HEMS용 취급물질코드매칭");
            cboJong.Items.Add("83" + "." + "HEMS용 소견코드매칭");
            cboJong.Items.Add("85" + "." + "HEMS용 질병분류코드매칭");
            cboJong.SelectedIndex = 0;

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
                fn_Screen_Clear();
                cboJong.Focus();
            }
            else if (sender == btnView)
            {
                int nRead = 0;
                string strJong = "";
                string strName = "";

                strJong = VB.Left(cboJong.Text, 2);
                strName = txtName.Text;

                pnlSearch.Enabled = false;
                SS1.Enabled = true;

                List<HIC_CODE> list = hicCodeService.GetItembyGubunName(strJong, strName);

                nRead = list.Count;
                SS1.ActiveSheet.RowCount = nRead + 700;
                for (int i = 0; i < nRead; i++)
                {
                    SS1.ActiveSheet.Cells[i, 0].Text = "";
                    SS1.ActiveSheet.Cells[i, 1].Text = list[i].CODE;
                    SS1.ActiveSheet.Cells[i, 2].Text = list[i].NAME;
                    SS1.ActiveSheet.Cells[i, 3].Text = list[i].SORT.To<string>();
                    SS1.ActiveSheet.Cells[i, 4].Text = list[i].GCODE;
                    SS1.ActiveSheet.Cells[i, 5].Text = list[i].GCODE1;
                    SS1.ActiveSheet.Cells[i, 6].Text = list[i].GCODE2;
                    SS1.ActiveSheet.Cells[i, 7].Text = list[i].GBDEL;
                    SS1.ActiveSheet.Cells[i, 8].Text = list[i].ROWID;
                }

                btnOK.Enabled = true;
                btnCancel.Enabled = true;
                btnPrint.Enabled = true;
            }
            else if (sender == btnOK)
            {
                string strDel = "";
                string strCode = "";
                string strName = "";
                string strSort = "";
                string strGCode = "";
                string strGCode1 = "";
                string strGCode2 = "";
                
                string strGbDel = "";
                string strROWID = "";
                string strChange = "";

                int result = 0;

                //자료에 오류가 있는지 Check
                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strDel = SS1.ActiveSheet.Cells[i, 0].Text;

                    strCode = SS1.ActiveSheet.Cells[i, 1].Text;
                    strName = SS1.ActiveSheet.Cells[i, 2].Text;
                    strSort = SS1.ActiveSheet.Cells[i, 3].Text;
                    strGCode = SS1.ActiveSheet.Cells[i, 4].Text;
                    strGCode1 = SS1.ActiveSheet.Cells[i, 5].Text;
                    strGCode2 = SS1.ActiveSheet.Cells[i, 6].Text;

                    strGbDel = SS1.ActiveSheet.Cells[i, 7].Text;

                    if (strCode.IsNullOrEmpty())
                    {
                        MessageBox.Show("코드가 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (strName.IsNullOrEmpty())
                    {
                        MessageBox.Show("코드명칭이 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (strGbDel != "Y" && strGbDel != "")
                    {
                        MessageBox.Show("삭제여부 오류(삭제여부 Y=삭제 공란=사용중)", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    switch (VB.Left(cboJong.Text, 2))
                    {
                        case "18":  //사업장기호
                            if (strCode.Length > 8)
                            {
                                MessageBox.Show(i + " 번줄 사업장기호가 8자 이상입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            break;
                        case "21":  //사업장기호
                            if (strCode.Length != 4)
                            {
                                MessageBox.Show(i + " 번줄 건강보험 지사코드는 4자리가 아님", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            break;
                        case "22":  //사업장기호
                            if (strCode.Length != 3)
                            {
                                MessageBox.Show(i + " 번줄 통계분류코드가 3자리가 아님", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            break;
                        default:
                            break;
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
                    strCode = SS1.ActiveSheet.Cells[i, 1].Text;
                    strName = SS1.ActiveSheet.Cells[i, 2].Text;
                    strSort = SS1.ActiveSheet.Cells[i, 3].Text;
                    strGCode = SS1.ActiveSheet.Cells[i, 4].Text;

                    strGCode1 = SS1.ActiveSheet.Cells[i, 5].Text;
                    strGCode2 = SS1.ActiveSheet.Cells[i, 6].Text;

                    strGbDel = SS1.ActiveSheet.Cells[i, 7].Text;
                    strROWID = SS1.ActiveSheet.Cells[i, 8].Text;
                    strChange = SS1.ActiveSheet.Cells[i, 9].Text;

                    HIC_CODE item = new HIC_CODE();

                    item.GUBUN = VB.Left(cboJong.Text, 2);
                    item.CODE = strCode;
                    item.NAME = strName;
                    item.SORT = strSort.To<decimal>();
                    item.GCODE = strGCode;
                    item.GCODE1 = strGCode1;
                    item.GCODE2 = strGCode2;
                    item.GBDEL = strGbDel;
                    item.ROWID = strROWID;

                    if (strROWID.IsNullOrEmpty())
                    {
                        if (strDel != "True")
                        {
                            result = hicCodeService.Insert(item.GUBUN, item.CODE, item.NAME, item.GBDEL, item.GCODE);
                        }
                        else
                        {
                            if (strDel == "True")
                            {
                                result = hicCodeService.Delete(item.ROWID);
                            }
                            else if (strChange == "Y")
                            {
                                result = hicCodeService.Update(item.NAME, item.GCODE, item.ROWID);
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

                strTitle = cboJong.Text + " 코드집";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("출력일시: " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + "  PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);
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
