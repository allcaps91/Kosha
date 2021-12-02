using ComDbB; //DB연결
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace ComBase
{
    public class clsVbEmr
    {
        //private string myIniFileName = "";
        //private string myFileReaded = "";
        //private bool BmyFileReaded = false;
        //private SortedList myFileLines = new SortedList();		// This collection contains all the lines by order
        //private SortedList mySectionKeysLines = new SortedList();
        //private long MaxFileLines = 0;

        enum iniLineType { empty, comment, section, keyValue };

        /// <summary>
        /// EMR exe 실행
        /// </summary>
        /// <param name="strPtNo">등록번호</param>
        /// <param name="strIdNumber">사번(비어있으면 로그인한 사번)</param>
        /// <param name="strPassWord">비밀번호(비어있으면 로그인한 비밀번호)</param>
        /// <param name="strStyle"></param>
        public static void EXECUTE_NewTextEmrView(string strPtNo, string strIdNumber = "", string strPassWord = "", string strStyle = "vbNormalFocus")
        {
            FileInfo nFILE = new FileInfo("C:\\PSMHEXE\\exenet\\EmrViewExe.exe");
            if (nFILE.Exists == false)
            {
                ComFunc.MsgBox("EmrViewExe.exe가 설치되지 않았습니다.", "오류");
                return;
            }

            if (string.IsNullOrWhiteSpace(strIdNumber))
            {
                strIdNumber = clsType.User.IdNumber;
            }

            if (string.IsNullOrWhiteSpace(strPassWord))
            {
                strPassWord = clsType.User.PasswordChar;
            }

            //프로세스 체크
            bool ActiveProc = false;
            System.Diagnostics.Process[] ProcessEx = System.Diagnostics.Process.GetProcessesByName("EmrViewExe");
            if (ProcessEx.Length > 0)
            {
                System.Diagnostics.Process[] Pro1 = System.Diagnostics.Process.GetProcessesByName("EmrViewExe");
                System.Diagnostics.Process CurPro = System.Diagnostics.Process.GetCurrentProcess();
                foreach (System.Diagnostics.Process Proc in Pro1)
                {
                    if (Proc.Id != CurPro.Id)
                    {
                        ActiveProc = true;
                        break;
                    }
                }
            }

            if (ActiveProc == true)
            {
                string strFIleNm = "C:\\cmc\\ocsexe\\EMRPTNO.ini";
                FileInfo pRevEmrFile = new FileInfo(strFIleNm);
                if (pRevEmrFile.Exists == true)
                {
                    pRevEmrFile.Delete();
                }

                using (StreamWriter sw = new StreamWriter(strFIleNm))
                {
                    sw.Write(strPtNo + "/" + strIdNumber);
                }
                return;
            }

            strStyle = strStyle.Replace("vb", "");
            VB.Shell("C:\\PSMHEXE\\exenet\\EmrViewExe.exe " + clsAES.AES(strIdNumber + "/" + strPassWord + "/" + strPtNo + "/"), strStyle);

        }


        /// <summary>
        /// BIT 영상 EMR 호출
        /// Author : 박웅규
        /// Create Date : 2017.08.01
        /// PSMHH\BaseFile\VbEMR.bas / EXECUTE_EMR
        /// </summary>
        /// <param name="strPtNo">등록번호</param>
        /// <param name="strSaBun">사번</param>
        public static void ExecuteEmr(PsmhDb pDbCon, string strPtNo, string strSaBun = "", string strStyle = "NormalFocus")
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            FileInfo nFILE = null;

            string strDept = "";
            string strAuto = "";
            string strAutoTray = "";

            nFILE = new FileInfo("C:\\cmc\\ocsviewer\\Autoupdate.exe");
            if (nFILE.Exists == false)
            {
                ComFunc.MsgBox("EMR Viewer가 설치되지 않았습니다.", "오류");
                return;
            }

            nFILE = null;

            string strFIleNm = "C:\\cmc\\ocsviewer\\anyviewer.ini";
            nFILE = new FileInfo(strFIleNm);

            if (nFILE.Exists == false)
            {
                return;
            }

            strAuto = "0";
            strAutoTray = "0";
            string strLine;

            using (StreamReader file = new StreamReader(strFIleNm))
            {
                while ((strLine = file.ReadLine()) != null)
                {
                    if (VB.InStr(strLine, "AUTOSHELL=") > 0)
                    {
                        strAuto = VB.Mid(strLine, VB.InStr(strLine, "=") + 1, 1);
                    }
                    if (VB.InStr(strLine, "AUTOTRAY=") > 0)
                    {
                        strAutoTray = VB.Mid(strLine, VB.InStr(strLine, "=") + 1, 1);
                    }
                }
                file.Close();
            }
               

            if (strPtNo != "")
            {
                //       '의사진료과표기하기위해 새로 읽어옴
                try
                {
                    SQL = "";
                    SQL = "SELECT DEPTCODE ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR";
                    SQL = SQL + ComNum.VBLF + "WHERE SABUN = '" + strSaBun + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strDept = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    }
                    else
                    {
                        strDept = "MD";
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
                    clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox(ex.Message);
                }

                if (strAuto == "1") //emrtray shell 실행
                {
                    FileInfo nFILE1 = new FileInfo("C:\\cmc\\EXE\\EMRTRAY.exe");
                    if (nFILE1.Exists == false)
                    {
                        nFILE1 = null;
                        return;
                    }
                    nFILE1 = null;

                    //프로세스 체크
                    bool ActiveProc = false;

                    System.Diagnostics.Process[] ProcessEx = System.Diagnostics.Process.GetProcessesByName("EMRTRAY");
                    if (ProcessEx.Length > 0)
                    {
                        System.Diagnostics.Process[] Pro1 = System.Diagnostics.Process.GetProcessesByName("EMRTRAY");
                        System.Diagnostics.Process CurPro = System.Diagnostics.Process.GetCurrentProcess();
                        foreach (System.Diagnostics.Process Proc in Pro1)
                        {
                            if (Proc.Id != CurPro.Id)
                            {
                                ActiveProc = true;
                            }
                        }
                    }
                    if (ActiveProc == false)
                    {
                        VB.Shell("C:\\cmc\\EXE\\EMRTRAY.exe");
                    }

                    string strFIleNmX = "C:\\cmc\\ocsviewer\\PATID.ini";
                    FileInfo pRevEmrFile = new FileInfo(strFIleNmX);
                    if (pRevEmrFile.Exists == true)
                    {
                        pRevEmrFile.Delete();
                    }

                    using (StreamWriter sw = new StreamWriter(strFIleNmX))
                    {
                        sw.Write(strPtNo + "/" + strDept + "/" + strSaBun);
                    }
                }
                else //shell로 실행
                {
                    //프로세스 체크
                    System.Diagnostics.Process[] ProcessEx = System.Diagnostics.Process.GetProcessesByName("OCSViewer");
                    if (ProcessEx.Length > 0)
                    {
                        System.Diagnostics.Process[] Pro1 = System.Diagnostics.Process.GetProcessesByName("OCSViewer");
                        System.Diagnostics.Process CurPro = System.Diagnostics.Process.GetCurrentProcess();
                        foreach (System.Diagnostics.Process Proc in Pro1)
                        {
                            if (Proc.Id != CurPro.Id)
                            {
                                Proc.Kill();
                                Application.DoEvents();
                                System.Threading.Thread.Sleep(500);
                                Application.DoEvents();
                                System.Threading.Thread.Sleep(500);
                                Application.DoEvents();
                                System.Threading.Thread.Sleep(500);
                                Application.DoEvents();
                            }
                        }
                    }

                    VB.Shell("C:\\cmc\\ocsviewer\\OCSViewer.exe " + strPtNo + "/" + clsType.User.IdNumber + "/" + strDept + "/001/1/*", "MaximizedFocus");
                }

            }
        }

        /// <summary>
        /// Old Text EMR Viewer를 실행한다.
        /// Author : 박웅규
        /// Create Date : 2017.08.01
        /// PSMHH\mtsEmr\clsCallEmrView.cls 함수 컨버젼 EXECUTE_TextEmrView
        /// </summary>
        /// <param name="strPtNo">등록번호</param>
        /// <param name="strIdNumber">사용자ID(strUseId, GnJobSabun)</param>
        /// <param name="strStyle">폼로드시 포커스(strFocus)</param>
        public static void EXECUTE_TextEmrView(string strPtNo, string strIdNumber = "", string strStyle = "vbNormalFocus")
        {
            FileInfo nFILE = null;
            nFILE = new FileInfo("C:\\cmc\\ocsexe\\mhemrviewer.exe");
            if (nFILE.Exists == false)
            {
                ComFunc.MsgBox("mhemrviewer.exe가 설치되지 않았습니다.", "오류");
                return;
            }
            nFILE = null;

            //프로세스 체크
            bool ActiveProc = false;
            System.Diagnostics.Process[] ProcessEx = System.Diagnostics.Process.GetProcessesByName("mhemrviewer");
            if (ProcessEx.Length > 0)
            {
                System.Diagnostics.Process[] Pro1 = System.Diagnostics.Process.GetProcessesByName("mhemrviewer");
                System.Diagnostics.Process CurPro = System.Diagnostics.Process.GetCurrentProcess();
                foreach (System.Diagnostics.Process Proc in Pro1)
                {
                    if (Proc.Id != CurPro.Id)
                    {
                        ActiveProc = true;
                    }
                }
            }

            if (ActiveProc == true)
            {
                string strFIleNm = "C:\\cmc\\ocsexe\\EMRPTNO.ini";
                FileInfo pRevEmrFile = new FileInfo(strFIleNm);
                if (pRevEmrFile.Exists == true)
                {
                    pRevEmrFile.Delete();
                }

                using (StreamWriter sw = new StreamWriter(strFIleNm))
                {
                    sw.Write(strPtNo + "/" + strIdNumber);
                }
                return;
            }
            strStyle = strStyle.Replace("vb", "");
            VB.Shell("C:\\cmc\\ocsexe\\mhemrviewer.exe " + strPtNo + "," + strIdNumber, strStyle);

        }

        /// <summary>
        /// Old Text EMR Viewer를 실행한다.
        /// Author : 박웅규
        /// Create Date : 2017.08.01
        /// PSMHH\mtsEmr\clsCallEmrView.cls 함수 컨버젼 EXECUTE_TextEmrViewEx
        /// </summary>
        /// <param name="strPtNo">등록번호</param>
        /// <param name="strIdNumber">사용자 id(strUseId, GnJobSabun)</param>
        /// <param name="strPara">파라메타</param>
        public static void EXECUTE_TextEmrViewEx(string strPtNo, string strIdNumber = "", string strPara = "")
        {
            #region //진료OCS
            //string strJobEmrName = ""; //외래 : EMROORDER, 입원 : EMRIORDER, 응급실 : EMREORDER
            //string strFrDate = "";
            //string strInoutCls = "O"; //외래 입원 구분(외래 : O , 입원 : I , 응급실 : 
            //string strPara = "";

            //#region //외래
            //strJobEmrName = "EMROORDER";
            //strFrDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
            //strInoutCls = "O";
            //strPara = strJobEmrName + "," + strInoutCls + "," + strFrDate + "," + clsOrdFunction.Pat.DeptCode.Trim() + "," + clsOrdFunction.Pat.DrCode.Trim();
            //#endregion

            //#region //입원
            //strJobEmrName = "EMRIORDER";
            //strFrDate = clsOrdFunction.Pat.INDATE.Replace("-", "");
            //strInoutCls = "I";
            //strPara = strJobEmrName + "," + strInoutCls + "," + strFrDate + "," + clsOrdFunction.Pat.DeptCode.Trim() + "," + clsOrdFunction.Pat.DrCode.Trim();
            //#endregion

            //#region //응급실
            //strJobEmrName = "EMREORDER";
            //strFrDate = clsOrdFunction.Pat.INDATE.Replace("-", "");
            //strInoutCls = "I";
            //strPara = strJobEmrName + "," + strInoutCls + "," + strFrDate + "," + clsOrdFunction.Pat.DeptCode.Trim() + "," + clsOrdFunction.Pat.DrCode.Trim();
            //#endregion

            //clsVbEmr.EXECUTE_TextEmrViewEx(strPano, (VB.Val(clsType.User.IdNumber)).ToString(), strPara);

            #endregion //진료OCS

            #region //Nrinfo
            //strJobEmrName = "NRINFO";
            //strFrDate = INDATE.Replace("-", "");
            //strInoutCls = "I";
            //strPara = strJobEmrName + "," + strInoutCls + "," + strFrDate + "," + DeptCode.Trim() + "," + DrCode.Trim();
            //clsVbEmr.EXECUTE_TextEmrViewEx(strPano, (VB.Val(clsType.User.IdNumber)).ToString(), strPara);
            #endregion



            FileInfo nFILE = null;
            nFILE = new FileInfo("C:\\cmc\\ocsexe\\mhemrviewer.exe");
            if (nFILE.Exists == false)
            {
                ComFunc.MsgBox("mhemrviewer.exe가 설치되지 않았습니다.", "오류");
                return;
            }
            nFILE = null;

            //프로세스 체크
            bool ActiveProc = false;
            System.Diagnostics.Process[] ProcessEx = System.Diagnostics.Process.GetProcessesByName("mhemrviewer");
            if (ProcessEx.Length > 0)
            {
                System.Diagnostics.Process[] Pro1 = System.Diagnostics.Process.GetProcessesByName("mhemrviewer");
                System.Diagnostics.Process CurPro = System.Diagnostics.Process.GetCurrentProcess();
                foreach (System.Diagnostics.Process Proc in Pro1)
                {
                    if (Proc.Id != CurPro.Id)
                    {
                        ActiveProc = true;
                    }
                }
            }
             
            string dirPath = "C:\\PSMHEXE\\LOG\\" + DateTime.Now.ToShortDateString() + "_EMRLog.log";
            if (strPara != "")
            {
                File.AppendAllText(dirPath, DateTime.Now + " " + strPtNo + "," + strIdNumber + "," + strPara);               
            }
            else
            {
                File.AppendAllText(dirPath, DateTime.Now + " " + strPtNo + "," + strIdNumber);
            }

            if (ActiveProc == true)
            {
                string strFIleNm = "C:\\cmc\\ocsexe\\EMRPTNO.ini";
                FileInfo pRevEmrFile = new FileInfo(strFIleNm);
                if (pRevEmrFile.Exists == true)
                {
                    pRevEmrFile.Delete();
                }

                using (StreamWriter sw = new StreamWriter(strFIleNm))
                {
                    if (strPara != "")
                    {
                        sw.Write(strPtNo + "/" + strIdNumber + "/" + strPara);
                    }
                    else
                    {
                        sw.Write(strPtNo + "/" + strIdNumber);
                    }

                }
                return;
            }

            string strStyle = "vbNormalFocus";
            strStyle = strStyle.Replace("vb", "");
            if (strPara != "")
            {
                VB.Shell("C:\\cmc\\ocsexe\\mhemrviewer.exe " + strPtNo + "," + strIdNumber + "," + strPara, strStyle);
            }
            else
            {
                VB.Shell("C:\\cmc\\ocsexe\\mhemrviewer.exe " + strPtNo + "," + strIdNumber, strStyle);
            }

        }

        /// <summary>
        /// Old Text EMR Viewer를 실행한다.
        /// Author : 박웅규
        /// Create Date : 2017.08.01
        /// PSMHH\mtsEmr\clsCallEmrView.cls 함수 컨버젼 EXECUTE_TextEmrViewEx2
        /// </summary>
        /// <param name="strPtNo">등록번호</param>
        /// <param name="strIdNumber">사용자 id(strUseId, GnJobSabun)</param>
        /// <param name="strPara">파라메타</param>
        public static void EXECUTE_TextEmrViewEx2(string strPtNo, string strIdNumber = "", string strPara = "")
        {
            FileInfo nFILE = null;
            nFILE = new FileInfo("C:\\cmc\\ocsexe\\mhchartviewer.exe");
            if (nFILE.Exists == false)
            {
                ComFunc.MsgBox("mhchartviewer가 설치되지 않았습니다.", "오류");
                return;
            }
            nFILE = null;

            //프로세스 체크
            bool ActiveProc = false;
            System.Diagnostics.Process[] ProcessEx = System.Diagnostics.Process.GetProcessesByName("mhchartviewer");
            if (ProcessEx.Length > 0)
            {
                System.Diagnostics.Process[] Pro1 = System.Diagnostics.Process.GetProcessesByName("mhchartviewer");
                System.Diagnostics.Process CurPro = System.Diagnostics.Process.GetCurrentProcess();
                foreach (System.Diagnostics.Process Proc in Pro1)
                {
                    if (Proc.Id != CurPro.Id)
                    {
                        ActiveProc = true;
                    }
                }
            }

            if (ActiveProc == true)
            {
                string strFIleNm = "C:\\cmc\\ocsexe\\EMRPTNO.ini";
                FileInfo pRevEmrFile = new FileInfo(strFIleNm);
                if (pRevEmrFile.Exists == true)
                {
                    pRevEmrFile.Delete();
                }

                using (StreamWriter sw = new StreamWriter(strFIleNm))
                {
                    if (strPara != "")
                    {
                        sw.Write(strPtNo + "/" + strIdNumber + "/" + strPara);
                    }
                    else
                    {
                        sw.Write(strPtNo + "/" + strIdNumber);
                    }

                }
                return;
            }

            string strStyle = "vbNormalFocus";
            strStyle = strStyle.Replace("vb", "");

            if (strPara != "")
            {
                VB.Shell("C:\\cmc\\ocsexe\\mhchartviewer.exe " + strPtNo + "," + strIdNumber + "," + strPara, strStyle);
            }
            else
            {
                VB.Shell("C:\\cmc\\ocsexe\\mhchartviewer.exe " + strPtNo + "," + strIdNumber, strStyle);
            }
        }

        /// <summary>
        /// Old Text EMR Viewer를 실행한다.
        /// Author : 박웅규
        /// Create Date : 2017.08.01
        /// PSMHH\mtsEmr\clsCallEmrView.cls 함수 컨버젼 EXECUTE_TextEmrViewCX
        /// </summary>
        /// <param name="strPtNo">등록번호</param>
        /// <param name="strIdNumber">사용자ID(strUseId, GnJobSabun)</param>
        public static void EXECUTE_TextEmrViewCX(string strPtNo, string strIdNumber = "")
        {
            FileInfo nFILE = null;
            nFILE = new FileInfo("C:\\cmc\\ocsexe\\mhchartviewer.exe");
            if (nFILE.Exists == false)
            {
                ComFunc.MsgBox("mhchartviewer가 설치되지 않았습니다.", "오류");
                return;
            }
            nFILE = null;

            //프로세스 체크
            bool ActiveProc = false;
            System.Diagnostics.Process[] ProcessEx = System.Diagnostics.Process.GetProcessesByName("mhchartviewer");
            if (ProcessEx.Length > 0)
            {
                System.Diagnostics.Process[] Pro1 = System.Diagnostics.Process.GetProcessesByName("mhchartviewer");
                System.Diagnostics.Process CurPro = System.Diagnostics.Process.GetCurrentProcess();
                foreach (System.Diagnostics.Process Proc in Pro1)
                {
                    if (Proc.Id != CurPro.Id)
                    {
                        ActiveProc = true;
                    }
                }
            }

            if (ActiveProc == true)
            {
                string strFIleNm = "C:\\cmc\\ocsexe\\EMRPTNOCX.ini";
                FileInfo pRevEmrFile = new FileInfo(strFIleNm);
                if (pRevEmrFile.Exists == true)
                {
                    pRevEmrFile.Delete();
                }

                using (StreamWriter sw = new StreamWriter(strFIleNm))
                {
                    sw.Write(strPtNo + "/" + strIdNumber);
                }
                return;
            }

            string strStyle = "vbNormalFocus";
            strStyle = strStyle.Replace("vb", "");

            VB.Shell("C:\\cmc\\ocsexe\\mhchartviewer.exe " + strPtNo + "," + strIdNumber, strStyle);
        }

        /// <summary>
        /// careplan을 실행한다
        /// Author : 박웅규
        /// Create Date : 2017.08.01
        /// PSMHH\mtsEmr\clsCallEmrView.cls 함수 컨버젼 CarePlan_View
        /// </summary>
        /// <param name="argPTNO">등록번호</param>
        /// <param name="ArgInDate">입원일자</param>
        /// <param name="argIPDNO">입원번호</param>
        /// <param name="argUSEID">사용자ID</param>
        public static void CarePlan_View(string argPTNO, string ArgInDate, string argIPDNO, string argUSEID)
        {
            FileInfo nFILE = null;
            nFILE = new FileInfo("C:\\cmc\\ocsexe\\careplan.exe");
            if (nFILE.Exists == false)
            {
                ComFunc.MsgBox("careplan.exe 가 설치되지 않았습니다.", "오류");
                return;
            }
            nFILE = null;

            //프로세스 체크
            bool ActiveProc = false;
            System.Diagnostics.Process[] ProcessEx = System.Diagnostics.Process.GetProcessesByName("careplan");
            if (ProcessEx.Length > 0)
            {
                System.Diagnostics.Process[] Pro1 = System.Diagnostics.Process.GetProcessesByName("careplan");
                System.Diagnostics.Process CurPro = System.Diagnostics.Process.GetCurrentProcess();
                foreach (System.Diagnostics.Process Proc in Pro1)
                {
                    if (Proc.Id != CurPro.Id)
                    {
                        ActiveProc = true;
                    }
                }
            }

            if (ActiveProc == true)
            {
                string strFIleNm = "C:\\cmc\\ocsexe\\careplan.ini";
                FileInfo pRevEmrFile = new FileInfo(strFIleNm);
                if (pRevEmrFile.Exists == true)
                {
                    pRevEmrFile.Delete();
                }

                using (StreamWriter sw = new StreamWriter(strFIleNm))
                {
                    sw.Write(argPTNO + "|" + ArgInDate + "|" + argIPDNO + "|" + argUSEID);
                }
                return;
            }

            string strStyle = "vbNormalFocus";
            strStyle = strStyle.Replace("vb", "");

            VB.Shell("C:\\cmc\\ocsexe\\careplan.exe " + (argPTNO + "|" + ArgInDate + "|" + argIPDNO + "|" + argUSEID), strStyle);
        }




    }
}
