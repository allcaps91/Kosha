using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComHpcLibB.Dto;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcReading.cs
/// Description     : 사업장 공문등록
/// Author          : 김민철
/// Create Date     : 2019-09-18
/// Update History  : 
/// <seealso cref="\Hea\HaMain\Frm공문등록.frm.frm(Frm공문등록)"/>
/// 
namespace ComHpcLibB
{
    public partial class frmHcReading : BaseForm
    {
        clsHcFunc         cHF               = null;
        HIC_LTD           LtdHelpItem       = null;
        HicReadingService hicReadingService = null;
        HicLtdService     hicLtdService     = null;

        string FstrRowid = string.Empty;

        public frmHcReading()
        {
            InitializeComponent();
            SetControl();
            SetEvents();
        }

        private void SetEvents()
        {
            this.Load                   += new EventHandler(eFormLoad);
            this.btnSearch.Click        += new EventHandler(eBtnClick);
            this.btnLtdHelp.Click       += new EventHandler(eBtnClick);
            this.btnExit.Click          += new EventHandler(eBtnClick);
            this.btnFile.Click          += new EventHandler(eBtnClick);
            this.btnSave.Click          += new EventHandler(eBtnClick);
            this.btnDelete.Click        += new EventHandler(eBtnClick);
            this.btnCancel.Click        += new EventHandler(eBtnClick);
            this.txtLtdName.KeyPress    += new KeyPressEventHandler(eKeyPress);
            this.SS1.CellDoubleClick    += new CellClickEventHandler(eSpdDblClick);
        }

