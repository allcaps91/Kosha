using ComBase;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
//using EnterpriseDT.Net.Ftp;

namespace ComEmrBase
{
    public partial class frmEmrImageDrawing : Form
    {

        //이미지를 저장한 경우
        public delegate void SetSavedImage(string strFormNo, string strUpdateNo, string strEmrNo, string strItemName, string strFoldJob, string strSaveFlag, string strImageName, Control pCont);
        public event SetSavedImage rSetSavedImage;

        //이미지를 삭제한 경우
        public delegate void DeleteImage(string strFormNo, string strUpdateNo, Control pCont, Image basImage);
        public event DeleteImage rDeleteImage;

        //폼이 Close될 경우
        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        //private string mstrCallFlag = "A";  //기록지에서 왔는지 , 단독으로 띄웠는지 확인
        public string mstrTag = ""; //기존 등록된 이미지 이름
        public Image mbasImage = null;

        #region // 기록지 관련 변수(기록지번호, 명칭 등 수정요함)
        public FormEmrMessage mEmrCallForm;
        public string mstrFormNo = "";
        public string mstrUpdateNo = "0";
        public string mstrFormText = "";
        public EmrPatient p = null;
        public string mstrEmrNo = "0";
        public string mstrMode = "";
        //private mtsPanel.TransparentPanel panEditLock = null;
        public string mstrItemName = "";
        public string mstrSaveFlag = "1"; //인증저장
        public string mstrFormGb = "2"; //이미지
        private usTimeSet usTimeSetEvent;

        #endregion

        //변경을 하면 아이템이름_Update.png

        private string mstrFoldJob = @"\EmrImageTmp\New\";
        private string mstrFoldBase = @"\EmrImageTmp\New\";

        private Control fCont = null;

        private string mstrPath = "";

        public frmEmrImageDrawing()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 경과,재진,수술 기록
        /// </summary>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="po"></param>
        /// <param name="strEmrNo"></param>
        /// <param name="strMode"></param>
        public frmEmrImageDrawing(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, FormEmrMessage pEmrCallForm)
        {
            InitializeComponent();
            
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            p = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
            mEmrCallForm = pEmrCallForm;
        }

        /// <summary>
        /// 기본이미지를 기초코드에 등록한 경우 사용
        /// </summary>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="strEmrNo"></param>
        /// <param name="strMode"></param>
        /// <param name="strItemName"></param>
        /// <param name="pCont"></param>
        /// <param name="strTag"></param>
        /// <param name="pEmrCallForm"></param>
        public frmEmrImageDrawing(string strFormNo, string strUpdateNo, string strEmrNo, string strMode, string strItemName, Control pCont, string strTag, FormEmrMessage pEmrCallForm)
        {
            InitializeComponent();
            
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            mstrEmrNo = strEmrNo;
            //mstrMode = strMode;
            mstrItemName = strItemName;
            fCont = pCont;
            mstrTag = strTag;
            mEmrCallForm = pEmrCallForm;
        }

        /// <summary>
        /// 기본이미지를 서식지정보에 등록한 경우 사용
        /// </summary>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="strEmrNo"></param>
        /// <param name="strMode"></param>
        /// <param name="strItemName"></param>
        /// <param name="pCont"></param>
        /// <param name="strTag"></param>
        /// <param name="pEmrCallForm"></param>
        public frmEmrImageDrawing(string strFormNo, string strUpdateNo, string strEmrNo, string strMode, string strItemName, Control pCont, string strTag, Image basImage, FormEmrMessage pEmrCallForm)
        {
            InitializeComponent();

            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            mstrEmrNo = strEmrNo;
            //mstrMode = strMode;
            mstrItemName = strItemName;
            fCont = pCont;
            mstrTag = strTag;
            mbasImage = basImage;
            mEmrCallForm = pEmrCallForm;
        }

