using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using System;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHaASA.cs
/// Description     : 신체등급(ASA) : 내시경(진정) 검사전기록지
/// Author          : 이상훈
/// Create Date     : 2019-10-02
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmHeaASA.frm(FrmHeaASA)" />

namespace ComHpcLibB
{
    public partial class frmHaASA : Form
    {
        EndoJupmstService endoJupmstService = null;
        HicConsentService hicConsentService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();

        public delegate void SetGstrValue(string GstrValue);
        public event SetGstrValue rSetGstrValue;

        string FstrPtno;
        string FstrExDate;
        string FstrOLD;
        string FstrROWID;
        long FnDrno;
        string FstrSname;

        public frmHaASA(string strPtNo, string strSname, string strSdate, long nDrNo)
        {
            InitializeComponent();

            FstrPtno = strPtNo;
            FstrSname = strSname;
            FstrExDate = strSdate;
            FnDrno = nDrNo;

            SetEvent();
        }

        void SetEvent()
        {
            endoJupmstService = new EndoJupmstService();
            hicConsentService = new HicConsentService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            lblPano.Text = FstrPtno + " " + FstrSname + " " + FstrExDate;

            ENDO_JUPMST list = endoJupmstService.GetItembyPtNoGbSunap(FstrPtno, FstrExDate);

            FstrOLD = "";
            FstrROWID = "";

            if (list != null)
            {
                FstrOLD = list.ASA.To<string>("").Trim();
                FstrROWID = list.RID;

                if (list.GBSUNAP == "7" && FstrOLD != "")
                {
                    btnSave.Enabled = false;
                    pnlASA.Enabled = false;
                }

                switch (FstrOLD)
                {
                    case "1":
                        rdoASA1.Checked = true;
                        break;
                    case "2":
                        rdoASA2.Checked = true;
                        break;
                    case "3":
                        rdoASA3.Checked = true;
                        break;
                    case "4":
                        rdoASA4.Checked = true;
                        break;
                    case "5":
                        rdoASA5.Checked = true;
                        break;
                    case "6":
                        rdoASA6.Checked = true;
                        break;
                    case "E":
                        rdoASA7.Checked = true;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                btnSave.Enabled = false;
                pnlASA.Enabled = false;
                lblPano.Text = "내시경 자료가 없습니다.";
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            int result = 0;

            if (sender == btnExit)
            {
                rSetGstrValue(null);
                this.Close();
                return;
            }
            else if (sender == btnSave)
            {
                string strASA = "";

                if (rdoASA1.Checked == true)
                {
                    strASA = "1";
                }
                else if (rdoASA2.Checked == true)
                {
                    strASA = "2";
                }
                else if (rdoASA3.Checked == true)
                {
                    strASA = "3";
                }
                else if (rdoASA4.Checked == true)
                {
                    strASA = "4";
                }
                else if (rdoASA5.Checked == true)
                {
                    strASA = "5";
                }
                else if (rdoASA6.Checked == true)
                {
                    strASA = "6";
                }
                else if (rdoASA7.Checked == true)
                {
                    strASA = "E";
                }

                if (strASA != FstrOLD)
                {
                    result = endoJupmstService.UpdateASAbyRowId(strASA, FstrROWID);

                    if (result < 0)
                    {
                        MessageBox.Show("ASA 저장중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    result = hicConsentService.UpdateASAbyPtNoSDate(strASA, FnDrno, FstrPtno, FstrExDate);

                    if (result < 0)
                    {
                        MessageBox.Show("동의서 ASA 항목 저장중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                rSetGstrValue(strASA);

                this.Close();
                return;
            }
        }
    }
}
