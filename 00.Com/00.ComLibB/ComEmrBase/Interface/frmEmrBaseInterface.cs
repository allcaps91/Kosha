using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using ComBase;
using ComBase.Controls;
using ComDbB;
using Oracle.DataAccess.Client;

namespace ComEmrBase
{
    /// <summary>
    /// Class Name      : ComEmrBase
    /// File Name       : frmEmrBaseInterface.cs
    /// Description     : -
    /// Author          : 이현종
    /// Create Date     : 2020-06-24
    /// Update History  : 
    /// </summary>
    /// <history> 
    /// z:\vb60_new\ocs\ekg\imgaudio\imgaudio
    /// z:\vb60_new\ocs\ekg\imgaudio\imgabr
    /// z:\vb60_new\ocs\ekg\imgabi
    /// z:\vb60_new\ocs\ekg\imgbcm
    /// z:\vb60_new\ocs\ekg\imgtcd
    /// z:\vb60_new\ocs\ekg\imgauto
    /// z:\vb60_new\ocs\ekg\imgur
    /// z:\vb60_new\ocs\ekg\imgvng
    /// z:\vb60_new\Ocs\emg\emgauto
    /// 모든 기능검사 항목 폼을 합침.
    /// </history>
    /// <seealso cref= "\ocs\ekg\전체" />

    public partial class frmEmrBaseInterface : Form
    {
        #region 변수
        /// <summary>
        /// 제목 구분
        /// </summary>
        private string mstrTitle = string.Empty;

        /// <summary>
        /// 초 
        /// </summary>
        private int FnCnt = 0;

        /// <summary>
        /// 초2
        /// </summary>
        private int FnCnt2 = 0;

        /// <summary>
        /// 이미지 변환 경로
        /// </summary>
        private string InterfacePath = "C:\\PSMHEXE\\Interface";

        /// <summary>
        /// 저장용 
        /// </summary>
        private Bitmap mBitmap = null;


        #endregion

        #region 생ㅇ성자
        /// <summary>
        /// 이것만 사용
        /// </summary>
        /// <param name="strTitle"></param>
        public frmEmrBaseInterface(string strTitle)
        {
            mstrTitle = strTitle.ToUpper();
            InitializeComponent();
        }

        #endregion

        #region 폼 이벤트

        private void frmEmrBaseInterface_Load(object sender, EventArgs e)
        {
            if (Directory.Exists(InterfacePath) == false)
            {
                Directory.CreateDirectory(InterfacePath);
            }

            switch (mstrTitle.ToUpper())
            {
                case "AUDIO":
                    lblTitle.Text = "청력 결과 자동 영상 EMR 등록";
                    ChkEtc.Visible = true;
                    txtPano.Visible = true;
                    break;
                case "ABR":
                    lblTitle.Text = "ABR 자동 영상 EMR 등록";
                    txtPano.Visible = true;
                    break;
                case "BCM":
                    lblTitle.Text = "체수분 결과 자동 영상 EMR 등록";
                    break;
                case "TCD":
                    lblTitle.Text = mstrTitle + "결과 자동 영상 EMR 등록";
                    ChkEtc.Text = "사진4장";
                    ChkEtc.Visible = true;
                    break;
                case "VNG":
                    lblTitle.Text = mstrTitle + "결과 자동 영상 EMR 등록";
                    ChkEtc.Text = "사진3장";
                    ChkEtc.Visible = true;
                    break;
                case "ABI":
                    lblTitle.Text = mstrTitle + "결과 자동 영상 EMR 등록";
                    chkTO.Visible = true;
                    break;
                case "EMG":
                    panEMG.Visible = true;
                    panEMG2.Visible = true;
                    lblTitle.Text = mstrTitle + "결과 자동 영상 EMR 등록";
                    chkPostpay.Visible = true;
                    break;
                case "24HEKG":
                    lblTitle.Text = mstrTitle + "결과 자동 영상 EMR 등록";
                    break;
                case "HD":
                    lblTitle.Text = mstrTitle + "혈류 결과 자동 영상 EMR 등록";
                    break;
                default:
                    chkHic.Visible = mstrTitle.Equals("EKG");
                    chkHic.Checked = mstrTitle.Equals("EKG");
                    lblTitle.Text = mstrTitle + "결과 자동 영상 EMR 등록";
                    break;
            }

            cboMIN.Items.Clear();
            cboMIN.Items.Add("1초");
            cboMIN.Items.Add("2초");
            cboMIN.Items.Add("3초");
            cboMIN.Items.Add("4초");
            cboMIN.Items.Add("5초");
            cboMIN.Items.Add("6초");
            cboMIN.Items.Add("7초");
            cboMIN.Items.Add("8초");
            cboMIN.Items.Add("9초");
            cboMIN.Items.Add("10초");
            cboMIN.Items.Add("15초");
            cboMIN.Items.Add("20초");
            cboMIN.Items.Add("25초");
            cboMIN.Items.Add("30초");
            cboMIN.SelectedIndex = 9;

        }


        #endregion

        #region 버튼

        private void btnSave_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer1.Enabled = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region 타이머

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            FnCnt += 1;
            FnCnt2 += 1;

            DateTime dtpTime = new DateTime();

            if (FnCnt > VB.Val(VB.Pstr(cboMIN.Text, "초", 1)))
            {
                ComFunc.ReadSysDate(clsDB.DbCon);

                if (mstrTitle.Equals("EMG"))
                {
                    New_EMG_AUTO();
                }
                else
                {
                    New_Auto();

                    if (mstrTitle.Equals("UR"))
                    {
                        CHECK_EMR_TREATT();
                    }
                    else if (mstrTitle.Equals("EKG"))
                    {
                        ComFunc.Delay(500);
                        //New_EKG_AUTO_Port();
                        //ComFunc.Delay(1000);
                        New_EKG_Auto();
                    }
                }

                FnCnt = 0;

                dtpTime = ComQuery.CurrentDateTime(clsDB.DbCon, "S").To<DateTime>();
            }

            if (mstrTitle.Equals("VNG") && FnCnt2 > 200)
            {
                ComFunc.ReadSysDate(clsDB.DbCon);

                Consult_Miss();
                FnCnt2 = 0;
            }

            if (mstrTitle.Equals("VNG") && dtpTime.Hour == 2 && dtpTime.Minute >= 0 && dtpTime.Minute <= 1)
            {
                ComFunc.ReadSysDate(clsDB.DbCon);

                CREATE_골표지자();
            }

            lblCnt.Text = FnCnt.ToString();
            timer1.Start();
        }

        #endregion

        #region 함수