        private void mbtnTime_Click(object sender, EventArgs e)
        {
            //mMaskBox = mkText;
            if (usTimeSetEvent != null)
            {
                usTimeSetEvent.Dispose();
                usTimeSetEvent = null;
            }
            usTimeSetEvent = new usTimeSet();
            usTimeSetEvent.rSetTime += new usTimeSet.SetTime(usTimeSetEvent_SetTime);
            usTimeSetEvent.rEventClosed += new usTimeSet.EventClosed(usTimeSetEvent_EventClosed);
            this.Controls.Add(usTimeSetEvent);
            usTimeSetEvent.Top = txtMedFrTime.Top - 5;
            usTimeSetEvent.Left = txtMedFrTime.Left;
            usTimeSetEvent.BringToFront();
        }

        private void frmEmrImageDrawing_Load(object sender, EventArgs e)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = " SELECT BASCD ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD ";
            SQL = SQL + ComNum.VBLF + "WHERE BSNSCLS = '권한관리' ";
            SQL = SQL + ComNum.VBLF + "    AND UNITCLS = 'TOP' ";
            SQL = SQL + ComNum.VBLF + "    AND BASEXNAME = '보험심사팀' ";
            SQL = SQL + ComNum.VBLF + "    AND BASCD = '" + clsType.User.IdNumber + "' ";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count > 0)
            {
                mbtnPrint.Enabled = true;
            }
            dt.Dispose();
            dt = null;
            
