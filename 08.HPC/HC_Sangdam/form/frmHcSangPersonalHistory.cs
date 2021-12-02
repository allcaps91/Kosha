using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Sangdam
/// File Name       : frmHcSangPersonalHistory.cs
/// Description     : 개인별 History조회
/// Author          : 이상훈
/// Create Date     : 2020-01-16
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmPersonResult.frm(FrmHcAct02)" />

namespace HC_Sangdam
{
    public partial class frmHcSangPersonalHistory : Form
    {
        HicPatientService hicPatientService = null;
        HicJepsuSunapService hicJepsuSunapService = null;
        HicSunapdtlGroupcodeService hicSunapdtlGroupcodeService = null;
        HicMemoService hicMemoService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        string FSName = "";
        long FWrtNo = 0;
        string FstrValue = "";

        public frmHcSangPersonalHistory(string strValue)
        {
            InitializeComponent();
            FstrValue = strValue;
            SetEvent();
        }

        void SetEvent()
        {
            hicPatientService = new HicPatientService();
            hicJepsuSunapService = new HicJepsuSunapService();
            hicSunapdtlGroupcodeService = new HicSunapdtlGroupcodeService();
            hicMemoService = new HicMemoService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.rdoJob1.Click += new EventHandler(eRdoClick);
            this.rdoJob2.Click += new EventHandler(eRdoClick);
            this.rdoJob3.Click += new EventHandler(eRdoClick);
            this.rdoJob4.Click += new EventHandler(eRdoClick);
            this.txtSName.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtSName.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtSName.LostFocus += new EventHandler(eTxtLostFocus);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            string strData = "";

            rdoJob4.Checked = true;

            ComFunc.ReadSysDate(clsDB.DbCon);

            rdoJob3.Checked = true;
            SS2_Sheet1.Rows.Get(-1).Height = 24F;
            SS2_Sheet1.Columns.Get(5).Visible = false;
            btnRef.Visible = false;

            if (!FstrValue.IsNullOrEmpty())
            {
                txtSName.Text = FstrValue.Trim();                
            }

            if (clsHcVariable.GnWRTNO != 0)    //hcact화면에서 넘어옴
            {
                rdoJob4.Checked = true;
                eRdoClick(rdoJob4, new EventArgs());
                txtSName.Text = clsHcVariable.GnWRTNO.ToString();
            }
            eBtnClick(btnRef, new EventArgs());
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnRef)
            {
                eBtnClick(btnSearch, new EventArgs());
                eSpdDClick(SSList, new CellClickEventArgs(new SpreadView(), 0, 0, 0, 0, new MouseButtons(), false, false));
            }
            else if (sender == btnSearch)
            {
                string strSName = "";
                string strJob = "";

                sp.Spread_All_Clear(SSList);
                fn_Screen_Clear();

                if (!txtSName.Text.IsNullOrEmpty()) return;

                if (rdoJob4.Checked == true)
                {
                    strJob = "1";
                }
                else if (rdoJob1.Checked == true)
                {
                    strJob = "2";
                }
                else if (rdoJob2.Checked == true)
                {
                    strJob = "3";
                }
                else if (rdoJob3.Checked == true)
                {
                    strJob = "4";
                }

                List<HIC_PATIENT> list = hicPatientService.GetItembyName(strJob, strSName);

                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        SSList.ActiveSheet.Cells[i, 0].Text = list[i].PANO.ToString();
                        SSList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME.Trim();
                        SSList.ActiveSheet.Cells[i, 2].Text = list[i].JUMIN;
                    }
                }
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS2)
            {
                int nRow = 0;
                long nWRTNO = 0;
                string strTemp = "";
                string strJong = "";

                if (!SS2.ActiveSheet.Cells[e.Row, 0].Text.IsNullOrEmpty())
                {
                    nWRTNO = SS2.ActiveSheet.Cells[e.Row, 0].Text.To<long>();
                }

                strTemp = SS2.ActiveSheet.Cells[e.Row, 5].Text.Trim();

                //검진년도,반기,접수일자,검진번호,검진종류
                strJong = VB.Pstr(strTemp, "@@", 5);
                frmHaResultView f = new frmHaResultView(nWRTNO);
                f.ShowDialog();
            }
            else if (sender == SSList)
            {
                long nPano = 0;
                string strSname = "";
                string strJumin = "";
                string strAddExam = "";
                int nRow = 0;
                int nREAD = 0;
                long nWRTNO = 0;

                fn_Screen_Clear();

                nPano = long.Parse(SSList.ActiveSheet.Cells[e.Row, 0].Text);
                strSname = SSList.ActiveSheet.Cells[e.Row, 1].Text;
                strJumin = SSList.ActiveSheet.Cells[e.Row, 2].Text;

                //일반건진자 기초자료
                HIC_PATIENT list = hicPatientService.GetJusobyPano(nPano, "");

                if (!list.IsNullOrEmpty())
                {
                    SS1.ActiveSheet.Cells[0, 1].Text = nPano.ToString();    //종검번호
                    SS1.ActiveSheet.Cells[0, 3].Text = strSname;    //수진자명
                    if (VB.Mid(strJumin, 7, 1) == "1" || VB.Mid(strJumin, 7, 1) == "3") //성별
                    {
                        SS1.ActiveSheet.Cells[1, 1].Text = "남";
                    }
                    else if (VB.Mid(strJumin, 7, 1) == "2" || VB.Mid(strJumin, 7, 1) == "4")
                    {
                        SS1.ActiveSheet.Cells[1, 1].Text = "여";
                    }
                    SS1.ActiveSheet.Cells[1, 3].Text = VB.Left(strJumin, 2) + "년" + VB.Mid(strJumin, 3, 2) + "월" + VB.Mid(strJumin, 5, 2) + "일";
                    SS1.ActiveSheet.Cells[2, 1].Text = VB.Left(strJumin, 6) + "-" + VB.Right(strJumin, 7);
                    SS1.ActiveSheet.Cells[2, 3].Text = list.TEL;
                    SS1.ActiveSheet.Cells[3, 1].Text = list.JUSO1 + " " + list.JUSO2;
                    SS1.ActiveSheet.Cells[4, 1].Text = hb.READ_Ltd_Name(list.LTDCODE.ToString());
                }

                //건진실시 자료
                List<HIC_JEPSU_SUNAP> list2 = hicJepsuSunapService.GetItembyPaNo(nPano);

                nREAD = list2.Count;
                SS2.ActiveSheet.RowCount = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    nWRTNO = list2[i].WRTNO;
                    SS2.ActiveSheet.Cells[i, 0].Text = nWRTNO.ToString();
                    SS2.ActiveSheet.Cells[i, 1].Text = list2[i].JEPDATE;
                    SS2.ActiveSheet.Cells[i, 2].Text = hb.READ_GjJong_Name(list2[i].GJJONG);
                    SS2.ActiveSheet.Cells[i, 3].Text = list2[i].TOTAMT.ToString();
                    SS2.ActiveSheet.Cells[i, 5].Text = list2[i].GJYEAR.Trim() + "@@" + list2[i].GJBANGI.Trim() + "@@" + list2[i].JEPDATE + "@@" + nPano + "@@" + list2[i].GJJONG.Trim();

                    //추가검사가 있는지 Check
                    List<HIC_SUNAPDTL_GROUPCODE> list3 = hicSunapdtlGroupcodeService.GetCodeNamebyWrtNo(nWRTNO);

                    if (list3.Count > 0)
                    {
                        strAddExam = "";
                        for (int j = 0; j < list3.Count; j++)
                        {
                            strAddExam += list3[i].NAME.Trim() + ",";
                        }
                        strAddExam = VB.Left(strAddExam, strAddExam.Length - 1);
                        SS2.ActiveSheet.Cells[i, 4].Text = strAddExam;
                    }
                }

                nPano = 0;
                strSname = "";
                strAddExam = "";
                nWRTNO = 0;
                strJumin = "";
            }
        }

        void eRdoClick(object sender, EventArgs e)
        {
            if (sender == rdoJob1)
            {
                lblSearchTitle.Text = "수진자명";
                txtSName.Text = "";
            }
            else if (sender == rdoJob2)
            {
                lblSearchTitle.Text = "주민번호";
                txtSName.Text = "";
            }
            else if (sender == rdoJob3)
            {
                lblSearchTitle.Text = "회사명";
                txtSName.Text = "";
            }
            else if (sender == rdoJob4)
            {
                lblSearchTitle.Text = "건진번호";
                txtSName.Text = "";
            }
        }

        void eTxtGotFocus(object sender, EventArgs e)
        {
            if (sender == txtSName)
            {
                txtSName.ImeMode = ImeMode.Hangul;
            }
        }
        void eTxtKeyPress(object sender, EventArgs e)
        {
            if (sender == txtSName)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void eTxtLostFocus(object sender, EventArgs e)
        {
            if (sender == txtSName)
            {
                if (txtSName.Text.IsNullOrEmpty())
                {
                    return;
                }
            }
        }

        void fn_Screen_Clear()
        {
            lblSearchTitle.Text = "수진자명";
            for (int i = 0; i <= 2; i++)
            {
                SS1.ActiveSheet.Cells[i, 1].Text = "";
                SS1.ActiveSheet.Cells[i, 3].Text = "";
            }

            for (int i = 3; i < 4; i++)
            {
                for (int j = 1; j < 4; j++)
                {
                    SS1.ActiveSheet.Cells[i, j].Text = "";
                }
            }
            sp.Spread_All_Clear(SS2);
        }
    }
}
