using ComBase;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHcDustShootMissing.cs
/// Description     : 분진촬영 누락작업
/// Author          : 이상훈
/// Create Date     : 2019-09-10
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm분진촬영자명단.frm(Frm분진촬영자명단)" />

namespace HC_Main
{
    public partial class frmHcDustShootMissing : Form
    {
        HeaJepsuService heaJepsuService = null;
        HicXrayResultService hicXrayResultService = null;
        ComHpcLibBService comHpcLibBService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();

        long FnWRTNO;

        public frmHcDustShootMissing(long WrtNo)
        {
            InitializeComponent();

            FnWRTNO = WrtNo;

            SetEvent();
        }

        void SetEvent()
        {
            heaJepsuService = new HeaJepsuService();
            hicXrayResultService = new HicXrayResultService();
            comHpcLibBService = new ComHpcLibBService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnOK.Click += new EventHandler(eBtnClick);
            this.txtWrtNo.KeyPress += new KeyPressEventHandler(eKeyPress);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            txtWrtNo.Text = "";
            dtpSDate.Text = clsPublic.GstrSysDate;
            SS1_Sheet1.Columns.Get(4).Visible = true;
            SS1_Sheet1.Columns.Get(5).Visible = true;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnOK)
            {
                string strPtNo = "";
                string strPacsNo = "";
                string strSEX = "";
                string strLtd = "";
                string strName = "";
                long nAge = 0;
                string strGubun = "";

                if (MessageBox.Show("해당 수검자를 분진촬영으로 변환하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                int result = heaJepsuService.UpdateGbDust("Y", FnWRTNO);

                if (result < 0)
                {
                    MessageBox.Show("DUST오더로 변경시 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                strName = SS1.ActiveSheet.Cells[0, 1].Text.Trim();
                if (SS1.ActiveSheet.Cells[0, 1].Text.Trim() != "")
                {
                    nAge = long.Parse(SS1.ActiveSheet.Cells[0, 1].Text.Trim());
                }
                else
                {
                    nAge = 0;
                }
                strLtd = SS1.ActiveSheet.Cells[0, 1].Text.Trim();
                strPtNo = SS1.ActiveSheet.Cells[0, 1].Text.Trim();
                strSEX = SS1.ActiveSheet.Cells[0, 1].Text.Trim();

                if (rdoGubun1.Checked == true)
                {
                    strGubun = "1";
                }
                else
                {
                    strGubun = "2";
                }

                if (hicXrayResultService.GetCountbyPtNoPaNo(txtWrtNo.Text, dtpSDate.Text, strGubun) > 0)
                {
                    MessageBox.Show("해당일에 분진촬영 오더 존재함!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                List<COMHPC> list = comHpcLibBService.GetXrayDetailPacsNobySDate(strPtNo, dtpSDate.Text);

                if (list.Count > 1)
                {
                    MessageBox.Show("PACS NO가 2개이상 존재함! 작업불가!", "전산실 연락요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (list.Count == 0)
                {
                    return;                    
                }

                strPacsNo = list[0].PACSNO.Trim();

                HIC_XRAY_RESULT item = new HIC_XRAY_RESULT();

                item.JEPDATE = dtpSDate.Text;
                item.XRAYNO = strPacsNo;
                item.PANO = 0;
                item.SNAME = strName;
                item.SEX = strSEX;
                item.AGE = nAge;
                item.GJJONG = "83";
                item.GBCHUL = "N";
                item.LTDCODE = long.Parse(strLtd);
                item.XCODE = "A142";
                item.GBREAD = "2";
                item.GBSTS = "0";
                item.GBORDER_SEND = "Y";
                item.GBPACS = "Y";
                item.GBCONV = "Y";
                item.PTNO = strPtNo;
                item.ENTSABUN = long.Parse(clsType.User.IdNumber);

                int result1 = hicXrayResultService.Insert(item);

                if (result1 < 0)
                {
                    MessageBox.Show("건진 방사선 전송시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("작업 완료 !!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (sender == btnSearch)
            {
                fn_Screen_Display();
            }
        }

        void fn_Screen_Display()
        {
            string strGubun = "";

            if (rdoGubun1.Checked == true)
            {
                strGubun = "1";
            }
            else
            {
                strGubun = "2";
            }

            HEA_JEPSU list = heaJepsuService.GetItembyWrtNoPaNoPtNo(FnWRTNO, dtpSDate.Text, strGubun);

            if (list == null)
            {
                MessageBox.Show("Data가 존재하지 안습니다!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                SS1.ActiveSheet.Cells[0, 1].Text = list.SNAME.Trim();
                if (list.GBDUST.Trim() == "Y")
                {
                    SS1.ActiveSheet.Cells[0, 3].Text = "Y";
                }
                else
                {
                    SS1.ActiveSheet.Cells[0, 3].Text = "";
                }
                SS1.ActiveSheet.Cells[0, 4].Text = list.AGE.ToString();
                SS1.ActiveSheet.Cells[0, 5].Text = list.LTDCODE.ToString();
                SS1.ActiveSheet.Cells[1, 4].Text = list.PTNO;
                SS1.ActiveSheet.Cells[1, 5].Text = list.SEX;

                HIC_XRAY_RESULT list2 = hicXrayResultService.GetXrayNobyPtNo(list.PTNO, dtpSDate.Text);

                if (list2 == null)
                {
                    SS1.ActiveSheet.Cells[0, 1].Text = "미전송";
                    SS1.ActiveSheet.Cells[0, 3].Text = "없음";
                }
                else
                {
                    SS1.ActiveSheet.Cells[0, 1].Text = "전송";
                    SS1.ActiveSheet.Cells[0, 3].Text = list2.XRAYNO.Trim();
                }
            }
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtWrtNo)
            {
                if (e.KeyChar == 13)
                {
                    SendKeys.Send("{TAB}");
                    eBtnClick(btnSearch, new EventArgs());
                }
            }
        }
    }
}
