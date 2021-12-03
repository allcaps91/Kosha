using System.Data;
using System.IO;
using ComBase; //기본 클래스
using ComBase.Controls;
using ComDbB; //DB연결

namespace ComEmrBase
{
    public class clsFormPrint
    {
        /// <summary>
        /// 기록지구분(J : 진단서, C : 기록지)
        /// </summary>
        public static string mstrFalg = "C"; 
        /// <summary>
        /// 출력구분(0:원외용, 1:원내용, 2:심사용)
        /// </summary>
        public static string mstrPRINTFLAG = "0";

        /// <summary>
        /// 작성일자
        /// </summary>
        public static string mstrWRITEDATE = string.Empty;    
        /// <summary>
        /// 작성시간
        /// </summary>
        public static string mstrWRITETIME = string.Empty;    
        /// <summary>
        /// 작성자
        /// </summary>
        public static string mstrWRITENAME = string.Empty;    
        /// <summary>
        /// 출력일자
        /// </summary>
        public static string mstrPRINTDATE = string.Empty;    
        /// <summary>
        /// 출력자
        /// </summary>
        public static string mstrPRINTNAME = string.Empty;
        /// <summary>
        /// OCR NO
        /// </summary>
        public static string mstrOcrNo = string.Empty;

        /// <summary>
        /// 설명의사, 설명일시 출력 여부
        /// </summary>
        public static bool mExplicate = false;

        /// <summary>
        /// 확인자
        /// </summary>
        public static string mstrCOMPUSENAME = string.Empty;

        /// <summary>
        /// 이미지 변환 폴더
        /// </summary>
        public static string mstrCONVIMAGEPATH = string.Empty;    //

        private static string mstrEmrNo = "0";
        private static string mstrFormNo = "0";
        private static string mstrUpdateNo = "0";

        //-->진단서 관련
        public static string mstrMcrtMcNo = string.Empty;   //연번호
        //<--진단서 관련

        private static EmrPatient pPrint = null;
        private static EmrForm fPrint = null;

        private static FormPrinting15.FormPrinting15 fp = null;


        public static void PrintForm(PsmhDb pDbCon, string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, System.Windows.Forms.Control[] Arryf)
        {
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            mstrEmrNo = strEmrNo;

            pPrint = clsEmrChart.ClearPatient();
            fPrint = clsEmrChart.ClearEmrForm();
            if (po != null)
            {
                pPrint = po;
            }

            SetInitInfo(pDbCon);

            fp = new FormPrinting15.FormPrinting15(Arryf);
            SetPropertiesEx();
            fp.Print();

        }

        /// <summary>
        /// Rtf 출력용 함수
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="po"></param>
        /// <param name="strEmrNo"></param>
        /// <param name="pCon">리치텍스트 부모 패널</param>
        /// <param name="PrintCon">리치텍스트박스</param>
        /// <param name="strFalg"></param>
        /// <returns></returns>
        public static int PrintRtf(PsmhDb pDbCon, string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, System.Windows.Forms.Control pCon, System.Windows.Forms.Control PrintCon,  string strFalg)
        {
            int rtnVal = 0;
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            mstrEmrNo = strEmrNo;
            mstrFalg = strFalg;

            pPrint = clsEmrChart.ClearPatient();
            fPrint = clsEmrChart.ClearEmrForm();
            if (po != null)
            {
                pPrint = po;
            }

            try
            {
                ((mtsPanel15.mPanel)pCon).VerticalScroll.Value = 0;
            }
            catch { }

            SetInitInfo(pDbCon);

            fp = new FormPrinting15.FormPrinting15(pCon);
            SetPropertiesEx();
            rtnVal = fp.PrintRtf(PrintCon);
            fp = null;
            return rtnVal;
        }

