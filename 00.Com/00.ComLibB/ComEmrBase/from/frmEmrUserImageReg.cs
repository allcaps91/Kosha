using ComBase;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmEmrUserImageReg : Form
    {

        int nImage;
        int nSelectedImage;
        int nImageSaved;
        int nSelectedImageSaved;
        //int mPage = 0;

        private string mstrGRPGB = "";
        private string mstrUSEGB = "";
        //private string mstrSubSql = "";
        //private string mstrSubSql1 = "";
        //private string mstrDeptCd = "";
        
        private string mstrFoldBase = @"\EmrImageTmp\RegImageBase\";
        private string mstrFoldUser = @"\EmrImageTmp\RegImageUser\";

        public frmEmrUserImageReg()
        {
            InitializeComponent();
        }

        public frmEmrUserImageReg(string strGRPGB, string strUSEGB)
        {
            InitializeComponent();
            mstrGRPGB = strGRPGB;
            mstrUSEGB = strUSEGB;
        }

        private void frmEmrUserImageReg_Load(object sender, EventArgs e)
        {
            nImage = 0;
            nSelectedImage = 1;

            nImageSaved = 2;
            nSelectedImageSaved = 3;

            trvEmrGroup.ImageList = this.ImageList1;

            panAll.Dock = DockStyle.Fill;
            panDept.Dock = DockStyle.Fill;

            CheckFold();

            GetDataGrpFormList();
            SetDeptCd();
            optAll.Checked = true;

            optRegUse.Checked = true;

            //if (clsCommon.gstrEXENAME == "MHEMRMNGDG.EXE")
            //{
            //    optDept.Visible = false;
            //    cboDept.Visible = false;
            //    cboDeptReg.Visible = true;

            //    this.Text = "과별 이미지기록지 등록";
            //    lblTitle.Text = "과별 이미지기록지 등록";
            //    lblGRPGB.Text = "과별 이미지기록지";

            //    optRegUse.Visible = false;
            //    optRegDept.Checked = true;
            //}
            //else
            //{
            //    
            //}
        }

        private void SetDeptCd()
        {
            int i = 0;
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            cboDept.Items.Clear();
            cboDept.Items.Add("");
            cboDeptReg.Items.Clear();
            cboDeptReg.Items.Add("");

            string mstrSubSql = "";
            mstrSubSql = "";
            mstrSubSql = mstrSubSql + "WHERE SUB_SPECIAL =  '0' ";
            mstrSubSql = mstrSubSql + ComNum.VBLF + "ORDER BY RANK ";

            dt = clsEmrQuery.GetMedDeptInfo(clsDB.DbCon, "");

            if (dt == null)
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                cboDept.Items.Add(dt.Rows[i]["MEDDEPTCD"].ToString().Trim());
                cboDeptReg.Items.Add(dt.Rows[i]["MEDDEPTCD"].ToString().Trim());
            }
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        private void GetDataGrpFormList()
        {
            TreeNode oNodex = new TreeNode();

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            trvEmrGroup.Nodes.Clear();

            mtsImageList1.Clear();
            mtsImgMain1.ClearImage();

            mtsImageList1.tbSize = 100;

            //mPage = 0;
            ssImage_Sheet1.RowCount = 0;
            txtPage.Text = "";
            txtPageAll.Text = "";

            Cursor.Current = Cursors.WaitCursor;

            SQL = " SELECT BASCD AS BGB, BASNAME  ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD ";
            SQL = SQL + ComNum.VBLF + "WHERE BSNSCLS = '기본이미지' ";
            SQL = SQL + ComNum.VBLF + "    AND UNITCLS = '대분류' ";
            SQL = SQL + ComNum.VBLF + "ORDER BY BASCD";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                oNodex = trvEmrGroup.Nodes.Add(dt.Rows[i]["BGB"].ToString().Trim(), dt.Rows[i]["BASNAME"].ToString().Trim(), nImage, nSelectedImage);
            }
            dt.Dispose();
            dt = null;

            SQL = " SELECT BASEXNAME AS BGB, BASCD AS MGB, BASNAME  ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD ";
            SQL = SQL + ComNum.VBLF + "WHERE BSNSCLS = '기본이미지' ";
            SQL = SQL + ComNum.VBLF + "    AND UNITCLS = '중분류' ";
            SQL = SQL + ComNum.VBLF + "ORDER BY BASCD";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                oNodex = trvEmrGroup.Nodes.Find(dt.Rows[i]["BGB"].ToString().Trim(), true)[0].Nodes.Add(dt.Rows[i]["MGB"].ToString().Trim(), dt.Rows[i]["BASNAME"].ToString().Trim(), nImage, nSelectedImage);
            }
            dt.Dispose();
            dt = null;

            SQL = " SELECT BASEXNAME AS MGB, BASCD AS SGB, BASNAME  ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD ";
            SQL = SQL + ComNum.VBLF + "WHERE BSNSCLS = '기본이미지' ";
            SQL = SQL + ComNum.VBLF + "    AND UNITCLS = '소분류' ";
            SQL = SQL + ComNum.VBLF + "ORDER BY BASCD";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                oNodex = trvEmrGroup.Nodes.Find(dt.Rows[i]["MGB"].ToString().Trim(), true)[0].Nodes.Add(dt.Rows[i]["SGB"].ToString().Trim(), dt.Rows[i]["BASNAME"].ToString().Trim(), nImageSaved, nSelectedImageSaved);
            }
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

        }

        private void CheckFold()
        {
            mstrFoldBase = clsEmrType.EmrSvrInfo.EmrClient + mstrFoldBase;

            if (Directory.Exists(mstrFoldBase) == false)
            {
                Directory.CreateDirectory(mstrFoldBase);
            }
            mstrFoldUser = clsEmrType.EmrSvrInfo.EmrClient + mstrFoldUser;

            if (Directory.Exists(mstrFoldUser) == false)
            {
                Directory.CreateDirectory(mstrFoldUser);
            }
        }

        private void DeleteFold()
        {
            try
            {
                if (Directory.Exists(mstrFoldBase) == true)
                {
                    DirectoryInfo diSource1 = new DirectoryInfo(mstrFoldBase);
                    if (diSource1.GetFiles().Length > 0)
                    {
                        clsScan.DeleteFoldAll(mstrFoldBase);
                    }

                    diSource1 = null;
                    Directory.Delete(mstrFoldBase);
                }
                if (Directory.Exists(mstrFoldUser) == true)
                {
                    DirectoryInfo diSource1 = new DirectoryInfo(mstrFoldUser);
                    if (diSource1.GetFiles().Length > 0)
                    {
                        clsScan.DeleteFoldAll(mstrFoldUser);
                    }

                    diSource1 = null;
                    Directory.Delete(mstrFoldUser);
                }
            }
            catch
            {
            }
        }

        private void frmEmrUserImageReg_FormClosed(object sender, FormClosedEventArgs e)
        {
            DeleteFold();
        }

        private void mbtnExit_Click(object sender, EventArgs e)
        {
            //clsWinScp.DisConWinScp();
            this.Close();
        }

        private void optAll_CheckedChanged(object sender, EventArgs e)
        {
            if (optAll.Checked == true)
            {
                panAll.Visible = true;
                panDept.Visible = false;
                mtsImageListAll.Clear();
                mtsImageList1.Clear();
                ssImageAll_Sheet1.RowCount = 0;
                ssImage_Sheet1.RowCount = 0;

                mtsImgMain1.ClearImage();
            }
        }

        private void DispBaseImage(string strDept)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            mtsImageList1.Clear();
            mtsImgMain1.ClearImage();
            mtsImageList1.tbSize = 100;

            ssImage_Sheet1.RowCount = 0;

            if (strDept.Trim() == "") return;

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL = " SELECT A.IMAGENO, A.IMAGENAME, A.IMAGEEXT,  A.IMAGETHUMB";
            SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRBASIMAGE A";
            SQL = SQL + ComNum.VBLF + "    LEFT OUTER JOIN " + ComNum.DB_EMR + "AEMRIMAGEGUBUN B";
            SQL = SQL + ComNum.VBLF + "        ON A.IMAGECODE = B.IMAGECODE";
            SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRUSERIMAGE C                 ";
            SQL = SQL + ComNum.VBLF + "        ON A.IMAGENO = C.IMAGENO                     ";
            SQL = SQL + ComNum.VBLF + "    WHERE C.GRPGB = 'D'                             ";
            SQL = SQL + ComNum.VBLF + "      AND C.USEGB = '" + strDept.Trim() + "'                            ";//진료과 선택
            SQL = SQL + ComNum.VBLF + "    ORDER BY A.IMAGENAME, A.IMAGENO";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            ssImage_Sheet1.RowCount = dt.Rows.Count;
            ssImage_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssImage_Sheet1.Cells[i, 0].Text = dt.Rows[i]["IMAGENO"].ToString().Trim();
                ssImage_Sheet1.Cells[i, 1].Text = dt.Rows[i]["IMAGENAME"].ToString().Trim();
                ssImage_Sheet1.Cells[i, 2].Text = dt.Rows[i]["IMAGEEXT"].ToString().Trim();

                byte[] b = Convert.FromBase64String(dt.Rows[i]["IMAGETHUMB"].ToString());
                MemoryStream stream = new MemoryStream(b);
                Bitmap image1 = new Bitmap(stream);

                int intWidth = 70;
                int intHeight = 80;

                Bitmap newImage;
                if (image1.Width > image1.Height)
                {
                    intWidth = 80;
                    intHeight = 70;
                }

                newImage = new Bitmap(intWidth, intHeight, PixelFormat.Format24bppRgb);
                Graphics graphics_1 = Graphics.FromImage(newImage);
                graphics_1.CompositingQuality = CompositingQuality.HighQuality;
                graphics_1.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics_1.SmoothingMode = SmoothingMode.HighQuality;
                graphics_1.DrawImage(image1, 0, 0, intWidth, intHeight);

                newImage.Save(mstrFoldBase + "\\thumb_" + dt.Rows[i]["IMAGEEXT"].ToString().Trim());

                graphics_1.Dispose();
                graphics_1 = null;

                image1.Dispose();
                image1 = null;

                newImage.Dispose();
                newImage = null;

                mtsImageList1.AddThumbnail(mstrFoldBase + "\\thumb_" + dt.Rows[i]["IMAGEEXT"].ToString().Trim(), dt.Rows[i]["IMAGENAME"].ToString().Trim());
                Application.DoEvents();
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        private void DispBaseImageCheck()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            mtsImageList2.Clear();
            mtsImgMain2.ClearImage();
            mtsImageList2.tbSize = 100;

            ssImage2_Sheet1.RowCount = 0;
            if (mstrGRPGB == "D")
            {
                if (cboDeptReg.Text.Trim() == "") return;
            }

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL = " SELECT A.IMAGENO, A.IMAGENAME, A.IMAGEEXT,  TO_CHAR(A.IMAGETHUMB) AS IMAGETHUMB";
            SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRBASIMAGE A";
            SQL = SQL + ComNum.VBLF + "    LEFT OUTER JOIN " + ComNum.DB_EMR + "AEMRIMAGEGUBUN B";
            SQL = SQL + ComNum.VBLF + "        ON A.IMAGECODE = B.IMAGECODE";
            SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRUSERIMAGE C                 ";
            SQL = SQL + ComNum.VBLF + "        ON A.IMAGENO = C.IMAGENO                     ";
            SQL = SQL + ComNum.VBLF + "    WHERE C.GRPGB = '" + mstrGRPGB + "'                             ";
            if (mstrGRPGB == "D")
            {
                SQL = SQL + ComNum.VBLF + "      AND C.USEGB = '" + cboDeptReg.Text.Trim() + "'                            ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "      AND C.USEGB = '" + clsType.User.IdNumber + "'                            ";
            }
            SQL = SQL + ComNum.VBLF + "    ORDER BY A.IMAGENAME, A.IMAGENO";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            ssImage2_Sheet1.RowCount = dt.Rows.Count;
            ssImage2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssImage2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["IMAGENO"].ToString().Trim();
                ssImage2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["IMAGENAME"].ToString().Trim();
                ssImage2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["IMAGEEXT"].ToString().Trim();

                byte[] b = Convert.FromBase64String(dt.Rows[i]["IMAGETHUMB"].ToString());
                MemoryStream stream = new MemoryStream(b);
                Bitmap image1 = new Bitmap(stream);

                int intWidth = 70;
                int intHeight = 80;

                Bitmap newImage;
                if (image1.Width > image1.Height)
                {
                    intWidth = 80;
                    intHeight = 70;
                }

                newImage = new Bitmap(intWidth, intHeight, PixelFormat.Format24bppRgb);
                Graphics graphics_1 = Graphics.FromImage(newImage);
                graphics_1.CompositingQuality = CompositingQuality.HighQuality;
                graphics_1.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics_1.SmoothingMode = SmoothingMode.HighQuality;
                graphics_1.DrawImage(image1, 0, 0, intWidth, intHeight);

                newImage.Save(mstrFoldBase + "\\thumb_" + dt.Rows[i]["IMAGEEXT"].ToString().Trim());

                graphics_1.Dispose();
                graphics_1 = null;

                image1.Dispose();
                image1 = null;

                newImage.Dispose();
                newImage = null;

                mtsImageList2.AddThumbnail(mstrFoldBase + "\\thumb_" + dt.Rows[i]["IMAGEEXT"].ToString().Trim(), dt.Rows[i]["IMAGENAME"].ToString().Trim());
                Application.DoEvents();
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        private void mbtnSave_Click(object sender, EventArgs e)
        {
            
            if (mstrGRPGB == "D")
            {
                if (cboDeptReg.Text.Trim() == "") return;
            }
            else
            {
                if (clsType.User.IdNumber == "") return;
            }

            if (SaveData() == true)
            {
                DispBaseImageCheck();
            }
        }

        private bool SaveData()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
                //int intImageCount = 0;

                if (optAll.Checked == true)
                {
                    for (i = 1; i <= mtsImageListAll.Controls.Count; i++)
                    {
                        if (mtsImageListAll.ItemX(i).Selected == true)
                        {
                            SQL = "";
                            SQL = " SELECT GRPGB, USEGB, IMAGENO, USEID, WRITEDATE, WRITETIME, DSPSEQ            ";
                            SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRUSERIMAGE                        ";
                            SQL = SQL + ComNum.VBLF + "    WHERE GRPGB = '" + mstrGRPGB + "'                        ";
                            if (mstrGRPGB == "D")
                            {
                                SQL = SQL + ComNum.VBLF + "      AND USEGB = '" + cboDeptReg.Text.Trim() + "'                         ";
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + "      AND USEGB = '" + clsType.User.IdNumber + "'                         ";
                            }
                            SQL = SQL + ComNum.VBLF + "      AND IMAGENO = " + Convert.ToInt32(ssImageAll_Sheet1.Cells[i - 1, 0].Text) + "                         ";

                            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return false;
                            }

                            if (dt.Rows.Count == 0)
                            {
                                SQL = "";
                                SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRUSERIMAGE(GRPGB, USEGB, IMAGENO, USEID, WRITEDATE, WRITETIME, DSPSEQ)     ";
                                SQL = SQL + ComNum.VBLF + " VALUES(                                                        ";
                                SQL = SQL + ComNum.VBLF + "        '" + mstrGRPGB + "',                                                     ";
                                if (mstrGRPGB == "D")
                                {
                                    SQL = SQL + ComNum.VBLF + "        '" + cboDeptReg.Text.Trim() + "',                                    ";
                                }
                                else
                                {
                                    SQL = SQL + ComNum.VBLF + "        '" + clsType.User.IdNumber + "',                                    ";
                                }
                                SQL = SQL + ComNum.VBLF + "        " + Convert.ToInt32(ssImageAll_Sheet1.Cells[i - 1, 0].Text) + ",                                    ";
                                SQL = SQL + ComNum.VBLF + "        '" + clsType.User.IdNumber + "',                                    ";
                                SQL = SQL + ComNum.VBLF + "        '" + VB.Left(strCurDateTime, 8) + "',     ";
                                SQL = SQL + ComNum.VBLF + "        '" + VB.Right(strCurDateTime, 6) + "',     ";
                                SQL = SQL + ComNum.VBLF + "        999)            ";
                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBoxEx(this, SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return false;
                                }
                            }
                            dt.Dispose();
                            dt = null;
                        }
                    }
                }
                else if (optDept.Checked == true)
                {
                    for (i = 1; i <= mtsImageList1.Controls.Count; i++)
                    {
                        if (mtsImageList1.ItemX(i).Selected == true)
                        {
                            SQL = "";
                            SQL = " SELECT GRPGB, USEGB, IMAGENO, USEID, WRITEDATE, WRITETIME, DSPSEQ            ";
                            SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRUSERIMAGE                        ";
                            SQL = SQL + ComNum.VBLF + "    WHERE GRPGB = '" + mstrGRPGB + "'                        ";
                            if (mstrGRPGB == "D")
                            {
                                SQL = SQL + ComNum.VBLF + "      AND USEGB = '" + cboDeptReg.Text.Trim() + "'                         ";
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + "      AND USEGB = '" + clsType.User.IdNumber + "'                         ";
                            }
                            SQL = SQL + ComNum.VBLF + "      AND IMAGENO = " + Convert.ToInt32(ssImage_Sheet1.Cells[i - 1, 0].Text) + "                         ";

                            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return false;
                            }

                            if (dt.Rows.Count == 0)
                            {
                                SQL = "";
                                SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRUSERIMAGE(GRPGB, USEGB, IMAGENO, USEID, WRITEDATE, WRITETIME, DSPSEQ)     ";
                                SQL = SQL + ComNum.VBLF + " VALUES(                                                        ";
                                SQL = SQL + ComNum.VBLF + "        '" + mstrGRPGB + "',                                                     ";
                                if (mstrGRPGB == "D")
                                {
                                    SQL = SQL + ComNum.VBLF + "        '" + cboDeptReg.Text.Trim() + "',                                    ";
                                }
                                else
                                {
                                    SQL = SQL + ComNum.VBLF + "        '" + clsType.User.IdNumber + "',                                    ";
                                }
                                SQL = SQL + ComNum.VBLF + "        " + Convert.ToInt32(ssImage_Sheet1.Cells[i - 1, 0].Text) + ",                                    ";
                                SQL = SQL + ComNum.VBLF + "        '" + clsType.User.IdNumber + "',                                    ";
                                SQL = SQL + ComNum.VBLF + "        '" + VB.Left(strCurDateTime, 8) + "',     ";
                                SQL = SQL + ComNum.VBLF + "        '" + VB.Right(strCurDateTime, 6) + "',     ";
                                SQL = SQL + ComNum.VBLF + "        999)            ";
                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBoxEx(this, SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return false;
                                }
                            }
                            dt.Dispose();
                            dt = null;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, "저장하였습니다.");
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void mbtnDelete_Click(object sender, EventArgs e)
        {
            if (mstrGRPGB == "D")
            {
                if (cboDeptReg.Text.Trim() == "") return;
            }
            else
            {
                if (clsType.User.IdNumber == "") return;
            }

            if (DeleteData() == true)
            {
                DispBaseImageCheck();
            }
        }

        private bool DeleteData()
        {
            int i = 0;
            //DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                for (i = 1; i <= mtsImageList2.Controls.Count; i++)
                {
                    if (mtsImageList2.ItemX(i).Selected == true)
                    {
                        SQL = "";
                        SQL = "DELETE FROM " + ComNum.DB_EMR + "AEMRUSERIMAGE     ";
                        SQL = SQL + ComNum.VBLF + " WHERE GRPGB = '" + mstrGRPGB + "'                                        ";
                        if (mstrGRPGB == "D")
                        {
                            SQL = SQL + ComNum.VBLF + "   AND USEGB = '" + cboDeptReg.Text.Trim() + "'            ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "   AND USEGB = '" + clsType.User.IdNumber + "'            ";
                        }
                        SQL = SQL + ComNum.VBLF + "   AND USEID = '" + clsType.User.IdNumber + "'                                    ";
                        SQL = SQL + ComNum.VBLF + "   AND IMAGENO = " + Convert.ToInt32(ssImage2_Sheet1.Cells[i - 1, 0].Text) + "                                   ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this,  "삭제 하였습니다.");
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void mtsImageList1_ThumbnailClick(mtsImgList.ThumbImage Sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) return;

            lblImageName.Text = "";

            int intSel = mtsImageList1.Controls.IndexOf(Sender);
            string strName = mtsImageList1.ItemX(intSel + 1).TextLT;

            lblImageName.Text = strName;

            try
            {
                Ftpedt FtpedtX = new Ftpedt();
                //FtpedtX.FtpDownload("192.168.100.33", "emr", "emr1234", mstrFoldBase + "\\" + ssImageAll_Sheet1.Cells[intSel, 2].Text.Trim(), ssImageAll_Sheet1.Cells[intSel, 2].Text.Trim(), "/emr1/mtsemr/EmrImage/BaseImage");
                FtpedtX.FtpDownload(clsEmrType.EmrSvrInfo.EmrFtpSvrIp, clsEmrType.EmrSvrInfo.EmrFtpUser, clsEmrType.EmrSvrInfo.EmrFtpPasswd, mstrFoldBase + "\\" + ssImage2_Sheet1.Cells[intSel, 2].Text.Trim(), ssImage2_Sheet1.Cells[intSel, 2].Text.Trim(), clsEmrType.EmrSvrInfo.EmrFtpPath + "/EmrImage/BaseImage");
                FtpedtX = null;

                mtsImgMain1.ImagePath = mstrFoldBase + "\\" + ssImage_Sheet1.Cells[intSel, 2].Text.Trim();
                mtsImgMain1.DisplayImage();
                mtsImgMain1.ZoomOutImage();
                mtsImgMain1.ZoomInImage();

                Application.DoEvents();
                
                return;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                return;
            }
        }

        private void mtsImageList2_ThumbnailClick(mtsImgList.ThumbImage Sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) return;

            lblImageName2.Text = "";

            int intSel = mtsImageList2.Controls.IndexOf(Sender);
            string strName = mtsImageList2.ItemX(intSel + 1).TextLT;

            lblImageName2.Text = strName;


            try
            {
                Ftpedt FtpedtX = new Ftpedt();
                FtpedtX.FtpDownload(clsEmrType.EmrSvrInfo.EmrFtpSvrIp, clsEmrType.EmrSvrInfo.EmrFtpUser, clsEmrType.EmrSvrInfo.EmrFtpPasswd, mstrFoldBase + "\\" + ssImage2_Sheet1.Cells[intSel, 2].Text.Trim(), ssImage2_Sheet1.Cells[intSel, 2].Text.Trim(), clsEmrType.EmrSvrInfo.EmrFtpPath + "/EmrImage/BaseImage");
                FtpedtX = null;

                mtsImgMain2.ImagePath = mstrFoldBase + "\\" + ssImage2_Sheet1.Cells[intSel, 2].Text.Trim();
                mtsImgMain2.DisplayImage();
                mtsImgMain2.ZoomOutImage();
                mtsImgMain2.ZoomInImage();

                Application.DoEvents();
                
                return;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                return;
            }
        }

        private void trvEmrGroup_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode Node;

            Node = e.Node;
            if (Node.GetNodeCount(false) > 0)
            {
                return;
            }

            string strSGB = Node.Name.ToString();

            GetImageListAll(strSGB);
        }

        private void GetImageListAll(string strSGB)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            mtsImageListAll.Clear();
            mtsImgMain1.ClearImage();

            mtsImageListAll.tbSize = 100;

            //mPage = 0;
            ssImageAll_Sheet1.RowCount = 0;
            txtPage.Text = "";
            txtPageAll.Text = "";

            Cursor.Current = Cursors.WaitCursor;
            SQL = " SELECT A.IMAGENO, A.IMAGENAME, A.IMAGEEXT,  TO_CHAR(A.IMAGETHUMB) AS IMAGETHUMB";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASIMAGE A";
            SQL = SQL + ComNum.VBLF + "    WHERE A.IMAGEGB = '" + strSGB + "'";
            SQL = SQL + ComNum.VBLF + "    ORDER BY A.IMAGEGB, A.IMAGENO";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }
            ssImageAll_Sheet1.RowCount = dt.Rows.Count;
            ssImageAll_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssImageAll_Sheet1.Cells[i, 0].Text = dt.Rows[i]["IMAGENO"].ToString().Trim();
                ssImageAll_Sheet1.Cells[i, 1].Text = dt.Rows[i]["IMAGENAME"].ToString().Trim();
                ssImageAll_Sheet1.Cells[i, 2].Text = dt.Rows[i]["IMAGEEXT"].ToString().Trim();

                byte[] b = Convert.FromBase64String(dt.Rows[i]["IMAGETHUMB"].ToString());
                MemoryStream stream = new MemoryStream(b);
                Bitmap image1 = new Bitmap(stream);

                int intWidth = 70;
                int intHeight = 80;

                Bitmap newImage;
                if (image1.Width > image1.Height)
                {
                    intWidth = 80;
                    intHeight = 70;
                }

                newImage = new Bitmap(intWidth, intHeight, PixelFormat.Format24bppRgb);
                Graphics graphics_1 = Graphics.FromImage(newImage);
                graphics_1.CompositingQuality = CompositingQuality.HighQuality;
                graphics_1.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics_1.SmoothingMode = SmoothingMode.HighQuality;
                graphics_1.DrawImage(image1, 0, 0, intWidth, intHeight);

                newImage.Save(mstrFoldBase + "\\thumb_" + dt.Rows[i]["IMAGEEXT"].ToString().Trim());

                graphics_1.Dispose();
                graphics_1 = null;

                image1.Dispose();
                image1 = null;

                newImage.Dispose();
                newImage = null;

                mtsImageListAll.AddThumbnail(mstrFoldBase + "\\thumb_" + dt.Rows[i]["IMAGEEXT"].ToString().Trim(), dt.Rows[i]["IMAGENAME"].ToString().Trim());
                Application.DoEvents();
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        private void mtsImageListAll_ThumbnailClick(mtsImgList.ThumbImage Sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) return;

            lblImageName.Text = "";

            int intSel = mtsImageListAll.Controls.IndexOf(Sender);
            string strName = mtsImageListAll.ItemX(intSel + 1).TextLT;

            lblImageName.Text = strName;

            try
            {
                Ftpedt FtpedtX = new Ftpedt();
                FtpedtX.FtpDownload(clsEmrType.EmrSvrInfo.EmrFtpSvrIp, clsEmrType.EmrSvrInfo.EmrFtpUser, clsEmrType.EmrSvrInfo.EmrFtpPasswd, mstrFoldBase + "\\" + ssImageAll_Sheet1.Cells[intSel, 2].Text.Trim(), ssImageAll_Sheet1.Cells[intSel, 2].Text.Trim(), clsEmrType.EmrSvrInfo.EmrFtpPath + "/EmrImage/BaseImage");
                FtpedtX = null;

                mtsImgMain1.ImagePath = mstrFoldBase + "\\" + ssImageAll_Sheet1.Cells[intSel, 2].Text.Trim();
                mtsImgMain1.DisplayImage();
                mtsImgMain1.ZoomOutImage();
                mtsImgMain1.ZoomInImage();

                Application.DoEvents();
                
                return;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                return;
            }
        }

        private void optDept_CheckedChanged(object sender, EventArgs e)
        {
            if (optDept.Checked == true)
            {
                panAll.Visible = false;
                panDept.Visible = true;

                mtsImageListAll.Clear();
                mtsImageList1.Clear();
                mtsImgMain1.ClearImage();
                ssImageAll_Sheet1.RowCount = 0;
                ssImage_Sheet1.RowCount = 0;

                DispBaseImage(cboDept.Text.Trim());
            }
        }

        private void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (optDept.Checked == true)
            {
                DispBaseImage(cboDept.Text.Trim());
            }
        }

        private void cboDeptReg_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mstrGRPGB == "D")
            {
                DispBaseImageCheck();
            }
        }

        private void SetRegType()
        {
            mtsImageList2.Clear();
            mtsImgMain2.ClearImage();
            mtsImageList2.tbSize = 100;

            ssImage2_Sheet1.RowCount = 0;

            if (optRegUse.Checked == true)
            {
                mstrGRPGB = "U";
                mstrUSEGB = clsType.User.IdNumber;
                optDept.Visible = true;
                cboDept.Visible = true;
                cboDeptReg.Visible = false;

                this.Text = "개인별 이미지기록지 등록";
                lblTitle.Text = "개인별 이미지기록지 등록";
                lblGRPGB.Text = "개인별 이미지기록지";
                DispBaseImageCheck();
            }
            else if (optRegDept.Checked == true)
            {
                mstrGRPGB = "D";
                mstrUSEGB = "";

                optDept.Visible = false;
                cboDept.Visible = false;
                cboDeptReg.Visible = true;

                this.Text = "과별 이미지기록지 등록";
                lblTitle.Text = "과별 이미지기록지 등록";
                lblGRPGB.Text = "과별 이미지기록지";

                for (int i = 0; i < cboDeptReg.Items.Count; i++)
                {
                    if (clsType.User.DeptCode == cboDeptReg.Items[i].ToString().Trim())
                    {
                        cboDeptReg.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private void optRegUse_CheckedChanged(object sender, EventArgs e)
        {
            if (optRegUse.Checked == true)
            {
                SetRegType();
            }
        }

        private void optRegDept_CheckedChanged(object sender, EventArgs e)
        {
            if (optRegDept.Checked == true)
            {
                SetRegType();
            }
        }






    }
}