        private void New_EKG_AUTO_Port()
        {
            #region 변수
            string strPath = string.Empty;
            string strPathB = string.Empty;

            string strClinCode = string.Empty;
            string strDRCode = string.Empty;
            string strClass = string.Empty;
            string strTREATNO = string.Empty;
            string strSEQ = string.Empty;
            string strOutDate = string.Empty;
            string strNum = string.Empty;

            string strExamdate = string.Empty;

            string strFile = string.Empty;
            string strPtno = string.Empty;
            string strBdate = string.Empty;

            string strPathEMR = InterfacePath + "\\" + mstrTitle + "_IMG";

            string strRowid_JUPMST = string.Empty;

            //OracleDataReader reader = null;
            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            int PageNum = 0;
            string gstrFormcode = string.Empty;


            string strPathDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");

            string strflag = string.Empty;

            double nEKGCnt = 0;

            string strECGFile = string.Empty;
            #endregion

            try
            {
                strPath = "C:\\Ecgviewer\\AutoImage";//           '이미지 폴더
                strPathB = "C:\\Ecgviewer\\AutoImage\\AutoImage_backup";//   '이미지 폴더 백업폴더


                if (Directory.Exists(strPath) == false)
                {
                    Directory.CreateDirectory(strPath);
                }

                if (Directory.Exists(strPathB) == false)
                {
                    Directory.CreateDirectory(strPathB);
                }

                string[] strFiles = Directory.GetFiles(strPath);

                foreach (string file in strFiles)
                {
                    strFile = file.Substring(file.LastIndexOf("\\") + 1);
                    strNum = VB.Left(VB.Pstr(strFile.Trim(), "_", 1), 5);
                    strPtno = VB.Val(VB.Pstr(strFile.Trim(), "_", 2)).ToString("00000000");// '등록번호
                    strExamdate = VB.Left(VB.Pstr(strFile.Trim(), "_", 3), 8);// '처방일
                    strSEQ = VB.Pstr(strFile.Trim(), "_", 5);

                    if (VB.Pstr(strFile, "_", 4).Equals("O6"))
                    {
                        strPtno = "";
                    }


                    strClinCode = string.Empty;
                    strDRCode = string.Empty;
                    strClass = string.Empty;
                    strTREATNO = string.Empty;
                    strRowid_JUPMST = string.Empty;

                    if (strNum.ToUpper().Equals("WK")) //작업미완료 화일
                    {
                        strPtno = "";
                        strExamdate = "";
                    }

                    #region 각 항목에 맞는 환자정보 가져오기
                    if (string.IsNullOrWhiteSpace(strPtno) == false && string.IsNullOrWhiteSpace(strExamdate) == false)
                    {
                        strflag = "A";

                        #region 당일 2중 촬영이 있을경우 문제 발생
                        SQL = " SELECT COUNT(*) CNT ";
                        SQL += ComNum.VBLF + "      FROM KOSMOS_OCS.ETC_JUPMST ";
                        SQL += ComNum.VBLF + "     WHERE GBJOB ='3' ";// '접수
                        SQL += ComNum.VBLF + "       AND PTNO = '" + strPtno + "' ";
                        SQL += ComNum.VBLF + "       AND GUBUN ='1' ";// 'EKG
                        SQL += ComNum.VBLF + "       AND GbPort ='M' ";//  '포터블용
                        SQL += ComNum.VBLF + "       AND TRUNC(RDATE) =TO_DATE('" + strExamdate + "','YYYYMMDD') ";// '검사일자
                        SQL += ComNum.VBLF + "       AND BDATE <=TRUNC(SYSDATE)";

                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                        if (string.IsNullOrWhiteSpace(SqlErr) == false)
                        {
                            //ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            nEKGCnt = dt.Rows[0]["CNT"].To<double>();
                        }
                        dt.Dispose();
                        #endregion


                        #region 환자정보
                        SQL = " SELECT PTNO, TO_CHAR(BDATE,'YYYYMMDD') BDATE, DEPTCODE, DRCODE, GBIO , ROWID  ";
                        SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.ETC_JUPMST ";
                        SQL += ComNum.VBLF + " WHERE GBJOB IN ('1', '3')";//  '접수
                        SQL += ComNum.VBLF + "   AND PTNO   = '" + strPtno + "' ";
                        SQL += ComNum.VBLF + "   AND GUBUN  = '1' ";// 'EKG
                        SQL += ComNum.VBLF + "   AND GbPort = 'M' ";//  '포터블용
                        SQL += ComNum.VBLF + "   AND TRUNC(RDATE) =TO_DATE('" + strExamdate + "','YYYYMMDD') ";// '검사일자
                        SQL += ComNum.VBLF + "   AND BDATE <= TRUNC(SYSDATE)";
                        //SQL += ComNum.VBLF + "   AND (GBFTP IS NULL OR GBFTP <> 'Y')"; //2020-11-13 변경

                        //if (nEKGCnt > 1)
                        //{
                        //    SQL += ComNum.VBLF + "   AND IMAGE IS NULL";
                        //}

                        SQL += ComNum.VBLF + " ORDER BY  SENDDATE DESC ";

                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                        if (string.IsNullOrWhiteSpace(SqlErr) == false)
                        {
                            //ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            strRowid_JUPMST = dt.Rows[0]["ROWID"].ToString().Trim();
                            strClinCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                            strDRCode = dt.Rows[0]["DRCODE"].ToString().Trim();
                            strBdate = dt.Rows[0]["BDATE"].ToString().Trim();
                            strClass = dt.Rows[0]["GBIO"].ToString().Trim();
                        }
                        dt.Dispose();
                        #endregion

                        if (string.IsNullOrWhiteSpace(strRowid_JUPMST) == false && strSEQ.ToUpper().Equals("002.JPG"))
                        {
                            //'ecg view에서 사용할 파일 save
                            strECGFile = VB.Pstr(strFile, "_", 1) + ".ecg";
                            if (ECGFILE_FileToDB_FTP(strECGFile, strRowid_JUPMST, strNum, "X", strExamdate.Trim(), strPtno) == false)
                            {
                                continue;
                            }
                            strflag = "B";
                        }

                        #region 종검확인
                        if (string.IsNullOrWhiteSpace(strClinCode))
                        {
                            SQL = " SELECT   PTNO, TO_CHAR(SDATE,'YYYYMMDD') BDATE,  'TO' DEPTCODE, '99917' DRCODE,  'O' GBIO ";
                            SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.HEA_JEPSU ";
                            SQL += ComNum.VBLF + "WHERE PTNO ='" + strPtno + "' ";
                            SQL += ComNum.VBLF + "  AND SDATE =TO_DATE('" + strExamdate + "','YYYY-MM-DD') ";// '검사일자

                            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                            if (string.IsNullOrWhiteSpace(SqlErr) == false)
                            {
                                //ComFunc.MsgBoxEx(this, SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                return;
                            }

                            if (dt.Rows.Count > 0)
                            {
                                strClinCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                                strDRCode = dt.Rows[0]["DRCODE"].ToString().Trim();
                                if (strDRCode.Equals("1107") || strDRCode.Equals("1125"))
                                {
                                    strClinCode = "RA";
                                }
                                strBdate = dt.Rows[0]["BDATE"].ToString().Trim();
                                strClass = dt.Rows[0]["GBIO"].ToString().Trim();
                            }

                            dt.Dispose();
                        }
                        #endregion

                        #region 입원/외래 데이터 구분
                        SQL = " SELECT TREATNO, INDATE, OUTDATE  FROM KOSMOS_EMR.EMR_TREATT ";
                        SQL += ComNum.VBLF + " WHERE PATID    = '" + strPtno + "' ";
                        SQL += ComNum.VBLF + "   AND CLASS    = '" + strClass + "' ";
                        if (strClass.Equals("O"))
                        {
                            SQL += ComNum.VBLF + "   AND CLINCODE = '" + strClinCode + "' ";
                        }

                        if (strClass.Equals("I"))
                        {
                            SQL += ComNum.VBLF + "   AND INDATE   <= '" + strBdate + "' ";
                            SQL += ComNum.VBLF + " ORDER BY  INDATE DESC";
                        }
                        else
                        {
                            SQL += ComNum.VBLF + "   AND INDATE   = '" + strBdate + "' ";
                            SQL += ComNum.VBLF + " ORDER BY  INDATE ";
                        }

                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                        if (string.IsNullOrWhiteSpace(SqlErr) == false)
                        {
                            //ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            strTREATNO = dt.Rows[0]["TREATNO"].ToString().Trim();
                            strOutDate = dt.Rows[0]["OUTDATE"].ToString().Trim();
                            if (string.IsNullOrWhiteSpace(strOutDate))
                                strOutDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
                        }
                        dt.Dispose();
                        #endregion

                        #region ER 에서 전날 내원을 하고 다음날 EKG 검사해서 서버에 이미지파일 안넘어갈 경우 --20200212(김욱동)
                        if (mstrTitle.Equals("EKG") && strClass.Equals("O") && string.IsNullOrWhiteSpace(strTREATNO) && strClinCode.Equals("ER"))
                        {
                            strBdate = DateTime.ParseExact(strBdate, "yyyyMMdd", null).AddDays(-1).ToString("yyyyMMdd");

                            SQL = " SELECT TREATNO, INDATE, OUTDATE                             ";
                            SQL += ComNum.VBLF + "  FROM KOSMOS_EMR.EMR_TREATT                  ";
                            SQL += ComNum.VBLF + " WHERE PATID  ='" + strPtno + "'            ";
                            SQL += ComNum.VBLF + "   AND CLASS  = '" + strClass + "'            ";
                            SQL += ComNum.VBLF + "   AND INDATE = '" + strBdate + "'            ";
                            SQL += ComNum.VBLF + " ORDER BY INDATE                              ";

                            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                            if (string.IsNullOrWhiteSpace(SqlErr) == false)
                            {
                                //ComFunc.MsgBoxEx(this, SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                return;
                            }

                            if (dt.Rows.Count > 0)
                            {
                                strTREATNO = dt.Rows[0]["TREATNO"].ToString().Trim();
                                strOutDate = dt.Rows[0]["OUTDATE"].ToString().Trim();
                                if (string.IsNullOrWhiteSpace(strOutDate))
                                    strOutDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
                            }
                            dt.Dispose();
                        }
                        #endregion

                        #region EMR STRTREATNO 값 구하기
                        if (string.IsNullOrWhiteSpace(strTREATNO) == false && string.IsNullOrWhiteSpace(strRowid_JUPMST) == false)
                        {
                            if (Directory.Exists(strPathEMR) == false)
                            {
                                Directory.CreateDirectory(strPathEMR);
                                return;
                            }

                            foreach (string cvtFile in Directory.GetFiles(strPathEMR))
                            {
                                File.Delete(cvtFile);
                            }

                            #region UPLOAD 실시
                            if (strClass.Trim().Equals("I"))
                            {
                                gstrFormcode = "126"; //각종기능검사결과지
                            }
                            else
                            {
                                gstrFormcode = "008";
                            }

                            PageNum += 1;

                            mBitmap = new Bitmap(strPath + "\\" + strFile);
                            TifSave(strPathEMR + "\\" + strPtno + "_" + strBdate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                            string[] sdir = Directory.GetFiles(strPathEMR);
                            if (sdir.Length > 0 && strTREATNO.Equals("0") == false)
                            {
                                if (ADO_Upload(gstrFormcode, strTREATNO, strOutDate, sdir))
                                {
                                    File.Copy(file, strPathB + "\\" + strFile, true);
                                    File.Delete(file);
                                }
                            }

                            ss1_Sheet1.AddRows(0, 1);
                            ss1_Sheet1.RowCount = 500;

                            ss1_Sheet1.Cells[0, 0].Text = strExamdate;
                            ss1_Sheet1.Cells[0, 1].Text = strPtno;
                            ss1_Sheet1.Cells[0, 2].Text = strFile;
                            ss1_Sheet1.Cells[0, 3].Text = strTREATNO;

                            UpdateGBPort(strRowid_JUPMST, strPtno);

                            #endregion
                        }
                        else
                        {
                            ss1_Sheet1.AddRows(0, 1);
                            ss1_Sheet1.RowCount = 500;

                            ss1_Sheet1.Cells[0, 0].Text = strExamdate;
                            ss1_Sheet1.Cells[0, 1].Text = strPtno;
                            ss1_Sheet1.Cells[0, 2].Text = strFile;
                            ss1_Sheet1.Cells[0, 3].Text = strTREATNO;

                            ss1_Sheet1.Cells[0, 4].Text = "Error";
                        }
                        #endregion
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, "변환 도중 에러", clsDB.DbCon);
                ss1_Sheet1.AddRows(0, 1);
                ss1_Sheet1.RowCount = 500;

                ss1_Sheet1.Cells[0, 0].Text = strBdate;
                ss1_Sheet1.Cells[0, 1].Text = strPtno;
                ss1_Sheet1.Cells[0, 2].Text = strFile;
                ss1_Sheet1.Cells[0, 3].Text = strTREATNO;

                ss1_Sheet1.Cells[0, 4].Text = ex.Message;
            }
        }

        private void Consult_Miss()
        {
            string strPTNO = string.Empty;
            string strORDERNO = string.Empty;
            string strDRCode = string.Empty;
            string strDRSABUN = string.Empty;
            string strDeptCode = string.Empty;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int RowAffected = 0;
            DataTable dt = null;
            DataTable dt2 = null;

            //'이비인후과, 안과 컨설트 수가 미발생 처리 쿼리

            //'1) 대상자 조회
            SQL = " SELECT A.PTNO, A.BDATE, A.ORDERNO, A.DEPTCODE, A.SUCODE, A.ENTDATE, A.PICKUPDATE";
            SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_IORDER A, KOSMOS_PMPA.IPD_NEW_MASTER M";
            SQL += ComNum.VBLF + " WHERE BDATE >= TRUNC(SYSDATE - 30)";
            SQL += ComNum.VBLF + "   AND EXISTS (";
            SQL += ComNum.VBLF + "      SELECT ORDERNO";
            SQL += ComNum.VBLF + "        FROM KOSMOS_OCS.OCS_ITRANSFER B";
            SQL += ComNum.VBLF + "       WHERE BDATE >= TRUNC(SYSDATE - 30)";
            SQL += ComNum.VBLF + "         AND INPDATE IS NOT NULL";
            SQL += ComNum.VBLF + "         AND A.PTNO = B.PTNO";
            SQL += ComNum.VBLF + "         AND A.ORDERNO = B.ORDERNO";
            SQL += ComNum.VBLF + "         AND A.BDATE = B.BDATE)";
            SQL += ComNum.VBLF + "   AND A.DRCODE2 IS NULL";
            SQL += ComNum.VBLF + "   AND A.SUCODE IN ('C-EN','C-OT')";
            SQL += ComNum.VBLF + "   AND A.PTNO = M.PANO";
            SQL += ComNum.VBLF + "   AND M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD')";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
            if (!string.IsNullOrWhiteSpace(SqlErr))
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strPTNO = dt.Rows[i]["PTNO"].ToString().Trim();
                    strORDERNO = dt.Rows[i]["ORDERNO"].ToString().Trim();
                    strDeptCode = dt.Rows[i]["DEPTCODE"].ToString().Trim();

                    #region 3) 컨설트 내역 작성자 및 작성여부 확인
                    strDRSABUN = "";
                    SQL = " SELECT A.INPDATE, A.INPID, A.TOREMARK";
                    SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_ITRANSFER A";
                    SQL += ComNum.VBLF + " WHERE PTNO = '" + strPTNO + "'";
                    SQL += ComNum.VBLF + "   AND ORDERNO = " + strORDERNO;

                    SqlErr = clsDB.GetDataTableREx(ref dt2, SQL, clsDB.DbCon);
                    if (!string.IsNullOrWhiteSpace(SqlErr))
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }

                    if (dt2.Rows.Count > 0)
                    {
                        strDRSABUN = dt2.Rows[0]["INPID"].ToString().Trim();
                    }
                    dt2.Dispose();


                    strDRCode = "";
                    if (!string.IsNullOrWhiteSpace(strDRSABUN))
                    {
                        SQL = " SELECT DRCODE";
                        SQL += ComNum.VBLF + " FROM KOSMOS_OCS.OCS_DOCTOR ";
                        SQL += ComNum.VBLF + " WHERE SABUN = " + strDRSABUN;

                        SqlErr = clsDB.GetDataTableREx(ref dt2, SQL, clsDB.DbCon);
                        if (!string.IsNullOrWhiteSpace(SqlErr))
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }

                        if (dt2.Rows.Count > 0)
                        {
                            strDRCode = dt2.Rows[0]["DRCODE"].ToString().Trim();
                        }
                        dt2.Dispose();
                    }


                    if (!string.IsNullOrWhiteSpace(strDRCode) && !string.IsNullOrWhiteSpace(strDRSABUN) &&
                        !string.IsNullOrWhiteSpace(strPTNO) && !string.IsNullOrWhiteSpace(strORDERNO))
                    {
                        SQL = " UPDATE KOSMOS_OCS.OCS_IORDER SET";
                        //'''SQL = SQL & vbCr & " DRCODE2 = '" & strDRCode & "', GBSEND = ' * ' "
                        //'''SQL = SQL & vbCr & " DRCODE2 = '" & strDRCode & "', GBSEND = ' * ' , opdno =1 " '2018 - 11 - 01 변경후 건수 없음 - 정상작동중인듯
                        SQL += ComNum.VBLF + " opdno = 1 ";//  '2018-11-06 구분자만 일단 체크함
                        SQL += ComNum.VBLF + " WHERE PTNO = '" + strPTNO + "'";
                        SQL += ComNum.VBLF + "   AND ORDERNO = " + strORDERNO;

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                        if (!string.IsNullOrWhiteSpace(SqlErr))
                        {
                            //ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                            return;
                        }
                    }

                    #endregion
                }
            }


            dt.Dispose();
        }

        private void CREATE_골표지자()
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            int RowAffected = 0;

            if (clsDB.DbCon.Trs == null)
            {
                clsDB.setBeginTran(clsDB.DbCon);
            }

            try
            {

                SQL = " SELECT PTNO";
                SQL += ComNum.VBLF + " FROM KOSMOS_OCS.OCS_Osteocalcin ";
                //SQL += ComNum.VBLF + " WHERE BUILDDATE = TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + " WHERE BUILDDATE = TRUNC(SYSDATE) ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    dt.Dispose();
                    return;
                }

                dt.Dispose();

                DateTime dtpSys = ComQuery.CurrentDateTime(clsDB.DbCon, "S").To<DateTime>();

                SQL = " INSERT INTO KOSMOS_OCS.OCS_Osteocalcin(PTNO, BDATE, SUCODE, QTY, NAL, BUILDDATE, GBIO)";
                SQL += ComNum.VBLF + " SELECT PANO, BDATE, SUCODE, QTY, NAL, TO_DATE('" + dtpSys.ToString("yyyyMMdd") + "','YYYY-MM-DD'), 'O'";
                SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_SLIP";
                SQL += ComNum.VBLF + " WHERE BDATE = TO_DATE('" + dtpSys.AddDays(-1).ToString("yyyyMMdd") + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "   AND SUCODE IN ('C3630', 'C3932')";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    //ComFunc.MsgBoxEx(this, SqlErr);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, "CREATE_골표지자 에러", clsDB.DbCon);
                clsDB.setRollbackTran(clsDB.DbCon);
            }
        }

        #region New_Auto
        /// <summary>
        /// 변환 후 FTP 업로드
        /// </summary>
        private void New_Auto()
        {
            #region 변수
            string strPath = string.Empty;
            string strPathB = string.Empty;

            string strClinCode = string.Empty;
            string strDRCode = string.Empty;
            string strClass = string.Empty;
            string strTREATNO = string.Empty;
            string strSEQ = string.Empty;
            string strOutDate = string.Empty;
            string strNum = string.Empty;

            string strExamdate = string.Empty;

            string strFile = string.Empty;
            string strPtno = string.Empty;
            string strPtno2 = string.Empty;
            string strBdate = string.Empty;

            string strPathEMR = InterfacePath + "\\" + mstrTitle + "_IMG";

            string strRowid_JUPMST = string.Empty;

            //OracleDataReader reader = null;
            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            int PageNum = 0;
            string gstrFormcode = string.Empty;

            double nWRTNO = 0;

            string strPathDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");

            string strflag = string.Empty;

            double nEKGCnt = 0;

            string strECGFile = string.Empty;

            //21-03-05 ER EKG 입원에 붙어서 구분용도로 추가.
            string strRoomCode = string.Empty;
            #endregion

            try
            {
                if (mstrTitle.Equals("AUDIO"))
                {
                    strPath = "C:\\image";//           '이미지 폴더
                    strPathB = "C:\\image\\image_backup" + "\\" + strPathDate;//      '백업 폴더
                }
                else if (mstrTitle.Equals("ABR"))
                {
                    strPath = "D:\\" + mstrTitle + "_image";//           '이미지 폴더
                    strPathB = "D:\\" + mstrTitle + "_image_backup" + "\\" + strPathDate;//   '이미지 폴더 백업폴더
                }
                else if (mstrTitle.Equals("EKG"))
                {
                    if (clsType.User.JobGroup.Equals("JOB006002") || clsType.User.JobGroup.Equals("JOB026007") || clsType.User.JobGroup.Equals("JOB026011") || clsCompuInfo.gstrCOMIP.Equals("123.123.123.2"))
                    {
                        strPath = "C:\\EKG_image";// '이미지 폴더
                        strPathB = "C:\\EKG_Image\\backup_image";// '백업 폴더
                    }
                    else
                    {
                        strPath = "C:\\Ecgviewer\\AutoImage";//           '이미지 폴더
                        strPathB = "C:\\Ecgviewer\\AutoImage\\AutoImage_backup";//   '이미지 폴더 백업폴더
                    }
                }
                else// if (mstrTitle.Equals("ABI") || mstrTitle.Equals("TCD"))
                {
                    strPath = "D:\\" + mstrTitle + "_image";//           '이미지 폴더
                    strPathB = "D:\\" + mstrTitle + "_image_backup";//   '이미지 폴더 백업폴더
                }

                if (Directory.Exists(strPath) == false)
                {
                    Directory.CreateDirectory(strPath);
                }

                if (Directory.Exists(strPathB) == false)
                {
                    Directory.CreateDirectory(strPathB);
                }

                string[] strFiles = Directory.GetFiles(strPath);

                string Extension = string.Empty;
                foreach (string file in strFiles)
                {
                    FileInfo fileInfo = new FileInfo(file);

                    //strFile = // file.Substring(file.LastIndexOf("\\") + 1);
                    strFile = fileInfo.Name;
                    Extension = fileInfo.Extension;
                    strRowid_JUPMST = string.Empty;

                    if (Extension.Equals(".tmp"))
                    {
                        continue;
                    }

                    if (!mstrTitle.Equals("EKG"))
                    {
                        strPtno = VB.Val(VB.Pstr(strFile.Trim(), "_", 1)).ToString("00000000");// '등록번호
                        strBdate = VB.Left(VB.Pstr(strFile.Trim(), "_", 2), 8);// '처방일
                    }
                    else
                    {
                        if (clsType.User.JobGroup.Equals("JOB006002") || clsType.User.JobGroup.Equals("JOB026007") || clsType.User.JobGroup.Equals("JOB026011") || clsCompuInfo.gstrCOMIP.Equals("123.123.123.2"))
                        {
                            strPtno = VB.Val(VB.Pstr(strFile.Trim(), "_", 1)).ToString("00000000");// '등록번호
                            strPtno2 = VB.Val(VB.Pstr(strFile.Trim(), "_", 1)).ToString("00000000");// '등록번호
                            nWRTNO = VB.Val(VB.Pstr(strFile.Trim(), "_", 1));
                            strExamdate = VB.Left(VB.Pstr(strFile.Trim(), "_", 2), 8);// '처방일
                        }
                        else
                        {
                            strNum = VB.Left(VB.Pstr(strFile.Trim(), "_", 1), 5);
                            strPtno = VB.Val(VB.Pstr(strFile.Trim(), "_", 2)).ToString("00000000");// '등록번호
                            strPtno2 = VB.Val(VB.Pstr(strFile.Trim(), "_", 2)).ToString("00000000");// '등록번호
                            nWRTNO = VB.Val(VB.Pstr(strFile.Trim(), "_", 2));
                            strExamdate = VB.Left(VB.Pstr(strFile.Trim(), "_", 3), 8);// '처방일
                            strSEQ = VB.Pstr(strFile.Trim(), "_", 5);
                        }

                        strBdate = "";
                        if (VB.Pstr(strFile, "_", 4).Trim().Equals("O6"))
                        {
                            strPtno = "";
                        }
                    }


                    #region 2018-05-01 날짜가 정확하지 않을 경우 루틴 타지 않음
                    if (mstrTitle.Equals("EKG"))
                    {
                        if (!VB.IsDate(VB.Left(strExamdate, 4) + "-" + VB.Mid(strExamdate, 5, 2) + "-" + VB.Right(strExamdate, 2)))
                            break;
                    }
                    else
                    {
                        if (!VB.IsDate(VB.Left(strBdate, 4) + "-" + VB.Mid(strBdate, 5, 2) + "-" + VB.Right(strBdate, 2)))
                            break;
                    }

                    #endregion

                    if (mstrTitle.Equals("TCD"))
                    {
                        strSEQ = VB.Right(VB.Pstr(strFile.Trim(), ".", 1), 1);// '파일번호
                    }
                    else if (mstrTitle.Equals("ABI"))
                    {
                        strSEQ = VB.Left(VB.Right(VB.Pstr(strFile.Trim(), ".", 1), 2), 2);// '파일번호
                    }
                    else if (mstrTitle.Equals("VNG"))
                    {
                        strSEQ = VB.Left(VB.Right(VB.Pstr(strFile.Trim(), ".", 1), 2), 2);// '파일번호
                    }

                    strClinCode = string.Empty;
                    strDRCode = string.Empty;
                    strClass = string.Empty;
                    strTREATNO = string.Empty;

                    if (strNum.ToUpper().Equals("WK")) //작업미완료 화일
                    {
                        strPtno = "";
                        strExamdate = "";
                    }

                    if (strPtno.IsNullOrEmpty() || VB.Val(strPtno).ToString().Equals("00000000"))
                    {
                        continue;
                    }

                    #region 각 항목에 맞는 환자정보 가져오기
                    if (string.IsNullOrWhiteSpace(strPtno) == false && string.IsNullOrWhiteSpace(mstrTitle.Equals("EKG") ? strExamdate : strBdate) == false)
                    {
                        ComFunc.ReadSysDate(clsDB.DbCon);

                        #region EKG
                        if (mstrTitle.Equals("EKG"))
                        {
                            if (chkHic.Checked)
                            {
                                //'건진접수 번호 체크
                                SQL = " SELECT PTNO FROM KOSMOS_PMPA.HIC_JEPSU ";
                                SQL += ComNum.VBLF + " WHERE WRTNO =" + nWRTNO + "  ";
                                SQL += ComNum.VBLF + "  AND JEPDATE =TO_DATE('" + strExamdate + "','YYYYMMDD') ";

                                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                                {
                                    //ComFunc.MsgBoxEx(this, SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                    return;
                                }

                                if (dt.Rows.Count > 0)
                                {
                                    strPtno = dt.Rows[0]["PTNO"].ToString().Trim();
                                }
                                dt.Dispose();
                            }

                            strflag = "A";

                            #region 당일 2중 촬영이 있을경우 문제 발생
                            SQL = " SELECT COUNT(*) CNT ";
                            SQL += ComNum.VBLF + "      FROM KOSMOS_OCS.ETC_JUPMST ";
                            //SQL += ComNum.VBLF + "     WHERE GBJOB = '3' ";// '접수
                            SQL += ComNum.VBLF + "     WHERE PTNO  = '" + strPtno + "' ";
                            SQL += ComNum.VBLF + "       AND GUBUN = '1' ";// 'EKG
                            SQL += ComNum.VBLF + "       AND ORDERCODE IN('E6541', '01030110') ";

                            if (VB.Mid(strExamdate, 7, 2).Equals("01"))
                            {
                                SQL += ComNum.VBLF + "       AND (DEPTCODE = 'ER' AND SUBSTR(BDATE, 1, 10) <> SUBSTR(RDATE, 1, 10) AND TRUNC(RDATE) >= TO_DATE('" + strExamdate + "','YYYYMMDD') ";
                            }
                            else
                            {
                                SQL += ComNum.VBLF + "       AND (DEPTCODE = 'ER' AND SUBSTR(BDATE, 1, 10) <> SUBSTR(RDATE, 1, 10) AND TRUNC(RDATE) >= TO_DATE('" + (VB.Val(strExamdate) - 1) + "','YYYYMMDD') ";
                            }

                            SQL += ComNum.VBLF + "        OR  TRUNC(RDATE) = TO_DATE('" + strExamdate + "','YYYYMMDD')) ";// '검사일자
                            SQL += ComNum.VBLF + "       AND BDATE <=TRUNC(SYSDATE)";

                            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                            if (string.IsNullOrWhiteSpace(SqlErr) == false)
                            {
                                //ComFunc.MsgBoxEx(this, SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                return;
                            }

                            if (dt.Rows.Count > 0)
                            {
                                nEKGCnt = dt.Rows[0]["CNT"].To<double>();
                            }
                            dt.Dispose();
                            #endregion
                        }
                        #endregion

                        #region 환자정보
                        SQL = " SELECT PTNO, TO_CHAR(BDATE,'YYYYMMDD') BDATE, DEPTCODE, DRCODE, GBIO , ROWID, ROOMCODE,(CASE WHEN DEPTCODE <> 'HR' THEN '1' ELSE '0' END) AS EXCEPSORTCODE  ";
                        SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.ETC_JUPMST ";
                        if (mstrTitle.Equals("UR") || mstrTitle.Equals("HD"))
                        {
                            SQL += ComNum.VBLF + " WHERE GBJOB IN ('1','2') ";//  '접수,예약
                        }
                        else
                        {
                            if (mstrTitle.Equals("EKG"))
                            {
                                SQL += ComNum.VBLF + " WHERE CASE WHEN GBPORT = 'M' AND GBJOB IN ('1', '3') THEN 1                  ";//  '접수
                                SQL += ComNum.VBLF + "            WHEN GBJOB  = '3' THEN 1                                          ";//  '접수
                                SQL += ComNum.VBLF + "       END = 1                                                                ";//  '접수
                            }
                            else
                            {
                                SQL += ComNum.VBLF + " WHERE GBJOB = '3' ";//  '접수

                            }
                        }
                        SQL += ComNum.VBLF + "   AND PTNO = '" + strPtno + "' ";

                        #region 각 항목 쿼리 구분
                        if (mstrTitle.Equals("AUDIO"))
                        {
                            SQL += ComNum.VBLF + "   AND GUBUN = '6' ";// '청력 ABR
                            SQL += ComNum.VBLF + "   AND TRUNC(RDATE) = TO_DATE('" + strBdate + "','YYYY-MM-DD') ";// '검사일자
                            SQL += ComNum.VBLF + "   AND DEPTCODE <>'HR' ";

                            string strBun = VB.Mid(VB.Pstr(strFile.Trim(), "_", 2), 9, 1);// '구분

                            if (ChkEtc.Checked && txtPano.TextLength == 8)
                            {
                                SQL += ComNum.VBLF + "   AND ORDERCODE = 'E6910'";
                                SQL += ComNum.VBLF + "   AND BUN ='47'"; //청력 검사
                            }
                            else
                            {
                                if (strBun.Equals("2"))
                                {
                                    SQL += ComNum.VBLF + " AND ORDERCODE = 'E6941' ";// ' 임피단스 오디오메트리
                                }
                                else
                                {
                                    //'2020-02-03, 'F0341', 'F0342', 'F0343', 'F0344' 추가
                                    SQL += ComNum.VBLF + "  AND ORDERCODE IN ( 'E6931', 'E6910', 'F0341', 'F0342', 'F0343', 'F0344') ";//  '표준순음청력검사, 언어청각검사
                                }
                                SQL += ComNum.VBLF + "    AND BUN ='50' ";// '청력 검사
                            }
                        }
                        else if (mstrTitle.Equals("ABR"))
                        {
                            SQL += ComNum.VBLF + "   AND GUBUN = '6' ";// '청력 ABR
                            SQL += ComNum.VBLF + "   AND BDATE = TO_DATE('" + strBdate + "','YYYY-MM-DD') ";// '처방일자
                            SQL += ComNum.VBLF + "   AND DEPTCODE <>'HR' ";
                            SQL += ComNum.VBLF + "   AND ORDERCODE = 'F6400'";
                            SQL += ComNum.VBLF + "   AND BUN ='50'"; //청력 검사
                        }
                        else if (mstrTitle.Equals("HD"))
                        {
                            SQL += ComNum.VBLF + "   AND GUBUN = '24' ";// '청력 ABR
                            SQL += ComNum.VBLF + "   AND BDATE <= TRUNC(SYSDATE) ";// '처방일자
                            SQL += ComNum.VBLF + "   AND TRUNC(RDATE) = TO_DATE('" + strBdate + "','YYYYMMDD') ";
                        }
                        else if (mstrTitle.Equals("24HEKG"))
                        {
                            SQL += ComNum.VBLF + "   AND GUBUN = '10' ";// '24HEKG
                            SQL += ComNum.VBLF + "   AND BDATE = TO_DATE('" + strBdate + "','YYYY-MM-DD') ";// '처방일자
                            SQL += ComNum.VBLF + "   AND DEPTCODE <>'HR' ";
                            SQL += ComNum.VBLF + "   AND ORDERCODE = 'E6545'";
                            SQL += ComNum.VBLF + "   AND BUN ='44'"; //24HEKG 검사
                        }
                        else if (mstrTitle.Equals("ABI"))
                        {
                            SQL += ComNum.VBLF + "   AND GUBUN = '7' ";// 'ABI
                            SQL += ComNum.VBLF + "   AND ORDERCODE IN ('ABI-A','ABI') ";// 'ABI-A:외래, ABI:종검
                            SQL += ComNum.VBLF + "   AND TRUNC(RDATE) = TO_DATE('" + strBdate + "','YYYYMMDD') ";
                            SQL += ComNum.VBLF + "   AND BDATE <= TRUNC(SYSDATE)";
                        }
                        else if (mstrTitle.Equals("BCM"))
                        {
                            SQL += ComNum.VBLF + "   AND GUBUN = '23' ";// 'BCM
                            SQL += ComNum.VBLF + "   AND ORDERCODE = 'BCM'";//  '3개오더중 1개만  2014-11-01 배다혜선생 통화후
                            SQL += ComNum.VBLF + "   AND TRUNC(RDATE) = TO_DATE('" + strBdate + "','YYYYMMDD') ";
                            SQL += ComNum.VBLF + "   AND BDATE <= TRUNC(SYSDATE)";
                            SQL += ComNum.VBLF + "   AND DEPTCODE <>'HR' ";
                        }
                        else if (mstrTitle.Equals("TCD"))
                        {
                            SQL += ComNum.VBLF + "   AND GUBUN = '12' ";// 'TCD
                            SQL += ComNum.VBLF + "   AND TRUNC(RDATE) = TO_DATE('" + strBdate + "','YYYYMMDD') ";
                            SQL += ComNum.VBLF + "   AND BDATE <= TRUNC(SYSDATE)";
                        }
                        else if (mstrTitle.Equals("UR"))
                        {
                            SQL += ComNum.VBLF + "   AND GUBUN = '25' ";// 'UR
                            SQL += ComNum.VBLF + "   AND TRUNC(BDATE) = TO_DATE('" + strBdate + "','YYYYMMDD') ";
                            SQL += ComNum.VBLF + "   AND BDATE <= TRUNC(SYSDATE)";
                        }
                        else if (mstrTitle.Equals("VNG"))
                        {
                            SQL += ComNum.VBLF + "   AND GUBUN = '14' ";// 'VNG
                            SQL += ComNum.VBLF + "   AND ORDERCODE = 'F6332'";//  '3개오더중 1개만  2014-11-01 배다혜선생 통화후
                            SQL += ComNum.VBLF + "   AND TRUNC(RDATE) = TO_DATE('" + strBdate + "','YYYYMMDD') ";
                            SQL += ComNum.VBLF + "   AND BDATE <= TRUNC(SYSDATE)";
                            SQL += ComNum.VBLF + "   AND DEPTCODE <>'HR' ";
                        }
                        else if (mstrTitle.Equals("EKG"))
                        {
                            SQL += ComNum.VBLF + "       AND GUBUN = '1' ";// 'EKG
                            SQL += ComNum.VBLF + "       AND ORDERCODE IN('E6541', '01030110') ";

                            //'2020-01-31, ER에서 전날 접수 된 환자로 익일 검사 EMR 전송 안되는문제로 조건 변경함
                            //'SQL = SQL & "       AND TRUNC(RDATE) =TO_DATE('" & strExamdate & "','YYYYMMDD') " '검사일자

                            if (VB.Mid(strExamdate, 7, 2).Equals("01"))
                            {
                                SQL += ComNum.VBLF + "       AND (DEPTCODE = 'ER' AND SUBSTR(BDATE, 1, 10) <> SUBSTR(RDATE, 1, 10) AND TRUNC(RDATE) >= TO_DATE('" + strExamdate + "','YYYYMMDD') ";
                            }
                            else
                            {
                                SQL += ComNum.VBLF + "       AND (DEPTCODE = 'ER' AND SUBSTR(BDATE, 1, 10) <> SUBSTR(RDATE, 1, 10) AND TRUNC(RDATE) >= TO_DATE('" + (VB.Val(strExamdate) - 1) + "','YYYYMMDD') ";
                            }


                            SQL += ComNum.VBLF + "        OR  TRUNC(RDATE) = TO_DATE('" + strExamdate + "','YYYYMMDD')) ";// '검사일자
                            SQL += ComNum.VBLF + "       AND BDATE <=TRUNC(SYSDATE)";
                            //SQL += ComNum.VBLF + "       AND CASE WHEN DEPTCODE IN('HR', 'TO') THEN 1 ";  //2020-11-17 변경
                            //SQL += ComNum.VBLF + "                WHEN DEPTCODE NOT IN('HR', 'TO') AND (GBFTP IS NULL OR GBFTP <> 'Y') THEN 1 ";  //2020-11-17 변경
                            //SQL += ComNum.VBLF + "           END = 1";  //2020-11-17 변경

                            //SQL += ComNum.VBLF + "       AND (GbFTP IS NULL OR GbFTP <> 'Y') ";  //2020-11-13 변경
                            //if (nEKGCnt > 1)
                            //{
                            //    if (chkNo_DB.Checked)
                            //    {
                            //        SQL += ComNum.VBLF + " AND (GbFTP IS NULL OR GbFTP <> 'Y') ";
                            //    }
                            //    else
                            //    {
                            //        SQL += ComNum.VBLF + " AND IMAGE IS NULL";
                            //    }
                            //}

                            SQL += ComNum.VBLF + "  ORDER BY  EXCEPSORTCODE DESC, SENDDATE DESC ";

                        }
                        #endregion


                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                        if (string.IsNullOrWhiteSpace(SqlErr) == false)
                        {
                            //ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            strRowid_JUPMST = dt.Rows[0]["ROWID"].ToString().Trim();
                            strClinCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                            strDRCode = dt.Rows[0]["DRCODE"].ToString().Trim();
                            strBdate = dt.Rows[0]["BDATE"].ToString().Trim();
                            strClass = dt.Rows[0]["GBIO"].ToString().Trim();
                        }
                        else if (dt.Rows.Count == 0 && mstrTitle.Equals("EKG"))
                        {
                            dt.Dispose();

                            #region 당일 2중 촬영이 있을경우 문제 발생
                            SQL = " SELECT COUNT(*) CNT ";
                            SQL += ComNum.VBLF + "      FROM KOSMOS_OCS.ETC_JUPMST ";
                            //SQL += ComNum.VBLF + "     WHERE GBJOB ='3' ";// '접수
                            SQL += ComNum.VBLF + "     WHERE PTNO = '" + strPtno + "' ";
                            SQL += ComNum.VBLF + "       AND GUBUN ='1' ";// 'EKG
                            SQL += ComNum.VBLF + "       AND DEPTCODE = 'ER' ";// '검사일자
                            SQL += ComNum.VBLF + "       AND ORDERCODE IN('E6541', '01030110') ";

                            if (VB.Mid(strExamdate, 7, 2).Equals("01"))
                            {
                                SQL += ComNum.VBLF + "       AND TRUNC(RDATE) = TO_DATE('" + strExamdate + "','YYYYMMDD') ";
                            }
                            else
                            {
                                SQL += ComNum.VBLF + "       AND TRUNC(RDATE) = TO_DATE('" + (VB.Val(strExamdate) - 1) + "','YYYYMMDD') ";
                            }

                            SQL += ComNum.VBLF + "       AND BDATE <=TRUNC(SYSDATE)";
                            SQL += ComNum.VBLF + "       AND (GbFTP IS NULL OR GbFTP <> 'Y') ";

                            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                            if (string.IsNullOrWhiteSpace(SqlErr) == false)
                            {
                                //ComFunc.MsgBoxEx(this, SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                return;
                            }

                            if (dt.Rows.Count > 0)
                            {
                                nEKGCnt = dt.Rows[0]["CNT"].To<double>();
                            }
                            dt.Dispose();

                            //----------------------------------------------------------
                            SQL = " SELECT PTNO, TO_CHAR(BDATE,'YYYYMMDD') BDATE, DEPTCODE, DRCODE, GBIO , ROWID  ";
                            SQL += ComNum.VBLF + "      FROM KOSMOS_OCS.ETC_JUPMST ";
                            //SQL += ComNum.VBLF + "     WHERE GBJOB = '3' ";// '접수
                            SQL += ComNum.VBLF + "     WHERE PTNO  = '" + strPtno + "' ";
                            SQL += ComNum.VBLF + "       AND GUBUN = '1' ";// 'EKG
                            SQL += ComNum.VBLF + "       AND DEPTCODE = 'ER' ";// '검사일자
                            SQL += ComNum.VBLF + "       AND ORDERCODE IN('E6541', '01030110') ";

                            if (VB.Mid(strExamdate, 7, 2).Equals("01"))
                            {
                                SQL += ComNum.VBLF + "       AND TRUNC(RDATE) = TO_DATE('" + strExamdate + "','YYYYMMDD') ";
                            }
                            else
                            {
                                SQL += ComNum.VBLF + "       AND TRUNC(RDATE) = TO_DATE('" + (VB.Val(strExamdate) - 1) + "','YYYYMMDD') ";
                            }

                            SQL += ComNum.VBLF + "       AND BDATE <=TRUNC(SYSDATE)";
                            SQL += ComNum.VBLF + "       AND (GbFTP IS NULL OR GbFTP <> 'Y') ";
                            SQL += ComNum.VBLF + "ORDER BY  SENDDATE ASC ";

                            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                            if (string.IsNullOrWhiteSpace(SqlErr) == false)
                            {
                                //ComFunc.MsgBoxEx(this, SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                return;
                            }

                            if (dt.Rows.Count > 0)
                            {
                                strRowid_JUPMST = dt.Rows[0]["ROWID"].ToString().Trim();
                                strClinCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                                strDRCode = dt.Rows[0]["DRCODE"].ToString().Trim();
                                strBdate = dt.Rows[0]["BDATE"].ToString().Trim();
                                strClass = dt.Rows[0]["GBIO"].ToString().Trim();

                                #region 21-03-05 ER 오더가 ETC_JUPMST에 입원오더로 표시되서 ER EKG가 입원에 붙는 현상 때문에 추가함.
                                if (mstrTitle.Equals("EKG") && strRoomCode.Equals("100"))
                                {
                                    strClass = "O";
                                    strClinCode = "ER";
                                }
                                #endregion
                            }
                            #endregion
                        }
                        dt.Dispose();
                        #endregion

                        if (string.IsNullOrWhiteSpace(strRowid_JUPMST) == false)
                        {
                            int RowAffected = 0;
                            if (mstrTitle.Equals("AUDIO") || mstrTitle.Equals("ABR"))
                            {
                                //timer1.Stop();
                                AUDIOFILE_FileToDB_FTP(strPath + "\\" + strFile, strRowid_JUPMST, strPathB + "\\" + strFile, "X", strBdate.Trim(), strFile);
                                //timer1.Start();
                            }
                            else if (mstrTitle.Equals("BCM"))
                            {
                                //timer1.Stop();
                                BCM_JPGFILE_FileToDB_FTP(strPath + "\\" + strFile, strRowid_JUPMST, strPathB + "\\" + strFile, strBdate.Trim(), strFile);//  '2014-11-18
                                //timer1.Start();
                            }
                            else if (mstrTitle.Equals("HD"))
                            {
                                //timer1.Stop();
                                HD_JPGFILE_FileToDB_FTP(strPath + "\\" + strFile, strRowid_JUPMST, strPathB + "\\" + strFile, strBdate.Trim(), strFile);//  '2014-11-18
                                //timer1.Start();
                            }
                            else if (mstrTitle.Equals("TCD"))
                            {
                                if (ChkEtc.Checked && VB.Val(strSEQ) >= 4 || ChkEtc.Checked == false && VB.Val(strSEQ) >= 3)
                                {
                                    if (clsDB.DbCon.Trs == null)
                                    {
                                        clsDB.setBeginTran(clsDB.DbCon);
                                    }

                                    try
                                    {
                                        SQL = " UPDATE KOSMOS_OCS.ETC_JUPMST SET  ";
                                        SQL += ComNum.VBLF + " Image_Gbn = '07'";
                                        SQL += ComNum.VBLF + " WHERE ROWID = '" + strRowid_JUPMST + "' ";

                                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                                        if (!string.IsNullOrWhiteSpace(SqlErr))
                                        {
                                            //ComFunc.MsgBoxEx(this, SqlErr);
                                            clsDB.setRollbackTran(clsDB.DbCon);
                                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                            return;
                                        }

                                        clsDB.setCommitTran(clsDB.DbCon);
                                    }
                                    catch
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                    }

                                }
                            }
                            else if (mstrTitle.Equals("UR"))
                            {
                                if (clsDB.DbCon.Trs == null)
                                {
                                    clsDB.setBeginTran(clsDB.DbCon);
                                }

                                try
                                {
                                    SQL = " UPDATE KOSMOS_OCS.ETC_JUPMST SET  ";
                                    SQL += ComNum.VBLF + " Image_Gbn = '11',";
                                    SQL += ComNum.VBLF + " GBJOB = '3' ";
                                    SQL += ComNum.VBLF + " WHERE ROWID = '" + strRowid_JUPMST + "' ";

                                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                                    if (!string.IsNullOrWhiteSpace(SqlErr))
                                    {
                                        //ComFunc.MsgBoxEx(this, SqlErr);
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                        return;
                                    }

                                    clsDB.setCommitTran(clsDB.DbCon);
                                }
                                catch
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                }
                            }

                            else if (mstrTitle.Equals("VNG"))
                            {
                                if (ChkEtc.Checked && VB.Val(strSEQ) >= 4 || ChkEtc.Checked == false && VB.Val(strSEQ) >= 3)
                                {
                                    if (clsDB.DbCon.Trs == null)
                                    {
                                        clsDB.setBeginTran(clsDB.DbCon);
                                    }

                                    try
                                    {
                                        SQL = " UPDATE KOSMOS_OCS.ETC_JUPMST SET  ";
                                        SQL += ComNum.VBLF + " Image_Gbn = '02'";
                                        SQL += ComNum.VBLF + " WHERE ROWID = '" + strRowid_JUPMST + "' ";

                                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                                        if (!string.IsNullOrWhiteSpace(SqlErr))
                                        {
                                            //ComFunc.MsgBoxEx(this, SqlErr);
                                            clsDB.setRollbackTran(clsDB.DbCon);
                                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                            return;
                                        }

                                        clsDB.setCommitTran(clsDB.DbCon);
                                    }
                                    catch
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                    }

                                }
                            }
                            else if (mstrTitle.Equals("EKG"))
                            {
                                if (clsType.User.JobGroup.Equals("JOB006002") || clsType.User.JobGroup.Equals("JOB026007") || clsType.User.JobGroup.Equals("JOB026011") || clsCompuInfo.gstrCOMIP.Equals("123.123.123.2"))
                                {
                                    string CurrentDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");

                                    if (File.Exists("C:\\EGS9000\\Data\\" + CurrentDate + "\\" + VB.Pstr(strFile.Trim(), "_", 2) + ".ecg") == false)
                                    {
                                        strECGFile = strFile;
                                    }
                                    else
                                    {
                                        strECGFile = VB.Pstr(strFile.Trim(), "_", 2) + ".ecg";
                                    }

                                    if (ECGFILE_FileToDB_FTP2(strECGFile, strRowid_JUPMST, strExamdate.Trim(), strPtno) == false)
                                    {
                                        continue;
                                    }
                                    strflag = "B";
                                }
                                else
                                {
                                    //if (strClinCode.Equals("HR") || strClinCode.Equals("TO"))
                                    //{
                                    //    continue;
                                    //}

                                    if (strSEQ.ToUpper().Equals("002.JPG"))
                                    {
                                        //'ecg view에서 사용할 파일 save
                                        //timer1.Stop();
                                        strECGFile = VB.Pstr(strFile, "_", 1) + ".ecg";
                                        if (ECGFILE_FileToDB_FTP(strECGFile, strRowid_JUPMST, strNum, "X", strExamdate.Trim(), strPtno) == false)
                                        {
                                            continue;
                                        }
                                        strflag = "B";
                                        //timer1.Start();
                                    }
                                }
                            }
                        }

                        #region 종검확인
                        if (mstrTitle.Equals("EKG") && string.IsNullOrWhiteSpace(strClinCode))
                        {
                            SQL = " SELECT   PTNO, TO_CHAR(SDATE,'YYYYMMDD') BDATE,  'TO' DEPTCODE, '99917' DRCODE,  'O' GBIO ";
                            SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.HEA_JEPSU ";
                            SQL += ComNum.VBLF + "WHERE PTNO ='" + strPtno + "' ";
                            SQL += ComNum.VBLF + "  AND SDATE =TO_DATE('" + strExamdate + "','YYYY-MM-DD') ";// '검사일자

                            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                            if (string.IsNullOrWhiteSpace(SqlErr) == false)
                            {
                                //ComFunc.MsgBoxEx(this, SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                return;
                            }

                            if (dt.Rows.Count > 0)
                            {
                                strClinCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                                strDRCode = dt.Rows[0]["DRCODE"].ToString().Trim();
                                if (strDRCode.Equals("1107") || strDRCode.Equals("1125"))
                                {
                                    strClinCode = "RA";
                                }
                                strBdate = dt.Rows[0]["BDATE"].ToString().Trim();
                                strClass = dt.Rows[0]["GBIO"].ToString().Trim();
                            }

                            dt.Dispose();
                        }
                        #endregion

                        if (mstrTitle.Equals("EKG") && nEKGCnt > 1)
                        {
                            if (strClinCode.Equals("TO") == false && strClinCode.Equals("HR") == false)
                            {
                                continue;
                            }
                        }

                        #region 입원/외래 데이터 구분
                        SQL = " SELECT TREATNO, INDATE, OUTDATE  FROM KOSMOS_EMR.EMR_TREATT ";
                        SQL += ComNum.VBLF + " WHERE PATID    = '" + strPtno + "' ";
                        SQL += ComNum.VBLF + "   AND CLASS    = '" + strClass + "' ";
                        if (strClass.Equals("O"))
                        {
                            SQL += ComNum.VBLF + "   AND CLINCODE = '" + strClinCode + "' ";
                        }

                        if (strClass.Equals("I"))
                        {
                            SQL += ComNum.VBLF + "   AND INDATE   <= '" + strBdate + "' ";
                            SQL += ComNum.VBLF + " ORDER BY  INDATE DESC";
                        }
                        else
                        {
                            SQL += ComNum.VBLF + "   AND INDATE   = '" + strBdate + "' ";
                            SQL += ComNum.VBLF + " ORDER BY  INDATE ";
                        }

                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                        if (string.IsNullOrWhiteSpace(SqlErr) == false)
                        {
                            //ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            strTREATNO = dt.Rows[0]["TREATNO"].ToString().Trim();
                            strOutDate = dt.Rows[0]["OUTDATE"].ToString().Trim();
                            if (string.IsNullOrWhiteSpace(strOutDate))
                                strOutDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
                        }
                        dt.Dispose();

                        #region ER 에서 전날 내원을 하고 다음날 EKG 검사해서 서버에 이미지파일 안넘어갈 경우 --20200212(김욱동)
                        if (mstrTitle.Equals("EKG") && strClass.Equals("O") && string.IsNullOrWhiteSpace(strTREATNO) && strClinCode.Equals("ER"))
                        {
                            strBdate = DateTime.ParseExact(strBdate, "yyyyMMdd", null).AddDays(-1).ToString("yyyyMMdd");

                            SQL = " SELECT TREATNO, INDATE, OUTDATE                             ";
                            SQL += ComNum.VBLF + "  FROM KOSMOS_EMR.EMR_TREATT                  ";
                            SQL += ComNum.VBLF + " WHERE PATID  ='" + strPtno + "'            ";
                            SQL += ComNum.VBLF + "   AND CLASS  = '" + strClass + "'            ";
                            SQL += ComNum.VBLF + "   AND INDATE = '" + strBdate + "'            ";
                            SQL += ComNum.VBLF + " ORDER BY INDATE                              ";

                            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                            if (string.IsNullOrWhiteSpace(SqlErr) == false)
                            {
                                //ComFunc.MsgBoxEx(this, SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                return;
                            }

                            if (dt.Rows.Count > 0)
                            {
                                strTREATNO = dt.Rows[0]["TREATNO"].ToString().Trim();
                                strOutDate = dt.Rows[0]["OUTDATE"].ToString().Trim();
                                if (string.IsNullOrWhiteSpace(strOutDate))
                                    strOutDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
                            }
                            dt.Dispose();
                        }
                        #endregion

                        #endregion

                        #region EMR STRTREATNO 값 구하기
                        if (string.IsNullOrWhiteSpace(strTREATNO) == false && string.IsNullOrWhiteSpace(strRowid_JUPMST) == false)
                        {
                            #region 2020-12-09 1건 이상일경우 영상 EMR 전송 하지 않음.
                            //if (nEKGCnt > 1)
                            //{
                            //    continue;
                            //}
                            #endregion

                            if (Directory.Exists(strPathEMR) == false)
                            {
                                Directory.CreateDirectory(strPathEMR);
                                return;
                            }
                            #endregion

                            foreach (string cvtFile in Directory.GetFiles(strPathEMR))
                            {
                                File.Delete(cvtFile);

                            }

                            #region UPLOAD 실시
                            if (mstrTitle.Equals("24HEKG"))
                            {
                                if (strClass.Trim().Equals("I"))
                                {
                                    gstrFormcode = "148"; //심장검사 결과지
                                }
                                else
                                {
                                    gstrFormcode = "149";
                                }
                            }
                            else
                            {
                                if (strClass.Trim().Equals("I"))
                                {
                                    gstrFormcode = "126"; //각종기능검사결과지
                                }
                                else
                                {
                                    gstrFormcode = mstrTitle.Equals("EKG") ? "008" : "006";
                                }
                            }

                            PageNum += 1;

                            mBitmap = new Bitmap(strPath + "\\" + strFile);
                            TifSave(strPathEMR + "\\" + strPtno + "_" + strBdate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                            string[] sdir = Directory.GetFiles(strPathEMR);
                            if (sdir.Length > 0 && strTREATNO.Equals("0") == false)
                            {
                                if (ADO_Upload(gstrFormcode, strTREATNO, strOutDate, sdir))
                                {
                                    if (File.Exists(strPathB + "\\" + strFile))
                                    {
                                        string fName = strFile.Substring(0, strFile.IndexOf(".")) + "_" + ComQuery.CurrentDateTime(clsDB.DbCon, "D");
                                        File.Copy(file, strPathB + "\\" + fName + Extension, true);
                                    }
                                    else
                                    {
                                        File.Copy(file, strPathB + "\\" + strFile, true);
                                    }
                                    File.Delete(file);
                                }
                            }

                            ss1_Sheet1.AddRows(0, 1);
                            if (mstrTitle.Equals("EKG"))
                            {
                                ss1_Sheet1.RowCount = 500;
                            }

                            ss1_Sheet1.Cells[0, 0].Text = mstrTitle.Equals("EKG") ? strExamdate : strBdate;
                            ss1_Sheet1.Cells[0, 1].Text = strPtno;
                            ss1_Sheet1.Cells[0, 2].Text = strFile;
                            ss1_Sheet1.Cells[0, 3].Text = strTREATNO;
                            ss1_Sheet1.Cells[0, 4].Text = "OK";
                            #endregion
                        }
                        else
                        {
                            ss1_Sheet1.AddRows(0, 1);
                            if (mstrTitle.Equals("EKG"))
                            {
                                ss1_Sheet1.RowCount = 500;
                            }

                            ss1_Sheet1.Cells[0, 0].Text = mstrTitle.Equals("EKG") ? strExamdate : strBdate;
                            ss1_Sheet1.Cells[0, 1].Text = strPtno;
                            ss1_Sheet1.Cells[0, 2].Text = strFile;
                            ss1_Sheet1.Cells[0, 3].Text = strTREATNO;
                            ss1_Sheet1.Cells[0, 4].Text = "Error";
                        }
                        #endregion
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, mstrTitle + "변환 도중 에러", clsDB.DbCon);
                ss1_Sheet1.AddRows(0, 1);
                if (mstrTitle.Equals("EKG"))
                {
                    ss1_Sheet1.RowCount = 500;
                }

                ss1_Sheet1.Cells[0, 0].Text = strBdate;
                ss1_Sheet1.Cells[0, 1].Text = strPtno;
                ss1_Sheet1.Cells[0, 2].Text = strFile;
                ss1_Sheet1.Cells[0, 3].Text = strTREATNO;
                ss1_Sheet1.Cells[0, 4].Text = ex.Message;
            }
        }
        #endregion

        #region New_EKG_Auto
        /// <summary>
        /// 2020-12-09 KOSMOS_OCS.ETC_JUPMST_WORK Act 후 EMR에 붙이기.
        /// </summary>
        private void New_EKG_Auto()
        {
            if (!mstrTitle.Equals("EKG"))
                return;

            #region 변수
            string strPath = string.Empty;
            string strPathB = string.Empty;

            string strClinCode = string.Empty;
            string strDRCode = string.Empty;
            string strClass = string.Empty;
            string strTREATNO = string.Empty;
            string strSEQ = string.Empty;
            string strOutDate = string.Empty;
            string strNum = string.Empty;

            string strExamdate = string.Empty;

            string strFile = string.Empty;
            string strPtno = string.Empty;
            string strPtno2 = string.Empty;
            string strBdate = string.Empty;

            string strPathEMR = InterfacePath + "\\" + mstrTitle + "_IMG";

            string strRowid_JUPMST = string.Empty;

            //OracleDataReader reader = null;
            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            int PageNum = 0;
            string gstrFormcode = string.Empty;

            double nWRTNO = 0;

            string strPathDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");

            string strflag = string.Empty;

            //double nEKGCnt = 0;

            string strECGFile = string.Empty;
            #endregion

            try
            {
                if (clsType.User.JobGroup.Equals("JOB006002") || clsType.User.JobGroup.Equals("JOB026007") || clsType.User.JobGroup.Equals("JOB026011") || clsCompuInfo.gstrCOMIP.Equals("123.123.123.2"))
                {
                    return;
                }
                else
                {
                    strPath = "C:\\Ecgviewer\\AutoImage";//           '이미지 폴더
                    strPathB = "C:\\Ecgviewer\\AutoImage\\AutoImage_backup";//   '이미지 폴더 백업폴더
                }

                if (Directory.Exists(strPath) == false)
                {
                    Directory.CreateDirectory(strPath);
                }

                if (Directory.Exists(strPathB) == false)
                {
                    Directory.CreateDirectory(strPathB);
                }

                string[] strFiles = Directory.GetFiles(strPath);

                string Extension = string.Empty;
                foreach (string file in strFiles)
                {
                    FileInfo fileInfo = new FileInfo(file);

                    //strFile = // file.Substring(file.LastIndexOf("\\") + 1);
                    strFile = fileInfo.Name;
                    Extension = fileInfo.Extension;

                    if (Extension.Equals(".tmp"))
                    {
                        continue;
                    }


                    if (clsType.User.JobGroup.Equals("JOB006002") || clsType.User.JobGroup.Equals("JOB026007") || clsType.User.JobGroup.Equals("JOB026011") || clsCompuInfo.gstrCOMIP.Equals("123.123.123.2"))
                    {
                        return;

                        strPtno = VB.Val(VB.Pstr(strFile.Trim(), "_", 1)).ToString("00000000");// '등록번호
                        strPtno2 = VB.Val(VB.Pstr(strFile.Trim(), "_", 1)).ToString("00000000");// '등록번호
                        nWRTNO = VB.Val(VB.Pstr(strFile.Trim(), "_", 1));
                        strExamdate = VB.Left(VB.Pstr(strFile.Trim(), "_", 2), 8);// '처방일
                    }
                    else
                    {
                        strNum = VB.Left(VB.Pstr(strFile.Trim(), "_", 1), 5);
                        strPtno = VB.Val(VB.Pstr(strFile.Trim(), "_", 2)).ToString("00000000");// '등록번호
                        strPtno2 = VB.Val(VB.Pstr(strFile.Trim(), "_", 2)).ToString("00000000");// '등록번호
                        nWRTNO = VB.Val(VB.Pstr(strFile.Trim(), "_", 2));
                        strExamdate = VB.Left(VB.Pstr(strFile.Trim(), "_", 3), 8);// '처방일
                        strSEQ = VB.Pstr(strFile.Trim(), "_", 5);
                    }

                    strBdate = "";
                    if (VB.Pstr(strFile, "_", 4).Trim().Equals("O6"))
                    {
                        strPtno = "";
                    }

                    #region 2018-05-01 날짜가 정확하지 않을 경우 루틴 타지 않음
                    if (mstrTitle.Equals("EKG"))
                    {
                        if (!VB.IsDate(VB.Left(strExamdate, 4) + "-" + VB.Mid(strExamdate, 5, 2) + "-" + VB.Right(strExamdate, 2)))
                            break;
                    }
                    #endregion

                    strClinCode = string.Empty;
                    strDRCode = string.Empty;
                    strClass = string.Empty;
                    strTREATNO = string.Empty;
                    strRowid_JUPMST = string.Empty;

                    if (strNum.ToUpper().Equals("WK")) //작업미완료 화일
                    {
                        strPtno = "";
                        strExamdate = "";
                    }

                    if (strPtno.IsNullOrEmpty() || VB.Val(strPtno).ToString().Equals("00000000"))
                    {
                        continue;
                    }

                    #region 각 항목에 맞는 환자정보 가져오기
                    if (string.IsNullOrWhiteSpace(strPtno) == false && string.IsNullOrWhiteSpace(strExamdate) == false)
                    {
                        ComFunc.ReadSysDate(clsDB.DbCon);

                        #region EKG
                        //if (mstrTitle.Equals("EKG"))
                        //{
                        //    #region 당일 2중 촬영이 있을경우 문제 발생
                        //    SQL = " SELECT COUNT(*) CNT ";
                        //    SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.ETC_JUPMST ";
                        //    //SQL += ComNum.VBLF + " WHERE GBJOB = '3' ";// '접수
                        //    SQL += ComNum.VBLF + " WHERE PTNO  = '" + strPtno + "' ";
                        //    SQL += ComNum.VBLF + "   AND GUBUN = '1' ";// 'EKG
                        //    SQL += ComNum.VBLF + "   AND ORDERCODE IN('E6541', '01030110') ";

                        //    if (VB.Mid(strExamdate, 7, 2).Equals("01"))
                        //    {
                        //        SQL += ComNum.VBLF + "       AND (DEPTCODE = 'ER' AND SUBSTR(BDATE, 1, 10) <> SUBSTR(RDATE, 1, 10) AND TRUNC(RDATE) >= TO_DATE('" + strExamdate + "','YYYYMMDD') ";
                        //    }
                        //    else
                        //    {
                        //        SQL += ComNum.VBLF + "       AND (DEPTCODE = 'ER' AND SUBSTR(BDATE, 1, 10) <> SUBSTR(RDATE, 1, 10) AND TRUNC(RDATE) >= TO_DATE('" + (VB.Val(strExamdate) - 1) + "','YYYYMMDD') ";
                        //    }

                        //    SQL += ComNum.VBLF + "        OR  TRUNC(RDATE) = TO_DATE('" + strExamdate + "','YYYYMMDD')) ";// '검사일자
                        //    SQL += ComNum.VBLF + "       AND BDATE <=TRUNC(SYSDATE)";

                        //    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                        //    if (string.IsNullOrWhiteSpace(SqlErr) == false)
                        //    {
                        //        //ComFunc.MsgBoxEx(this, SqlErr);
                        //        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        //        return;
                        //    }

                        //    if (dt.Rows.Count > 0)
                        //    {
                        //        nEKGCnt = dt.Rows[0]["CNT"].To<double>();
                        //    }
                        //    dt.Dispose();
                        //    #endregion
                        //}
                        #endregion


                        #region 1건 일경우 넘어간다.
                        //if (nEKGCnt == 1)
                        //{
                        //    continue;
                        //}
                        #endregion

                        string WrtnoNum = VB.Pstr(strFile, "_", 1);
                        string WORK_ROWID = string.Empty;

                        #region 환자정보
                        SQL = " SELECT A.PTNO, TO_CHAR(A.BDATE,'YYYYMMDD') BDATE, A.DEPTCODE, A.DRCODE, A.GBIO , A.ROWID, b.rowid as RID                                                                                               ";
                        SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.ETC_JUPMST A                                                                                                                                ";
                        SQL += ComNum.VBLF + "    INNER JOIN KOSMOS_OCS.ETC_JUPMST_WORK B                                                                                                                   ";
                        SQL += ComNum.VBLF + "       ON A.WRTNO = B.WRTNO                                                                                                                                   ";
                        SQL += ComNum.VBLF + "      AND A.PTNO  = B.PTNO                                                                                                                                    ";
                        SQL += ComNum.VBLF + "      AND B.GBEMR = 'Y'                                                                                                                                       ";
                        SQL += ComNum.VBLF + "      AND B.EMRDATE IS NULL                                                                                                                                        ";
                        SQL += ComNum.VBLF + " WHERE A.PTNO  = '" + strPtno + "'                                                                                                                               ";
                        SQL += ComNum.VBLF + "   AND A.WRTNO =  " + WrtnoNum + "                                                                                                                               ";
                        SQL += ComNum.VBLF + "   AND A.GUBUN = '1'                                                                                                                                            ";// 'EKG
                        SQL += ComNum.VBLF + "   AND A.ORDERCODE IN('E6541', '01030110')                                                                                                                        ";
                        SQL += ComNum.VBLF + "   AND A.GBFTP = 'Y'                                                                                                                        ";

                        //if (VB.Mid(strExamdate, 7, 2).Equals("01"))
                        //{
                        //    SQL += ComNum.VBLF + "   AND (A.DEPTCODE = 'ER' AND SUBSTR(A.BDATE, 1, 10) <> SUBSTR(A.RDATE, 1, 10) AND TRUNC(A.RDATE) >= TO_DATE('" + strExamdate + "','YYYYMMDD')                    ";
                        //}
                        //else
                        //{
                        //    SQL += ComNum.VBLF + "   AND (A.DEPTCODE = 'ER' AND SUBSTR(A.BDATE, 1, 10) <> SUBSTR(A.RDATE, 1, 10) AND TRUNC(A.RDATE) >= TO_DATE('" + (VB.Val(strExamdate) - 1) + "','YYYYMMDD')      ";
                        //}

                        //SQL += ComNum.VBLF + "   OR TRUNC(A.RDATE) = TO_DATE('" + strExamdate + "','YYYYMMDD')) ";// '검사일자
                        //SQL += ComNum.VBLF + "   AND A.BDATE <=TRUNC(SYSDATE)";
                        SQL += ComNum.VBLF + " ORDER BY A.SENDDATE DESC ";

                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                        if (string.IsNullOrWhiteSpace(SqlErr) == false)
                        {
                            //ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            strRowid_JUPMST = dt.Rows[0]["ROWID"].ToString().Trim();
                            WORK_ROWID = dt.Rows[0]["RID"].ToString().Trim();
                            strClinCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                            strDRCode = dt.Rows[0]["DRCODE"].ToString().Trim();
                            strBdate = dt.Rows[0]["BDATE"].ToString().Trim();
                            strClass = dt.Rows[0]["GBIO"].ToString().Trim();
                        }
                        dt.Dispose();
                        #endregion

                        //데이터 없으면 넘어감
                        if (WORK_ROWID.IsNullOrEmpty() || strRowid_JUPMST.IsNullOrEmpty())
                        {
                            continue;
                        }

                        #region 입원/외래 데이터 구분
                        SQL = " SELECT TREATNO, INDATE, OUTDATE  FROM KOSMOS_EMR.EMR_TREATT ";
                        SQL += ComNum.VBLF + " WHERE PATID    = '" + strPtno + "' ";
                        SQL += ComNum.VBLF + "   AND CLASS    = '" + strClass + "' ";
                        if (strClass.Equals("O"))
                        {
                            SQL += ComNum.VBLF + "   AND CLINCODE = '" + strClinCode + "' ";
                        }

                        if (strClass.Equals("I"))
                        {
                            SQL += ComNum.VBLF + "   AND INDATE   <= '" + strBdate + "' ";
                            SQL += ComNum.VBLF + " ORDER BY  INDATE DESC";
                        }
                        else
                        {
                            SQL += ComNum.VBLF + "   AND INDATE   = '" + strBdate + "' ";
                            SQL += ComNum.VBLF + " ORDER BY  INDATE ";
                        }

                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                        if (string.IsNullOrWhiteSpace(SqlErr) == false)
                        {
                            //ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            strTREATNO = dt.Rows[0]["TREATNO"].ToString().Trim();
                            strOutDate = dt.Rows[0]["OUTDATE"].ToString().Trim();
                            if (string.IsNullOrWhiteSpace(strOutDate))
                                strOutDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
                        }
                        dt.Dispose();
                        #endregion

                        #region EMR STRTREATNO 값 구하기
                        if (string.IsNullOrWhiteSpace(strTREATNO) == false && string.IsNullOrWhiteSpace(strRowid_JUPMST) == false)
                        {
                            if (Directory.Exists(strPathEMR) == false)
                            {
                                Directory.CreateDirectory(strPathEMR);
                                return;
                            }

                            foreach (string cvtFile in Directory.GetFiles(strPathEMR))
                            {
                                File.Delete(cvtFile);
                            }

                            #region UPLOAD 실시
                            if (strClass.Trim().Equals("I"))
                            {
                                gstrFormcode = "126"; //각종기능검사결과지
                            }
                            else
                            {
                                gstrFormcode = mstrTitle.Equals("EKG") ? "008" : "006";
                            }

                            PageNum += 1;

                            mBitmap = new Bitmap(strPath + "\\" + strFile);
                            TifSave(strPathEMR + "\\" + strPtno + "_" + strBdate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                            string[] sdir = Directory.GetFiles(strPathEMR);

                            if (sdir.Length > 0 && strTREATNO.Equals("0") == false)
                            {
                                if (ADO_Upload(gstrFormcode, strTREATNO, strOutDate, sdir))
                                {
                                    if (File.Exists(strPathB + "\\" + strFile))
                                    {
                                        string fName = strFile.Substring(0, strFile.IndexOf(".")) + "_" + ComQuery.CurrentDateTime(clsDB.DbCon, "D");
                                        File.Copy(file, strPathB + "\\" + fName + Extension, true);
                                    }
                                    else
                                    {
                                        File.Copy(file, strPathB + "\\" + strFile, true);
                                    }
                                    File.Delete(file);

                                    #region EMRDATE = SYSDATE UPDATE

                                    if (strSEQ.ToUpper().Equals("002.JPG") && WORK_ROWID.NotEmpty())
                                    {
                                        Upd_EmrDate(WORK_ROWID);
                                    }
                                    #endregion
                                }
                            }

                            ss1_Sheet1.AddRows(0, 1);
                            if (mstrTitle.Equals("EKG"))
                            {
                                ss1_Sheet1.RowCount = 500;
                            }

                            ss1_Sheet1.Cells[0, 0].Text = mstrTitle.Equals("EKG") ? strExamdate : strBdate;
                            ss1_Sheet1.Cells[0, 1].Text = strPtno;
                            ss1_Sheet1.Cells[0, 2].Text = strFile;
                            ss1_Sheet1.Cells[0, 3].Text = strTREATNO;
                            ss1_Sheet1.Cells[0, 4].Text = "OK";
                            #endregion
                        }
                        else
                        {
                            ss1_Sheet1.AddRows(0, 1);
                            if (mstrTitle.Equals("EKG"))
                            {
                                ss1_Sheet1.RowCount = 500;
                            }

                            ss1_Sheet1.Cells[0, 0].Text = mstrTitle.Equals("EKG") ? strExamdate : strBdate;
                            ss1_Sheet1.Cells[0, 1].Text = strPtno;
                            ss1_Sheet1.Cells[0, 2].Text = strFile;
                            ss1_Sheet1.Cells[0, 3].Text = strTREATNO;
                            ss1_Sheet1.Cells[0, 4].Text = "Error";
                        }
                        #endregion
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, mstrTitle + "변환 도중 에러", clsDB.DbCon);
                ss1_Sheet1.AddRows(0, 1);
                if (mstrTitle.Equals("EKG"))
                {
                    ss1_Sheet1.RowCount = 500;
                }

                ss1_Sheet1.Cells[0, 0].Text = strBdate;
                ss1_Sheet1.Cells[0, 1].Text = strPtno;
                ss1_Sheet1.Cells[0, 2].Text = strFile;
                ss1_Sheet1.Cells[0, 3].Text = strTREATNO;
                ss1_Sheet1.Cells[0, 4].Text = ex.Message;
            }
        }
        #endregion


        #region CHECK_EMR_TREATT
        private void CHECK_EMR_TREATT()
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            try
            {
                SQL = " SELECT BDATE, PANO, DEPTCODE";
                SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_MASTER A";
                SQL += ComNum.VBLF + " WHERE BDate = TRUNC(SYSDATE)";
                SQL += ComNum.VBLF + "   AND DEPTCODE NOT IN ('HR') ";
                SQL += ComNum.VBLF + "   AND NOT EXISTS ( SELECT * ";
                SQL += ComNum.VBLF + "                      FROM KOSMOS_EMR.EMR_TREATT B";
                SQL += ComNum.VBLF + "                     WHERE B.INDATE = TO_CHAR(A.BDATE,'YYYYMMDD')";
                SQL += ComNum.VBLF + "                       AND A.DEPTCODE = B.CLINCODE";
                SQL += ComNum.VBLF + "                       AND A.PANO = B.PATID)";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    //ComFunc.MsgBoxEx(this, SqlErr);
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        NEW_TextEMR_TreatInterface(dt.Rows[i]["PANO"].ToString().Trim(), dt.Rows[i]["BDATE"].ToString().Trim(), dt.Rows[i]["DEPTCODE"].ToString().Trim(), "외래", "정상", "");
                    }
                }

                dt.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                //ComFunc.MsgBoxEx(this, ex.Message);
                return;
            }
        }

        #endregion

        #region New_EMG_AUTO
        private void New_EMG_AUTO()
        {
            #region 변수
            string strPath = string.Empty;
            string strPathB = string.Empty;

            string strClinCode = string.Empty;
            string strDRCode = string.Empty;
            string strClass = string.Empty;
            string strTREATNO = string.Empty;
            string strSEQ = string.Empty;
            string strOutDate = string.Empty;
            string strNum = string.Empty;
            string strFile = string.Empty;
            string strPtno = string.Empty;
            string strBdate = string.Empty;

            string strPathEMR = InterfacePath + "\\" + mstrTitle + "_IMG";

            string strRowid_JUPMST = string.Empty; //XRAY_DETAIL rowid 
            string strRowid_RESULT = string.Empty; // ETC_RESULT rowid

            //OracleDataReader reader = null;
            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            int RowAffected = 0;
            int PageNum = 0;
            long nWrtno = 0;
            string strPathDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");

            string strECGFile = string.Empty;
            #endregion

            try
            {
                strPath = "C:\\EMG";            //  이미지 폴더
                strPathB = "C:\\EMG\\emgbackup";//  이미지 폴더 백업폴더

                if (Directory.Exists(strPath) == false)
                {
                    Directory.CreateDirectory(strPath);
                }

                if (Directory.Exists(strPathB) == false)
                {
                    Directory.CreateDirectory(strPathB);
                }

                string[] strFiles = Directory.GetFiles(strPath);

                foreach (string file in strFiles)
                {
                    strFile = file.Substring(file.LastIndexOf("\\") + 1);
                    strPtno = VB.Val(VB.Pstr(strFile.Trim(), "_", 1)).ToString("00000000");// '등록번호
                    string strExamdate = VB.Left(VB.Pstr(strFile.Trim(), "_", 2), 8);
                    strSEQ = VB.Pstr(VB.Pstr(strFile.Trim(), "_", 3), ".", 1);

                    strClinCode = string.Empty;
                    strDRCode = string.Empty;
                    strClass = string.Empty;
                    strTREATNO = string.Empty;

                    nWrtno = 0;

                    #region 각 항목에 맞는 환자정보 가져오기
                    if (string.IsNullOrWhiteSpace(strPtno) == false && string.IsNullOrWhiteSpace(strExamdate) == false)
                    {
                        //strflag = "A";

                        SQL = " SELECT PANO, TO_CHAR(BDATE,'YYYYMMDD') BDATE , DEPTCODE, DRCODE,  IPDOPD , ROWID , EMGWRTNO  ";
                        SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.XRAY_DETAIL ";
                        SQL += ComNum.VBLF + " WHERE PANO = '" + strPtno + "' ";
                        SQL += ComNum.VBLF + "   AND XJONG ='E' ";
                        SQL += ComNum.VBLF + "   AND XCODE NOT IN ('E7190') ";
                        SQL += ComNum.VBLF + "   AND GBRESERVED ='7' ";
                        SQL += ComNum.VBLF + "   AND TRUNC(SEEKDATE) = TO_DATE('" + strExamdate + "','YYYYMMDD') ";// '검사일자
                        SQL += ComNum.VBLF + "  ORDER BY XCODE ";

                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                        if (string.IsNullOrWhiteSpace(SqlErr) == false)
                        {
                            //ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            strRowid_JUPMST = dt.Rows[0]["ROWID"].ToString().Trim();
                            strClinCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                            strDRCode = dt.Rows[0]["DRCODE"].ToString().Trim();
                            strBdate = dt.Rows[0]["BDATE"].ToString().Trim();
                            strClass = dt.Rows[0]["IPDOPD"].ToString().Trim();
                            nWrtno = dt.Rows[0]["EMGWRTNO"].To<long>();
                        }
                        dt.Dispose();

                        if (nWrtno == 0)
                        {
                            SQL = "SELECT MAX(WRTNO ) MWRTNO FROM KOSMOS_OCS.ETC_RESULT ";

                            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                            if (string.IsNullOrWhiteSpace(SqlErr) == false)
                            {
                                //ComFunc.MsgBoxEx(this, SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                return;
                            }
                            if (dt.Rows.Count > 0)
                            {
                                nWrtno = dt.Rows[0]["MWRTNO"].To<long>() + 1;
                            }

                            dt.Dispose();
                        }

                        if (strRowid_JUPMST.NotEmpty())
                        {
                            SQL = "UPDATE KOSMOS_PMPA.XRAY_DETAIL SET  EMGWRTNO = " + nWrtno;
                            SQL += ComNum.VBLF + " WHERE ROWID = '" + strRowid_JUPMST + "'";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                            if (!string.IsNullOrWhiteSpace(SqlErr))
                            {
                                //ComFunc.MsgBoxEx(this, SqlErr);
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                return;
                            }
                        }

                        #region 값등록
                        SQL = " INSERT INTO KOSMOS_OCS.ETC_RESULT (WRTNO, GUBUN, SDATE, PTNO, seqno) VALUES ( ";
                        SQL += ComNum.VBLF + nWrtno + ", '1', TO_DATE('" + strExamdate + "','YYYY-MM-DD') , '" + strPtno + "' , '" + strSEQ + "' ) ";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                        if (!string.IsNullOrWhiteSpace(SqlErr))
                        {
                            //ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                            return;
                        }
                        #endregion

                        #region ETC_RESULT ROWID
                        SQL = " SELECT ROWID FROM  KOSMOS_OCS.ETC_RESULT";
                        SQL += ComNum.VBLF + " WHERE WRTNO =  " + nWrtno;
                        SQL += ComNum.VBLF + "   AND GUBUN =  '1'";
                        SQL += ComNum.VBLF + "   AND PTNO  = '" + strPtno + "' ";
                        SQL += ComNum.VBLF + "   AND (GbFTP IS NULL OR GbFTP <> 'Y')  ";

                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                        if (string.IsNullOrWhiteSpace(SqlErr) == false)
                        {
                            //ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                            return;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            strRowid_RESULT = dt.Rows[0]["ROWID"].ToString().Trim();
                        }

                        dt.Dispose();
                        #endregion

                        string strEmgFile = string.Empty;
                        if (strRowid_RESULT.NotEmpty())
                        {
                            strEmgFile = strFile;
                            EMGFILE_FileToDB_FTP(strEmgFile, strRowid_RESULT, "X", (strExamdate).Trim());// '2014-11-18;

                            //strflag = "B"; //영상EMR 시작

                            if (strClass.Equals("O") && chkPostpay.Checked)
                            {
                                NEW_TextEMR_TreatInterface(strPtno.Trim(), strBdate.Trim(), strClinCode.Trim(), "외래", "정상", "");
                            }

                            #region 입원/외래 데이터 구분
                            SQL = " SELECT TREATNO, INDATE, OUTDATE  FROM KOSMOS_EMR.EMR_TREATT ";
                            SQL += ComNum.VBLF + " WHERE PATID    = '" + strPtno + "' ";
                            SQL += ComNum.VBLF + "   AND CLASS    = '" + strClass + "' ";
                            if (strClass.Equals("O"))
                            {
                                SQL += ComNum.VBLF + "   AND CLINCODE = '" + strClinCode + "' ";
                                SQL += ComNum.VBLF + "   AND INDATE   = '" + strBdate + "' ";
                                SQL += ComNum.VBLF + " ORDER BY  INDATE ";
                            }
                            else
                            {
                                SQL += ComNum.VBLF + "   AND INDATE  <= '" + strBdate + "' ";
                                SQL += ComNum.VBLF + " ORDER BY INDATE DESC ";
                            }

                            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                            if (string.IsNullOrWhiteSpace(SqlErr) == false)
                            {
                                //ComFunc.MsgBoxEx(this, SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                return;
                            }

                            if (dt.Rows.Count > 0)
                            {
                                strTREATNO = dt.Rows[0]["TREATNO"].ToString().Trim();
                                strOutDate = dt.Rows[0]["OUTDATE"].ToString().Trim();
                                if (string.IsNullOrWhiteSpace(strOutDate))
                                    strOutDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
                            }
                            dt.Dispose();
                            #endregion
                        }

                        #region EMR STRTREATNO 값 구하기
                        if (string.IsNullOrWhiteSpace(strTREATNO) == false && string.IsNullOrWhiteSpace(strRowid_RESULT) == false)
                        {
                            //strflag = "C";

                            if (Directory.Exists(strPathEMR) == false)
                            {
                                Directory.CreateDirectory(strPathEMR);
                                return;
                            }

                            foreach (string cvtFile in Directory.GetFiles(strPathEMR))
                            {
                                File.Delete(cvtFile);
                            }

                            string gstrFormcode;
                            #region UPLOAD 실시
                            if (strClass.Trim().Equals("I"))
                            {
                                gstrFormcode = "129"; //근전도 결과지
                            }
                            else
                            {
                                gstrFormcode = "016";
                            }

                            PageNum += 1;

                            mBitmap = new Bitmap(strPath + "\\" + strFile);
                            TifSave(strPathEMR + "\\" + strPtno + "_" + strBdate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                            string[] sdir = Directory.GetFiles(strPathEMR);

                            if (sdir.Length > 0 && strTREATNO.Equals("0") == false)
                            {
                                if (ADO_Upload(gstrFormcode, strTREATNO, strOutDate, sdir))
                                {
                                    File.Copy(file, strPathB + "\\" + strFile, true);
                                    File.Delete(file);
                                }
                            }

                            ss1_Sheet1.AddRows(0, 1);

                            ss1_Sheet1.Cells[0, 0].Text = strExamdate;
                            ss1_Sheet1.Cells[0, 1].Text = strPtno;
                            ss1_Sheet1.Cells[0, 2].Text = strFile;
                            ss1_Sheet1.Cells[0, 4].Text = strTREATNO + "=>" + strClass;
                            //ss1_Sheet1.Cells[0, 4].Text = "OK";
                            #endregion
                        }
                        else
                        {
                            ss1_Sheet1.AddRows(0, 1);

                            ss1_Sheet1.Cells[0, 0].Text = strExamdate;
                            ss1_Sheet1.Cells[0, 1].Text = strPtno;
                            ss1_Sheet1.Cells[0, 2].Text = strFile;
                            ss1_Sheet1.Cells[0, 4].Text = strTREATNO + "=>" + strClass;

                            //ss1_Sheet1.Cells[0, 4].Text = "Error";
                        }
                        #endregion
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, "변환 도중 에러", clsDB.DbCon);
                ss1_Sheet1.AddRows(0, 1);

                ss1_Sheet1.Cells[0, 0].Text = strBdate;
                ss1_Sheet1.Cells[0, 1].Text = strPtno;
                ss1_Sheet1.Cells[0, 2].Text = strFile;
                ss1_Sheet1.Cells[0, 3].Text = strTREATNO;

                ss1_Sheet1.Cells[0, 4].Text = ex.Message;
            }
        }


        #region NEW_TextEMR_TreatInterface

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ArgPatid">등록번호</param>
        /// <param name="ArgBDate">발생일자</param>
        /// <param name="ArgDeptCode">과</param>
        /// <param name="argGUBUN">입원,외래</param>
        /// <param name="ArgSTS">정상,취소</param>
        /// <param name="ArgDrCode">의사코드 취소시사용</param>
        private void NEW_TextEMR_TreatInterface
            (string ArgPatid, string ArgBDate, string ArgDeptCode, string argGUBUN, string ArgSTS, string ArgDrCode)
        {
            ArgBDate = ArgBDate.Replace("-", "");

            string strOutDate = string.Empty;
            string strJumin = string.Empty;//주민암호화

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int RowAffected = 0;
            DataTable dt = null;
            DataTable dt2 = null;

            SQL = " SELECT P.PANO, P.SNAME, P.SEX, P.JUMIN1,P.JUMIN2,P.JUMIN3, E.PATID , E.ROWID " + ComNum.VBLF +
           "   FROM KOSMOS_PMPA.BAS_PATIENT  P , KOSMOS_EMR.EMR_PATIENTT E" + ComNum.VBLF +
           " WHERE E.PATID (+)=P.PANO AND " + ComNum.VBLF +
           "  P.PANO ='" + ArgPatid.Trim() + "' ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                //ComFunc.MsgBoxEx(this, SqlErr);
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return;
            }

            if (clsDB.DbCon.Trs == null)
            {
                clsDB.setBeginTran(clsDB.DbCon);
            }

            try
            {
                if (ArgSTS.Equals("취소"))
                {
                    dt.Dispose();

                    strOutDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");

                    if (argGUBUN.Equals("HR") || argGUBUN.Equals("TO"))
                    {
                        #region 쿼리
                        SQL = "SELECT TREATNO, ROWID  FROM KOSMOS_EMR.EMR_TREATT ";
                        SQL += ComNum.VBLF + "  WHERE PATID    = '" + ArgPatid + "' ";
                        SQL += ComNum.VBLF + "    AND INDATE   = '" + ArgBDate + "'";
                        SQL += ComNum.VBLF + "    AND CLINCODE = '" + ArgDeptCode + "'";
                        SQL += ComNum.VBLF + "    AND CLASS = 'O'";

                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                        if (string.IsNullOrWhiteSpace(SqlErr) == false)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                            //ComFunc.MsgBoxEx(this, SqlErr);
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                SQL = " UPDATE KOSMOS_EMR.EMR_TREATT SET ";
                                SQL += ComNum.VBLF + "  DELDATE = '" + strOutDate + "'"; //'2009-09-07 윤조연 수정
                                SQL += ComNum.VBLF + "  WHERE ROWID = '" + dt.Rows[i]["ROWID"].ToString().Trim() + "' ";

                                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                                if (!string.IsNullOrWhiteSpace(SqlErr))
                                {
                                    //ComFunc.MsgBoxEx(this, SqlErr);
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                    return;
                                }
                            }
                        }
                        dt.Dispose();
                        #endregion
                    }
                    else
                    {
                        #region 쿼리
                        SQL = "SELECT TREATNO, ROWID  FROM KOSMOS_EMR.EMR_TREATT ";
                        SQL += ComNum.VBLF + "  WHERE PATID   = '" + ArgPatid + "' ";
                        SQL += ComNum.VBLF + "    AND INDATE  ='" + ArgBDate + "'";
                        if (ArgDeptCode.Equals("MD") && (ArgDrCode.Equals("1107") || ArgDrCode.Equals("1125"))) //'내과 오동호 과장은 RA로 2009-09-17 윤조연
                        {
                            SQL += ComNum.VBLF + "    AND CLINCODE = 'RA'";
                        }
                        else
                        {
                            SQL += ComNum.VBLF + "    AND CLINCODE = '" + ArgDeptCode + "'";
                        }
                        if (argGUBUN.Equals("외래"))
                        {
                            SQL += ComNum.VBLF + "    AND CLASS = 'O' ";// '외래
                        }
                        else if (argGUBUN.Equals("입원"))
                        {
                            SQL += ComNum.VBLF + "    AND CLASS = 'I' ";// '외래
                        }
                        else
                        {
                            SQL += ComNum.VBLF + "    AND CLASS = '' ";
                        }

                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                        if (string.IsNullOrWhiteSpace(SqlErr) == false)
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                            clsDB.setRollbackTran(clsDB.DbCon);
                            //ComFunc.MsgBoxEx(this, SqlErr);
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                SQL = " UPDATE KOSMOS_EMR.EMR_TREATT SET ";
                                SQL += ComNum.VBLF + "  DELDATE = '" + strOutDate + "'"; //'2009-09-07 윤조연 수정
                                SQL += ComNum.VBLF + "  WHERE ROWID = '" + dt.Rows[i]["ROWID"].ToString().Trim() + "' ";

                                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                                if (!string.IsNullOrWhiteSpace(SqlErr))
                                {
                                    //ComFunc.MsgBoxEx(this, SqlErr);
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                    return;
                                }
                            }
                        }
                        dt.Dispose();
                        #endregion
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    return;
                }


                strJumin = dt.Rows[0]["Jumin1"].ToString().Trim() + VB.Left(dt.Rows[0]["Jumin2"].ToString().Trim(), 1) + "******";

                #region 'EMR_PATIENTT 테이블에 환자가 없다.
                if (string.IsNullOrWhiteSpace(dt.Rows[0]["PATID"].ToString().Trim()))
                {

                    SQL = "INSERT INTO KOSMOS_EMR.EMR_PATIENTT(PATID, JUMINNO, NAME, SEX) " + " " +
                          " VALUES('" + dt.Rows[0]["Pano"].ToString().Trim() + "' ," +
                          " '" + strJumin + "', " +
                          " '" + dt.Rows[0]["Sname"].ToString().Trim() + "', " +
                          " '" + dt.Rows[0]["Sex"].ToString().Trim() + "') ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                    if (!string.IsNullOrWhiteSpace(SqlErr))
                    {
                        ComFunc.MsgBoxEx(this, SqlErr);
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        return;
                    }
                }
                else
                {
                    SQL = "UPDATE KOSMOS_EMR.EMR_PATIENTT" + " ";
                    SQL += ComNum.VBLF + "  SET NAME ='" + dt.Rows[0]["Sname"].ToString().Trim() + "'";
                    SQL += ComNum.VBLF + "    , SEX  ='" + dt.Rows[0]["Sex"].ToString().Trim() + "'";
                    SQL += ComNum.VBLF + "    , JUMINNO ='" + strJumin + "' ";
                    SQL += ComNum.VBLF + "  WHERE ROWID = '" + dt.Rows[0]["ROWID"].ToString().Trim() + "' ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                    if (!string.IsNullOrWhiteSpace(SqlErr))
                    {
                        //ComFunc.MsgBoxEx(this, SqlErr);
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        return;
                    }
                }

                dt.Dispose();
                #endregion

                if (argGUBUN.Equals("외래"))
                {
                    #region 외래
                    SQL = "SELECT m.pano, TO_CHAR(M.BDATE, 'YYYYMMDD') Bdate ,m.deptcode, d.sabun, M.ROWID   ";
                    SQL += ComNum.VBLF + " from kosmos_pmpa.opd_master m, kosmos_ocs.ocs_doctor  d ";
                    SQL += ComNum.VBLF + "  WHERE d.drcode = m.drcode AND M.BDATE >= TO_DATE('2009-07-07', 'YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND M.PANO = '" + ArgPatid.Trim() + "' and  m.DeptCode = '" + ArgDeptCode.Trim() + "'";
                    SQL += ComNum.VBLF + "    AND (m.EMR = '0' OR m.EMR IS NULL) ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                    if (string.IsNullOrWhiteSpace(SqlErr) == false)
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        clsDB.setRollbackTran(clsDB.DbCon);
                        //ComFunc.MsgBoxEx(this, SqlErr);
                        return;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        dt = null;

                        SQL = " SELECT DrSabun as Sabun, DeptCode, TO_CHAR(BDATE, 'YYYYMMDD') Bdate,Pano,ROWID ";
                        SQL += ComNum.VBLF + "  From KOSMOS_PMPA.OPD_MASTER          ";
                        SQL += ComNum.VBLF + " Where Pano ='" + (ArgPatid).Trim() + "'                 ";
                        SQL += ComNum.VBLF + "   AND DeptCode = '" + (ArgDeptCode).Trim() + "'  ";
                        SQL += ComNum.VBLF + "   AND (EMR ='0' OR EMR IS NULL )              ";
                        SQL += ComNum.VBLF + "   AND BDATE >= TO_DATE('2009-07-07', 'YYYY-MM-DD')              ";

                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                        if (string.IsNullOrWhiteSpace(SqlErr) == false)
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                            clsDB.setRollbackTran(clsDB.DbCon);
                            //ComFunc.MsgBoxEx(this, SqlErr);
                            return;
                        }
                    }


                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string strDept = string.Empty;

                            if (ArgDeptCode.Equals("MD") && dt.Rows[i]["SABUN"].ToString().Trim().Equals("19094") ||
                                dt.Rows[i]["SABUN"].ToString().Trim().Equals("30322"))
                            {
                                strDept = "RA";
                            }
                            else
                            {
                                strDept = dt.Rows[i]["DeptCode"].ToString().Trim();
                            }

                            SQL = "SELECT TREATNO, ROWID  FROM KOSMOS_EMR.EMR_TREATT ";
                            SQL += ComNum.VBLF + "  WHERE PATID = '" + dt.Rows[i]["Pano"].ToString().Trim() + "' ";
                            SQL += ComNum.VBLF + "    AND INDATE  ='" + dt.Rows[i]["BDATE"].ToString().Trim() + "'";
                            SQL += ComNum.VBLF + "    AND CLINCODE = '" + strDept + "'";
                            SQL += ComNum.VBLF + "    AND CLASS = 'O' ";

                            SqlErr = clsDB.GetDataTableREx(ref dt2, SQL, clsDB.DbCon);
                            if (string.IsNullOrWhiteSpace(SqlErr) == false)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                //ComFunc.MsgBoxEx(this, SqlErr);
                                return;
                            }

                            if (dt2.Rows.Count == 0)
                            {
                                SQL = "INSERT INTO KOSMOS_EMR.EMR_TREATT(TREATNO, PATID, CLASS, INDATE, CLINCODE, OUTDATE, DOCCODE, ";
                                SQL += ComNum.VBLF + " ERFLAG, INTIME, OLDPATID, FSTFLAG, WARD, ROOM, COMPLETED";
                                SQL += ComNum.VBLF + " ) values( KOSMOS_EMR.SEQ_TREATNO.NEXTVAL, '" + (ArgPatid).Trim() + "' ,";
                                SQL += ComNum.VBLF + "'O' ,";//  'CLASS
                                SQL += ComNum.VBLF + "'" + dt.Rows[i]["BDATE"].ToString().Trim() + "' ,";// 'INDATE
                                SQL += ComNum.VBLF + "'" + strDept + "' ,";// 'CLINCODE 2009-09-17 윤조연수정
                                SQL += ComNum.VBLF + "'' ,";//   'OUTDATE
                                SQL += ComNum.VBLF + "'" + dt.Rows[i]["SABUN"].To<long>() + "',  ";// 'DOCCODE
                                SQL += ComNum.VBLF + "'0',  ";//'ERFLAG
                                SQL += ComNum.VBLF + "'000000',  ";//  'INITTIME
                                SQL += ComNum.VBLF + "'" + (ArgPatid).Trim() + "',  ";//'OLDPATID
                                SQL += ComNum.VBLF + "'2',  ";                           //'FST                    
                                SQL += ComNum.VBLF + "'',  ";//'WARD
                                SQL += ComNum.VBLF + "'', ";//'ROOM
                                SQL += ComNum.VBLF + "'1' )";//'COMPLETE
                            }
                            else
                            {
                                SQL = " UPDATE KOSMOS_EMR.EMR_TREATT SET ";
                                SQL += ComNum.VBLF + "  DELDATE ='', ";//  '2009-09-07 윤조연수정
                                SQL += ComNum.VBLF + "  DOCCODE = '" + dt.Rows[i]["SABUN"].To<long>() + "'";
                                SQL += ComNum.VBLF + "  WHERE ROWID = '" + dt2.Rows[0]["ROWID"].ToString().Trim() + "' ";
                            }

                            dt2.Dispose();

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                            if (!string.IsNullOrWhiteSpace(SqlErr))
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                //ComFunc.MsgBoxEx(this, SqlErr);
                                return;
                            }

                            //'ocs서버 업데이트. 적용시점에 새로 시작
                            SQL = " UPDATE kosmos_pmpa.opd_master SET   EMR = '1' WHERE ROWID = '" + dt.Rows[i]["ROWID"].ToString() + "' ";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                            if (!string.IsNullOrWhiteSpace(SqlErr))
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                //ComFunc.MsgBoxEx(this, SqlErr);
                                return;
                            }
                        }
                    }
                    dt.Dispose();
                    #endregion
                }
                else if (argGUBUN.Equals("입원"))
                {
                    #region 입원
                    SQL = " SELECT  S.PANO, TO_CHAR(S.INDATE, 'YYYYMMDD') INDATE,  TO_CHAR(S.OUTDATE, 'YYYYMMDD') OUTDATE, S.DeptCode, S.ROWID,  D.SABUN ";
                    SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.ipd_new_master S, kosmos_ocs.ocs_doctor d ";
                    SQL += ComNum.VBLF + "  WHERE S.DrCode = d.drcode ";
                    SQL += ComNum.VBLF + "    AND S.PANO = '" + ArgPatid + "' ";
                    SQL += ComNum.VBLF + "    AND S.DeptCode = '" + ArgDeptCode + "' ";
                    SQL += ComNum.VBLF + "    AND (S.EMR = '0'  OR S.EMR IS NULL)";// '나중에 적용

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                    if (string.IsNullOrWhiteSpace(SqlErr) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        //ComFunc.MsgBoxEx(this, SqlErr);
                        return;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        clsDB.setCommitTran(clsDB.DbCon);
                        dt.Dispose();
                        dt = null;
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            //string strOk = "OK";
                            //if (string.IsNullOrWhiteSpace(dt.Rows[i]["SABUN"].ToString().Trim()))
                            //{
                            //    //strOk = "NO";
                            //}

                            string strDept = string.Empty;
                            if (ArgDeptCode.Equals("MD") && dt.Rows[i]["SABUN"].ToString().Trim().Equals("19094") ||
                                dt.Rows[i]["SABUN"].ToString().Trim().Equals("30322"))
                            {
                                strDept = "RA";
                            }
                            else
                            {
                                strDept = dt.Rows[i]["DeptCode"].ToString().Trim();
                            }

                            SQL = "SELECT TREATNO, ROWID  FROM KOSMOS_EMR.EMR_TREATT ";
                            SQL += ComNum.VBLF + "  WHERE PATID = '" + ArgPatid + "' ";
                            SQL += ComNum.VBLF + "    AND INDATE  ='" + dt.Rows[i]["INDATE"].ToString().Trim() + "'";
                            SQL += ComNum.VBLF + "    AND CLINCODE = '" + dt.Rows[i]["DeptCode"].ToString().Trim() + "'";
                            SQL += ComNum.VBLF + "    AND CLASS = 'I' ";

                            SqlErr = clsDB.GetDataTableREx(ref dt2, SQL, clsDB.DbCon);
                            if (string.IsNullOrWhiteSpace(SqlErr) == false)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                //ComFunc.MsgBoxEx(this, SqlErr);
                                return;
                            }

                            if (dt2.Rows.Count == 0)
                            {
                                SQL = "INSERT INTO KOSMOS_EMR.EMR_TREATT(TREATNO, PATID, CLASS, INDATE, CLINCODE, OUTDATE, DOCCODE, ";
                                SQL += ComNum.VBLF + " ERFLAG, INTIME, OLDPATID, FSTFLAG, WARD, ROOM, COMPLETED";
                                SQL += ComNum.VBLF + " ) values( KOSMOS_EMR.SEQ_TREATNO.NEXTVAL, '" + (ArgPatid).Trim() + "' ,";
                                SQL += ComNum.VBLF + "'I' ,";//  'CLASS
                                SQL += ComNum.VBLF + "'" + dt.Rows[i]["INDATE"].ToString().Trim() + "' ,";// 'INDATE
                                SQL += ComNum.VBLF + "'" + strDept + "' ,";// 'CLINCODE 2009-09-17 윤조연수정
                                SQL += ComNum.VBLF + "'' ,";//   'OUTDATE
                                SQL += ComNum.VBLF + "'" + dt.Rows[i]["SABUN"].To<long>() + "',  ";// 'DOCCODE
                                SQL += ComNum.VBLF + "'0',  ";//'ERFLAG
                                SQL += ComNum.VBLF + "'000000',  ";//  'INITTIME
                                SQL += ComNum.VBLF + "'" + (ArgPatid).Trim() + "',  ";//'OLDPATID
                                SQL += ComNum.VBLF + "'2',  ";                           //'FST                    
                                SQL += ComNum.VBLF + "'',  ";//'WARD
                                SQL += ComNum.VBLF + "'', ";//'ROOM
                                SQL += ComNum.VBLF + "'1' )";//'COMPLETE
                            }
                            else
                            {
                                SQL = " UPDATE KOSMOS_EMR.EMR_TREATT SET ";
                                SQL += ComNum.VBLF + "  DELDATE ='', ";//  '2009-09-07 윤조연수정
                                SQL += ComNum.VBLF + "  DOCCODE = '" + dt.Rows[i]["SABUN"].To<long>() + "'";
                                SQL += ComNum.VBLF + "  OUTDATE = '" + dt.Rows[i]["OUTDATE"].ToString() + "'";
                                SQL += ComNum.VBLF + "  WHERE ROWID = '" + dt2.Rows[0]["ROWID"].ToString().Trim() + "' ";
                            }

                            dt2.Dispose();

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                            if (!string.IsNullOrWhiteSpace(SqlErr))
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                //ComFunc.MsgBoxEx(this, SqlErr);
                                return;
                            }

                            //'ocs서버 업데이트. 적용시점에 새로 시작
                            SQL = " UPDATE kosmos_pmpa.opd_master SET   EMR = '1' WHERE ROWID = '" + dt.Rows[i]["ROWID"].ToString() + "' ";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                            if (!string.IsNullOrWhiteSpace(SqlErr))
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                //ComFunc.MsgBoxEx(this, SqlErr);
                                return;
                            }
                        }
                    }
                    dt.Dispose();
                    #endregion
                }
                else if (argGUBUN.Equals("HR") || argGUBUN.Equals("TO"))
                {
                    #region 입원
                    SQL = "SELECT TREATNO, ROWID  FROM KOSMOS_EMR.EMR_TREATT ";
                    SQL += ComNum.VBLF + "  WHERE PATID    = '" + ArgPatid + "' ";
                    SQL += ComNum.VBLF + "    AND INDATE   = '" + ArgBDate + "'";
                    SQL += ComNum.VBLF + "    AND CLINCODE = '" + ArgDeptCode + "'";
                    SQL += ComNum.VBLF + "    AND CLASS    = 'O' ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                    if (string.IsNullOrWhiteSpace(SqlErr) == false)
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        clsDB.setRollbackTran(clsDB.DbCon);
                        //ComFunc.MsgBoxEx(this, SqlErr);
                        return;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        SQL = "INSERT INTO KOSMOS_EMR.EMR_TREATT(TREATNO, PATID, CLASS, INDATE, CLINCODE, OUTDATE, DOCCODE, ";
                        SQL += ComNum.VBLF + " ERFLAG, INTIME, OLDPATID, FSTFLAG, WARD, ROOM, COMPLETED";
                        SQL += ComNum.VBLF + " ) values( KOSMOS_EMR.SEQ_TREATNO.NEXTVAL, '" + (ArgPatid).Trim() + "' ,";
                        SQL += ComNum.VBLF + "'O' ,";//  'CLASS
                        SQL += ComNum.VBLF + "'" + ArgBDate + "' ,";// 'INDATE
                        SQL += ComNum.VBLF + "'" + ArgDeptCode + "' ,";// 'CLINCODE 2009-09-17 윤조연수정
                        SQL += ComNum.VBLF + "'' ,";//   'OUTDATE
                        SQL += ComNum.VBLF + "'" + ArgDrCode + "',  ";// 'DOCCODE
                        SQL += ComNum.VBLF + "'0',  ";//'ERFLAG
                        SQL += ComNum.VBLF + "'000000',  ";//  'INITTIME
                        SQL += ComNum.VBLF + "'" + (ArgPatid).Trim() + "',  ";//'OLDPATID
                        SQL += ComNum.VBLF + "'2',  ";                           //'FST                    
                        SQL += ComNum.VBLF + "'',  ";//'WARD
                        SQL += ComNum.VBLF + "'', ";//'ROOM
                        SQL += ComNum.VBLF + "'1' )";//'COMPLETE
                    }
                    else
                    {
                        SQL = " UPDATE KOSMOS_EMR.EMR_TREATT SET ";
                        SQL += ComNum.VBLF + "  DELDATE ='', ";//  '2009-09-07 윤조연수정
                        SQL += ComNum.VBLF + "  DOCCODE = '" + ArgDrCode + "'";
                        SQL += ComNum.VBLF + "  WHERE ROWID = '" + dt.Rows[0]["ROWID"].ToString().Trim() + "' ";
                    }

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                    if (!string.IsNullOrWhiteSpace(SqlErr))
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        //ComFunc.MsgBoxEx(this, SqlErr);
                        return;
                    }
                    dt.Dispose();
                }
                #endregion
                else if (argGUBUN.Equals("접종"))
                {
                    #region 접종
                    SQL = "SELECT TREATNO, ROWID  FROM KOSMOS_EMR.EMR_TREATT ";
                    SQL += ComNum.VBLF + "  WHERE PATID    = '" + ArgPatid + "' ";
                    SQL += ComNum.VBLF + "    AND INDATE   = '" + ArgBDate + "'";
                    SQL += ComNum.VBLF + "    AND CLINCODE = '" + ArgDeptCode + "'";
                    SQL += ComNum.VBLF + "    AND CLASS    = 'O' ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                    if (string.IsNullOrWhiteSpace(SqlErr) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        //ComFunc.MsgBoxEx(this, SqlErr);
                        return;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        SQL = "INSERT INTO KOSMOS_EMR.EMR_TREATT(TREATNO, PATID, CLASS, INDATE, CLINCODE, OUTDATE, DOCCODE, ";
                        SQL += ComNum.VBLF + " ERFLAG, INTIME, OLDPATID, FSTFLAG, WARD, ROOM, COMPLETED";
                        SQL += ComNum.VBLF + " ) values( KOSMOS_EMR.SEQ_TREATNO.NEXTVAL, '" + (ArgPatid).Trim() + "' ,";
                        SQL += ComNum.VBLF + "'O' ,";//  'CLASS
                        SQL += ComNum.VBLF + "'" + ArgBDate + "' ,";// 'INDATE
                        SQL += ComNum.VBLF + "'" + ArgDeptCode + "' ,";// 'CLINCODE 2009-09-17 윤조연수정
                        SQL += ComNum.VBLF + "'' ,";//   'OUTDATE
                        SQL += ComNum.VBLF + "'" + ArgDrCode + "',  ";// 'DOCCODE
                        SQL += ComNum.VBLF + "'0',  ";//'ERFLAG
                        SQL += ComNum.VBLF + "'000000',  ";//  'INITTIME
                        SQL += ComNum.VBLF + "'" + (ArgPatid).Trim() + "',  ";//'OLDPATID
                        SQL += ComNum.VBLF + "'2',  ";                           //'FST                    
                        SQL += ComNum.VBLF + "'',  ";//'WARD
                        SQL += ComNum.VBLF + "'', ";//'ROOM
                        SQL += ComNum.VBLF + "'1' )";//'COMPLETE
                    }
                    else
                    {
                        SQL = " UPDATE KOSMOS_EMR.EMR_TREATT SET ";
                        SQL += ComNum.VBLF + "  DELDATE ='', ";//  '2009-09-07 윤조연수정
                        SQL += ComNum.VBLF + "  DOCCODE = '" + ArgDrCode + "'";
                        SQL += ComNum.VBLF + "  WHERE ROWID = '" + dt.Rows[0]["ROWID"].ToString().Trim() + "' ";
                    }

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                    if (!string.IsNullOrWhiteSpace(SqlErr))
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        //ComFunc.MsgBoxEx(this, SqlErr);
                        return;
                    }
                    dt.Dispose();
                    #endregion
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                //ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }

        }

        #endregion

        #region KOSMOS_OCS.ETC_JUPMST_WORK (EMRDATE = SYSDATE)
        private bool Upd_EmrDate(string strRowid)
        {
            bool rtnVal = false;

            PsmhDb NewCon = clsDB.DBConnect();
            string SQL = string.Empty;
            clsDB.setBeginTran(NewCon);

            try
            {
                int RowAffected = 0;
                SQL = "UPDATE KOSMOS_OCS.ETC_JUPMST_WORK ";
                SQL += ComNum.VBLF + "SET";
                SQL += ComNum.VBLF + "EMRDATE = SYSDATE";
                SQL += ComNum.VBLF + "WHERE ROWID = '" + strRowid + "'";

                string SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                if (SqlErr.NotEmpty())
                {
                    clsDB.SaveSqlErrLog(SqlErr + "\r\n KOSMOS_OCS.ETC_JUPMST_WORK  업데이트 도중 에러발생", SQL, NewCon);
                    clsDB.setRollbackTran(NewCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, NewCon);
                    return rtnVal;
                }


                clsDB.setCommitTran(NewCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(NewCon);
                clsDB.SaveSqlErrLog(ex.Message, "UpdateGBPort", NewCon);
            }

            NewCon.DisDBConnect();
            return rtnVal;

        }


        #endregion

        #region GBPORT = 'M' UPDATE NEW CONNECTION 
        private bool UpdateGBPort(string strRowid, string strPTNO)
        {
            bool rtnVal = false;

            PsmhDb NewCon = clsDB.DBConnect();
            clsDB.setBeginTran(NewCon);

            try
            {
                int RowAffected = 0;
                string SQL = " UPDATE KOSMOS_OCS.ETC_JUPMST SET";
                SQL += ComNum.VBLF + "GbJob ='3'";//  '2018-11-06 구분자만 일단 체크함
                SQL += ComNum.VBLF + " WHERE ROWID = '" + strRowid + "'";
                SQL += ComNum.VBLF + "   AND GbPort = 'M'";
                SQL += ComNum.VBLF + "   AND GbJob  = '1'";

                string SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    //ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.setRollbackTran(NewCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, NewCon);
                    return rtnVal;
                }

                clsDB.setCommitTran(NewCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(NewCon);
                clsDB.SaveSqlErrLog(ex.Message, "UpdateGBPort", NewCon);
            }

            NewCon.DisDBConnect();
            return rtnVal;

        }


        #endregion

        #region ECGFILE_FileToDB_FTP
        private bool ECGFILE_FileToDB_FTP(
            string ArgFileName, string ArgRowid, string ArgNum,
            string argGubun, string ArgYYYYMM, string strPtno)
        {

            bool rtnVal = false;
            string strPath = "c:\\ecgwave\\" + ArgNum + "\\";
            string strPathBackup = "c:\\ecgwave\\backup\\";

            if (File.Exists(strPath + ArgFileName) == false && File.Exists(strPathBackup + ArgFileName) == false)
                return rtnVal;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int RowAffected = 0;

            PsmhDb NewCon = clsDB.DBConnect();
            clsDB.setBeginTran(NewCon);

            try
            {
                #region FTP OCS파일 전송 2014-11-18
                if (string.IsNullOrWhiteSpace(argGubun))
                {
                    byte[] bytes = File.ReadAllBytes(strPath + ArgFileName);

                    //using (Image img = Image.FromFile(strPath + ArgFileName))
                    //{
                    //    using (MemoryStream ms = new MemoryStream())
                    //    {
                    //        img.Save(ms, ImageFormat.Jpeg);
                    //        bytes = ms.ToArray();
                    //    }
                    //}

                    SQL = "UPDATE KOSMOS_OCS.ETC_JUPMST";
                    SQL += ComNum.VBLF + "SET ";
                    SQL += ComNum.VBLF + "IMAGE = :IMG";
                    SQL += ComNum.VBLF + "WHERE ROWID = '" + ArgRowid + "'";

                    SqlErr = clsDB.ExecuteLongRawQueryEx(SQL, bytes, ref RowAffected, NewCon);
                    if (!string.IsNullOrWhiteSpace(SqlErr))
                    {
                        //ComFunc.MsgBoxEx(this, SqlErr);
                        clsDB.setRollbackTran(NewCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, NewCon);
                        clsDB.DisDBConnect(NewCon);
                        return rtnVal;
                    }
                }

                string InsertNum = ArgFileName.Replace(".ecg", "");

                string strWRTNO = READ_SEQNO_WRTNO(NewCon, "KOSMOS_PMPA", "SEQ_OCSETC_WRTNO").ToString() + "_";
                string strLocal = string.Empty;
                if (File.Exists(strPath + ArgFileName))
                {
                    strLocal = strPath + ArgFileName.Substring(ArgFileName.LastIndexOf("\\") + 1);
                }
                else if (File.Exists(strPathBackup + ArgFileName))
                {
                    strLocal = strPathBackup + ArgFileName.Substring(ArgFileName.LastIndexOf("\\") + 1);
                }

                string strHost = "/data/ocs_etc/" + ArgYYYYMM + "/" + strWRTNO + ArgFileName;
                string strHost_folder = "/data/ocs_etc/" + ArgYYYYMM + "/";

                if (FTP_OCS_FILESEND(strLocal, strHost, strHost_folder))
                {
                    rtnVal = true;

                    DataTable dt = null;
                    SQL = " SELECT PTNO FROM KOSMOS_OCS.ETC_JUPMST_WORK ";
                    SQL += ComNum.VBLF + " WHERE WRTNO = " + InsertNum + "  ";
                    SQL += ComNum.VBLF + "   AND PTNO  = '" + strPtno + "'";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                    if (string.IsNullOrWhiteSpace(SqlErr) == false)
                    {
                        clsDB.setRollbackTran(NewCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, NewCon);
                        clsDB.DisDBConnect(NewCon);
                        return rtnVal;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        #region INSERT
                        // '저장 flag
                        SQL = "  INSERT INTO KOSMOS_OCS.ETC_JUPMST_WORK ";
                        SQL += ComNum.VBLF + " (";
                        SQL += ComNum.VBLF + " WRTNO, PTNO, EXDATE, FILENAME";
                        SQL += ComNum.VBLF + " ,    GBEMR";
                        SQL += ComNum.VBLF + " )";
                        SQL += ComNum.VBLF + " SELECT ";
                        SQL += ComNum.VBLF + InsertNum;
                        SQL += ComNum.VBLF + ", '" + strPtno + "'";
                        SQL += ComNum.VBLF + ", SYSDATE";
                        SQL += ComNum.VBLF + ", '" + strWRTNO + ArgFileName + "'";
                        SQL += ComNum.VBLF + " ,    'N'";
                        SQL += ComNum.VBLF + "  FROM DUAL";
                        SQL += ComNum.VBLF + " WHERE NOT EXISTS";
                        SQL += ComNum.VBLF + " (";
                        SQL += ComNum.VBLF + "  SELECT 1";
                        SQL += ComNum.VBLF + "    FROM KOSMOS_OCS.ETC_JUPMST_WORK";
                        SQL += ComNum.VBLF + "   WHERE WRTNO = " + InsertNum;
                        SQL += ComNum.VBLF + "     AND PTNO  = '" + strPtno + "'";
                        SQL += ComNum.VBLF + " )";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                        if (!string.IsNullOrWhiteSpace(SqlErr))
                        {
                            clsDB.setRollbackTran(NewCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, NewCon);
                            clsDB.DisDBConnect(NewCon);
                            return rtnVal;
                        }
                        #endregion

                        dt.Dispose();

                        SQL = " SELECT PTNO, DEPTCODE, ROWID            ";
                        SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.ETC_JUPMST                  ";
                        SQL += ComNum.VBLF + " WHERE PTNO  = '" + strPtno + "'";
                        SQL += ComNum.VBLF + "   AND RDATE >= TO_DATE('" + ArgYYYYMM + "', 'YYYYMMDD')";
                        SQL += ComNum.VBLF + "   AND RDATE < TO_DATE('" + ArgYYYYMM + "', 'YYYYMMDD') + 1";
                        SQL += ComNum.VBLF + "   AND GUBUN = '1' -- EKG";
                        SQL += ComNum.VBLF + "   AND ORDERCODE IN('E6541', '01030110') ";
                        SQL += ComNum.VBLF + "   AND BDATE <= TRUNC(SYSDATE)";
                        //2021-02-23 추가
                        SQL += ComNum.VBLF + "   AND CASE WHEN DEPTCODE IN('HR', 'TO') THEN 1 ";
                        SQL += ComNum.VBLF + "            WHEN DEPTCODE NOT IN('HR', 'TO') AND (GBFTP IS NULL OR GBFTP <> 'Y') THEN 1 ";
                        SQL += ComNum.VBLF + "       END = 1";
                        SQL += ComNum.VBLF + " ORDER BY SENDDATE DESC";

                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                        if (string.IsNullOrWhiteSpace(SqlErr) == false)
                        {
                            clsDB.setRollbackTran(NewCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, NewCon);
                            clsDB.DisDBConnect(NewCon);
                            return rtnVal;
                        }

                        if (dt.Rows.Count == 1 || dt.Rows.Count > 0 && (dt.Rows[0]["DEPTCODE"].ToString().Trim().Equals("TO") ||
                                                                        dt.Rows[0]["DEPTCODE"].ToString().Trim().Equals("HR")))
                        {

                            SQL = "  UPDATE KOSMOS_OCS.ETC_JUPMST  SET ";
                            SQL += ComNum.VBLF + "      GbFTP = 'Y'                                                  ";
                            SQL += ComNum.VBLF + "  ,   FilePath ='" + strWRTNO + ArgFileName + "'                  ";
                            SQL += ComNum.VBLF + "  ,   WRTNO = " + VB.Pstr(ArgFileName.Replace(".ecg", ""), "_", 1);
                            SQL += ComNum.VBLF + " WHERE ROWID = '" + ArgRowid + "' ";
                            SQL += ComNum.VBLF + "   AND PTNO  = '" + strPtno + "' ";
                            SQL += ComNum.VBLF + "   AND CASE WHEN DEPTCODE IN('HR', 'TO') THEN 1 ";
                            SQL += ComNum.VBLF + "            WHEN DEPTCODE NOT IN('HR', 'TO') AND (GBFTP IS NULL OR GBFTP <> 'Y') THEN 1 ";
                            SQL += ComNum.VBLF + "       END = 1";
                            //SQL += ComNum.VBLF + "   AND (GBFTP IS NULL OR GBFTP <> 'Y')";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, NewCon);
                            if (!string.IsNullOrWhiteSpace(SqlErr))
                            {
                                //ComFunc.MsgBoxEx(this, SqlErr);
                                clsDB.setRollbackTran(NewCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, NewCon);
                                clsDB.DisDBConnect(NewCon);
                                return rtnVal;
                            }

                            #region  ETC_JUPMST에 성공적으로 업데이트 후 치게
                            if (RowAffected > 0)
                            {
                                SQL = "  UPDATE KOSMOS_OCS.ETC_JUPMST_WORK  SET  ";
                                SQL += ComNum.VBLF + "      GBCHK   = '1'        ";
                                SQL += ComNum.VBLF + "  ,   UPDT    = SYSDATE    ";
                                SQL += ComNum.VBLF + "  ,   GBEMR   = 'Y'        ";
                                SQL += ComNum.VBLF + " WHERE WRTNO = " + VB.Pstr(ArgFileName.Replace(".ecg", ""), "_", 1);
                                SQL += ComNum.VBLF + "   AND PTNO  = '" + strPtno + "' ";

                                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, NewCon);
                                if (!string.IsNullOrWhiteSpace(SqlErr))
                                {
                                    //ComFunc.MsgBoxEx(this, SqlErr);
                                    clsDB.setRollbackTran(NewCon);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, NewCon);
                                    clsDB.DisDBConnect(NewCon);
                                    return rtnVal;
                                }
                            }
                            #endregion
                        }

                        dt.Dispose();
                        dt = null;
                    }
                    else
                    {
                        dt.Dispose();

                        #region UPDATE

                        SQL = " SELECT PTNO, DEPTCODE  ";
                        SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.ETC_JUPMST ";
                        SQL += ComNum.VBLF + " WHERE PTNO  = '" + strPtno + "'";
                        SQL += ComNum.VBLF + "   AND RDATE >= TO_DATE('" + ArgYYYYMM + "', 'YYYYMMDD')";
                        SQL += ComNum.VBLF + "   AND RDATE < TO_DATE('" + ArgYYYYMM + "', 'YYYYMMDD') + 1";
                        SQL += ComNum.VBLF + "   AND GUBUN = '1' -- EKG                             ";
                        SQL += ComNum.VBLF + "   AND ORDERCODE IN('E6541', '01030110')              ";
                        SQL += ComNum.VBLF + "   AND BDATE <= TRUNC(SYSDATE)                        ";

                        //2021-02-23 추가
                        SQL += ComNum.VBLF + "   AND CASE WHEN DEPTCODE IN('HR', 'TO') THEN 1 ";
                        SQL += ComNum.VBLF + "            WHEN DEPTCODE NOT IN('HR', 'TO') AND (GBFTP IS NULL OR GBFTP <> 'Y') THEN 1 ";
                        SQL += ComNum.VBLF + "       END = 1";

                        SQL += ComNum.VBLF + " ORDER BY SENDDATE DESC";

                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                        if (string.IsNullOrWhiteSpace(SqlErr) == false)
                        {
                            clsDB.setRollbackTran(NewCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, NewCon);
                            clsDB.DisDBConnect(NewCon);
                            return rtnVal;
                        }

                        if (dt.Rows.Count == 1 || dt.Rows.Count > 0 && (dt.Rows[0]["DEPTCODE"].ToString().Trim().Equals("TO") ||
                                                                        dt.Rows[0]["DEPTCODE"].ToString().Trim().Equals("HR")))
                        {
                            SQL = "  UPDATE KOSMOS_OCS.ETC_JUPMST  SET ";
                            SQL += ComNum.VBLF + "      GbFTP ='Y'                                                  ";
                            SQL += ComNum.VBLF + "  ,   FilePath ='" + strWRTNO + ArgFileName + "'                  ";
                            SQL += ComNum.VBLF + "  ,   WRTNO = " + VB.Pstr(ArgFileName.Replace(".ecg", ""), "_", 1);
                            SQL += ComNum.VBLF + " WHERE ROWID = '" + ArgRowid + "' ";
                            SQL += ComNum.VBLF + "   AND PTNO  = '" + strPtno + "' ";
                            SQL += ComNum.VBLF + "   AND CASE WHEN DEPTCODE IN('HR', 'TO') THEN 1 ";
                            SQL += ComNum.VBLF + "            WHEN DEPTCODE NOT IN('HR', 'TO') AND (GBFTP IS NULL OR GBFTP <> 'Y') THEN 1 ";
                            SQL += ComNum.VBLF + "       END = 1";

                            //SQL += ComNum.VBLF + "   AND (GBFTP IS NULL OR GBFTP <> 'Y')";

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, NewCon);
                            if (!string.IsNullOrWhiteSpace(SqlErr))
                            {
                                //ComFunc.MsgBoxEx(this, SqlErr);
                                clsDB.setRollbackTran(NewCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, NewCon);
                                clsDB.DisDBConnect(NewCon);
                                return rtnVal;
                            }

                            #region ETC_JUPMST에 성공적으로 쳐졌을경우만.
                            if (RowAffected > 0)
                            {
                                SQL = "  UPDATE KOSMOS_OCS.ETC_JUPMST_WORK  SET  ";
                                SQL += ComNum.VBLF + "      GBCHK   = '1'        ";
                                SQL += ComNum.VBLF + "  ,   UPDT    = SYSDATE    ";
                                SQL += ComNum.VBLF + "  ,   GBEMR   = 'Y'        ";
                                SQL += ComNum.VBLF + " WHERE WRTNO = " + VB.Pstr(ArgFileName.Replace(".ecg", ""), "_", 1);
                                SQL += ComNum.VBLF + "   AND PTNO  = '" + strPtno + "' ";

                                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, NewCon);
                                if (!string.IsNullOrWhiteSpace(SqlErr))
                                {
                                    //ComFunc.MsgBoxEx(this, SqlErr);
                                    clsDB.setRollbackTran(NewCon);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, NewCon);
                                    clsDB.DisDBConnect(NewCon);
                                    return rtnVal;
                                }
                            }
                            #endregion
                        }

                        dt.Dispose();
                        dt = null;

                        #endregion
                    }
                    if (dt != null)
                    {
                        dt.Dispose();
                    }

                    if (File.Exists(strPath + ArgFileName))
                    {
                        File.Copy(strPath + ArgFileName, strPathBackup + ArgFileName, true);
                        File.Delete(strPath + ArgFileName);
                    }

                }

                #endregion

                clsDB.setCommitTran(NewCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(NewCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, NewCon);
            }

            clsDB.DisDBConnect(NewCon);

            return rtnVal;
        }

        /// <summary>
        /// 종검용
        /// </summary>
        /// <param name="ArgFileName"></param>
        /// <param name="ArgRowid"></param>
        /// <param name="ArgYYYYMM"></param>
        /// <param name="strPtno"></param>
        /// <param name="strFullFile"></param>
        /// 
        private bool ECGFILE_FileToDB_FTP2(string ArgFileName, string ArgRowid, string ArgYYYYMM, string strPtno)
        {

            string CurrentDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");

            string strPath = ArgFileName.IndexOf(".ecg") != -1 ? "C:\\EGS9000\\Data\\" + CurrentDate + "\\" : "C:\\EKG_image\\";

            string strPathBackup = ArgFileName.IndexOf(".ecg") != -1 ? "C:\\EGS9000\\Data\\" + CurrentDate + "\\backup\\" : "C:\\EKG_image\\backup_image\\";

            if (Directory.Exists(strPath) == false)
            {
                Directory.CreateDirectory(strPath);
            }

            if (Directory.Exists(strPathBackup) == false)
            {
                Directory.CreateDirectory(strPathBackup);
            }

            bool rtnVal = false;

            if (File.Exists(strPath + ArgFileName) == false && File.Exists(strPathBackup + ArgFileName) == false)
                return rtnVal;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int RowAffected = 0;

            PsmhDb NewCon = clsDB.DBConnect();
            clsDB.setBeginTran(NewCon);

            try
            {

                #region FTP OCS파일 전송 2014-11-18
                string strWRTNO = READ_SEQNO_WRTNO(NewCon, "KOSMOS_PMPA", "SEQ_OCSETC_WRTNO").ToString() + "_";

                string strLocal = string.Empty;

                if (File.Exists(strPath + ArgFileName))
                {
                    strLocal = strPath + ArgFileName.Substring(ArgFileName.LastIndexOf("\\") + 1);
                }
                else if (File.Exists(strPathBackup + ArgFileName))
                {
                    strLocal = strPathBackup + ArgFileName.Substring(ArgFileName.LastIndexOf("\\") + 1);
                }

                //string strLocal = strPath + ArgFileName.Substring(ArgFileName.LastIndexOf("\\") + 1);
                string strHost = "/data/ocs_etc/" + ArgYYYYMM + "/" + strWRTNO + ArgFileName;
                string strHost_folder = "/data/ocs_etc/" + ArgYYYYMM + "/";


                if (FTP_OCS_FILESEND(strLocal, strHost, strHost_folder))
                {
                    // '저장 flag
                    SQL = "  UPDATE KOSMOS_OCS.ETC_JUPMST  SET ";
                    SQL += ComNum.VBLF + "  GbFTP ='Y', FilePath ='" + strWRTNO + ArgFileName + "' ";

                    if (ArgFileName.IndexOf(".ecg") == -1)
                    {
                        SQL += ComNum.VBLF + ", IMAGE_GBN = '09'";
                    }

                    SQL += ComNum.VBLF + " WHERE ROWID = '" + ArgRowid + "' ";
                    SQL += ComNum.VBLF + "   AND PTNO  = '" + strPtno + "' ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, NewCon);
                    if (!string.IsNullOrWhiteSpace(SqlErr))
                    {
                        //ComFunc.MsgBoxEx(this, SqlErr);
                        clsDB.setRollbackTran(NewCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, NewCon);
                        clsDB.DisDBConnect(NewCon);
                        return rtnVal;
                    }

                    if (RowAffected > 0)
                    {
                        if (ArgFileName.IndexOf(".ecg") != -1)
                        {
                            if (File.Exists(strPath + ArgFileName))
                            {
                                File.Copy(strPath + ArgFileName, strPathBackup + ArgFileName, true);
                                File.Delete(strPath + ArgFileName);
                            }
                        }

                        rtnVal = true;
                    }
                }


                #endregion

                clsDB.setCommitTran(NewCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(NewCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, NewCon);
            }

            NewCon.DisDBConnect();

            return rtnVal;
        }
        #endregion

        #region EMGFILE_FileToDB_FTP
        private bool EMGFILE_FileToDB_FTP(
            string ArgFileName, string ArgRowid, string argGubun, string ArgYYYYMM)
        {

            string strPath = "c:\\emg\\";
            bool rtnVal = false;
            //string strPathBackup = "c:\\emg\\emgbackup\\";

            if (File.Exists(strPath + ArgFileName) == false)
                return rtnVal;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int RowAffected = 0;

            PsmhDb NewCon = clsDB.DBConnect();
            clsDB.setBeginTran(NewCon);

            try
            {

                #region FTP OCS파일 전송 2014-11-18


                if (string.IsNullOrWhiteSpace(argGubun))
                {
                    byte[] bytes = null;

                    using (Image img = Image.FromFile(strPath + ArgFileName))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            img.Save(ms, ImageFormat.Jpeg);
                            bytes = ms.ToArray();
                        }
                    }

                    SQL = "UPDATE KOSMOS_OCS.ETC_RESULT";
                    SQL += ComNum.VBLF + "SET ";
                    SQL += ComNum.VBLF + "IMAGE = :IMG";
                    SQL += ComNum.VBLF + "WHERE ROWID = '" + ArgRowid + "'";

                    SqlErr = clsDB.ExecuteLongRawQueryEx(SQL, bytes, ref RowAffected, NewCon);
                    if (!string.IsNullOrWhiteSpace(SqlErr))
                    {
                        //ComFunc.MsgBoxEx(this, SqlErr);
                        clsDB.setRollbackTran(NewCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, NewCon);
                        NewCon.DisDBConnect();
                        return rtnVal;
                    }
                }


                string strWRTNO = READ_SEQNO_WRTNO(NewCon, "KOSMOS_PMPA", "SEQ_OCSETC_WRTNO").ToString() + "_";

                string strLocal = strPath + ArgFileName.Substring(ArgFileName.LastIndexOf("\\") + 1);
                string strHost = "/data/ocs_etc/" + ArgYYYYMM + "/" + strWRTNO + ArgFileName;
                string strHost_folder = "/data/ocs_etc/" + ArgYYYYMM + "/";


                if (FTP_OCS_FILESEND(strLocal, strHost, strHost_folder))
                {
                    // '저장 flag
                    SQL = "  UPDATE KOSMOS_OCS.ETC_RESULT  SET ";
                    SQL += ComNum.VBLF + "  GbFTP ='Y', FileName ='" + strWRTNO + ArgFileName + "' ";
                    SQL += ComNum.VBLF + " WHERE ROWID = '" + ArgRowid + "' ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, NewCon);
                    if (!string.IsNullOrWhiteSpace(SqlErr))
                    {
                        //ComFunc.MsgBoxEx(this, SqlErr);
                        clsDB.setRollbackTran(NewCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, NewCon);
                        NewCon.DisDBConnect();
                        return rtnVal;
                    }

                    rtnVal = true;
                }
                //File.Copy(strPath + ArgFileName, strPathBackup + ArgFileName);
                //File.Delete(strPath + ArgFileName);
                #endregion

                clsDB.setCommitTran(NewCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(NewCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, NewCon);
            }

            NewCon.DisDBConnect();

            return rtnVal;
        }
        #endregion
        #region HD_JPGFILE_FileToDB_FTP
        private bool HD_JPGFILE_FileToDB_FTP(
            string ArgFilePathName, string ArgRowid,
            string ArgBackupPath, string ArgYYYYMM, string ArgFile)
        {

            bool rtnVal = false;

            if (File.Exists(ArgFilePathName) == false)
                return rtnVal;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int RowAffected = 0;

            PsmhDb NewCon = clsDB.DBConnect();
            clsDB.setBeginTran(NewCon);

            try
            {

                #region FTP OCS파일 전송 2014-11-18
                string strWRTNO = READ_SEQNO_WRTNO(NewCon, "KOSMOS_PMPA", "SEQ_OCSETC_WRTNO").ToString() + "_";

                string strLocal = ArgFilePathName;
                string strHost = "/data/ocs_etc/" + ArgYYYYMM + "/" + strWRTNO + ArgFile;
                string strHost_folder = "/data/ocs_etc/" + ArgYYYYMM + "/";


                if (FTP_OCS_FILESEND(strLocal, strHost, strHost_folder))
                {
                    // '저장 flag
                    SQL = "  UPDATE KOSMOS_OCS.ETC_JUPMST  SET ";
                    SQL += ComNum.VBLF + "  GbFTP ='Y', FilePath ='" + strWRTNO + ArgFile + "' ";
                    SQL += ComNum.VBLF + " WHERE ROWID = '" + ArgRowid + "' ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, NewCon);
                    if (!string.IsNullOrWhiteSpace(SqlErr))
                    {
                        clsDB.setRollbackTran(NewCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, NewCon);
                        NewCon.DisDBConnect();
                        return rtnVal;
                    }
                }

                #endregion


                SQL = " UPDATE KOSMOS_OCS.ETC_JUPMST SET  ";
                SQL += ComNum.VBLF + " Image_Gbn = '10',";
                SQL += ComNum.VBLF + " GBJOB = '3' ";
                SQL += ComNum.VBLF + " WHERE ROWID = '" + ArgRowid + "' ";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, NewCon);
                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.setRollbackTran(NewCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, NewCon);
                    NewCon.DisDBConnect();
                    return rtnVal;
                }

                rtnVal = true;

                clsDB.setCommitTran(NewCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(NewCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, NewCon);
            }

            NewCon.DisDBConnect();

            return rtnVal;
        }
        #endregion

        #region BCM_JPGFILE_FileToDB_FTP
        private bool BCM_JPGFILE_FileToDB_FTP(
            string ArgFilePathName, string ArgRowid,
            string ArgBackupPath, string ArgYYYYMM, string ArgFile)
        {

            bool rtnVal = false;

            if (File.Exists(ArgFilePathName) == false)
                return rtnVal;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int RowAffected = 0;

            PsmhDb NewCon = clsDB.DBConnect();
            clsDB.setBeginTran(NewCon);

            try
            {

                #region FTP OCS파일 전송 2014-11-18
                string strWRTNO = READ_SEQNO_WRTNO(NewCon, "KOSMOS_PMPA", "SEQ_OCSETC_WRTNO").ToString() + "_";

                string strLocal = ArgFilePathName;
                string strHost = "/data/ocs_etc/" + ArgYYYYMM + "/" + strWRTNO + ArgFile;
                string strHost_folder = "/data/ocs_etc/" + ArgYYYYMM + "/";


                if (FTP_OCS_FILESEND(strLocal, strHost, strHost_folder))
                {
                    // '저장 flag
                    SQL = "  UPDATE KOSMOS_OCS.ETC_JUPMST  SET ";
                    SQL += ComNum.VBLF + "  GbFTP ='Y', FilePath ='" + strWRTNO + ArgFile + "' ";
                    SQL += ComNum.VBLF + " WHERE ROWID = '" + ArgRowid + "' ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, NewCon);
                    if (!string.IsNullOrWhiteSpace(SqlErr))
                    {
                        clsDB.setRollbackTran(NewCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, NewCon);
                        NewCon.DisDBConnect();
                        return rtnVal;
                    }
                }

                #endregion


                SQL = " UPDATE KOSMOS_OCS.ETC_JUPMST SET  ";
                SQL += ComNum.VBLF + " Image_Gbn = '08'";
                SQL += ComNum.VBLF + " WHERE ROWID = '" + ArgRowid + "' ";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, NewCon);
                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.setRollbackTran(NewCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, NewCon);
                    NewCon.DisDBConnect();
                    return rtnVal;
                }

                rtnVal = true;

                clsDB.setCommitTran(NewCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(NewCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, NewCon);
            }

            NewCon.DisDBConnect();

            return rtnVal;
        }
        #endregion

        #region AUDIOFILE_FileToDB_FTP
        private bool AUDIOFILE_FileToDB_FTP(
            string ArgFilePathName, string ArgRowid, string ArgBackupPath,
            string argGUBUN, string ArgYYYYMM, string ArgFile)
        {
            bool rtnVal = false;

            if (File.Exists(ArgFilePathName) == false)
                return rtnVal;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int RowAffected = 0;

            PsmhDb NewCon = clsDB.DBConnect();
            clsDB.setBeginTran(NewCon);

            try
            {

                if (string.IsNullOrWhiteSpace(argGUBUN))
                {
                    byte[] bytes = null;

                    using (Image img = Image.FromFile(ArgFilePathName))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            img.Save(ms, ImageFormat.Jpeg);
                            bytes = ms.ToArray();
                        }
                    }

                    SQL = "UPDATE KOSMOS_OCS.ETC_JUPMST";
                    SQL += ComNum.VBLF + "SET ";
                    SQL += ComNum.VBLF + "IMAGE = :IMG";
                    SQL += ComNum.VBLF + "WHERE ROWID = '" + ArgRowid + "'";

                    SqlErr = clsDB.ExecuteLongRawQueryEx(SQL, bytes, ref RowAffected, NewCon);
                    if (!string.IsNullOrWhiteSpace(SqlErr))
                    {
                        clsDB.setRollbackTran(NewCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, NewCon);
                        NewCon.DisDBConnect();
                        return rtnVal;
                    }
                }

                #region FTP OCS파일 전송 2014-11-18
                string strWRTNO = READ_SEQNO_WRTNO(NewCon, "KOSMOS_PMPA", "SEQ_OCSETC_WRTNO").ToString() + "_";

                string strLocal = ArgFilePathName;
                string strHost = "/data/ocs_etc/" + ArgYYYYMM + "/" + strWRTNO + ArgFile;
                string strHost_folder = "/data/ocs_etc/" + ArgYYYYMM + "/";


                if (FTP_OCS_FILESEND(strLocal, strHost, strHost_folder))
                {
                    // '저장 flag
                    SQL = "  UPDATE KOSMOS_OCS.ETC_JUPMST  SET ";
                    SQL += ComNum.VBLF + "  GbFTP ='Y', FilePath ='" + strWRTNO + ArgFile + "' ";
                    SQL += ComNum.VBLF + " WHERE ROWID = '" + ArgRowid + "' ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, NewCon);
                    if (!string.IsNullOrWhiteSpace(SqlErr))
                    {
                        clsDB.setRollbackTran(NewCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, NewCon);
                        NewCon.DisDBConnect();
                        return rtnVal;
                    }

                    rtnVal = true;
                }

                #endregion

                clsDB.setCommitTran(NewCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(NewCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, NewCon);
            }

            NewCon.DisDBConnect();
            return rtnVal;
        }
        #endregion


        #region FTP_OCS_FILESEND

        /// <summary>
        /// FTP_OCS_FILESEND
        /// </summary>
        /// <param name="ArgLocal"></param>
        /// <param name="ArgHost"></param>
        /// <param name="ArgFolder"></param>
        /// <returns></returns>
        private bool FTP_OCS_FILESEND(string ArgLocal, string ArgHost, string ArgFolder)
        {
            bool rtnVal = false;

            using (Ftpedt ftpedt = new Ftpedt())
            {
                string FTP_Pass = ftpedt.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle");
                if (ftpedt.FtpConnetBatch("192.168.100.31", "oracle", FTP_Pass) == false)
                {
                    ftpedt.FtpDisConnetBatch();
                    //ftpedt.Dispose();
                    //ComFunc.MsgBoxEx(this, "서버 접속 실패!");
                    return rtnVal;
                }

                if (ftpedt.FtpUploadBatchEx2(ArgLocal, ArgHost, ArgFolder) == false)
                {
                    clsDB.SaveSqlErrLog("OCS파일 업로드중 에러 : " + ArgLocal, "", clsDB.DbCon);
                    //ComFunc.MsgBoxEx(this, "생성된 서식지를 서버에 저장하는데 실패하였습니다.\r\n다시 변환 버튼을 눌러 주십시요");
                    return rtnVal;
                }

                ftpedt.FtpDisConnetBatch();
            }

            rtnVal = true;
            return rtnVal;
        }
        #endregion

        #region READ_SEQNO_WRTNO
        private long READ_SEQNO_WRTNO(PsmhDb psmhDb, string ArgOracleUser, string ArgSEQ_Name)
        {
            string strSEQ = " SELECT " + ArgOracleUser + "." + ArgSEQ_Name + ".NEXTVAL wrtno_seqno FROM DUAL ";
            string SqlErr = string.Empty;
            OracleDataReader reader = null;

            long rtnVal = 0;

            SqlErr = clsDB.GetAdoRs(ref reader, strSEQ, psmhDb);
            if (!string.IsNullOrWhiteSpace(SqlErr))
            {
                clsDB.SaveSqlErrLog(SqlErr, strSEQ, psmhDb);
                //ComFunc.MsgBoxEx(this, SqlErr);
                return rtnVal;
            }

            if (reader.HasRows && reader.Read())
            {
                rtnVal = reader.GetValue(0).To<long>();
            }

            reader.Dispose();

            return rtnVal;
        }
        #endregion

        #region EMGFILE_DBToFile
        private void EMGFILE_DBToFile(string ArgFileName, string ArgROWID, string ViewerExe = "")
        {
            string FolderPath = "C:\\PSMHEXE\\EMG";
            if (Directory.Exists(FolderPath) == false)
            {
                Directory.CreateDirectory(FolderPath);
            }

            #region 변수
            OracleDataReader reader = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            string strFTP = string.Empty;
            string RDATE = string.Empty;
            string FilePath = string.Empty;
            #endregion

            try
            {
                #region '2014-11-18 FTP저장체크
                SQL = "SELECT ROWID,TO_CHAR(SDATE,'YYYYMMDD') RDATE,FileName as FILEPATH";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_OCS.etc_RESULT ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + ArgROWID + "' ";
                SQL = SQL + ComNum.VBLF + "   AND GbFTP ='Y' ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (SqlErr.NotEmpty())
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    return;
                }

                if (reader.HasRows)
                {
                    strFTP = "Y";
                    RDATE = reader.GetValue(1).ToString().Trim();
                    FilePath = reader.GetValue(2).ToString().Trim();
                }
                #endregion

                reader.Dispose();

                if (strFTP.Equals("Y"))
                {
                    string strRemotePath = "/data/ocs_etc/" + RDATE + "/";
                    using (Ftpedt FtpedtX = new Ftpedt())
                    {
                        FtpedtX.FtpDownload("192.168.100.31", "oracle", FtpedtX.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle"), FolderPath + "\\EMG_0.jpg", FilePath, strRemotePath);
                    }

                }
                else
                {
                    if (File.Exists(ArgFileName))
                    {
                        byte[] bytes = null;
                        int RowAffected = 0;

                        using (Image img = Image.FromFile(ArgFileName))
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                img.Save(ms, ImageFormat.Jpeg);
                                bytes = ms.ToArray();
                            }
                        }


                        SQL = "UPDATE KOSMOS_OCS.ETC_RESULT";
                        SQL += ComNum.VBLF + "SET ";
                        SQL += ComNum.VBLF + "IMAGE = :IMG";
                        SQL += ComNum.VBLF + "WHERE ROWID = '" + ArgROWID + "'";

                        SqlErr = clsDB.ExecuteLongRawQueryEx(SQL, bytes, ref RowAffected, clsDB.DbCon);
                        if (!string.IsNullOrWhiteSpace(SqlErr))
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                            return;
                        }
                    }
                }

                if (ViewerExe.Equals("1"))
                {
                    Process.Start(FolderPath + "\\EMG_0.jpg");
                }
            }
            catch (Exception EX)
            {
                clsDB.SaveSqlErrLog(EX.Message, SQL, clsDB.DbCon);
            }
        }
        #endregion

        #region ADO_LabUpload
        /// <summary>
        /// 이미지 업로드
        /// </summary>
        /// <param name="crtFormCode">폼코드(서식지 번호)</param>
        /// <param name="strTREATNO">내원번호</param>
        /// <param name="strOutDate">퇴원날짜</param>
        /// <param name="sDir">파일 리스트</param>
        /// <returns></returns>
        private bool ADO_Upload(string crtFormCode, string strTREATNO, string strOutDate, string[] sDir)
        {
            #region 변수
            bool rtnVal = false;
            long nFileSize = 0;
            long nPage = 0;
            string strPAGE = string.Empty;
            string REMOTE_PATH = string.Empty;
            string strPathID = string.Empty;
            string strRemotePath = string.Empty;
            string strLocation = string.Empty;
            string strCUserID = "TransAUDIO";

            if (string.IsNullOrWhiteSpace(strTREATNO))
                return rtnVal;

            if (string.IsNullOrWhiteSpace(strPathID))
            {
                //strPathID = "0001";
                strPathID = GetTreatNoToPathInfo(strTREATNO);  //clsPath. SelectAll
            }

            OracleDataReader reader = null;
            #endregion

            //string strPath = @"C:\Program Files\BitNixChart\IMG";
            Ftpedt ftpedt = new Ftpedt();
            string strIp = string.Empty;
            string SQL = "SELECT  ipaddress ,pathport , localpath,FTPUSER, FTPPASSWD   FROM KOSMOS_EMR.EMR_PATHT WHERE PATHID ='" + strPathID + "'";
            string SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                return rtnVal;
            }

            if (reader.HasRows && reader.Read())
            {
                strRemotePath = reader.GetValue(2).ToString().Trim();
                strIp = reader.GetValue(0).ToString().Trim();
                if (ftpedt.FtpConnetBatch(strIp, reader.GetValue(3).ToString().Trim(), reader.GetValue(4).ToString().Trim()) == false)
                {
                    reader.Dispose();
                    ftpedt.FtpDisConnetBatch();
                    ftpedt.Dispose();
                    ComFunc.MsgBox("서버 접속 실패!");
                    return rtnVal;
                }
            }

            reader.Dispose();

            string istrFormCode = string.Empty;
            string[] FORMCODE;

            #region 차트 업로드?
            for (int i = 0; i < sDir.Length; i++)
            {
                FORMCODE = sDir[i].Substring(sDir[i].LastIndexOf("\\") + 1).Split('_');
                crtFormCode = FORMCODE[3].Trim();

                FileInfo fileInfo = new FileInfo(sDir[i]);
                string CryptFile = fileInfo.FullName.Replace(fileInfo.Extension, ".env");

                clsCyper.Encrypt(sDir[i], CryptFile);

                sDir[i] = CryptFile;

                fileInfo = new FileInfo(CryptFile);
                nFileSize = fileInfo.Length;

                #region 이전로직
                //if (ADO_InsertImage(ref nPage, strTREATNO, crtFormCode, strPathID, strCUserID, nFileSize, "tif", strOutDate, ref strLocation) == false)
                //{
                //    return rtnVal;
                //}
                #endregion


                #region 신규로직
                if (ADO_InsertImage(ref nPage, strTREATNO, crtFormCode, strPathID, strCUserID, nFileSize, "env", strOutDate, ref strLocation) == false)
                {
                    return rtnVal;
                }
                #endregion

                #region 스캔된 이미지 파일을 sftsvr에 업로드 한다.
                if (nPage < 1000)
                {
                    strPAGE = VB.Val(nPage.ToString()).ToString("0000");
                }
                else
                {
                    strPAGE = VB.Right(nPage.ToString(), 4);
                }

                string strServerPath = string.Empty;

                #region 이전로직
                //if (strIp.Equals("192.168.100.33"))
                //{
                //    strServerPath = strRemotePath.Replace("\\", "/") + "/" + strLocation;
                //    REMOTE_PATH = strRemotePath.Replace("\\", "/") + "/" + strLocation + "/" + nPage + ".tif";
                //}
                //else
                //{
                //    strServerPath = strRemotePath;
                //    REMOTE_PATH = strRemotePath + @"/" + strPAGE + "/" + nPage + ".tif";
                //}
                #endregion

                #region 신규로직
                if (strIp.Equals("192.168.100.33"))
                {
                    strServerPath = strRemotePath.Replace("\\", "/") + "/" + strLocation;
                    REMOTE_PATH = strRemotePath.Replace("\\", "/") + "/" + strLocation + "/" + nPage + ".env";
                }
                else
                {
                    strServerPath = strRemotePath;
                    REMOTE_PATH = strRemotePath + @"/" + strPAGE + "/" + nPage + ".env";
                }
                #endregion

                if (ftpedt.FtpUploadBatchEx2(sDir[i], REMOTE_PATH, strServerPath) == false)
                {
                    //ComFunc.MsgBox("생성된 서식지를 서버에 저장하는데 실패하였습니다.\r\n다시 변환 버튼을 눌러 주십시요");
                    return rtnVal;
                }

                #endregion

                //istrFormCode = string.Empty;

            }
            #endregion

            ftpedt.Dispose();
            rtnVal = true;
            return rtnVal;
        }

        public static string GetTreatNoToPathInfo(string TreatNo)
        {
            string rtnVal = string.Empty;

            return TreatNoToIndate(clsDB.DbCon, TreatNo).To<int>() < 2020 ? "0001" : "0003";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string TreatNoToIndate(PsmhDb pDbCon, string TreatNo)
        {
            string SQL = string.Empty;    //Query문
            string SqlErr = string.Empty; //에러문 받는 변수

            DataTable dt = null;

            SQL = "";
            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "     INDATE";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMR_TREATT";
            SQL = SQL + ComNum.VBLF + "WHERE TREATNO = " + TreatNo;

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return string.Empty;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                return "2020";
            }

            string rtnVal = dt.Rows[0]["INDATE"].ToString().Trim().Substring(0, 4);
            dt.Dispose();

            return rtnVal;
        }

        public static bool ADO_InsertImage(ref long nPageNo, string ntreat, string strFormCode, string strPathID, string strUserID, long nFileSize, string strExten, string strOutDate, ref string strLocation)
        {
            #region 변수
            string strdates = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
            string strtimes = ComQuery.CurrentDateTime(clsDB.DbCon, "T");
            bool rtnVal = false;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int RowAffected = 0;
            OracleDataReader reader = null;
            #endregion

            if (clsDB.DbCon.Trs == null)
            {
                clsDB.setBeginTran(clsDB.DbCon);
            }

            try
            {
                #region SQL
                SQL = "SELECT KOSMOS_EMR.SEQ_PAGENO.NEXTVAL SEQ_PAGENO FROM DUAL";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    return rtnVal;
                }


                if (reader.HasRows && reader.Read())
                {
                    nPageNo = (long)VB.Val(reader.GetValue(0).ToString().Trim());
                }

                reader.Dispose();
                #endregion

                if (strOutDate.Equals("20041231"))
                {
                    strLocation = VB.Left(strOutDate, 4) + "/" + strOutDate + "/" + VB.Right(nPageNo.ToString(), 4);
                }
                else if (DateTime.ParseExact(strOutDate, "yyyyMMdd", null) <= DateTime.ParseExact("20040101", "yyyyMMdd", null))
                {
                    strLocation = VB.Left(strOutDate, 4) + "/" + strOutDate + "/" + VB.Right(nPageNo.ToString(), 2);
                }
                else
                {
                    strLocation = VB.Left(strOutDate, 4) + "/" + strOutDate + "/" + VB.Right(nPageNo.ToString(), 1);
                }

                #region CHARTPAGET에 INSERT 
                SQL = "INSERT INTO KOSMOS_EMR.EMR_PAGET(PAGENO ,PATHID, CDATE, CUSERID, FILESIZE, EXTENSION, LOCATION) " +
                      "VALUES (" + nPageNo + ", '" + strPathID + "'," + strdates + ",'" + strUserID + "', " + nFileSize + ",'" + strExten + "' ,'" + strLocation + "')";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return rtnVal;
                }

                #endregion


                #region SQL
                SQL = "SELECT NVL(MAX(PAGE), 0) + 1  PAGE   FROM KOSMOS_EMR.EMR_CHARTPAGET ";
                SQL += ComNum.VBLF + " WHERE TREATNO = " + ntreat + " AND FORMCODE = '" + strFormCode + "'";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    return rtnVal;
                }


                if (reader.HasRows && reader.Read())
                {
                    SQL = "INSERT INTO KOSMOS_EMR.EMR_CHARTPAGET(PAGENO, TREATNO, FORMCODE, PAGE, CDATE, CUSERID) VALUES(" +
                    nPageNo + ", " + ntreat + ", '" + strFormCode + "' , " + reader.GetValue(0).ToString().Trim() + ", '" + strdates + "', '" + strUserID + "') ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                    if (string.IsNullOrWhiteSpace(SqlErr) == false)
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return rtnVal;
                    }

                }

                reader.Dispose();
                #endregion

                clsDB.setCommitTran(clsDB.DbCon);
                rtnVal = true;
            }

            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                clsDB.setRollbackTran(clsDB.DbCon);
            }

            return rtnVal;
        }

        #endregion

        #region TIF 

        /// <summary>
        /// TIF 저장
        /// </summary>
        /// <param name="strFileName"></param>
        private void TifSave(string strFileName)
        {
            ImageCodecInfo myImageCodecInfo;
            System.Drawing.Imaging.Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;

            myImageCodecInfo = GetEncoderInfo("image/tiff");

            myEncoder = System.Drawing.Imaging.Encoder.Quality;
            myEncoderParameters = new EncoderParameters(1);
            myEncoderParameter = new EncoderParameter(
                myEncoder,
                (long)EncoderValue.CompressionLZW);
            myEncoderParameters.Param[0] = myEncoderParameter;
            mBitmap.Save(strFileName, myImageCodecInfo, myEncoderParameters);

            if (mBitmap != null)
            {
                mBitmap.Dispose();
                mBitmap = null;
            }
        }

        private ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        #endregion

        #endregion

        private void frmEmrBaseInterface_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mBitmap != null)
            {
                mBitmap.Dispose();
                mBitmap = null;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ss1_Sheet1.RowCount = 0;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtPanoSearch.Text.IsNullOrEmpty())
                return;

            string strPano = txtPanoSearch.Text.Trim().PadLeft(8, '0');
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            SQL = " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE , TO_CHAR(SEEKDATE,'YYYY-MM-DD') SEEKDATE, A.ROWID AROWID , B.SUNAMEK, C.ROWID  CROWID, C.IMAGE,C.GbFTP   ";
            SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.XRAY_DETAIL A, KOSMOS_PMPA.BAS_SUN B ,  KOSMOS_OCS.ETC_RESULT C ";
            SQL += ComNum.VBLF + "  WHERE A.PANO = '" + strPano + "'";
            SQL += ComNum.VBLF + "    AND A.XCODE = B.SUNEXT "; ;
            SQL += ComNum.VBLF + "    AND A.EMGWRTNO = C.WRTNO ";
            SQL += ComNum.VBLF + "    AND A.XJONG ='E'";
            SQL += ComNum.VBLF + "    AND XCODE NOT IN ('F6181')";
            SQL += ComNum.VBLF + "    AND GBRESERVED ='7' ";
            SQL += ComNum.VBLF + "  ORDER BY C.SEQNO DESC  ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                //ComFunc.MsgBoxEx(this, SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                return;
            }

            ss2_Sheet1.RowCount = 0;
            ss2_Sheet1.RowCount = dt.Rows.Count;
            ss2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ss2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BDate"].ToString().Trim();
                    ss2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SEEKDATE"].ToString().Trim();
                    ss2_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SUNAMEK"].ToString().Trim();
                    ss2_Sheet1.Cells[i, 4].Text = dt.Rows[i]["image"].ToString().Trim().NotEmpty() ? "▦" : "";

                    if (dt.Rows[i]["GBFTP"].ToString().Equals("Y"))
                    {
                        ss2_Sheet1.Cells[i, 4].Text = "▦";
                    }

                    ss2_Sheet1.Cells[i, 5].Text = dt.Rows[i]["CROWID"].ToString().Trim();
                    ss2_Sheet1.Cells[i, 6].Text = dt.Rows[i]["AROWID"].ToString().Trim();

                }
            }

            dt.Dispose();

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComFunc.MsgBoxQEx(this, "정말로 삭제 하시겠습니까?", "확인") == DialogResult.No)
                return;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int RowAffected = 0;

            PsmhDb NewCon = clsDB.DBConnect();
            clsDB.setBeginTran(NewCon);

            try
            {

                for (int i = 0; i < ss2_Sheet1.NonEmptyRowCount; i++)
                {
                    if (ss2_Sheet1.Cells[i, 0].Text.Trim().Equals("True"))
                    {
                        string strRowid = ss2_Sheet1.Cells[i, 5].Text.Trim();

                        SQL = "DELETE KOSMOS_OCS.ETC_RESULT  WHERE ROWID = '" + strRowid + "'";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, NewCon);
                        if (!string.IsNullOrWhiteSpace(SqlErr))
                        {
                            //ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.setRollbackTran(NewCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, NewCon);
                            return;
                        }
                    }
                }

                clsDB.setCommitTran(NewCon);

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(NewCon);
                clsDB.SaveSqlErrLog(ex.Message, "", NewCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            clsDB.DisDBConnect(NewCon);
            btnSearch.PerformClick();
        }

        private void txtPanoSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetPatientName();
            }
        }

        private void GetPatientName()
        {
            string strPano = txtPanoSearch.Text.Trim().PadLeft(8, '0');
            lblName.Text = clsVbfunc.GetPatientName(clsDB.DbCon, strPano) + "/" + clsVbfunc.READ_SEX(clsDB.DbCon, strPano);
        }

        private void ss1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ss1_Sheet1.RowCount == 0 || mstrTitle.Equals("EMG") == false)
                return;


            txtPanoSearch.Text = ss1_Sheet1.Cells[e.Row, 1].Text.Trim();
            GetPatientName();
        }

        private void ss2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ss2_Sheet1.RowCount == 0)
                return;

            string strRowid = ss2_Sheet1.Cells[e.Row, 5].Text.Trim();
            if (ss2_Sheet1.Cells[e.Row, 4].Text.Trim().NotEmpty())
            {
                EMGFILE_DBToFile("C:\\PSMHEXE\\EMG\\EMG_0.jpg", strRowid, "1");
            }
        }
    }
}