        private void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtLtdName && e.KeyChar == (char)13)
            {
                if (!txtLtdName.Text.Trim().IsNullOrEmpty()) { Ltd_Code_Help(txtLtdName); }
            }
        }

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            string sDirPath = "C:\\HICEXCEL";
            string strFileName = string.Empty;
            string strServerName = string.Empty;

            DirectoryInfo Dir = new DirectoryInfo(sDirPath);

            if (Dir.Exists == false)
            {
                Dir.Create();
            }

            HIC_READING item = SS1.GetRowData(e.Row) as HIC_READING;

            txtLtdName.Text = item.CODE + "." + item.NAME.Trim();
            txtPcDocuFile1.Text = item.DATA1;
            cboYEAR.Text = item.YEAR;
            FstrRowid = item.RID;

            if (txtPcDocuFile1.Text.IsNullOrEmpty())
            {
                MessageBox.Show("첨부파일이 없습니다.", "확인");
                return;
            }

            if (VB.Right(txtPcDocuFile1.Text, 4) == "xlsx")
            {
                strFileName = "c:\\HICEXCEL\\AAA.xlsx";
            }
            else
            {
                strFileName = "c:\\HICEXCEL\\AAA" + VB.Right(txtPcDocuFile1.Text, 4);
            }

            string strHost = "/data/DOCU_READING/";
            strServerName = "/data/DOCU_READING/" + txtPcDocuFile1.Text;

            Ftpedt FtpedtX = new Ftpedt();

            if (FtpedtX.FtpDownload("192.168.100.31", "oracle", FtpedtX.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle"), strFileName, strServerName, strHost) == true)
            {
                Process.Start(strFileName);
            }
            else
            {
                MessageBox.Show("파일이 존재하지 않습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            FtpedtX = null;
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                Search_Data();
            }
            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnLtdHelp)
            {
                Ltd_Code_Help(txtLtdName);
            }
            else if (sender == btnFile)
            {
                using (OpenFileDialog OpenFile = new OpenFileDialog())
                {
                    OpenFile.Title = "열기";
                    OpenFile.Filter = "Excel Files|*.xls;*.xlsx;";
                    OpenFile.InitialDirectory = "C:\\HICEXCEL";
                    OpenFile.ShowDialog();

                    //txtPcDocuFile1.Text = OpenFile.FileName.Split('\\')[OpenFile.FileName.Split('\\').Length - 1];
                    txtPcDocuFile1.Text = OpenFile.FileName;
                }
            }
            else if (sender == btnDelete)
            {
                Delete_Data();
            }
            else if (sender == btnSave)
            {
                Save_Data();
            }
            else if (sender == btnCancel)
            {
                Screen_Clear();
            }
        }

        private void Save_Data()
        {
            string strSeq           = "";
            string strCODE          = "";
            string strName          = "";
            string strReg_Date      = "";
            string strDATA1         = "";  //첨부파일1
            string strDATA1_1       = "";  //저장된 첨부파일명1
            string strAddContent1   = "";  //저장할 파일명1
            string strROWID         = "";
            string strYEAR          = "";
            int result = 0;

            strReg_Date = DateTime.Now.ToShortDateString();
            strDATA1 = txtPcDocuFile1.Text.Trim();
            strROWID = FstrRowid;
            strCODE = VB.Pstr(txtLtdName.Text, ".", 1).Trim();
            strName = VB.Pstr(txtLtdName.Text, ".", 2).Trim();
            strYEAR = cboYEAR.Text.Trim();

            if (strDATA1.IsNullOrEmpty()) { MessageBox.Show("파일을 선택해주세요.", "확인"); return; }
            if (strCODE.IsNullOrEmpty() || strName.IsNullOrEmpty()) { MessageBox.Show("계약처 다시 선택해주세요.", "확인"); return; }

            if (!strROWID.IsNullOrEmpty())
            {
                strDATA1_1 = hicReadingService.GetItemByRowid(strROWID);
            }

            //===========================================================================================
            //첨부파일 추가하기

            strAddContent1 = Add_File_Filtering(strDATA1);

            if (txtPcDocuFile1.Text.Trim() != "" && strDATA1_1 != txtPcDocuFile1.Text.Trim())
            {
                strDATA1 = strSeq + "" + strAddContent1;
            }
            else
            {
                strDATA1 = txtPcDocuFile1.Text.Trim();
            }

            //===========================================================================================

            HIC_READING item = new HIC_READING
            {
                CODE = strCODE,
                NAME = strName,
                REG_DATE = Convert.ToDateTime(strReg_Date),
                REG_SABUN = clsType.User.IdNumber,
                DATA1 = strDATA1,
                YEAR = strYEAR,
                RID = FstrRowid
            };

            if (strROWID.IsNullOrEmpty())
            {
                result = hicReadingService.Insert(item);

                if (result <= 0)
                {
                    MessageBox.Show("저장실패!", "오류");
                    return;
                }
            }
            else
            {
                result = hicReadingService.UpDate(item);

                if (result <= 0)
                {
                    MessageBox.Show("저장실패!", "오류");
                    return;
                }
            }

            Attachment_FTP_Send_1(strDATA1);    //첨부파일1 서버로 전송

            Search_Data();
        }

        private void Attachment_FTP_Send_1(string strDATA1)
        {
            string strHost = "/data/DOCU_READING/";

            Ftpedt FtpedtX = new Ftpedt();

            if (FtpedtX.FtpUpload("192.168.100.31", "oracle", FtpedtX.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle"), strDATA1, txtPcDocuFile1.Text, strHost) == false)
            {
                FtpedtX = null;
                ComFunc.MsgBox("자료 등록 중 오류 발생");                
                return;
            }

            FtpedtX = null;
        }

        private string Add_File_Filtering(string ArgAddFile)
        {
            //첨부파일 추가하기
            string rtnVal = string.Empty;
            string strAdd = string.Empty;
            int nAddCnt = 0;


            rtnVal = "";
            nAddCnt = 0;
            strAdd = ArgAddFile;

            for (int i = 1; i <= strAdd.Length; i++)
            {
                if (VB.Mid(strAdd, i, 1) == "\\")
                {
                    nAddCnt = nAddCnt + 1;
                }
            }

            string[] strTemp = ArgAddFile.Split('\\');

            for (int K = 0; K <= nAddCnt; K++)
            {
                if (strAdd != "")
                {
                    rtnVal = strTemp[K].ToString();
                }
            }

            return rtnVal;
        }

        private void Delete_Data()
        {
            if (FstrRowid == "")
            {
                MessageBox.Show("삭제 항목을 선택해 주세요.", "확인");
                return;
            }

            //이상규, 이정희 만 작업가능
            if (clsType.User.IdNumber == "18551" || clsType.User.IdNumber == "1859")
            {
                int result = hicReadingService.Delete(FstrRowid);

                if (result <= 0)
                {
                    MessageBox.Show("작업실패!");
                }
            }
            else
            {
                MessageBox.Show("삭제권한이 없습니다.");
            }

            Search_Data();
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            string sDirPath = "C:\\HICEXCEL";

            DirectoryInfo Dir = new DirectoryInfo(sDirPath);

            if (Dir.Exists == false)
            {
                Dir.Create();
            }

            Screen_Clear();
            Search_Data();
        }

        private void Screen_Clear()
        {
            panSub01.Initialize();
            txtLtdName.Text = "";
        }

        /// <summary>
        /// 사업장 코드 검색창 연동
        /// </summary>
        private void Ltd_Code_Help(TextBox tx)
        {
            string strFind = "";

            if (tx.Text.Contains("."))
            {
                strFind = VB.Pstr(tx.Text, ".", 2).Trim();
            }
            else
            {
                strFind = tx.Text.Trim();
            }

            frmHcLtdHelp frm = new frmHcLtdHelp(strFind);
            frm.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(ePost_value_LTD);
            frm.ShowDialog();

            if (LtdHelpItem.CODE > 0 && !LtdHelpItem.IsNullOrEmpty())
            {
                tx.Text = LtdHelpItem.CODE.To<string>();
                tx.Text += "." + LtdHelpItem.SANGHO;
            }
            else
            {
                tx.Text = "";
            }
        }

        private void ePost_value_LTD(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        private void Search_Data()
        {
            SS1.DataSource = hicReadingService.GetListByYear(cboVYear.Text, "");
        }

        private void SetControl()
        {
            cHF                 = new clsHcFunc();
            LtdHelpItem         = new HIC_LTD();
            hicReadingService   = new HicReadingService();
            hicLtdService       = new HicLtdService();

            string strYEAR = VB.Left(DateTime.Now.ToShortDateString(), 4) + VB.Mid(DateTime.Now.ToShortDateString(), 6 , 2); ;

            strYEAR = cHF.DATE_YYMM_ADD(strYEAR, 2);

            cboYEAR.Items.Clear();
            cboVYear.Items.Clear();
            
            for (int i = 1; i <= 3; i++)
            {
                cboYEAR.Items.Add(VB.Left(strYEAR, 4));
                cboVYear.Items.Add(VB.Left(strYEAR, 4));
                strYEAR = cHF.DATE_YYMM_ADD(strYEAR, -12);
            }

            cboYEAR.SelectedIndex = 0;
            cboVYear.SelectedIndex = 0;

            SS1.Initialize();
            SS1.AddColumn("코드",       nameof(HIC_READING.CODE),           48, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true });
            SS1.AddColumn("계약처명",   nameof(HIC_READING.NAME),          180, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, IsSort = true, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("파일명",     nameof(HIC_READING.DATA1),         240, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, IsSort = true, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("작성자",     nameof(HIC_READING.REG_SABUN_NM),   64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, IsSort = true, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("작성일자",   nameof(HIC_READING.REG_DATE),       78, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("RID",        nameof(HIC_READING.RID),            94, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, IsVisivle = false });
        }
    }
}
