
using ComHpcLibB;
/// <summary>
/// Description     : 건진센터 공용모듈 / return 값이 없거나 단순 Function
/// Author          : 김민철
/// Create Date     : 2019-07-12
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
namespace ComHpcLibB
{
    using ComBase;
    using ComDbB;
    using System;
    using System.Data;
    using System.Windows.Forms;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Service;
    using System.Collections.Generic;
    using ComBase.Controls;
    using System.IO;
    using System.Drawing;
    using ComHpcLibB.Model;
    using Microsoft.Win32;
    using ComEmrBase;
    using System.Threading.Tasks;
    using System.Threading;

    public class clsHcFunc
    {
        ComHpcLibBService       comHpcLibBService                 = null;
        HicBcodeService         hicBcodeService                   = null;
        HicExcodeReferService   hicExcodeReferService             = null;
        HicCodeService          hicCodeService                    = null;
        HicExcodeService        hicExcodeService                  = null;
        BasPcconfigService      basPcconfigService                = null;
        HeaJepsuService         heaJepsuService                   = null;
        HeaAutopanService       heaAutopanService                 = null;
        HeaAutopanLogicService  heaAutopanLogicService            = null;
        HicResultService        hicResultService                  = null;
        HicResultExCodeService  hicResultExCodeService            = null;
        HeaAutoPanMatchResultService heaAutoPanMatchResultService = null;
        InsaMstService          insaMstService                    = null;
        InsaMsthService         insaMsthService                   = null;
        OcsDoctorService        ocsDoctorService                  = null;
        BasScheduleService      basScheduleService                = null;
        EtcJupmstService        etcJupmstService                  = null;
        HicPatientService       hicPatientService                 = null;
        HicJepsuService         hicJepsuService                   = null;
        HicConsentService       hicConsentService                 = null;
        HicIeMunjinViewService  hicIeMunjinViewService            = null;
        HicIeMunjinNewService   hicIeMunjinNewService             = null;
        
        ComFunc cf = new ComFunc();
        Ftpedt ftp = new Ftpedt();
        clsHaBase hb = new clsHaBase();
        clsHcVariable chv = new clsHcVariable();


        public clsHcFunc()
        {
            comHpcLibBService            = new ComHpcLibBService();
            hicBcodeService              = new HicBcodeService();
            hicExcodeReferService        = new HicExcodeReferService();
            hicCodeService               = new HicCodeService();
            hicExcodeService             = new HicExcodeService();
            basPcconfigService           = new BasPcconfigService();
            heaJepsuService              = new HeaJepsuService();
            heaAutopanLogicService       = new HeaAutopanLogicService();
            hicResultService             = new HicResultService();
            hicResultExCodeService       = new HicResultExCodeService();
            heaAutoPanMatchResultService = new HeaAutoPanMatchResultService();
            insaMstService               = new InsaMstService();
            insaMsthService              = new InsaMsthService();
            ocsDoctorService             = new OcsDoctorService();
            basScheduleService           = new BasScheduleService();
            etcJupmstService             = new EtcJupmstService();
            hicPatientService            = new HicPatientService();
            hicJepsuService              = new HicJepsuService();
            hicConsentService            = new HicConsentService();
            hicIeMunjinViewService       = new HicIeMunjinViewService();
            hicIeMunjinNewService        = new HicIeMunjinNewService();
        }

        /// <summary>
        /// 폼 중복 로드 방지(폼이 로드 되어 있는지 체크)
        /// </summary>
        /// <param name="strFormName"></param>
        /// <returns></returns>
        public bool OpenForm_Check(string strFormName)
        {
            bool rtnVal = false;

            foreach (Form Form in Application.OpenForms) //떠있는지 체크
            {
                if (Form.Name == strFormName)
                {
                    Form.Activate();
                    Form.BringToFront();
                    rtnVal = true;
                    return rtnVal;
                }
            }

            return rtnVal;
        }

        public Form OpenForm_Check_Return(string strFormName)
        {
            Form rtnVal = null;

            foreach (Form Form in Application.OpenForms) //떠있는지 체크
            {
                if (Form.Name == strFormName)
                {
                    rtnVal = Form;
                    return rtnVal;
                }
            }

            return rtnVal;
        }

        public void Diplay_IE_Munjin(frmHcSangInternetMunjinView frmIEMunjin, string strFormName, long fnWRTNO, string fstrJepDate, string pTNO, string fstrGjJong, string fstrROWID)
        {
            Form frmMunJinView = OpenForm_Check_Return(strFormName);

            if (frmMunJinView != null)
            {
                frmMunJinView.Close();
                frmMunJinView.Dispose();
                frmMunJinView = null;
            }

            frmIEMunjin = new frmHcSangInternetMunjinView(fnWRTNO, fstrJepDate, pTNO, fstrGjJong, fstrROWID);
            frmIEMunjin.Show();
            frmIEMunjin.WindowState = FormWindowState.Minimized;

        }

        public void Diplay_IE_Munjin_New(frmHcIEMunjinVIew frmIEMunjin, string argJepDate, string argPtno, string argGjJong)
        {
            Form frmMunJinView = OpenForm_Check_Return("frmHcIEMunjinVIew");

            if (frmMunJinView != null)
            {
                frmMunJinView.Close();
                frmMunJinView.Dispose();
                frmMunJinView = null;
            }

            string strFrDate = VB.Left(clsPublic.GstrSysDate, 4) + "-01-01";
            string strToDate = DateTime.Parse(argJepDate).AddDays(1).ToShortDateString();
            string strROWID = "";
            string strForm = "";
            string strViewId = "";
            string strOSVer = "";
            string strIEVer = "";

            int result = 0;
            long FnMunID = 0;

            //인터넷문진표(New)
            HIC_IE_MUNJIN_NEW list = hicIeMunjinNewService.GetItembyPtNoJepDateGjJong(argPtno, strFrDate, strToDate, argGjJong);

            if (!list.IsNullOrEmpty())
            {
                strROWID = list.ROWID.Trim();
                strForm = list.RECVFORM.Trim();
                if (VB.InStr(strForm, "12001") > 0)
                {
                    strForm = strForm.Replace("12001", "12001,12005");
                }

                if (hicIeMunjinViewService.GetAllbyViewKey(strROWID, clsPublic.GstrSysDate) == 0)
                {
                    clsDB.setBeginTran(clsDB.DbCon);

                    result = hicIeMunjinViewService.Insert(clsPublic.GstrSysDate, strROWID);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("인터넷 문진 저장 중 오류발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    clsDB.setCommitTran(clsDB.DbCon);
                }

                Application.DoEvents();

                strOSVer = fn_Find_OS_version().ToUpper();

                Thread.Sleep(1000);

                strViewId = hicIeMunjinViewService.GetViewIdbyViewKey(strROWID, clsPublic.GstrSysDate);

                if (!strViewId.IsNullOrEmpty())
                {
                    FnMunID = strViewId.To<long>();
                }

                if (FnMunID == 0)
                {
                    return;
                }
                else
                {
                    var ieVersion = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Internet Explorer").GetValue("Version");

                    //XP,VISTA는 IE Old버전으로 설정
                    if (VB.InStr(strOSVer, "XP") > 0) { strIEVer = "8.0"; }
                    if (VB.InStr(strOSVer, "VISTA") > 0) { strIEVer = "8.0"; }
                    if (VB.InStr(strOSVer, "98") > 0) { strIEVer = "8.0"; }
                    if (VB.InStr(strOSVer, "10") > 0) { strIEVer = "11.0"; }
                    else { strIEVer = VB.Left(ieVersion.To<string>(), 1) + ".0"; }

                    frmIEMunjin = new frmHcIEMunjinVIew(FnMunID, strForm, argPtno);
                    frmIEMunjin.Show();

                    ComFunc.KillProc("friendly omr.exe");
                }
            }
        }

        /// <summary>
        /// 콤보박스에 BCode 항목을 추가한다.
        /// </summary>
        /// <param name="ArgCombobox"></param>
        /// <param name="argGubun"></param>
        /// <param name="ArgClear"></param>
        /// <param name="ArgTYPE"></param>
        /// <param name="ArgNULL"></param>
        /// <param name="ArgTable"></param>
        public void Combo_HCode_SET(PsmhDb pDbCon, ComboBox ArgCombobox, string argGubun, bool ArgClear, int ArgTYPE, string ArgTable, string ArgNULL = "")
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";
            int i = 0;

            string strTable = "HIC_CODE";       //기본 Table

            if (ArgClear == true)
            {
                ArgCombobox.Items.Clear();
            }

            if (ArgTable != "")
            {
                strTable = ArgTable;
            }

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT Sort,Code,Name                    ";
            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + strTable;
            SQL = SQL + ComNum.VBLF + " WHERE 1 = 1                             ";
            SQL = SQL + ComNum.VBLF + "   AND Gubun = '" + argGubun + "'        ";
            SQL = SQL + ComNum.VBLF + "   AND (GBDEL IS NULL OR GBDEL = '')     ";
            SQL = SQL + ComNum.VBLF + "ORDER BY Sort,Code                       ";

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                if (ArgNULL != "N")
                {
                    ArgCombobox.Items.Add(" ");
                }

                if (Dt.Rows.Count > 0)
                {

                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        if (ArgTYPE == 1)
                        {
                            ArgCombobox.Items.Add(Dt.Rows[i]["Code"].ToString().Trim() + "." + Dt.Rows[i]["Name"].ToString().Trim());
                        }

                        else if (ArgTYPE == 2)
                        {
                            ArgCombobox.Items.Add(Dt.Rows[i]["Code"].ToString().Trim());
                        }

                        else if (ArgTYPE == 3)
                        {
                            ArgCombobox.Items.Add(Dt.Rows[i]["Name"].ToString().Trim());
                        }
                    }
                }
                else
                {
                    ComFunc.MsgBox("자료가 없습니다.");
                }

