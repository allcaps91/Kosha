using ComBase; //기본 클래스
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComSupLibB.SupPthl
{

    /// <summary>
    /// Class Name : ComSupLibB.SubPthl
    /// File Name : frmComSupPthlPrt01.cs
    /// Title or Description : 병리 출력용
    /// Author : 김홍록
    /// Create Date : 2017-06-07
    /// Update History : 
    /// </summary>
    public partial class frmComSupPthlPrt01 : Form
    { 

        clsPthlSQL pthlSql = new clsPthlSQL();

        clsSpread spread = new clsSpread();
         
        /// <summary>출력폼 형태</summary>
        public enum enmType { ss_CR, ss_PS, ss_P, ss_PU, ss_C, ss_S, ss_AC, ss_AC2 , ss_C_1, ss_AC_1 };     
        // SS4 : PS, SS5 : P, SSPU : PU, C : Cytology

        /// <summary>생성자</summary>
        public frmComSupPthlPrt01()
        {
            InitializeComponent();
        }

        /// <summary>생성자</summary>
        /// <param name="enmtype">폼형태</param>
        /// <param name="strSpecNo">검체번호</param>
        /// <param name="isPre">미리보기여부</param>
        /// <param name="strSpecimen">Nature & Source of Specimen</param>
        /// <param name="strDiagnosis">Clinical Diagnosis</param>
        /// <param name="strClinicalHistory">Clinical History & Information</param>
        /// <param name="strCytologyExamination">Information on previous Cytology Examination</param>
        public frmComSupPthlPrt01(enmType enmtype, string strSpecNo, bool isPre, bool isRslt)
        {
            InitializeComponent();

            DataTable dt = pthlSql.sel_EXAM_ANATMST_ANATPrint(clsDB.DbCon, strSpecNo);

            if (ComFunc.isDataTableNull(dt) == true)
            {
                ComFunc.MsgBox("출력 대상이 존재 하지 않습니다.");
            }
            else
            {
                setCytology(enmtype, dt, isRslt);

                if (enmtype == enmType.ss_S)
                {
                    printSS_S(dt, isPre);
                }
                else if (enmtype == enmType.ss_C)
                {
                    if (dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTSABUN.ToString()].ToString() != "" && dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTSABUN.ToString()].ToString() == "김미진")
                    {
                        enmtype = enmType.ss_C_1;
                    }
                    printSS(enmtype, isPre);
                }
                else if (enmtype == enmType.ss_AC)
                {
                    if (dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTSABUN.ToString()].ToString() != "" && dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTSABUN.ToString()].ToString() == "김미진")
                    {
                        enmtype = enmType.ss_AC_1;
                    }
                    printSS(enmtype, isPre);
                }
                else
                {
                    printSS(enmtype, isPre);
                }
            } 
        }

        

        void setCytology(enmType pType, DataTable dt, bool isRslt) 
        { 


            string strBDATE_PANO = "   등록번호 : " + dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.PANO.ToString()].ToString();

            if (pType == enmType.ss_CR)
            {
                this.ss_CR.ActiveSheet.Cells[1, 1].Value   = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.PANO.ToString()].ToString();
                this.ss_CR.ActiveSheet.Cells[1, 3].Value   = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.ANATNO.ToString()].ToString().Substring(0,3) + "-" + dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.ANATNO.ToString()].ToString().Substring(3);

                this.ss_CR.ActiveSheet.Cells[2, 1].Value   = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.SNAME.ToString()].ToString();
                this.ss_CR.ActiveSheet.Cells[2, 3].Value   = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.WARD_NAME.ToString()].ToString();

                this.ss_CR.ActiveSheet.Cells[3, 1].Value   = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.JUMIN1.ToString()].ToString() + "-" + dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.JUMIN2.ToString()].ToString();
                
                this.ss_CR.ActiveSheet.Cells[5, 1].Value   = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.BDATE.ToString()].ToString();

                this.ss_CR.ActiveSheet.Cells[4, 2].Value   = "Dr. "   + dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.DEPTCODE.ToString()].ToString()  + " " + dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.DR_NAME.ToString()].ToString();

                //this.ssCytology.ActiveSheet.Cells[5, 3].Value   = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.DR_NAME.ToString()].ToString();

                this.ss_CR.ActiveSheet.Cells[7, 0].Value   = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.REMARK1.ToString()].ToString();
                this.ss_CR.ActiveSheet.Cells[9, 0].Value   = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.REMARK4.ToString()].ToString(); 
                this.ss_CR.ActiveSheet.Cells[11, 0].Value  = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.REMARK2.ToString()].ToString();
                this.ss_CR.ActiveSheet.Cells[13, 0].Value  = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.REMARK3.ToString()].ToString();  
            }
            else if (pType == enmType.ss_AC2) 
            {
                this.ss_AC2.ActiveSheet.Cells[1, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.PANO.ToString()].ToString();
                this.ss_AC2.ActiveSheet.Cells[1, 3].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.ANATNO.ToString()].ToString().Substring(0, 4) + "-" + dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.ANATNO.ToString()].ToString().Substring(4);

                this.ss_AC2.ActiveSheet.Cells[2, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.SNAME.ToString()].ToString();
                this.ss_AC2.ActiveSheet.Cells[2, 3].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.WARD_NAME.ToString()].ToString();

                this.ss_AC2.ActiveSheet.Cells[3, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.JUMIN1.ToString()].ToString() + "-" + dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.JUMIN2.ToString()].ToString();

                this.ss_AC2.ActiveSheet.Cells[5, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.BDATE.ToString()].ToString();

                this.ss_AC2.ActiveSheet.Cells[4, 2].Value = "Dr. " + dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.DEPTCODE.ToString()].ToString() + " " + dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.DR_NAME.ToString()].ToString();

                //this.ssCytology.ActiveSheet.Cells[5, 3].Value   = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.DR_NAME.ToString()].ToString();

                this.ss_AC2.ActiveSheet.Cells[7, 0].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.REMARK1.ToString()].ToString();
                this.ss_AC2.ActiveSheet.Cells[9, 0].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.REMARK4.ToString()].ToString();
                this.ss_AC2.ActiveSheet.Cells[11, 0].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.REMARK2.ToString()].ToString();
                this.ss_AC2.ActiveSheet.Cells[13, 0].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.REMARK3.ToString()].ToString();
            }
            else if (pType == enmType.ss_PS)
            {
                // SS4 : PS, SS5 : P, SSPU : PU, C : Cytology

                this.ss_PS.ActiveSheet.Cells[1, 0].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.ANATNO.ToString()].ToString().Substring(0, 4) + "-" + dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.ANATNO.ToString()].ToString().Substring(4) + "    " + strBDATE_PANO;

                this.ss_PS.ActiveSheet.Cells[2, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.SNAME.ToString()].ToString();
                this.ss_PS.ActiveSheet.Cells[2, 4].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.AGE_SEX.ToString()].ToString();
                this.ss_PS.ActiveSheet.Cells[2, 7].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.DEPT_NM.ToString()].ToString();

                this.ss_PS.ActiveSheet.Cells[3, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.JUMIN.ToString()].ToString();
                this.ss_PS.ActiveSheet.Cells[3, 5].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.COMP_NM.ToString()].ToString();


                if (isRslt == true)
                {
                    if (string.IsNullOrEmpty(dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString()) == false)
                    {

                        this.ss_PS.ActiveSheet.Cells[5, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(0, 1) == "1" ? "True" : ""; //1.적정
                        this.ss_PS.ActiveSheet.Cells[6, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(1, 1) == "1" ? "True" : ""; //2.재한적
                        this.ss_PS.ActiveSheet.Cells[7, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(2, 1) == "1" ? "True" : ""; //3.불량

                        this.ss_PS.ActiveSheet.Cells[8, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(3, 1) == "1" ? "True" : "";  //1음성
                        this.ss_PS.ActiveSheet.Cells[9, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(4, 1) == "1" ? "True" : "";  //2음성
                        this.ss_PS.ActiveSheet.Cells[10, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(5, 1) == "1" ? "True" : ""; //3.의양성
                        this.ss_PS.ActiveSheet.Cells[11, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(6, 1) == "1" ? "True" : ""; //4.양성
                        this.ss_PS.ActiveSheet.Cells[12, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(7, 1) == "1" ? "True" : ""; //5.양성  

                        this.ss_PS.ActiveSheet.Cells[13, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(8, 1) == "1" ? "True" : "";  // '가래 채취 방법 설명 후 재검사
                        this.ss_PS.ActiveSheet.Cells[14, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(9, 1) == "1" ? "True" : "";  // '정기적 조사
                        this.ss_PS.ActiveSheet.Cells[15, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(10, 1) == "1" ? "True" : ""; // '6개월재검 => 재검변경
                        this.ss_PS.ActiveSheet.Cells[16, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(11, 1) == "1" ? "True" : ""; // '4정밀 검사 필요
                        this.ss_PS.ActiveSheet.Cells[17, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(12, 1) == "1" ? "True" : ""; // '암

                        this.ss_PS.ActiveSheet.Cells[15, 5].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(13, 1) == "1" ? "True" : ""; // ' 즉시
                        this.ss_PS.ActiveSheet.Cells[15, 7].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(14, 1) == "1" ? "True" : ""; // ' ' 6개월이내

                    }

                    if (string.IsNullOrEmpty(dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK5.ToString()].ToString()) == false)
                    {
                        this.ss_PS.ActiveSheet.Cells[18, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK5.ToString()].ToString();
                        
                    }

                    this.ss_PS.ActiveSheet.Cells[27, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RECEIVEDATE.ToString()].ToString();
                    this.ss_PS.ActiveSheet.Cells[28, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.JDATE.ToString()].ToString();
                    this.ss_PS.ActiveSheet.Cells[29, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTDATE.ToString()].ToString();
                }
                else 
                {

                    this.ss_PS.ActiveSheet.Cells[18, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.DRCOMMENT.ToString()].ToString();
                    this.ss_PS.ActiveSheet.Cells[27, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RECEIVEDATE.ToString()].ToString();
                    this.ss_PS.ActiveSheet.Cells[28, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.JDATE.ToString()].ToString();

                }
            }
            else if (pType == enmType.ss_PU)
            {
                this.ss_PU.ActiveSheet.Cells[1, 0].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.ANATNO.ToString()].ToString().Substring(0, 4) + "-" + dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.ANATNO.ToString()].ToString().Substring(4) + "    " + strBDATE_PANO;

                this.ss_PU.ActiveSheet.Cells[2, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.SNAME.ToString()].ToString();
                this.ss_PU.ActiveSheet.Cells[2, 4].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.AGE_SEX.ToString()].ToString();
                this.ss_PU.ActiveSheet.Cells[2, 7].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.DEPT_NM.ToString()].ToString();

                this.ss_PU.ActiveSheet.Cells[3, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.JUMIN.ToString()].ToString();
                this.ss_PU.ActiveSheet.Cells[3, 5].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.COMP_NM.ToString()].ToString();

                if (isRslt == true)
                {


                    if (string.IsNullOrEmpty(dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString()) == false)
                    {
                        this.ss_PU.ActiveSheet.Cells[5, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(0, 1) == "1" ? "True" : ""; //1.적정
                        this.ss_PU.ActiveSheet.Cells[6, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(1, 1) == "1" ? "True" : ""; //2.재한적
                        this.ss_PU.ActiveSheet.Cells[7, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(2, 1) == "1" ? "True" : ""; //3.불량

                        this.ss_PU.ActiveSheet.Cells[8, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(3, 1) == "1" ? "True" : "";  //1음성
                        this.ss_PU.ActiveSheet.Cells[9, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(4, 1) == "1" ? "True" : "";  //2음성
                        this.ss_PU.ActiveSheet.Cells[10, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(5, 1) == "1" ? "True" : ""; //3.의양성
                        this.ss_PU.ActiveSheet.Cells[11, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(6, 1) == "1" ? "True" : ""; //4.양성
                        this.ss_PU.ActiveSheet.Cells[12, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(7, 1) == "1" ? "True" : ""; //5.양성  

                        this.ss_PU.ActiveSheet.Cells[13, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(8, 1) == "1" ? "True" : ""; //'소변 채취 방법 설명 후 재검사
                        this.ss_PU.ActiveSheet.Cells[14, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(9, 1) == "1" ? "True" : ""; //'정밀검사필요
                    }

                    if (string.IsNullOrEmpty(dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK5.ToString()].ToString()) == false)
                    {
                        this.ss_PU.ActiveSheet.Cells[15, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK5.ToString()].ToString();
                    }
                    this.ss_PU.ActiveSheet.Cells[24, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RECEIVEDATE.ToString()].ToString();
                    this.ss_PU.ActiveSheet.Cells[25, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.JDATE.ToString()].ToString();
                    this.ss_PU.ActiveSheet.Cells[26, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTDATE.ToString()].ToString();
                }
                else
                {
                    this.ss_PU.ActiveSheet.Cells[15, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.DRCOMMENT.ToString()].ToString();
                    this.ss_PU.ActiveSheet.Cells[24, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RECEIVEDATE.ToString()].ToString();
                    this.ss_PU.ActiveSheet.Cells[25, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.JDATE.ToString()].ToString();
                }

            }
            else if (pType == enmType.ss_P)
            {
                // SS4 : PS, SS5 : P, SSPU : PU, C : Cytology
                this.ss_P.ActiveSheet.Cells[1, 0].Value =  strBDATE_PANO + "    " + dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.ANATNO.ToString()].ToString().Substring(0, 3) + "-" + dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.ANATNO.ToString()].ToString().Substring(3);

                this.ss_P.ActiveSheet.Cells[2, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.SNAME.ToString()].ToString();
                this.ss_P.ActiveSheet.Cells[2, 4].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.AGE_SEX.ToString()].ToString();
                this.ss_P.ActiveSheet.Cells[2, 7].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.DEPT_NM.ToString()].ToString();

                this.ss_P.ActiveSheet.Cells[3, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.JUMIN.ToString()].ToString();
                this.ss_P.ActiveSheet.Cells[3, 5].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.COMP_NM.ToString()].ToString();

                if (isRslt == true)
                {
                    if (string.IsNullOrEmpty(dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString()) == false)
                    {
                        this.ss_P.ActiveSheet.Cells[5, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(0, 1) == "1" ? "True" : "";   // '1.적정
                        this.ss_P.ActiveSheet.Cells[5, 6].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(1, 1) == "1" ? "True" : "";   // '부적절

                        this.ss_P.ActiveSheet.Cells[6, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(2, 1) == "1" ? "True" : "";   // '1.유
                        this.ss_P.ActiveSheet.Cells[6, 6].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(3, 1) == "1" ? "True" : "";   // '2.무

                        this.ss_P.ActiveSheet.Cells[8, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(4, 1) == "1" ? "True" : "";   // '1.음성
                        this.ss_P.ActiveSheet.Cells[8, 6].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(5, 1) == "1" ? "True" : "";   // '2.상피세포이상
                        this.ss_P.ActiveSheet.Cells[9, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(6, 1) == "1" ? "True" : "";   // ''기타(자궁내말세포출혈)

                        this.ss_P.ActiveSheet.Cells[10, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(7, 1) == "1" ? "True" : "";   // '비정형 편평상피세포
                        this.ss_P.ActiveSheet.Cells[10, 6].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(8, 1) == "1" ? "True" : "";   // '일반
                        this.ss_P.ActiveSheet.Cells[10, 9].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(9, 1) == "1" ? "True" : "";   // '고위험

                        this.ss_P.ActiveSheet.Cells[11, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(10, 1) == "1" ? "True" : "";   // '2.저등급 편평상피내 병변
                        this.ss_P.ActiveSheet.Cells[12, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(11, 1) == "1" ? "True" : "";   // '3.고등급 편평상피내
                        this.ss_P.ActiveSheet.Cells[13, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(12, 1) == "1" ? "True" : "";   // '4.침윤성
                        this.ss_P.ActiveSheet.Cells[14, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(13, 1) == "1" ? "True" : "";   // '1.비정형

                        if (dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Length > 30)
                        {
                            if (dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(30, 1) != "")
                            {
                                this.ss_P.ActiveSheet.Cells[14, 6].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(30, 1) == "1" ? "True" : "";   // '일반
                            }
                            if (dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(31, 1) != "")
                            {
                                this.ss_P.ActiveSheet.Cells[14, 9].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(31, 1) == "1" ? "True" : "";   // '종양성 
                            }
                        }

                        this.ss_P.ActiveSheet.Cells[15, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(14, 1) == "1" ? "True" : "";   // '2.상피내
                        this.ss_P.ActiveSheet.Cells[16, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(15, 1) == "1" ? "True" : "";   // '3.침윤성
                        this.ss_P.ActiveSheet.Cells[17, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(16, 1) == "1" ? "True" : "";   // '4.기타:

                        this.ss_P.ActiveSheet.Cells[18, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(17, 1) == "1" ? "True" : "";   // '1.반으성 세포변화
                        this.ss_P.ActiveSheet.Cells[19, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(18, 1) == "1" ? "True" : "";   // '2.트리코모나스
                        this.ss_P.ActiveSheet.Cells[20, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(19, 1) == "1" ? "True" : "";   // '3.캔디다
                        this.ss_P.ActiveSheet.Cells[20, 6].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(20, 1) == "1" ? "True" : "";   // '4.방선균
                        this.ss_P.ActiveSheet.Cells[21, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(21, 1) == "1" ? "True" : "";   // '5.헤르페스 바이러스
                        this.ss_P.ActiveSheet.Cells[22, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(22, 1) == "1" ? "True" : "";   // '6.기타

                        this.ss_P.ActiveSheet.Cells[23, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(23, 1) == "1" ? "True" : "";   // '1.이상소견없음. 
                        this.ss_P.ActiveSheet.Cells[24, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(24, 1) == "1" ? "True" : "";   // '2.염증성 또는 감염성 질환
                        this.ss_P.ActiveSheet.Cells[25, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(25, 1) == "1" ? "True" : "";   // '3.상피세포 이상

                        //2018.05.24.김홍록:갑자기 생김...
                        if (dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Length >= 30)
                        {
                            this.ss_P.ActiveSheet.Cells[26, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(29, 1) == "1" ? "True" : "";   // '4.자궁경부암 전구단계 의심
                        }

                        this.ss_P.ActiveSheet.Cells[27, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(26, 1) == "1" ? "True" : "";   // '4.자궁경부암 의심
                        this.ss_P.ActiveSheet.Cells[28, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(27, 1) == "1" ? "True" : "";   // '5.기타
                        this.ss_P.ActiveSheet.Cells[29, 2].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK1.ToString()].ToString().Substring(28, 1) == "1" ? "True" : "";   // '기존 자궁경부암환자
                    }

                    if (string.IsNullOrEmpty(dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK2.ToString()].ToString()) == false)
                    {
                        this.ss_P.ActiveSheet.Cells[17, 4].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK2.ToString()].ToString();
                    }

                    if (string.IsNullOrEmpty(dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK3.ToString()].ToString()) == false)
                    {
                        this.ss_P.ActiveSheet.Cells[22, 4].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK3.ToString()].ToString();
                    }

                    if (string.IsNullOrEmpty(dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK4.ToString()].ToString()) == false)
                    {
                        this.ss_P.ActiveSheet.Cells[28, 4].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK4.ToString()].ToString();
                    }

                    if (string.IsNullOrEmpty(dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK5.ToString()].ToString()) == false)
                    {
                        this.ss_P.ActiveSheet.Cells[30, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.HRREMARK5.ToString()].ToString();
                    }

                    this.ss_P.ActiveSheet.Cells[31, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RECEIVEDATE.ToString()].ToString();
                    this.ss_P.ActiveSheet.Cells[32, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.JDATE.ToString()].ToString();
                    this.ss_P.ActiveSheet.Cells[33, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTDATE.ToString()].ToString();
                }
                else
                {
                    this.ss_P.ActiveSheet.Cells[30, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.DRCOMMENT.ToString()].ToString();
                    this.ss_P.ActiveSheet.Cells[31, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RECEIVEDATE.ToString()].ToString();
                    this.ss_P.ActiveSheet.Cells[32, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.JDATE.ToString()].ToString();
                }


            }

            else if (pType == enmType.ss_C)
            {
                if (dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTSABUN.ToString()].ToString() != "" && dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTSABUN.ToString()].ToString() == "박미옥")
                {

                    this.ss_C.ActiveSheet.Cells[1, 0].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.ANATNO.ToString()].ToString().Substring(0, 3) + "-" + dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.ANATNO.ToString()].ToString().Substring(3);
                    this.ss_C.ActiveSheet.Cells[2, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.PANO.ToString()].ToString();
                    this.ss_C.ActiveSheet.Cells[2, 4].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.SNAME.ToString()].ToString();
                    this.ss_C.ActiveSheet.Cells[2, 7].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.AGE_SEX.ToString()].ToString();


                    this.ss_C.ActiveSheet.Cells[3, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.WARD_NAME.ToString()].ToString();
                    this.ss_C.ActiveSheet.Cells[3, 4].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.DEPT_NM.ToString()].ToString();
                    this.ss_C.ActiveSheet.Cells[3, 7].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.DR_NAME.ToString()].ToString();

                    if (isRslt == true)
                    {
                        this.ss_C.ActiveSheet.Cells[5, 0].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULT_C.ToString()].ToString();

                        this.ss_C.ActiveSheet.Cells[32, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTSABUN.ToString()].ToString();
                        this.ss_C.ActiveSheet.Cells[32, 7].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTDATE.ToString()].ToString();

                    }

                    this.ss_C.ActiveSheet.Cells[30, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.SPECODE.ToString()].ToString();
                    this.ss_C.ActiveSheet.Cells[30, 7].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RECEIVEDATE.ToString()].ToString();

                    this.ss_C.ActiveSheet.Cells[31, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.GBSNAME.ToString()].ToString();
                    this.ss_C.ActiveSheet.Cells[31, 7].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.JDATE.ToString()].ToString();
                }

                else if (dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTSABUN.ToString()].ToString() != "" && dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTSABUN.ToString()].ToString() == "김미진")
                {


                    this.ss_C_1.ActiveSheet.Cells[1, 0].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.ANATNO.ToString()].ToString().Substring(0, 3) + "-" + dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.ANATNO.ToString()].ToString().Substring(3);
                    this.ss_C_1.ActiveSheet.Cells[2, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.PANO.ToString()].ToString();
                    this.ss_C_1.ActiveSheet.Cells[2, 4].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.SNAME.ToString()].ToString();
                    this.ss_C_1.ActiveSheet.Cells[2, 7].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.AGE_SEX.ToString()].ToString();


                    this.ss_C_1.ActiveSheet.Cells[3, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.WARD_NAME.ToString()].ToString();
                    this.ss_C_1.ActiveSheet.Cells[3, 4].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.DEPT_NM.ToString()].ToString();
                    this.ss_C_1.ActiveSheet.Cells[3, 7].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.DR_NAME.ToString()].ToString();

                    if (isRslt == true)
                    {
                        this.ss_C_1.ActiveSheet.Cells[5, 0].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULT_C.ToString()].ToString();

                        this.ss_C_1.ActiveSheet.Cells[32, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTSABUN.ToString()].ToString();
                        this.ss_C_1.ActiveSheet.Cells[32, 7].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTDATE.ToString()].ToString();

                    }

                    this.ss_C_1.ActiveSheet.Cells[30, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.SPECODE.ToString()].ToString();
                    this.ss_C_1.ActiveSheet.Cells[30, 7].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RECEIVEDATE.ToString()].ToString();

                    this.ss_C_1.ActiveSheet.Cells[31, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.GBSNAME.ToString()].ToString();
                    this.ss_C_1.ActiveSheet.Cells[31, 7].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.JDATE.ToString()].ToString();
                }
          
                

             
                 
            }
            else if (pType == enmType.ss_AC)
            {
                if (dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTSABUN.ToString()].ToString() != "" && dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTSABUN.ToString()].ToString() == "박미옥")
                {
                    this.ss_AC.ActiveSheet.Cells[1, 0].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.ANATNO.ToString()].ToString().Substring(0, 4) + "-" + dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.ANATNO.ToString()].ToString().Substring(4);
                    this.ss_AC.ActiveSheet.Cells[2, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.PANO.ToString()].ToString();
                    this.ss_AC.ActiveSheet.Cells[2, 4].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.SNAME.ToString()].ToString();
                    this.ss_AC.ActiveSheet.Cells[2, 7].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.AGE_SEX.ToString()].ToString();


                    this.ss_AC.ActiveSheet.Cells[3, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.WARD_NAME.ToString()].ToString();
                    this.ss_AC.ActiveSheet.Cells[3, 4].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.DEPT_NM.ToString()].ToString();
                    this.ss_AC.ActiveSheet.Cells[3, 7].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.DR_NAME.ToString()].ToString();

                    if (isRslt == true)
                    {
                        this.ss_AC.ActiveSheet.Cells[5, 0].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULT_C.ToString()].ToString();

                        this.ss_AC.ActiveSheet.Cells[32, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTSABUN.ToString()].ToString();
                        this.ss_AC.ActiveSheet.Cells[32, 7].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTDATE.ToString()].ToString();

                    }

                    this.ss_AC.ActiveSheet.Cells[30, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.SPECODE.ToString()].ToString();
                    this.ss_AC.ActiveSheet.Cells[30, 7].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RECEIVEDATE.ToString()].ToString();

                    this.ss_AC.ActiveSheet.Cells[31, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.GBSNAME.ToString()].ToString();
                    this.ss_AC.ActiveSheet.Cells[31, 7].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.JDATE.ToString()].ToString();
                }

                else if (dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTSABUN.ToString()].ToString() != "" && dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTSABUN.ToString()].ToString() == "김미진")
                {

                    this.ss_AC_1.ActiveSheet.Cells[1, 0].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.ANATNO.ToString()].ToString().Substring(0, 4) + "-" + dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.ANATNO.ToString()].ToString().Substring(4);
                    this.ss_AC_1.ActiveSheet.Cells[2, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.PANO.ToString()].ToString();
                    this.ss_AC_1.ActiveSheet.Cells[2, 4].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.SNAME.ToString()].ToString();
                    this.ss_AC_1.ActiveSheet.Cells[2, 7].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.AGE_SEX.ToString()].ToString();


                    this.ss_AC_1.ActiveSheet.Cells[3, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.WARD_NAME.ToString()].ToString();
                    this.ss_AC_1.ActiveSheet.Cells[3, 4].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.DEPT_NM.ToString()].ToString();
                    this.ss_AC_1.ActiveSheet.Cells[3, 7].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.DR_NAME.ToString()].ToString();

                    if (isRslt == true)
                    {
                        this.ss_AC_1.ActiveSheet.Cells[5, 0].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULT_C.ToString()].ToString();

                        this.ss_AC_1.ActiveSheet.Cells[32, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTSABUN.ToString()].ToString();
                        this.ss_AC_1.ActiveSheet.Cells[32, 7].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTDATE.ToString()].ToString();

                    }

                    this.ss_AC_1.ActiveSheet.Cells[30, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.SPECODE.ToString()].ToString();
                    this.ss_AC_1.ActiveSheet.Cells[30, 7].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RECEIVEDATE.ToString()].ToString();

                    this.ss_AC_1.ActiveSheet.Cells[31, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.GBSNAME.ToString()].ToString();
                    this.ss_AC_1.ActiveSheet.Cells[31, 7].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.JDATE.ToString()].ToString();
                }

            

            }
            else if (pType == enmType.ss_S)
            {
                if (dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTSABUN.ToString()].ToString() != "" && dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTSABUN.ToString()].ToString() == "박미옥")
                {
                    this.ss_S.ActiveSheet.Cells[1, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.ANATNO.ToString()].ToString().Length == 8 ? dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.ANATNO.ToString()].ToString().Substring(0, 3) + "-" + dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.ANATNO.ToString()].ToString().Substring(3) : dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.ANATNO.ToString()].ToString().Substring(0, 4) + "-" + dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.ANATNO.ToString()].ToString().Substring(4);
                    this.ss_S.ActiveSheet.Cells[2, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.PANO.ToString()].ToString();
                    this.ss_S.ActiveSheet.Cells[2, 3].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.JUMIN.ToString()].ToString();

                    this.ss_S.ActiveSheet.Cells[3, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.SNAME.ToString()].ToString();
                    this.ss_S.ActiveSheet.Cells[3, 3].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.SEX.ToString()].ToString();
                    this.ss_S.ActiveSheet.Cells[3, 5].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.AGE.ToString()].ToString();

                    this.ss_S.ActiveSheet.Cells[4, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.DEPT_NM.ToString()].ToString();
                    this.ss_S.ActiveSheet.Cells[4, 3].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.WARD_NAME.ToString()].ToString();
                    this.ss_S.ActiveSheet.Cells[4, 5].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.DR_NAME.ToString()].ToString();

                    #region 2019-03-19 안정수, 출력방식 변경
                    int i = 0;
                    int ii = 0;
                    int nPageTot = 0;
                    string strlast = "";
                    string strfirst = "";
                    char sresultremark;
                    //string[] sResultRemark1 = new string[120];
                    string[] sResultRemark1 = null;
                    sResultRemark1 = new string[1];

                    strfirst = dt.Rows[0]["RESULT1"].ToString() + dt.Rows[0]["RESULT2"].ToString();
                    int nlen = strfirst.Length;

                    for (ii = 1; ii < 4000; ii++)
                    {
                        sresultremark = VB.Mid(strfirst, ii, 1) != "" ? Convert.ToChar(VB.Mid(strfirst, ii, 1)) : ' ';
                        strlast = strlast + VB.Mid(strfirst, ii, 1);


                        //sresultremark = Convert.ToChar(VB.Mid(strfirst, ii, 1));                       

                        if (sresultremark == VB.Chr(13) || strlast.Length >= 75)
                        {
                            i += 1;

                            if (strlast.Length == 1 && (Convert.ToChar(strlast) == VB.Chr(13)))
                            {
                                Array.Resize<string>(ref sResultRemark1, sResultRemark1.Length + 1);
                                sResultRemark1[i] = "";
                                strlast = "";
                            }
                            else if (strlast.Length >= 75 && Convert.ToChar(VB.Left(strlast, 1)) != VB.Chr(10))
                            {
                                Array.Resize<string>(ref sResultRemark1, sResultRemark1.Length + 1);
                                sResultRemark1[i] = strlast;
                                strlast = "";
                            }
                            else if (strlast.Length >= 75 && Convert.ToChar(VB.Left(strlast, 1)) == VB.Chr(10))
                            {
                                Array.Resize<string>(ref sResultRemark1, sResultRemark1.Length + 1);
                                sResultRemark1[i] = VB.Right(strlast, strlast.Length - 1);
                                strlast = "";
                            }
                            else if (Convert.ToChar(VB.Left(strlast, 1)) == VB.Chr(10) && i != 1)
                            {
                                strlast = VB.Right(strlast, strlast.Length - 1);
                                Array.Resize<string>(ref sResultRemark1, sResultRemark1.Length + 1);
                                sResultRemark1[i] = VB.Left(strlast, strlast.Length - 1);
                                strlast = "";
                            }
                            else if (Convert.ToChar(VB.Right(strlast, 1)) == VB.Chr(13) && i != 1)
                            {
                                strlast = VB.Right(strlast, strlast.Length - 1);
                                Array.Resize<string>(ref sResultRemark1, sResultRemark1.Length + 1);
                                sResultRemark1[i] = VB.Left(strlast, strlast.Length - 1);
                                strlast = "";
                            }
                            else if (i == 1)
                            {
                                Array.Resize<string>(ref sResultRemark1, sResultRemark1.Length + 1);
                                sResultRemark1[i] = VB.Left(strlast, strlast.Length - 1);
                                strlast = "";
                            }
                        }
                        sresultremark = ' ';
                    }

                    i += 1;

                    if (strlast != "" && (strlast.Length >= 1 && Convert.ToChar(VB.Left(strlast, 1)) == VB.Chr(13) || Convert.ToChar(VB.Left(strlast, 1)) == VB.Chr(10)))
                    {
                        Array.Resize<string>(ref sResultRemark1, sResultRemark1.Length + 1);
                        sResultRemark1[i] = VB.Right(strlast, strlast.Length - 1);
                        strlast = "";
                    }
                    else if (strlast.Length >= 1)
                    {
                        this.ss_S.ActiveSheet.Cells[this.ss_S.ActiveSheet.Rows.Count - 1, 0].Text = strlast.Trim();
                        strlast = "";
                    }

                    this.ss_S.ActiveSheet.Rows.Count += 1;
                    this.ss_S.ActiveSheet.Cells[5, 0].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.AnatName.ToString()].ToString();

                    this.ss_S.ActiveSheet.Rows.Count += 1;

                    if (sResultRemark1.Length > 0)
                    {
                        //Array.Resize<string>(ref sResultRemark1, sResultRemark1.Length + 1);
                        //sResultRemark1[54] = "          mailgnancy of this case.";

                        for (int j = 0; j < sResultRemark1.Length; j++)
                        {
                            this.ss_S.ActiveSheet.Rows.Count += 1;
                            this.ss_S.ActiveSheet.AddSpanCell(6 + j, 0, 1, this.ss_S.ActiveSheet.Columns.Count - 1);
                            if (j > 19 && VB.Left(sResultRemark1[j], 15) == "               ")
                            {
                                if (VB.Left(sResultRemark1[j], 45) == "                                             ")
                                {
                                    this.ss_S.ActiveSheet.Cells[6 + j, 0].Text = "         " + sResultRemark1[j];
                                }
                                else
                                {
                                    this.ss_S.ActiveSheet.Cells[6 + j, 0].Text = "   " + sResultRemark1[j];
                                }
                            }
                            else
                            {
                                this.ss_S.ActiveSheet.Cells[6 + j, 0].Text = sResultRemark1[j];
                            }
                            //2019-04-23 안정수 추가( 2장분 내용을 1장으로하기위함
                            if (VB.Left(this.ss_S.ActiveSheet.Cells[1, 1].Value.ToString(), 2) == "OS")
                            {
                                this.ss_S.ActiveSheet.Cells[6 + j, 0].Row.Height = 16;
                            }
                        }
                    }
                }
                #endregion
                else if (dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTSABUN.ToString()].ToString() != "" && dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTSABUN.ToString()].ToString() == "김미진")
                {

                    this.ss_S_1.ActiveSheet.Cells[1, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.ANATNO.ToString()].ToString().Length == 8 ? dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.ANATNO.ToString()].ToString().Substring(0, 3) + "-" + dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.ANATNO.ToString()].ToString().Substring(3) : dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.ANATNO.ToString()].ToString().Substring(0, 4) + "-" + dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.ANATNO.ToString()].ToString().Substring(4);
                    this.ss_S_1.ActiveSheet.Cells[2, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.PANO.ToString()].ToString();
                    this.ss_S_1.ActiveSheet.Cells[2, 3].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.JUMIN.ToString()].ToString();

                    this.ss_S_1.ActiveSheet.Cells[3, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.SNAME.ToString()].ToString();
                    this.ss_S_1.ActiveSheet.Cells[3, 3].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.SEX.ToString()].ToString();
                    this.ss_S_1.ActiveSheet.Cells[3, 5].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.AGE.ToString()].ToString();

                    this.ss_S_1.ActiveSheet.Cells[4, 1].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.DEPT_NM.ToString()].ToString();
                    this.ss_S_1.ActiveSheet.Cells[4, 3].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.WARD_NAME.ToString()].ToString();
                    this.ss_S_1.ActiveSheet.Cells[4, 5].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.DR_NAME.ToString()].ToString();

                    #region 2019-03-19 안정수, 출력방식 변경
                    int i = 0;
                    int ii = 0;
                    int nPageTot = 0;
                    string strlast = "";
                    string strfirst = "";
                    char sresultremark;
                    //string[] sResultRemark1 = new string[120];
                    string[] sResultRemark1 = null;
                    sResultRemark1 = new string[1];

                    strfirst = dt.Rows[0]["RESULT1"].ToString() + dt.Rows[0]["RESULT2"].ToString();
                    int nlen = strfirst.Length;

                    for (ii = 1; ii < 4000; ii++)
                    {
                        sresultremark = VB.Mid(strfirst, ii, 1) != "" ? Convert.ToChar(VB.Mid(strfirst, ii, 1)) : ' ';
                        strlast = strlast + VB.Mid(strfirst, ii, 1);


                        //sresultremark = Convert.ToChar(VB.Mid(strfirst, ii, 1));                       

                        if (sresultremark == VB.Chr(13) || strlast.Length >= 75)
                        {
                            i += 1;

                            if (strlast.Length == 1 && (Convert.ToChar(strlast) == VB.Chr(13)))
                            {
                                Array.Resize<string>(ref sResultRemark1, sResultRemark1.Length + 1);
                                sResultRemark1[i] = "";
                                strlast = "";
                            }
                            else if (strlast.Length >= 75 && Convert.ToChar(VB.Left(strlast, 1)) != VB.Chr(10))
                            {
                                Array.Resize<string>(ref sResultRemark1, sResultRemark1.Length + 1);
                                sResultRemark1[i] = strlast;
                                strlast = "";
                            }
                            else if (strlast.Length >= 75 && Convert.ToChar(VB.Left(strlast, 1)) == VB.Chr(10))
                            {
                                Array.Resize<string>(ref sResultRemark1, sResultRemark1.Length + 1);
                                sResultRemark1[i] = VB.Right(strlast, strlast.Length - 1);
                                strlast = "";
                            }
                            else if (Convert.ToChar(VB.Left(strlast, 1)) == VB.Chr(10) && i != 1)
                            {
                                strlast = VB.Right(strlast, strlast.Length - 1);
                                Array.Resize<string>(ref sResultRemark1, sResultRemark1.Length + 1);
                                sResultRemark1[i] = VB.Left(strlast, strlast.Length - 1);
                                strlast = "";
                            }
                            else if (Convert.ToChar(VB.Right(strlast, 1)) == VB.Chr(13) && i != 1)
                            {
                                strlast = VB.Right(strlast, strlast.Length - 1);
                                Array.Resize<string>(ref sResultRemark1, sResultRemark1.Length + 1);
                                sResultRemark1[i] = VB.Left(strlast, strlast.Length - 1);
                                strlast = "";
                            }
                            else if (i == 1)
                            {
                                Array.Resize<string>(ref sResultRemark1, sResultRemark1.Length + 1);
                                sResultRemark1[i] = VB.Left(strlast, strlast.Length - 1);
                                strlast = "";
                            }
                        }
                        sresultremark = ' ';
                    }

                    i += 1;

                    if (strlast != "" && (strlast.Length >= 1 && Convert.ToChar(VB.Left(strlast, 1)) == VB.Chr(13) || Convert.ToChar(VB.Left(strlast, 1)) == VB.Chr(10)))
                    {
                        Array.Resize<string>(ref sResultRemark1, sResultRemark1.Length + 1);
                        sResultRemark1[i] = VB.Right(strlast, strlast.Length - 1);
                        strlast = "";
                    }
                    else if (strlast.Length >= 1)
                    {
                        this.ss_S_1.ActiveSheet.Cells[this.ss_S_1.ActiveSheet.Rows.Count - 1, 0].Text = strlast.Trim();
                        strlast = "";
                    }

                    this.ss_S_1.ActiveSheet.Rows.Count += 1;
                    this.ss_S_1.ActiveSheet.Cells[5, 0].Value = dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.AnatName.ToString()].ToString();

                    this.ss_S_1.ActiveSheet.Rows.Count += 1;

                    if (sResultRemark1.Length > 0)
                    {
                        //Array.Resize<string>(ref sResultRemark1, sResultRemark1.Length + 1);
                        //sResultRemark1[54] = "          mailgnancy of this case.";

                        for (int j = 0; j < sResultRemark1.Length; j++)
                        {
                            this.ss_S_1.ActiveSheet.Rows.Count += 1;
                            this.ss_S_1.ActiveSheet.AddSpanCell(6 + j, 0, 1, this.ss_S_1.ActiveSheet.Columns.Count - 1);
                            if (j > 19 && VB.Left(sResultRemark1[j], 15) == "               ")
                            {
                                if (VB.Left(sResultRemark1[j], 45) == "                                             ")
                                {
                                    this.ss_S_1.ActiveSheet.Cells[6 + j, 0].Text = "         " + sResultRemark1[j];
                                }
                                else
                                {
                                    this.ss_S_1.ActiveSheet.Cells[6 + j, 0].Text = "   " + sResultRemark1[j];
                                }
                            }
                            else
                            {
                                this.ss_S_1.ActiveSheet.Cells[6 + j, 0].Text = sResultRemark1[j];
                            }
                            //2019-04-23 안정수 추가( 2장분 내용을 1장으로하기위함
                            if (VB.Left(this.ss_S_1.ActiveSheet.Cells[1, 1].Value.ToString(), 2) == "OS")
                            {
                                this.ss_S_1.ActiveSheet.Cells[6 + j, 0].Row.Height = 16;
                            }
                        }
                    }
                }
                //this.ss_S.AllowCellOverflow = true;

             

                #endregion
            }

        }


        void printSS_S(DataTable dt, bool isPre)
        {
            string header = string.Empty;
            string foot = string.Empty;
            string DrGubun = string.Empty;
            char s = '"';

            clsSpread.SpdPrint_Margin margin = new clsSpread.SpdPrint_Margin(100, 50, 10, 10, 10, 10);
            clsSpread.SpdPrint_Option option = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait
                                            , PrintType.All, 0, 0, false, false, false, false, false, false, false);

            foot  = spread.setSpdPrint_String("                검체체취일 : " + dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RECEIVEDATE.ToString()].ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            foot += spread.setSpdPrint_String("                검사접수일 : " + dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.JDATE.ToString()].ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            foot += spread.setSpdPrint_String("                결과보고일 : " + dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTDATE.ToString()].ToString() + "               (" + " /p " + " / " + " /pc " + ")", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            if (dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTSABUN.ToString()].ToString() != "" && dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTSABUN.ToString()].ToString() == "박미옥")
            {
                //foot += spread.setSpdPrint_String("                결과입력자 : " + dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.GBSNAME.ToString()].ToString() + "                 판독의사 : 김미진 " + " /g" + s + 0 + s, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                foot += spread.setSpdPrint_String("                결과입력자 : " + dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.GBSNAME.ToString()].ToString() + "                 판독의사 :" + dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTSABUN.ToString()].ToString() + " /g" + s + 0 + s, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                foot += spread.setSpdPrint_String("                ---------------------------------------------------------------------------", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                foot += spread.setSpdPrint_String("                            우 37661 경북 포항시 남구 대잠동길 17", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                foot += spread.setSpdPrint_String("                        포항성모병원 병리과 ☎ :직) (054)260-8265, 대) (054)272-0151", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }

            else if (dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTSABUN.ToString()].ToString() != "" && dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTSABUN.ToString()].ToString() == "김미진")
            {
                foot += spread.setSpdPrint_String("                결과입력자 : " + dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.GBSNAME.ToString()].ToString() + "                 판독의사 :" + dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTSABUN.ToString()].ToString() + " /g" + s + 0 + s, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true); 
                foot += spread.setSpdPrint_String("                ---------------------------------------------------------------------------", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                foot += spread.setSpdPrint_String("                            우 37661 경북 포항시 남구 대잠동길 17", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                foot += spread.setSpdPrint_String("                        포항성모병원 병리과 ☎ :직) (054)260-8265, 대) (054)272-0151", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }
            else
            {
                foot += spread.setSpdPrint_String("                결과입력자 : " + dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.GBSNAME.ToString()].ToString() + "                 판독의사 :" + dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTSABUN.ToString()].ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                foot += spread.setSpdPrint_String("                ---------------------------------------------------------------------------", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                foot += spread.setSpdPrint_String("                            우 37661 경북 포항시 남구 대잠동길 17", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                foot += spread.setSpdPrint_String("                        포항성모병원 병리과 ☎ :직) (054)260-8265, 대) (054)272-0151", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }



            // 스프레드에 이미지를 추가해야 할 경우 참조(아래 주소를 통해 가면 예제 첨부파일이 있음.)

            // http://cafe.naver.com/grapecity : [재문의]바닥글 또는 머릿글에 이미지와 텍스트 같이 넣는 방법
            //DataTable dt = new DataTable();
            //dt.Columns.Add("Bool", typeof(bool));
            //dt.Columns.Add("Name", typeof(string));
            //dt.Rows.Add(true, "A");
            //dt.Rows.Add(true, "B");
            //dt.Rows.Add(false, "C");
            //dt.Rows.Add(false, "D");
            //fpSpread_Display.DataSource = dt;

            //FarPoint.Win.Spread.PrintInfo printset = new FarPoint.Win.Spread.PrintInfo();
            //printset.Colors = new Color[] { Color.Red, Color.Blue };
            //char s = '"';
            //printset.Header = "Print Job For /g" + s + 0 + s + " FPT Inc.";
            //printset.Footer = "This is Page /p/nof /pc Pages";
            //printset.Images = new Image[] { Image.FromFile("selectall.png"), Image.FromFile("selectall.png") };
            //printset.RepeatColStart = 1;
            //printset.RepeatColEnd = 25;
            //printset.RepeatRowStart = 1;
            //printset.RepeatRowEnd = 25;
            //printset.Preview = true;
            //// Assign the printer settings to the sheet and print it
            //fpSpread_Display.Sheets[0].PrintInfo = printset;
            //fpSpread_Display.PrintSheet(0);



            header += "/r                                                                           (" + " /p " + "/" + " /pc " + ")";
            if (dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTSABUN.ToString()].ToString() != "" && dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTSABUN.ToString()].ToString() == "박미옥")
            {
                spread.setSpdPrint(this.ss_S, isPre, margin, option, header, foot, Centering.Horizontal);
            }
            else if (dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTSABUN.ToString()].ToString() != "" && dt.Rows[0][clsPthlSQL.enmSel_EXAM_ANATMST_ANATPrint.RESULTSABUN.ToString()].ToString() == "김미진")
            {
                spread.setSpdPrint(this.ss_S_1, isPre, margin, option, header, foot, Centering.Horizontal);
            }

          


        }
      

        void printSS(enmType pType, bool isPre)
        {
            string header = string.Empty;
            string foot = string.Empty;

            clsSpread.SpdPrint_Margin margin = new clsSpread.SpdPrint_Margin(30, 10, 10, 10, 10, 10);
            clsSpread.SpdPrint_Option option = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait
                                            , PrintType.All, 0, 0, false, false, false, false, false, false, false);

            if (pType == enmType.ss_CR)
            {
                spread.setSpdPrint(this.ss_CR, isPre, margin, option, header, foot, Centering.Horizontal);
            }
            else if (pType == enmType.ss_PS)
            {
                spread.setSpdPrint(this.ss_PS, isPre, margin, option, header, foot, Centering.Horizontal);
            }
            else if (pType == enmType.ss_P)
            {
                spread.setSpdPrint(this.ss_P, isPre, margin, option, header, foot, Centering.Horizontal);
            }
            else if (pType == enmType.ss_PU)
            {
                spread.setSpdPrint(this.ss_PU, isPre, margin, option, header, foot, Centering.Horizontal);
            }
            else if (pType == enmType.ss_P)
            {
                spread.setSpdPrint(this.ss_P, isPre, margin, option, header, foot, Centering.Horizontal);
            }
            else if (pType == enmType.ss_C)
            {
                spread.setSpdPrint(this.ss_C, isPre, margin, option, header, foot, Centering.Horizontal);
            }
            else if (pType == enmType.ss_AC)
            {
                spread.setSpdPrint(this.ss_AC, isPre, margin, option, header, foot, Centering.Horizontal);
            }
            else if (pType == enmType.ss_AC2)
            {
                spread.setSpdPrint(this.ss_AC2, isPre, margin, option, header, foot, Centering.Horizontal);
            }
            else if (pType == enmType.ss_C_1)
            {
                spread.setSpdPrint(this.ss_C_1, isPre, margin, option, header, foot, Centering.Horizontal);
            }
            else if (pType == enmType.ss_AC_1)
            {
                spread.setSpdPrint(this.ss_AC_1, isPre, margin, option, header, foot, Centering.Horizontal);
            }

        }



    }
}
