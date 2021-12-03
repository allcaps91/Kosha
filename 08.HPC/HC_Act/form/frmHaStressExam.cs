using ComBase;
using ComDbB; //DB연결
using ComBase.Mvc;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;
using ComBase.Controls;
using FarPoint.Win.Spread;
using System.Drawing;
using System.IO;
using System.Threading;
using Microsoft.VisualBasic;
using System.Text;
using System.Data.OleDb;

/// <summary>
/// Class Name      : HC_Act
/// File Name       : frmHaStressExam.cs
/// Description     : 스트레스검사 인터페이스
/// Author          : 이상훈
/// Create Date     : 2019-08-28
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm스트레스검사.frm(Frm스트레스검사)" />

namespace HC_Act
{
    public partial class frmHaStressExam : Form
    {
        HicJepsuResultService hicJepsuResultService = null;
        HeaSangdamWaitService heaSangdamWaitService = null;
        HicResultService hicResultService = null;
        HicXrayResultService hicXrayResultService = null;
        HeaJepsuService heaJepsuService = null;
        HeaSangdamHisService heaSangdamHisService = null;
        HicJepsuService hicJepsuService = null;

        ComHpcLibBService comHpcLibBService = null;
        EndoJupmstService endoJupmstService = null;
        EtcJupmstService etcJupmstService = null;

        HeaResultService heaResultService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();

        clsHcVariable.clsBasPatient cBasPatient = null;

        string FstrInfo;

        long FnTimerCnt;
        long FnWRTNO;
        string FstrPano;
        string FstrBDate;
        string FstrDeptCode;
        string FstrDrCode;
        string FstrSName;
        string FstrSex;
        long FnAge;
        string FstrROWID;

        string SQL = "";
        string SqlErr = "";

        public frmHaStressExam(string strInfo)
        {
            InitializeComponent();

            FstrInfo = strInfo;

            SetEvent();
        }

