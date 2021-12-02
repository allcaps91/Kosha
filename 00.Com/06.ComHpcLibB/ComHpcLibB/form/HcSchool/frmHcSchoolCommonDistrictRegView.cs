using ComBase;
using ComLibB;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using ComBase.Controls;
using System.IO;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcSchoolCommonDistrictRegView.cs
/// Description     : 상용구 등록 및 조회
/// Author          : 이상훈
/// Create Date     : 2020-02-03
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmUseWardSet1.frm(HcSchool50)" />

namespace ComHpcLibB
{
    public partial class frmHcSchoolCommonDistrictRegView : Form
    {
        HicResultwardService hicResultwardService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        string FstrGbn;

        public frmHcSchoolCommonDistrictRegView()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            hicResultwardService = new HicResultwardService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSeq.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnSearch1.Click += new EventHandler(eBtnClick);
            this.btnSearch2.Click += new EventHandler(eBtnClick);
            this.btnSearch3.Click += new EventHandler(eBtnClick);
            this.btnSearch4.Click += new EventHandler(eBtnClick);
            this.btnSearch5.Click += new EventHandler(eBtnClick);
            this.btnSearch6.Click += new EventHandler(eBtnClick);
            this.btnSearch7.Click += new EventHandler(eBtnClick);
            this.btnMenuSave.Click += new EventHandler(eBtnClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            btnSeq.Enabled = false;
            btnMenuSave.Enabled = false;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
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

                strTitle = "종검판정 코드";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("맑은 고딕", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("인쇄일자 : " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime, new Font("맑은 고딕", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
            else if (sender == btnSeq)
            {
                string strCode = "";
                string strSName = "";
                long nMaxNo = 0;

                //최대 번호를 찾음
                nMaxNo = 0;
                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    if (SS1.ActiveSheet.Cells[i, 0].Text != "True")
                    {
                        strCode = SS1.ActiveSheet.Cells[i, 1].Text.Trim();
                        strSName = SS1.ActiveSheet.Cells[i, 2].Text.Trim();
                        if (strCode != "")
                        {
                            if (long.Parse(strCode) > nMaxNo)
                            {
                                nMaxNo = long.Parse(strCode);
                            }
                        }
                    }
                }

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    if (SS1.ActiveSheet.Cells[i, 0].Text != "True")
                    {
                        strCode = SS1.ActiveSheet.Cells[i, 1].Text.Trim();
                        strSName = SS1.ActiveSheet.Cells[i, 2].Text.Trim();
                        if (strCode == "" && strSName != "")
                        {
                            nMaxNo += 1;
                            SS1.ActiveSheet.Cells[i, 1].Text = string.Format("{0:00000}", nMaxNo);
                        }
                    }
                }
            }
            else if (sender == btnSearch1)
            {
                FstrGbn = "01";

                sp.Spread_All_Clear(SS1);
                fn_Screen_Display();

                btnSeq.Enabled = true;
                btnMenuSave.Enabled = true;
                this.Text = "상용구 등록 및 조회(학생판정)";
            }
            else if (sender == btnSearch2)
            {
                FstrGbn = "02";

                sp.Spread_All_Clear(SS1);
                fn_Screen_Display();

                btnSeq.Enabled = true;
                btnMenuSave.Enabled = true;
                this.Text = "상용구 등록 및 조회(학생가정조치)";
            }
            else if (sender == btnSearch3)
            {
                FstrGbn = "03";

                sp.Spread_All_Clear(SS1);
                fn_Screen_Display();

                btnSeq.Enabled = true;
                btnMenuSave.Enabled = true;
                this.Text = "상용구 등록 및 조회(학생구강판정)";
            }
            else if (sender == btnSearch4)
            {
                FstrGbn = "04";

                sp.Spread_All_Clear(SS1);
                fn_Screen_Display();

                btnSeq.Enabled = true;
                btnMenuSave.Enabled = true;
                this.Text = "상용구 등록 및 조회(일반상담문구(통합))";
            }
            else if (sender == btnSearch5)
            {
                FstrGbn = "05";

                sp.Spread_All_Clear(SS1);
                fn_Screen_Display();

                btnSeq.Enabled = true;
                btnMenuSave.Enabled = true;
                this.Text = "상용구 등록 및 조회(구강상담문구)";
            }
            else if (sender == btnSearch6)
            {
                FstrGbn = "06";

                sp.Spread_All_Clear(SS1);
                fn_Screen_Display();

                btnSeq.Enabled = true;
                btnMenuSave.Enabled = true;
                this.Text = "상용구 등록 및 조회(암상담문구)";
            }
            else if (sender == btnSearch7)
            {
                FstrGbn = "07";

                sp.Spread_All_Clear(SS1);
                fn_Screen_Display();

                btnSeq.Enabled = true;
                btnMenuSave.Enabled = true;
                this.Text = "상용구 등록 및 조회(학생상담문구)";
            }
            else if (sender == btnMenuSave)
            {
                int nNo1 = 0;
                int nNo2 = 0;
                string strCODE = "";
                string strName = "";
                string strROWID = "";
                string strChk = "";
                int result = 0;

                clsDB.setBeginTran(clsDB.DbCon);

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strChk = "";
                    strCODE = "";
                    strName = "";
                    strROWID = "";

                    strChk = SS1.ActiveSheet.Cells[i, 0].Text;
                    strCODE = SS1.ActiveSheet.Cells[i, 1].Text;
                    strName = SS1.ActiveSheet.Cells[i, 2].Text;
                    strROWID = SS1.ActiveSheet.Cells[i, 3].Text;
                    strName = strName.Trim().Replace("'", "`");

                    if (strChk == "True")
                    {
                        if (strROWID != "")
                        {
                            result = hicResultwardService.DeletebyRowId(strROWID);                                     
                        }
                    }
                    else
                    {
                        if (strROWID == "" && (strCODE != "" || strName != ""))
                        {
                            result = hicResultwardService.Insert(clsType.User.IdNumber, strCODE, strName, FstrGbn);
                        }
                        else if (strROWID != "")
                        {
                            result = hicResultwardService.Update(strROWID, strCODE, strName);
                        }
                    }

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("저장 오류", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);

                sp.Spread_All_Clear(SS1);
                fn_Screen_Display();
            }
        }

        void fn_Screen_Display()
        {
            int nRow = 0;
            int nRead = 0;

            SS1_Sheet1.Columns.Get(3).Visible = false;

            List<HIC_RESULTWARD> list = hicResultwardService.GetItembyGubun(FstrGbn);

            nRead = list.Count;
            SS1.ActiveSheet.RowCount = nRead + 30;

            for (int i = 0; i < nRead; i++)
            {
                SS1.ActiveSheet.Cells[i, 1].Text = list[i].CODE.To<int>().To<string>();
                SS1.ActiveSheet.Cells[i, 2].Text = list[i].WARDNAME;
                SS1.ActiveSheet.Cells[i, 3].Text = list[i].ROWID;
            }

            for (int i = 0; i < nRead; i++)
            {
                Size size = SS1.ActiveSheet.GetPreferredCellSize(i, 2);
                SS1.ActiveSheet.Rows[i].Height = size.Height;
            }
        }
    }
}