                Dt.Dispose();
                Dt = null;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message, "에러발생");

                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }
            }
        }

        public void fn_ClearMemory(Form FormName)
        {
            FormName.Dispose();
            FormName = null;
            clsApi.FlushMemory();
        }

        public void ComboJong_AddItem_HIC(PsmhDb pDbCon, ComboBox Argcbo, string argGubun)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";
            int i = 0;

            string strBun = string.Empty;

            switch (argGubun)
            {
                case "JONG1": strBun = "1"; break;
                case "JONG2": strBun = "2"; break;
                case "JONG3": strBun = "3"; break;
                case "JONG5": strBun = "6"; break;
                default:
                    break;
            }

            Argcbo.Items.Clear();

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT Code,Name                         ";
            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "HIC_EXJONG  ";
            SQL = SQL + ComNum.VBLF + " WHERE 1 = 1                             ";
            SQL = SQL + ComNum.VBLF + "   AND BUN = '" + strBun + "'            ";
            SQL = SQL + ComNum.VBLF + "ORDER BY Code                            ";

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                
                if (Dt.Rows.Count > 0)
                {

                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        Argcbo.Items.Add(Dt.Rows[i]["Code"].ToString().Trim() + "." + Dt.Rows[i]["Name"].ToString().Trim());
                    }
                }
               
                Dt.Dispose();
                Dt = null;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message, "에러발생");

                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }
            }

        }

        public void ComboJong_AddItem_HEA(PsmhDb pDbCon, ComboBox Argcbo)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";
            int i = 0;
            
            Argcbo.Items.Clear();

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT Code,Name                         ";
            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "HEA_EXJONG  ";
            SQL = SQL + ComNum.VBLF + " WHERE 1 = 1                             ";
            SQL = SQL + ComNum.VBLF + "ORDER BY Code                            ";

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                if (Dt.Rows.Count > 0)
                {

                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        Argcbo.Items.Add(Dt.Rows[i]["Code"].ToString().Trim() + "." + Dt.Rows[i]["Name"].ToString().Trim());
                    }
                }

                Dt.Dispose();
                Dt = null;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message, "에러발생");

                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }
            }

        }

        /// <summary>
        /// Description : 검진 기초코드 조회
        /// Author : 김민철
        /// Create Date : 2019.01.22
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgGubun"></param>
        /// <param name="ArgCode"></param>
        /// <returns></returns>
        /// <seealso cref="HcBas.bas / READ_HIC_CODE"/>        
        public string READ_HIC_CODE(PsmhDb pDbCon, string ArgGubun, string ArgCode)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Name                              ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "HIC_CODE    ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1                             ";
                SQL = SQL + ComNum.VBLF + "   AND GUBUN = '" + ArgGubun + "'        ";
                SQL = SQL + ComNum.VBLF + "   AND CODE = '" + ArgCode + "'          ";
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (Dt.Rows.Count > 0)
                {
                    rtnVal = Dt.Rows[0]["Name"].ToString().Trim();
                }

                Dt.Dispose();
                Dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message, "에러발생");

                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }
                return "";
            }
        }

        #region vbHic자료사전.bas(이상훈)
        /// <summary>
        /// 프로그램에서 필요한 설정값을 읽어 Global 변수에 보관하는 작업
        /// Form_Load에서 한번만 처리하면 됩니다.
        /// </summary>
        public void SET_자료사전_VALUE()
        {
            string strTemp = "";

            B01_SET_Sangdam_Doctor();
            B01_SET_JONGGUM_Sabun();
            B01_SET_ENDO_ExCode();
            B02_GET_Panjeng_DrNo();
            B03_GET_Basic_Setup();
            B04_GET_NOT_PATIENT();
            B05_GET_CHEST_XRAY();
            B06_GET_위내시경코드();
            B07_GET_대장내시경코드();
            B08_GET_MisuAdminSabun();
            B09_GET_JUMIN_ALL();
            G36_SET_Night_Code_List();
            G37_SET_Doct_Exam();
            G38_SET_Result_Not_Check();
            G39_SET_HEMS_2차특수수납항목();


            //일반건진 관리자 사번
            clsHcVariable.GbHicAdminSabun = false;
            strTemp = hicBcodeService.GetRowIdbySabun(clsType.User.IdNumber.To<long>());

            if (!strTemp.IsNullOrEmpty())
            {
                clsHcVariable.GbHicAdminSabun = true;
            }

            //일반건진 접수관리자 사번(접수직원)
            clsHcVariable.GbHicJupsuAdminSabun = false;
            strTemp = hicBcodeService.GetRowIdbySabunJupsu(clsType.User.IdNumber.To<long>());

            if (!strTemp.IsNullOrEmpty())
            {
                clsHcVariable.GbHicJupsuAdminSabun = true;
            }

            //종합건진 관리자 사번
            clsHcVariable.GbHeaAdminSabun = false;
            strTemp = hicBcodeService.GetHeaRowIdbySabun(clsType.User.IdNumber.To<long>());

            if (!strTemp.IsNullOrEmpty())
            {
                clsHcVariable.GbHeaAdminSabun = true;
            }
        }

        private void B01_SET_JONGGUM_Sabun()
        {
            //인사마스타에 부서코드 '044510'만 종검직원으로 설정함
            string strBuse = comHpcLibBService.GetInsaMstBuseBySabun(clsType.User.IdNumber);

            if (strBuse == "044510")
            {
                clsHcVariable.B01_JONGGUM_SABUN = true;
            }

            string strTemp = hicBcodeService.GetCodeNamebyGubunCode("HEA_종검예약가능사번", clsType.User.IdNumber);

            if (!strTemp.IsNullOrEmpty())
            {
                clsHcVariable.B01_JONGGUM_SABUN = true;
            }
        }

        /// <summary>
        /// 01.상담의사 단축키 설정
        /// </summary>
        public void B01_SET_Sangdam_Doctor()
        {
            int nRead = 0;
            string strCODE = "";
            string strName = "";
            string strData = "";
            string strList1 = "";
            string strList2 = "";

            string strGubun = "HIC_상담의사단축키";

            List<HIC_BCODE> list = hicBcodeService.GetCodeNamebyBcode(strGubun);
            string[] arrCode = list.GetStringArray("CODE");
            string[] arrName = list.GetStringArray("NAME");

            nRead = list.Count;
            for (int i = 0; i < nRead - 1; i++)
            {
                strCODE = arrCode[i];
                strName = arrName[i];
                strList1 += strCODE + "." + VB.Pstr(strName, "(", 1) + " ";
                strList2 += strCODE + "=" + VB.STRCUT(strName, "(", ")") + ",";
            }

            clsHcVariable.B01_SANGDAM_DRLIST = strList1;
            clsHcVariable.B01_SANGDAM_DRNO = strList2;
        }

        public long B01_GET_Sangdam_DrNo(Keys argKeyCode)
        {
            long rtnVal = 0;
            long nDrNo = 0;

            switch (argKeyCode)
            {
                case Keys.F3:
                    nDrNo = long.Parse(VB.STRCUT(clsHcVariable.B01_SANGDAM_DRNO, "F3=", ","));
                    break;
                case Keys.F4:
                    nDrNo = long.Parse(VB.STRCUT(clsHcVariable.B01_SANGDAM_DRNO, "F4=", ","));
                    break;
                case Keys.F5:
                    nDrNo = long.Parse(VB.STRCUT(clsHcVariable.B01_SANGDAM_DRNO, "F5=", ","));
                    break;
                case Keys.F6:
                    nDrNo = long.Parse(VB.STRCUT(clsHcVariable.B01_SANGDAM_DRNO, "F6=", ","));
                    break;
                case Keys.F7:
                    nDrNo = long.Parse(VB.STRCUT(clsHcVariable.B01_SANGDAM_DRNO, "F7=", ","));
                    break;
                case Keys.F8:
                    nDrNo = long.Parse(VB.STRCUT(clsHcVariable.B01_SANGDAM_DRNO, "F8=", ","));
                    break;
                case Keys.F9:
                    nDrNo = long.Parse(VB.STRCUT(clsHcVariable.B01_SANGDAM_DRNO, "F9=", ","));
                    break;
                case Keys.F10:
                    nDrNo = long.Parse(VB.STRCUT(clsHcVariable.B01_SANGDAM_DRNO, "F10=", ","));
                    break;
                case Keys.F11:
                    nDrNo = long.Parse(VB.STRCUT(clsHcVariable.B01_SANGDAM_DRNO, "F11=", ","));
                    break;
                case Keys.F12:
                    nDrNo = long.Parse(VB.STRCUT(clsHcVariable.B01_SANGDAM_DRNO, "F12=", ","));
                    break;
                default:
                    nDrNo = 0;
                    break;
            }

            rtnVal = nDrNo;

            return rtnVal;
        }

        /// <summary>
        /// 01.B01_ENDO_EXCODE
        /// </summary>
        public void B01_SET_ENDO_ExCode()
        {
            int nRead = 0;
            string strCode = "";
            string strList = "";

            string strGubun = "HEA_내시경검사코드";

            List<HIC_BCODE> list = hicBcodeService.GetCodeNamebyBcode(strGubun);
            string[] arrCode = list.GetStringArray("CODE");

            nRead = list.Count;
            for (int i = 0; i <= nRead - 1; i++)
            {
                strCode = arrCode[i];
                strList += strCode + ",";
            }

            if (strList != "")
            {
                strList = VB.Left(strList, strList.Length - 1);
            }

            clsHcVariable.B01_ENDO_EXCODE = strList;
        }

        /// <summary>
        /// B02.로그인한 사번으로 판정할 의사면허번호 목록을 생성함
        /// </summary>
        public void B02_GET_Panjeng_DrNo()
        {
            int nRead = 0;
            string strData = "";
            string strTemp = "";
            long nDrNo = 0;
            string strDrNo = "";
            string strDrSabun = "";

            string strGubun = "HIC_퇴직의사판정설정(면허번호)";

            //본인의 면허번호를 읽음
            if (clsHcVariable.GnHicLicense == 0)
            {
                clsHcVariable.B02_PANJENG_DRNO = "";
                clsHcVariable.B02_PANJENG_SABUN = "";
                return;
            }

            strDrNo = string.Format("{0:#0}", clsHcVariable.GnHicLicense) + ",";

            //자료사전의 정보를 읽음
            List<HIC_BCODE> list = hicBcodeService.GetCodeNamebyBcode(strGubun, string.Format("{0:#0}", clsHcVariable.GnHicLicense));
            string[] arrName = list.GetStringArray("NAME");

            nRead = list.Count;
            for (int i = 0; i < nRead - 1; i++)
            {
                strData = arrName[i];
                strTemp = VB.STRCUT(strData, ":", "(");
                if (strTemp == "ALL")
                {
                    strDrNo = "ALL";
                    break;
                }
                strDrNo += strTemp + ",";
            }

            if (VB.Right(strDrNo, 1) == ",")
            {
                strDrNo = VB.Left(strDrNo, strDrNo.Length - 1);
            }
            clsHcVariable.B02_PANJENG_DRNO = strDrNo;

            strDrSabun = string.Format("{0:#0}", clsPublic.GnJobSabun) + ",";
            //상담의사
            strGubun = "HIC_퇴직의사판정설정(사번)";
            List<HIC_BCODE> list2 = hicBcodeService.GetCodeNamebyBcode(strGubun, string.Format("{0:#0}", clsHcVariable.GnHicLicense));
            string[] arrName2 = list2.GetStringArray("NAME");

            nRead = list2.Count;
            for (int i = 0; i < nRead - 1; i++)
            {
                strData = arrName[i];
                strTemp = VB.STRCUT(strData, ":", "(");
                if (strTemp == "ALL")
                {
                    strDrSabun = "ALL";
                    break;
                }
                strDrSabun += strTemp + ",";
            }

            if (VB.Right(strDrSabun, 1) == ",")
            {
                strDrSabun = VB.Left(strDrSabun, strDrSabun.Length - 1);
            }
            clsHcVariable.B02_PANJENG_SABUN = strDrSabun;
        }

        /// <summary>
        /// B03.기본설정값을 읽음
        /// </summary>
        public void B03_GET_Basic_Setup()
        {
            int nRead = 0;
            string strData = "";
            string strTemp = "";
            long nDrNO = 0;
            string strDrNo = "";

            string strGubun = "BAS_환경설정값";

            clsHcVariable.B03_DNT_OPD_JIN = false;

            //자료사전의 정보를 읽음
            List<HIC_BCODE> list = hicBcodeService.GetCodeNamebyBcode(strGubun, "");
            string[] arrName = list.GetStringArray("NAME");
            string[] arrCode = list.GetStringArray("CODE");

            nRead = list.Count;

            for (int i = 0; i < nRead - 1; i++)
            {
                strData = arrName[i];
                strTemp = VB.STRCUT(strData, "=", "");

                switch (arrCode[i])
                {
                    case "1001":    //구강검진 외래치과 진료
                        if (strTemp == "Y")
                        {
                            clsHcVariable.B03_DNT_OPD_JIN = true;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// B04.BAS_PATIENT에서 제외할 환자성명(중복차트 관련)
        /// </summary>
        public void B04_GET_NOT_PATIENT()
        {
            int nRead = 0;
            string strData = "";
            //string strResult = "";
            List<string> strResult = new List<string>();

            string strGubun = "BAS_환자마스타제외성명";

            clsHcVariable.B04_NOT_PATIENT.Clear();
            strResult.Clear();

            //자료사전의 정보를 읽음
            List<HIC_BCODE> list = hicBcodeService.GetCodeNamebyBcode(strGubun, "");
            string[] arrName = list.GetStringArray("NAME");
            string[] arrCode = list.GetStringArray("CODE");

            nRead = list.Count;
            for (int i = 0; i < nRead - 1; i++)
            {
                strData = arrName[i];
                if (strData != "")
                {
                    strResult.Add(strData);
                    //strResult += strData + ",";
                }
            }
            //if (strResult != null)
            //{
            //    strResult = VB.Left(strResult, strResult.Length - 1);
            //}

            clsHcVariable.B04_NOT_PATIENT = strResult;
        }

        /// <summary>
        /// B05.흉부방사선 코드 목록
        /// </summary>
        public void B05_GET_CHEST_XRAY()
        {
            int nRead = 0;
            string strData = "";
            string strResult = "";

            string strGubun = "HIC_흉부방사선목록";

            clsHcVariable.B05_CHEST_XRAY = "";
            strResult = "";

            //자료사전의 정보를 읽음
            List<HIC_BCODE> list = hicBcodeService.GetCodeNamebyBcode(strGubun, "");
            string[] arrName = list.GetStringArray("NAME");

            nRead = list.Count;
            for (int i = 0; i < nRead - 1; i++)
            {
                strData = arrName[i];
                if (strData != "")
                {
                    strResult += strData + ",";
                }
            }
            if (strResult != "")
            {
                strResult = VB.Left(strResult, strResult.Length - 1);
            }

            clsHcVariable.B05_CHEST_XRAY = strResult;
        }

        /// <summary>
        /// 06.위내시경코드
        /// </summary>
        public void B06_GET_위내시경코드()
        {
            int nRead = 0;
            string strData = "";
            string strResult = "";

            string strGubun = "HIC_위내시경코드";

            clsHcVariable.B06_위내시경코드 = "";
            strResult = "";

            //자료사전의 정보를 읽음
            List<HIC_BCODE> list = hicBcodeService.GetCodeNamebyBcode(strGubun, "");
            string[] arrName = list.GetStringArray("NAME");

            nRead = list.Count;
            for (int i = 0; i < nRead - 1; i++)
            {
                strData = arrName[i];
                if (strData != "")
                {
                    strResult += strData + ",";
                }
            }
            if (strResult != "")
            {
                strResult = VB.Left(strResult, strResult.Length - 1);
            }

            clsHcVariable.B06_위내시경코드 = strResult;
        }

        /// <summary>
        /// 07.대장내시경코드
        /// </summary>
        public void B07_GET_대장내시경코드()
        {
            int nRead = 0;
            string strData = "";
            string strResult = "";

            string strGubun = "HIC_대장내시경코드";

            clsHcVariable.B07_대장내시경코드 = "";
            strResult = "";

            //자료사전의 정보를 읽음
            List<HIC_BCODE> list = hicBcodeService.GetCodeNamebyBcode(strGubun, "");
            string[] arrName = list.GetStringArray("NAME");

            nRead = list.Count;
            for (int i = 0; i < nRead - 1; i++)
            {
                strData = arrName[i];
                if (strData != "")
                {
                    strResult += strData + ",";
                }
            }
            if (strResult != "")
            {
                strResult = VB.Left(strResult, strResult.Length - 1);
            }

            clsHcVariable.B07_대장내시경코드 = strResult;
        }

        /// <summary>
        /// 08.미수관리 수정,삭제 관리자 사번
        /// </summary>
        public void B08_GET_MisuAdminSabun()
        {
            int nRead = 0;
            string strData = "";
            string strResult = "";

            string strGubun = "MISU_관리자사번";

            clsHcVariable.B08_MisuAdminSabun = "";
            strResult = ",";

            //자료사전의 정보를 읽음
            List<HIC_BCODE> list = hicBcodeService.GetCodeNamebyBcode(strGubun, "");
            string[] arrCode = list.GetStringArray("CODE");

            nRead = list.Count;
            for (int i = 0; i < nRead - 1; i++)
            {
                strData = arrCode[i];
                if (strData != "")
                {
                    strResult += strData + ",";
                }
            }
            if (strResult != "")
            {
                strResult = VB.Left(strResult, strResult.Length - 1);
            }

            clsHcVariable.B08_MisuAdminSabun = strResult;
        }

        /// <summary>
        /// 09.주민등록번호 모두 표시할 사번
        /// </summary>
        public void B09_GET_JUMIN_ALL()
        {
            string strGubun = "BAS_주민등록표시할사번";

            clsHcVariable.B09_JUMIN_ALL = false;

            //자료사전의 정보를 읽음
            List<HIC_BCODE> list = hicBcodeService.GetCodeNamebyBcode(strGubun, "");
            string[] arrCode = list.GetStringArray("CODE");

            if (list.Count > 0)
            {
                clsHcVariable.B09_JUMIN_ALL = true;
            }
        }

        /// <summary>
        /// 36.야간작업 묶음코드 목록
        /// </summary>
        public void G36_SET_Night_Code_List()
        {
            int nRead = 0;
            //string strData = "";
            List<string> strData = new List<string>();

            string strGubun = "36";

            clsHcVariable.G36_NIGHT_CODE.Clear();

            //자료사전의 정보를 읽음
            List<HIC_CODE> list = hicCodeService.GetCodeNamebyGubun(strGubun);
            //string[] arrCode = list.GetStringArray("CODE");

            nRead = list.Count;
            strData.Clear();

            if (nRead > 0)
            {                   
                for (int i = 0; i < nRead; i++)
                {
                    strData.Add(list[i].CODE.Trim());
                }
            }

            clsHcVariable.G36_NIGHT_CODE = strData;
        }

        /// <summary>
        /// 37.판정의사가 직접 입력하는 검사코드 목록
        /// </summary>
        public void G37_SET_Doct_Exam()
        {
            int nRead = 0;
            List<string> strData = new List<string>();

            string strGubun = "37";

            clsHcVariable.G37_DOCT_ENTCODE.Clear();

            //자료사전의 정보를 읽음
            List<HIC_CODE> list = hicCodeService.GetListCodebyGubun(strGubun);
            string[] arrCode = list.GetStringArray("CODE");

            nRead = list.Count;
            strData.Clear();
            for (int i = 0; i < nRead; i++)
            {
                //strData.Add(arrCode[i]);
                strData.Add(arrCode[i].Trim());
            }

            clsHcVariable.G37_DOCT_ENTCODE = strData;
        }

        /// <summary>
        /// 38.검사결과입력완료 미체크 항목
        /// </summary>
        public void G38_SET_Result_Not_Check()
        {
            int nRead = 0;
            string strData = "";

            string strGubun = "38";

            clsHcVariable.G38_RESULT_NOT_CHECK = "";

            //자료사전의 정보를 읽음
            List<HIC_BCODE> list = hicBcodeService.GetCodeNamebyBcode(strGubun, "");
            string[] arrCode = list.GetStringArray("CODE");

            nRead = list.Count;
            strData = "";
            for (int i = 0; i < nRead - 1; i++)
            {
                strData += arrCode[i] + ",";
            }
            if (strData != "")
            {
                strData = VB.Left(strData, strData.Length - 1);
            }

            clsHcVariable.G38_RESULT_NOT_CHECK = strData;
        }

        /// <summary>
        /// 39.HEMS 특수2차 수납항목
        /// </summary>
        public void G39_SET_HEMS_2차특수수납항목()
        {
            int nRead = 0;
            string strData = "";

            string strGubun = "HEMS_2차특수수납항목";

            clsHcVariable.G39_HEMS_2차특수수납항목 = "";

            //자료사전의 정보를 읽음
            List<HIC_BCODE> list = hicBcodeService.GetCodeNamebyBcode(strGubun, "");
            string[] arrCode = list.GetStringArray("CODE");

            nRead = list.Count;
            strData = "";
            for (int i = 0; i < nRead - 1; i++)
            {
                strData += arrCode[i] + ",";
            }
            if (strData != "")
            {
                strData = VB.Left(strData, strData.Length - 1);
            }

            clsHcVariable.G39_HEMS_2차특수수납항목 = strData;
        }
        #endregion

        #region VbHicPFT.bas(이상훈)
        public void PFTFILE_DBToFile(string argRowId, string strViewerExe = "", int Inx = 0)
        {
            if (clsVbfunc.GetFile("C:\\WINDOWS\\SYSTEM32\\SHIMGVW.DLL") == "")
            {
                MessageBox.Show("WINDOWS IMAGE Viewer가 설치되지 안았습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Ftpedt FtpedtX = new Ftpedt();
            MemoryStream stream = null;
            Bitmap image = null;

            string strFileName = "";
            string strAudioFile = "";
            string strRemotePath = "";
            string strFTP = "";
            string strLocalFilePass = @"c:\CMC\";
            string strLocalFileName = string.Format("{0:#0}", Inx) + ".jpg";

            strFileName = @"c:\CMC\ETC_" + string.Format("{0:#0}", Inx) + ".jpg";
            strFTP = "";

            //FTP저장체크
            ETC_JUPMST list = etcJupmstService.GetIetmbyRowId(argRowId);

            if (!list.IsNullOrEmpty())
            {
                if (strFTP == "Y")
                {
                    strRemotePath = "/data/ocs_etc/" + list.RDATE + "/" + list.FILEPATH.Trim();

                    //2014-11-18 서버에서 PC로 파일을 다운로드함
                    clsHcVariable.FTP_Pass = ftp.READ_FTP(clsDB.DbCon, clsHcVariable.FTP_IpAddr, clsHcVariable.FTP_User);
                    ftp.FtpDownloadEx(clsHcVariable.FTP_IpAddr, clsHcVariable.FTP_User, clsHcVariable.FTP_Pass, strLocalFilePass, strRemotePath + strLocalFileName, strRemotePath);
                }
                else
                {
                    ETC_JUPMST list2 = etcJupmstService.GetImagebyRowId(argRowId);

                    if (list2.IsNullOrEmpty())
                    {
                        FileInfo f = new FileInfo(strFileName);

                        if (f.Exists == true)
                        {
                            f.Delete();
                        }
                        else
                        {
                            byte[] bImage = null;

                            bImage = Convert.FromBase64String(list2.IMAGE.To<string>());
                            stream = new MemoryStream(bImage);
                            image = new Bitmap(stream);
                            image.Save(strFileName);
                        }
                    }

                    //AUDIO viwer 실행
                    if (strViewerExe == "1")
                    {
                        DirectoryInfo Dir = new DirectoryInfo(strFileName);
                        if (Dir.Exists == true)
                        {
                            strAudioFile = "rundll32.exe shimgvw.dll, ImageView_Fullscreen " + strFileName;
                            VB.Shell(strAudioFile, "MaximizedFocus");
                            Application.DoEvents();
                        }
                    }
                }
            }
        }

        public void AudioFILE_DBToFile(string argRowId, string strViewerExe = "", int Inx = 0)
        {
            if (clsVbfunc.GetFile("C:\\WINDOWS\\SYSTEM32\\SHIMGVW.DLL") == "")
            {
                MessageBox.Show("WINDOWS IMAGE Viewer가 설치되지 안았습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Ftpedt FtpedtX = new Ftpedt();
            MemoryStream stream = null;
            Bitmap image = null;

            string strFileName = "";
            string strAudioFile = "";
            string strRemotePath = "";
            string strFTP = "";
            string strLocalFilePass = @"c:\";
            string strLocalFileName = "ETC.jpg";

            strFileName = @"c:\ETC.jpg";
            strFTP = "";

            //FTP저장체크
            ETC_JUPMST list = etcJupmstService.GetIetmbyRowId(argRowId);

            if (!list.IsNullOrEmpty())
            {
                if (strFTP == "Y")
                {
                    strRemotePath = "/data/ocs_etc/" + list.RDATE + "/" + list.FILEPATH.Trim();

                    //2014-11-18 서버에서 PC로 파일을 다운로드함
                    clsHcVariable.FTP_Pass = ftp.READ_FTP(clsDB.DbCon, clsHcVariable.FTP_IpAddr, clsHcVariable.FTP_User);
                    ftp.FtpDownload(clsHcVariable.FTP_IpAddr, clsHcVariable.FTP_User, clsHcVariable.FTP_Pass, strLocalFilePass, strRemotePath + strLocalFileName, strRemotePath);
                }
                else
                {
                    ETC_JUPMST list2 = etcJupmstService.GetImagebyRowId(argRowId);

                    if (list2.IsNullOrEmpty())
                    {
                        FileInfo f = new FileInfo(strFileName);

                        if (f.Exists == true)
                        {
                            f.Delete();
                            //System.Diagnostics.Process[] ProcessEx = System.Diagnostics.Process.GetProcessesByName(strFileName);
                            //if (ProcessEx.Length > 0)
                            //{
                            //    System.Diagnostics.Process[] Process = System.Diagnostics.Process.GetProcessesByName(strFileName);
                            //    System.Diagnostics.Process CurProcess = System.Diagnostics.Process.GetCurrentProcess();
                            //    foreach (System.Diagnostics.Process Proc in Process)
                            //    {
                            //        if (Proc.Id != CurProcess.Id)
                            //        {
                            //            Proc.Kill();
                            //            Delay(500);
                            //            break;
                            //        }
                            //    }
                            //}
                        }
                        else
                        {
                            byte[] bImage = null;

                            bImage = Convert.FromBase64String(list2.IMAGE.To<string>());
                            stream = new MemoryStream(bImage);
                            image = new Bitmap(stream);
                            image.Save(strFileName);
                        }
                    }

                    //AUDIO viwer 실행
                    if (strViewerExe == "1")
                    {
                        strAudioFile = "rundll32.exe shimgvw.dll, ImageView_Fullscreen " + strFileName;
                        VB.Shell(strAudioFile, "MaximizedFocus");
                        Application.DoEvents();
                    }
                }
            }
        }

        public void AudioFILE_DBToFile1(string argRowId, string strViewerExe = "", int Inx = 0)
        {
            if (clsVbfunc.GetFile("C:\\WINDOWS\\SYSTEM32\\SHIMGVW.DLL") == "")
            {
                MessageBox.Show("WINDOWS IMAGE Viewer가 설치되지 안았습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Ftpedt FtpedtX = new Ftpedt();
            MemoryStream stream = null;
            Bitmap image = null;

            string strFileName = "";
            string strAudioFile = "";
            string strRemotePath = "";
            string strRemoteFileName = "";
            string strFTP = "";
            string strLocalFileName = string.Format("{0:#0}", Inx) + ".jpg";

            strFileName = @"c:\CMC\팀파노이미지_"+ string.Format("{0:#0}", Inx) + ".jpg";
            strFTP = "";

            //FTP저장체크
            ETC_JUPMST list = etcJupmstService.GetIetmbyRowId(argRowId);

            if (!list.IsNullOrEmpty())
            {
                strFTP = "Y";
            }

            if (strFTP == "Y")
            {
                strRemotePath = "/data/ocs_etc/" + list.RDATE + "/";
                strRemoteFileName = list.FILEPATH.Trim();

                //2014-11-18 서버에서 PC로 파일을 다운로드함
                clsHcVariable.FTP_Pass = ftp.READ_FTP(clsDB.DbCon, clsHcVariable.FTP_IpAddr, clsHcVariable.FTP_User);
                ftp.FtpDownload(clsHcVariable.FTP_IpAddr, clsHcVariable.FTP_User, clsHcVariable.FTP_Pass, strFileName, strRemotePath + strRemoteFileName, strRemotePath);
            }
            else
            {
                ETC_JUPMST list2 = etcJupmstService.GetImagebyRowId(argRowId);

                if (list2.IsNullOrEmpty())
                {
                    FileInfo f = new FileInfo(strFileName);

                    if (f.Exists == true)
                    {
                        f.Delete();
                    }
                    else
                    {
                        byte[] bImage = null;

                        bImage = Convert.FromBase64String(list2.IMAGE.To<string>());
                        stream = new MemoryStream(bImage);
                        image = new Bitmap(stream);
                        image.Save(strFileName);
                    }
                }
            }

            //AUDIO viwer 실행
            if (strViewerExe == "1")
            {
                strAudioFile = "rundll32.exe shimgvw.dll, ImageView_Fullscreen " + strFileName;
                VB.Shell(strAudioFile, "MaximizedFocus");
                Application.DoEvents();
            }
        }
        #endregion

        #region vbHicDojang.bas(이상훈)

        #endregion

        #region vbExamRefer.bas(이상훈)
        /// <summary>
        /// 검사코드 참고치 변경 관련된 변수 설정
        /// </summary>
        public void SET_REFER_CHANGE_VALUE()
        {
            int nRead = 0;
            string strCode = "";
            //string strList1 = "";
            List<string> strList1 = new List<string>();
            string strList2 = "";
            string strList3 = "";

            string strResultType = "1";

            clsHcVariable.REFER_OLD_CNT = 0;
            clsHcVariable.REFER_NEW_CNT = 0;

            List<HIC_EXCODE_REFER> list = hicExcodeReferService.FindAll(strResultType);
            string[] arrCode = list.GetStringArray("CODE");
            string[] arrFromDate = list.GetStringArray("FROMDATE");
            string[] arrToDate = list.GetStringArray("TODATE");
            string[] arrMin_M = list.GetStringArray("MIN_M");
            string[] arrMax_M = list.GetStringArray("MAX_M");
            string[] arrMin_F = list.GetStringArray("MIN_F");
            string[] arrMax_F = list.GetStringArray("MAX_F");
            string[] arrUnit = list.GetStringArray("UNIT");

            nRead = list.Count;
            for (int i = 0; i < nRead; i++)
            {
                strCode = arrCode[i].Trim();
                //코드목록
                if (VB.InStr(strList1[i], strCode) == 0)
                {
                    strList1.Add(strCode);
                    //strList1 += strCode + ",";
                }
                //남자 정상 참고치
                strList2 += strCode + "{}";
                strList2 += arrFromDate[i] + "{}" + arrToDate[i];
                strList2 += arrMin_M[i] + "{}" + arrMax_M[i];
                strList2 += arrUnit[i] + "{@}";
                clsHcVariable.REFER_OLD_CNT += 1;
                //여자 정상 참고치
                strList3 += strCode + "{}";
                strList3 += arrFromDate[i] + "{}" + arrToDate[i];
                strList3 += arrMin_F[i] + "{}" + arrMax_F[i];
                strList3 += arrUnit[i] + "{@}";
            }

            //if (strList1 != null)
            //{
            //    strList1 = VB.Left(strList1, strList1.Length - 1);
            //}

            clsHcVariable.REFER_CHANGE_CODELIST = strList1.ToString();
            clsHcVariable.REFER_OLD_남자_VALUE = strList2;
            clsHcVariable.REFER_OLD_여자_VALUE = strList3;

            //List<HIC_EXCODE> list2 = hicExcodeService.GetReferencebyCodeList(clsHcVariable.REFER_CHANGE_CODELIST);
            List<HIC_EXCODE> list2 = hicExcodeService.GetReferencebyCodeList(strList1);

            strList1 = null;
            strList2 = "";
            strList3 = "";

            string[] arrMin_M2 = list2.GetStringArray("MIN_M");
            string[] arrMax_M2 = list2.GetStringArray("MAX_M");
            string[] arrMin_F2 = list2.GetStringArray("MIN_F");
            string[] arrMax_F2 = list2.GetStringArray("MAX_F");
            string[] arrUnit2 = list2.GetStringArray("UNIT");

            nRead = list2.Count;
            for (int i = 0; i < nRead; i++)
            {
                //남자 정상 참고치
                strList2 += arrMin_M[i] + "{}" + arrMax_M[i];
                strList2 += arrUnit[i] + "{@}";                
                //여자 정상 참고치
                strList3 += arrMin_F[i] + "{}" + arrMax_F[i];
                strList3 += arrUnit[i] + "{@}";
                clsHcVariable.REFER_NEW_CNT += 1;
            }
            clsHcVariable.REFER_NEW_남자_VALUE = strList2;
            clsHcVariable.REFER_NEW_여자_VALUE = strList3;
        }

        /// <summary>
        /// 참고치가 변경된 코드인지 점검(True=변경된 코드)
        /// </summary>
        /// <param name="argCode"></param>
        /// <returns></returns>
        public bool Check_ReferValue_ChangeCode(string argCode)
        {
            bool rtnVal = false;

            if (VB.InStr(clsHcVariable.REFER_CHANGE_CODELIST, argCode) > 0)
            {
                rtnVal = true;
            }

            return rtnVal;
        }

        public string GET_Refer_Value(string argCode, string argSex, string argDate, string argGbUnit)
        {
            string rtnVal = "";
            int nCnt = 0;
            string strRec = "";
            string strRefer = "";
            string strCode = "";
            string strFromDate = "";
            string strToDate = "";

            //종전의 참고값을 찾음
            for (int i = 0; i < clsHcVariable.REFER_OLD_CNT; i++)
            {
                if (argSex == "M")
                {
                    strRec = VB.Pstr(clsHcVariable.REFER_OLD_남자_VALUE, "{@}", i);
                }
                else
                {
                    strRec = VB.Pstr(clsHcVariable.REFER_OLD_여자_VALUE, "{@}", i);
                }
                strCode = VB.Pstr(strRec, "{}", 1);

                if (strCode == argCode)
                {
                    strFromDate = VB.Pstr(strRec, "{}", 2);
                    strToDate = VB.Pstr(strRec, "{}", 3);
                    if (string.Compare(argDate, strFromDate) >= 0 && string.Compare(argDate, strToDate) <= 0)
                    {
                        strRefer = VB.STRCUT(strRec, "{}" + strToDate + "{}", "");
                    }
                }
            }

            //종전의 참고치 대상이 아니면 HIC_EXCODE의 참고치를 가져옴
            if (strRefer == "")
            {
                if (argSex == "M")
                {
                    strRefer = VB.STRCUT(clsHcVariable.REFER_OLD_남자_VALUE, "{}", "{@}");
                }
                else
                {
                    strRefer = VB.STRCUT(clsHcVariable.REFER_OLD_여자_VALUE, "{}", "{@}");
                }
            }

            if (argGbUnit == "Y")
            {
                if (VB.Pstr(strRefer, "{}", 1) == "~")
                {
                    strRefer = "";
                }
                else
                {
                    strRefer = strRefer.Replace("{}", "");
                }
            }
            else
            {
                strRefer = VB.Pstr(strRefer, "{}", 1);
            }

            rtnVal = strRefer;

            return rtnVal;
        }
        #endregion

        #region Read_INI()(이상훈)
        public List<BAS_PCCONFIG> Read_INI(ComboBox argComboBox = null)
        {
            List<BAS_PCCONFIG> rtnval = null;

            clsCompuInfo.SetComputerInfo();

            List<BAS_PCCONFIG> Configlst = basPcconfigService.GetConfig(clsCompuInfo.gstrCOMIP);

            if (argComboBox != null)
            {
                argComboBox.SetItems(Configlst, "VALUEV", "CODE");
                argComboBox.SelectedIndex = 0;
            }

            BAS_PCCONFIG Config_PftSNlst = basPcconfigService.GetConfig_PFTSN(clsCompuInfo.gstrCOMIP);
            //select* FROM KOSMOS_PMPA.BAS_PCCONFIG
            // WHERE GUBUN = '폐활량장비S/N'

            if (!Config_PftSNlst.IsNullOrEmpty())
            {
                clsHcVariable.GstrPFTSN = Config_PftSNlst.VALUEV;   //폐활량장비S/N
            }

            clsHcVariable.GstrHeaAutoActingYN = "";
            clsHcVariable.GstrHeaMonitorOffYN = "";
            clsHcVariable.GstrHeaInbodySendYN = "";

            clsHcVariable.GstrHicPart = basPcconfigService.GetConfig_Code(clsCompuInfo.gstrCOMIP, "검진센터부서");
            clsHcVariable.GstrHicPartName = basPcconfigService.GetConfig_Check(clsCompuInfo.gstrCOMIP, "검진센터부서");
            clsHcVariable.GstrHeaAutoActingYN = basPcconfigService.GetConfig_Check(clsCompuInfo.gstrCOMIP, "자동액팅PC여부");
            clsHcVariable.GstrHeaMonitorOffYN = basPcconfigService.GetConfig_Check(clsCompuInfo.gstrCOMIP, "대기순번모니터끄기여부");
            clsHcVariable.GstrHeaInbodySendYN = basPcconfigService.GetConfig_Check(clsCompuInfo.gstrCOMIP, "InBody전송PC여부");


            return rtnval;
        }
        #endregion

        /// <summary>
        /// 주민번호 뒷자리로 성별 체크
        /// </summary>
        /// <param name="argJumin2"></param>
        /// <returns></returns>
        public string Chk_Sex_JuminNo(string argJumin2)
        {
            string rtnVal = "M";

            if (argJumin2.IsNullOrEmpty())
            {
                return rtnVal;
            }

            if (argJumin2.Equals("0") || argJumin2.Equals("1") || argJumin2.Equals("3") ||
                argJumin2.Equals("5") || argJumin2.Equals("7") || argJumin2.Equals("9"))
            {
                rtnVal = "M";
            }
            else if (argJumin2.Equals("2") || argJumin2.Equals("4") || argJumin2.Equals("6") || argJumin2.Equals("8"))
            {
                rtnVal = "F";
            }

            return rtnVal;
        }

        /// <summary>
        /// 주민번호 점검로직
        /// </summary>
        /// <param name="argJumin1"></param>
        /// <param name="argJumin2"></param>
        /// <returns></returns>
        public string JuminNo_Check_New(string argJumin1, string argJumin2)
        {
            string rtnVal = "";
            int nCheckTotal = 0;
            int nCheckDigit = 0;
            int nCheckCount = 0;
            int j = 0;

            //값이 없으면 오류
            if (argJumin1.IsNullOrEmpty() || argJumin2.IsNullOrEmpty()) { return rtnVal; }
            
            //건진 전산실연습 테스트 번호
            if (argJumin1.Equals("600101") && argJumin2.Equals("1111111")) { return rtnVal; }
            if (argJumin1.Equals("600101") && argJumin2.Equals("2111111")) { return rtnVal; }
            if (argJumin1.Equals("800101") && argJumin2.Equals("1111111")) { return rtnVal; }
            if (argJumin1.Equals("800101") && argJumin2.Equals("2111111")) { return rtnVal; }
            if (argJumin1.Equals("900101") && argJumin2.Equals("1111111")) { return rtnVal; }
            if (argJumin1.Equals("900101") && argJumin2.Equals("2111111")) { return rtnVal; }
            if (argJumin1.Equals("000101") && argJumin2.Equals("3111111")) { return rtnVal; }
            if (argJumin1.Equals("000101") && argJumin2.Equals("4111111")) { return rtnVal; }

            //주민번호 길이 오류체크
            if (argJumin1.Length != 6 || argJumin2.Length != 7) { return "ERROR"; }

            //신생아번호 Check 안함
            if (VB.Mid(argJumin2 , 2, 6) == "000000") { return rtnVal; }
            //행려환자 Check
            if (VB.Mid(argJumin2, 1, 1) == "5" || VB.Mid(argJumin2, 1, 1) == "6") { return rtnVal; }
            //외국인
            if (VB.Mid(argJumin2, 1, 1) == "7" || VB.Mid(argJumin2, 1, 1) == "8") { return rtnVal; }

            int chkSum = 0;
            if (!int.TryParse(argJumin2.Substring(0, 1), out chkSum)) { return "ERROR"; }

            for (int i = 1; i <= 12; i++)
            {
                j = i + 1;
                if (j > 9) { j = j - 8; }

                if (i >= 1 && i <= 6)
                {
                    nCheckDigit = (VB.Mid(argJumin1, i, 1)).To<int>() * j;
                }
                else
                {
                    nCheckDigit = (VB.Mid(argJumin2, i - 6, 1)).To<int>() * j;
                }

                nCheckTotal = nCheckTotal + nCheckDigit;
            }

            nCheckDigit = nCheckTotal / 11;
            nCheckDigit = nCheckDigit * 11;
            nCheckCount = nCheckTotal - nCheckDigit;

            switch (nCheckCount)
            { 
                case 0:     nCheckDigit = 1; break;
                case 1:     nCheckDigit = 0; break;
                default:    nCheckDigit = 11 - nCheckCount; break;
            }

            if (nCheckTotal < 20 || nCheckDigit != (VB.Mid(argJumin2, 7, 1)).To<int>())
            { 
                return "ERROR";
            }

            return rtnVal;
        }

        /// <summary>
        /// 콤보박스 Year + MM 형식추가
        /// </summary>
        /// <param name="argYYMM"></param>
        /// <param name="argADD"></param>
        /// <returns></returns>
        public string DATE_YYMM_ADD(string ArgYYMM, int argADD)
        {
            string rtnVal = string.Empty;

            int ArgJ = 0;
            int ArgYY = 0;
            int ArgMM = 0;

            if (ArgYYMM.Length != 6 || argADD == 0)
            {
                rtnVal = ArgYYMM;
                return rtnVal;
            }

            ArgYY = VB.Val(VB.Left(ArgYYMM, 4)).To<int>();
            ArgMM = VB.Val(VB.Right(ArgYYMM, 2)).To<int>();

            ArgJ = argADD;

            if (ArgJ < 0) { ArgJ = ArgJ * -1; }

            for (int ArgI = 1; ArgI <= ArgJ; ArgI++)
            {
                if (argADD < 0)
                {
                    ArgMM = ArgMM - 1;
                    if (ArgMM == 0) { ArgMM = 12; ArgYY = ArgYY - 1; }
                }
                else
                {
                    ArgMM = ArgMM + 1;
                    if (ArgMM == 13) { ArgMM = 1; ArgYY = ArgYY + 1; }
                }
            }

            rtnVal = VB.Format(ArgYY, "0000") + VB.Format(ArgMM, "00");

            return rtnVal;
        }

        /// <summary>
        /// 휴일을 제외한 일수 계산
        /// </summary>
        /// <param name="ArgFDate"></param>
        /// <param name="ArgTDate"></param>
        /// <returns></returns>
        /// <seealso cref="VbFunction> DATE_ILSU_YOIL"/>
        public int DATE_ILSU_YOIL(string ArgFDate, string ArgTDate)
        {
            int rtnVal = 0;

            if (ArgFDate.Length != 10 ||VB.IsDate(ArgFDate) == false || ArgTDate.Length != 10 || VB.IsDate(ArgTDate) == false)
            {
                return rtnVal;
            }

            if (string.Compare(ArgFDate, ArgTDate) > 0)
            {
                return rtnVal;
            }

            rtnVal = comHpcLibBService.Date_Count_OutHoliDay(ArgFDate, ArgTDate);

            return rtnVal;
        }

        public void ETC_FILE_DBToFile(string strROWID, string strPtNo, string strViewerExe)
        {
            if (clsVbfunc.GetFile("C:\\WINDOWS\\SYSTEM32\\SHIMGVW.DLL") == "")
            {
                ComFunc.MsgBox("WINDOWS IMAGE Viewer가 설치되지 안았습니다.");
                return;
            }

            Ftpedt FtpedtX = new Ftpedt();
            byte[] b = null;
            MemoryStream stream = null;
            Bitmap image = null;

            string strFileName = "";
            string strRemotePath = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strAudioFile = "";

            DataTable dt = null;

            try
            {

                //'2014-11-18 FTP저장체크

                SQL = " SELECT ROWID,TO_CHAR(RDATE,'YYYYMMDD') AS RDATE, TO_CHAR(BDATE,'YYYYMMDD') AS BDATE,  GUBUN,FILEPATH       ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.ETC_JUPMST ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + strROWID + "' ";
                SQL = SQL + ComNum.VBLF + "  AND GBFTP ='Y' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                strFileName = @"C:\CMC\ETC.jpg";

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["GUBUN"].ToString().Trim() == "6" || dt.Rows[0]["GUBUN"].ToString().Trim() == "23")
                    {
                        strRemotePath = "/data/ocs_etc/" + dt.Rows[0]["BDATE"].ToString().Trim() + "/";
                    }
                    else
                    {
                        strRemotePath = "/data/ocs_etc/" + dt.Rows[0]["RDATE"].ToString().Trim() + "/";
                    }

                    //    '2014-11-18 서버에서 PC로 파일을 다운로드함
                    FtpedtX.FtpDownload("192.168.100.31", "oracle", FtpedtX.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle"), strFileName, dt.Rows[0]["FILEPATH"].ToString().Trim(), strRemotePath);
                    FtpedtX = null;

                    dt.Dispose();
                    dt = null;
                }
                else
                {
                    dt.Dispose();
                    dt = null;

                    SQL = "";
                    SQL = "SELECT IMAGE FROM KOSMOS_OCS.ETC_JUPMST  WHERE ROWID = '" + strROWID + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        b = (byte[])(dt.Rows[0]["IMAGE"]);

                        stream = new MemoryStream(b);
                        image = new Bitmap(stream);
                        image.Save(strFileName);
                    }

                    dt.Dispose();
                    dt = null;
                }

                //'ecg viwer 실행
                if (strViewerExe == "1")
                {
                    if (clsVbfunc.GetFile("%ProgramFiles%\\Windows Photo Gallery\\PhotoViewer.dll") != "")
                    {
                        strAudioFile = "rundll32.exe %ProgramFiles%\\Windows Photo Gallery\\PhotoViewer.dll, ImageView_Fullscreen " + strFileName;
                    }
                    else
                    {
                        strAudioFile = "rundll32.exe shimgvw.dll, ImageView_Fullscreen " + strFileName;

                    }
                    VB.Shell(strAudioFile, "MaximizedFocus");
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }

        }

        public void ECGFILE_DBToFile(string strROWID, string strPtNo, string strViewerExe)
        {
            bool bolWin7 = true;
            bool bolWin10 = true;

            byte[] b = null;
            MemoryStream stream = null;
            Bitmap image = null;
            string strECGFile = "";

            string strFileName = "";
            string strRemotePath = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            DataTable dt = null;

            if (clsVbfunc.GetFile("C:\\Program Files\\NKC\\ECG Viewer3\\FileViewer.exe") == "")
            {
                bolWin7 = false;
            }
            else
            {
                strECGFile = @"C:\Program Files\NKC\ECG Viewer3\FileViewer.exe";
            }

            if (clsVbfunc.GetFile("C:\\Program Files (x86)\\NKC\\ECG Viewer3\\FileViewer.exe") == "")
            {
                bolWin10 = false;
            }
            else
            {
                strECGFile = @"C:\Program Files (x86)\NKC\ECG Viewer3\FileViewer.exe";
            }

            if (bolWin7 == false && bolWin10 == false)
            {
                ComFunc.MsgBox("ECG(EKG) Viewer가 설치되지 안았습니다.");
                return;
            }

            try
            {   
                //'2014-11-18 FTP저장체크
                SQL = " SELECT ROWID,TO_CHAR(RDATE,'YYYYMMDD') AS RDATE, FILEPATH FROM KOSMOS_OCS.ETC_JUPMST ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + strROWID + "' ";
                SQL = SQL + ComNum.VBLF + "  AND GbFTP ='Y' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                strFileName = @"C:\CMC\ECG_" + strPtNo + ".ecg";

                if (dt.Rows.Count > 0)
                {

                    strRemotePath = "/data/ocs_etc/" + dt.Rows[0]["RDATE"].ToString().Trim() + "/";


                    //'2014-11-18 서버에서 PC로 파일을 다운로드함
                    Ftpedt FtpedtX = new Ftpedt();
                    FtpedtX.FtpDownload("192.168.100.31", "oracle", FtpedtX.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle"), strFileName, dt.Rows[0]["FILEPATH"].ToString().Trim(), strRemotePath);
                    FtpedtX = null;

                    dt.Dispose();
                    dt = null;
                }
                else
                {
                    dt.Dispose();
                    dt = null;

                    SQL = "";
                    SQL = "SELECT IMAGE ";
                    SQL = SQL + ComNum.VBLF + "FROM KOSMOS_OCS.ETC_JUPMST ";
                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        b = (byte[])(dt.Rows[0]["IMAGE"]);

                        stream = new MemoryStream(b);
                        image = new Bitmap(stream);
                        image.Save(strFileName);
                    }

                    dt.Dispose();
                    dt = null;
                }

                //'ecg viwer 실행
                if (clsVbfunc.GetFile(strFileName) != "")
                {
                    if (strViewerExe == "1")
                    {
                        //프로그램이 사용중인지 체크
                        bool isRunning = false;

                        System.Diagnostics.Process[] ProcessEx = System.Diagnostics.Process.GetProcessesByName("FileViewer");
                        if (ProcessEx.Length > 0)
                        {
                            System.Diagnostics.Process[] Pro1 = System.Diagnostics.Process.GetProcessesByName("FileViewer");
                            System.Diagnostics.Process CurPro = System.Diagnostics.Process.GetCurrentProcess();
                            foreach (System.Diagnostics.Process Proc in Pro1)
                            {
                                if (Proc.Id != CurPro.Id)
                                {
                                    isRunning = true;
                                    Proc.Kill();
                                    Delay(500);
                                    isRunning = false;
                                    break;
                                }
                            }
                        }

                        if (isRunning == false)
                        {
                            Delay(500);
                            System.Diagnostics.Process program = System.Diagnostics.Process.Start(strECGFile, strFileName + " ");
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }
        }

        public static DateTime Delay(int MS)
        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS);
            DateTime AfterWards = ThisMoment.Add(duration);
            while (AfterWards >= ThisMoment)
            {
                System.Windows.Forms.Application.DoEvents();
                ThisMoment = DateTime.Now;
            }
            return DateTime.Now;
        }

        #region HaMain_AutoPan.bas
        public string ReadAutoPan(string argJepNo)
        {
            string rtnVal = "";

            int nRead = 0;
            string strTemp = "";
            string[] strWrtNoTemp = new string[1];

            string[] strWrtNo = new string[1];
            string[] strGrpNo = new string[1];
            string[] strAutoPanWrtno = new string[1];
            string[] strAutoPanGrpno = new string[1];

            string strOK = "";
            string strSex = "";

            string strCompare1 = "";
            string strCompare1Cnt = "";
            string strCompare2 = "";
            string strCompare2Cnt = "";
            string strCompare3 = "";
            string strCompare3Cnt = "";

            int j = 0;
            int k = 0;

            //코드 없이 여부 확인
            //1. 혈압약 복용 여부
            //2. 흡연 여부
            //3. 당뇨병 치료 중
            //4. 고혈압 치료중
            //5. 대장 내시경 실시 여부
            //6. 전립선 초음파
            //7. 요침검사

            Array.Resize(ref strWrtNo, 0);
            Array.Resize(ref strAutoPanWrtno, 0);

            strSex = heaJepsuService.GetSexbyWrtNo(argJepNo) == "F" ? "여자" : "남자";

            List<COMHPC> list = comHpcLibBService.GetSumGrpNobyWrtNo(argJepNo);

            if (list.Count > 0)
            {
                Array.Resize(ref strWrtNo, list.Count);
                Array.Resize(ref strAutoPanWrtno, list.Count);
                for (int i = 0; i < list.Count; i++)
                {
                    strWrtNo[i] = list[i].WRTNO.ToString();
                    strGrpNo[i] = list[i].GRPNO.Trim();
                }
            }

            if (list.Count < 1)
            {
                rtnVal = "";
                return rtnVal;
            }

            Array.Resize(ref strAutoPanGrpno, 0);

            for (int i = 0; i < VB.UBound(strWrtNo); i++)
            {
                List<COMHPC> list2 = comHpcLibBService.GetWrtNoSeqNobyWrtNo(strWrtNo[i]);
                nRead = list2.Count;
                if (nRead > 0)
                {
                    for (int kk = 0; kk < nRead; kk++)
                    {
                        strOK = "OK";
                        if (VB.UBound(strAutoPanGrpno) > 0)
                        {
                            for (int l = 0; l < VB.UBound(strAutoPanGrpno); l++)
                            {
                                if (strGrpNo[i] == strAutoPanGrpno[l])
                                {
                                    strOK = "NO";
                                }
                            }
                        }

                        if (ReadAutoPanExCode(argJepNo, strWrtNo[i], strSex, list2[kk].SEQNO.ToString()) == true && strOK == "OK")
                        {
                            j += 1;
                            Array.Resize(ref strAutoPanWrtno, j);
                            Array.Resize(ref strAutoPanGrpno, j);
                            strAutoPanWrtno[j - 1] = strWrtNo[i];
                            strAutoPanGrpno[j - 1] = strGrpNo[i];
                        }
                    }
                }
            }

            Array.Resize(ref strWrtNoTemp, VB.UBound(strAutoPanWrtno));

            k = 0;
            List<HEA_AUTOPAN> list3 = heaAutopanService.GetWrtNo();

            if (list3.Count > 0)
            {
                for (int i = 0; i < list3.Count; i++)
                {
                    for (int jj = 0; jj < VB.UBound(strAutoPanWrtno); jj++)
                    {
                        if (list3[i].WRTNO.ToString() == strAutoPanWrtno[jj])
                        {
                            strWrtNoTemp[k] = strAutoPanWrtno[jj];
                            k += 1;
                        }
                    }
                }
            }

            strTemp = "";
            if (strWrtNoTemp[0] != "")
            {
                for (int i = 0; i < VB.UBound(strAutoPanWrtno); i++)
                {
                    strTemp += ViewResultSyntex(strWrtNoTemp[i], argJepNo) + "\r\n" + "\r\n";
                }
            }

            rtnVal = strTemp;

            return rtnVal;
        }

        public bool ReadAutoPanExCode(string argJepNo, string argWrtNo, string argSex, string argSeqNo)
        {
            bool rtnVal = false;
            int nREAD = 0;

            string strOldEXCODE = "";
            string strSEX = "";
            string strValue1 = "";
            string strValue2 = "";
            string strLogic = "";
            string strCalc = "";
            string strRetVal = "";
            int nCntR1 = 0;
            int nCntR2 = 0;
            string strLeftValue = "";
            string strLeftLogic = "";
            string strMidValue = "";
            string strRightValue = "";
            string strRightLogic = "";
            bool bLogic1 = false;
            bool bLogic2 = false;
            bool bLogic3 = false;
            bool bLogic4 = false;
            bool bLogic5 = false;

            nCntR1 = heaAutopanLogicService.GetCountbyWrtNo(argWrtNo);
            nCntR2 = hicResultService.GetCountbyWrtNoExCode(argWrtNo, argJepNo);

            if (nCntR1 != nCntR2)
            {
                rtnVal = false;
                return rtnVal;
            }

            bLogic1 = false;

            List<COMHPC> list = comHpcLibBService.GetAutopanLogicResultbyWrtNo(argJepNo, argWrtNo, argSeqNo);

            nREAD = list.Count;

            if (nREAD > 0)
            {
                for (int i = 0; i < nREAD; i++)
                {
                    strSEX = list[i].SEX.Trim();
                    strValue1 = list[i].RESULT.Trim();
                    strValue2 = list[i].RESULTVALUE.Trim();
                    strLogic = list[i].LOGIC.Trim();
                    if (strSEX == "")
                    {
                        bLogic1 = CompareLogic(strValue1, strValue2, strLogic);
                    }
                    else if (strSEX == argSex)
                    {
                        bLogic1 = CompareLogic(strValue1, strValue2, strLogic);
                    }
                    else
                    {
                        bLogic1 = false;
                    }

                    if (bLogic1 == false)
                    {
                        rtnVal = false;
                        return rtnVal;
                    }
                }
            }

            if (bLogic1 == false)
            {
                rtnVal = false;
                return rtnVal;
            }

            List<COMHPC> list2 = comHpcLibBService.GetAutopanLogicResultbyWrtNo_Second(argJepNo, argWrtNo, argSeqNo);

            nREAD = list2.Count;

            if (nREAD > 0)
            {
                for (int i = 0; i < nREAD; i++)
                {                    
                    strSEX = list2[i].SEX.Trim();
                    strValue1 = list2[i].RESULT.Trim();
                    strValue2 = list2[i].RESULTVALUE.Trim();
                    strLogic = list2[i].LOGIC.Trim();
                    if (strOldEXCODE != list2[i].EXCODE.Trim())
                    {
                        strRightValue = list2[i].RESULTVALUE.Trim();
                        strRightLogic = list2[i].LOGIC.Trim();
                        strMidValue = list2[i].RESULT.Trim();
                    }
                    else
                    {
                        strLeftValue = list2[i].RESULTVALUE.Trim();
                        strLeftLogic = list2[i].LOGIC.Trim();

                        if (strSEX == "")
                        {
                            bLogic1 = CompareLogic2(strLeftValue, strLeftLogic, strRightValue, strRightLogic, strMidValue);
                        }
                        else if (strSEX == argSex)
                        {
                            bLogic1 = CompareLogic2(strLeftValue, strLeftLogic, strRightValue, strRightLogic, strMidValue);
                        }
                        else
                        {
                            bLogic1 = false;
                        }
                    }
                    strOldEXCODE = list2[i].EXCODE.Trim();
                    
                    if (bLogic1 == false)
                    {
                        rtnVal = false;
                        return rtnVal;
                    }
                }
            }

            bLogic2 = false;
            List<COMHPC> list3 = comHpcLibBService.GetAutopanLogicResultbyWrtNo_Third(argJepNo, argWrtNo, argSeqNo);

            nREAD = list3.Count;

            if (nREAD > 0)
            {
                for (int i = 0; i < nREAD; i++)
                {
                    strSEX = list3[i].SEX.Trim();
                    strValue1 = list3[i].RESULT.Trim();
                    strValue2 = list3[i].RESULTVALUE.Trim();
                    strLogic = list3[i].LOGIC.Trim();
                    
                    if (strSEX == "")
                    {
                        if (CompareLogic(strValue1, strValue2, strLogic) == true)
                        {
                            bLogic2 = true;
                            break;
                        }
                    }
                    else
                    {
                        if (strSEX == argSex)
                        {
                            if (CompareLogic(strValue1, strValue2, strLogic) == true)
                            {
                                bLogic2 = true;
                                break;
                            }
                        }
                    }
                    
                    if (bLogic1 == false)
                    {
                        rtnVal = false;
                        return rtnVal;
                    }
                }
            }

            if (nREAD < 1)
            {
                bLogic2 = true;
            }

            bLogic3 = false;
            List<COMHPC> list4 = comHpcLibBService.GetAutopanLogicResultbyWrtNo_forth(argJepNo, argWrtNo, argSeqNo);

            nREAD = list4.Count;

            if (nREAD > 0)
            {
                for (int i = 0; i < nREAD; i++)
                {
                    strSEX = list4[i].SEX.Trim();
                    strValue1 = list4[i].RESULT.Trim();
                    strValue2 = list4[i].RESULTVALUE.Trim();
                    strLogic = list4[i].LOGIC.Trim();
                    strCalc = list4[i].CALC.Trim();
                    strRetVal = list4[i].RESULTVALUE.Trim();

                    if (strSEX == "")
                    {
                        if (CompareCalc(strValue1, strValue2, strCalc, double.Parse(strRetVal), strLogic) == true)
                        {
                            bLogic3 = true;
                            break;
                        }
                    }
                    else
                    {
                        if (strSEX == argSex)
                        {
                            if (CompareCalc(strValue1, strValue2, strCalc, double.Parse(strRetVal), strLogic) == true)
                            {
                                bLogic3 = true;
                                break;
                            }
                        }
                    }
                }
            }

            if (nREAD < 1)
            {
                bLogic3 = true;
            }

            bLogic4 = true;
            //기타검사 조건이 있으면 무조건 FALSE
            //이후 문진표 전산화 이 후 적용 예정
            if (comHpcLibBService.GetExetcLogicbyWrtNo(argWrtNo, argSeqNo) > 0)
            {
                bLogic4 = false;
            }

            if (bLogic4 == true)
            {
                List<COMHPC> list5 = comHpcLibBService.GetCountAutoPanLogicExamByJepNo(argJepNo, argSeqNo, argWrtNo);

                if (list5.Count < 1)
                {
                    bLogic4 = true;
                }
                else
                {
                    strLogic = "실시(포함)";
                    List<COMHPC> list6 = comHpcLibBService.GetCountAutoPanLogicExamByLogic(argJepNo, argSeqNo, argWrtNo, strLogic);

                    nREAD = list6.Count;
                    if (list6.Count > 0)
                    {
                        if (nREAD < 1)
                        {
                            bLogic4 = false;
                        }
                    }

                    strLogic = "미실시(미포함)";
                    List<COMHPC> list7 = comHpcLibBService.GetCountAutoPanLogicExamByLogic(argJepNo, argSeqNo, argWrtNo, strLogic);

                    nREAD = list7.Count;
                    if (list7.Count > 0)
                    {
                        if (nREAD > 0)
                        {
                            bLogic4 = false;
                        }
                    }
                }
            }

            if (bLogic1 == true && bLogic2 == true && bLogic3 == true && bLogic4 == true)
            {
                rtnVal = true;
            }

            return rtnVal;
        }

        public bool CompareLogic2(string arg1, string arg1Logic, string arg2, string arg2Logic, string argRet)
        {
            bool rtnVal = false;

            bool bleft = false;
            bool bRight = false;

            switch (arg1Logic)
            {
                case ">":
                    bleft = (VB.Val(arg1) > VB.Val(argRet));
                    break;
                case ">=":
                    bleft = (VB.Val(arg1) >= VB.Val(argRet));
                    break;
                case "<":
                    bleft = (VB.Val(arg1) < VB.Val(argRet));
                    break;
                case "<=":
                    bleft = (VB.Val(arg1) <= VB.Val(argRet));
                    break;
                default:
                    break;
            }

            if (bleft == true && bRight == true)
            {
                rtnVal = true;
            }

            return rtnVal;
        }

        public bool CompareLogic(string arg1, string arg2, string argLogic)
        {
            bool rtnVal = false;

            switch (argLogic)
            {
                case "=":
                    if (VB.IsNumeric(arg1) == true && VB.IsNumeric(arg2) == true)
                    {
                        rtnVal = (VB.Val(arg1) == VB.Val(arg2));
                    }
                    else
                    {
                        rtnVal = (arg1 == arg2);
                    }
                    break;
                case "!=":
                    if (VB.IsNumeric(arg1) == true && VB.IsNumeric(arg2) == true)
                    {
                        rtnVal = (VB.Val(arg1) != VB.Val(arg2));
                    }
                    else
                    {
                        rtnVal = (arg1 == arg2);
                    }
                    break;
                case ">":
                    rtnVal = (VB.Val(arg1) > VB.Val(arg2));
                    break;
                case ">=":
                    rtnVal = (VB.Val(arg1) >= VB.Val(arg2));
                    break;
                case "<":
                    rtnVal = (VB.Val(arg1) < VB.Val(arg2));
                    break;
                case "<=":
                    rtnVal = (VB.Val(arg1) <= VB.Val(arg2));
                    break;
                default:
                    rtnVal = false;
                    break;
            }

            return rtnVal;
        }

        public bool CompareCalc(string arg1, string arg2, string argCalc, double argRetVal, string argLogic)
        {
            bool rtnVal = false;
            bool bCompare = false;
            double nCalcRet = 0;

            switch (argCalc)
            {
                case "==":
                    if (VB.IsNumeric(arg1) == true && VB.IsNumeric(arg2) == true)
                    {
                        rtnVal = (VB.Val(arg1) == VB.Val(arg2));
                    }
                    else
                    {
                        rtnVal = (arg1 == arg2);
                    }
                    break;
                case "!=":
                    if (VB.IsNumeric(arg1) == true && VB.IsNumeric(arg2) == true)
                    {
                        rtnVal = (VB.Val(arg1) != VB.Val(arg2));
                    }
                    else
                    {
                        rtnVal = (arg1 != arg2);
                    }
                    break;
                case ">":
                    rtnVal = (VB.Val(arg1) > VB.Val(arg2));
                    break;
                case ">=":
                    rtnVal = (VB.Val(arg1) >= VB.Val(arg2));
                    break;
                case "<":
                    rtnVal = (VB.Val(arg1) < VB.Val(arg2));
                    break;
                case "<=":
                    rtnVal = (VB.Val(arg1) <= VB.Val(arg2));
                    break;
                default:
                    break;
            }

            if (rtnVal == true)
            {
                return rtnVal;
            }

            switch (argCalc)
            {
                case "-":
                    nCalcRet = VB.Val(arg1) - VB.Val(arg2);
                    break;
                case "+":
                    nCalcRet = VB.Val(arg1) - VB.Val(arg2);
                    break;
                case "/":
                    nCalcRet = VB.Val(arg1) - VB.Val(arg2);
                    break;
                case "*":
                    nCalcRet = VB.Val(arg1) - VB.Val(arg2);
                    break;
                default:
                    break;
            }

            if (nCalcRet == 0)
            {
                return rtnVal;
            }

            switch (argLogic)
            {
                case "=":
                    rtnVal = (nCalcRet == argRetVal);
                    break;
                case "!=":
                    rtnVal = (nCalcRet != argRetVal);
                    break;
                case ">":
                    rtnVal = (nCalcRet > argRetVal);
                    break;
                case ">=":
                    rtnVal = (nCalcRet >= argRetVal);
                    break;
                case "<":
                    rtnVal = (nCalcRet < argRetVal);
                    break;
                case "<=":
                    rtnVal = (nCalcRet <= argRetVal);
                    break;
                default:
                    rtnVal = false;
                    break;
            }

            return rtnVal;
        }

        string ViewResultSyntex(string argWrtNo, string argJepNo)
        {
            string rtnVal = "";
            string strSyntex = "";
            string[] strMCode  = new string[10];  
            string[] strExcode = new string[10];
            string[] strResult = new string[10];

            if (argWrtNo == "")
            {
                return rtnVal;
            }

            strSyntex = heaAutopanService.GetTextbyWrtNo(long.Parse(argWrtNo));

            if (strSyntex == "")
            {
                return rtnVal;
            }

            List<HEA_AUTOPAN_MATCH_RESULT> list = heaAutoPanMatchResultService.GetItembyWrtno(argWrtNo, argJepNo);

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    strSyntex = strSyntex.Replace(list[i].MCODE, list[i].RESULT);
                }
            }

            rtnVal = strSyntex;

            return rtnVal;
        }
        #endregion

        public void Dir_Check_FileDelete(string sDirPath, string sFormat)
        {
            DirectoryInfo Dir = new DirectoryInfo(sDirPath);
            if (Dir.Exists == false)
            {
                Dir.Create();
            }
            else
            {
                System.IO.FileInfo[] files = Dir.GetFiles(sFormat, SearchOption.AllDirectories);

                foreach (System.IO.FileInfo file in files)
                {
                    file.Delete();
                }
            }
        }

        public bool Dir_Check_YN(string sDirPath)
        {
            bool rtnVal = false;

            DirectoryInfo Dir = new DirectoryInfo(sDirPath);
            if (Dir.Exists == true)
            {
                rtnVal = true;
            }

            return rtnVal;
        }

        /// <summary>
        /// 로컬 Directory에 Download 하지 않고 해당 Spread에 바로 매칭 되도록 변경
        /// </summary>
        /// <param name="spd"></param>
        /// <param name="row"></param>
        /// <param name="Col"></param>
        /// <param name="sabun"></param>
        public void SetDojangImage(FarPoint.Win.Spread.FpSpread spd, int row, int Col, string sabun)
        {
            FarPoint.Win.Spread.CellType.ImageCellType imgCellType = new FarPoint.Win.Spread.CellType.ImageCellType();
            imgCellType.Style = FarPoint.Win.RenderStyle.StretchAndScale;

            Image ImageX = GetDrDojang(sabun, "");

            spd.ActiveSheet.Cells[row, Col].CellType = imgCellType;
            spd.ActiveSheet.Cells[row, Col].Value = ImageX;

            ImageX = null;
            imgCellType = null;
        }

        public Image GetDrDojang(string strSabun, string strgubun)
        {
            Image rtnVal = null;
            MemoryStream stream = null;
            Image image = null;
            byte[] bImage = null;

            if (string.IsNullOrEmpty(strSabun)) return rtnVal;

            List<COMHPC> list = comHpcLibBService.GetItemHicDojang(strSabun);
            
            if (list.Count > 0)
            {
                bImage = list[0].IMAGE;

                stream = new MemoryStream(bImage);
                image = Image.FromStream(stream);
            }
            else
            {
                bImage = null;
            }

            rtnVal = image;

            return rtnVal;
        }

        public void SET_FileAttributes(string argPath, long FILE_ATTRIBUTE)
        {
            clsHcVariable.SetFileAttributes(argPath, FILE_ATTRIBUTE);
        }

        /// <summary>
        /// READ_금액표시기_SETTING  => 사용안함 확인일자 2020.07.20 김민철
        /// </summary>
        /// <param name="argForm"></param>
        /// <param name="argListBox"></param>
        public void Read_Amountindicator(Form argForm, Control argListBox)
        {
            //Set cMonitor = New clsmonitor
            //Set cMonitors = New clsmonitors


            //'Refresh the monitors listbox
            //Call RefreshMonitors(ArgListBox)


            //'=====================================
            //'모니터의듀얼유뮤채크
            //'단일 lReturn = 1
            //'그이상 lReturn not 1
            //'=====================================
            //Dim lReturn As Long


            //lReturn = cMonitors.Monitors.Count
            //selmon = lReturn


            //'Windows 95 and Windows NT 4 will return 1
            //If lReturn = 1 Then
            //    singmon = lReturn
            //    selmon = 1
            //    SetWindowPos argForm.hwnd, HWND_TOPMOST, 0, 0, 10, 10, SWP_NOMOVE

            //Else
            //    singmon = 2
            //End If


            //If lReturn<> 1 Then


            //    '===========================================
            //    '슬레이브모니터의 위치 파악
            //    '===========================================
            //    With cMonitors
            //        If.DesktopLeft < 0 Then
            //          selmonitor = 0
            //        Else
            //           selmonitor = 1
            //        End If
            //    End With


            //    '=============================================
            //    '슬레이브좌표변수에 실좌표 입력
            //    '=============================================


            //    Set cMonitor = New clsmonitor

            //    For Each cMonitor In cMonitors.Monitors
            //       With cMonitor
            //            If.Handle = ArgListBox.List(selmonitor) Then
            //              '========================
            //               slavecoodinate = .Left
            //              '========================
            //               dualmonX = .Width
            //               dualmonY = .Height
            //            Else
            //                singmonX = .Width
            //                singmonY = .Height
            //            End If
            //       End With
            //    Next
            //End If
        }

        /// <summary>
        /// 사용안함 확인일자 2020.07.20 김민철
        /// </summary>
        /// <param name="ArgListBox"></param>
        public void RefreshMonitors(Control ArgListBox)
        {
            ////==========================================================
            ////Refreshes the monitors collection and the listbox contents
            ////==========================================================
            ///
            //cMonitors.Refresh
            //ArgListBox.Clear

            //'Iterate through all the monitors, add their handle to the listbox
            //For Each cMonitor In cMonitors.Monitors
            //    ArgListBox.AddItem cMonitor.Handle
            //Next
        }

        /// <summary>
        /// 처방전 발행일에 의사가 휴일인지 점검
        /// </summary>
        /// <param name="ArgSabun"></param>
        /// <param name="ArgDate"></param>
        /// <returns></returns>
        public bool Check_Sabun_Huil(string ArgSabun, string ArgDate)
        {
            bool rtnVal = false;
            string strSabun = "";
            string strYear = "";
            string strToiDay = "";
            string strGunTae = "";
            string strData = "";
            int nIlsu = 0;
            string strDrCode = "";

            strYear = VB.Left(ArgDate, 4);
            strSabun = string.Format("{0:00000}", ArgSabun);

            //퇴사일자를 읽어 퇴사일자 이후이면 False 처리
            strToiDay = insaMstService.GetToiDay(clsType.User.IdNumber);

            if (strToiDay != "")
            {
                if (string.Compare(strToiDay, ArgDate) < 0)
                {
                    rtnVal = false;
                    return rtnVal;
                }
            }

            //해당일자에 휴가,교육,.. 인지 점검
            strGunTae = insaMsthService.GetGunTaebySabunYear(strSabun, strYear);

            nIlsu = cf.DATE_ILSU(clsDB.DbCon, ArgDate, strYear + "-01-01");
            strData = VB.Mid(strGunTae, nIlsu, 1);
            rtnVal = false;
            switch (strData)
            {
                case "A":
                    rtnVal = true;  //휴가
                    break;
                case "B":
                    rtnVal = true;  //월차
                    break;
                case "C":
                    rtnVal = true;  //특일
                    break;
                case "D":
                    rtnVal = true;  //교육
                    break;
                case "E":
                    rtnVal = true;  //출장
                    break;
                case "F":
                    rtnVal = true;  //병가
                    break;
                case "G":
                    rtnVal = true;  //분만
                    break;
                case "H":
                    rtnVal = true;  //피정
                    break;
                case "I":
                    rtnVal = true;  //훈련
                    break;
                case "J":
                    rtnVal = true;  //생휴
                    break;
                case "K":
                    rtnVal = true;  //학회
                    break;
                case "L":
                    rtnVal = true;  //결근
                    break;
                case "R":
                    rtnVal = true;  //휴직
                    break;
                default:
                    break;
            }

            //인사에 휴일이지만 근무스케쥴에 진료,출장검진,채용상담이면 휴무 아님
            if (rtnVal == true)
            {
                strDrCode = ocsDoctorService.GetDrCodebySabun(strSabun);

                BAS_SCHEDULE list = basScheduleService.GetGbJinGbJin2bySchDateDrCode(ArgDate, strDrCode);

                if (list.GBJIN == "1") rtnVal = false;
                if (list.GBJIN2 == "1") rtnVal = false;
                if (list.GBJIN == "B") rtnVal = false;
                if (list.GBJIN2 == "B") rtnVal = false;
                if (list.GBJIN == "C") rtnVal = false;
                if (list.GBJIN2 == "C") rtnVal = false;
            }

            return rtnVal;
        }

        public string READ_Res_Name_new(string argGubun, string argCode)
        {
            string rtnVal = "";
            string strName = "";

            if (argGubun.Trim() == "")
            {
                rtnVal = "";
                return rtnVal;
            }

            switch (argGubun)
            {
                case "01":
                    switch (argCode)
                    {
                        case "1":
                            rtnVal = "있다";
                            break;
                        case "2":
                            rtnVal = "없다";
                            break;
                        default:
                            break;
                    }
                    break;
                case "02":
                    switch (argCode)
                    {
                        case "1":
                            rtnVal = "있다";
                            break;
                        case "2":
                            rtnVal = "없다";
                            break;
                        case "3":
                            rtnVal = "모르겠다";
                            break;
                        default:
                            break;
                    }
                    break;
                case "03":
                    switch (argCode)
                    {
                        case "1":
                            rtnVal = "그렇다";
                            break;
                        case "2":
                            rtnVal = "보통이다";
                            break;
                        case "3":
                            rtnVal = "아니다";
                            break;
                        default:
                            break;
                    }
                    break;
                case "04":
                    switch (argCode)
                    {
                        case "1":
                            rtnVal = "예";
                            break;
                        case "2":
                            rtnVal = "아니오";
                            break;
                        case "3":
                            rtnVal = "모르겠다";
                            break;
                        default:
                            break;
                    }
                    break;
                case "05":
                    switch (argCode)
                    {
                        case "1":
                            rtnVal = "없음";
                            break;
                        case "2":
                            rtnVal = "있음";
                            break;
                        default:
                            break;
                    }
                    break;
                case "06":
                    switch (argCode)
                    {
                        case "1":
                            rtnVal = "없음";
                            break;
                        case "2":
                            rtnVal = "요교정";
                            break;
                        case "3":
                            rtnVal = "교정중";
                            break;
                        default:
                            break;
                    }
                    break;
                case "07":
                    switch (argCode)
                    {
                        case "1":
                            rtnVal = "우수";
                            break;
                        case "2":
                            rtnVal = "보통";
                            break;
                        case "3":
                            rtnVal = "개선요망";
                            break;
                        default:
                            break;
                    }
                    break;
                case "08":
                    switch (argCode)
                    {
                        case "1":
                            rtnVal = "과잉치";
                            break;
                        case "2":
                            rtnVal = "유치잔존";
                            break;
                        case "3":
                            rtnVal = "기타";
                            break;
                        default:
                            break;
                    }
                    break;
                case "09":
                    switch (argCode)
                    {
                        case "1":
                            rtnVal = "정상";
                            break;
                        case "2":
                            rtnVal = "이상";
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    rtnVal = "";
                    break;
            }
            return rtnVal;
        }

        public string READ_Res_Panel(string argGubun)
        {
            string rtnVal = "";
            string strName = "";

            if (argGubun.Trim() == "")
            {
                rtnVal = "";
                return rtnVal;
            }

            switch (argGubun)
            {
                case "01":
                    rtnVal = "1.있다 , 2.없다";
                    break;
                case "02":
                    rtnVal = "1.있다 , 2.없다 , 3.모르겠다";
                    break;
                case "03":
                    rtnVal = "1.그렇다 , 2.보통이다 , 3.아니다";
                    break;
                case "04":
                    rtnVal = "1.예 , 2.아니오 , 3.불소치약이 무엇인지모름";
                    break;
                case "05":
                    rtnVal = "1.없음 , 2.있음";
                    break;
                case "06":
                    rtnVal = "1.없음 , 2.요교정 , 3.교정중";
                    break;
                case "07":
                    rtnVal = "1.우수 , 2.보통 , 3.개선요망";
                    break;
                case "08":
                    rtnVal = "1.과잉치 , 2.유치잔존 , 3.그밖의치아상태";
                    break;
                case "09":
                    rtnVal = "1.정상 , 2.이상";
                    break;
                default:
                    break;
            }
            return rtnVal;
        }

        public string GET_Naksang_Flag(long aGE, string jEPDATE, string pTNO)
        {
            string rtnVal = "N";

            //만70세이상은 낙상주의
            if (aGE >= 70) { return "Y"; }

            //종검 수면내시경 환자는 낙상주의
            if (rtnVal == "N")
            {
                List<HIC_RESULT_EXCODE> lst1 = hicResultExCodeService.GetHeaEndoExListByWrtno(heaJepsuService.GetListWrtnoByPtnoSDate(pTNO, jEPDATE));

                if (lst1.Count > 0)
                {
                    for (int i = 0; i < lst1.Count; i++)
                    {
                        //if (lst1[i].ENDOGUBUN3 == "Y") { rtnVal = "Y"; }
                        //if (lst1[i].ENDOGUBUN5 == "Y") { rtnVal = "Y"; }
                        if (!lst1[i].ENDOGUBUN3.IsNullOrEmpty() && lst1[i].ENDOGUBUN3.Trim() == "Y") { rtnVal = "Y"; }
                        if (!lst1[i].ENDOGUBUN5.IsNullOrEmpty() && lst1[i].ENDOGUBUN5.Trim() == "Y") { rtnVal = "Y"; }
                    }
                }
            }

            //건진 수면내시경 환자는 낙상주의
            if (rtnVal == "N")
            {
                List<long> lstHicWrtno = hicJepsuService.GetListWrtnoByPtnoJepDate(pTNO, jEPDATE);

                if (lstHicWrtno.Count > 0)
                {
                    List<HIC_RESULT_EXCODE> lst2 = hicResultExCodeService.GetHicEndoExListByWrtnoIN(lstHicWrtno);

                    for (int i = 0; i < lst2.Count; i++)
                    {
                        //if (lst2[i].ENDOGUBUN3 == "Y") { rtnVal = "Y"; }  
                        //if (lst2[i].ENDOGUBUN5 == "Y") { rtnVal = "Y"; }
                        if (!lst2[i].ENDOGUBUN3.IsNullOrEmpty() && lst2[i].ENDOGUBUN3.Trim() == "Y") { rtnVal = "Y"; }
                        if (!lst2[i].ENDOGUBUN5.IsNullOrEmpty() && lst2[i].ENDOGUBUN5.Trim() == "Y") { rtnVal = "Y"; }
                    }
                }
            }

            return rtnVal;
        }

        public string READ_Biman(string argGbn, string argCode)
        {
            string rtnVal = "";

            if (argGbn == "")
            {
                return rtnVal;
            }

            if (argGbn == "1")
            {
                rtnVal = "1.정상,2.과체중,3.비만,4.저체중";
            }
            else if (argGbn == "2")
            {
                switch (argCode)
                {
                    case "1":
                        rtnVal = "정상";
                        break;
                    case "2":
                        rtnVal = "비만위험군";
                        break;
                    case "3":
                        rtnVal = "비만";
                        break;
                    case "4":
                        rtnVal = "저체중";
                        break;
                    default:
                        break;
                }
            }
            else if (argGbn == "3")
            {
                rtnVal = "1.정상,2.경도비만,3.중증도비만,4.고도비만";
            }
            else if (argGbn == "4")
            {
                switch (argCode)
                {
                    case "1":
                        rtnVal = "정상";
                        break;
                    case "2":
                        rtnVal = "경도비만";
                        break;
                    case "3":
                        rtnVal = "중증도비만";
                        break;
                    case "4":
                        rtnVal = "고도비만";
                        break;
                    default:
                        break;
                }
            }
            return rtnVal;
        }

        public string READ_YN(string argGbn, string argCode)
        {
            string rtnVal = "";

            if (argGbn == "1")
            {
                rtnVal = "1.정상,2.정밀검사";
            }
            else if (argGbn == "2")
            {
                switch (argCode)
                {
                    case "1":
                        rtnVal = "정상";
                        break;
                    case "2":
                        rtnVal = "정밀검사";
                        break;
                    default:
                        break;
                }
            }
            return rtnVal;
        }

        public string READ_YN2(string argGbn, string argCode)
        {
            string rtnVal = "";

            if (argGbn == "1")
            {
                rtnVal = "1.정상,2.정밀검사,3.측정불가";
            }
            else if (argGbn == "2")
            {
                switch (argCode)
                {
                    case "1":
                        rtnVal = "정상";
                        break;
                    case "2":
                        rtnVal = "정밀검사";
                        break;
                    case "3":
                        rtnVal = "측정불가";
                        break;
                    default:
                        break;
                }
            }
            return rtnVal;
        }

        public string READ_YN_NEW(string argGbn, string argCode)
        {
            string rtnVal = "";

            if (argGbn == "")
            {
                rtnVal = "1.정상,2.예방필요,3.정밀검사";
            }
            else if (argGbn == "2")
            {
                switch (argCode)
                {
                    case "1":
                        rtnVal = "정상";
                        break;
                    case "2":
                        rtnVal = "예방필요";
                        break;
                    case "3":
                        rtnVal = "정밀검사";
                        break;
                    default:
                        break;
                }
            }
            return rtnVal;
        }

        public string READ_Old1(string argGbn, string argCode)
        {
            string rtnVal = "";

            if (argGbn == "")
            {
                return rtnVal;
            }

            if (argGbn == "1")
            {
                rtnVal = "1.양호,2.개선필요";
            }
            else if (argGbn == "2")
            {
                switch (argCode)
                {
                    case "1":
                        rtnVal = "양호";
                        break;
                    case "2":
                        rtnVal = "개선필요";
                        break;
                    default:
                        break;
                }
            }
            return rtnVal;
        }

        public string READ_Old2(string argGbn, string argCode)
        {
            string rtnVal = "";

            if (argGbn == "")
            {
                return rtnVal;
            }

            if (argGbn == "1")
            {
                rtnVal = "1.양호,2.보통,3.불량";
            }
            else if (argGbn == "2")
            {
                switch (argCode)
                {
                    case "1":
                        rtnVal = "양호";
                        break;
                    case "2":
                        rtnVal = "보통";
                        break;
                    case "3":
                        rtnVal = "불량";
                        break;
                    default:
                        break;
                }
            }
            return rtnVal;
        }

        public string READ_YN_1(string argGbn, string argCode)
        {
            string rtnVal = "";

            if (argGbn == "")
            {
                return rtnVal;
            }

            if (argGbn == "1")
            {
                rtnVal = "1.무,2.유";
            }
            else if (argGbn == "2")
            {
                switch (argCode)
                {
                    case "1":
                        rtnVal = "무";
                        break;
                    case "2":
                        rtnVal = "유";
                        break;
                    default:
                        break;
                }
            }
            return rtnVal;
        }

        public string READ_PM(string argGbn, string argCode)
        {
            string rtnVal = "";

            if (argGbn == "")
            {
                return rtnVal;
            }

            if (argGbn == "1")
            {
                rtnVal = "1.음성,2.양성";
            }
            else if (argGbn == "2")
            {
                switch (argCode)
                {
                    case "1":
                        rtnVal = "음성";
                        break;
                    case "2":
                        rtnVal = "양성";
                        break;
                    default:
                        break;
                }
            }
            return rtnVal;
        }

        public string READ_Hear(string argGbn, string argCode)
        {
            string rtnVal = "";

            if (argGbn == "")
            {
                return rtnVal;
            }

            if (argGbn == "1")
            {
                rtnVal = "1.없음,2.중이염,3.외이도염,4.기타";
            }
            else if (argGbn == "2")
            {
                switch (argCode)
                {
                    case "1":
                        rtnVal = "없음";
                        break;
                    case "2":
                        rtnVal = "중이염";
                        break;
                    case "3":
                        rtnVal = "외이도염";
                        break;
                    case "4":
                        rtnVal = "기타";
                        break;
                    default:
                        break;
                }
            }
            return rtnVal;
        }

        public string READ_Neck(string argGbn, string argCode)
        {
            string rtnVal = "";

            if (argGbn == "")
            {
                return rtnVal;
            }

            if (argGbn == "1")
            {
                rtnVal = "1.없음,2.편도비대,3.임파절증대,4.갑상선비대,5.기타";
            }
            else if (argGbn == "2")
            {
                switch (argCode)
                {
                    case "1":
                        rtnVal = "없음";
                        break;
                    case "2":
                        rtnVal = "편도비대";
                        break;
                    case "3":
                        rtnVal = "임파절증대";
                        break;
                    case "4":
                        rtnVal = "갑상선비대";
                        break;
                    case "5":
                        rtnVal = "기타";
                        break;
                    default:
                        break;
                }
            }
            return rtnVal;
        }

        public string READ_Eye(string argGbn, string argCode)
        {
            string rtnVal = "";

            if (argGbn == "")
            {
                return rtnVal;
            }

            if (argGbn == "1")
            {
                rtnVal = "1.없음,2.결막염,3.눈썹찔림증,4.사시,5.기타";
            }
            else if (argGbn == "2")
            {
                switch (argCode)
                {
                    case "1":
                        rtnVal = "없음";
                        break;
                    case "2":
                        rtnVal = "결막염";
                        break;
                    case "3":
                        rtnVal = "눈썹찔림증";
                        break;
                    case "4":
                        rtnVal = "사시";
                        break;
                    case "5":
                        rtnVal = "기타";
                        break;
                    default:
                        break;
                }
            }
            return rtnVal;
        }

        public string READ_Nose(string argGbn, string argCode)
        {
            string rtnVal = "";

            if (argGbn == "")
            {
                return rtnVal;
            }

            if (argGbn == "1")
            {
                rtnVal = "1.없음,2.부비동염,3.비염,4.기타";
            }
            else if (argGbn == "2")
            {
                switch (argCode)
                {
                    case "1":
                        rtnVal = "없음";
                        break;
                    case "2":
                        rtnVal = "부비동염";
                        break;
                    case "3":
                        rtnVal = "비염";
                        break;
                    case "4":
                        rtnVal = "기타";
                        break;
                    default:
                        break;
                }
            }
            return rtnVal;
        }

        public string READ_Skin(string argGbn, string argCode)
        {
            string rtnVal = "";

            if (argGbn == "")
            {
                return rtnVal;
            }

            if (argGbn == "1")
            {
                rtnVal = "1.없음,2.아토피성피부염,3.전염성피부염,4.기타";
            }
            else if (argGbn == "2")
            {
                switch (argCode)
                {
                    case "1":
                        rtnVal = "없음";
                        break;
                    case "2":
                        rtnVal = "아토피성피부염";
                        break;
                    case "3":
                        rtnVal = "전염성피부염";
                        break;
                    case "4":
                        rtnVal = "기타";
                        break;
                    default:
                        break;
                }
            }
            return rtnVal;
        }

        public string READ_URO(string argGbn, string argCode)
        {
            string rtnVal = "";

            if (argGbn == "")
            {
                return rtnVal;
            }

            if (argGbn == "1")
            {
                rtnVal = "1.음성,2.약양성,3.+1,4.+2,5.+3,6.+4";
            }
            else if (argGbn == "2")
            {
                switch (argCode)
                {
                    case "1":
                        rtnVal = "음성";
                        break;
                    case "2":
                        rtnVal = "약양성";
                        break;
                    case "3":
                        rtnVal = "+1";
                        break;
                    case "4":
                        rtnVal = "+2";
                        break;
                    case "5":
                        rtnVal = "+3";
                        break;
                    case "6":
                        rtnVal = "+4";
                        break;
                    default:
                        break;
                }
            }
            return rtnVal;
        }

        public string READ_Xray(string argGbn, string argCode)
        {
            string rtnVal = "";

            if (argGbn == "")
            {
                return rtnVal;
            }

            if (argGbn == "1")
            {
                rtnVal = "1.정상(A),2.사진불량(B),3.비활동성(C),4.폐결핵경증(D-A),5.폐결핵중등증(D-B),6.폐결핵중증(D-C),7.폐결핵의증(E),8.비폐결핵성질환(F)#,9.순환기계질환(F)#심비대,10.진단미정,11.미촬영 ";
            }
            else if (argGbn == "2")
            {
                switch (argCode)
                {
                    case "1":
                        rtnVal = "정상(A)";
                        break;
                    case "2":
                        rtnVal = "사진불량(B)";
                        break;
                    case "3":
                        rtnVal = "비활동성(C)";
                        break;
                    case "4":
                        rtnVal = "폐결핵경증(D-A)";
                        break;
                    case "5":
                        rtnVal = "폐결핵중등증(D-B)";
                        break;
                    case "6":
                        rtnVal = "폐결핵중증(D-C)";
                        break;
                    case "7":
                        rtnVal = "폐결핵의증(E)";
                        break;
                    case "8":
                        rtnVal = "비폐결핵성질환(F)#";
                        break;
                    case "9":
                        rtnVal = "순환기계질환(F)#심비대";
                        break;
                    case "10":
                        rtnVal = "진단미정";
                        break;
                    case "11":
                        rtnVal = "11.미촬영";
                        break;
                    default:
                        break;
                }
            }
            return rtnVal;
        }

        public string READ_Mush(string argGbn, string argCode)
        {
            string rtnVal = "";

            if (argGbn == "")
            {
                return rtnVal;
            }

            argCode = string.Format("{0:00}", argCode);

            if (argGbn == "1")
            {
                rtnVal = "1.정상,2.흉부측만,3.흉부후만,4.흉부전만,5.오목가슴,6.새가슴,7.요통,8.어깨결림,9.발달장애,10.기타";
            }
            else if (argGbn == "2")
            {
                switch (argCode)
                {
                    case "1":
                        rtnVal = "정상";
                        break;
                    case "2":
                        rtnVal = "흉부측만";
                        break;
                    case "3":
                        rtnVal = "흉부후만";
                        break;
                    case "4":
                        rtnVal = "흉부전만";
                        break;
                    case "5":
                        rtnVal = "오목가슴";
                        break;
                    case "6":
                        rtnVal = "새가슴";
                        break;
                    case "7":
                        rtnVal = "요통";
                        break;
                    case "8":
                        rtnVal = "어깨결림";
                        break;
                    case "9":
                        rtnVal = "발달장애";
                        break;
                    case "10":
                        rtnVal = "기타";
                        break;
                    default:
                        break;
                }
            }
            return rtnVal;
        }

        /// <summary>
        /// 혈당(식전)
        /// </summary>
        /// <param name="argResult"></param>
        /// <returns></returns>
        public string READ_HYEOLDANG(string argResult)
        {
            string rtnVal = "";

            if (argResult == "")
            {
                return rtnVal;
            }

            if (long.Parse(argResult) >= 100)
            {
                rtnVal = "질환의심";
            }
            else
            {
                rtnVal = "정상";
            }
            return rtnVal;
        }

        /// <summary>
        /// READ_총콜레스테롤
        /// </summary>
        /// <param name="argResult"></param>
        /// <returns></returns>
        public string READ_TotalCholesterol(string argResult)
        {
            string rtnVal = "";

            if (argResult == "")
            {
                return rtnVal;
            }

            if (long.Parse(argResult) >= 170)
            {
                rtnVal = "질환의심";
            }
            else
            {
                rtnVal = "정상";
            }
            return rtnVal;
        }

        /// <summary>
        /// AST
        /// </summary>
        /// <param name="argResult"></param>
        /// <returns></returns>
        public string READ_AST(string argResult, long argAge)
        {
            string rtnVal = "";

            if (argResult == "")
            {
                return rtnVal;
            }

            if (argAge < 10)
            {
                if (long.Parse(argResult) >= 55)
                {
                    rtnVal = "질환의심";
                }
                else
                {
                    rtnVal = "정상";
                }
            }
            else
            {
                if (long.Parse(argResult) >= 45)
                {
                    rtnVal = "질환의심";
                }
                else
                {
                    rtnVal = "정상";
                }
            }
            return rtnVal;
        }

        /// <summary>
        /// ALT
        /// </summary>
        /// <param name="argResult"></param>
        /// <param name="argAge"></param>
        /// <returns></returns>
        public string READ_ALT(string argResult)
        {
            string rtnVal = "";

            if (argResult == "")
            {
                return rtnVal;
            }
            
            if (long.Parse(argResult) >= 45)
            {
                rtnVal = "질환의심";
            }
            else
            {
                rtnVal = "정상";
            }
            
            return rtnVal;
        }

        /// <summary>
        /// READ_혈색소
        /// </summary>
        /// <param name="argResult"></param>
        /// <returns></returns>
        public string READ_Hemoglobin(string argResult, string argSex)
        {
            string rtnVal = "";

            if (argResult == "")
            {
                return rtnVal;
            }

            if (argSex == "F")
            {
                if (long.Parse(argResult) >= 12)
                {
                    rtnVal = "정상";
                }
                else
                {
                    rtnVal = "질환의심";
                }
            }
            return rtnVal;
        }

        public string CALC_Chejil_Rate(double argWeight, double argHeight, long argAge, string argSex)
        {
            string rtnVal = "";
            double nChejil = 0;
            string strTable = "";
            int nRate = 0;
            string strBun = "";

            //체질량지수 = (몸무게 * 10, 000) / (키 * 키)
            nChejil = (argWeight * 10000) / (argHeight * argHeight);
            //소숫2째자리 반올림
            nChejil = double.Parse(string.Format("{0:###0.0}", nChejil));

            //남자 연령별 체질량지수 백분위
            if (argSex == "M")
            {
                switch (argAge)
                {
                    case 0:
                        strTable = "11.0/11.3/11.6/12.1/13.1/14.2/14.9/15.5/16.6";
                        break;
                    case 5:
                        strTable = "13.8/14.2/14.5/14.9/15.6/16.5/17.1/17.5/18.3";
                        break;
                    case 6:
                        strTable = "13.6/14.0/14.2/14.7/15.4/16.3/16.9/17.5/18.5";
                        break;
                    case 7:
                        strTable = "13.9/14.2/14.5/14.9/15.7/17.0/18.0/18.9/20.0";
                        break;
                    case 8:
                        strTable = "14.0/14.4/14.7/15.1/16.0/17.5/18.5/19.5/21.4";
                        break;
                    case 9:
                        strTable = "14.2/14.6/14.9/15.5/16.6/18.5/19.8/20.9/22.2";
                        break;
                    case 10:
                        strTable = "14.3/14.7/15.0/15.4/16.8/18.6/20.4/21.4/22.9";
                        break;
                    case 11:
                        strTable = "14.9/15.3/15.6/16.3/17.8/20.1/21.8/22.8/24.4";
                        break;
                    case 12:
                        strTable = "14.8/15.5/16.0/16.8/18.7/21.0/22.6/23.8/25.1";
                        break;
                    case 13:
                        strTable = "15.8/16.3/16.7/17.4/19.1/21.5/23.4/24.5/26.6";
                        break;
                    case 14:
                        strTable = "16.3/16.8/17.2/18.1/19.6/21.9/23.5/24.6/26.5";
                        break;
                    case 15:
                        strTable = "16.5/17.1/17.5/18.1/19.7/22.0/23.7/25.6/27.9";
                        break;
                    case 16:
                        strTable = "16.8/17.5/18.0/18.8/20.3/22.5/24.0/25.0/27.7";
                        break;
                    case 17:
                        strTable = "17.7/18.2/18.6/19.3/20.8/22.8/23.9/25.6/26.9";
                        break;                    
                    default:
                        if (argAge >= 18 && argAge <= 200)
                        {
                            strTable = "17.8/18.5/19.0/19.5/21.1/23.1/24.4/25.3/26.8";
                        }
                        break;
                }
            }
            else
            {
                switch (argAge)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        strTable = "10.9/11.4/11.8/12.3/13.2/14.2/14.9/15.4/16.1";
                        break;
                    case 5:
                        strTable = "13.6/14.0/14.3/14.6/15.4/16.3/16.8/17.1/17.8";
                        break;
                    case 6:
                        strTable = "13.3/13.8/14.1/14.6/15.3/16.2/16.7/17.3/18.3";
                        break;
                    case 7:
                        strTable = "13.5/13.9/14.1/14.6/15.6/16.7/17.5/17.9/19.0";
                        break;
                    case 8:
                        strTable = "13.6/13.9/14.2/14.7/15.6/17.0/17.9/18.6/19.9";
                        break;
                    case 9:
                        strTable = "13.8/14.2/14.5/15.0/16.1/17.6/18.8/19.7/21.1";
                        break;
                    case 10:
                        strTable = "14.0/14.4/14.8/15.3/16.6/18.4/19.8/20.8/21.8";
                        break;
                    case 11:
                        strTable = "14.2/14.7/15.1/15.8/17.4/19.6/21.0/21.7/23.1";
                        break;
                    case 12:
                        strTable = "14.8/15.4/15.8/16.7/18.4/20.2/21.5/22.5/23.9";
                        break;
                    case 13:
                        strTable = "15.5/16.2/16.6/17.5/19.4/21.0/22.9/23.7/25.1";
                        break;
                    case 14:
                        strTable = "16.0/16.7/17.3/18.1/19.9/21.9/23.1/24.5/26.3";
                        break;
                    case 15:
                        strTable = "16.6/17.3/17.9/18.8/20.3/22.2/23.5/24.6/26.2";
                        break;
                    case 16:
                        strTable = "17.3/17.9/18.4/19.0/20.7/22.7/23.9/24.7/26.2";
                        break;
                    case 17:
                        strTable = "17.6/18.1/18.5/19.1/20.8/22.2/23.5/24.8/26.1";
                        break;
                    default:
                        if (argAge >= 18 && argAge <= 200)
                        {
                            strTable = "17.9/18.3/18.7/19.5/20.9/22.6/23.6/24.5/26.0";
                        }
                        break;
                }
            }

            //표에서 백분위(%)를 찾음
            nRate = 10;
            for (int i = 1; i <= 9; i++)
            {
                if (VB.Pstr(strTable, "/", i).To<double>() >= nChejil)
                {
                    nRate = i;
                    break;
                }
            }

            switch (nRate)
            {
                case 1:
                    rtnVal = "1.(5%)정상";
                    break;
                case 2:
                    rtnVal = "1.(10%)정상";
                    break;
                case 3:
                    rtnVal = "1.(15%)정상";
                    break;
                case 4:
                    rtnVal = "1.(25%)정상";
                    break;
                case 5:
                    rtnVal = "1.(50%)정상";
                    break;
                case 6:
                    rtnVal = "1.(75%)정상";
                    break;
                case 7:
                    rtnVal = "2.(85%)위험군";
                    break;
                case 8:
                    rtnVal = "2.(90%)위험군";
                    break;
                case 9:
                    rtnVal = "2.(95%)위험군";
                    break;
                case 10:
                    rtnVal = "3.(95%초과)비만";
                    break;
                default:
                    break;
            }

            return rtnVal;
        }

        public void SignImage_Spread_Set(FarPoint.Win.Spread.FpSpread spd, int argRow, int argCol, string argSabun, string argAlign, string argDirPath, string argFile)
        {
            bool blnExist = false;
            string strFile = "";

            strFile = argDirPath + argFile;

            DirectoryInfo Dir = new DirectoryInfo(argDirPath);
            if (Dir.Exists == false)
            {
                Dir.Create();
            }
            else
            {
                FileInfo[] File = Dir.GetFiles(argFile, SearchOption.AllDirectories);

                foreach (FileInfo file in File)
                {
                    blnExist = true;
                    break;
                }
            }

            if (blnExist == true)
            {
                FileStream fs = new FileStream(strFile, FileMode.Open);
                Bitmap bmp = new Bitmap(fs);
                fs.Close();

                FarPoint.Win.Spread.CellType.TextCellType cellType = new FarPoint.Win.Spread.CellType.TextCellType();
                cellType.BackgroundImage = new FarPoint.Win.Picture(bmp, FarPoint.Win.RenderStyle.Stretch);
                spd.ActiveSheet.Cells[argRow, argCol].CellType = cellType;
            }
            else
            {
                spd.ActiveSheet.Cells[argRow, argCol].Text = "";
            }
        }

        /// <summary>
        /// 한글/영문 혼합 문자열 길이를 Return
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public int GetLength(string str)
        {
            int rtnVal = 0;

            if (str.IsNullOrEmpty())
            {
                return rtnVal;
            }

            byte[] temp = System.Text.Encoding.Default.GetBytes(str);

            rtnVal = temp.Length;

            return rtnVal;

        }

        /// <summary>
        /// 건진 History
        /// </summary>
        /// <param name="SpdNm"></param>
        public void fn_His_Screen_Display(FarPoint.Win.Spread.FpSpread SpdNm, string argJumin, long argPano)
        {
            int nRead = 0;
            string strData = "";
            string strJong = "";
            long nHeaPano = 0;

            //종검의 등록번호를 찾음
            nHeaPano = 0;

            if (!argJumin.IsNullOrEmpty())
            {
                nHeaPano = hicPatientService.GetPanobyJumin(clsAES.AES(argJumin));
            }

            //일반건진, 종합검진의 접수내역을 Display
            List<HIC_JEPSU> list = hicJepsuService.GetItembyOnlyPaNo(argPano, nHeaPano);

            nRead = list.Count;
            SpdNm.ActiveSheet.RowCount = nRead;
            for (int i = 0; i < nRead; i++)
            {
                strJong = list[i].GJJONG.Trim();

                SpdNm.ActiveSheet.Cells[i, 0].Text = list[i].JEPDATE;
                if (strJong == "XX")
                {
                    SpdNm.ActiveSheet.Cells[i, 1].Text = "종검";
                }
                else
                {
                    SpdNm.ActiveSheet.Cells[i, 1].Text = hb.READ_GjJong_Name(strJong);
                }
                SpdNm.ActiveSheet.Cells[i, 2].Text = list[i].WRTNO.ToString();
                SpdNm.ActiveSheet.Cells[i, 3].Text = list[i].GJJONG;
                SpdNm.ActiveSheet.Cells[i, 4].Text = list[i].GJCHASU;
            }
        }

        /// <summary>
        /// Directory 존재 여부 체크
        /// </summary>
        /// <param name="sDirPath"></param>
        /// <param name="sExe"></param>
        public void Dir_Check(string sDirPath, string sExe = "*.*")
        {
            DirectoryInfo Dir = new DirectoryInfo(sDirPath);
            if (Dir.Exists == false)
            {
                Dir.Create();
            }
            //else
            //{
            //    FileInfo[] File = Dir.GetFiles(sExe, SearchOption.AllDirectories);

            //    foreach (FileInfo file in File)
            //    {
            //        file.Delete();
            //    }
            //}
        }

        public void Excel_File_Create(string argPath, string argFileName, FarPoint.Win.Spread.FpSpread argSpdNm, FarPoint.Win.Spread.SheetView argSpdSheetView)
        {
            string strPath = "";
            bool bOK = false;

            Dir_Check(argPath);

            strPath = argFileName + ".xlsx";

            Cursor.Current = Cursors.WaitCursor;

            argSpdSheetView.Protect = false;
            argSpdNm.SaveExcel(strPath, FarPoint.Excel.ExcelSaveFlags.UseOOXMLFormat | FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders);

            MessageBox.Show(argFileName + ".xlsx 이 저장 되었습니다!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Cursor.Current = Cursors.Default;
        }

        #region HcMunjinNight.bas
        /// <summary>
        /// 야간작업 1차(불면증 지수) 선택값
        /// </summary>
        /// <param name="argNo"></param>
        /// <param name="argDate"></param>
        /// <returns></returns>
        public string Munjin_Night_Value1(long argNo, string argData)
        {
            string rtnVal = "";

            if (argNo <= 3)
            {
                switch (argData)
                {
                    case "1":
                        rtnVal = "없음";
                        break;
                    case "2":
                        rtnVal = "약간";
                        break;
                    case "3":
                        rtnVal = "중간";
                        break;
                    case "4":
                        rtnVal = "심함";
                        break;
                    case "5":
                        rtnVal = "매우심함";
                        break;
                    default:
                        rtnVal = "";
                        break;
                }
            }
            else if (argNo == 4)
            {
                switch (argData)
                {
                    case "1":
                        rtnVal = "매우만족";
                        break;
                    case "2":
                        rtnVal = "약간만족";
                        break;
                    case "3":
                        rtnVal = "그저그렇다";
                        break;
                    case "4":
                        rtnVal = "약간불만족";
                        break;
                    case "5":
                        rtnVal = "매우불만족";
                        break;
                    default:
                        rtnVal = "";
                        break;
                }
            }
            else
            {
                switch (argData)
                {
                    case "1":
                        rtnVal = "전혀";
                        break;
                    case "2":
                        rtnVal = "약간";
                        break;
                    case "3":
                        rtnVal = "다소";
                        break;
                    case "4":
                        rtnVal = "상당히";
                        break;
                    case "5":
                        rtnVal = "매우많이";
                        break;
                    default:
                        rtnVal = "";
                        break;
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 야간작업 2차(수면의질) 선택값
        /// </summary>
        /// <param name="argNo"></param>
        /// <param name="argData"></param>
        /// <returns></returns>
        public string Munjin_Night_Value2(long argNo, string argData)
        {
            string rtnVal = "";

            if (argNo == 1 || argNo == 3)
            {
                rtnVal = string.Format("{0:00}", argData) + "시";
            }
            else if (argNo == 2)
            {
                switch (argData)
                {
                    case "1":
                        rtnVal = "15분이내";
                        break;
                    case "2":
                        rtnVal = "16-30분";
                        break;
                    case "3":
                        rtnVal = "31-60분";
                        break;
                    case "4":
                        rtnVal = "60분이상";
                        break;
                    default:
                        rtnVal = "";
                        break;
                }
            }
            else if (argNo == 4)
            {
                switch (argData)
                {
                    case "1":
                        rtnVal = "7시간이상";
                        break;
                    case "2":
                        rtnVal = "6-7시간";
                        break;
                    case "3":
                        rtnVal = "5-6시간";
                        break;
                    case "4":
                        rtnVal = "5시간이하";
                        break;
                    default:
                        rtnVal = "";
                        break;
                }
            }
            else if ((argNo >= 5 && argNo <= 14) || argNo == 16 || argNo == 17)
            {
                switch (argData)
                {
                    case "1":
                        rtnVal = "없었다";
                        break;
                    case "2":
                        rtnVal = "주1회미만";
                        break;
                    case "3":
                        rtnVal = "주1-2회";
                        break;
                    case "4":
                        rtnVal = "주3회이상";
                        break;
                    default:
                        rtnVal = "";
                        break;
                }
            }
            else if (argNo == 15)
            {
                switch (argData)
                {
                    case "1":
                        rtnVal = "아주좋다";
                        break;
                    case "2":
                        rtnVal = "대체로좋다";
                        break;
                    case "3":
                        rtnVal = "대체로나쁘다";
                        break;
                    case "4":
                        rtnVal = "아주나쁘다";
                        break;
                    default:
                        rtnVal = "";
                        break;
                }
            }
            else if (argNo == 18)
            {
                switch (argData)
                {
                    case "1":
                        rtnVal = "전혀";
                        break;
                    case "2":
                        rtnVal = "별로";
                        break;
                    case "3":
                        rtnVal = "약간";
                        break;
                    case "4":
                        rtnVal = "매우";
                        break;
                    default:
                        rtnVal = "";
                        break;
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 야간작업 2차(주간졸림증) 선택값
        /// </summary>
        /// <param name="argNo"></param>
        /// <param name="argData"></param>
        /// <returns></returns>
        public string Munjin_Night_Value3(long argNo, string argData)
        {
            string rtnVal = "";

            if (argNo >= 1 && argNo <= 8)
            {
                switch (argData)
                {
                    case "1":
                        rtnVal = "전혀";
                        break;
                    case "2":
                        rtnVal = "조금";
                        break;
                    case "3":
                        rtnVal = "상당히";
                        break;
                    case "4":
                        rtnVal = "매우";
                        break;
                    default:
                        rtnVal = "";
                        break;
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 위장관질환(공통) 선택값
        /// </summary>
        /// <param name="argNo"></param>
        /// <param name="argDate"></param>
        /// <returns></returns>
        public string Munjin_Night_Value4(long argNo, string argData)
        {
            string rtnVal = "";

            if (argNo == 1 || argNo == 3 || argNo == 5)
            {
                switch (argData)
                {
                    case "1":
                        rtnVal = "전혀없음";
                        break;
                    case "2":
                        rtnVal = "한달에 하루 미만";
                        break;
                    case "3":
                        rtnVal = "한달에 하루";
                        break;
                    case "4":
                        rtnVal = "한달에 2-3일";
                        break;
                    case "5":
                        rtnVal = "일주일에 하루";
                        break;
                    case "6":
                        rtnVal = "일주일에 2일 이상";
                        break;
                    case "7":
                        rtnVal = "거의 매일";
                        break;
                    default:
                        rtnVal = "";
                        break;
                }
            }
            else
            {
                switch (argData)
                {
                    case "1":
                        rtnVal = "아니오";
                        break;
                    case "2":
                        rtnVal = "예";
                        break;
                    default:
                        rtnVal = "";
                        break;
                }
            }
            return rtnVal;
        }

        /// <summary>
        /// 유방암(여성) 선택값
        /// </summary>
        /// <param name="argNo"></param>
        /// <param name="argDate"></param>
        /// <returns></returns>
        public string Munjin_Night_Value5(long argNo, string argData)
        {
            string rtnVal = "";

            if (argNo == 1)
            {
                switch (argData)
                {
                    case "1":
                        rtnVal = "거의 한 적이 없다";
                        break;
                    case "2":
                        rtnVal = "몇 번은 한 적이 있다";
                        break;
                    case "3":
                        rtnVal = "거의 매번 지켰다";
                        break;
                    default:
                        rtnVal = "";
                        break;
                }
            }
            else if(argNo ==2 || argNo == 3 || argNo == 4 || argNo == 5)
            {
                switch (argData)
                {
                    case "1":
                        rtnVal = "아니오";
                        break;
                    case "2":
                        rtnVal = "예";
                        break;
                    default:
                        rtnVal = "";
                        break;
                }
            }
            else if (argNo == 6)
            {
                switch (argData)
                {
                    case "1":
                        rtnVal = "한 적이 없다";
                        break;
                    case "2":
                        rtnVal = "한 적이 있다";
                        break;
                    default:
                        rtnVal = "";
                        break;
                }

            }
            return rtnVal;
        }

        /// <summary>
        /// 야간작업 문진 판정값
        /// argGbn: 1.불면증지수 2.수면의질 3.주간졸림증
        /// </summary>
        /// <param name="argGbn"></param>
        /// <param name="argData"></param>
        /// <returns></returns>
        public string Munjin_Night_Panjeng(string argGbn, string argData)
        {
            string rtnVal = "";

            if (argGbn == "1")  //불면증지수
            {
                switch (argData)
                {
                    case "1":
                        rtnVal = "1.정상";
                        break;
                    case "2":
                        rtnVal = "2.경미한 불면증";
                        break;
                    case "3":
                        rtnVal = "3.중증도 불면증";
                        break;
                    case "4":
                        rtnVal = "4.심한 불면증";
                        break;
                    default:
                        rtnVal = "";
                        break;
                }
            }
            else if (argGbn == "2")  //수면의질
            {
                switch (argData)
                {
                    case "1":
                        rtnVal = "1.수면의 질에 문제없음";
                        break;
                    case "2":
                        rtnVal = "2.수면의 질에 문제있음";
                        break;
                    default:
                        rtnVal = "";
                        break;
                }
            }
            else if (argGbn == "3")  //주간졸림증
            {
                switch (argData)
                {
                    case "1":
                        rtnVal = "1.정상";
                        break;
                    case "2":
                        rtnVal = "2.중증도 주간졸림증";
                        break;
                    case "3":
                        rtnVal = "3.심한 주간졸림증";
                        break;
                    default:
                        rtnVal = "";
                        break;
                }
            }
            else
            {
                rtnVal = "";
            }

            return rtnVal;
        }
        #endregion

        public void fn_ProcessKill(string argName)
        {
            System.Diagnostics.Process[] ProcessEx = System.Diagnostics.Process.GetProcessesByName(argName);

            if (ProcessEx.Length > 0)
            {
                System.Diagnostics.Process[] Pro1 = System.Diagnostics.Process.GetProcessesByName(argName);
                System.Diagnostics.Process CurPro = System.Diagnostics.Process.GetCurrentProcess();
                foreach (System.Diagnostics.Process Proc in Pro1)
                {
                    if (Proc.Id != CurPro.Id)
                    {
                        ComFunc.KillProc(argName);
                    }
                }
            }
        }

        /// <summary>
        /// Process 검색 후 실행파일 실행
        /// </summary>
        /// <param name="argPath"></param>
        /// <param name="argFileName"></param>
        public void fn_Find_ProcessExecute(string argPath, string argFileName)
        {
            bool bOK = false;

            List<clsHcVariable.PROCESSENTRY32> processes = EnumProcesses();

            if (!processes.IsNullOrEmpty())
            {
                foreach (clsHcVariable.PROCESSENTRY32 pe32 in processes)
                {
                    if (pe32.szExeFile.ToLower() == argFileName)
                    {
                        bOK = true;
                        return;
                    }
                }
            }

            if (bOK == false)
            {
                VB.Shell(argPath + argFileName, "NormalFocus");
            }
        }

        public static List<clsHcVariable.PROCESSENTRY32> EnumProcesses()
        {
            IntPtr hSnapshot = clsHcVariable.CreateToolhelp32Snapshot(clsHcVariable.TH32CS_SNAPPROCESS, 0);
            if (hSnapshot == IntPtr.Zero)
            {
                return null;
            }

            clsHcVariable.PROCESSENTRY32 pe32 = new clsHcVariable.PROCESSENTRY32();
            pe32.dwSize = clsHcVariable.PROCESSENTRY32.Size;
            if (clsHcVariable.Process32First(hSnapshot, ref pe32) == 0)
            {
                clsHcVariable.CloseHandle(hSnapshot);
                return null;
            }

            List<clsHcVariable.PROCESSENTRY32> lstProcesses = new List<clsHcVariable.PROCESSENTRY32>();
            do
            {
                lstProcesses.Add(pe32);
            } while (clsHcVariable.Process32Next(hSnapshot, ref pe32) != 0);

            clsHcVariable.CloseHandle(hSnapshot);
            return lstProcesses;
        }

        public bool fn_Find_Process(string argFileName)
        {
            bool rtnVal = false;

            List<clsHcVariable.PROCESSENTRY32> processes = EnumProcesses();

            if (!processes.IsNullOrEmpty())
            {
                foreach (clsHcVariable.PROCESSENTRY32 pe32 in processes)
                {
                    if (pe32.szExeFile.ToLower() == argFileName)
                    {
                        rtnVal = true;
                        return rtnVal;
                    }
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// OS 정보 읽기
        /// </summary>
        /// <returns></returns>
        public string fn_Find_OS_version()
        {
            string rtnVal = "";
            string releaseId = "";
            string ProductName = "";

            releaseId = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ReleaseId", "").To<string>(); 
            ProductName = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName", "").To<string>();

            if (releaseId != "Error")
            {
                for (int i = 1; i < GetLength(releaseId); i++)
                {
                    if (VB.Asc(VB.Mid(releaseId, i, 1)) == 0)
                    {
                        break;
                    }
                }

                //clsPublic.GstrOSVER = VB.Left(releaseId, GetLength(releaseId) - 1);
                //rtnVal = VB.Left(releaseId, GetLength(releaseId) - 1);
                clsPublic.GstrOSVER = VB.Left(ProductName, GetLength(ProductName) - 1);
                rtnVal = VB.Left(ProductName, GetLength(ProductName) - 1);
            }

            if (releaseId == "Error")
            {
                //rtnVal = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ReleaseId", "").To<string>();
                rtnVal = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName", "").To<string>();
                if (rtnVal == "Error")
                {
                    rtnVal = "Microsoft Windows 98";
                }

                //clsPublic.GstrOSVER = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ReleaseId", "").To<string>();
                clsPublic.GstrOSVER = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName", "").To<string>();
                if (clsPublic.GstrOSVER == "Error")
                {
                    clsPublic.GstrOSVER = "Microsoft Windows 98";
                }
            }

            //rtnVal = releaseId;
            rtnVal = ProductName;

            return rtnVal;
        }

        public string Read_HPhone(long argWrtNo)
        {
            string rtnVal = "";
            string strPhone = "";
            
            if (argWrtNo == 0)
            {
                return rtnVal;
            }

            rtnVal = hicPatientService.GetHphonebyWrtNo(argWrtNo);

            return rtnVal;
        }

        /// <summary>
        /// 전자동의서 작성 Main Function
        /// </summary>
        /// <param name="eCon"></param>
        /// <param name="eParam"></param>
        public void fn_Emr_Consent(EMR_CONSENT eCon)
        {
            //Form ActiveFormWrite = null;
            
            frmEasViewer ActiveFormWrite = null;
            
            EmrForm fWrite = null;
            EmrPatient AcpEmr = null;
            
            //EMR 환자정보 세팅
            AcpEmr = clsEmrChart.ClearPatient();
            AcpEmr = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, eCon.PTNO, "O", eCon.EXDATE.Replace("-", ""), eCon.DEPT);

            if (AcpEmr == null)
            {
                AcpEmr = new EmrPatient()
                {
                    ptNo = eCon.PTNO,
                    medFrDate = eCon.EXDATE.Replace("-", ""),
                    medDeptCd = eCon.DEPT,
                    medDrCd = ocsDoctorService.GetDrCodebySabun(eCon.DRNO),
                    medDrName = ocsDoctorService.GetDrNamebySabun(eCon.DRNO),
                    inOutCls = "O"
                };
            }
            else
            {
                //SetEmrPatInfoOcs에서는 외래, 입원, EMR_TREATT 테이블을 참조하여 의사세팅 함.
                //검진의사는 7101, 7102로 세팅되어있으므로 동의서 작성의사로 재설정함.
                AcpEmr.medDrCd = ocsDoctorService.GetDrCodebySabun(eCon.DRNO);
                AcpEmr.medDrName = ocsDoctorService.GetDrNamebySabun(eCon.DRNO);
            }

            //서식지 정보 Form에 세팅
            fWrite = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, eCon.FORMNO.To<string>(""), eCon.UPDATENO.To<string>(""));

            if (fWrite == null)
            {
                MessageBox.Show("등록된 기록지 폼이 없습니다.");
                fWrite = clsEmrChart.ClearEmrForm();
                return;
            }

            //서식지작성용 파라미터 매칭
            EasParam eParam= EasParamByFormCD(eCon);

            EasManager easManager = EasManager.Instance;

            ActiveFormWrite = easManager.GetEasFormViewer();
            
            long formDataId = easManager.GetFormDataId(fWrite, AcpEmr,0);

            if (formDataId > 0)
            {
                easManager.Update(fWrite, AcpEmr, formDataId);
            }
            else
            {
                easManager.Write(fWrite, AcpEmr, eParam);
            }

            easManager.ShowTabletMoniror(eParam, formDataId);   //수검자 작성용 화면

            ActiveFormWrite.IsAutoCloseBySave = true;
            ActiveFormWrite.exitDelegate += EasFormViewr_exitDelegate;
            ActiveFormWrite.Show();                             //의사 설명용 화면
            ActiveFormWrite.LocationToRight();
        }

        /// <summary>
        /// 동의서 서식지 별 argument 매칭
        /// </summary>
        /// <param name="eCon"></param>
        /// <returns></returns>
        public EasParam EasParamByFormCD(EMR_CONSENT eCon)
        {
            string strTag1 = "", strTag2 = "", strTag3 = "", strTag4 = "", strTag5 = "", strTag6 = "", strTag7 = "", strTag8 = "", strTag9 = "" , strTag10 = "";

            EasParam eParam = new EasParam();

            if (eCon.FORMCODE == "D10")         //위내시경
            {
                if (eCon.MUNDATA.ToString().Trim() != "")
                {
                    if (eCon.MUNDATA[0].ToString()  == "1") { strTag1  = clsHcType.MUNJIN11; } else { strTag1  = clsHcType.MUNJIN12; }
                    if (eCon.MUNDATA[3].ToString()  == "1") { strTag2  = clsHcType.MUNJIN21; } else { strTag2  = clsHcType.MUNJIN22; }
                    if (eCon.MUNDATA[6].ToString()  == "1") { strTag3  = clsHcType.MUNJIN31; } else { strTag3  = clsHcType.MUNJIN32; }
                    if (eCon.MUNDATA[9].ToString()  == "1") { strTag4  = clsHcType.MUNJIN41; } else { strTag4  = clsHcType.MUNJIN42; }
                    if (eCon.MUNDATA[12].ToString() == "1") { strTag5  = clsHcType.MUNJIN51; } else { strTag5  = clsHcType.MUNJIN52; }
                    if (eCon.MUNDATA[15].ToString() == "1") { strTag6  = clsHcType.MUNJIN61; } else { strTag6  = clsHcType.MUNJIN62; }
                    if (eCon.MUNDATA[18].ToString() == "1") { strTag7  = clsHcType.MUNJIN71; } else { strTag7  = clsHcType.MUNJIN72; }
                    if (eCon.MUNDATA[21].ToString() == "1") { strTag8  = clsHcType.MUNJIN81; } else { strTag8  = clsHcType.MUNJIN82; }
                    if (eCon.MUNDATA[24].ToString() == "1") { strTag9  = clsHcType.MUNJIN91; } else { strTag9  = clsHcType.MUNJIN92; }
                    if (eCon.MUNDATA[27].ToString() == "1") { strTag10 = clsHcType.MUNJIN01; } else { strTag10 = clsHcType.MUNJIN02; }

                    eParam.ControlInit = "inputbox_1581408496701*" + eCon.STOMACHDATE + ",inputbox_1581408418699*" + eCon.DEPTNAME
                    + strTag1 + ",inputbox_1581409383124*" + eCon.MUNDATA[2]     //기왕력
                    + strTag2 + ",inputbox_1581409387268*" + eCon.MUNDATA[5]     //알레르기
                    + strTag3 + ",inputbox_1581409444395*" + eCon.MUNDATA[8]     //당뇨병
                    + strTag4 + ",inputbox_1581409549349*" + eCon.MUNDATA[11]    //저고혈압
                    + strTag5 + ",inputbox_1581409690676*" + eCon.MUNDATA[14]    //호흡기질환
                    + strTag6 + ",inputbox_1581409703699*" + eCon.MUNDATA[17]    //심장질환
                    + strTag7 + ",inputbox_1581409695805*" + eCon.MUNDATA[20]    //신장질환
                    + strTag8 + ",inputbox_1581409706890*" + eCon.MUNDATA[23]    //출혈소인
                    + strTag9 + ",inputbox_1581409699216*" + eCon.MUNDATA[26]    //약물중독
                    + strTag10                                                   //치아상태
                    + ",inputbox_1581409682854*" + eCon.MUNDATA[29];             //기타
                }
                else
                {
                    eParam.ControlInit = "inputbox_1581408496701*" + eCon.STOMACHDATE + ",inputbox_1581408418699*" + eCon.DEPTNAME;
                }
            }   
            else if (eCon.FORMCODE == "D20")    //대장내시경
            {
                if (eCon.MUNDATA.ToString().Trim() != "")
                {
                    if (eCon.MUNDATA[0].ToString() == "1") { strTag1 = clsHcType.MUNJIN11; } else { strTag1 = clsHcType.MUNJIN12; }
                    if (eCon.MUNDATA[3].ToString() == "1") { strTag2 = clsHcType.MUNJIN21; } else { strTag2 = clsHcType.MUNJIN22; }
                    if (eCon.MUNDATA[6].ToString() == "1") { strTag3 = clsHcType.MUNJIN31; } else { strTag3 = clsHcType.MUNJIN32; }
                    if (eCon.MUNDATA[9].ToString() == "1") { strTag4 = clsHcType.MUNJIN41; } else { strTag4 = clsHcType.MUNJIN42; }
                    if (eCon.MUNDATA[12].ToString() == "1") { strTag5 = clsHcType.MUNJIN51; } else { strTag5 = clsHcType.MUNJIN52; }
                    if (eCon.MUNDATA[15].ToString() == "1") { strTag6 = clsHcType.MUNJIN61; } else { strTag6 = clsHcType.MUNJIN62; }
                    if (eCon.MUNDATA[18].ToString() == "1") { strTag7 = clsHcType.MUNJIN71; } else { strTag7 = clsHcType.MUNJIN72; }
                    if (eCon.MUNDATA[21].ToString() == "1") { strTag8 = clsHcType.MUNJIN81; } else { strTag8 = clsHcType.MUNJIN82; }
                    if (eCon.MUNDATA[24].ToString() == "1") { strTag9 = clsHcType.MUNJIN91; } else { strTag9 = clsHcType.MUNJIN92; }
                    if (eCon.MUNDATA[27].ToString() == "1") { strTag10 = clsHcType.MUNJIN01; } else { strTag10 = clsHcType.MUNJIN02; }

                    eParam.ControlInit = "inputbox_1581408496701*" + eCon.STOMACHDATE + ",inputbox_1581408418699*" + eCon.DEPTNAME
                    + strTag1 + ",inputbox_1581409383124*" + eCon.MUNDATA[2]     //기왕력
                    + strTag2 + ",inputbox_1581409387268*" + eCon.MUNDATA[5]     //알레르기
                    + strTag3 + ",inputbox_1581409444395*" + eCon.MUNDATA[8]     //당뇨병
                    + strTag4 + ",inputbox_1581409549349*" + eCon.MUNDATA[11]    //저고혈압
                    + strTag5 + ",inputbox_1581409690676*" + eCon.MUNDATA[14]    //호흡기질환
                    + strTag6 + ",inputbox_1581409703699*" + eCon.MUNDATA[17]    //심장질환
                    + strTag7 + ",inputbox_1581409695805*" + eCon.MUNDATA[20]    //신장질환
                    + strTag8 + ",inputbox_1581409706890*" + eCon.MUNDATA[23]    //출혈소인
                    + strTag9 + ",inputbox_1581409699216*" + eCon.MUNDATA[26]    //약물중독
                    + ",inputbox_1581409682854*" + eCon.MUNDATA[29];             //기타
                }
                else
                {
                    eParam.ControlInit = "inputbox_1581408496701*" + eCon.STOMACHDATE + ",inputbox_1581408418699*" + eCon.DEPTNAME;
                }
            }
            else if (eCon.FORMCODE == "D40")    //조영제동의서
            {
                eParam.ControlInit = "inputbox_1580450335113*" + eCon.EXNAME + ",inputbox_1581408418699*" + eCon.DEPTNAME + ",inputbox_1580450282051*" + eCon.EXDATE + ",inputbox_1580450339113*" + eCon.DEPTNAME;
            }
            else if (eCon.FORMCODE == "D50")    //개인정보동의서
            {
                //자동입력
            }
            else if (eCon.FORMCODE == "D52")    //정보활용동의서
            {
                eParam.ControlInit = "checkbox_1609139700135*" + eCon.GJJONG11_YN + ",checkbox_1609139704868*" + eCon.GJJONG31_YN + ",inputboxtel_1609142266850*" + eCon.JUMINNO1 + ",inputboxtel_1609142271875*" + eCon.JUMINNO2;
            }
            else if (eCon.FORMCODE == "D53")    //건강검진 동시진행 동의서
            {
                eParam.ControlInit = "checkbox_1609143063765*1";
            }
            else if (eCon.FORMCODE == "D54")    //건강진단 개인표
            {
                eParam.ControlInit = "inputbox_1609143862721*" + eCon.SNAME + "inputbox_1614060310363*" + eCon.HPHONE + ",inputbox_1609143602400*" + eCon.LTDNAME + ",inputbox_1614060080803*" + eCon.BUSENAME;
                eParam.ControlInit += "inputbox_1614060057805*" + eCon.IPSADATE + ",inputboxtel_1609144339097*" + eCon.JUMINNO1 + ",inputboxtel_1609144342351*" + eCon.JUMINNO2 + "textarea_1614062040863*" + eCon.JUSO;
                eParam.ControlInit += "textarea_1614060377936*" + eCon.UCODENAMES + "inputbox_1614060595152*" + eCon.BUSE + "inputbox_1614060664082*" + eCon.GONGJENG + "inputbox_1614060592760*" + eCon.BUSEIPSA;
                eParam.ControlInit += "inputbox_1614060653303*" + eCon.P_GIGAN + "inputbox_1614060738588" + eCon.P_GIGAN_1DAY + "inputbox_1614061055265" + eCon.WORK_GONGJENG1 + "inputbox_1614061051172" + eCon.WORK_YEAR1;
                eParam.ControlInit += "eas-date_1614061758204" + eCon.WORK_YEAR1 + "inputbox_1614061057357" + eCon.WORK_GONGJENG2 + "inputbox_1614061063262" + eCon.WORK_YEAR2 + "eas-date_1614061758204" + eCon.WORK_DYAS2;
                eParam.ControlInit += "textarea_1614061498419" + eCon.PYOJANGGI + "textarea_1614062202647" + eCon.JINSOGEN;

            }
            else if (eCon.FORMCODE == "D55")    //자궁세포암 검사 시술 설명 및 동의서
            {
                //자동입력
            }
            else if (eCon.FORMCODE == "D56")    //생물학적 노출지표검사 (소변)
            {
                eParam.ControlInit = "inputbox_1609222448999*" + eCon.AGREEDATE + ",inputbox_1609145859223*" + eCon.LTDNAME + ",inputboxtel_1609145875161*" + eCon.JUMINNO1 + ",inputboxtel_1609145877930*" + eCon.JUMINNO2;
            }
            else if (eCon.FORMCODE == "D57")    //생물학적 노출지표검사 (혈액)
            {
                eParam.ControlInit = "inputbox_1609146923572*" + eCon.AGREEDATE + ",inputbox_1609146908451*" + eCon.LTDNAME + ",inputboxtel_1609146939430*" + eCon.JUMINNO1 + ",inputboxtel_1609146944827*" + eCon.JUMINNO2;
            }



            return eParam;
        }

        /// <summary>
        /// 개인정보 동의서 종료 이벤트
        /// </summary>
        public void EasFormViewr_exitDelegate(EmrForm emrForm, EmrPatient ePAT, string formDataId)
        {
            var t = Task.Delay(1000);
            t.Wait();

            string strFrmCD = "";
            string strDrSabun = "";

            EasManager easManager = EasManager.Instance;

            long formId = easManager.GetFormDataId(emrForm, ePAT, 0);

            //작성이 완료되었으면
            if (formId > 0)
            {
                strFrmCD = hicConsentService.GetFormCDByFormNo(emrForm.FmFORMNO);
                strDrSabun = ocsDoctorService.GetSabunByDrCode(ePAT.medDrCd);

                long nConCnt = hicConsentService.GetListByFormNoPtnoDate(emrForm.FmFORMNO, ePAT.ptNo, ePAT.medFrDate, ePAT.medDeptCd);

                if (nConCnt > 0)
                {
                    HIC_CONSENT item = new HIC_CONSENT
                    {
                        PTNO = ePAT.ptNo,
                        DRSABUN = strDrSabun.To<long>(0),
                        FORMCODE = strFrmCD,
                        SDATE = ePAT.medFrDate,
                        DEPTCODE = ePAT.medDeptCd,
                        EMRNO = formId
                    };

                    int result = hicConsentService.UpdateItem2(item);

                    if (result < 0)
                    {
                        MessageBox.Show("동의서 작성기록 UPDATE 중 오류발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                }
            }
            
        }

        /// <summary>
        /// 숫자를 한글로 표현 K.M.C
        /// </summary>
        /// <param name="lngNumber"></param>
        /// <returns></returns>
        public string Number2Hangle(long lngNumber)
        {
            string[] NumberChar  = new string[] { "", "일", "이", "삼"
                                               , "사", "오", "육"
                                               , "칠", "팔", "구" };
            string[] LevelChar   = new string[] { "", "십", "백", "천" };
            string[] DecimalChar = new string[] { "", "만", "억", "조", "경" };

            string strMinus = string.Empty;

            if (lngNumber < 0)
            {
                strMinus = "마이너스";
                lngNumber *= -1;
            }

            string strValue = string.Format("{0}", lngNumber);
            string NumToKorea = string.Empty;
            bool UseDecimal = false;

            if (lngNumber == 0) return "영";

            for (int i = 0; i < strValue.Length; i++)
            {
                int Level = strValue.Length - i;
                if (strValue.Substring(i, 1) != "0")
                {
                    UseDecimal = true;
                    if (((Level - 1) % 4) == 0)
                    {
                        if (DecimalChar[(Level - 1) / 4] != string.Empty
                           && strValue.Substring(i, 1) == "1")
                            NumToKorea = NumToKorea + DecimalChar[(Level - 1) / 4];
                        else
                            NumToKorea = NumToKorea
                                              + NumberChar[int.Parse(strValue.Substring(i, 1))]
                                              + DecimalChar[(Level - 1) / 4];
                        UseDecimal = false;
                    }
                    else
                    {
                        if (strValue.Substring(i, 1) == "1")
                            NumToKorea = NumToKorea
                                               + LevelChar[(Level - 1) % 4];
                        else
                            NumToKorea = NumToKorea
                                               + NumberChar[int.Parse(strValue.Substring(i, 1))]
                                               + LevelChar[(Level - 1) % 4];
                    }
                }
                else
                {
                    if ((Level % 4 == 0) && UseDecimal)
                    {
                        NumToKorea = NumToKorea + DecimalChar[Level / 4];
                        UseDecimal = false;
                    }
                }
            }
            return strMinus + NumToKorea;
        }
    }
}

