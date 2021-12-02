using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.SupXray;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ComSupLibB.Com
{
    /// <summary>
    /// Class Name      : ComSupLibB.Com
    /// File Name       : frmComSupPRT01.cs
    /// Description     : 기능검사 결과 출력관련 공통 폼
    /// Author          : 윤조연
    /// Create Date     : 2017-07-25
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\ocs\nvc\Frm결과지인쇄.frm(Frm결과지인쇄) >> frmComSupPRT01.cs c#에서 폼 추가함" />
    public partial class frmComSupPRT01 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic();
        clsSpread spread = new clsSpread();
        clsPrint cp = new clsPrint();
        clsComSup sup = new clsComSup();
        

        string  gROWID="";
        string gPrintName = "";

        Int32 PrtX = 0;
        Int32 PrtY = 0;

        #endregion

        void setCtrlInit()
        {
            clsCompuInfo.SetComputerInfo();
            DataTable dt = ComQuery.Select_BAS_PCCONFIG(clsDB.DbCon, clsCompuInfo.gstrCOMIP, "프로그램PC세팅", "프린트설정", "내시경인쇄XY");

            if (ComFunc.isDataTableNull(dt) == false)
            {
                string s = dt.Rows[0]["VALUEV"].ToString();
                if (s != "")
                {
                    PrtX = Convert.ToInt32(clsComSup.setP(s, ",", 1).Trim());
                    PrtY = Convert.ToInt32(clsComSup.setP(s, ",", 2).Trim());
                }

            }
        }

        public frmComSupPRT01()
        {
            InitializeComponent();
            setCtrlInit();
        }

        public frmComSupPRT01(clsComSupSpd.enmPrtType prtType, string PrtName, string pRowId = "", string pano = "", string bDate = "", string specNo = "", bool prePrint = false)
        {
            InitializeComponent();
            setCtrlInit();

            gROWID =pRowId;
            gPrintName = PrtName;

            if (prtType == clsComSupSpd.enmPrtType.NVC)
            {
                if (setNVC(clsDB.DbCon, ssNvc,gROWID)==true)
                {
                    SpreadPrint(this.ssNvc, prePrint);
                }
            }
           

        }

        public frmComSupPRT01(clsComSupSpd.enmPrtType prtType, string PrtName, clsComSupSpd.cComSupPRT_ENDOBar argCls , string gdrname = "")
        {
            InitializeComponent();
            setCtrlInit();

            read_sysdate();

            gPrintName = PrtName;
            
            if (prtType == clsComSupSpd.enmPrtType.ENDO_BAR)
            {
                if (setENDO_BAR(ssEndo1, argCls , gdrname) ==true)
                {
                    SpreadPrint(this.ssEndo1, false);
                }
            }

        }

        /// <summary>
        /// 2019-09-27 안정수 추가, 조영제 출력에 사용
        /// </summary>
        /// <param name="prtType"></param>
        /// <param name="PrtName"></param>
        /// <param name="argCls"></param>
        public frmComSupPRT01(clsComSupSpd.enmPrtType prtType, string PrtName, clsComSupXraySpd.cComSupPRT_XRAYCont argCls)
        {
            InitializeComponent();
            setCtrlInit();

            read_sysdate();

            gPrintName = PrtName;

            if (prtType == clsComSupSpd.enmPrtType.ENDO_BAR)
            {
                if (setXrayCon_BAR(ssEndo1, argCls) == true)
                {
                    SpreadPrint(this.ssEndo1, false);
                }
            }

        }

        public frmComSupPRT01(clsComSupSpd.enmPrtType prtType, string PrtName, clsComSupSpd.cComSupPRT_ENDO_Pathology argCls)
        {
            InitializeComponent();
            setCtrlInit();

            Print_PATHOLOGY( prtType, PrtName,  argCls);


        }

        public frmComSupPRT01(clsComSupSpd.enmPrtType prtType, string PrtName, clsComSupXraySQL.cXrayDetail[] argCls)
        {
            InitializeComponent();
            setCtrlInit();

            read_sysdate();

            gPrintName = PrtName;

            if (prtType == clsComSupSpd.enmPrtType.XRAY_PRT)
            {
                PrtX = 20;
                PrtY = 0;

                if (setXRAY_PRT(ssXray, argCls) == true)
                {
                    //고객보관용
                    SpreadPrint(this.ssXray, false);
                }

                ComFunc.Delay(500);

                if (setXRAY_PRT(ssXray, argCls) == true)
                {
                    //영상의학과용
                    setXRAY_PRT_clear(this.ssXray, "A1");                    
                    SpreadPrint(this.ssXray, false);
                }
                               
            }

        }

        
        public void SpreadPrint(FpSpread o, bool prePrint)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {                
                return;
            }
            else
            {
                string sPrtName = ""; //선택 프린트명
                string bPrtName = ""; //기본 프린트명
                if (gPrintName.ToUpper() =="기본프린트")
                {
                    sPrtName = clsPrint.gGetDefaultPrinter();
                }
                else
                {
                    sPrtName = cp.getPrinter_Chk(gPrintName.ToUpper());
                }                

                if (sPrtName != "")
                {
                    bPrtName = clsPrint.gGetDefaultPrinter();
                    clsPrint.gSetDefaultPrinter(sPrtName);

                    string header = string.Empty;
                    string foot = string.Empty;
                                        
                    clsSpread.SpdPrint_Margin margin = new clsSpread.SpdPrint_Margin(0, 0, PrtY + 0, 0, PrtX + 0, 0);
                    clsSpread.SpdPrint_Option option = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait
                                                    , PrintType.All, 0, 0, false, false, false, false, false, false, false);

                    spread.setSpdPrint(o, prePrint, margin, option, header, foot);
                    
                    ComFunc.Delay(500);
                    clsPrint.gSetDefaultPrinter(bPrtName);

                }

            }
            
        }

        void Print_PATHOLOGY(clsComSupSpd.enmPrtType prtType, string PrtName, clsComSupSpd.cComSupPRT_ENDO_Pathology argCls)
        {
            read_sysdate();

            gPrintName = PrtName;

            try
            {
                if (prtType == clsComSupSpd.enmPrtType.ENDO_PATHOL)
                {
                    PrtX = 60;
                    PrtY = 90;

                    if (argCls.GbJob == "2")
                    {
                        if (argCls.Result6 != "")
                        {
                            argCls.Gubun = "1";
                            setENDO_PATHOLOGY_clear(ssPathology);
                            if (setENDO_PATHOLOGY(ssPathology, argCls) == true)
                            {
                                SpreadPrint(this.ssPathology, false);
                            }
                        }
                        if (argCls.Result62 != "")
                        {
                            argCls.Gubun = "2";
                            setENDO_PATHOLOGY_clear(ssPathology);
                            if (setENDO_PATHOLOGY(ssPathology, argCls) == true)
                            {
                                SpreadPrint(this.ssPathology, false);
                            }
                        }
                        if (argCls.Result63 != "")
                        {
                            argCls.Gubun = "3";
                            setENDO_PATHOLOGY_clear(ssPathology);
                            if (setENDO_PATHOLOGY(ssPathology, argCls) == true)
                            {
                                SpreadPrint(this.ssPathology, false);
                            }
                        }
                    }
                    else if (argCls.GbJob == "3")
                    {
                        if (argCls.Result6 != "")
                        {
                            argCls.Gubun = "1";
                            setENDO_PATHOLOGY_clear(ssPathology);
                            if (setENDO_PATHOLOGY(ssPathology, argCls) == true)
                            {
                                SpreadPrint(this.ssPathology, false);
                            }
                        }
                        if (argCls.Result62 != "" || argCls.Result63 != "")
                        {
                            argCls.Gubun = "2";
                            setENDO_PATHOLOGY_clear(ssPathology);
                            if (setENDO_PATHOLOGY(ssPathology, argCls) == true)
                            {
                                SpreadPrint(this.ssPathology, false);
                            }
                        }
                    }
                    else
                    {
                        if (setENDO_PATHOLOGY(ssPathology, argCls) == true)
                        {
                            SpreadPrint(this.ssPathology, false);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }

        /// <summary>
        /// NVC결과지 출력
        /// </summary>
        /// <param name="Spd"></param>
        /// <param name="argROWID"></param>
        /// <returns></returns>
        bool setNVC(PsmhDb pDbCon, FpSpread Spd,string argROWID)
        {
            bool b = true;
            DataTable dt = null;
            string strTemp = "";

            dt = sup.sel_ETC_RESULT_NVC(pDbCon, "", "", "", "", argROWID);

            if (ComFunc.isDataTableNull(dt) == false)
            {
                #region NVC-View

                Spd.ActiveSheet.Rows[31].Visible = false;

                Spd.ActiveSheet.Cells[3, 2].Text = dt.Rows[0]["PtNo"].ToString().Trim();
                Spd.ActiveSheet.Cells[4, 2].Text = dt.Rows[0]["SName"].ToString().Trim();
                Spd.ActiveSheet.Cells[4, 4].Text = dt.Rows[0]["RDate"].ToString().Trim();
                Spd.ActiveSheet.Cells[5, 2].Text = dt.Rows[0]["Age"].ToString().Trim() + "/" + dt.Rows[0]["Sex"].ToString().Trim();

                strTemp = VB.Space(10);
                if (dt.Rows[0]["PtNo"].ToString().Trim()=="Y")
                {
                    strTemp += "유";
                }
                else if (dt.Rows[0]["PtNo"].ToString().Trim() == "N")
                {
                    strTemp += "무";
                }
                //Raynaud Phenomenon
                strTemp +=  VB.Space(10) + "onset: ";
                strTemp +=  dt.Rows[0]["Onset"].ToString().Trim() + "세";
                Spd.ActiveSheet.Cells[6, 2].Text = strTemp;

                //진단명
                Spd.ActiveSheet.Cells[7, 2].Text =  VB.Space(2) + dt.Rows[0]["Diagnosis"].ToString().Trim();


                //검사부위①
                strTemp =  VB.Space(2);
                if (dt.Rows[0]["Part11"].ToString().Trim()!="")
                {
                    if (dt.Rows[0]["Part11"].ToString().Trim()=="R")
                    {
                        strTemp += "Right ";
                    }
                    else if (dt.Rows[0]["Part11"].ToString().Trim() == "L")
                    {
                        strTemp += "Left ";
                    }
                    else if (dt.Rows[0]["Part11"].ToString().Trim() == "B")
                    {
                        strTemp += "Both ";
                    }

                    if (dt.Rows[0]["Part12"].ToString().Trim() == "4")
                    {
                        strTemp += "4th finger";
                    }
                    else if (dt.Rows[0]["Part12"].ToString().Trim() == "5")
                    {
                        strTemp += "5th finger";
                    }
                    else if (dt.Rows[0]["Part12"].ToString().Trim() == "B")
                    {
                        strTemp += "4,5th finger";
                    }

                    if (dt.Rows[0]["Part21"].ToString().Trim() =="")
                    {
                        Spd.ActiveSheet.Cells[9, 2].Text = strTemp;
                    }
                    else if (dt.Rows[0]["Part21"].ToString().Trim() == "")
                    {
                        Spd.ActiveSheet.Cells[8, 2].Text = strTemp;
                    }                    
                }

                //검사부위②
                strTemp = VB.Space(2);
                if (dt.Rows[0]["Part21"].ToString().Trim() != "")
                {
                    if (dt.Rows[0]["Part21"].ToString().Trim() == "R")
                    {
                        strTemp += "Right ";
                    }
                    else if (dt.Rows[0]["Part21"].ToString().Trim() == "L")
                    {
                        strTemp += "Left ";
                    }
                    else if (dt.Rows[0]["Part21"].ToString().Trim() == "B")
                    {
                        strTemp += "Both ";
                    }

                    if (dt.Rows[0]["Part22"].ToString().Trim() == "4")
                    {
                        strTemp += "4th finger";
                    }
                    else if (dt.Rows[0]["Part22"].ToString().Trim() == "5")
                    {
                        strTemp += "5th finger";
                    }
                    else if (dt.Rows[0]["Part22"].ToString().Trim() == "B")
                    {
                        strTemp += "4,5th finger";
                    }
                                        
                    Spd.ActiveSheet.Cells[9, 2].Text = strTemp;
                   
                }

                strTemp = VB.Space(2);
                if (dt.Rows[0]["Part23"].ToString().Trim() != "")
                {
                    if (dt.Rows[0]["Part23"].ToString().Trim() == "1")
                    {
                        strTemp += "(pitting scar)";
                    }
                    else if (dt.Rows[0]["Part23"].ToString().Trim() == "2")
                    {
                        strTemp += "(active ulcer)";
                    }
                    else if (dt.Rows[0]["Part23"].ToString().Trim() == "3")
                    {
                        strTemp += "(gangrene)";
                    }
                    Spd.ActiveSheet.Cells[10, 2].Text = strTemp;
                }

                //Findings-①
                strTemp = "①Giant capillaries";
                if (dt.Rows[0]["Findings11"].ToString().Trim() == "1")
                {
                    strTemp += " (homogeneously enlaged) : ";
                }
                else if(dt.Rows[0]["Findings11"].ToString().Trim() == "2")
                {
                    strTemp += " (irregulary enlaged) : ";
                }
                else
                {
                    strTemp += ": ";
                }
                if (dt.Rows[0]["Findings12"].ToString().Trim() == "1")
                {
                    strTemp += "▶None";
                }
                else if (dt.Rows[0]["Findings12"].ToString().Trim() == "2")
                {
                    strTemp += "▶A few";
                }
                else if (dt.Rows[0]["Findings12"].ToString().Trim() == "3")
                {
                    strTemp += "▶Moderate";
                }
                else if (dt.Rows[0]["Findings12"].ToString().Trim() == "4")
                {
                    strTemp += "▶Frequent";
                }
                Spd.ActiveSheet.Cells[17, 1].Text = strTemp;

                //Findings-②
                strTemp = "②Architectural arrangement : ";
                if (dt.Rows[0]["Findings2"].ToString().Trim() == "1")
                {
                    strTemp += "▶well preserved";
                }
                else if (dt.Rows[0]["Findings2"].ToString().Trim() == "2")
                {
                    strTemp += "▶mild disorganization";
                }
                else if (dt.Rows[0]["Findings2"].ToString().Trim() == "3")
                {
                    strTemp += "▶Moderate disorganization";
                }
                else if (dt.Rows[0]["Findings2"].ToString().Trim() == "4")
                {
                    strTemp += "▶severe disorganization";
                }
                Spd.ActiveSheet.Cells[18, 1].Text = strTemp;

                //Findings-③
                strTemp = "③Loss of capillaries (<30 over 5mm in the distal row of nailfold) : ";
                if (dt.Rows[0]["Findings3"].ToString().Trim() == "1")
                {
                    strTemp += "▶None";
                }
                else if (dt.Rows[0]["Findings3"].ToString().Trim() == "2")
                {
                    strTemp += "▶Mild";
                }
                else if (dt.Rows[0]["Findings3"].ToString().Trim() == "3")
                {
                    strTemp += "▶Moderate";
                }
                else if (dt.Rows[0]["Findings3"].ToString().Trim() == "4")
                {
                    strTemp += "▶Severe";
                }
                Spd.ActiveSheet.Cells[19, 1].Text = strTemp;

                //Findings-④
                strTemp = "④Avascular area : ";
                if (dt.Rows[0]["Findings4"].ToString().Trim() == "1")
                {
                    strTemp += "▶None";
                }
                else if (dt.Rows[0]["Findings4"].ToString().Trim() == "2")
                {
                    strTemp += "▶Some";
                }
                else if (dt.Rows[0]["Findings4"].ToString().Trim() == "3")
                {
                    strTemp += "▶Moderate";
                }
                else if (dt.Rows[0]["Findings4"].ToString().Trim() == "4")
                {
                    strTemp += "▶Extensive";
                }
                Spd.ActiveSheet.Cells[20, 1].Text = strTemp;

                //Findings-⑤
                strTemp = "⑤Hemorrhage : ";
                if (dt.Rows[0]["Findings5"].ToString().Trim() == "1")
                {
                    strTemp += "▶None";
                }
                else if (dt.Rows[0]["Findings5"].ToString().Trim() == "2")
                {
                    strTemp += "▶Some";
                }
                else if (dt.Rows[0]["Findings5"].ToString().Trim() == "3")
                {
                    strTemp += "▶Moderate";
                }
                else if (dt.Rows[0]["Findings5"].ToString().Trim() == "4")
                {
                    strTemp += "▶Frequent";
                }
                Spd.ActiveSheet.Cells[21, 1].Text = strTemp;

                //Findings-⑥
                strTemp = "⑥Ramified capillaries (Angiogenesis) : ";
                if (dt.Rows[0]["Findings6"].ToString().Trim() == "1")
                {
                    strTemp += "▶None";
                }
                else if (dt.Rows[0]["Findings6"].ToString().Trim() == "2")
                {
                    strTemp += "▶A few";
                }
                else if (dt.Rows[0]["Findings6"].ToString().Trim() == "3")
                {
                    strTemp += "▶Moderate";
                }
                else if (dt.Rows[0]["Findings6"].ToString().Trim() == "4")
                {
                    strTemp += "▶Frequent";
                }
                Spd.ActiveSheet.Cells[22, 1].Text = strTemp;

                //Conclusions
                strTemp = dt.Rows[0]["Conclusions"].ToString().Trim();
                if (strTemp !="")
                {
                    for (int i = 1; i < clsComSup.setL(strTemp,"\r\n"); i++)
                    {
                        if (i>4)
                        {
                            break;
                        }
                        Spd.ActiveSheet.Cells[24+i, 1].Text = "  " + clsComSup.setP(strTemp,"\r\n",i);
                    }
                }


                #endregion                
            }
            else
            {
                ComFunc.MsgBox("조회 조건에 해당하는 데이터가 존재 하지 않습니다.");
                b = false;
            }

            return b;
        }

        bool setENDO_BAR(FpSpread Spd, clsComSupSpd.cComSupPRT_ENDOBar argCls , string gdrname = "")
        {
            bool b = true;

            #region 내시경 바코드

            spread.Spread_Clear(Spd, Spd.ActiveSheet.RowCount, Spd.ActiveSheet.ColumnCount);

            Spd.ActiveSheet.Cells[1, 1].Text = argCls.strPano + " " + argCls.strSName + " " + argCls.strSexAge;

            if (argCls.strType == "")
            {
                Spd.ActiveSheet.Cells[2, 1].Text = argCls.strSuCode + "(IV)" + " (" + argCls.strDrName + ")" + " " + gdrname ;
            }
            else
            {
                Spd.ActiveSheet.Cells[2, 1].Text = argCls.strSuCode + "(" + argCls.strType + ")" + " (" + argCls.strDrName + ")" + " " + gdrname;
            }

            Spd.ActiveSheet.Cells[3, 1].Text = argCls.strSuName;
            Spd.ActiveSheet.Cells[4, 1].Text = cpublic.strSysDate.Replace("-","/");

            #endregion

            return b;
        }

        /// <summary>
        /// 2019-09-27 안정수 추가(조영제 바코드 출력시 사용)
        /// </summary>
        /// <param name="Spd"></param>
        /// <param name="argCls"></param>
        /// <returns></returns>
        bool setXrayCon_BAR(FpSpread Spd, clsComSupXraySpd.cComSupPRT_XRAYCont argCls)
        {
            bool b = true;

            #region 내시경 바코드

            spread.Spread_Clear(Spd, Spd.ActiveSheet.RowCount, Spd.ActiveSheet.ColumnCount);

            Spd.ActiveSheet.Cells[1, 1].Text = argCls.strPano + " " + argCls.strSName + " " + argCls.strSexAge;
            
            Spd.ActiveSheet.Cells[2, 1].Text = argCls.strSuCode;            

            Spd.ActiveSheet.Cells[3, 1].Text = argCls.strSuName;
            Spd.ActiveSheet.Cells[4, 1].Text = cpublic.strSysDate.Replace("-", "/");

            #endregion

            return b;
        }

        bool setENDO_PATHOLOGY(FpSpread Spd, clsComSupSpd.cComSupPRT_ENDO_Pathology argCls)
        {
            bool b = true;
            DataTable dt = null;
            string strResult1 = "";
            string strResult2 = "";
            string strResult3 = "";
            string strResult4 = "";
            string strResult5 = "";
            string strResult6 = "";

            #region 내시경 의뢰지

            //spread.Spread_Clear(Spd, Spd.ActiveSheet.RowCount, Spd.ActiveSheet.ColumnCount);

            if (argCls.SuCode !="")
            {
                Spd.ActiveSheet.Cells[3, 0].Text =  ComFunc.LeftH("의무기록번호 : " + argCls.Ptno + VB.Space(38),38) + VB.Left("병리번호 : " + VB.Space(20),20) + " <BIOPSY>";
            }
            else
            {
                Spd.ActiveSheet.Cells[3, 0].Text = ComFunc.LeftH("의무기록번호 : " + argCls.Ptno + VB.Space(38), 38) + "병리번호 : " ;
            }            
            if (argCls.SuCode != "")
            {
                if (argCls.GbJob == "1")
                {
                    Spd.ActiveSheet.Cells[4, 0].Text = ComFunc.LeftH("성        명 : " + argCls.SName + VB.Space(38), 38) + "성  별 / 연 령 : " + argCls.Sex + "/" + argCls.Age + VB.Space(3) + " BFS : " + argCls.SuCode;
                }
                else if (argCls.GbJob == "2")
                {
                    Spd.ActiveSheet.Cells[4, 0].Text = ComFunc.LeftH("성        명 : " + argCls.SName + VB.Space(38), 38) + "성  별 / 연 령 : " + argCls.Sex + "/" + argCls.Age + VB.Space(3) + " GFS : " + argCls.SuCode;
                }
                else if (argCls.GbJob == "3")
                {
                    Spd.ActiveSheet.Cells[4, 0].Text = ComFunc.LeftH("성        명 : " + argCls.SName + VB.Space(38), 38) + "성  별 / 연 령 : " + argCls.Sex + "/" + argCls.Age + VB.Space(3) + " CFS : " + argCls.SuCode;
                }
                else if (argCls.GbJob == "4")
                {
                    Spd.ActiveSheet.Cells[4, 0].Text = ComFunc.LeftH("성        명 : " + argCls.SName + VB.Space(38), 38) + "성  별 / 연 령 : " + argCls.Sex + "/" + argCls.Age + VB.Space(3) + " GFS : " + argCls.SuCode;
                }

            }
            else
            {
                Spd.ActiveSheet.Cells[4, 0].Text = ComFunc.LeftH("성        명 : " + argCls.SName + VB.Space(38), 38) + "성  별 / 연 령 : " + argCls.Sex + "/" + argCls.Age;
            }
            Spd.ActiveSheet.Cells[5, 0].Text = ComFunc.LeftH("주 민  번 호 : " + argCls.Jumin + VB.Space(38), 38) + "진료과 / 병 동 : " + argCls.DeptCode + "("+ argCls.DrName +")"+ "/" + argCls.Room;
            Spd.ActiveSheet.Cells[6, 0].Text = ComFunc.LeftH("검사요청지 발행일 : " + argCls.RDate + VB.Space(38), 38) + "Free : ";
            Spd.ActiveSheet.Cells[7, 0].Text = "Chief Complaints : " + argCls.Remark1 + "\r\n" + "Clinical Diagnosis : " + argCls.Remark4;
            
            #region //결과읽기
            if (argCls.Result1 !="")
            {
                if (VB.Asc(VB.Mid(argCls.Result1.Trim(), VB.Len(argCls.Result1)-1,1)) == 13)
                {
                    strResult1 = VB.Mid(argCls.Result1.Trim(),1,VB.Len(argCls.Result1) -2);
                }
                else
                {
                    strResult1 = argCls.Result1.Trim();
                    if (argCls.GbJob =="3")
                    {
                        strResult1 = "<small Intestinal>   " + argCls.Result1.Trim();
                    }
                }
            }
            if (argCls.Result2 != "")
            {
                if (VB.Asc(VB.Mid(argCls.Result2.Trim(), VB.Len(argCls.Result2) - 1, 1)) == 13)
                {
                    strResult2 = VB.Mid(argCls.Result2.Trim(), 1, VB.Len(argCls.Result2) - 2);
                }
                else
                {
                    strResult2 = argCls.Result2.Trim();                    
                }
            }
            if (argCls.Result3 != "")
            {
                if (VB.Asc(VB.Mid(argCls.Result3.Trim(), VB.Len(argCls.Result3) - 1, 1)) == 13)
                {
                    strResult3 = VB.Mid(argCls.Result3.Trim(), 1, VB.Len(argCls.Result3) - 2);
                }
                else
                {
                    strResult3 = argCls.Result3.Trim();
                }
            }
            if (argCls.Result5 != "")
            {
                if (VB.Asc(VB.Mid(argCls.Result5.Trim(), VB.Len(argCls.Result5) - 1, 1)) == 13)
                {
                    strResult5 = VB.Mid(argCls.Result5.Trim(), 1, VB.Len(argCls.Result5) - 2);
                }
                else
                {
                    strResult5 = argCls.Result5.Trim();
                    if (argCls.GbJob == "3")
                    {
                        strResult5 = "<rectum>   " + argCls.Result5.Trim();
                    }
                }
            }
            if (argCls.Result4 != "")
            {
                if (VB.Asc(VB.Mid(argCls.Result4.Trim(), VB.Len(argCls.Result4) - 1, 1)) == 13)
                {
                    strResult4 = VB.Mid(argCls.Result4.Trim(), 1, VB.Len(argCls.Result4) - 2);
                }
                else
                {
                    strResult4 = argCls.Result4.Trim();
                    if (argCls.GbJob == "3")
                    {
                        strResult4 = "<large Intestinal>    " + argCls.Result4.Trim();
                    }
                }
            }
            if (argCls.Result6 != "" || argCls.Result62 != "" || argCls.Result63 != "")
            {
                if (argCls.GbJob =="2" && argCls.Gubun =="1")
                {
                    if (VB.Asc(VB.Mid(argCls.Result6.Trim(), VB.Len(argCls.Result6) - 1, 1)) == 13)
                    {
                        strResult6 = VB.Mid(argCls.Result6.Trim(), 1, VB.Len(argCls.Result6) - 2);
                    }
                    else
                    {
                        strResult6 = argCls.Result6.Trim();
                    }
                }
                else if (argCls.GbJob == "2" && argCls.Gubun == "2")
                {
                    if (VB.Asc(VB.Mid(argCls.Result62.Trim(), VB.Len(argCls.Result62) - 1, 1)) == 13)
                    {
                        strResult6 = VB.Mid(argCls.Result62.Trim(), 1, VB.Len(argCls.Result62) - 2);
                    }
                    else
                    {
                        strResult6 = argCls.Result62.Trim();
                    }
                }
                else if (argCls.GbJob == "2" && argCls.Gubun == "3")
                {
                    if (VB.Asc(VB.Mid(argCls.Result63.Trim(), VB.Len(argCls.Result63) - 1, 1)) == 13)
                    {
                        strResult6 = VB.Mid(argCls.Result63.Trim(), 1, VB.Len(argCls.Result63) - 2);
                    }
                    else
                    {
                        strResult6 = argCls.Result63.Trim();
                    }
                }
                else if (argCls.GbJob == "3" && argCls.Gubun == "1")
                {
                    if (VB.Asc(VB.Mid(argCls.Result6.Trim(), VB.Len(argCls.Result6) - 1, 1)) == 13)
                    {
                        strResult6 = VB.Mid(argCls.Result6.Trim(), 1, VB.Len(argCls.Result6) - 2);
                    }
                    else
                    {
                        strResult6 = argCls.Result6.Trim();
                    }
                }
                else if (argCls.GbJob == "3" && argCls.Gubun == "2")
                {
                    strResult6 = "";

                    if (VB.Asc(VB.Mid(argCls.Result62.Trim(), VB.Len(argCls.Result62) - 1, 1)) == 13)
                    {
                        strResult6 += VB.Mid(argCls.Result62.Trim(), 1, VB.Len(argCls.Result62) - 2);
                    }
                    else
                    {
                        strResult6 += argCls.Result62.Trim();
                    }
                    if (argCls.Result63!="")
                    {
                        if (VB.Asc(VB.Mid(argCls.Result63.Trim(), VB.Len(argCls.Result63) - 1, 1)) == 13)
                        {
                            if (strResult6 != "")
                            {
                                strResult6 += "\r\n";
                            }
                            strResult6 += VB.Mid(argCls.Result63.Trim(), 1, VB.Len(argCls.Result63) - 2);
                        }
                        else
                        {
                            if (strResult6 != "")
                            {
                                strResult6 += "\r\n";
                            }
                            strResult6 += argCls.Result63.Trim();
                        }
                    }
                    

                }
                else
                {
                    if (VB.Asc(VB.Mid(argCls.Result6.Trim(), VB.Len(argCls.Result6) - 1, 1)) == 13)
                    {
                        strResult6 = VB.Mid(argCls.Result6.Trim(), 1, VB.Len(argCls.Result6) - 2);
                    }
                    else
                    {
                        strResult6 = argCls.Result6.Trim();
                    }
                }
                
            }
            
            #endregion
            Spd.ActiveSheet.Cells[9, 0].Text = strResult6;
            //높이재설정
            if (strResult6=="" || Spd.ActiveSheet.Rows.Get(9).Height  + 10  < 50)
            {
                Spd.ActiveSheet.Rows.Get(9).Height = 50 + 30;
            }
            else
            {
                Spd.ActiveSheet.Rows.Get(9).Height = Spd.ActiveSheet.Rows[9].GetPreferredHeight()+30;
            }
            
            
            if (argCls.GbJob == "2")
            {
                if (argCls.Gubun == "1")
                {
                    Spd.ActiveSheet.Cells[11, 0].Text = strResult1 + "\r\n" ;
                }
                else if (argCls.Gubun == "2")
                {
                    Spd.ActiveSheet.Cells[11, 0].Text = strResult2 + "\r\n";
                }
                else if (argCls.Gubun == "3")
                {
                    Spd.ActiveSheet.Cells[11, 0].Text = strResult3 + "\r\n";
                }
                Spd.ActiveSheet.Rows.Get(11).Height = Spd.ActiveSheet.Rows[11].GetPreferredHeight() + 30;
            }
            else if (argCls.GbJob == "3")
            {
                string s = ""; // strResult1 + "\r\n" + strResult4 + "\r\n" + strResult5 + "\r\n" + strResult3 + "\r\n";
                if (strResult1!="")
                {
                    s += strResult1 + "\r\n";
                }
                if (strResult4 != "")
                {
                    s += strResult4 + "\r\n";
                }
                if (strResult5 != "")
                {
                    s += strResult5 + "\r\n";
                }
                if (strResult3 != "")
                {
                    s += strResult3 + "\r\n";
                }

                Spd.ActiveSheet.Cells[11, 0].Text = s;

                Spd.ActiveSheet.Rows.Get(11).Height = Spd.ActiveSheet.Rows[11].GetPreferredHeight() + 30;
            }
            else if (argCls.GbJob == "4")
            {
                Spd.ActiveSheet.Cells[11, 0].Text = strResult1 + "\r\n" + strResult2 + "\r\n" + strResult3 + "\r\n";
                Spd.ActiveSheet.Rows.Get(11).Height = Spd.ActiveSheet.Rows[11].GetPreferredHeight() + 30;
            }
            //높이재설정
            if (Spd.ActiveSheet.Rows.Get(11).Height + 10 < 50)
            {
                Spd.ActiveSheet.Rows.Get(11).Height = 50 + 30;
            }
            else
            {
                Spd.ActiveSheet.Rows.Get(11).Height = Spd.ActiveSheet.Rows[11].GetPreferredHeight()+30;
            }

            if (argCls.GbJob == "1" || argCls.GbJob == "2" || argCls.GbJob == "4")
            {
                Spd.ActiveSheet.Cells[15, 0].Text = strResult4 + "\r\n";
            }
            else if (argCls.GbJob == "3" )
            {
                Spd.ActiveSheet.Cells[15, 0].Text = strResult2 + "\r\n";
            }
            //높이재설정
            if (Spd.ActiveSheet.Rows.Get(15).Height + 10 < 50)
            {
                Spd.ActiveSheet.Rows.Get(15).Height = 50 + 15;
            }
            else
            {
                Spd.ActiveSheet.Rows.Get(15).Height = Spd.ActiveSheet.Rows[15].GetPreferredHeight() + 15;
            }

            if (argCls.GbJob =="2" || argCls.GbJob == "3")
            {
                Spd.ActiveSheet.Cells[18, 0].Text = VB.Space(75) + "Name of Doctor in Charge :" + clsVbfunc.GetOCSDrNameSabun(clsDB.DbCon, argCls.RDrName);
            }
            else
            {
                Spd.ActiveSheet.Cells[18, 0].Text = "Date : " + argCls.RDate + VB.Space(35) + "Name of Doctor in Charge :" + clsVbfunc.GetOCSDrNameSabun(clsDB.DbCon, argCls.RDrName);
            }
            
            Spd.ActiveSheet.Cells[19, 0].Text = "Signature :";
            #region //의사 사인이미지
            try
            {
                dt = sup.sel_OCS_DOCTOR(clsDB.DbCon, argCls.RDrName);
                if (ComFunc.isDataTableNull(dt) == false)
                {
                    byte[] bImage = null;

                    bImage = (byte[])dt.Rows[0]["SIGNATURE"];
                    if (bImage != null)
                    {
                        Spd.ActiveSheet.Cells[19, 1].Value = new Bitmap(new MemoryStream(bImage));
                    }
                }

            }
            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
            #endregion
            
            #endregion

            return b;
        }

        void setENDO_PATHOLOGY_clear(FpSpread Spd)
        {
            #region 내시경 의뢰지

            Spd.ActiveSheet.Cells[3, 0].Text = "";
            Spd.ActiveSheet.Cells[4, 0].Text = "";
            Spd.ActiveSheet.Cells[5, 0].Text = "";
            Spd.ActiveSheet.Cells[6, 0].Text = "";
            Spd.ActiveSheet.Cells[7, 0].Text = "";
            Spd.ActiveSheet.Cells[9, 0].Text = ""; 
            Spd.ActiveSheet.Cells[11, 0].Text = "";
            Spd.ActiveSheet.Cells[15, 0].Text = "";
            Spd.ActiveSheet.Cells[18, 0].Text = "";
            Spd.ActiveSheet.Cells[19, 0].Text = "";
            
            #endregion

        }

        bool setXRAY_PRT(FpSpread Spd, clsComSupXraySQL.cXrayDetail[] argCls)
        {
            bool b = true;
            int i = 0;
            string s = string.Empty;
            string strPo = string.Empty;
            string strCR = string.Empty;

            #region 영상의학과 접수증

            setXRAY_PRT_clear(Spd,"");

            Spd.ActiveSheet.Cells[1, 1].Text = argCls[0].Pano; //바코드
            Spd.ActiveSheet.Cells[1, 3].Text = VB.Left(clsComSup.setP(argCls[0].EnterDate," ",2),5);
            Spd.ActiveSheet.Cells[3, 1].Text = argCls[0].SName +"("+ argCls[0].Pano + ")";
            Spd.ActiveSheet.Cells[4, 1].Text = "나이(성별):" + argCls[0].Age + "(" + argCls[0].Sex + ")";
            Spd.ActiveSheet.Cells[5, 1].Text = "(고객보관용)";
            Spd.ActiveSheet.Cells[6, 1].Text = "주민번호:" + argCls[0].Jumin;
            Spd.ActiveSheet.Cells[7, 1].Text = "예약일자:" + argCls[0].SeekDate;
            Spd.ActiveSheet.Cells[8, 1].Text = "접수일자:" + VB.Left(argCls[0].EnterDate,16);

            for (i = 0; i < argCls.Length; i++)
            {
                strCR = " ";
                if ( VB.Left(argCls[i].XCode,2) == "GR")
                {
                    strCR = " ▶ ";
                }

                strPo = "";
                if (argCls[i].GbPort =="P")
                {
                    strPo = "Po)";
                }

                s += strPo + argCls[i].OrderName + " " +  argCls[i].Remark + " " + argCls[i].PacsNo + strCR + argCls[i].XCode + "\r\n" ;
            }

            Spd.ActiveSheet.Cells[10, 1].Text = s;

            //높이 재설정
            Spd.ActiveSheet.Rows.Get(10).Height = Spd.ActiveSheet.Rows[10].GetPreferredHeight() + 15;
            
            #endregion

            return b;
        }

        void setXRAY_PRT_clear(FpSpread Spd,string argJob)
        {
            int i = 0;

            if (argJob =="")
            {
                for (i = 0; i < 14; i++)
                {
                    if (i != 12)
                    {
                        Spd.ActiveSheet.Cells[i, 1].Text = "";
                    }

                }

                Spd.ActiveSheet.Cells[1, 3].Text = "";
            }
            else if (argJob == "A1")
            {
                Spd.ActiveSheet.Cells[5, 1].Text = "";
            }

        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

    }
}
