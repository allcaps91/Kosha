using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHaConsentApproval.cs
/// Description     : 동의서 의사 승인
/// Author          : 이상훈
/// Create Date     : 2019-10-21
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm동의서승인.frm(Frm동의서승인)" />

namespace ComHpcLibB
{
    public partial class frmHaConsentApproval : Form
    {
        HicConsentService hicConsentService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();

        string FstrDate;

        public frmHaConsentApproval()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            hicConsentService = new HicConsentService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.btnAllSelect.Click += new EventHandler(eBtnClick);
            this.btnCancleSelect.Click += new EventHandler(eBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.rdoJob1.Click += new EventHandler(eRdoClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);
            sp.Spread_All_Clear(SS1);
            hf.Dir_Check_FileDelete("c:\\Temp", "*.jpg");
            hf.Dir_Check_FileDelete("c:\\Temp\\Consent", "*.jpg");

            dtpDate.Text = clsPublic.GstrSysDate;
            FstrDate = "";

            //임시설정(버튼Visible-----------------------
            //btnAllSelect.Visible = false;
            //btnCancleSelect.Visible = false;
            //btnOK.Visible = false;
            //btnCancel.Visible = false;
            //label1.Visible = false;
            //label2.Visible = false;
            //----------------------
            for (int i = 0; i < 15; i++)
            {
                tabControl1.TabPages[i].Text = "";
            }

            Application.DoEvents();

            eBtnClick(btnSearch, new EventArgs());
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
                int nRow = 0;
                string strFormCode = "";
                string strOK = "";

                string strFrDate = "";
                string strToDate = "";
                string strJob = "";
                string strDrSabun = "";

                strFrDate = dtpDate.Text;
                strToDate = DateTime.Parse(dtpDate.Text).AddDays(1).ToShortDateString();

                //strDrSabun = clsType.User.IdNumber;
                strDrSabun = "";

                if (rdoJob1.Checked == true)
                {
                    strJob = "1";
                }
                else if (rdoJob2.Checked == true)
                {
                    strJob = "2";
                }

                List<HIC_CONSENT> list = hicConsentService.GetItembySabun(strDrSabun, strFrDate, strToDate, strJob);

                nREAD = list.Count;
                SS1.ActiveSheet.RowCount = nREAD;
                nRow = 0;
                for (int i = 0; i < nREAD; i++)
                {
                    strFormCode = list[i].FORMCODE;
                    SS1.ActiveSheet.Cells[i, 0].Text = "";
                    if (rdoJob1.Checked == true)
                    {
                        SS1.ActiveSheet.Cells[i, 0].Text = "True";  //미승인
                    }
                    SS1.ActiveSheet.Cells[i, 1].Text = VB.Left(list[i].ENTDATE.To<string>(), 10);
                    SS1.ActiveSheet.Cells[i, 2].Text = list[i].WRTNO.To<string>();
                    SS1.ActiveSheet.Cells[i, 3].Text = list[i].SNAME;
                    SS1.ActiveSheet.Cells[i, 4].Text = " " + strFormCode;
                    SS1.ActiveSheet.Cells[i, 5].Text = list[i].PAGECNT.To<string>();
                    SS1.ActiveSheet.Cells[i, 7].Text = list[i].DRNAME;
                    SS1.ActiveSheet.Cells[i, 8].Text = list[i].DOCTSIGN.To<string>();
                    SS1.ActiveSheet.Cells[i, 9].Text = list[i].RID;
                    SS1.ActiveSheet.Cells[i, 10].Text = list[i].PTNO;
                    SS1.ActiveSheet.Cells[i, 11].Text = list[i].DEPTCODE;

                    //서식명
                    SS1.ActiveSheet.Cells[i, 6].Text = list[i].FORMNAME;

                    if (rdoJob1.Checked == true)
                    {
                        btnOK.Enabled = true;
                        btnCancel.Enabled = false;
                    }
                    else
                    {
                        btnOK.Enabled = true;
                        btnCancel.Enabled = true;
                    }
                }
            }
            else if (sender == btnOK)
            {
                int nCNT = 0;
                string strDate = "";
                string strChk = "";
                string strSuCode = "";
                string strApproveTime = "";
                string strROWID = "";
                string strList = "";
                string strBDate = "";
                string strGbSite = "";
                string strMsg = "";
                long nPano = 0;

                //처방전 발행일에 의사가 휴일인지 점검
                strBDate = dtpDate.Text;
                if (hb.Check_Sabun_Huil(clsType.User.IdNumber, strBDate) == true)
                {
                    strMsg = strBDate + "일은 근무일이 아닙니다.";
                    if (MessageBox.Show(strMsg, "오류", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }

                ComFunc.ReadSysDate(clsDB.DbCon);

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strChk = SS1.ActiveSheet.Cells[i, 0].Text.Trim();
                    strApproveTime = SS1.ActiveSheet.Cells[i, 8].Text.Trim();
                    strROWID = SS1.ActiveSheet.Cells[i, 9].Text.Trim();
                    if (strChk == "Trie" && strApproveTime.IsNullOrEmpty())
                    {
                        //EMR에 동의서를 전송함
                        if (fn_Emr_Send(strROWID) == false)
                        {
                            MessageBox.Show("동의서를 EMR에 전송 시 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                eBtnClick(btnSearch, new EventArgs());
            }
            else if (sender == btnCancel)
            {
                string strChk = "";
                string strROWID = "";

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strChk = SS1.ActiveSheet.Cells[i, 0].Text;
                    strROWID = SS1.ActiveSheet.Cells[i, 0].Text;
                    if (strChk == "True")
                    {
                        //EMR에 동의서를 전송함
                        if (fn_Emr_Send_Cancel(strROWID) == false)
                        {
                            MessageBox.Show("동의서를 EMR에 전송취소 시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                eBtnClick(btnSearch, new EventArgs());
            }
            else if (sender == btnAllSelect)
            {
                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    SS1.ActiveSheet.Cells[i, 0].Text = "True";
                }
            }
            else if (sender == btnCancleSelect)
            {
                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    SS1.ActiveSheet.Cells[i, 0].Text = "";
                }
            }
        }

        /// <summary>
        /// EMR에 동의서를 전송
        /// </summary>
        /// <param name="strRowId"></param>
        /// <returns></returns>
        bool fn_Emr_Send(string strRowId)
        {
            //---( EMR에 전송 루틴 추가 )-------
            bool rtnVal = false;
            string strDocSign = "";
            strDocSign = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;

            int result = hicConsentService.UpdateDocSignbyRowId(strDocSign, strRowId);

            if (result < 0)
            {
                MessageBox.Show("EMR 전송시 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                rtnVal = false;
            }
            else
            {
                rtnVal = true;
            }

            return rtnVal;
        }

        bool fn_Emr_Send_Cancel(string strRowId)
        {
            //---( EMR에 전송취소 루틴 추가 )-------
            bool rtnVal = false;
            string strDocSign = "";

            strDocSign = "";

            int result = hicConsentService.UpdateDocSignbyRowId(strDocSign, strRowId);

            if (result < 0)
            {
                MessageBox.Show("EMR 전송 취소시 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                rtnVal = false;
            }
            else
            {
                rtnVal = true;
            }

            return rtnVal;
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                long nWRTNO = 0;
                string strForm = "";
                string strPtNo = "";
                string strDeptCode = "";
                int nCNT = 0;
                string strPath = "";
                string strFile = "";
                string strLocal = "";
                string strServer = "";
                string strHost = "";

                strPath = "c:\\Temp\\Consent";
                //strPath = @"c:\Temp\Consent\";

                nWRTNO = SS1.ActiveSheet.Cells[e.Row, 2].Text.To<long>();
                strForm = SS1.ActiveSheet.Cells[e.Row, 4].Text.Trim();
                nCNT = SS1.ActiveSheet.Cells[e.Row, 5].Text.To<int>();
                strPtNo = SS1.ActiveSheet.Cells[e.Row, 10].Text.Trim();
                strDeptCode = SS1.ActiveSheet.Cells[e.Row, 11].Text.Trim();

                for (int i = 0; i < 15; i++)
                {
                    tabControl1.TabPages[i].Text = "";

                    PictureBox pictureBox = (Controls.Find("pictureBox" + (i + 1).ToString(), true)[0] as PictureBox);
                    pictureBox.Image = null;
                }

                tabControl1.SelectedIndex = 0;

                hf.Dir_Check_FileDelete("c:\\Temp\\Consent", "*.jpg");

                for (int i = 0; i < nCNT; i++)
                {
                    strFile = nWRTNO.To<string>() + "_" + strDeptCode + "_" + VB.Left(strForm, 2) + i + "_1.jpg";
                    strLocal = strPath + "\\" + strFile;
                    strServer = "/data/hic_result/consent_temp/" + strFile;
                    strHost = "/data/hic_result/consent_temp/";
                    //hf.Dir_Check(strLocal);

                    Ftpedt FtpedtX = new Ftpedt();

                    if (FtpedtX.FtpDownload("192.168.100.31", "oracle", FtpedtX.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle"), strLocal, strServer, strHost) == false)
                    {
                        MessageBox.Show(strFile + " 다운로드 실패", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        FtpedtX = null;
                        return;
                    }

                    FtpedtX = null;
                    Thread.Sleep(1000);

                    if (hf.Dir_Check_YN(strPath) == true)
                    {
                        tabControl1.TabPages[i].Text = (i + 1) + "페이지";

                        FileStream fs = new FileStream(strLocal, FileMode.Open);
                        Bitmap bmp = new Bitmap(fs);
                        fs.Close();

                        PictureBox pictureBox = (Controls.Find("pictureBox" + (i + 1).ToString(), true)[0] as PictureBox);
                        pictureBox.Image = bmp;
                        pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
            }
        }

        void eRdoClick(object sender, EventArgs e)
        {
            eBtnClick(btnSearch, new EventArgs());
        }
    }
}
