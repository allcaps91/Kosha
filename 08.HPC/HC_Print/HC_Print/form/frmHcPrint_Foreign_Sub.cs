using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;


/// <summary>
/// Class Name      : HC_Print
/// File Name       : frmHcPrint_Foreign_Sub.cs
/// Description     : 외국인채용검진결과지
/// Author          : 김경동
/// Create Date     : 2021-02-19
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm채용신체검사_일반.frm(FrmIDateChange)" />

namespace HC_Print
{
    public partial class frmHcPrint_Foreign_Sub : Form
    {


        HIC_JEPSU_RES_SPECIAL_PATIENT nHJRSP = null;

        ComFunc cf = new ComFunc();
        clsHaBase hb = new clsHaBase();
        clsSpread cSpd = new clsSpread();

        HicJepsuPatientService hicJepsuPatientService = null;
        HicResultExCodeService hicResultExCodeService = null;
        HicJepsuPatientLtdService hicJepsuPatientLtdService = null;
        HicSpcPanjengService hicSpcPanjengService = null;



        public frmHcPrint_Foreign_Sub()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }

        public frmHcPrint_Foreign_Sub(HIC_JEPSU_RES_SPECIAL_PATIENT argnHJRSP, string argEname, string argGubun)
        {
            InitializeComponent();
            SetControl();
            SetEvent();

            nHJRSP = argnHJRSP;

        }
        private void SetControl()
        {
            hicJepsuPatientService = new HicJepsuPatientService();
            hicResultExCodeService = new HicResultExCodeService();
            hicJepsuPatientLtdService = new HicJepsuPatientLtdService();
            hicSpcPanjengService = new HicSpcPanjengService();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormload);
        }

        private void eFormload(object sender, EventArgs e)
        {
            this.Hide();
            Result_Setting();

            Result_Print_Sub1();
            Result_Print_Sub2();
            Result_Print_Sub3();

            ComFunc.Delay(1500);
            this.Close();

        }
        private void Result_Setting()
        {
            long nWRTNO = 0;
            long nWrtno2 = 0;
            long nChk = 0;
            string strPart = "";
            string strResult = "";
            string strResCode = "";
            string strResultType = "";
            string strGbCodeUse = "";
            string strNomal = "";
            string strDrname = "";
            string strYear = "";
            string strSex = "";
            string strGbSpc = "";


            List<HIC_RESULT_EXCODE> list = hicResultExCodeService.GetItembyOnlyWrtNo(nHJRSP.WRTNO);

            if (list.Count > 0)
            {
                SS2.ActiveSheet.RowCount = list.Count;

                for (int i = 0; i <= list.Count - 1; i++)
                {

                    strResult = list[i].RESULT;             //검사실 결과값
                    strResCode = list[i].RESCODE;           //결과값 코드
                    strResultType = list[i].RESULTTYPE;      //결과값 TYPE
                    strGbCodeUse = list[i].GBCODEUSE;       //결과값코드 사용여부

                    SS2.ActiveSheet.Cells[i, 0].Text = list[i].EXCODE;
                    SS2.ActiveSheet.Cells[i, 1].Text = list[i].HNAME;
                    SS2.ActiveSheet.Cells[i, 2].Text = strResult;
                    SS2.ActiveSheet.Cells[i, 3].Text = list[i].UNIT;
                    if (strGbCodeUse == "Y")
                    {
                        if (!strResult.IsNullOrEmpty())
                        {
                            SS2.ActiveSheet.Cells[i, 4].Text = hb.READ_ResultName(strResCode, strResult);
                        }
                    }
                    if (list[i].PANJENG == "2")
                    {
                        SS2.ActiveSheet.Cells[i, 5].Text = "*";
                    }

                    //참고치 Display
                    if (strSex == "M")
                    {
                        strNomal = list[i].MIN_M + "~" + list[i].MAX_M;
                    }
                    else
                    {
                        strNomal = list[i].MIN_F + "~" + list[i].MAX_F;
                    }

                    if (strNomal == "~") { strNomal = ""; }

                    SS2.ActiveSheet.Cells[i, 6].Text = strNomal;
                }
            }
        }

        private void Result_Print_Sub1()
        {
            long nCNT = 0;
            long nREAD = 0;
            long nResult = 0;
            string strData = "";
            string strCode = "";
            string strHH = "";
            string strHL = "";
            string strYD = "";
            string strYJ = "";
            string strDan = "";
            string strABO = "";
            string strRh = "";
            string strHBsAg = "";
            string strHBsAb = "";
            string strHBS = "";
            string strPanjeng = "";
            string strjuksung = "";
            string strJochi = "";
            string strSogen = "";
            string strRemark = "";
            string strPandate = "";
            string strJepDate = "";
            string strHname = "";
            string strE923 = "";
            string strTemp = "";



            HIC_JEPSU_PATIENT_LTD item = hicJepsuPatientLtdService.GetItemByWrtno(nHJRSP.WRTNO);
            if (!item.IsNullOrEmpty())
            {

                SSPrint1.ActiveSheet.Cells[5, 5].Text = nHJRSP.SNAME;

                strTemp = VB.Mid(item.JUMIN, 7, 1);
                switch (strTemp)
                {
                    case "1":
                    case "2":
                    case "5":
                    case "6":
                        strTemp = "19"; break;
                    case "3":
                    case "4":
                    case "7":
                    case "8":
                        strTemp = "20"; break;
                    default:
                        strTemp = "18"; break;
                }

                SSPrint1.ActiveSheet.Cells[5, 14].Text = strTemp + VB.Left(item.JUMIN, 2) + "  년  " + VB.Mid(item.JUMIN, 3, 2) + "  월  " + VB.Right(item.JUMIN, 5) + "  일";
                SSPrint1.ActiveSheet.Cells[6, 5].Text = item.SANGHO;
                SSPrint1.ActiveSheet.Cells[6, 14].Text = item.HPHONE;

                for (int i = 0; i <= SS2.ActiveSheet.RowCount - 1; i++)
                {
                    strData = SS2.ActiveSheet.Cells[i, 2].Text;
                    if (!strData.IsNullOrEmpty())
                    {
                        strCode = SS2.ActiveSheet.Cells[i, 0].Text;
                        switch (strCode)
                        {
                            case "A101": SSPrint1.ActiveSheet.Cells[10, 5].Text = strData.Trim(); break;           //신장
                            case "A102": SSPrint1.ActiveSheet.Cells[10, 16].Text = strData.Trim(); break;           //체중

                            case "ZD04": SSPrint1.ActiveSheet.Cells[11, 5].Text = strData.Trim(); break;            //흉부

                            case "A108": strHH = strData.Trim(); break;         //혈압 고
                            case "A109": strHL = strData.Trim(); break;         //혈압 저

                            case "A104": SSPrint1.ActiveSheet.Cells[12, 5].Text = "좌 : " + strData.Trim(); break;
                            case "A105": SSPrint1.ActiveSheet.Cells[13, 5].Text = "우 : " + strData.Trim(); break;
                            case "TE05": SSPrint1.ActiveSheet.Cells[12, 13].Text = strData.Trim(); break;
                            case "A106": SSPrint1.ActiveSheet.Cells[12, 20].Text = "좌 : " + strData.Trim(); break;
                            case "A107": SSPrint1.ActiveSheet.Cells[13, 20].Text = "우 : " + strData.Trim(); break;

                            case "TZ18": SSPrint1.ActiveSheet.Cells[14, 5].Text = strData.Trim(); break;
                            case "TZ23": SSPrint1.ActiveSheet.Cells[14, 16].Text = strData.Trim(); break;
                            case "ZD00": SSPrint1.ActiveSheet.Cells[15, 5].Text = strData.Trim(); break;
                            case "TZ24": SSPrint1.ActiveSheet.Cells[15, 16].Text = strData.Trim(); break;
                            case "TZ19": SSPrint1.ActiveSheet.Cells[16, 5].Text = strData.Trim(); break;
                            case "TZ25": SSPrint1.ActiveSheet.Cells[16, 16].Text = strData.Trim(); break;
                            case "TZ20": SSPrint1.ActiveSheet.Cells[17, 5].Text = strData.Trim(); break;
                            case "TZ26": SSPrint1.ActiveSheet.Cells[17, 16].Text = strData.Trim(); break;
                            case "TZ21": SSPrint1.ActiveSheet.Cells[18, 5].Text = strData.Trim(); break;
                            case "TZ27": SSPrint1.ActiveSheet.Cells[18, 16].Text = strData.Trim(); break;

                            //요당
                            case "A111":
                                if (strData.Trim() == "01")
                                {
                                    strYD = "-";
                                }
                                else if (strData.Trim() == "02")
                                {
                                    strYD = "+";
                                }
                                break;

                            //요반백
                            case "A112":
                                if (strData.Trim() == "01")
                                {
                                    strDan = "-";
                                }
                                else if (strData.Trim() == "02")
                                {
                                    strDan = "+-";
                                }
                                else if (strData.Trim() == "03")
                                {
                                    strDan = "+";
                                }
                                else if (strData.Trim() == "04")
                                {
                                    strDan = "+2";
                                }
                                else if (strData.Trim() == "05")
                                {
                                    strDan = "+3";
                                }
                                else if (strData.Trim() == "06")
                                {
                                    strDan = "+4";
                                }
                                break;

                            //요잠혈
                            case "A113":
                                if (strData.Trim() == "01")
                                {
                                    strYJ = "-";
                                }
                                else if (strData.Trim() == "02")
                                {
                                    strYJ = "+-";
                                }
                                else if (strData.Trim() == "03")
                                {
                                    strYJ = "+";
                                }
                                else if (strData.Trim() == "04")
                                {
                                    strYJ = "+2";
                                }
                                else if (strData.Trim() == "05")
                                {
                                    strYJ = "+3";
                                }
                                else if (strData.Trim() == "06")
                                {
                                    strYJ = "+4";
                                }
                                break;

                            case "TZ22": SSPrint1.ActiveSheet.Cells[19, 5].Text = strData.Trim(); break;
                            case "LU39": SSPrint1.ActiveSheet.Cells[19, 16].Text = strData.Trim(); break;

                            case "A142":
                                switch (strData)
                                {
                                    case "01": SSPrint1.ActiveSheet.Cells[20, 5].Text = "정상"; break;
                                    case "02": SSPrint1.ActiveSheet.Cells[20, 5].Text = "사진불량"; break;
                                    case "03": SSPrint1.ActiveSheet.Cells[20, 5].Text = "비활동성(정상)"; break;
                                    case "07": SSPrint1.ActiveSheet.Cells[20, 5].Text = "폐결핵 의증"; break;
                                    case "08": SSPrint1.ActiveSheet.Cells[20, 5].Text = "비결핵성 질환"; break;
                                    case "09": SSPrint1.ActiveSheet.Cells[20, 5].Text = "순환기계 질환"; break;
                                    case "10": SSPrint1.ActiveSheet.Cells[20, 5].Text = "진단미정"; break;
                                    case "11": SSPrint1.ActiveSheet.Cells[20, 5].Text = "미촬영"; break;
                                    case "12": SSPrint1.ActiveSheet.Cells[20, 5].Text = "유질환자"; break;
                                    case "13": SSPrint1.ActiveSheet.Cells[20, 5].Text = "비활동성 폐결핵"; break;
                                    default:
                                        break;
                                }
                                break;
                            //case "E921":  SSPrint1.ActiveSheet.Cells[20, 5].Text = VB.IIf(strData == "01", " Non Reactive", strData); break;
                            //case "E922":  SSPrint1.ActiveSheet.Cells[20, 5].Text = VB.IIf(strData.Trim() == "01", "Negative", strData); break;
                            case "E924":  SSPrint1.ActiveSheet.Cells[20, 5].Text = strData.Trim(); break;
                            case "E925":  SSPrint1.ActiveSheet.Cells[20, 5].Text = strData.Trim(); break;
                            case "E926":SSPrint1.ActiveSheet.Cells[20, 5].Text = strData.Trim(); break;

                            default:
                                break;
                        }
                    }
                }

                if (!strHH.IsNullOrEmpty() || !strHL.IsNullOrEmpty())
                {
                    SSPrint1.ActiveSheet.Cells[11, 16].Text = strHH + "  /  " + strHL + " mm/Hg";
                }

                //검사일자
                strJepDate = item.JEPDATE;
                SSPrint1.ActiveSheet.Cells[25, 13].Text = VB.Left(strJepDate, 4) + "  년  " + VB.Mid(strJepDate, 6, 2) + "  월  " + VB.Right(strJepDate, 2) + "  일";

                SSPrint1.ActiveSheet.Cells[27, 16].Text = hb.READ_License_DrName(item.PANJENGDRNO);

                HIC_SPC_PANJENG item2 = hicSpcPanjengService.GetAllbyWrtNo(nHJRSP.WRTNO);

                //검사결과합격여부
                if (!item2.IsNullOrEmpty())
                {
                    strPanjeng = item2.PANJENG;
                    strJochi = item2.JOCHICODE;
                    strSogen = item2.SOGENCODE;
                }
                
                if (strPanjeng == "1" || strPanjeng == "2" || strPanjeng == "3" || strPanjeng == "4")
                {
                    SSPrint1.ActiveSheet.Cells[32, 7].Text = "√";
                }
                else if (strPanjeng == "5" || strPanjeng == "6" )
                {
                    SSPrint1.ActiveSheet.Cells[36, 7].Text = "√";
                }
                else if ( strPanjeng == "7" || strPanjeng == "8")
                {
                    SSPrint1.ActiveSheet.Cells[34, 7].Text = "√";
                }

                //불합격사유
                if (strPanjeng == "5" || strPanjeng == "6")
                {
                    SSPrint1.ActiveSheet.Cells[31, 17].Text = " 정밀검사요함";
                }
                else
                {
                    SSPrint1.ActiveSheet.Cells[31, 17].Text = " 없음";
                }


                    //환자유의사항
                if (strPanjeng == "7" || strPanjeng == "8")
                {
                    strRemark = item2.SOGENREMARK;
                    if(!strRemark.IsNullOrEmpty())
                    {
                        SSPrint1.ActiveSheet.Cells[38, 6].Text = " "+ strRemark;
                    }
                }
                else
                {
                    SSPrint1.ActiveSheet.Cells[38, 6].Text = " 없음";
                }

                //판정일자
                SSPrint1.ActiveSheet.Cells[45, 13].Text = VB.Left(strPandate, 4) + "  년  " + VB.Mid(strPandate, 6, 2) + "  월  " + VB.Right(strPandate, 2) + "  일";
            }

            //출력
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "";
            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Auto, PrintType.All, 0, 0, false, false, true, true, false, false, true);

            cSpd.setSpdPrint(SSPrint1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }


        private void Result_Print_Sub2()
        {

            long nCNT = 0;
            long nREAD = 0;
            long nResult = 0;
            string strData = "";
            string strCode = "";
            string strHH = "";
            string strHL = "";
            string strYD = "";
            string strYJ = "";
            string strDan = "";
            string strABO = "";
            string strRh = "";
            string strHBsAg = "";
            string strHBsAb = "";
            string strHBS = "";
            string strPanjeng = "";
            string strjuksung = "";
            string strJochi = "";
            string strSogen = "";
            string strRemark = "";
            string strPandate = "";
            string strJepDate = "";
            string strHname = "";
            string strE923 = "";
            string strTemp = "";
            string strResult1 = "";



            HIC_JEPSU_PATIENT_LTD item = hicJepsuPatientLtdService.GetItemByWrtno(nHJRSP.WRTNO);
            if (!item.IsNullOrEmpty())
            {

                SSPrint2.ActiveSheet.Cells[5, 5].Text = nHJRSP.SNAME;

                strTemp = VB.Mid(item.JUMIN, 7, 1);
                switch (strTemp)
                {
                    case "1":
                    case "2":
                    case "5":
                    case "6":
                        strTemp = "19"; break;
                    case "3":
                    case "4":
                    case "7":
                    case "8":
                        strTemp = "20"; break;
                    default:
                        strTemp = "18"; break;
                }

                SSPrint2.ActiveSheet.Cells[5, 14].Text = strTemp + VB.Left(item.JUMIN, 2) + "  년  " + VB.Mid(item.JUMIN, 3, 2) + "  월  " + VB.Right(item.JUMIN, 5) + "  일";
                if(!item.SANGHO.IsNullOrEmpty())
                {
                    SSPrint2.ActiveSheet.Cells[6, 5].Text = item.SANGHO;
                }
                else
                {
                    SSPrint2.ActiveSheet.Cells[6, 5].Text = item.JUSO1 + " " +item.JUSO2;
                }
                SSPrint2.ActiveSheet.Cells[6, 14].Text = item.HPHONE;

                for (int i = 0; i <= SS2.ActiveSheet.RowCount - 1; i++)
                {
                    strData = SS2.ActiveSheet.Cells[i, 2].Text;
                    if (!strData.IsNullOrEmpty())
                    {
                        strCode = SS2.ActiveSheet.Cells[i, 0].Text;
                        switch (strCode)
                        {
                            case "A101": SSPrint2.ActiveSheet.Cells[10, 4].Text = strData.Trim(); break;           //신장
                            case "A102": SSPrint2.ActiveSheet.Cells[10, 13].Text = strData.Trim(); break;           //체중

                            case "A108": strHH = strData.Trim(); break;         //혈압 고
                            case "A109": strHL = strData.Trim(); break;         //혈압 저

                            case "A104": SSPrint2.ActiveSheet.Cells[11, 4].Text = "좌 : " + strData.Trim(); break;
                            case "A105": SSPrint2.ActiveSheet.Cells[12, 4].Text = "우 : " + strData.Trim(); break;
                            case "TE05": SSPrint2.ActiveSheet.Cells[11, 13].Text = strData.Trim(); break;
                            case "A106": SSPrint2.ActiveSheet.Cells[11, 20].Text = "좌 : " + strData.Trim(); break;
                            case "A107": SSPrint2.ActiveSheet.Cells[12, 20].Text = "우 : " + strData.Trim(); break;

                            case "TZ27": SSPrint2.ActiveSheet.Cells[13, 16].Text = strData.Trim(); break;
                            case "LU39": SSPrint2.ActiveSheet.Cells[14, 5].Text = strData.Trim(); break;

                            case "A142":
                                switch (strData)
                                {
                                    case "01": SSPrint2.ActiveSheet.Cells[13, 5].Text = "정상"; break;
                                    case "02": SSPrint2.ActiveSheet.Cells[13, 5].Text = "사진불량"; break;
                                    case "03": SSPrint2.ActiveSheet.Cells[13, 5].Text = "비활동성(정상)"; break;
                                    case "07": SSPrint2.ActiveSheet.Cells[13, 5].Text = "폐결핵 의증"; break;
                                    case "08": SSPrint2.ActiveSheet.Cells[13, 5].Text = "비결핵성 질환"; break;
                                    case "09": SSPrint2.ActiveSheet.Cells[13, 5].Text = "순환기계 질환"; break;
                                    case "10": SSPrint2.ActiveSheet.Cells[13, 5].Text = "진단미정"; break;
                                    case "11": SSPrint2.ActiveSheet.Cells[13, 5].Text = "미촬영"; break;
                                    case "12": SSPrint2.ActiveSheet.Cells[13, 5].Text = "유질환자"; break;
                                    case "13": SSPrint2.ActiveSheet.Cells[13, 5].Text = "비활동성 폐결핵"; break;
                                    default:
                                        break;
                                }
                                break;
                            case "A131": strResult1 = strData.Trim(); break;

                            //case "E922":  SSPrint2.ActiveSheet.Cells[16, 3].Text = VB.IIf(strData == "01", " Non Reactive", strData); break;
                            case "E924": SSPrint2.ActiveSheet.Cells[16, 7].Text = strData.Trim(); break;
                            case "E925": SSPrint2.ActiveSheet.Cells[16, 12].Text = strData.Trim(); break;
                            case "E926": SSPrint2.ActiveSheet.Cells[16, 16].Text = strData.Trim(); break;

                            default: break;
                        }
                    }
                }

                if (!strHH.IsNullOrEmpty() || !strHL.IsNullOrEmpty())
                {
                    SSPrint1.ActiveSheet.Cells[10, 20].Text = strHH + "  /  " + strHL + " mm/Hg";
                }
                if (strResult1 =="01")
                {
                    SSPrint1.ActiveSheet.Cells[14, 16].Text = "음성";
                }
                else if (strResult1 == "02")
                {
                    SSPrint1.ActiveSheet.Cells[14, 16].Text = "양성";
                }


                //검사일자
                strJepDate = item.JEPDATE;
                SSPrint2.ActiveSheet.Cells[19, 13].Text = VB.Left(strJepDate, 4) + "  년  " + VB.Mid(strJepDate, 6, 2) + "  월  " + VB.Right(strJepDate, 2) + "  일";

                SSPrint2.ActiveSheet.Cells[21, 16].Text = hb.READ_License_DrName(item.PANJENGDRNO);

                HIC_SPC_PANJENG item2 = hicSpcPanjengService.GetAllbyWrtNo(nHJRSP.WRTNO);

                //검사결과합격여부
                if (!item2.IsNullOrEmpty())
                {
                    strPanjeng = item2.PANJENG;
                    strJochi = item2.JOCHICODE;
                    strSogen = item2.SOGENCODE;
                }

                if (strPanjeng == "1" || strPanjeng == "2" || strPanjeng == "3" || strPanjeng == "4")
                {
                    SSPrint2.ActiveSheet.Cells[26, 8].Text = "√";
                }
                else if (strPanjeng == "5" || strPanjeng == "6")
                {
                    SSPrint2.ActiveSheet.Cells[26, 11].Text = "√";
                }


                //불합격사유
                if (strPanjeng == "5" || strPanjeng == "6")
                {
                    SSPrint2.ActiveSheet.Cells[28, 11].Text = " 정밀검사요함";
                }
                else
                {
                    SSPrint2.ActiveSheet.Cells[28, 11].Text = " 없음";
                }


                //환자유의사항
                if (strPanjeng == "7" || strPanjeng == "8")
                {
                    strRemark = item2.SOGENREMARK;
                    if (!strRemark.IsNullOrEmpty())
                    {
                        SSPrint2.ActiveSheet.Cells[29, 6].Text = " " + strRemark;
                    }
                }
                else
                {
                    SSPrint2.ActiveSheet.Cells[29, 6].Text = " 없음";
                }

                //판정일자
                SSPrint2.ActiveSheet.Cells[39, 13].Text = VB.Left(strPandate, 4) + "  년  " + VB.Mid(strPandate, 6, 2) + "  월  " + VB.Right(strPandate, 2) + "  일";
            }

            //출력
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "";
            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Auto, PrintType.All, 0, 0, false, false, true, true, false, false, true);

            cSpd.setSpdPrint(SSPrint2, PrePrint, setMargin, setOption, strHeader, strFooter);
        }


        private void Result_Print_Sub3()
        {
            long nCNT = 0;
            long nREAD = 0;
            long nResult = 0;
            string strData = "";
            string strCode = "";
            string strHH = "";
            string strHL = "";
            string strYD = "";
            string strYJ = "";
            string strDan = "";
            string strABO = "";
            string strRh = "";
            string strHBsAg = "";
            string strHBsAb = "";
            string strHBS = "";
            string strPanjeng = "";
            string strjuksung = "";
            string strJochi = "";
            string strSogen = "";
            string strRemark = "";
            string strPandate = "";
            string strJepDate = "";
            string strHname = "";
            string strE923 = "";
            string strTemp = "";
            string strResult1 = "";



            HIC_JEPSU_PATIENT_LTD item = hicJepsuPatientLtdService.GetItemByWrtno(nHJRSP.WRTNO);
            if (!item.IsNullOrEmpty())
            {

                SSPrint3.ActiveSheet.Cells[5, 5].Text = nHJRSP.SNAME;

                strTemp = VB.Mid(item.JUMIN, 7, 1);
                switch (strTemp)
                {
                    case "1":
                    case "2":
                    case "5":
                    case "6":
                        strTemp = "19"; break;
                    case "3":
                    case "4":
                    case "7":
                    case "8":
                        strTemp = "20"; break;
                    default:
                        strTemp = "18"; break;
                }

                SSPrint3.ActiveSheet.Cells[5, 14].Text = strTemp + VB.Left(item.JUMIN, 2) + "  년  " + VB.Mid(item.JUMIN, 3, 2) + "  월  " + VB.Right(item.JUMIN, 5) + "  일";
                if (!item.SANGHO.IsNullOrEmpty())
                {
                    SSPrint3.ActiveSheet.Cells[6, 5].Text = item.SANGHO;
                }
                else
                {
                    SSPrint3.ActiveSheet.Cells[6, 5].Text = item.JUSO1 + " " + item.JUSO2;
                }
                SSPrint3.ActiveSheet.Cells[6, 14].Text = item.HPHONE;

                for (int i = 0; i <= SS2.ActiveSheet.RowCount - 1; i++)
                {
                    strData = SS2.ActiveSheet.Cells[i, 2].Text;
                    if (!strData.IsNullOrEmpty())
                    {
                        strCode = SS2.ActiveSheet.Cells[i, 0].Text;
                        switch (strCode)
                        {
                            case "A101": SSPrint3.ActiveSheet.Cells[10, 4].Text = strData.Trim() + "cm"; break;           //신장
                            case "A102": SSPrint3.ActiveSheet.Cells[10, 13].Text = strData.Trim() + "kg"; break;           //체중

                            case "A108": strHH = strData.Trim(); break;         //혈압 고
                            case "A109": strHL = strData.Trim(); break;         //혈압 저

                            case "A104": SSPrint3.ActiveSheet.Cells[11, 4].Text = "좌 : " + strData.Trim(); break;
                            case "A105": SSPrint3.ActiveSheet.Cells[12, 4].Text = "우 : " + strData.Trim(); break;
                            case "TE05": SSPrint3.ActiveSheet.Cells[11, 13].Text = strData.Trim(); break;
                            case "A106": SSPrint3.ActiveSheet.Cells[11, 20].Text = "좌 : " + strData.Trim(); break;
                            case "A107": SSPrint3.ActiveSheet.Cells[12, 20].Text = "우 : " + strData.Trim(); break;

                            //case "E922":  SSPrint3.ActiveSheet.Cells[16, 3].Text = VB.IIf(strData == "01", " Non Reactive", strData); break;
                            case "E924": SSPrint3.ActiveSheet.Cells[16, 7].Text = strData.Trim(); break;
                            case "E925": SSPrint3.ActiveSheet.Cells[16, 12].Text = strData.Trim(); break;
                            case "E926": SSPrint3.ActiveSheet.Cells[16, 16].Text = strData.Trim(); break;

                            default: break;
                        }
                    }
                }

                if (!strHH.IsNullOrEmpty() || !strHL.IsNullOrEmpty())
                {
                    SSPrint1.ActiveSheet.Cells[10, 20].Text = strHH + "  /  " + strHL + " mm/Hg";
                }


                //검사일자
                strJepDate = item.JEPDATE;
                SSPrint3.ActiveSheet.Cells[19, 13].Text = VB.Left(strJepDate, 4) + "  년  " + VB.Mid(strJepDate, 6, 2) + "  월  " + VB.Right(strJepDate, 2) + "  일";

                SSPrint3.ActiveSheet.Cells[21, 16].Text = hb.READ_License_DrName(item.PANJENGDRNO);

                HIC_SPC_PANJENG item2 = hicSpcPanjengService.GetAllbyWrtNo(nHJRSP.WRTNO);

                //검사결과합격여부
                if (!item2.IsNullOrEmpty())
                {
                    strPanjeng = item2.PANJENG;
                    strJochi = item2.JOCHICODE;
                    strSogen = item2.SOGENCODE;
                }

                if (strPanjeng == "1" || strPanjeng == "2" || strPanjeng == "3" || strPanjeng == "4")
                {
                    SSPrint3.ActiveSheet.Cells[26, 8].Text = "√";
                }
                else if (strPanjeng == "5" || strPanjeng == "6")
                {
                    SSPrint3.ActiveSheet.Cells[26, 11].Text = "√";
                }


                //불합격사유
                if (strPanjeng == "5" || strPanjeng == "6")
                {
                    SSPrint3.ActiveSheet.Cells[28, 11].Text = " 정밀검사요함";
                }
                else
                {
                    SSPrint3.ActiveSheet.Cells[28, 11].Text = " 없음";
                }


                //환자유의사항
                if (strPanjeng == "7" || strPanjeng == "8")
                {
                    strRemark = item2.SOGENREMARK;
                    if (!strRemark.IsNullOrEmpty())
                    {
                        SSPrint3.ActiveSheet.Cells[29, 6].Text = " " + strRemark;
                    }
                }
                else
                {
                    SSPrint3.ActiveSheet.Cells[29, 6].Text = " 없음";
                }

                //판정일자
                SSPrint3.ActiveSheet.Cells[39, 13].Text = VB.Left(strPandate, 4) + "  년  " + VB.Mid(strPandate, 6, 2) + "  월  " + VB.Right(strPandate, 2) + "  일";
            }
            //출력
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "";
            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Auto, PrintType.All, 0, 0, false, false, true, true, false, false, true);

            cSpd.setSpdPrint(SSPrint3, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

    }
}
