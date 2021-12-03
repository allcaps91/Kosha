using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcConsentformView.cs
/// Description     : 건진 동의서 보기
/// Author          : 이상훈
/// Create Date     : 2020-06-30
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm건진동의서보기.frm(Frm건진동의서보기)" />

namespace ComHpcLibB
{
    public partial class frmHcConsentformView : Form
    {


        string fstrSname = "";
        string fstrBdate = "";


        HicConsentService hicConsentService = null;
        HicHeaJepsuConsentService hicHeaJepsuConsentService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();

        public frmHcConsentformView()
        {
            InitializeComponent();
            SetEvent();
        }

        public frmHcConsentformView(string argSname, string argBdate)
        {
            InitializeComponent();
            SetEvent();

            fstrSname = argSname;
            fstrBdate = argBdate;

        }

        void SetEvent()
        {
            hicConsentService = new HicConsentService();
            hicHeaJepsuConsentService = new HicHeaJepsuConsentService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.txtSName.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            string sDirPath = @"C:\Temp";
            string sDirPath1 = @"C:\Temp\Consent";

            this.Location = new System.Drawing.Point(10, 10);
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFrDate.Text = clsPublic.GstrSysDate;
            dtpToDate.Text = clsPublic.GstrSysDate;
            txtSName.Text = "";
            
            DirectoryInfo Dir = new DirectoryInfo(sDirPath);

            if (Dir.Exists == false)
            {
                Dir.Create();
            }

            DirectoryInfo Dir1 = new DirectoryInfo(sDirPath1);

            if (Dir1.Exists == false)
            {
                Dir1.Create();
            }

            SS1.ActiveSheet.ColumnHeader.Cells[0, 0, 0, SS1.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
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
                string strSName = "";

                sp.Spread_All_Clear(SS1);

                if (!fstrSname.IsNullOrEmpty()) { txtSName.Text = fstrSname; }
                if (!fstrBdate.IsNullOrEmpty()) { dtpFrDate.Text = fstrBdate; dtpToDate.Text = fstrBdate; }
                txtSName.Text = txtSName.Text.Trim();
                strSName = txtSName.Text.Trim();
                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;

                //자료를 Select
                List<HIC_HEA_JEPSU_CONSENT> list = hicHeaJepsuConsentService.GetItembyJepDate(strFrDate, strToDate, strSName);

                nREAD = list.Count;
                nRow = 0;
                SS1.ActiveSheet.RowCount = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    SS1.ActiveSheet.Cells[i, 0].Text = list[i].PTNO;
                    SS1.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                    if (list[i].DEPTCODE == "HR")
                    {
                        //SS1.ActiveSheet.Cells[i, 1].BackColor = Color.Lavender; //ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFC0FF"));
                        SS1.ActiveSheet.Cells[i, 1].BackColor = Color.Aquamarine;
                    }
                    SS1.ActiveSheet.Cells[i, 2].Text = list[i].AGE + "/" + list[i].SEX;
                    SS1.ActiveSheet.Cells[i, 3].Text = list[i].SDATE;
                    if (list[i].D10 > 0)
                    {
                        SS1.ActiveSheet.Cells[i, 4].Text = "◎";
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[i, 4].Text = "";
                    }
                    if (list[i].D20 > 0)
                    {
                        SS1.ActiveSheet.Cells[i, 5].Text = "◎";
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[i, 5].Text = "";
                    }
                    if (list[i].D30 > 0)
                    {
                        SS1.ActiveSheet.Cells[i, 6].Text = "◎";
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[i, 6].Text = "";
                    }
                    if (list[i].D40 > 0)
                    {
                        SS1.ActiveSheet.Cells[i, 7].Text = "◎";
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[i, 7].Text = "";
                    }
                    
                    SS1.ActiveSheet.Cells[i, 8].Text = list[i].ASA;
                    SS1.ActiveSheet.Cells[i, 9].Text = list[i].WRTNO.To<string>();
                }
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (sender == txtSName)
                {
                    SendKeys.Send("{TAB}");
                }
            }
        }

         void eSpdDClick(object sender, CellClickEventArgs e)
        {
            int nREAD = 0;
            long nWRTNO = 0;
            string strForm = "";
            string strPtNo = "";
            string strDeptCode = "";
            long nCNT = 0;
            string strPath = "";
            string strFile = "";
            string strLocal = "";
            string strServer = "";
            string strServerPath = "";
            string strHost = "";
            string strFileList = "";
            int nFileCnt = 0;
            int nCol = 0;

            strPath = @"C:\Temp\Consent";

            strPtNo = SS1.ActiveSheet.Cells[e.Row, 0].Text;
            nWRTNO = SS1.ActiveSheet.Cells[e.Row, 9].Text.To<long>();
           
            nCol = e.Column;

            if (e.Column >= 4 && e.Column <= 7)
            {
                if (SS1.ActiveSheet.Cells[e.Row, e.Column].Text != "◎")
                {
                    MessageBox.Show("해당 동의서가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            for (int i = 0; i < 15; i++)
            {
                tabControl1.TabPages[i].Text = "";

                PictureBox pictureBox = (Controls.Find("pictureBox" + (i + 1).ToString(), true)[0] as PictureBox);
                pictureBox.Image = null;
            }

            tabControl1.SelectedIndex = 0;

            hf.Dir_Check_FileDelete("c:\\Temp\\Consent", "*.jpg");

            List<HIC_CONSENT> list = hicConsentService.GetItembyWrtNo(nWRTNO, nCol);

            nREAD = list.Count;
            strFileList = "";
            nFileCnt = 0;

            int nTab = 1;

            for (int i = 0; i < nREAD; i++)
            {
                nCNT = list[i].PAGECNT;
                strForm = list[i].FORMCODE;
                strDeptCode = list[i].DEPTCODE;

                for (int j = 0; j < nCNT; j++)
                {
                    strFile = nWRTNO.To<string>() + "_" + strDeptCode + "_" + VB.Left(strForm, 2) + j + "_1.jpg";
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
                    Thread.Sleep(500);

                    if (hf.Dir_Check_YN(strPath) == true)
                    {
                        tabControl1.TabPages[nTab - 1].Text = nTab + "페이지";

                        FileStream fs = new FileStream(strLocal, FileMode.Open);
                        Bitmap bmp = new Bitmap(fs);
                        fs.Close();

                        PictureBox pictureBox = (Controls.Find("pictureBox" + nTab.ToString(), true)[0] as PictureBox);
                        pictureBox.Image = bmp;
                        pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    }

                    nTab++;
                }
            }
        }
    }
}
