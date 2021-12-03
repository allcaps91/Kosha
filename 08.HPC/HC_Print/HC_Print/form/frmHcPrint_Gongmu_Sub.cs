using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HC_Print
{
    public partial class frmHcPrint_Gongmu_Sub : Form
    {
        string fstrJunminAll = "";
        string fstrJong = "";

        HIC_JEPSU_RES_SPECIAL_PATIENT nHJRSP = null;

        ComFunc cf = new ComFunc();
        clsHaBase hb = new clsHaBase();
        clsSpread cSpd = new clsSpread();

        HicResultExCodeService hicResultExCodeService = null;
        HicSunapdtlService hicSunapdtlService = null;
        HicResSpecialService hicResSpecialService = null;
        HicJepsuService hicJepsuService = null;
        HicSpcPanjengService hicSpcPanjengService = null;

        public frmHcPrint_Gongmu_Sub()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }

        public frmHcPrint_Gongmu_Sub(HIC_JEPSU_RES_SPECIAL_PATIENT argnHJRSP, string argGubunJumin, string argGubunJong)
        {
            InitializeComponent();
            SetControl();
            SetEvent();

            nHJRSP = argnHJRSP;
            fstrJunminAll = argGubunJumin;
            fstrJong = argGubunJong;
        }

        private void SetControl()
        {

            nHJRSP = new HIC_JEPSU_RES_SPECIAL_PATIENT();

            hicResultExCodeService = new HicResultExCodeService();
            hicSunapdtlService = new HicSunapdtlService();
            hicResSpecialService = new HicResSpecialService();
            hicJepsuService = new HicJepsuService();
            hicSpcPanjengService = new HicSpcPanjengService();

        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormload);
        }

        private void eFormload(object sender, EventArgs e)
        {
            this.Hide();

            Result_Print_Clear();       //Sheet Clear

            Result_Setting();
            Result_Print_Sub();

            //HIC_RES_SPECIAL 인쇄일자, 사번을 업데이트
            hicResSpecialService.UpdatePRINTbyWrtNo(nHJRSP.WRTNO, clsType.User.IdNumber.To<long>());

            //HIC_JEPSU 인쇄일자, 사번을 업데이트
            hicJepsuService.UpdatePRINTbyWrtNo(nHJRSP.WRTNO, clsType.User.IdNumber.To<long>(), clsPublic.GstrSysDate);

            ComFunc.Delay(1500);
            this.Close();
        }

        private void Result_Print_Clear()
        {

            #region 채용검진
            SS_Print.ActiveSheet.Cells[7, 12].Text = "";
            SS_Print.ActiveSheet.Cells[9, 14].Text = "";

            for (int i = 12; i < 23; i++)
            {
                if (i != 14 || i != 15)
                {
                    SS_Print.ActiveSheet.Cells[i, 4].Text = "";
                    SS_Print.ActiveSheet.Cells[i, 14].Text = "";
                }
            }

            SS_Print.ActiveSheet.Cells[14, 3].Text = "";
            SS_Print.ActiveSheet.Cells[14, 11].Text = "";
            SS_Print.ActiveSheet.Cells[14, 16].Text = "";

            SS_Print.ActiveSheet.Cells[15, 3].Text = "";
            SS_Print.ActiveSheet.Cells[15, 16].Text = "";

            SS_Print.ActiveSheet.Cells[23, 3].Text = "";

            //검사일자
            SS_Print.ActiveSheet.Cells[27, 11].Text = "";
            SS_Print.ActiveSheet.Cells[29, 14].Text = "";

            //검사결과합격여부
            SS_Print.ActiveSheet.Cells[33, 5].Text = "";
            SS_Print.ActiveSheet.Cells[35, 5].Text = "";
            SS_Print.ActiveSheet.Cells[37, 5].Text = "";

            //불합격사유
            SS_Print.ActiveSheet.Cells[33, 14].Text = "";

            //정밀검진대상자 및 적성검사자
            SS_Print.ActiveSheet.Cells[40, 5].Text = "";

            //판정일자
            SS_Print.ActiveSheet.Cells[46, 11].Text = "";
            #endregion

            #region 소방공무원
            SS_Print2.ActiveSheet.Cells[8, 12].Text = "";
            SS_Print2.ActiveSheet.Cells[9, 14].Text = "";

            for (int i = 12; i < 23; i++)
            {
                if (i != 14 || i != 15)
                {
                    SS_Print2.ActiveSheet.Cells[i, 4].Text = "";
                    SS_Print2.ActiveSheet.Cells[i, 14].Text = "";
                }
            }

            SS_Print2.ActiveSheet.Cells[12, 3].Text = "";

            SS_Print2.ActiveSheet.Cells[14, 3].Text = "";
            SS_Print2.ActiveSheet.Cells[14, 11].Text = "";
            SS_Print2.ActiveSheet.Cells[14, 16].Text = "";

            SS_Print2.ActiveSheet.Cells[15, 3].Text = "";
            SS_Print2.ActiveSheet.Cells[15, 16].Text = "";



            //검사일자
            SS_Print2.ActiveSheet.Cells[27, 11].Text = "";
            SS_Print2.ActiveSheet.Cells[29, 14].Text = "";

            //검사결과합격여부
            SS_Print2.ActiveSheet.Cells[33, 5].Text = "";
            SS_Print2.ActiveSheet.Cells[35, 5].Text = "";
            SS_Print2.ActiveSheet.Cells[37, 5].Text = "";

            //불합격사유
            SS_Print2.ActiveSheet.Cells[32, 13].Text = "";

            //정밀검진대상자 및 적성검사자
            SS_Print2.ActiveSheet.Cells[40, 5].Text = "";

            //판정일자
            SS_Print2.ActiveSheet.Cells[46, 11].Text = "";
            #endregion   

        }


        private void Result_Setting()
        {
            string strResult = "";
            string strResCode = "";
            string strResultType = "";
            string strGbCodeUse = "";
            string strNomal = "";
            string strSex = "";

            List<HIC_RESULT_EXCODE> list = hicResultExCodeService.GetItembyOnlyWrtNo(nHJRSP.WRTNO);

            if (list.Count > 0)
            {
                SS2.ActiveSheet.RowCount = list.Count;

                for (int i = 0; i < list.Count; i++)
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
        private void Result_Print_Sub()
        {

            long nWrtno = 0;
            long nCNT = 0;
            long nREAD = 0;
            long nResult = 0;
            long nDrno = 0;
            bool bPolice = false;

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
            string strRemark1 = "";
            string strGbSpc = "";

            nWrtno = nHJRSP.WRTNO;
            strJepDate = nHJRSP.JEPDATE;

            //검사결과 SQL_RESULT
            HIC_RES_SPECIAL item = hicResSpecialService.GetItemByWrtno(nWrtno);
            if (!item.IsNullOrEmpty())
            {
                nDrno = item.PANJENGDRNO;
                strPandate = item.PANJENGDATE;
                strGbSpc = item.GBSPC;
            }

            HIC_JEPSU item2 = hicJepsuService.GetItemByWRTNO(nWrtno);


            if (hicSunapdtlService.GetCountbyWrtNoCode(nWrtno, "2116") > 0)
            {
                bPolice = true; ;
            }
        }

        private void Result_Print_Sub1()
        {

            long nWrtno = 0;
            long nCNT = 0;
            long nREAD = 0;
            long nResult = 0;
            long nDrno = 0;
            bool bPolice = false;

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
            string strRemark1 = "";
            string strGbSpc = "";

            //성명
            SS_Print.ActiveSheet.Cells[7, 12].Text = nHJRSP.SNAME;

            //한자성명
            SS_Print.ActiveSheet.Cells[8, 12].Text = nHJRSP.HNAME;

            //주민등록번호
            if (fstrJunminAll == "Y")
            {

            }
            else
            {
                SS_Print.ActiveSheet.Cells[9, 14].Text = VB.Left(nHJRSP.JUMIN, 6) + "-" + VB.Right(nHJRSP.JUMIN, 7);
            }

            for (int i = 0; i <= SS2.ActiveSheet.RowCount - 1; i++)
            {
                strData = SS2.ActiveSheet.Cells[i, 2].Text;
                if (!strData.IsNullOrEmpty())
                {
                    strCode = SS2.ActiveSheet.Cells[i, 0].Text;
                    switch (strCode)
                    {
                        case "A101": SS_Print.ActiveSheet.Cells[12, 4].Text = strData.Trim(); break;           //신장
                        case "A102": SS_Print.ActiveSheet.Cells[12, 14].Text = strData.Trim(); break;           //체중

                        case "A115": SS_Print.ActiveSheet.Cells[13, 4].Text = strData.Trim(); break;            //허리둘레

                        case "A108": strHH = strData.Trim(); break;         //혈압 고
                        case "A109": strHL = strData.Trim(); break;         //혈압 저

                        case "A104": SS_Print.ActiveSheet.Cells[14, 3].Text = "좌 : " + strData.Trim(); break;
                        case "A105": SS_Print.ActiveSheet.Cells[15, 3].Text = "우 : " + strData.Trim(); break;
                        case "TE05": SS_Print.ActiveSheet.Cells[14, 11].Text = strData.Trim(); break;
                        case "A106": SS_Print.ActiveSheet.Cells[14, 16].Text = "좌 : " + strData.Trim(); break;
                        case "A107": SS_Print.ActiveSheet.Cells[15, 16].Text = "우 : " + strData.Trim(); break;

                        case "TH12":
                        case "TH22":
                            if (bPolice = true)
                            {
                                SS_Print.ActiveSheet.Cells[14, 16].Text = "좌 : " + strData.Trim() + "dB";
                                SS_Print.ActiveSheet.Cells[15, 16].Text = "우 : " + strData.Trim() + "dB";
                            }
                            break;

                        case "TW07": SS_Print.ActiveSheet.Cells[16, 4].Text = strData.Trim(); break;
                        case "TZ23": SS_Print.ActiveSheet.Cells[16, 14].Text = strData.Trim(); break;
                        case "TZ24": SS_Print.ActiveSheet.Cells[17, 4].Text = strData.Trim(); break;
                        case "TW08": SS_Print.ActiveSheet.Cells[17, 14].Text = strData.Trim(); break;
                        case "TW09": SS_Print.ActiveSheet.Cells[18, 4].Text = strData.Trim(); break;
                        case "TW10": SS_Print.ActiveSheet.Cells[18, 14].Text = strData.Trim(); break;
                        case "TW11": SS_Print.ActiveSheet.Cells[19, 4].Text = strData.Trim(); break;
                        case "TW12": SS_Print.ActiveSheet.Cells[19, 14].Text = strData.Trim(); break;
                        case "TZ25": SS_Print.ActiveSheet.Cells[20, 4].Text = strData.Trim(); break;
                        case "TZ26": SS_Print.ActiveSheet.Cells[20, 14].Text = strData.Trim(); break;
                        case "TW13": SS_Print.ActiveSheet.Cells[21, 4].Text = strData.Trim(); break;
                        case "TZ18": SS_Print.ActiveSheet.Cells[21, 14].Text = strData.Trim(); break;
                        case "TZ27": SS_Print.ActiveSheet.Cells[22, 4].Text = strData.Trim(); break;

                        case "A142":
                            switch (strData)
                            {
                                case "01": SS_Print.ActiveSheet.Cells[22, 14].Text = "정상"; break;
                                case "02": SS_Print.ActiveSheet.Cells[22, 14].Text = "사진불량"; break;
                                case "03": SS_Print.ActiveSheet.Cells[22, 14].Text = "비활동성(정상)"; break;
                                case "07": SS_Print.ActiveSheet.Cells[22, 14].Text = "폐결핵 의증"; break;
                                case "08": SS_Print.ActiveSheet.Cells[22, 14].Text = "비결핵성 질환"; break;
                                case "09": SS_Print.ActiveSheet.Cells[22, 14].Text = "순환기계 질환"; break;
                                case "10": SS_Print.ActiveSheet.Cells[22, 14].Text = "진단미정"; break;
                                case "11": SS_Print.ActiveSheet.Cells[22, 14].Text = "미촬영"; break;
                                case "12": SS_Print.ActiveSheet.Cells[22, 14].Text = "유질환자"; break;
                                case "13": SS_Print.ActiveSheet.Cells[22, 14].Text = "비활동성 폐결핵"; break;
                                default:
                                    break;
                            }
                            break;

                        case "LU39": SS_Print.ActiveSheet.Cells[23, 3].Text = "매독반응 : " + strData.Trim(); break;

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

                        case "YA01": strRemark = strData.Trim(); break;
                        case "YA02": strRemark1 = strData.Trim(); break;
                        default:
                            break;
                    }
                }
            }

            //기타소견
            if (strRemark1 != ".")
            {
                SS_Print.ActiveSheet.Cells[23, 3].Text += " / " + strRemark1;
            }

            if (!strHH.IsNullOrEmpty() || !strHL.IsNullOrEmpty())
            {
                SS_Print.ActiveSheet.Cells[13, 14].Text = strHH + "  /  " + strHL;
            }

            SS_Print.ActiveSheet.Cells[27, 11].Text = VB.Left(strJepDate, 4) + "  년  " + VB.Mid(strJepDate, 6, 2) + "  일  " + VB.Right(strJepDate, 2) + "  일";


            SS_Print.ActiveSheet.Cells[29, 14].Text = hb.READ_License_DrName(nDrno) + " (" + nDrno + ")";

            HIC_SPC_PANJENG item3 = hicSpcPanjengService.GetAllbyWrtNo(nWrtno);

            if (!item3.IsNullOrEmpty())
            {
                strPanjeng = item3.PANJENG;
                strJochi = item3.JOCHICODE;
                strSogen = item3.SOGENCODE;

                if (strPanjeng == "1" || strPanjeng == "2" || strPanjeng == "3" || strPanjeng == "4")
                {
                    SS_Print.ActiveSheet.Cells[33, 5].Text = "√";
                    SS_Print.ActiveSheet.Cells[33, 12].Text = "'신체검사 불합격 판정기준'에 해당하지 않음";
                }
                else if (strPanjeng == "5" || strPanjeng == "6" || strPanjeng == "7" || strPanjeng == "8")
                {
                    SS_Print.ActiveSheet.Cells[37, 5].Text = "√";
                    SS_Print.ActiveSheet.Cells[40, 5].Text = strRemark;
                }


                //판정일자
                SS_Print.ActiveSheet.Cells[29, 14].Text = VB.Left(strPandate, 4) + "  년  " + VB.Mid(strPandate, 6, 2) + "  월  " + VB.Right(strPandate, 2) + "  일";
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
            cSpd.setSpdPrint(SS_Print, PrePrint, setMargin, setOption, strHeader, strFooter);


            //검사결과 상세내역 인쇄
            strHeader = cSpd.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strHeader += cSpd.setSpdPrint_String(VB.Space(10) + "접수번호(" + nWrtno + ") " + nHJRSP.SNAME + " 검진결과", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += cSpd.setSpdPrint_String("검진번호: " + nHJRSP.PANO + "▶성명: " + nHJRSP.SNAME + "(" + nHJRSP.AGE + ")    ▶검진일자: " + nHJRSP.JEPDATE + VB.Space(10) + "【 수검자용 】", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += cSpd.setSpdPrint_String("회 사 명: " + nHJRSP.LTDCODE + "   ▶종류:" + nHJRSP.GJJONG + VB.Space(5) + "인쇄시각: " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = cSpd.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false, 0.7f);
            cSpd.setSpdPrint(SS2, PrePrint, setMargin, setOption, strHeader, strFooter);
        }
        private void Result_Print_Sub2()
        {
            long nWrtno = 0;
            long nCNT = 0;
            long nREAD = 0;
            long nResult = 0;
            long nDrno = 0;
            bool bPolice = false;

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
            string strRemark1 = "";
            string strGbSpc = "";

            //성명
            SS_Print2.ActiveSheet.Cells[7, 12].Text = nHJRSP.SNAME;

            //주민등록번호
            if (fstrJunminAll == "Y")
            {

            }
            else
            {
                SS_Print2.ActiveSheet.Cells[8, 12].Text = VB.Left(nHJRSP.JUMIN, 6) + "-" + VB.Right(nHJRSP.JUMIN, 7);
            }

            for (int i = 0; i <= SS2.ActiveSheet.RowCount - 1; i++)
            {
                strData = SS2.ActiveSheet.Cells[i, 2].Text;
                if (!strData.IsNullOrEmpty())
                {
                    strCode = SS2.ActiveSheet.Cells[i, 0].Text;
                    switch (strCode)
                    {
                        case "A101": SS_Print2.ActiveSheet.Cells[12, 4].Text = strData.Trim(); break;           //신장
                        case "A102": SS_Print2.ActiveSheet.Cells[12, 14].Text = strData.Trim(); break;           //체중

                        case "A115": SS_Print2.ActiveSheet.Cells[13, 4].Text = strData.Trim(); break;            //허리둘레

                        case "A108": strHH = strData.Trim(); break;         //혈압 고
                        case "A109": strHL = strData.Trim(); break;         //혈압 저

                        case "A104": SS_Print2.ActiveSheet.Cells[14, 3].Text = "좌 : " + strData.Trim(); break;
                        case "A105": SS_Print2.ActiveSheet.Cells[15, 3].Text = "우 : " + strData.Trim(); break;
                        case "TE05": SS_Print2.ActiveSheet.Cells[14, 11].Text = strData.Trim(); break;
                        case "A106": SS_Print2.ActiveSheet.Cells[14, 16].Text = "좌 : " + strData.Trim(); break;
                        case "A107": SS_Print2.ActiveSheet.Cells[15, 16].Text = "우 : " + strData.Trim(); break;

                        case "TH12":
                        case "TH22":
                            SS_Print2.ActiveSheet.Cells[14, 16].Text = "좌 : " + strData.Trim() + "dB";
                            SS_Print2.ActiveSheet.Cells[15, 16].Text = "우 : " + strData.Trim() + "dB";
                            break;
                        case "TW07": SS_Print2.ActiveSheet.Cells[16, 4].Text = strData.Trim(); break;
                        case "TZ23": SS_Print2.ActiveSheet.Cells[16, 14].Text = strData.Trim(); break;
                        case "TZ24": SS_Print2.ActiveSheet.Cells[17, 4].Text = strData.Trim(); break;
                        case "TW08": SS_Print2.ActiveSheet.Cells[17, 14].Text = strData.Trim(); break;
                        case "TW09": SS_Print2.ActiveSheet.Cells[18, 4].Text = strData.Trim(); break;
                        case "TW10": SS_Print2.ActiveSheet.Cells[18, 14].Text = strData.Trim(); break;
                        case "TW11": SS_Print2.ActiveSheet.Cells[19, 4].Text = strData.Trim(); break;
                        case "TW12": SS_Print2.ActiveSheet.Cells[19, 14].Text = strData.Trim(); break;
                        case "TZ25": SS_Print2.ActiveSheet.Cells[20, 4].Text = strData.Trim(); break;
                        case "TZ26": SS_Print2.ActiveSheet.Cells[20, 14].Text = strData.Trim(); break;
                        case "TW13": SS_Print2.ActiveSheet.Cells[21, 4].Text = strData.Trim(); break;
                        case "TZ18": SS_Print2.ActiveSheet.Cells[21, 14].Text = strData.Trim(); break;
                        case "TZ27": SS_Print2.ActiveSheet.Cells[22, 4].Text = strData.Trim(); break;

                        case "A142":
                            switch (strData)
                            {
                                case "01": SS_Print2.ActiveSheet.Cells[22, 14].Text = "정상"; break;
                                case "02": SS_Print2.ActiveSheet.Cells[22, 14].Text = "사진불량"; break;
                                case "03": SS_Print2.ActiveSheet.Cells[22, 14].Text = "비활동성(정상)"; break;
                                case "07": SS_Print2.ActiveSheet.Cells[22, 14].Text = "폐결핵 의증"; break;
                                case "08": SS_Print2.ActiveSheet.Cells[22, 14].Text = "비결핵성 질환"; break;
                                case "09": SS_Print2.ActiveSheet.Cells[22, 14].Text = "순환기계 질환"; break;
                                case "10": SS_Print2.ActiveSheet.Cells[22, 14].Text = "진단미정"; break;
                                case "11": SS_Print2.ActiveSheet.Cells[22, 14].Text = "미촬영"; break;
                                case "12": SS_Print2.ActiveSheet.Cells[22, 14].Text = "유질환자"; break;
                                case "13": SS_Print2.ActiveSheet.Cells[22, 14].Text = "비활동성 폐결핵"; break;
                                default:
                                    break;
                            }
                            break;

                        //경찰공무원

                        case "TW17": SS_Print2.ActiveSheet.Cells[23, 4].Text = strData.Trim(); break;
                        case "TW19": SS_Print2.ActiveSheet.Cells[23, 14].Text = strData.Trim(); break;
                        case "TW18":SS_Print2.ActiveSheet.Cells[24, 4].Text = strData.Trim(); break;
                        case "E923":SS_Print2.ActiveSheet.Cells[24, 14].Text = strData.Trim(); break;

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

                        case "YA01": strRemark = strData.Trim(); break;
                        case "YA02": strRemark1 = strData.Trim(); break;
                        default:
                            break;
                    }
                }
            }




            if (!strHH.IsNullOrEmpty() || !strHL.IsNullOrEmpty())
            {
                SS_Print2.ActiveSheet.Cells[13, 14].Text = strHH + "  /  " + strHL;
            }

            SS_Print2.ActiveSheet.Cells[27, 11].Text = VB.Left(strJepDate, 4) + "  년  " + VB.Mid(strJepDate, 6, 2) + "  일  " + VB.Right(strJepDate, 2) + "  일";


            SS_Print2.ActiveSheet.Cells[29, 14].Text = hb.READ_License_DrName(nDrno) + " (" + nDrno + ")";

            HIC_SPC_PANJENG item3 = hicSpcPanjengService.GetAllbyWrtNo(nWrtno);

            if (!item3.IsNullOrEmpty())
            {
                strPanjeng = item3.PANJENG;
                strJochi = item3.JOCHICODE;
                strSogen = item3.SOGENCODE;

                if (strPanjeng == "1" || strPanjeng == "2" || strPanjeng == "3" || strPanjeng == "4")
                {
                    SS_Print2.ActiveSheet.Cells[33, 5].Text = "√";
                    SS_Print2.ActiveSheet.Cells[33, 12].Text = "'신체검사 불합격 판정기준'에 해당하지 않음";
                }
                else if (strPanjeng == "5" || strPanjeng == "6" || strPanjeng == "7" || strPanjeng == "8")
                {
                    SS_Print2.ActiveSheet.Cells[37, 5].Text = "√";
                    SS_Print2.ActiveSheet.Cells[40, 5].Text = strRemark;
                }


                //판정일자
                SS_Print2.ActiveSheet.Cells[29, 14].Text = VB.Left(strPandate, 4) + "  년  " + VB.Mid(strPandate, 6, 2) + "  월  " + VB.Right(strPandate, 2) + "  일";
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
            cSpd.setSpdPrint(SS_Print2, PrePrint, setMargin, setOption, strHeader, strFooter);


            //검사결과 상세내역 인쇄
            strHeader = cSpd.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strHeader += cSpd.setSpdPrint_String(VB.Space(10) + "접수번호(" + nWrtno + ") " + nHJRSP.SNAME + " 검진결과", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += cSpd.setSpdPrint_String("검진번호: " + nHJRSP.PANO + "▶성명: " + nHJRSP.SNAME + "(" + nHJRSP.AGE + ")    ▶검진일자: " + nHJRSP.JEPDATE + VB.Space(10) + "【 수검자용 】", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += cSpd.setSpdPrint_String("회 사 명: " + nHJRSP.LTDCODE + "   ▶종류:" + nHJRSP.GJJONG + VB.Space(5) + "인쇄시각: " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = cSpd.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false, 0.7f);
            cSpd.setSpdPrint(SS2, PrePrint, setMargin, setOption, strHeader, strFooter);

        }
        private void Result_Print_Sub3()
        {
            //소방공무원
            long nWrtno = 0;
            long nCNT = 0;
            long nREAD = 0;
            long nResult = 0;
            long nDrno = 0;
            bool bPolice = false;

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
            string strRemark1 = "";
            string strGbSpc = "";

            //성명
            SS_Print3.ActiveSheet.Cells[7, 12].Text = nHJRSP.SNAME;

            //한자성명
            SS_Print3.ActiveSheet.Cells[8, 12].Text = nHJRSP.HNAME;

            //주민등록번호
            if (fstrJunminAll == "Y")
            {

            }
            else
            {
                SS_Print3.ActiveSheet.Cells[8, 12].Text = VB.Left(nHJRSP.JUMIN, 6) + "-" + VB.Right(nHJRSP.JUMIN, 7);
            }

            for (int i = 0; i <= SS2.ActiveSheet.RowCount - 1; i++)
            {
                strData = SS2.ActiveSheet.Cells[i, 2].Text;
                if (!strData.IsNullOrEmpty())
                {
                    strCode = SS2.ActiveSheet.Cells[i, 0].Text;
                    switch (strCode)
                    {
                        case "A101": SS_Print3.ActiveSheet.Cells[12, 3].Text = strData.Trim(); break;           //신장
                        case "A102": SS_Print3.ActiveSheet.Cells[12, 13].Text = strData.Trim(); break;           //체중

                        case "ZD04": SS_Print3.ActiveSheet.Cells[13, 4].Text = strData.Trim(); break;            //흉부

                        case "A108": strHH = strData.Trim(); break;         //혈압 고
                        case "A109": strHL = strData.Trim(); break;         //혈압 저

                        case "A104": SS_Print3.ActiveSheet.Cells[14, 3].Text = "좌 : " + strData.Trim(); break;
                        case "A105": SS_Print3.ActiveSheet.Cells[15, 3].Text = "우 : " + strData.Trim(); break;
                        case "TE05": SS_Print3.ActiveSheet.Cells[14, 11].Text = strData.Trim(); break;
                        case "A106": SS_Print3.ActiveSheet.Cells[14, 16].Text = "좌 : " + strData.Trim(); break;
                        case "A107": SS_Print3.ActiveSheet.Cells[15, 16].Text = "우 : " + strData.Trim(); break;

                        case "TH12":
                        case "TH22":
                            SS_Print3.ActiveSheet.Cells[14, 16].Text = "좌 : " + strData.Trim() + "dB";
                            SS_Print3.ActiveSheet.Cells[15, 16].Text = "우 : " + strData.Trim() + "dB";
                            break;

                        case "TZ18": SS_Print3.ActiveSheet.Cells[16, 4].Text = strData.Trim(); break;
                        case "TZ23": SS_Print3.ActiveSheet.Cells[16, 14].Text = strData.Trim(); break;
                        case "ZD00": SS_Print3.ActiveSheet.Cells[17, 4].Text = strData.Trim(); break;
                        case "TZ24": SS_Print3.ActiveSheet.Cells[17, 14].Text = strData.Trim(); break;
                        case "TZ19": SS_Print3.ActiveSheet.Cells[18, 4].Text = strData.Trim(); break;
                        case "TZ25": SS_Print3.ActiveSheet.Cells[18, 14].Text = strData.Trim(); break;
                        case "TZ20": SS_Print3.ActiveSheet.Cells[19, 4].Text = strData.Trim(); break;
                        case "TZ26": SS_Print3.ActiveSheet.Cells[19, 14].Text = strData.Trim(); break;
                        case "TZ21": SS_Print3.ActiveSheet.Cells[20, 4].Text = strData.Trim(); break;
                        case "TZ27": SS_Print3.ActiveSheet.Cells[20, 14].Text = strData.Trim(); break;

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


                        case "LU39": SS_Print3.ActiveSheet.Cells[22, 14].Text = strData.Trim(); break;

                        case "A142":
                            switch (strData)
                            {
                                case "01": SS_Print3.ActiveSheet.Cells[22, 4].Text = "정상"; break;
                                case "02": SS_Print3.ActiveSheet.Cells[22, 4].Text = "사진불량"; break;
                                case "03": SS_Print3.ActiveSheet.Cells[22, 4].Text = "비활동성(정상)"; break;
                                case "07": SS_Print3.ActiveSheet.Cells[22, 4].Text = "폐결핵 의증"; break;
                                case "08": SS_Print3.ActiveSheet.Cells[22, 4].Text = "비결핵성 질환"; break;
                                case "09": SS_Print3.ActiveSheet.Cells[22, 4].Text = "순환기계 질환"; break;
                                case "10": SS_Print3.ActiveSheet.Cells[22, 4].Text = "진단미정"; break;
                                case "11": SS_Print3.ActiveSheet.Cells[22, 4].Text = "미촬영"; break;
                                case "12": SS_Print3.ActiveSheet.Cells[22, 4].Text = "유질환자"; break;
                                case "13": SS_Print3.ActiveSheet.Cells[22, 4].Text = "비활동성 폐결핵"; break;
                                default:
                                    break;
                            }
                            break;

                        case "TZ22": SS_Print3.ActiveSheet.Cells[21, 4].Text = strData.Trim(); break;
                        case "TZ30": SS_Print3.ActiveSheet.Cells[22, 14].Text = "정형외과: " + strData.Trim(); break;
                        case "H840": strABO = strData.Trim(); break;
                        case "H841": strRh= strData.Trim(); break;
                        case "E923": strE923 = strData.Trim(); break;

                        default:
                        break;
                    }
                }
            }




            if (!strHH.IsNullOrEmpty() || !strHL.IsNullOrEmpty())
            {
                SS_Print3.ActiveSheet.Cells[13, 14].Text = strHH + "  /  " + strHL;
            }

            strJepDate = nHJRSP.JEPDATE;
            SS_Print3.ActiveSheet.Cells[27, 11].Text = VB.Left(strJepDate, 4) + "  년  " + VB.Mid(strJepDate, 6, 2) + "  일  " + VB.Right(strJepDate, 2) + "  일";


            SS_Print3.ActiveSheet.Cells[29, 14].Text = hb.READ_License_DrName(nDrno) + " (" + nDrno + ")";

            HIC_SPC_PANJENG item3 = hicSpcPanjengService.GetAllbyWrtNo(nWrtno);

            if (!item3.IsNullOrEmpty())
            {
                strPanjeng = item3.PANJENG;
                strJochi = item3.JOCHICODE;
                strSogen = item3.SOGENCODE;

                if (strPanjeng == "1" || strPanjeng == "2" || strPanjeng == "3" || strPanjeng == "4")
                {
                    SS_Print3.ActiveSheet.Cells[33, 5].Text = "√";
                }
                else if (strPanjeng == "5" || strPanjeng == "6" )
                {
                    SS_Print3.ActiveSheet.Cells[35, 5].Text = "√";
                }
                else if (strPanjeng == "7" || strPanjeng == "8")
                {
                    SS_Print3.ActiveSheet.Cells[37, 5].Text = "√";
                }


                if (strPanjeng == "5" || strPanjeng == "6")
                {
                    SS_Print3.ActiveSheet.Cells[32, 13].Text = "정밀검사요함";
                }
                else
                {
                    SS_Print3.ActiveSheet.Cells[32, 13].Text = "없음";
                }


                //환자유의사항
                if (strPanjeng == "7" || strPanjeng == "8")
                {
                    if(!item3.SOGENREMARK.IsNullOrEmpty())
                    {
                        SS_Print3.ActiveSheet.Cells[40, 5].Text = item3.SOGENREMARK;
                    }
                    else
                    {
                        SS_Print3.ActiveSheet.Cells[40, 5].Text = "없음";
                    }
                    
                }

                //판정일자
                SS_Print3.ActiveSheet.Cells[46, 11].Text = VB.Left(strPandate, 4) + "  년  " + VB.Mid(strPandate, 6, 2) + "  월  " + VB.Right(strPandate, 2) + "  일";
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
            cSpd.setSpdPrint(SS_Print3, PrePrint, setMargin, setOption, strHeader, strFooter);


            //검사결과 상세내역 인쇄
            strHeader = cSpd.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strHeader += cSpd.setSpdPrint_String(VB.Space(10) + "접수번호(" + nWrtno + ") " + nHJRSP.SNAME + " 검진결과", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += cSpd.setSpdPrint_String("검진번호: " + nHJRSP.PANO + "▶성명: " + nHJRSP.SNAME + "(" + nHJRSP.AGE + ")    ▶검진일자: " + nHJRSP.JEPDATE + VB.Space(10) + "【 수검자용 】", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += cSpd.setSpdPrint_String("회 사 명: " + nHJRSP.LTDCODE + "   ▶종류:" + nHJRSP.GJJONG + VB.Space(5) + "인쇄시각: " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = cSpd.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false, 0.7f);
            cSpd.setSpdPrint(SS2, PrePrint, setMargin, setOption, strHeader, strFooter);
        }
    }
}

