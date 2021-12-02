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
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcPersonalHistory.cs
/// Description     : 개인 History 조회
/// Author          : 이상훈
/// Create Date     : 2019-08-29
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "HaMain19.frm(FrmPersonResult)" />

namespace ComHpcLibB
{
    public partial class frmHcPersonalHistory : Form
    {
        long FnWrtNo = 0;

        HicPatientService hicPatientService = null;
        HicJepsuSunapService hicJepsuSunapService = null;
        HicSunapdtlService hicSunapdtlService = null;
        HeaJepsuSunapService heaJepsuSunapService = null;
        HeaSunapdtlService heaSunapdtlService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();

        public frmHcPersonalHistory(long nWrtNo)
        {
            InitializeComponent();
            FnWrtNo = nWrtNo;
            SetEvent();
        }

        void SetEvent()
        {
            hicPatientService = new HicPatientService();
            hicJepsuSunapService = new HicJepsuSunapService();
            hicSunapdtlService = new HicSunapdtlService();
            heaJepsuSunapService = new HeaJepsuSunapService();
            heaSunapdtlService = new HeaSunapdtlService();

            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.SS2.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            fn_Form_Load();

            clsCompuInfo.SetComputerInfo();
        }

        void fn_Form_Load()
        {
            rdoJob1.Checked = true;
            SS2_Sheet1.Rows.Get(-1).Height = 24F;
            SS2_Sheet1.Columns.Get(5).Visible = false;
            btnRef.Visible = false;
            if (clsHcVariable.GstrValue != "")
            {
                txtName.Text = clsHcVariable.GstrValue;
                eBtnClick(btnRef, new EventArgs());
            }

            if (FnWrtNo != 0)   //hcact화면에서 넘어옴
            {
                fn_rdoJob_Click("1");
                txtName.Text = FnWrtNo.To<string>();
                eBtnClick(btnRef, new EventArgs());
            }
        }

        void fn_rdoJob_Click(string strGubun)
        {
            switch (strGubun)
            {
                case "1":
                    if (rdoJob1.Checked == true)
                    {
                        lblSearchTitle.Text = "수진자명";
                        txtName.Text = "";
                    }
                    break;
                case "2":
                    if (rdoJob2.Checked == true)
                    {
                        lblSearchTitle.Text = "주민번호";
                        txtName.Text = "";
                    }
                    break;
                case "3":
                    if (rdoJob3.Checked == true)
                    {
                        lblSearchTitle.Text = "회사명";
                        txtName.Text = "";
                    }
                    break;
                case "4":
                    if (rdoJob4.Checked == true)
                    {
                        lblSearchTitle.Text = "건진번호";
                        txtName.Text = "";
                    }
                    break;
                case "5":
                    if (rdoJob5.Checked == true)
                    {
                        lblSearchTitle.Text = "외래번호";
                        txtName.Text = "";
                    }
                    break;
                default:
                    break;
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSearch)
            {
                fn_Search();
            }
            else if (sender == btnRef)
            {
                fn_Search();
                CellClickEventArgs cellClickEvent = new CellClickEventArgs(new SpreadView(), SSList.ActiveSheet.ActiveRowIndex, SSList.ActiveSheet.ActiveColumnIndex, 0, 0, MouseButtons.Left, false, false);
                eSpreadDClick(SSList, cellClickEvent);
            }
        }

        void fn_Search()
        {
            string sGubun = "";

            sp.Spread_All_Clear(SSList);
            fn_Screen_Clear();

            if (txtName.Text.Trim() == "") return;

            if (rdoJob1.Checked == true)
            {
                sGubun = "1";
            }
            else if (rdoJob2.Checked == true)
            {
                sGubun = "2";
            }
            else if (rdoJob3.Checked == true)
            {
                sGubun = "3";
            }
            else if (rdoJob4.Checked == true)
            {
                sGubun = "4";
            }
            else if (rdoJob5.Checked == true)
            {
                sGubun = "5";
            }

            List<HIC_PATIENT> list = hicPatientService.GetPanobyItem(txtName.Text.Trim(), sGubun);

            SSList.ActiveSheet.RowCount = list.Count;

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    SSList.ActiveSheet.Cells[i, 0].Text = list[i].PANO.To<string>();
                    SSList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                    SSList.ActiveSheet.Cells[i, 2].Text = VB.Left(list[i].JUMIN, 6) + "-" + VB.Right(list[i].JUMIN, 7);
                    SSList.ActiveSheet.Cells[i, 3].Text = hb.READ_Ltd_Name(list[i].LTDCODE.To<string>());
                    SSList.ActiveSheet.Cells[i, 4].Text = list[i].PTNO;
                }
            }
        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            int nRow = 0;
            long nPano = 0;
            string strSname = "";
            string strJumin = "";
            string strAddExam = "";
            int nREAD = 0;
            long nWRTNO = 0;

            FpSpread s = (FpSpread)sender;

