using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ComBase
{
    public partial class frmCopyScanView : Form
    {
        string mstrPageNo = string.Empty;

        string strViewFileName = "";

        public frmCopyScanView(string strPageNo)
        {
            InitializeComponent();
            mstrPageNo = strPageNo;
        }

        private void frmCopyScanView_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회

            //picBig1.Visible = true;
            LoadScanChartPSMH();
        }

        #region LoadScanChartPSMH
        /// <summary>
        /// TREATNO로 환자 스캔 정보 가져오는 함수
        /// </summary>
        private void LoadScanChartPSMH()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            if (VB.Val(mstrPageNo) == 0)
            {
                return;
            }

            int i = 0;
            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                #region 현재 살아있는 스캔 이미지
                SQL.AppendLine(" SELECT  ");
                SQL.AppendLine("   T.PATID, C.TREATNO, C.PAGENO, C.PAGE, P.PATHID,  ");
                SQL.AppendLine("    CASE  ");
                SQL.AppendLine("        WHEN P.EXTENSION = '' OR P.EXTENSION IS NULL THEN 'tif'  ");
                SQL.AppendLine("        ELSE P.EXTENSION  ");
                SQL.AppendLine("    END AS EXTENSION,  ");
                SQL.AppendLine("    C.SECURITY, P.FILESIZE, P.CDATE, ");
                SQL.AppendLine("    C.FORMCODE, C.UNREADY, C.CDNO, T.CLASS , ");
                SQL.AppendLine("    (SELECT C1.NAME  ");
                SQL.AppendLine("        FROM KOSMOS_EMR.EMR_CLINICT C1  ");
                SQL.AppendLine("        WHERE C1.CLINCODE = T.CLINCODE) AS LOCATIONNM,  ");
                SQL.AppendLine("    T.INDATE, P.LOCATION, ");
                SQL.AppendLine("    S.IPADDRESS, S.FTPUSER, S.FTPPASSWD, S.LOCALPATH, ");
                SQL.AppendLine("    ( S.LOCALPATH || '/' || P.LOCATION ) AS SVRFILEPATH  ");
                SQL.AppendLine("FROM KOSMOS_EMR.EMR_PAGET P  ");
                SQL.AppendLine("INNER JOIN KOSMOS_EMR.EMR_CHARTPAGET C ");
                SQL.AppendLine("   ON P.PAGENO = C.PAGENO ");
                SQL.AppendLine("  AND C.PAGENO = " + VB.Val(mstrPageNo));
                SQL.AppendLine("  AND C.PAGE > 0 ");
                SQL.AppendLine("INNER JOIN KOSMOS_EMR.EMR_TREATT T ");
                SQL.AppendLine("   ON C.TREATNO = T.TREATNO ");
                SQL.AppendLine("INNER JOIN KOSMOS_EMR.EMR_PATHT S ");
                SQL.AppendLine("   ON P.PATHID = S.PATHID ");
                #endregion

                #region 변환하면서 날라간 리스트
                SQL.AppendLine(" UNION ALL  ");
                SQL.AppendLine(" SELECT  ");
                SQL.AppendLine("   T.PATID, C.TREATNO, C.PAGENO, C.PAGE, P.PATHID,  ");
                SQL.AppendLine("    CASE  ");
                SQL.AppendLine("        WHEN P.EXTENSION = '' OR P.EXTENSION IS NULL THEN 'tif'  ");
                SQL.AppendLine("        ELSE P.EXTENSION  ");
                SQL.AppendLine("    END AS EXTENSION,  ");
                SQL.AppendLine("    '' AS SECURITY, P.FILESIZE, P.CDATE, ");
                SQL.AppendLine("    C.FORMCODE, '' AS UNREADY, '' AS CDNO, T.CLASS , ");
                SQL.AppendLine("    (SELECT C1.NAME  ");
                SQL.AppendLine("        FROM KOSMOS_EMR.EMR_CLINICT C1  ");
                SQL.AppendLine("        WHERE C1.CLINCODE = T.CLINCODE) AS LOCATIONNM,  ");
                SQL.AppendLine("    T.INDATE, P.LOCATION, ");
                SQL.AppendLine("    S.IPADDRESS, S.FTPUSER, S.FTPPASSWD, S.LOCALPATH, ");
                SQL.AppendLine("    ( S.LOCALPATH || '/' || P.LOCATION ) AS SVRFILEPATH  ");
                SQL.AppendLine("FROM KOSMOS_EMR.EMR_PAGET P  ");
                SQL.AppendLine("INNER JOIN KOSMOS_EMR.EMR_DELETEPAGET C ");
                SQL.AppendLine("   ON P.PAGENO = C.PAGENO ");
                SQL.AppendLine("  AND C.PAGENO = " + VB.Val(mstrPageNo));
                SQL.AppendLine("  AND C.PAGE > 0 ");
                SQL.AppendLine("INNER JOIN KOSMOS_EMR.EMR_TREATT T ");
                SQL.AppendLine("   ON C.TREATNO = T.TREATNO ");
                SQL.AppendLine("INNER JOIN KOSMOS_EMR.EMR_PATHT S ");
                SQL.AppendLine("   ON P.PATHID = S.PATHID ");
                #endregion

                SqlErr = clsDB.GetDataTable(ref dt, SQL.ToString().Trim(), clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    return;
                }

                string strSVRFILEPATH = dt.Rows[i]["SVRFILEPATH"].ToString().Trim();
                string strEXTENSION = dt.Rows[i]["EXTENSION"].ToString().Trim();
                string strFTPUSER = dt.Rows[i]["FTPUSER"].ToString().Trim();
                string strFTPPASSWD = dt.Rows[i]["FTPPASSWD"].ToString().Trim();
                string strLOCALPATH = dt.Rows[i]["LOCALPATH"].ToString().Trim();

                string strSVRIP = dt.Rows[i]["IPADDRESS"].ToString().Trim();
                string strSVRID = dt.Rows[i]["FTPUSER"].ToString().Trim();
                string strSVRPW = dt.Rows[i]["FTPPASSWD"].ToString().Trim();
                string strFileNm = dt.Rows[i]["PAGENO"].ToString().Trim() + "." + dt.Rows[i]["EXTENSION"].ToString().Trim();
                string strSvrPath = dt.Rows[i]["SVRFILEPATH"].ToString().Trim();
                strSvrPath = strSvrPath.Replace("\\", "/");

                Cursor.Current = Cursors.Default;
                string strCurDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
                string mstrViewPathInit = @"C:\PSMHEXE\ScanTmp\Formname";
                string mstrViewPath = mstrViewPathInit + "\\" + strCurDate;

                #region FILE DOWNLOAD => VIEW
                using (Ftpedt FtpedtX = new Ftpedt())
                {
                    FtpedtX.FtpConBatchEx = FtpedtX.FtpConnetBatchEx(strSVRIP, strSVRID, strSVRPW);
                    if (FtpedtX.FtpConBatchEx == null)
                    {
                        FtpedtX.Dispose();
                        return;
                    }

                    bool blnDown = FtpedtX.FtpDownloadBatchEx(FtpedtX.FtpConBatchEx, mstrViewPath + "\\" + strFileNm, strFileNm, strSvrPath); //파일다운로드
                    if (blnDown)
                    {
                        //if(ltkPageView.Load(mstrViewPath + "\\" + strFileNm, 1))
                        //{
                        //    ltkPageView.BestFit();
                        //}
                        strViewFileName = mstrViewPath + "\\" + VB.Replace(strFileNm, "env", "tif");
                        if (VB.Right(strFileNm, 3) == "env")
                        {
                            clsCyper.Decrypt(mstrViewPath + "\\" + strFileNm, strViewFileName);
                        }
                        SetImaeViewEx(strViewFileName, picBig1);
                    }
                }


                #endregion

                dt.Dispose();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void SetImaeViewEx(string strImagePath, PictureBox picBig)
        {
            Bitmap BackImage;
            using (FileStream stream = new FileStream(strImagePath, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    using (var memoryStream = new MemoryStream(reader.ReadBytes((int)stream.Length)))
                    {
                        BackImage = new Bitmap(memoryStream);
                    }
                }
            }

            //double horzRatio = (double)((double)Width / (double)BackImage.Width);
            //double vertRatio = (double)((double)(Height - 60) / (double)BackImage.Height);
            //double ratio = (double)(horzRatio < vertRatio ? horzRatio : vertRatio);

            //picBig.Width = (int)(BackImage.Width * ratio);
            //picBig.Height = (int)(BackImage.Height * ratio) - 60;
    
            picBig.Image = (Image)BackImage;
            picBig.SizeMode = PictureBoxSizeMode.Zoom;
            picBig.Visible = true;
            picBig.BringToFront();
        }
        #endregion


        private void btnExit_Click(object sender, EventArgs e)
        {
            if(System.IO.File.Exists(strViewFileName))
            {
                System.IO.File.Delete(strViewFileName);
            }
            Close();
        }
    }
}
