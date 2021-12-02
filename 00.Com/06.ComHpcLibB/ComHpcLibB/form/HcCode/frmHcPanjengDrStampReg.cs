using ComBase;
using ComBase.Controls;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcPanjengDrStampReg.cs
/// Description     : 판정의사 도장 등록
/// Author          : 이상훈
/// Create Date     : 2020-06-19
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm판정의사도장등록.frm(Frm판정의사도장등록)" />

namespace ComHpcLibB
{
    public partial class frmHcPanjengDrStampReg : Form
    {
        ComHpcLibBService comHpcLibBService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        ComFunc cf = new ComFunc();
        clsHcFunc hf = new clsHcFunc();
        clsHcBill chb = new clsHcBill();
        clsHcType ht = new clsHcType();

        string FstrSabun;
        string FstrName;
        string FstrFilePath;
        long FnLicense;

        public frmHcPanjengDrStampReg()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            comHpcLibBService = new ComHpcLibBService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnFile.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnDelete.Click += new EventHandler(eBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.chkOut.Click += new EventHandler(eChkClick);
            //this.txtLtdCode.Click += new EventHandler(eTxtClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            fn_Display_All_Doctor();

            picStamp.Visible = false;
            btnSave.Enabled = false;
            lblDoct.Text = "";
            FstrSabun = "";
            FstrName = "";
            FnLicense = 0;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnFile)
            {
                OpenFileDialog file = new OpenFileDialog();

                file.Filter = "BMP파일 (*.bmp)|*.bmp|GIF파일(*.gif)|*.gif|Jepg파일(*.jepg)|*.Jepg|Jpg파일(*.jpg)|*.Jpg";

                if (file.ShowDialog() == DialogResult.OK)
                {
                    FstrFilePath = file.FileName.Trim();
                    txtFileName.Text = FstrFilePath;
                    picStamp.Load(FstrFilePath);
                    picStamp.SizeMode = PictureBoxSizeMode.StretchImage;
                    picStamp.Refresh();
                    btnSave.Enabled = true;
                    Application.DoEvents();
                }
                //else if (file.ShowDialog() == DialogResult.Cancel)
                //{
                //    FstrFilePath = "";
                //    picStamp = null;
                //    picStamp.Refresh();
                //    btnSave.Enabled = false;
                //    Application.DoEvents();
                //    return;
                //}
            }
            else if (sender == btnSave)
            {
                string strMsg = "";

                int result = 0;

                if (FstrName.IsNullOrEmpty())
                {
                    return;
                }

                strMsg = "(" + FstrName.Trim() + ") 과장님 도장을 저장 하시겠습니까?";
                if (MessageBox.Show(strMsg, "저장", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                List<COMHPC> list = comHpcLibBService.GetItemHicDojangbySabun(FstrSabun);

                if (list.Count == 0)
                {
                    clsDB.setBeginTran(clsDB.DbCon);

                    result = comHpcLibBService.InsertHicDojang(FstrSabun, FnLicense);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("도장 정보 저장시 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                }

                //도장 이미지를 저장함

                FileStream fs = new FileStream(txtFileName.Text, FileMode.Open, FileAccess.Read);                
                byte[] bImage = new byte[fs.Length];
                fs.Read(bImage, 0, (int)fs.Length);
                fs.Close();

                result = comHpcLibBService.UpdateHicDojangImage(FstrSabun, bImage);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("도장 이미지 저장시 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                fn_Display_All_Doctor();

                MessageBox.Show("저장 완료!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (sender == btnDelete)
            {
                string strMsg = "";
                int result = 0;

                strMsg = FstrSabun + "(" + FstrName + "과장님) 도장을 삭제 하시겠습니까?";
                if (MessageBox.Show(strMsg, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                clsDB.setBeginTran(clsDB.DbCon);

                result = comHpcLibBService.DeleteDojangbySabun(FstrSabun);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("삭제오류", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                clsDB.setCommitTran(clsDB.DbCon);

                picStamp.Visible = false;

                fn_Display_All_Doctor();
            }
        }

        void fn_Display_All_Doctor()
        {
            int nRead = 0;

            List<COMHPC> list = comHpcLibBService.GetSabunNameHicDojangbyLicense();

            nRead = list.Count;
            SS1.ActiveSheet.RowCount = nRead;
            for (int i = 0; i < nRead; i++)
            {
                SS1.ActiveSheet.Cells[i, 0].Text = list[i].SABUN;
                SS1.ActiveSheet.Cells[i, 1].Text = hb.READ_HIC_InsaName(list[i].SABUN);
                SS1.ActiveSheet.Cells[i, 2].Text = "";
                SS1.ActiveSheet.Cells[i, 3].Text = "";
                if (list[i].ROWID.Trim() != "")
                {
                    SS1.ActiveSheet.Cells[i, 3].Text = "▦";
                }
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                string strFileName = "";
                string strChk = "";
                string strAudioFile = "";
                int nRead = 0;

                lblDoct.Text = "";

                FstrSabun = SS1.ActiveSheet.Cells[e.Row, 0].Text;
                FstrName = SS1.ActiveSheet.Cells[e.Row, 1].Text;
                if (!SS1.ActiveSheet.Cells[e.Row, 2].Text.IsNullOrEmpty())
                {
                    FnLicense = long.Parse(SS1.ActiveSheet.Cells[e.Row, 2].Text);
                }
                else
                {
                    FnLicense = 0;
                }
                strChk = SS1.ActiveSheet.Cells[e.Row, 3].Text;

                lblDoct.Text = "사번:" + FstrSabun + " 의사명:" + FstrName;
                FstrFilePath = "";
                picStamp.Visible = false;
                btnSave.Enabled = false;

                if (strChk == "▦")
                {
                    strFileName = @"C:\etc.jpg";

                    COMHPC list2 = comHpcLibBService.GetImagebySabun(FstrSabun);

                    if (!list2.IsNullOrEmpty())
                    {
                        byte[] bImage = null;
                        bImage = (byte[])list2.IMAGE;
                        if (bImage != null)
                        {
                            picStamp.Image = new Bitmap(new MemoryStream(bImage));
                        }

                        picStamp.Visible = true;
                        btnSave.Enabled = true;
                    }
                }
            }
        }

        void eChkClick(object sender, EventArgs e)
        {
            if (sender == chkOut)
            {
                if (chkOut.Checked == true)
                {
                    fn_Display_All_Doctor();
                }
            }
        }
    }
}