            if (sender == this.SSList)
            {
                fn_Screen_Clear();
                nRow = e.Row;
                nPano = long.Parse(SSList.ActiveSheet.Cells[nRow, 0].Text);
                strSname = SSList.ActiveSheet.Cells[nRow, 1].Text.Trim();
                strJumin = SSList.ActiveSheet.Cells[nRow, 2].Text.Trim();

                //종검자/일반건진자 기초자료
                HIC_PATIENT lst = hicPatientService.GetJusobyPano(nPano, "");

                if (lst != null)
                {
                    strJumin = clsAES.DeAES(lst.JUMIN2);
                    ssPatInfo.ActiveSheet.Cells[0, 1].Text = nPano.To<string>();  //종검번호
                    ssPatInfo.ActiveSheet.Cells[0, 3].Text = strSname.Trim();   //수진자명
                    if (VB.Mid(strJumin, 7, 1) == "1" || VB.Mid(strJumin, 7, 1) == "3") //성별
                    {
                        ssPatInfo.ActiveSheet.Cells[1, 1].Text = "남";
                    }
                    else if (VB.Mid(strJumin, 7, 1) == "2" || VB.Mid(strJumin, 7, 1) == "4") //성별
                    {
                        ssPatInfo.ActiveSheet.Cells[1, 1].Text = "여";
                    }
                }

                if (clsHcVariable.GstrHicPart == "1")   //종검
                {
                    //종검실시 자료
                    List<HEA_JEPSU_SUNAP> list = heaJepsuSunapService.GetItembyPaNo(nPano);

                    nREAD = list.Count;
                    SS2.ActiveSheet.RowCount = nREAD;

                    for (int i = 0; i < nREAD; i++)
                    {
                        nWRTNO = list[i].WRTNO;
                        SS2.ActiveSheet.Cells[i, 0].Text = nWRTNO.To<string>();
                        SS2.ActiveSheet.Cells[i, 1].Text = list[i].SDATE;
                        SS2.ActiveSheet.Cells[i, 2].Text = list[i].GJJONG;
                        SS2.ActiveSheet.Cells[i, 3].Text = list[i].TOTAMT.To<string>();

                        //추가검사가 있는지 Check
                        List<HEA_SUNAPDTL> listName = heaSunapdtlService.GetNamebyWrtNo(nWRTNO);

                        if (listName.Count > 0)
                        {
                            strAddExam = "";
                            for (int j = 0; j < listName.Count; j++)
                            {
                                strAddExam += listName[j].YNAME + ",";
                            }
                            strAddExam = VB.Left(strAddExam, strAddExam.Length - 1);
                            SS2.ActiveSheet.Cells[i, 4].Text = string.Format("{0:###,###,##0}", strAddExam);
                        }

                        if (list[i].SANGDAM_ONE == "Y")
                        {
                            SS2.ActiveSheet.Cells[i + 1, 0].BackColor = Color.FromArgb(255, 0, 0);
                        }
                        else
                        {
                            SS2.ActiveSheet.Cells[i + 1, 0].BackColor = Color.FromArgb(255, 255, 255);
                        }
                    }

                    if (e.Column == 0)
                    {
                        clsPublic.GstrRetValue = nPano + "@@";
                    }
                    else
                    {
                        clsPublic.GstrRetValue = "";
                    }
                }
                else if (clsHcVariable.GstrHicPart == "2")  //일검
                {
                    //건진실시 자료
                    List<HIC_JEPSU_SUNAP> list = hicJepsuSunapService.GetItembyPaNo(nPano);

                    nREAD = list.Count;
                    SS2.ActiveSheet.RowCount = nREAD;

                    for (int i = 0; i < nREAD; i++)
                    {
                        nWRTNO = list[i].WRTNO;
                        nWRTNO = long.Parse(SS2.ActiveSheet.Cells[i, 0].Text);
                        SS2.ActiveSheet.Cells[i, 1].Text = list[i].JEPDATE;
                        SS2.ActiveSheet.Cells[i, 2].Text = list[i].GJJONG;
                        SS2.ActiveSheet.Cells[i, 3].Text = list[i].TOTAMT.To<string>();
                        SS2.ActiveSheet.Cells[i, 5].Text = list[i].GJYEAR + "@@" + list[i].GJBANGI + list[i].JEPDATE + "@@" + nPano + "@@" + list[i].GJJONG;

                        //추가검사가 있는지 Check
                        List<HIC_SUNAPDTL> listName = hicSunapdtlService.GetNamebyWrtNo(nWRTNO);

                        if (listName.Count > 0)
                        {
                            strAddExam = "";
                            for (int j = 0; j < listName.Count; j++)
                            {
                                strAddExam += listName[j].NAME + ",";
                            }
                            strAddExam = VB.Left(strAddExam, strAddExam.Length - 1);
                            SS2.ActiveSheet.Cells[i, 4].Text = strAddExam;
                        }
                    }
                }
                nPano = 0;
                strSname = "";
                strAddExam = "";
                nWRTNO = 0;
                strJumin = "";
            }
            else if (sender == this.SS2)
            {
                nRow = 0;
                FnWrtNo = 0;

                if (SS2.ActiveSheet.Cells[e.Row, 0].Text.Trim() != "")
                {
                    FnWrtNo = long.Parse(SS2.ActiveSheet.Cells[e.Row, 0].Text);
                }

                frmHcResultView f = new frmHcResultView("HEA", FnWrtNo);
                f.ShowDialog();
                f.Dispose();
                FnWrtNo = 0;
            }
        }

        void fn_Screen_Clear()
        {
            lblSearchTitle.Text = "수진자명";
            for (int i = 0; i <= 1; i++)
            {
                ssPatInfo.ActiveSheet.Cells[i, 1].Text = "";
                ssPatInfo.ActiveSheet.Cells[i, 3].Text = "";
            }

            sp.Spread_All_Clear(SS2);
        }
    }
}