        void SetEvent()
        {
            hicJepsuResultService = new HicJepsuResultService();
            heaSangdamWaitService = new HeaSangdamWaitService();
            hicResultService = new HicResultService();
            hicXrayResultService = new HicXrayResultService();
            heaJepsuService = new HeaJepsuService();
            heaSangdamHisService = new HeaSangdamHisService();
            hicJepsuService = new HicJepsuService();

            comHpcLibBService = new ComHpcLibBService();
            endoJupmstService = new EndoJupmstService();
            etcJupmstService = new EtcJupmstService();

            heaResultService = new HeaResultService();

            this.Load += new EventHandler(eFormLoad);
            this.btnStart.Click += new EventHandler(eBtnClick);
            this.btnFtpSend.Click += new EventHandler(eBtnClick); 
            this.btnExit.Click += new EventHandler(eBtnClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            fn_Form_Load();

            ComFunc.ReadSysDate(clsDB.DbCon);
        }

        void fn_Form_Load()
        {
            lblMsg.Text = "검사시작을 클릭하세요..";
            lblSName.Text = VB.Pstr(FstrInfo, "{}", 1) + " " + VB.Pstr(FstrInfo, "{}", 3) + "(";
            lblSName.Text += VB.Pstr(FstrInfo, "{}", 4) + "/" + VB.Pstr(FstrInfo, "{}", 5) + ")";
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                timer1.Enabled = false;
                this.Close();
                return;
            }
            else if (sender == btnStart)
            {
                btnStart.Enabled = false;

                FnWRTNO = long.Parse(VB.Pstr(FstrInfo, "{}", 1));
                FstrPano = VB.Pstr(FstrInfo, "{}", 2).Trim();
                FstrSName = VB.Pstr(FstrInfo, "{}", 3).Trim();
                FstrSex = VB.Pstr(FstrInfo, "{}", 4).Trim();
                FnAge = long.Parse(VB.Pstr(FstrInfo, "{}", 5));
                FstrBDate = VB.Pstr(FstrInfo, "{}", 6).Trim();
                FstrDeptCode = "TO";
                FstrDrCode = "7102";

                if (fn_STRESS_CHK_FM_Interface(FstrPano) == false) return;

                fn_INSERT_STRESS_ORDER();

                FnTimerCnt = 0;
                timer1.Enabled = true;
                lblMsg.Text = "검사결과를 기다리고 있습니다.";
                Application.DoEvents();
            }
            else if (sender == btnFtpSend)
            {
                int nREAD = 0;
                string strDate = "";
                string strPATH = "";
                string strLocal = "";
                string strHost = "";
                string strHost_Folder = "";

                List<ETC_JUPMST> list = etcJupmstService.GetItemEtcJupMst();

                nREAD = list.Count;
                for (int i = 0; i < nREAD; i++)
                {
                    strPATH = list[i].FILEPATH;
                    strDate = VB.Pstr(strPATH, "_", 3);
                    strLocal = "c:\\test\\DDR_" + VB.STRCUT(strPATH, "DDR_", ".jpg") + ".jpg";

                    DirectoryInfo dir = new DirectoryInfo(@"C:\CMC\");
                    FileInfo[] files = dir.GetFiles();
                    if (dir.Exists == true)
                    {
                        Thread.Sleep(1000);
                        strHost = "/data/ocs_etc/" + strDate + "/" + strPATH;
                        strHost_Folder = "/data/ocs_etc/" + strDate + "/";
                        if (FTP_OCS_FILESEND(strLocal, strHost, strHost_Folder) != "OK")
                        {
                            MessageBox.Show("전송 실패", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                MessageBox.Show("작업 완료", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        long fn_Read_SeqNo_WrtNo(string ArgOracleUser, string ArgSEQ_Name)
        {
            long rtnVal = 0;
            string strSEQ = "";

            rtnVal = comHpcLibBService.GetSeqNo(ArgOracleUser, ArgSEQ_Name);

            return rtnVal;
        }

        string FTP_OCS_FILESEND(string ArgLocal, string ArgHost, string ArgFolder)
        {
            string rtnVal = "";

            Ftpedt FtpedtX = new Ftpedt();

            FtpedtX.FtpDeleteFile("192.168.100.31", "oracle", FtpedtX.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle"), ArgFolder, ArgHost);

            if ((FtpedtX.FtpUpload("192.168.100.31", "oracle", FtpedtX.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle"), ArgLocal, ArgFolder, ArgHost) == false))
            {
                FtpedtX = null;
                return rtnVal;
            }
            else
            {
                rtnVal = "OK";
            }

            FtpedtX = null;

            return rtnVal;
        }

        bool fn_STRESS_CHK_FM_Interface(string strPtNo)
        {
            bool rtnVal = true;
            string strPATH = "";
            string strSex = "";
            long nSeq = 0;

            DataTable dt = null;
            DataTable dt1 = null;

            ComFunc.ReadSysDate(clsDB.DbCon);

            strPATH = "Y:\\PatientDB.mdb";

            if (clsDbAccessDb.DBConnect(strPATH) == false)
            {
                ComFunc.MsgBox("MDB 접속중 문제가 발생했습니다");
                rtnVal = false;
                return rtnVal;
            }

            dt = fn_sel_MDB_PatientDB_stress("", " PatientID ", " PatientID DESC ");

            if (ComFunc.isDataTableNull(dt) == false)
            {
                nSeq = Convert.ToInt32(dt.Rows[0]["PatientID"].ToString().Trim()) + 1;
            }
            else
            {
                ComFunc.MsgBox("조회된 값이 존재하지 않습니다.");
            }

            dt.Dispose();
            dt = null;
            
            dt = fn_sel_MDB_PatientDB_stress(strPtNo, " ChartNO ", "");

            //인바디 환자정보 체크
            if (dt.Rows.Count == 0)
            {
                cBasPatient = new clsHcVariable.clsBasPatient();
                //원내정보 체크
                cBasPatient = fn_sel_Bas_Patient_cls(strPtNo);

                strSex = "";
                if (cBasPatient.Sex == "F")
                {
                    strSex = "Female";
                }
                else
                {
                    strSex = "Male";
                }

                cBasPatient.Sex = strSex;

                //자료 생성
                clsDbAccessDb.setBeginTran();

                try
                {
                    if (fn_ins_MDB_PatientDB_stress(cBasPatient, nSeq, clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime) == true)
                    {
                        clsDbAccessDb.setCommitTran();
                        rtnVal = true;
                    }
                    else
                    {
                        clsDbAccessDb.setRollbackTran();
                        rtnVal = false;
                    }
                }
                catch (Exception ex)
                {
                    clsDbAccessDb.setRollbackTran();
                    ComFunc.MsgBox(ex.Message);
                    rtnVal = false;
                    return rtnVal;
                }
            }

            dt.Dispose();
            dt = null;

            clsDbAccessDb.DisDBConnect();

            return rtnVal;
        }

        DataTable fn_sel_MDB_PatientDB_stress(string argPano, string argCols, string argOrderBy)
        {
            SQL = "";
            SQL += " SELECT                                                         \r";
            SQL += "    " + argCols + "                                             \r";
            SQL += "   FROM PatientDB                                               \r";
            SQL += "  WHERE 1 = 1                                                   \r";
            if (argPano != "")
            {
                SQL += "    AND ChartNO = '" + argPano + "'                         \r";
            }
            if (argOrderBy != "")
            {
                SQL += "   ORDER BY " + argOrderBy + "                              \r";
            }

            try
            {
                return clsDbAccessDb.GetDataTable(SQL);
            }
            catch (System.Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return null;
            }
        }

        bool fn_ins_MDB_PatientDB_stress(clsHcVariable.clsBasPatient argCls, long argSeq, string argSysDate)
        {
            bool rtnVal = false;

            SQL = "";
            SQL += " INSERT INTO PatientDB                                                  \r";
            SQL += "        (PatientID,ChartNO,Name,PersonalID,Sex,Age,RegDate,LatestDate)  \r";
            SQL += " VALUES                                                                 \r";
            SQL += "      (" + argSeq + "                                                   \r";
            SQL += "      , '" + argCls.Pano + "'                                           \r";
            SQL += "      , '" + argCls.SName + "'                                          \r";
            SQL += "      , '" + argCls.Jumin1 + argCls.Jumin3 + "'                         \r";
            SQL += "      , '" + argCls.Sex + "'                                            \r";
            SQL += "      , '" + argCls.Age + "'                                            \r";
            SQL += "      , '" + argSysDate + "'                                            \r";
            SQL += "      , '" + argSysDate + "'                                            \r";
            SQL += "      )                                                                 \r";

            rtnVal = clsDbAccessDb.ExecuteNonQuery(SQL);

            return rtnVal;
        }

        void fn_INSERT_STRESS_ORDER()
        {
            int nRead = 0;
            string strJob = "";

            strJob = "1";   //종검은 스트레스 검사만 실시함
            FstrROWID = "";

            if (comHpcLibBService.GetEtcJupMstbyPaNo(FstrPano, FstrBDate, FstrDeptCode) == 0)
            {
                //clsDB.setBeginTran(clsDB.DbCon);

                ETC_JUPMST item = new ETC_JUPMST();

                item.BDATE = FstrBDate;
                item.PTNO = FstrPano;
                item.SNAME = FstrSName;
                item.SEX = FstrSex;
                item.AGE = FnAge;
                item.ORDERCODE = "STRESS";
                item.ORDERNO = 55;
                item.GBIO = "O";
                item.BUN = "47";
                item.DEPTCODE = FstrDeptCode;
                item.DRCODE = FstrDrCode;
                item.REMARK = "FM stress1 auto send";
                item.AMT = 0;
                item.GBJOB = "1";
                item.GBER = "";
                item.GUBUN = "18";

                int result = etcJupmstService.Insert_Etc_JupMst(item);

                if (result < 0)
                {
                    //clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("스트레스 처방 발생 중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //clsDB.setCommitTran(clsDB.DbCon);

                ETC_JUPMST list = etcJupmstService.GetRowIdbyPtNoBDateDeptcode(FstrPano, FstrBDate, FstrDeptCode);

                if (!list.IsNullOrEmpty())
                {
                    FstrROWID = list.ROWID;
                }
            }
            else
            {
                ETC_JUPMST list2 = etcJupmstService.GetRowIdbyPtNoBDateDeptcode(FstrPano, FstrBDate, FstrDeptCode);

                if (!list2.IsNullOrEmpty())
                {
                    FstrROWID = list2.ROWID;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            long FNo = 0;
            string strOK = "";
            string strFileName1 = "";
            string strFileName2 = "";
            string strFile = "";
            string strBDATE = "";
            string strSogen = "";
            string strTitle = "";
            string strLocal = "";
            string strHost = "";
            string strHost_Folder = "";
            string strWRTNO = "";
            string sPath = "";
            string line = "";
            int nCnt = 0;
            int result = 0;
            string fileName;
            string destFile;

            FnTimerCnt += 1;
            if (FnTimerCnt >= 300)  //5분
            {
                timer1.Enabled = false;
                MessageBox.Show("스트레스검사 인터페이스 오류", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            strOK = "";

            strBDATE = FstrBDate.Replace("-", "");
            strFileName1 = "";
            strFileName2 = "";

            DirectoryInfo dir = new DirectoryInfo(@"X:\backup_image");
            if (dir.Exists == false)
            {
                dir.Create();
            }

            DirectoryInfo di = new System.IO.DirectoryInfo(@"X:\");
            FileInfo[] fi = di.GetFiles("*.jpg");

            Application.DoEvents();

            foreach (FileInfo file in fi)
            {
                strFile = file.Name.ToLower();
                if (VB.Right(strFile, 4) == ".jpg")
                {
                    if (VB.Pstr(VB.Pstr(strFile, ".jpg", 1), "_", 3) == FstrPano)
                    {
                        strOK = "OK1";
                        strFileName1 = VB.Pstr(strFile, ".jpg", 1) + ".jpg";
                    }
                }

                //결과값이 있는지 점검
                if (strOK == "OK1")
                {
                    if (VB.Right(strFile, 4) == ".ini")
                    {
                        if (VB.Pstr(strFile, "_", 2) == strBDATE)
                        {
                            if (VB.Pstr(VB.Pstr(strFile, ".ini", 1), "_", 3) == FstrPano)
                            {
                                strOK = "OK2";
                                strFileName2 = VB.Pstr(strFile, ".ini", 1).ToUpper() + ".ini";
                                break;
                            }
                        }
                    }
                }
            }

            //jpg와 결과값이 모두 없으면 대기함
            if (strOK != "OK2")
            {
                return;
            }

            lblMsg.Text = "검사결과를 가져오고 있습니다.";
            Application.DoEvents();

            timer1.Enabled = false;

            //소견처리
            strSogen = "";
            FNo = FileSystem.FreeFile();

            DirectoryInfo Dir = new System.IO.DirectoryInfo(@"X:\" + strFileName2);

            if (Dir.Exists == true)
            {
                sPath = "X:\\" + strFileName2;

                if (File.Exists(sPath) == true)
                {
                    File.Open(sPath, FileMode.Open);
                    StreamReader file = new StreamReader(sPath);

                    while ((line = file.ReadLine()) != null)
                    {
                        strTitle = line;
                        if (!strTitle.IsNullOrEmpty())
                        {
                            if (VB.Left(strTitle, 8) == "Commnet=")
                            {
                                strSogen = strTitle.Replace("Commnet=", "");
                            }
                        }
                        nCnt++;
                    }
                    file.Close();
                }
            }

            //clsDB.setBeginTran(clsDB.DbCon);

            result = etcJupmstService.UpdateImageGbJobSogenbyRowId("03", "3", strSogen, FstrROWID);

            if (result < 0)
            {
                //clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("ETC_JUPMST UPDATE 중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //FTP OCS파일 전송
            strWRTNO = fn_Read_SeqNo_WrtNo("KOSMOS_PMPA", "SEQ_OCSETC_WRTNO").To<string>();

            strLocal = @"X:\" + strFileName1;
            strHost = @"/data/ocs_etc/" + strBDATE + "/" + strWRTNO + "_" + strFileName1;
            strHost_Folder = @"/data/ocs_etc/" + strBDATE + "/";

            Thread.Sleep(1000);
            if (FTP_OCS_FILESEND(strLocal, strHost, strHost_Folder) == "OK")
            {
                //저장flag
                result = etcJupmstService.UpdateGbFtpFilePathbyWorId("Y", strWRTNO + "_" + strFileName1, FstrROWID);

                if (result < 0)
                {
                    //clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("ETC_JUPMST UPDATE 중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            //스트레스검사 액팅완료 처리
            result = heaResultService.UpdateResultActivebyWrtNoExCode("01", "Y", clsType.User.IdNumber, FnWRTNO, "A985");

            if (result < 0)
            {
                //clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("HEA_RESULT UPDATE 중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //파일처리 및 백업
            DirectoryInfo Dir2 = new System.IO.DirectoryInfo(@"X:\" + strFileName1);

            if (Dir.Exists == true)
            {
                File.Copy(@"X:\" + strFileName1, @"X:\backup_image\" + strFileName1);
                File.Delete(@"X:\" + strFileName1);
                clsPublic.GstrHelpCode = @"X:\backup_image\" + strFileName1;
            }

            //소견파일 백업, 삭제
            DirectoryInfo Dir3 = new System.IO.DirectoryInfo(@"X:\" + strFileName2);

            if (Dir.Exists == true)
            {
                File.Copy(@"X:\" + strFileName2, @"X:\backup_image\" + strFileName2);
                File.Delete(@"X:\" + strFileName2);
            }

            lblMsg.Text = "검사결과 인터페이스 완료";
            Application.DoEvents();

            Thread.Sleep(500);  //약 0.5초간 대기

            this.Close();
        }

        private clsHcVariable.clsBasPatient fn_sel_Bas_Patient_cls(string argPano)
        {
            DataTable dt = null;
            clsHcVariable.clsBasPatient cBasPat = new clsHcVariable.clsBasPatient(); 

            SQL = "";
            SQL += " SELECT PANO,SNAME,SEX,HPHONE,GBBIRTH,EMAIL             \r";
            SQL += "      , JUMIN1,JUMIN2,JUMIN3 ,EKGMSG,TEL                \r";
            SQL += "      , ZIPCODE1,ZIPCODE2,ZIPCODE3                      \r";
            SQL += "      , BUILDNO,RELIGION,DEPTCODE,DRCODE                \r";
            SQL += "      , ROADDETAIL,JUSO,JIKUP,BI                        \r";
            SQL += "      , TO_CHAR(BIRTH,'YYYY-MM-DD') BIRTH               \r";
            SQL += "      , TO_CHAR(STARTDATE,'YYYY-MM-DD') STARTDATE       \r";
            SQL += "      , TO_CHAR(LASTDATE,'YYYY-MM-DD') LASTDATE         \r";
            SQL += "      , JUMIN1 || '-' || JUMIN2 JUMINFULL               \r";
            SQL += "   FROM KOSMOS_PMPA.BAS_PATIENT                         \r";
            SQL += "  WHERE PANO = '" + argPano + "'                        \r";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return null;
                }

                if (ComFunc.isDataTableNull(dt) == false)
                {
                    cBasPat.Pano = dt.Rows[0]["Pano"].ToString().Trim();
                    cBasPat.SName = dt.Rows[0]["SName"].ToString().Trim();
                    cBasPat.Sex = dt.Rows[0]["Sex"].ToString().Trim();
                    cBasPat.EMail = dt.Rows[0]["EMail"].ToString().Trim();
                    cBasPat.DeptCode = dt.Rows[0]["DeptCode"].ToString().Trim();
                    cBasPat.DrCode = dt.Rows[0]["DrCode"].ToString().Trim();
                    cBasPat.Bi = dt.Rows[0]["Bi"].ToString().Trim();
                    cBasPat.Jumin1 = dt.Rows[0]["Jumin1"].ToString().Trim();
                    cBasPat.Jumin2 = dt.Rows[0]["Jumin2"].ToString().Trim();
                    if (dt.Rows[0]["Jumin3"].ToString().Trim() != "")
                    {
                        cBasPat.Jumin3 = clsAES.DeAES(dt.Rows[0]["Jumin3"].ToString().Trim());
                    }
                    else
                    {
                        cBasPat.Jumin3 = "";
                    }

                    cBasPat.JuminFull = dt.Rows[0]["JuminFULL"].ToString().Trim();
                    cBasPat.ZipCode1 = dt.Rows[0]["ZipCode1"].ToString().Trim();
                    cBasPat.ZipCode2 = dt.Rows[0]["ZipCode2"].ToString().Trim();
                    cBasPat.ZipCode3 = dt.Rows[0]["ZipCode3"].ToString().Trim();
                    cBasPat.BuildNo = dt.Rows[0]["BuildNo"].ToString().Trim();
                    cBasPat.RoadDetail = dt.Rows[0]["RoadDetail"].ToString().Trim();
                    cBasPat.Juso = dt.Rows[0]["Juso"].ToString().Trim();
                    cBasPat.Religion = dt.Rows[0]["Religion"].ToString().Trim();//종교
                    cBasPat.StartDate = dt.Rows[0]["StartDate"].ToString().Trim();
                    cBasPat.LastDate = dt.Rows[0]["LastDate"].ToString().Trim();
                    cBasPat.HPhone = dt.Rows[0]["HPhone"].ToString().Trim();
                    cBasPat.Tel = dt.Rows[0]["Tel"].ToString().Trim();
                    cBasPat.Birth = dt.Rows[0]["Birth"].ToString().Trim();
                    cBasPat.GbBirth = dt.Rows[0]["GbBirth"].ToString().Trim();
                    cBasPat.Jikup = dt.Rows[0]["Jikup"].ToString().Trim();
                    cBasPat.EkgMsg = dt.Rows[0]["EkgMsg"].ToString().Trim();
                    cBasPat.Age = ComFunc.AgeCalc(clsDB.DbCon, cBasPat.Jumin1 + cBasPat.Jumin3);

                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return null;
            }

            return cBasPat;

        }
    }
}