        public static int PrintFormLong(PsmhDb pDbCon, string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, System.Windows.Forms.Control pCon, string strFalg)
        {
            int rtnVal = 0;
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            mstrEmrNo = strEmrNo;
            mstrFalg = strFalg;

            pPrint = clsEmrChart.ClearPatient();
            fPrint = clsEmrChart.ClearEmrForm();
            if (po != null)
            {
                pPrint = po;
            }

            try
            {
                if (pCon.Name == "panChart") ((mtsPanel15.mPanel)pCon).VerticalScroll.Value = 0;
            }
            catch { }

            SetInitInfo(pDbCon);

            fp = new FormPrinting15.FormPrinting15(pCon);
            SetPropertiesEx();
            rtnVal = fp.PrintLong();
            fp = null;
            return rtnVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strFormNo">기록지번호</param>
        /// <param name="strUpdateNo">기록지 업데이트번호</param>
        /// <param name="po">환자정보</param>
        /// <param name="strEmrNo">EMRNO</param>
        /// <param name="pCon">panChart</param>
        /// <param name="strFalg">기록지 구분</param>
        /// <param name="Explicate">설명의사 출력 여부 기본: false</param>
        /// <returns></returns>
        public static int PrintFormAgreement(PsmhDb pDbCon, string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, System.Windows.Forms.Control pCon, string strFalg, bool Explicate = true)
        {
            int rtnVal = 0;
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            mstrEmrNo = strEmrNo;
            mstrFalg = strFalg;
            mExplicate = Explicate;

            pPrint = clsEmrChart.ClearPatient();
            fPrint = clsEmrChart.ClearEmrForm();
            if (po != null)
            {
                pPrint = po;
            }

            try
            {
                if (pCon.Name == "panChart") ((mtsPanel15.mPanel)pCon).VerticalScroll.Value = 0;
            }
            catch { }

            SetInitInfo(pDbCon);
            mstrOcrNo = po != null && po.ptNo.NotEmpty() ? clsEmrQuery.GetOcrNo(fPrint, pPrint, 0) :  string.Empty;

            fp = new FormPrinting15.FormPrinting15(pCon);
            SetPropertiesEx();

            rtnVal = fp.PrintLongAgreement();

            if (mstrOcrNo.NotEmpty())
            {
                mstrOcrNo = clsEmrQuery.GetOcrNo(fPrint, pPrint, rtnVal, mstrOcrNo.To<long>());
            }

            fp = null;
            return rtnVal;
        }

        public static int PrintFormLong(PsmhDb pDbCon, string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, System.Windows.Forms.Control pCon, string strFalg, string strMcrtMcNo)
        {
            int rtnVal = 0;
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            mstrEmrNo = strEmrNo;
            mstrFalg = strFalg;
            mstrMcrtMcNo = strMcrtMcNo;

            pPrint = clsEmrChart.ClearPatient();
            fPrint = clsEmrChart.ClearEmrForm();
            if (po != null)
            {
                pPrint = po;
            }

            try
            {
                if (pCon.Name == "panChart") ((mtsPanel15.mPanel)pCon).VerticalScroll.Value = 0;
            }
            catch { }

            SetInitInfo(pDbCon);

            fp = new FormPrinting15.FormPrinting15(pCon);
            SetPropertiesEx();
            rtnVal = fp.PrintLong();
            fp = null;
            return rtnVal;
        }

        public static int PrintToTifFileLong(PsmhDb pDbCon, string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, System.Windows.Forms.Control pCon, string strFalg)
        {
            int rtnVal = 0;
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            mstrEmrNo = strEmrNo;
            mstrFalg = strFalg;

            pPrint = clsEmrChart.ClearPatient();
            fPrint = clsEmrChart.ClearEmrForm();
            if (po != null)
            {
                pPrint = po;
            }

            try
            {
                if (pCon.Name == "panChart") ((mtsPanel15.mPanel)pCon).VerticalScroll.Value = 0;
                if (pCon.Name == "panChartSummary") ((mtsPanel15.mPanel)pCon).VerticalScroll.Value = 0;
            }
            catch { }

            SetInitInfo(pDbCon);

            fp = new FormPrinting15.FormPrinting15(pCon);
            SetPropertiesEx();
            rtnVal = fp.PrintToTifFileLong(VB.Val(mstrEmrNo).ToString());
            fp = null;
            return rtnVal;
        }

        public static int PrintToTifFileLong(PsmhDb pDbCon, string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, System.Windows.Forms.Control pCon, string strFalg, string strMcrtMcNo)
        {
            int rtnVal = 0;
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            mstrEmrNo = strEmrNo;
            mstrFalg = strFalg;
            mstrMcrtMcNo = strMcrtMcNo;

            pPrint = clsEmrChart.ClearPatient();
            fPrint = clsEmrChart.ClearEmrForm();
            if (po != null)
            {
                pPrint = po;
            }

            try
            {
                if (pCon.Name == "panChart") ((mtsPanel15.mPanel)pCon).VerticalScroll.Value = 0;
                if (pCon.Name == "panChartSummary") ((mtsPanel15.mPanel)pCon).VerticalScroll.Value = 0;
            }
            catch { }

            SetInitInfo(pDbCon);

            fp = new FormPrinting15.FormPrinting15(pCon);
            SetPropertiesEx();
            rtnVal = fp.PrintToTifFileLong(VB.Val(mstrEmrNo).ToString());
            fp = null;
            return rtnVal;
        }

        private static void SetInitInfo(PsmhDb pDbCon)
        {
            mstrPRINTDATE = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(pDbCon, "D"), "D");
            mstrPRINTNAME = clsType.User.UserName;
            //기록지 정보를 세팅
            fPrint = clsEmrChart.SerEmrFormInfo(pDbCon, mstrFormNo, mstrUpdateNo);

            //차트작성정보를 세팅
            if (VB.Val(mstrEmrNo) > 0)
            {
                string SQL    = string.Empty;    //Query문
                string SqlErr = string.Empty; //에러문 받는 변수
                DataTable dt = null;

                SQL = string.Empty;
                SQL = SQL + ComNum.VBLF + " SELECT A.CHARTDATE, A.CHARTTIME, A.CHARTUSEID, A.COMPUSEID";
                SQL = SQL + ComNum.VBLF + "  ,  (SELECT USENAME FROM ADMIN.AVIEWEMRUSER WHERE USEID = A.CHARTUSEID) AS USENAME";
                SQL = SQL + ComNum.VBLF + "  ,  (SELECT USENAME FROM ADMIN.AVIEWEMRUSER WHERE USEID = A.COMPUSEID) AS USENAME2 -- 이중인증 작성자";
                if (mstrFalg.Equals("C"))
                {
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AVIEWCHARTMST A";
                    SQL = SQL + ComNum.VBLF + " WHERE A.EMRNO = " + VB.Val(mstrEmrNo);
                }
                else if (mstrFalg.Equals("C2"))
                {
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRCHARTMSTHIS A";
                    SQL = SQL + ComNum.VBLF + " WHERE A.EMRNOHIS = " + VB.Val(mstrEmrNo);
                    mstrFalg = "C";
                }
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    //Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    mstrWRITEDATE = string.Empty;
                    mstrWRITETIME = string.Empty;
                    mstrWRITENAME = string.Empty;                   
                }
                else
                {
                    mstrWRITEDATE = ComFunc.FormatStrToDate((dt.Rows[0]["CHARTDATE"].ToString()).Trim(), "D");
                    mstrWRITETIME = ComFunc.FormatStrToDate((dt.Rows[0]["CHARTTIME"].ToString()).Trim(), "M");
                    mstrWRITENAME = (dt.Rows[0]["USENAME"].ToString()).Trim();

                    if (fPrint.FmFORMNO != 1232 && dt.Rows[0]["COMPUSEID"].ToString().Trim().NotEmpty() &&
                        dt.Rows[0]["CHARTUSEID"].ToString().Trim().Equals(dt.Rows[0]["COMPUSEID"].ToString().Trim()) == false)
                    {
                        mstrCOMPUSENAME = dt.Rows[0]["USENAME2"].ToString().Trim();
                    }

                    pPrint.age = ComFunc.AgeCalcX1(pPrint.ssno1 + pPrint.ssno2, dt.Rows[0]["CHARTDATE"].ToString().Trim());

                    if (pPrint.inOutCls.Equals("O") && pPrint.medDeptCd.Equals("ER") && pPrint.medDrCd.Equals("1199"))
                    {
                        pPrint.medDrName = mstrWRITENAME;
                    }
                }

                dt.Dispose();
            }
            else
            {

                if (pPrint.inOutCls.Equals("O") && pPrint.medDeptCd.Equals("ER") && pPrint.medDrCd.Equals("1199"))
                {
                    pPrint.medDrName = "내과"; //2021-01-12 기록실 이동춘 팀장님 통화후 1199일경우 무조건 내과로 출력.
                }

                mstrWRITEDATE = string.Empty;
                mstrWRITETIME = string.Empty;
                mstrWRITENAME = string.Empty;
                mstrOcrNo = string.Empty;               
            }
        }

