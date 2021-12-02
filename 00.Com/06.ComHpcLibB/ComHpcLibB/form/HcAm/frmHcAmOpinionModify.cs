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
/// File Name       : frmHcAmOpinionModify.cs
/// Description     : 암판정 소견문 일괄 수정작업
/// Author          : 이상훈
/// Create Date     : 2019-12-31
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm소견문수정.frm(Frm소견문수정)" />

namespace ComHpcLibB
{
    public partial class frmHcAmOpinionModify : Form
    {
        HicCancerNewService hicCancerNewService = null;
        HicJepsuCancerNewService hicJepsuCancerNewService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();

        public frmHcAmOpinionModify()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            hicCancerNewService = new HicCancerNewService();
            hicJepsuCancerNewService = new HicJepsuCancerNewService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFrDate.Text = clsPublic.GstrSysDate;
            dtpToDate.Text = clsPublic.GstrSysDate;

            sp.Spread_All_Clear(SSList);
            SSList.ActiveSheet.RowCount = 50;

            //작업구분
            cboJob.Items.Clear();
            cboJob.Items.Add("1.위장조영촬영");         //txtS_Sogen  
            cboJob.Items.Add("2.위내시경검사");         //txtS_Sogen2
            cboJob.Items.Add("3.분변잠혈반응검사");     //txtC_Sogen
            cboJob.Items.Add("4.대장이중조영검사");     //txtC_Sogen2
            cboJob.Items.Add("5.대장내시경검사");       //txtC_Sogen3
            cboJob.Items.Add("6.간암검사");             //txtL_Sogen
            cboJob.Items.Add("7.유방암검사");           //txtB_Sogen
            cboJob.Items.Add("8.자궁경부암검사");       //txtW_Sogen
            cboJob.SelectedIndex = 0;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSave)
            {
                long nWrtNo = 0;
                string strTemp = "";
                int result = 0;
                string strJob = "";

                if (SSList.ActiveSheet.RowCount == 0) return;

                strJob = VB.Left(cboJob.Text, 1);

                clsDB.setBeginTran(clsDB.DbCon);

                for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
                {
                    nWrtNo = long.Parse(SSList.ActiveSheet.Cells[i, 0].Text);
                    strTemp = SSList.ActiveSheet.Cells[i, 0].Text.Trim();

                    strTemp = strTemp.Replace("\r\n", "");
                    strTemp = strTemp.Replace("\r", "");
                    strTemp = strTemp.Replace("\n", "");
                    
                    result = hicCancerNewService.Update(strTemp, nWrtNo, strJob);
                    
                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("자료를 등록중 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);

                eBtnClick(btnSearch, new EventArgs());
            }
            else if (sender == btnSearch)
            {
                int j = 0;
                string strTemp = "";
                string strJob = "";
                string strFrDate = "";
                string strToDate = "";

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;
                strJob = VB.Left(cboJob.Text, 1);

                sp.Spread_All_Clear(SSList);

                List<HIC_JEPSU_CANCER_NEW> list = hicJepsuCancerNewService.GetItembyJepDate(strFrDate, strToDate, strJob);

                for (int i = 0; i < list.Count; i++)
                {
                    strTemp = "";
                    switch (VB.Left(cboJob.Text, 1))
                    {
                        case "1":
                            strTemp = list[i].S_SOGEN;
                            break;
                        case "2":
                            strTemp = list[i].S_SOGEN2;
                            break;
                        case "3":
                            strTemp = list[i].C_SOGEN;
                            break;
                        case "4":
                            strTemp = list[i].C_SOGEN2;
                            break;
                        case "5":
                            strTemp = list[i].C_SOGEN3;
                            break;
                        case "6":
                            strTemp = list[i].L_SOGEN;
                            break;
                        case "7":
                            strTemp = list[i].B_SOGEN;
                            break;
                        case "8":
                            strTemp = list[i].W_SOGEN;
                            break;
                        default:
                            break;
                    }

                    if (VB.L(strTemp, "\r\n") > 1 || VB.L(strTemp, "\r") > 1 || VB.L(strTemp, "\n") > 1)
                    {
                        SSList.ActiveSheet.RowCount += 1;
                        j += 1;
                        SSList.ActiveSheet.Cells[j - 1, 0].Text = list[i].WRTNO.ToString();
                        SSList.ActiveSheet.Cells[j - 1, 1].Text = list[i].SNAME;
                        SSList.ActiveSheet.Cells[j - 1, 2].Text = VB.Left(cboJob.Text, 1);
                        switch (VB.Left(cboJob.Text, 1))
                        {
                            case "1":
                                SSList.ActiveSheet.Cells[j - 1, 3].Text = list[i].S_SOGEN;
                                break;
                            case "2":
                                SSList.ActiveSheet.Cells[j - 1, 3].Text = list[i].S_SOGEN2;
                                break;
                            case "3":
                                SSList.ActiveSheet.Cells[j - 1, 3].Text = list[i].C_SOGEN;
                                break;
                            case "4":
                                SSList.ActiveSheet.Cells[j - 1, 3].Text = list[i].C_SOGEN2;
                                break;
                            case "5":
                                SSList.ActiveSheet.Cells[j - 1, 3].Text = list[i].C_SOGEN3;
                                break;
                            case "6":
                                SSList.ActiveSheet.Cells[j - 1, 3].Text = list[i].L_SOGEN;
                                break;
                            case "7":
                                SSList.ActiveSheet.Cells[j - 1, 3].Text = list[i].B_SOGEN;
                                break;
                            case "8":
                                SSList.ActiveSheet.Cells[j - 1, 3].Text = list[i].W_SOGEN;
                                break;
                            default:
                                break;
                        }
                        strTemp = "";
                    }
                }

                //RowHeight 자동 조정
                for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
                {
                    if (!SSList.ActiveSheet.Cells[i, 3].Text.IsNullOrEmpty())
                    {
                        Size size = SSList.ActiveSheet.GetPreferredCellSize(i, 3);
                        SSList.ActiveSheet.Rows[i].Height = size.Height;
                    }
                    else
                    {
                        SSList.ActiveSheet.Rows[i].Height = 20;
                    }
                }
            }
        }
    }
}