            if (mstrMode == "")
            {
                clsEmrFunc.CheckImageJobFold(ref mstrFoldJob, ref mstrFoldBase, mstrFormNo, mstrUpdateNo, mstrEmrNo, mstrItemName);
                panChartDate.Visible = false;
                SetDefaultImage();
            }
            else
            {
                mbtnExit.Visible = false;
                panChartDate.Visible = true;
                clsEmrFunc.SetTimeComboBox(txtMedFrTime);
                SetItemCd();
                clsEmrFunc.CheckImageJobFold(ref mstrFoldJob, ref mstrFoldBase, mstrFormNo, mstrUpdateNo, mstrEmrNo, mstrItemName);

                if (VB.Val(mstrEmrNo) > 0)
                {
                    GetDataImage();
                }
                else
                {
                    dtMedFrDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
                }
            }
            optUse.Checked = true;
        }

        private void SetItemCd()
        {
            mstrItemName = "I0000029773_0"; //재진이미지
            if (mstrFormNo == "337") 
            {
                mstrItemName = "I0000029770_0"; //경과이미지
            }
            else if (mstrFormNo == "336")
            {
                mstrItemName = "I0000029773_0"; //재진이미지
            }
        }

        private void usTimeSetEvent_SetTime(string strText)
        {
            if (usTimeSetEvent != null)
            {
                usTimeSetEvent.Dispose();
                usTimeSetEvent = null;
            }
            txtMedFrTime.Text = strText;
        }

        private void usTimeSetEvent_EventClosed()
        {
            if (usTimeSetEvent != null)
            {
                usTimeSetEvent.Dispose();
                usTimeSetEvent = null;
            }
        }

        /// <summary>
        /// 저장된 이미지가 있으면 불러 온다.
        /// </summary>
        private void GetDataImage()
        {
            if (VB.Val(mstrEmrNo) <= 0) return;

            mstrTag = "";

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strITEMCD = "";
            string strITEMVALUE = "";

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT A.EMRNO, A.CHARTDATE, A.CHARTTIME, A.FORMNO, A.UPDATENO, ";
            SQL = SQL + ComNum.VBLF + "        R.ITEMCD,  R.IMAGENO AS ITEMVALUE";
            SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
            SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTIMAGE R";
            SQL = SQL + ComNum.VBLF + "        ON R.EMRNO = A.EMRNO";
            SQL = SQL + ComNum.VBLF + "    WHERE A.EMRNO = " + VB.Val(mstrEmrNo);
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
            strITEMCD = dt.Rows[i]["ITEMCD"].ToString().Trim();
            strITEMVALUE = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
            mstrTag = strITEMVALUE;
            dtMedFrDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(dt.Rows[i]["CHARTDATE"].ToString().Trim(), "D"));
            txtMedFrTime.Text = ComFunc.FormatStrToDate(dt.Rows[i]["CHARTTIME"].ToString().Trim(), "M");
            dt.Dispose();
            dt = null;

            clsEmrFunc.CheckImageJobFold(ref mstrFoldJob, ref mstrFoldBase, mstrFormNo, mstrUpdateNo, mstrEmrNo, strITEMCD);

            //서버의 이미지를 다운로드한다.
            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT A.EMRNO, A.ITEMCD, A.IMAGENO, A.IMGSVR, A.IMGPATH, A.IMGEXTENSION ";
            SQL = SQL + ComNum.VBLF + "        , B.BASNAME , B.BASEXNAME , B.REMARK1, B.REMARK2 , B.VFLAG1 ";
            SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRCHARTIMAGE A";
            SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "        ON B.BSNSCLS = '기록지관리'";
            SQL = SQL + ComNum.VBLF + "        AND B.UNITCLS = '이미지PATH'";
            SQL = SQL + ComNum.VBLF + "        AND B.BASCD = A.IMGSVR";
            SQL = SQL + ComNum.VBLF + "    WHERE A.EMRNO = " + VB.Val(mstrEmrNo);
            SQL = SQL + ComNum.VBLF + "        AND A.ITEMCD = '" + strITEMCD + "'";

            DataTable dt1 = null;
            SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            
            if (dt1.Rows.Count == 0)
            {
                dt1.Dispose();
                dt1 = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            string strImageNameJpg = dt1.Rows[0]["IMAGENO"].ToString().Trim() + ".jpg";
            string strImageNameDgr = dt1.Rows[0]["IMAGENO"].ToString().Trim() + ".dgr";
            string strServerImgPath = dt1.Rows[0]["IMGPATH"].ToString().Trim();

            string ServerAddress = dt1.Rows[0]["BASNAME"].ToString().Trim();
            string strServerPath = dt1.Rows[0]["BASEXNAME"].ToString().Trim();
            //string ServerPort = "21";
            string UserName = dt1.Rows[0]["REMARK1"].ToString().Trim();
            string Password = dt1.Rows[0]["REMARK2"].ToString().Trim();
            string HomePath = dt1.Rows[0]["VFLAG1"].ToString().Trim();

            dt1.Dispose();
            dt1 = null;

            Ftpedt FtpedtX = new Ftpedt();
            FtpedtX.FtpDownload(ServerAddress, UserName, Password, mstrFoldJob + "\\" + strImageNameJpg, strImageNameJpg, strServerPath + "/" + strServerImgPath);
            FtpedtX = null;
            
            //이미지를 표시한다.
            SetBackImageOld();

            Cursor.Current = Cursors.Default;
        }

        private void frmEmrImageDrawing_FormClosed(object sender, FormClosedEventArgs e)
        {
            mtsImgMain1.ImagePath = "";
        }

        private void optAll_CheckedChanged(object sender, EventArgs e)
        {
            if (optAll.Checked == true)
            {
                DispBaseImage("A");
            }
        }

        private void optDept_CheckedChanged(object sender, EventArgs e)
        {
            if (optDept.Checked == true)
            {
                DispBaseImage("D");
            }
        }

        private void optUse_CheckedChanged(object sender, EventArgs e)
        {
            if (optUse.Checked == true)
            {
                DispBaseImage("U");
            }
        }

        private void SetDefaultImage()
        {
            //기존 등록된 이미지가 있는지 확인하고 없으면 기본 배경이미지를 보여준다.
            if (mstrTag == "")
            {
                SetBackImageInit();
            }
            else
            {
                SetBackImageOld();
            }
        }

        public void SetBackImageInit()
        {
            string strBaseImage = "";
            if (mbasImage == null) return;
            strBaseImage = clsEmrType.EmrSvrInfo.EmrClient + "\\baseImageTmp.png";
            SaveImg(strBaseImage, mbasImage);
            //mbasImage.Save(strBaseImage, ImageFormat.Png);
            axActXForm1.PathBack(strBaseImage);
        }

        private void SaveImg(string strFile, Image img)
        {
            using (Bitmap bitmap = new Bitmap(img))
            {
                bitmap.Save(strFile, ImageFormat.Png);
            }
        }



        public void SetBackImageInitOld()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                
                SQL = " SELECT ";
                SQL = SQL + ComNum.VBLF + "        A.FORMNO, A.UPDATENO, A.CONTROLID, A.GRPTYPE, A.GRPGB, A.USEGB, A.IMAGENO, B.IMAGEEXT";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRFORMIMAGE A";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRBASIMAGE B";
                SQL = SQL + ComNum.VBLF + "        ON A.IMAGENO = B.IMAGENO";
                SQL = SQL + ComNum.VBLF + "    WHERE A.FORMNO = " + mstrFormNo;
                SQL = SQL + ComNum.VBLF + "        AND A.UPDATENO = " + mstrUpdateNo;
                SQL = SQL + ComNum.VBLF + "        AND A.CONTROLID = '" + mstrItemName + "'";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    //string mstrFoldJob = @"\EmrImageTmp\";
                    string strImageName = dt.Rows[0]["IMAGEEXT"].ToString().Trim();
                    
                    Ftpedt FtpedtX = new Ftpedt();
                    FtpedtX.FtpDownload(clsEmrType.EmrSvrInfo.EmrFtpSvrIp, clsEmrType.EmrSvrInfo.EmrFtpUser, clsEmrType.EmrSvrInfo.EmrFtpPasswd, mstrFoldJob + "\\" + strImageName, strImageName, clsEmrType.EmrSvrInfo.EmrFtpPath + "/EmrImage/BaseImage");
                    FtpedtX = null;

                    //이미지 사이즈 조정
                    string strTempPath = ChangeSize(mstrFoldJob, strImageName);
                    //백그라운드 이미지
                    axActXForm1.PathBack(strTempPath);

                    File.Delete(mstrFoldJob + strImageName);
                    File.Delete(strTempPath);
                    
                }
                dt.Dispose();
                dt = null;
                   
            }
            catch
            {
            }
        }

        public void SetBackImageOld()
        {
            try
            {
                if (VB.Val(mstrEmrNo) == 0)
                {
                    //임시폴드에 있는 이미지를 로드한다
                    string strDgr = "";
                    strDgr = mstrTag.ToUpper().Replace(".JPG", "");
                    strDgr = mstrFoldJob + "\\" + strDgr;
                    axActXForm1.Path(strDgr);
                }
                else
                {
                    //기존 작성된 이미지를 로드한다.
                    string strDgr = "";
                    strDgr = mstrTag.ToUpper().Replace(".JPG", "");
                    strDgr = mstrFoldJob + "\\" + strDgr;
                    axActXForm1.Path(strDgr);
                }
            }
            catch
            {
            }
        }

        private void DispBaseImage(string strGRPGB)
        {
            //테이블을 파일을 내려 받는다.
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            mtsImageList1.Clear();
            mtsImgMain1.ClearImage();
            mtsImageList1.tbSize = 80;

            ssImage_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL = " SELECT A.IMAGENO, A.IMAGENAME, A.IMAGEEXT,  TO_CHAR(A.IMAGETHUMB) AS IMAGETHUMB";
            SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRBASIMAGE A";
            SQL = SQL + ComNum.VBLF + "    LEFT OUTER JOIN " + ComNum.DB_EMR + "AEMRIMAGEGUBUN B";
            SQL = SQL + ComNum.VBLF + "        ON A.IMAGECODE = B.IMAGECODE";
            SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRUSERIMAGE C                 ";
            SQL = SQL + ComNum.VBLF + "        ON A.IMAGENO = C.IMAGENO                     ";
            SQL = SQL + ComNum.VBLF + "    WHERE C.GRPGB = '" + strGRPGB + "'                             ";
            if (strGRPGB == "D")
            {
                SQL = SQL + ComNum.VBLF + "      AND C.USEGB = '" + clsType.User.DeptCode + "'                            ";
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

        private void mbtnExit_Click(object sender, EventArgs e)
        {
            rEventClosed();
        }

        private void mtsImageList1_ThumbnailClick(mtsImgList.ThumbImage Sender, MouseEventArgs e)
        {
            lblImageName.Text = "";

            int i = 0;
            int intSel = mtsImageList1.Controls.IndexOf(Sender);
            string strName = mtsImageList1.ItemX(intSel + 1).TextLT;

            lblImageName.Text = strName;

            for (i = 1; i <= mtsImageList1.Controls.Count; i++)
            {
                mtsImageList1.ItemX(i).Selected = false;
                mtsImageList1.ItemX(i).BackColor = Color.LightGray;
            }

            if (mtsImageList1.ItemX(intSel + 1).Selected == false)
            {
                mtsImageList1.ItemX(intSel + 1).Selected = true;
                mtsImageList1.ItemX(intSel + 1).BackColor = Color.Blue;
            }


            try
            {
                
                Ftpedt FtpedtX = new Ftpedt();
                FtpedtX.FtpDownload(clsEmrType.EmrSvrInfo.EmrFtpSvrIp, clsEmrType.EmrSvrInfo.EmrFtpUser, clsEmrType.EmrSvrInfo.EmrFtpPasswd, mstrFoldBase + "\\" + ssImage_Sheet1.Cells[intSel, 2].Text.Trim(), ssImage_Sheet1.Cells[intSel, 2].Text.Trim(), clsEmrType.EmrSvrInfo.EmrFtpPath + "/EmrImage/BaseImage");
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
                ComFunc.MsgBoxEx(this,  ex.Message);
                return;
            }
        }

        private void mtsImageList1_ThumbnailDoubleClick(mtsImgList.ThumbImage Sender, MouseEventArgs e)
        {
            int intSel = mtsImageList1.Controls.IndexOf(Sender);
            string strPath = mstrFoldBase + "\\" + ssImage_Sheet1.Cells[intSel, 2].Text.Trim();
            string strName = ssImage_Sheet1.Cells[intSel, 2].Text.Trim();

            //이미지 사이즈 조정
            string strTempPath = ChangeSize(mstrFoldBase + "\\", ssImage_Sheet1.Cells[intSel, 2].Text.Trim());
            //백그라운드 이미지
            axActXForm1.PathBack(strTempPath);

        }

        private string ChangeSize(string strPath, string strImage)
        {
            string rtnVal = "";
            string strFilePath = strPath + strImage;
            string sFileNameNew = "TmpImage.jpg";

            mstrPath = strPath + "TmpImage";

            try
            {
                //Canvas Size 계산
                int intWidthD = axActXForm1.Width - 20;
                int intHeightD = axActXForm1.Height- 170;
                //이미지 사이즈
                int intWidth = 660;
                int intHeight = 700;

                Bitmap image1 = null;
                FileStream fs = new FileStream(strFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                image1 = (Bitmap)Image.FromStream(fs);
                
                if (image1.Width > image1.Height)
                {
                    intWidth = intHeightD;
                    intHeight = (intWidthD * image1.Height) / image1.Width;
                }
                else
                {
                    intHeight = intHeightD;
                    intWidth = (intWidthD * image1.Width) / image1.Height;
                }

                Bitmap newImage = null;
                newImage = new Bitmap(intWidth, intHeight, PixelFormat.Format24bppRgb);

                Graphics graphics_1 = Graphics.FromImage(newImage);
                graphics_1.CompositingQuality = CompositingQuality.HighQuality;
                graphics_1.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics_1.SmoothingMode = SmoothingMode.HighQuality;
                graphics_1.DrawImage(image1, 0, 0, intWidth, intHeight);

                newImage.Save(strPath + sFileNameNew, ImageFormat.Jpeg);

                graphics_1.Dispose();
                graphics_1 = null;

                image1.Dispose();
                image1 = null;

                newImage.Dispose();
                newImage = null;

                fs.Dispose();
                fs = null;

                rtnVal = strPath + sFileNameNew;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this,  ex.Message);
                return rtnVal;
            }
        }

        private void mbtnLoadBackImage_Click(object sender, EventArgs e)
        {
            //string strText1 = @"D:\Image\0052.gif";
            //axActXForm1.PathBack(strText1);
            axActXForm1.Path(mstrFoldJob + "\\" + mstrItemName);
        }

        private void mbtnClearBack_Click(object sender, EventArgs e)
        {
            axActXForm1.BackClear();
        }

        private void mbtnSave_Click(object sender, EventArgs e)
        {
            if (mstrMode == "")
            {
                SaveDataForm();
            }
            else
            {
                SaveDataNotForm();
            }
        }

        private void SaveDataForm()
        {
            string strImageNo = "";

            if (mstrTag != "")
            {
                //기존이미지를 삭제하고
                if (File.Exists(mstrFoldJob + "\\" + mstrTag + ".jpg") == true) File.Delete(mstrFoldJob + "\\" + mstrTag + ".jpg");
                if (File.Exists(mstrFoldJob + "\\" + mstrTag + ".dgr") == true) File.Delete(mstrFoldJob + "\\" + mstrTag + ".dgr");
            }

            strImageNo = (ComQuery.GetSequencesNo(clsDB.DbCon , "" + ComNum.DB_EMR + "GETSEQ_AEMRCHARTIMAGE")).ToString();
            string strText1 = mstrFoldJob + "\\" + strImageNo;
            axActXForm1.SaveU(strText1);
            int intSaveName = axActXForm1.SaveName;

            string strImageName = strImageNo; //strImageNo + SEQ

            if (intSaveName == 1)
            {
                //ComFunc.MsgBoxEx(this, "저장완료");
                rSetSavedImage(mstrFormNo, mstrUpdateNo, mstrEmrNo, strImageNo, mstrFoldJob, mstrSaveFlag, strImageName, fCont);
                return;
            }
            else if (intSaveName == 0)
            {
                ComFunc.MsgBoxEx(this, "저장중 에러가 발생했습니다.");
            }
        }

        private void SaveDataNotForm()
        {
            string strImageNo = "";

            if (mstrTag != "")
            {
                //기존이미지를 삭제하고
                if (File.Exists(mstrFoldJob + "\\" + mstrTag + ".jpg") == true) File.Delete(mstrFoldJob + "\\" + mstrTag + ".jpg");
                if (File.Exists(mstrFoldJob + "\\" + mstrTag + ".dgr") == true) File.Delete(mstrFoldJob + "\\" + mstrTag + ".dgr");
            }
            
            strImageNo = (ComQuery.GetSequencesNo(clsDB.DbCon, "" + ComNum.DB_EMR + "GETSEQ_AEMRCHARTIMAGE")).ToString();

            string strText1 = mstrFoldJob + "\\" + strImageNo;
            axActXForm1.SaveU(strText1);
            int intSaveName = axActXForm1.SaveName;
            
            string strImageName = strImageNo; //strImageNo + SEQ

            if (intSaveName == 1)
            {
                //rSetSavedImage(mstrFormNo, mstrUpdateNo, mstrEmrNo, strImageNo, mstrFoldJob, strSaveFlag, strImageName, fCont);
                if (SavedImageChart(strImageNo, strImageName) == true)
                {
                    mEmrCallForm.MsgSave("0");
                }
                
                return;
            }
            else if (intSaveName == 0)
            {
                ComFunc.MsgBoxEx(this, "저장중 에러가 발생했습니다.");
            }
        }

        private bool SavedImageChart(string strImageNo, string strImageName)
        {
            Cursor.Current = Cursors.WaitCursor;
            string SqlErr = "";

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                //TODO gGetSequencesNo
                double dblEmrNoNew = 0; // ComQuery.GetSequencesNo("" + ComNum.DB_EMR + "GETSEQ_AEMRCHARTMST");

                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
                string strCurDate = VB.Left(strCurDateTime, 8);
                string strCurTime = VB.Right(strCurDateTime, 6);
                double dblEmrHisNo = ComQuery.GetSequencesNo(clsDB.DbCon, "" + ComNum.DB_EMR + "GETSEQ_AEMRCHARTMSTHIS");

                if (VB.Val(mstrEmrNo) > 0)
                {
                    #region //과거기록 백업
                    SqlErr = clsEmrQuery.SaveChartMastHis(clsDB.DbCon, mstrEmrNo, dblEmrHisNo, strCurDate, strCurTime, "C", "", clsType.User.IdNumber);
                    if (SqlErr != "OK")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, SqlErr);
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                    #endregion

                    dblEmrNoNew = VB.Val(mstrEmrNo);
                }
                else
                {
                    dblEmrNoNew = ComQuery.GetSequencesNo(clsDB.DbCon, "" + ComNum.DB_EMR + "GETSEQ_AEMRCHARTMST");
                }

                string strSAVECERT = "1";
                if (clsEmrQuery.SaveChartMstOnly(clsDB.DbCon, dblEmrNoNew, dblEmrHisNo, p, mstrFormNo, mstrUpdateNo,
                                    dtMedFrDate.Value.ToString("yyyyMMdd"), txtMedFrTime.Text.Trim().Replace(":",""),
                                    clsType.User.IdNumber, clsType.User.IdNumber, mstrSaveFlag, strSAVECERT, mstrFormGb,
                                    strCurDate, strCurTime, "") == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                
                if (clsEmrQuery.SaveChartImageOnly(clsDB.DbCon, mstrFormNo, mstrUpdateNo, dblEmrNoNew, VB.Val(mstrEmrNo), mstrItemName, strImageNo) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this,  "저장 하였습니다.");
                Cursor.Current = Cursors.Default;

                //데이타를 다시 설정을 한다.
                mstrEmrNo = dblEmrNoNew.ToString();
                mstrTag = strImageNo;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this,  ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void mbtnClear_Click(object sender, EventArgs e)
        {
            //axActXForm1.BackClear();
            axActXForm1.New();
        }

        private void mbtnDelete_Click(object sender, EventArgs e)
        {
            
            if (mstrMode == "")
            {
                axActXForm1.BackClear();
                axActXForm1.New();
                SetBackImageInit();
                mstrTag = "";
                rDeleteImage(mstrFormNo, mstrUpdateNo, fCont, mbasImage);
            }
            else
            {
                DeleteDataNotForm();
            }
        }

        private void DeleteDataNotForm()
        {
            if (VB.Val(mstrEmrNo) == 0)
            {
                return;
            }
            //TODO gDeleteEmrXml
            //if (clsXML.gDeleteEmrXml(mstrEmrNo, clsType.User.IdNumber) == true)
            //{
            //    mstrEmrNo = "0";
            //    axActXForm1.BackClear();
            //    axActXForm1.New();
            //    SetDefaultImage();
            //    mEmrCallForm.MsgDelete();
            //}
        }

        private void mbtnSaveUserImage_Click(object sender, EventArgs e)
        {
            using (frmEmrUserImageReg frm = new frmEmrUserImageReg("U", ""))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog();
            }

            if (optUse.Checked == false)
            {
                optUse.Checked = true;
            }
            else
            {
                DispBaseImage("U");
            }
        }

        private void mbtnPrint_Click(object sender, EventArgs e)
        {
            if (VB.Val(mstrEmrNo) <= 0) return;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                System.Drawing.Printing.PrintDocument ScanPrint = new System.Drawing.Printing.PrintDocument();
                System.Drawing.Printing.PrintController printController = new System.Drawing.Printing.StandardPrintController();
                ScanPrint.PrintController = printController;  //기본인쇄창 없애기

                System.Drawing.Printing.PageSettings ps = new System.Drawing.Printing.PageSettings();
                ps.Margins = new System.Drawing.Printing.Margins(10, 10, 10, 10);
                ScanPrint.DefaultPageSettings = ps;

                ScanPrint.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(pBox_PrintPageOne);

                ScanPrint.Print();
                Cursor.Current = Cursors.Default;
                return;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this,  ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void pBox_PrintPageOne(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            string strDgr = "";
            strDgr = mstrTag.ToUpper().Replace(".JPG", "");
            strDgr = mstrFoldJob + "\\" + strDgr + ".jpg";

            Bitmap image1 = (Bitmap)Image.FromFile(strDgr, true);

            Image pImage = image1;

            if (pImage.Size.Width > pImage.Size.Height)
            {
                e.Graphics.DrawImage(pImage, 30, 30, 1100, 760); //'가로 이미지 크기
            }
            else
            {
                e.Graphics.DrawImage(pImage, 30, 30, 760, 1100); //'세로 이미지 크기
            }

            image1 = null;

            e.HasMorePages = false;
        }

        /// <summary>
        /// 동국대 경주병원만 사용함
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mbtnSmartPhonImage_Click(object sender, EventArgs e)
        {
            Image img = null;
            img = Clipboard.GetImage();

            //axActXForm1.Path(@"C:\마리아홀.png");

            axActXForm1.PathBack(@"C:\마리아홀.png");
        }

        


    }
}