        private static void SetPropertiesEx()
        {
            //' Set printing options
            fp.TextBoxBoxed = true;
            fp.TabControlBoxed = true;
            fp.LabelInBold = true;
            //if (clsCommon.gstrEXENAME == "MHEMRMNGDG.EXE")
            //{
            //    fp.PrintPreview = true;
            //}
            //else
            //{
            fp.PrintPreview = false;
            //}
            fp.DisabledControlsInGray = true;
            fp.PageNumbering = true;

            //'fp.Orientation = FormPrinting15.FormPrinting15.OrientationENum.Automatic
            fp.Orientation = FormPrinting15.FormPrinting15.OrientationENum.Portrait;
            //'fp.Orientation = FormPrinting15.FormPrinting15.OrientationENum.Lanscape

            //'fp.DelegatePrintingReportTitle = AddressOf MrOwnPrintReportTitle
            fp.DelegatePrintingReportTitle = null;

            //fp.TopMargin = 50;
            //fp.BottomMargin = 50;
            //fp.LeftMargin = 40;
            //fp.RightMargin = 60;

            fp.TopMargin = 170;
            fp.BottomMargin = 100;  //80
            fp.LeftMargin = 50;
            fp.RightMargin = 83;

            fp.mstrFalg = mstrFalg; //'기록지구분(J : 진단서, C : 차트)
            fp.mstrTitle = fPrint.FmFORMNAMEPRINT;   //'기록지 제목
            fp.mstrDocNo = "MR-" + fPrint.FmGRPFORMNO.ToString() + "-" + ComFunc.SetAutoZero(fPrint.FmFORMNO.ToString(), 5) + "-" + fPrint.FmREGDATE;     //'기록지 제목
            fp.mstrHpName = clsType.HosInfo.strNameKor;   //'병원

            if (clsType.HosInfo.strIMAGEUSE == "1")
            {
                //ComFunc.MsgBox("0");
                if (File.Exists(@"C:\Mentorsoft\exenet\hsplog.png"))
                {
                    //ComFunc.MsgBox("1");
                    fp.mHpLogo = System.Drawing.Image.FromFile(@"C:\Mentorsoft\exenet\hsplog.png");   //'병원 로고
                    //ComFunc.MsgBox("2");
                }
            }

            fp.mstrPRINTFLAG = clsFormPrint.mstrPRINTFLAG;  // mstrPRINTFLAG;    //'출력구분(0:원외용, 1:원내용)
            fp.mstrPTNO = pPrint.ptNo;    //'등록번호
            fp.mstrPTNAME = pPrint.ptName;  //'성명
            fp.mstrAGE = pPrint.age;     //'나이
            fp.mstrSEX = pPrint.sex;     //'성별

            fp.Explicate = mExplicate;// 설명의사 출력여부 

            if (clsFormPrint.mstrPRINTFLAG == "1" || clsFormPrint.mstrPRINTFLAG == "2")
            {
                fp.mstrSSNO = pPrint.ssno1 + "-" + (pPrint.ssno2 == null || pPrint.ssno2 != null && pPrint.ssno2.Length == 0 ? "" : pPrint.ssno2.Substring(0, 1));    //'주민번호
            }
            else
            {
                fp.mstrSSNO = pPrint.ssno1 + "-" + (pPrint.ssno2 == null || pPrint.ssno2 != null && pPrint.ssno2.Length == 0 ? "" : pPrint.ssno2.Substring(0, 1));    //'주민번호
                //fp.mstrSSNO = pPrint.ssno1 + "-" + pPrint.ssno2;    //'주민번호
            }

            fp.mstrDEPTNAME = pPrint.medDeptKorName;    //'진료과
            fp.mstrDRNAME = pPrint.medDrName;      //'의사
            fp.mstrMEDFRDATE = ComFunc.FormatStrToDate(pPrint.medFrDate, "D");     //'내원(입원일자)
            fp.mstrINOUTCLS = pPrint.inOutCls;     //'O : 외래, I :입원구분
            fp.mstrWARD = pPrint.ward;     //'병동
            fp.mstrROOM = pPrint.room;    //'병실
            fp.mstrWRITEDATE = mstrWRITEDATE;   //'작성일
            fp.mstrWRITENAME = mstrWRITENAME;   //'작성자
            fp.mstrPRINTDATE = mstrPRINTDATE;   //'출력일자
            fp.mstrPRINTNAME = mstrPRINTNAME;   //'출력자
            fp.mstrCOMPUSENAME = mstrCOMPUSENAME; //확인자
            fp.mstrOcrNo = mstrOcrNo;

            fp.mstrMcrtMcNo = mstrMcrtMcNo;   //'진단서 연번호

            if (mstrCONVIMAGEPATH == "")
            {
                fp.mstrImagePath = clsType.SvrInfo.strClient + "\\FormToImage\\";   //이미지 경로
            }
            else
            {
                fp.mstrImagePath = mstrCONVIMAGEPATH;   //이미지 경로
            }

            fp.mPrintVisible = mstrEmrNo.To<int>() > 0 && 
                fPrint.FmFORMNO != 2492 && 
                fPrint.FmFORMNO != 3577 && 
                fPrint.FmFORMNO != 3578 && 
                fPrint.FmFORMNO != 3579 &&
                fPrint.FmFORMNO != 3611;
        }
    }

}
