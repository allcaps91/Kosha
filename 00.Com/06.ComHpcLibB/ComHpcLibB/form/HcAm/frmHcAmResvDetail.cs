using ComBase;
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
using ComBase.Controls;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcAmResvDetail.cs
/// Description     : 상세 내역 리스트
/// Author          : 이상훈
/// Create Date     : 2020-01-01
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmResvDetail.frm(HcAm04)" />

namespace ComHpcLibB
{
    public partial class frmHcAmResvDetail : Form
    {
        HicCancerResv2Service hicCancerResv2Service = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        public frmHcAmResvDetail()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            hicCancerResv2Service = new HicCancerResv2Service();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.rdoJong1.Click += new EventHandler(eRdoClick);
            this.rdoJong2.Click += new EventHandler(eRdoClick);
            this.rdoJong3.Click += new EventHandler(eRdoClick);
            this.rdoJong4.Click += new EventHandler(eRdoClick);
            this.rdoJong5.Click += new EventHandler(eRdoClick);
            this.rdoJong6.Click += new EventHandler(eRdoClick);
            this.rdoJong7.Click += new EventHandler(eRdoClick);
            this.rdoJong8.Click += new EventHandler(eRdoClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            txtSName.Text = "";
            fn_ComMon_Set(cboYYMM, 12);
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
                int nREAD = 0;
                string strFDate = "";
                string strTDate = "";               
                string strTemp = "";
                string strTemp1 = "";
                string StrJumin = "";
                int nRow = 0;
                int[] nCNT = new int[7];
                string strJong = "";
                string strName = "";

                if (rdoJong1.Checked == true)
                {
                    strJong = "1";
                }
                else if (rdoJong2.Checked == true)
                {
                    strJong = "2";
                }
                else if (rdoJong3.Checked == true)
                {
                    strJong = "3";
                }
                else if (rdoJong4.Checked == true)
                {
                    strJong = "4";
                }
                else if (rdoJong5.Checked == true)
                {
                    strJong = "5";
                }
                else if (rdoJong6.Checked == true)
                {
                    strJong = "6";
                }
                else if (rdoJong7.Checked == true)
                {
                    strJong = "7";
                }

                strName = txtSName.Text.Trim();

                sp.Spread_All_Clear(SS1);
                for (int i = 0; i < 6; i++)
                {
                    nCNT[i] = 0;
                }

                strFDate = VB.Left(cboYYMM.Text, 4) + "-" + VB.Mid(cboYYMM.Text, 7, 2);

                strFDate += "-01 00:00";
                strTDate = cf.READ_LASTDAY(clsDB.DbCon, VB.Left(strFDate, 10)) + " 23:59";

                List<HIC_CANCER_RESV2> list = hicCancerResv2Service.GetItembyRTime(strFDate, strTDate, strJong, strName);

                nREAD = list.Count;
                SS1.ActiveSheet.RowCount = nREAD;
                progressBar1.Maximum = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    StrJumin = clsAES.DeAES(list[i].JUMIN2);

                    SS1.ActiveSheet.Cells[i, 0].Text = VB.Left(list[i].RTIME.ToString(), 10);
                    strTemp1 = SS1.ActiveSheet.Cells[i, 0].Text;
                    SS1.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                    SS1.ActiveSheet.Cells[i, 2].Text = VB.Left(StrJumin, 6) + "-" + VB.Mid(StrJumin, 7, 1) + "******";
                    SS1.ActiveSheet.Cells[i, 3].Text = list[i].TEL;
                    SS1.ActiveSheet.Cells[i, 4].Text = list[i].HPHONE;
                    SS1.ActiveSheet.Cells[i, 5].Text = list[i].GBUGI;
                    if (list[i].GBUGI == "Y")
                    {
                        nCNT[0] += 1;
                        SS1.ActiveSheet.Cells[i, 5].Text = "◎";
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[i, 5].Text = "";
                    }

                    if (list[i].GBGFS == "Y")
                    {
                        nCNT[1] += 1;
                        SS1.ActiveSheet.Cells[i, 6].Text = "◎";
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[i, 6].Text = "";
                    }

                    if (list[i].GBMAMMO == "Y")
                    {
                        nCNT[2] += 1;
                        SS1.ActiveSheet.Cells[i, 7].Text = "◎";
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[i, 7].Text = "";
                    }

                    if (list[i].GBRECUTM == "Y")
                    {
                        nCNT[3] += 1;
                        SS1.ActiveSheet.Cells[i, 8].Text = "◎";
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[i, 8].Text = "";
                    }

                    if (list[i].GBSONO == "Y")
                    {
                        nCNT[4] += 1;
                        SS1.ActiveSheet.Cells[i, 9].Text = "◎";
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[i, 9].Text = "";
                    }

                    if (list[i].GBWOMB == "Y")
                    {
                        nCNT[5] += 1;
                        SS1.ActiveSheet.Cells[i, 10].Text = "◎";
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[i, 10].Text = "";
                    }
                    if (list[i].GBCT == "Y")
                    {
                        nCNT[6] += 1;
                        SS1.ActiveSheet.Cells[i, 11].Text = "◎";
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[i, 11].Text = "";
                    }

                    SS1.ActiveSheet.Cells[i, 12].Text = list[i].REMARK;
                    progressBar1.Value = i + 1;
                }
                SS1.ActiveSheet.RowCount = nREAD + 1;
                SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, 0].Text = "합계";
                SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, 5].Text = nCNT[0].ToString();
                SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, 6].Text = nCNT[1].ToString();
                SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, 7].Text = nCNT[2].ToString();
                SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, 8].Text = nCNT[3].ToString();
                SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, 9].Text = nCNT[4].ToString();
                SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, 10].Text = nCNT[5].ToString();
                SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, 11].Text = nCNT[6].ToString();
                txtSName.Text = "";

                ////RowHeight 자동 조정
                //for (int i = 0; i < SS1.ActiveSheet.NonEmptyRowCount; i++)
                //{
                //    if (!SS1.ActiveSheet.Cells[i, 12].Text.IsNullOrEmpty())
                //    {
                //        Size size = SS1.ActiveSheet.GetPreferredCellSize(i, 12);
                //        SS1.ActiveSheet.Rows[i].Height = size.Height;
                //    }
                //    else
                //    {
                //        SS1.ActiveSheet.Rows[i].Height = 20;
                //    }
                //}
            }
        }

        void eRdoClick(object sender, EventArgs e)
        {
            eBtnClick(btnSearch, new EventArgs());
        }

        void fn_ComMon_Set(ComboBox ArgCombo, int ArgMonthCNT)
        {
            long ArgYY = 0;
            long ArgMM = 0;
            long nLocate = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);
            ArgYY = long.Parse(VB.Left(clsPublic.GstrSysDate, 4));
            nLocate = long.Parse(VB.Mid(clsPublic.GstrSysDate, 6, 2)) - 1;
            ArgMM = 1;
            if (ArgMM == 12)
            {
                ArgMM = 1;
            }
            ArgCombo.Items.Clear();
            for (int i = 1; i <= ArgMonthCNT; i++)
            {
                ArgCombo.Items.Add(string.Format("{0:0000}", ArgYY) + "년 " + string.Format("{0:00}", ArgMM) + "월분");
                ArgMM += 1;
                if (ArgMM == 0)
                {
                    ArgMM = 12;
                    ArgYY -= 1;
                }
            }
            ArgCombo.SelectedIndex = (int)nLocate;

        }
    }
}
