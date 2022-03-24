using ComBase;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : SupDrst
    /// File Name       : frmSupDrstDifDown.cs
    /// Description     : First DIS DIF-Reference 다운로더
    /// Author          : 이정현
    /// Create Date     : 2017-12-04
    /// <history> 
    /// First DIS DIF-Reference 다운로더
    /// </history>
    /// <seealso>
    /// PSMH\drug\drinfo\FrmDifDown.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\drinfo\drinfo.vbp
    /// </vbp>
    /// </summary>
    public partial class frmSupDrstDifDown : Form
    {
        private string GstrExeCode = "";
        private string GstrDrugCode = "";

        public frmSupDrstDifDown()
        {
            InitializeComponent();
        }

        public frmSupDrstDifDown(string strExeCode, string strDrugCode)
        {
            InitializeComponent();

            GstrExeCode = strExeCode;
            GstrDrugCode = strDrugCode;
        }

        private void frmSupDrstDifDown_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");
            
            DownLoad();
        }

        private void DownLoad()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strFile = "";
            string strExe = "";
            //string strDir = "";
            //string strOK = "";
            string strExeFile = "";

            //프로그램이 사용중인지 체크
            System.Diagnostics.Process[] ProcessEx = System.Diagnostics.Process.GetProcessesByName("ClinicalRef");

            if (ProcessEx.Length > 0)
            {
                System.Diagnostics.Process[] Pro1 = System.Diagnostics.Process.GetProcessesByName("ClinicalRef");
                System.Diagnostics.Process CurPro = System.Diagnostics.Process.GetCurrentProcess();
                foreach (System.Diagnostics.Process Proc in Pro1)
                {
                    if (Proc.Id != CurPro.Id)
                    {
                        if (ComFunc.MsgBoxQ("프로그램이 이미 실행중입니다." + ComNum.VBLF + "재실행 하시겠습니까?", "알림", MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        {
                            ComFunc.KillProc("ClinicalRef");
                        }
                        else
                        {
                            this.Close();
                            return;
                        }
                    }
                }
            }

            ssView_Sheet1.RowCount = 0;
            progressBar1.Value = 0;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     NAME, CODE";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'DRUG_DIF_FILELIST'";
                SQL = SQL + ComNum.VBLF + "ORDER BY CODE ASC";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    ComFunc.MsgBox("다운로드 파일이 없습니다."
                        + ComNum.VBLF + "전산정보팀에 연락 바랍니다.");
                    dt.Dispose();
                    dt = null;

                    this.Close();
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["NAME"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["CODE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (CheckDel() == true)
                {
                    progressBar1.Value = 0;
                    progressBar1.Maximum = ssView_Sheet1.RowCount;

                    for (i = 0; i < ssView_Sheet1.RowCount; i++)
                    {
                        progressBar1.Value = i;

                        strFile = ssView_Sheet1.Cells[i, 1].Text.Trim();
                        strExe = ssView_Sheet1.Cells[i, 2].Text.Trim();

                        //strDir = "c:\\cmc\\ocsexe\\";

                        Application.DoEvents();

                        Dir_Check("c:\\cmc\\ocsexe", strFile);
                    }
                }

                progressBar1.Value = 0;
                progressBar1.Maximum = ssView_Sheet1.RowCount - 1;

                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    progressBar1.Value = i;

                    strFile = ssView_Sheet1.Cells[i, 1].Text.Trim();
                    strExe = ssView_Sheet1.Cells[i, 2].Text.Trim();

                    //strDir = "c:\\cmc\\ocsexe\\";

                    Application.DoEvents();

                    if (VB.Len(strExe) > 2)
                    {
                        DOWNLOADnCREATE("c:\\cmc\\ocsexe", strFile, strExe, "", "FIRSTDIS");
                        strExeFile = strFile;
                    }
                    else
                    {
                        DOWNLOADnCREATE("c:\\cmc\\ocsexe", strFile, "", "", "FIRSTDIS");
                    }
                    
                    if (File.Exists("c:\\cmc\\ocsexe\\" + strFile) == false)
                    {
                        //strOK = "NO";
                    }
                    else
                    {
                        switch (GstrExeCode)
                        {
                            case "MTSIORDER":
                            case "MTSOORDER":
                            case "EORDER":
                            case "CADEX":
                            case "NRINFO":
                                VB.Shell("c:\\cmc\\ocsexe\\" + strExeFile + " " + (GstrDrugCode != "" ? GstrDrugCode + ",MFDS" : ""));
                                break;
                            case "BUSUGA":
                            case "RUAENT":
                            case "RUBENT":
                            case "RUTENT":
                                VB.Shell("c:\\cmc\\ocsexe\\" + strExeFile + " " + (GstrDrugCode != "" ? GstrDrugCode + ",HIRA" : ""));
                                break;
                            default:
                                VB.Shell("c:\\cmc\\ocsexe\\" + strExeFile);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void Dir_Check(string sDirPath, string strFile)
        {
            DirectoryInfo Dir = new DirectoryInfo(sDirPath);

            if (Dir.Exists == false)
            {
                Dir.Create();
            }
            else
            {
                FileInfo[] File = Dir.GetFiles(strFile);

                foreach (FileInfo file in File)
                {
                    file.Delete();
                }
            }
        }

        private void DOWNLOADnCREATE(string strArgPath, string strEXE, string strName = "", string strBUSE = "", string strGUBUN = "")
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strIP1 = clsCompuInfo.gstrCOMIP;
            string[] strIP = VB.Split(strIP1, ".");
            string strIP2 = "";
            string strLocal = "";
            string strPath = "";
            string strHost = "";

            for (i = 0; i < strIP.Length; i++)
            {
                strIP2 = strIP2 + VB.Val(strIP[i]).ToString("000") + ".";
            }

            strIP2 = VB.Mid(strIP2, 1, strIP2.Length - 1);

            if (strBUSE != "")
            {
                try
                {
                    SQL = "";
                    SQL = "SELECT * FROM " + ComNum.DB_ERP + "JAS_MASTER ";
                    SQL = SQL + ComNum.VBLF + "     WHERE IPADDR = '" + strIP2 + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND BUCODE = '" + strBUSE + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        
                        dt.Dispose();
                        dt = null;
                        return;
                    }

                    dt.Dispose();
                    dt = null;
                }
                catch (Exception ex)
                {
                    if (dt != null)
                    {
                        dt.Dispose();
                        dt = null;
                    }

                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                }
            }

            DirectoryInfo Dir = new DirectoryInfo(strArgPath.ToLower() + "\\" + strEXE);
            
            if (Dir.Exists == false)
            {
                if (strGUBUN != "FIRSTDIS")
                {
                    strEXE = strEXE.ToLower();
                    strArgPath = strArgPath.ToLower();
                }

                strLocal = strArgPath + "\\" + strEXE;
                
                Ftpedt FtpedtX = new Ftpedt();
                
                if (FtpedtX.FtpConnetBatch("192.168.100.33", "pcnfs", "pcnfs1") == false)
                {
                    ComFunc.MsgBox("FTP Server Connect ERROR !!!", "오류");
                    return;
                }

                if (strArgPath == "c:\\cmc\\exe")
                {
                    strPath = "/pcnfs/exe/" + strEXE;
                    strHost = "/pcnfs/exe";
                }
                else
                {
                    strPath = "/pcnfs/ocsexe/" + strEXE;
                    strHost = "/pcnfs/ocsexe";
                }
                
                if (FtpedtX.FtpDownload("192.168.100.33", "pcnfs", "pcnfs1", strLocal, strPath, strHost) == false)
                {
                    ComFunc.MsgBox("다운로드 실패", "종료");
                    FtpedtX.FtpDisConnetBatch();
                    FtpedtX = null;
                    return;
                }

                FtpedtX.FtpDisConnetBatch();
                FtpedtX = null;
            }

            //if (strName != "")
            //{
            //    VB.Shell(strArgPath + "\\" + strEXE);
            //}

            this.Close();
        }

        private bool CheckDel()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            bool rtnVal = false;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     CODE";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "     WHERE GUBUN = 'DRUG_DIF_교체'";
                SQL = SQL + ComNum.VBLF + "         AND CODE = 'DEL'";
                SQL = SQL + ComNum.VBLF + "         AND NAME = 'Y'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = true;
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }
    }
}
