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


/// <summary>
/// Class Name      : HC_Print
/// File Name       : frmHcPrint_Xray_Sub.cs
/// Description     : 방사선 결과지 인쇄
/// Author          : 김경동
/// Create Date     : 2021-06-29
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm방사선종사결과_new.frm(Frm방사선종사결과_new)" />

namespace HC_Print
{
    public partial class frmHcPrint_Xray_Sub : Form
    {

        long fnWrtno = 0;
        long fnDrno = 0;

        string fstrGubun = "";
        string fstrBogunso = "";
        string fstrJong = "";

        clsHcPrint cHPrt = new clsHcPrint();
        ComFunc cf = new ComFunc();
        clsHaBase hb = new clsHaBase();
        clsSpread cSpd = new clsSpread();

        HicXMunjinJepsuPatientService hicXMunjinJepsuPatientService = null;
        HicSunapdtlService hicSunapdtlService = null;
        HicResultService hicResultService = null;
        HicResultExCodeService hicResultExCodeService = null;
        HicXMunjinService hicXMunjinService = null;
        HicJepsuService hicJepsuService = null;

        public frmHcPrint_Xray_Sub()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }

        public frmHcPrint_Xray_Sub(long argWRTNO, string argGubun)
        {
            InitializeComponent();
            SetControl();
            SetEvent();

            fnWrtno = argWRTNO;
            fstrGubun = argGubun;
        }

        private void SetControl()
        {
            hicXMunjinJepsuPatientService = new HicXMunjinJepsuPatientService();
            hicSunapdtlService = new HicSunapdtlService();
            hicResultService = new HicResultService();
            hicResultExCodeService = new HicResultExCodeService();
            hicXMunjinService = new HicXMunjinService();
            hicJepsuService = new HicJepsuService();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormload);
        }

        private void eFormload(object sender, EventArgs e)
        {
            this.Hide();

            Result_Print_Clear();       //Sheet Clear
            if(fstrGubun == "Y")
            {
                //신규양식
                Result_Print_Sub2();
            }
            else
            {
                //기존양식
                Result_Print_Sub();
            }

            Result_Print_Main();

            if (fstrJong == "51")
            {
                Result_Print_Main_1();
            }
            else if (fstrJong == "50")
            {
                Result_Print_Main_2();
            }

            ComFunc.Delay(1500);
            this.Close();

        }

        private void Result_Print_Clear()
        {
            //SS1
            //성명, 성별, 나이
            SS1.ActiveSheet.Cells[3, 3].Text = "";
            SS1.ActiveSheet.Cells[3, 6].Text = "";
            SS1.ActiveSheet.Cells[3, 8].Text = "";
            //주민등록번호
            SS1.ActiveSheet.Cells[4, 3].Text = "";
            //과거질병력, 가족력, 최근특이증상, 방사선관련특이사항
            SS1.ActiveSheet.Cells[6, 2].Text = " ■ 과거질병력 : ";
            SS1.ActiveSheet.Cells[7, 2].Text = " ■ 가족력 : ";
            SS1.ActiveSheet.Cells[8, 2].Text = " ■ 최근 특이증상 (※급격한 시력저하 등) : ";
            SS1.ActiveSheet.Cells[9, 2].Text = " ■ 방사선 작업과 관련된 기타 특이사항 (※해당 경우에 작성) : ";

            //백혈구, 혈소판, 혈색소    
            SS1.ActiveSheet.Cells[10, 3].Text = "";
            SS1.ActiveSheet.Cells[11, 3].Text = "";
            SS1.ActiveSheet.Cells[12, 3].Text = "";
            //판정소견
            SS1.ActiveSheet.Cells[13, 3].Text = "";
            //채혈일
            SS1.ActiveSheet.Cells[14, 3].Text = "";
            //
            SS1.ActiveSheet.Cells[18, 6].Text = "";
            SS1.ActiveSheet.Cells[19, 6].Text = "";

            //SS2
            //성명
            SS2.ActiveSheet.Cells[3, 3].Text = "";
            //주민등록번호
            SS2.ActiveSheet.Cells[4, 3].Text = "";
            //과거질병력, 가족력, 최근특이증상, 방사선관련특이사항
            SS2.ActiveSheet.Cells[6, 2].Text = " ■ 최근 특이증상 (※급격한 시력저하 등) : ";
            SS2.ActiveSheet.Cells[8, 2].Text = " ■ 방사선 작업과 관련된 기타 특이사항 (※해당 경우에 작성) : ";

            //백혈구, 혈소판, 혈색소    
            SS2.ActiveSheet.Cells[10, 3].Text = "";
            SS2.ActiveSheet.Cells[11, 3].Text = "";
            SS2.ActiveSheet.Cells[12, 3].Text = "";
            //판정소견
            SS2.ActiveSheet.Cells[13, 3].Text = "";
            //채혈일
            SS2.ActiveSheet.Cells[14, 3].Text = "";
            //
            SS2.ActiveSheet.Cells[18, 6].Text = "";
            SS2.ActiveSheet.Cells[19, 6].Text = "";

            //SS3
            SS2.ActiveSheet.Cells[3, 3].Text = "";
            SS2.ActiveSheet.Cells[3, 7].Text = "";

            SS2.ActiveSheet.Cells[4, 3].Text = "";
            SS2.ActiveSheet.Cells[4, 7].Text = "";

            SS2.ActiveSheet.Cells[5, 3].Text = "";
            SS2.ActiveSheet.Cells[5, 7].Text = "";

            //검사결과치
            for (int i = 8; i < 28; i++)
            {
                SS2.ActiveSheet.Cells[i, 4].Text = "";
                SS2.ActiveSheet.Cells[i, 6].Text = "";
            }

            //소견, 조치
            SS2.ActiveSheet.Cells[28, 4].Text = "";
            SS2.ActiveSheet.Cells[29, 4].Text = "";
            //검진일자 및 판정의사
            SS2.ActiveSheet.Cells[32, 4].Text = "";
            SS2.ActiveSheet.Cells[34, 4].Text = "";
            SS2.ActiveSheet.Cells[34, 7].Text = "";
        }

        private void Result_Print_Sub()
        {
            string[] strExcode = new string[] { "A121", "A282", "A283", "H805" };

            string strJumin = "";
            string strLtdCode = "";
            string strJepDate = "";
            string strTemp1 = "";
            string strJikJong = "";
            string strJilByung = "";


            HIC_X_MUNJIN_JEPSU_PATIENT item = hicXMunjinJepsuPatientService.GetAllItemsByWrtno(fnWrtno);
            if(!item.IsNullOrEmpty())
            {
                fstrJong = item.GJJONG;
                strLtdCode = item.LTDCODE;
                strJepDate = item.JEPDATE;
                strJumin = clsAES.DeAES(item.JUMIN2);

                fstrBogunso = "";
                if (fstrJong == "51")
                {
                    if( hicSunapdtlService.GetCountbyWrtNoCode(fnWrtno, "5106")> 0)
                    {
                        fstrBogunso = "Y";
                    }
                }

                
                //보건소제출용
                if(fstrJong == "51" && fstrBogunso == "Y")
                {
                    SS4.ActiveSheet.Cells[3, 2].Text = cf.Read_Ltd_Name(clsDB.DbCon, item.LTDCODE);

                    strJikJong = "";
                    SS4.ActiveSheet.Cells[3, 6].Text = "";
                    if (item.JIKJONG1 == "1") { strJikJong += "비파괴검사,"; }
                    if (item.JIKJONG2 == "1") { strJikJong += "방사선사,"; }
                    if (!item.JIKJONG3.IsNullOrEmpty()) { strJikJong += item.JIKJONG3.Trim(); }
                    if (strJikJong == "") { strJikJong = "방사선사"; }
                    if (VB.Right(strJikJong, 1) == ","){ strJikJong = VB.Left(strJikJong, VB.Len(strJikJong) - 1); }
                    SS4.ActiveSheet.Cells[3, 6].Text = strJikJong;

                    SS4.ActiveSheet.Cells[4, 2].Text = item.SNAME.Trim();
                    SS4.ActiveSheet.Cells[4, 6].Text = VB.Left(strJumin, 6) + "-" + VB.Mid(strJumin, 7, 1) + "******";
                    SS4.ActiveSheet.Cells[5, 2].Text = item.JEPDATE.Trim();
                    SS4.ActiveSheet.Cells[5, 6].Text = item.IPSADATE.Trim();


                    //문진사항
                    SS4.ActiveSheet.Cells[8, 8].Text = VB.IIf(item.XP1 == "Y", "유", "무").ToString();
                    SS4.ActiveSheet.Cells[9, 8].Text = "무";
                    if(item.XP1 == "Y")
                    {
                        if (!item.XPJONG.IsNullOrEmpty() && !item.XPLACE.IsNullOrEmpty())
                        {
                            SS4.ActiveSheet.Cells[9, 8].Text = "유";
                        }
                    }
                    SS4.ActiveSheet.Cells[10, 8].Text = "없음";
                    if (!item.XJUNGSAN.IsNullOrEmpty()) { SS4.ActiveSheet.Cells[10, 8].Text = item.XJUNGSAN; }

                    //검사항목

                    
                    List<HIC_RESULT> list = hicResultService.GetExCodebyWrtNo_All(fnWrtno, strExcode);

                    if(!list.IsNullOrEmpty())
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            switch (list[i].EXCODE)
                            {
                                case "A121":
                                    SS4.ActiveSheet.Cells[13, 7].Text = list[i].RESULT.Trim();
                                    break;
                                case "A283":
                                    SS4.ActiveSheet.Cells[14, 7].Text = list[i].RESULT.Trim();
                                    break;
                                case "A282":
                                    SS4.ActiveSheet.Cells[15, 7].Text = list[i].RESULT.Trim();
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    SS4.ActiveSheet.Cells[16, 7].Text = "";
                    //판독일자
                    SS4.ActiveSheet.Cells[21, 1].Text = VB.Mid(item.PANJENGDATE, 1, 4) + "년 " + VB.Mid(item.PANJENGDATE, 6, 2) + "월 " + VB.Mid(item.PANJENGDATE, 9, 2) + "일";
                    //판정의사명, 면허번호
                    SS4.ActiveSheet.Cells[23, 1].Text = "판정의사명: " + hb.READ_License_DrName(item.PANJENGDRNO);
                    SS4.ActiveSheet.Cells[24, 1].Text = "면 허 번 호 : " + item.PANJENGDRNO;
                    fnDrno = item.PANJENGDRNO;
                }
                else if (fstrJong == "51")
                {

                    SS1.ActiveSheet.Cells[3, 3].Text = item.SNAME.Trim();
                    SS1.ActiveSheet.Cells[3, 6].Text = VB.IIf(item.SEX == "M", "남", "여").ToString();
                    SS1.ActiveSheet.Cells[3, 8].Text = item.AGE.Trim();

                    SS1.ActiveSheet.Cells[4, 3].Text = " " + VB.Left(strJumin, 6) + "-" + VB.Mid(strJumin, 7, 1) + "******";
                    SS1.ActiveSheet.Cells[4, 6].Text = " " + cf.Read_Ltd_Name(clsDB.DbCon, item.LTDCODE);

                    strJilByung = "";
                    if (item.JILBYUNG =="1")
                    {
                        if(item.BLOOD.Trim() == "1") { strJilByung += "빈혈,"; }
                        if (item.BLOOD2.Trim() == "1") { strJilByung += "백혈병,"; }
                        if (!item.BLOOD3.IsNullOrEmpty()) { strJilByung += "기타: " + item.BLOOD3.Trim()+ ","; }
                        if (item.SKIN1.Trim() == "1") { strJilByung += "아토피,"; }
                        if (item.SKIN2.Trim() == "1") { strJilByung += "습진,"; }
                        if (!item.SKIN3.IsNullOrEmpty()) { strJilByung += "기타: " + item.SKIN3.Trim()+ ","; }
                        if (!item.NERVOUS1.IsNullOrEmpty()) { strJilByung += "신경계질환: " + item.NERVOUS1.Trim()+","; }
                        if (item.EYE1.Trim() == "1") { strJilByung += "백내장,"; }
                        if (!item.EYE2.IsNullOrEmpty()) { strJilByung += "기타: "+ item.EYE2.Trim() + ","; }
                        if (!item.CANCER1.IsNullOrEmpty()) { strJilByung += "암: "+ item.CANCER1.Trim() + ","; }
                        if (VB.Right(strJilByung, 1) == ",") { strJilByung = VB.Left(strJilByung, VB.Len(strJilByung) - 1); }
                        SS1.ActiveSheet.Cells[6, 2].Text = strJilByung;
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[6, 2].Text = "없 음";
                    }
                    
                    string strGajok = "";
                    strGajok = "";
                    if(item.GAJOK =="1")
                    {
                        if (!item.BLOOD.IsNullOrEmpty()) { strGajok +=  item.BLOOD.Trim() + "," + ComNum.VBLF; }
                        if (!item.NERVOUS2.IsNullOrEmpty()) { strGajok += item.NERVOUS2.Trim() + "," + ComNum.VBLF; }
                        if (!item.CANCER2.IsNullOrEmpty()) { strGajok +=  item.CANCER2.Trim() + "," + ComNum.VBLF; }
                        if (VB.Right(strGajok, 1) == ",") { strGajok = VB.Left(strGajok, VB.Len(strGajok) - 1); }

                        SS1.ActiveSheet.Cells[7, 2].Text = strGajok;
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[7, 2].Text = "없 음";
                    }
                    
                    //최근특이사항
                    if(!item.SYMPTON.IsNullOrEmpty())
                    {
                        SS1.ActiveSheet.Cells[8, 2].Text ="  "+ item.SYMPTON.Trim() + "," + ComNum.VBLF;
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[8, 2].Text = "없 음";
                    }

                    //방사선작업과관련된증상
                    if (!item.JIKJONG1.IsNullOrEmpty() || !item.JIKJONG2.IsNullOrEmpty() || !item.JIKJONG3.IsNullOrEmpty())
                        {
                        if (item.JIKJONG1 == "1") { strJikJong += "비파괴검사,"; }
                        if (item.JIKJONG2 == "1") { strJikJong += "방사선사,"; }
                        if (!item.JIKJONG3.IsNullOrEmpty()) { strJikJong += item.JIKJONG3.Trim(); }
                        if (VB.Right(strJikJong, 1) == ",") { strJikJong = VB.Left(strJikJong, VB.Len(strJikJong) - 1); }
                        SS1.ActiveSheet.Cells[9, 2].Text = strJikJong;
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[9, 2].Text = "없 음";
                    }

                    //검사결과 인쇄
                    List<HIC_RESULT> list = hicResultService.GetExCodebyWrtNo_All(fnWrtno, strExcode);

                    if (!list.IsNullOrEmpty())
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            switch (list[i].EXCODE)
                            {
                                case "A121":    //혈색소
                                    SS1.ActiveSheet.Cells[12, 3].Text = list[i].RESULT.Trim();
                                    break;
                                case "A282":    //백혈구
                                    SS1.ActiveSheet.Cells[10, 3].Text = list[i].RESULT.Trim();
                                    break;
                                case "H805":    //혈소판
                                    SS1.ActiveSheet.Cells[11, 3].Text = list[i].RESULT.Trim();
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    //조치 및 종합소견
                    strTemp1 = "";
                    strTemp1 = item.SOGEN.Trim() + "/" + item.PANJENG.Trim();
                    SS1.ActiveSheet.Cells[13, 3].Text = strTemp1;

                    //채혈일자
                    SS1.ActiveSheet.Cells[14, 3].Text = item.JEPDATE;

                    //판정의사명, 면허번호
                    SS1.ActiveSheet.Cells[18, 6].Text = "면 허 번 호 : " + item.PANJENGDRNO + VB.Space(14);
                    SS1.ActiveSheet.Cells[19, 6].Text = "판정의사명: " + hb.READ_License_DrName(item.PANJENGDRNO);
                    fnDrno = item.PANJENGDRNO;
                }
                else
                {


                    SS3.ActiveSheet.Cells[3, 3].Text = cf.Read_Ltd_Name(clsDB.DbCon, item.LTDCODE);
                    SS3.ActiveSheet.Cells[3, 7].Text = item.XTERM1.Trim();
                    SS3.ActiveSheet.Cells[4, 3].Text = item.SNAME.Trim();
                    SS3.ActiveSheet.Cells[4, 7].Text = VB.Left(strJumin, 6) + "-" + VB.Mid(strJumin, 7, 1) + "******";
                    SS3.ActiveSheet.Cells[5, 3].Text = VB.IIf(item.JINGBN == "Y", "●신규 ,○정기", "○신규 ,●정기").ToString();
                    SS3.ActiveSheet.Cells[5, 7].Text = item.JEPDATE.Trim();

                    //검사결과 인쇄
                    List<HIC_RESULT_EXCODE> list = hicResultExCodeService.GetExcodeResultListByWrtno(fnWrtno, item.SEX.Trim());
                    if(!list.IsNullOrEmpty())
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            switch (list[i].EXCODE.Trim())
                            {
                                case "A121": //
                                    SS3.ActiveSheet.Cells[8, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[8, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "A281": //
                                    SS3.ActiveSheet.Cells[9, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[9, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "A282": //
                                    SS3.ActiveSheet.Cells[10, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[10, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "A283": //
                                    SS3.ActiveSheet.Cells[11, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[11, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "H805": //
                                    SS3.ActiveSheet.Cells[12, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[12, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "H806": //
                                    SS3.ActiveSheet.Cells[13, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[13, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "H807": //
                                    SS3.ActiveSheet.Cells[14, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[14, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "H808": //
                                    SS3.ActiveSheet.Cells[15, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[15, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "H809": //
                                    SS3.ActiveSheet.Cells[16, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[16, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "H810": //
                                    SS3.ActiveSheet.Cells[17, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[17, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "H811": //
                                    SS3.ActiveSheet.Cells[18, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[18, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "H812": //
                                    SS3.ActiveSheet.Cells[19, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[19, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "H831": //
                                    SS3.ActiveSheet.Cells[20, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[20, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "H833": //
                                    SS3.ActiveSheet.Cells[21, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[21, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "H834": //
                                    SS3.ActiveSheet.Cells[22, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[22, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "H835": //
                                    SS3.ActiveSheet.Cells[23, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[23, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "H836": //
                                    SS3.ActiveSheet.Cells[24, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[24, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "H825": //
                                    SS3.ActiveSheet.Cells[25, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[25, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "H837": //
                                    SS3.ActiveSheet.Cells[26, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[26, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "TE42": //
                                    SS3.ActiveSheet.Cells[27, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[27, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                default:
                                    break;
                            }
                        }

                        //초지 및 종합소견
                        strTemp1 = "";
                        strTemp1 = item.SOGEN.Trim() + "/";
                        SS3.ActiveSheet.Cells[28, 4].Text = strTemp1;
                        strTemp1 = item.PANJENG.Trim();
                        SS3.ActiveSheet.Cells[29, 4].Text = strTemp1;

                        //판독일자
                        SS3.ActiveSheet.Cells[32, 4].Text = VB.Mid(item.PANJENGDATE, 1, 4) + "년 " + VB.Mid(item.PANJENGDATE, 6, 2) + "월 " + VB.Mid(item.PANJENGDATE, 9, 2) + "일";

                        //판정의사명, 면허번호
                        SS3.ActiveSheet.Cells[34, 4].Text = hb.READ_License_DrName(item.PANJENGDRNO);
                        SS3.ActiveSheet.Cells[34, 7].Text = item.PANJENGDRNO.Trim();
                        fnDrno = item.PANJENGDRNO;
                    }
                }
            }
        }

        private void Result_Print_Sub2()
        {
            string[] strExcode = new string[] { "A121", "A282", "A283", "H805" };

            string strJumin = "";
            string strLtdCode = "";
            string strJepDate = "";
            string strTemp1 = "";
            string strJikJong = "";

            HIC_X_MUNJIN_JEPSU_PATIENT item = hicXMunjinJepsuPatientService.GetAllItemsByWrtno(fnWrtno);
            if (!item.IsNullOrEmpty())
            {
                fstrJong = item.GJJONG;
                strLtdCode = item.LTDCODE;
                strJepDate = item.JEPDATE;
                strJumin = clsAES.DeAES(item.JUMIN2);

                fstrBogunso = "";
                if (fstrJong == "51")
                {
                    if (hicSunapdtlService.GetCountbyWrtNoCode(fnWrtno, "5106") > 0)
                    {
                        fstrBogunso = "Y";
                    }
                }

                //보건소제출용
                if (fstrJong == "51" && fstrBogunso == "Y")
                {
                    SS4.ActiveSheet.Cells[3, 2].Text = cf.Read_Ltd_Name(clsDB.DbCon, item.LTDCODE);

                    strJikJong = "";
                    SS4.ActiveSheet.Cells[3, 6].Text = "";
                    if (item.JIKJONG1 == "1") { strJikJong += "비파괴검사,"; }
                    if (item.JIKJONG2 == "1") { strJikJong += "방사선사,"; }
                    if (!item.JIKJONG3.IsNullOrEmpty()) { strJikJong += item.JIKJONG3.Trim(); }
                    if (strJikJong == "") { strJikJong = "방사선사"; }
                    if (VB.Right(strJikJong, 1) == ",") { strJikJong = VB.Left(strJikJong, VB.Len(strJikJong) - 1); }
                    SS4.ActiveSheet.Cells[3, 6].Text = strJikJong;

                    SS4.ActiveSheet.Cells[4, 2].Text = item.SNAME.Trim();
                    SS4.ActiveSheet.Cells[4, 6].Text = VB.Left(strJumin, 6) + "-" + VB.Mid(strJumin, 7, 1) + "******";
                    SS4.ActiveSheet.Cells[5, 2].Text = item.JEPDATE.Trim();
                    SS4.ActiveSheet.Cells[5, 6].Text = item.IPSADATE.Trim();


                    //문진사항
                    SS4.ActiveSheet.Cells[8, 8].Text = VB.IIf(item.XP1 == "Y", "유", "무").ToString();
                    SS4.ActiveSheet.Cells[9, 8].Text = "무";
                    if (item.XP1 == "Y")
                    {
                        if (!item.XPJONG.IsNullOrEmpty() && !item.XPLACE.IsNullOrEmpty())
                        {
                            SS4.ActiveSheet.Cells[9, 8].Text = "유";
                        }
                    }
                    SS4.ActiveSheet.Cells[10, 8].Text = "없음";
                    if (!item.XJUNGSAN.IsNullOrEmpty()) { SS4.ActiveSheet.Cells[10, 8].Text = item.XJUNGSAN; }

                    //검사항목


                    List<HIC_RESULT> list = hicResultService.GetExCodebyWrtNo_All(fnWrtno, strExcode);

                    if (!list.IsNullOrEmpty())
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            switch (list[i].EXCODE)
                            {
                                case "A121":
                                    SS4.ActiveSheet.Cells[13, 7].Text = list[i].RESULT.Trim();
                                    break;
                                case "A283":
                                    SS4.ActiveSheet.Cells[14, 7].Text = list[i].RESULT.Trim();
                                    break;
                                case "A282":
                                    SS4.ActiveSheet.Cells[15, 7].Text = list[i].RESULT.Trim();
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    SS4.ActiveSheet.Cells[16, 7].Text = "";
                    //판독일자
                    SS4.ActiveSheet.Cells[21, 1].Text = VB.Mid(item.PANJENGDATE, 1, 4) + "년 " + VB.Mid(item.PANJENGDATE, 6, 2) + "월 " + VB.Mid(item.PANJENGDATE, 9, 2) + "일";
                    //판정의사명, 면허번호
                    SS4.ActiveSheet.Cells[23, 1].Text = "판정의사명: " + hb.READ_License_DrName(item.PANJENGDRNO);
                    SS4.ActiveSheet.Cells[24, 1].Text = "면 허 번 호 : " + item.PANJENGDRNO;
                    fnDrno = item.PANJENGDRNO;
                }
                else if (fstrJong == "51")
                {

                    SS2.ActiveSheet.Cells[3, 3].Text = item.SNAME.Trim();
                    SS2.ActiveSheet.Cells[4, 3].Text = " " + VB.Left(strJumin, 6) + "-" + VB.Mid(strJumin, 7, 1) + "******";

                    //최근특이사항
                    if (!item.SYMPTON.IsNullOrEmpty())
                    {
                        SS2.ActiveSheet.Cells[7, 2].Text = "  " + item.SYMPTON.Trim() + "," + ComNum.VBLF;
                    }
                    else
                    {
                        SS2.ActiveSheet.Cells[7, 2].Text = "없 음";
                    }

                    //방사선작업과관련된증상
                    if (!item.JIKJONG1.IsNullOrEmpty() || !item.JIKJONG2.IsNullOrEmpty() || !item.JIKJONG3.IsNullOrEmpty())
                    {
                        if (item.JIKJONG1 == "1") { strJikJong += "비파괴검사,"; }
                        if (item.JIKJONG2 == "1") { strJikJong += "방사선사,"; }
                        if (!item.JIKJONG3.IsNullOrEmpty()) { strJikJong += item.JIKJONG3.Trim(); }
                        if (VB.Right(strJikJong, 1) == ",") { strJikJong = VB.Left(strJikJong, VB.Len(strJikJong) - 1); }
                        SS2.ActiveSheet.Cells[8, 2].Text = strJikJong;
                    }
                    else
                    {
                        SS2.ActiveSheet.Cells[8, 2].Text = "없 음";
                    }

                    //검사결과 인쇄
                    List<HIC_RESULT> list = hicResultService.GetExCodebyWrtNo_All(fnWrtno, strExcode);

                    if (!list.IsNullOrEmpty())
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            switch (list[i].EXCODE)
                            {
                                case "A121":    //혈색소
                                    SS2.ActiveSheet.Cells[12, 3].Text = list[i].RESULT.Trim();
                                    break;
                                case "A282":    //백혈구
                                    SS2.ActiveSheet.Cells[10, 3].Text = list[i].RESULT.Trim();
                                    break;
                                case "H805":    //혈소판
                                    SS2.ActiveSheet.Cells[11, 3].Text = list[i].RESULT.Trim();
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    //조치 및 종합소견
                    strTemp1 = "";
                    strTemp1 = item.SOGEN.Trim() + "/" + item.PANJENG.Trim();
                    SS2.ActiveSheet.Cells[13, 3].Text = strTemp1;

                    //채혈일자
                    SS2.ActiveSheet.Cells[14, 3].Text = item.JEPDATE;

                    //판정의사명, 면허번호
                    SS2.ActiveSheet.Cells[18, 6].Text = "면 허 번 호 : " + item.PANJENGDRNO + VB.Space(14);
                    SS2.ActiveSheet.Cells[19, 6].Text = "판정의사명: " + hb.READ_License_DrName(item.PANJENGDRNO);
                    fnDrno = item.PANJENGDRNO;
                }
                else
                {


                    SS3.ActiveSheet.Cells[3, 3].Text = cf.Read_Ltd_Name(clsDB.DbCon, item.LTDCODE);
                    SS3.ActiveSheet.Cells[3, 7].Text = item.XTERM1.Trim();
                    SS3.ActiveSheet.Cells[4, 3].Text = item.SNAME.Trim();
                    SS3.ActiveSheet.Cells[4, 7].Text = VB.Left(strJumin, 6) + "-" + VB.Mid(strJumin, 7, 1) + "******";
                    SS3.ActiveSheet.Cells[5, 3].Text = VB.IIf(item.JINGBN == "Y", "●신규 ,○정기", "○신규 ,●정기").ToString();
                    SS3.ActiveSheet.Cells[5, 7].Text = item.JEPDATE.Trim();

                    //검사결과 인쇄
                    List<HIC_RESULT_EXCODE> list = hicResultExCodeService.GetExcodeResultListByWrtno(fnWrtno, item.SEX.Trim());
                    if (!list.IsNullOrEmpty())
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            switch (list[i].EXCODE.Trim())
                            {
                                case "A121": //
                                    SS3.ActiveSheet.Cells[8, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[8, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "A281": //
                                    SS3.ActiveSheet.Cells[9, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[9, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "A282": //
                                    SS3.ActiveSheet.Cells[10, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[10, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "A283": //
                                    SS3.ActiveSheet.Cells[11, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[11, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "H805": //
                                    SS3.ActiveSheet.Cells[12, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[12, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "H806": //
                                    SS3.ActiveSheet.Cells[13, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[13, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "H807": //
                                    SS3.ActiveSheet.Cells[14, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[14, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "H808": //
                                    SS3.ActiveSheet.Cells[15, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[15, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "H809": //
                                    SS3.ActiveSheet.Cells[16, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[16, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "H810": //
                                    SS3.ActiveSheet.Cells[17, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[17, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "H811": //
                                    SS3.ActiveSheet.Cells[18, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[18, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "H812": //
                                    SS3.ActiveSheet.Cells[19, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[19, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "H831": //
                                    SS3.ActiveSheet.Cells[20, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[20, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "H833": //
                                    SS3.ActiveSheet.Cells[21, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[21, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "H834": //
                                    SS3.ActiveSheet.Cells[22, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[22, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "H835": //
                                    SS3.ActiveSheet.Cells[23, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[23, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "H836": //
                                    SS3.ActiveSheet.Cells[24, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[24, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "H825": //
                                    SS3.ActiveSheet.Cells[25, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[25, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "H837": //
                                    SS3.ActiveSheet.Cells[26, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[26, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                case "TE42": //
                                    SS3.ActiveSheet.Cells[27, 4].Text = list[i].RESULT.Trim();
                                    SS3.ActiveSheet.Cells[27, 7].Text = list[i].DMIN.To<string>("") + "~" + list[i].DMAX.To<string>("");
                                    break;
                                default:
                                    break;
                            }
                        }

                        //초지 및 종합소견
                        strTemp1 = "";
                        strTemp1 = item.SOGEN.Trim() + "/";
                        SS3.ActiveSheet.Cells[28, 4].Text = strTemp1;
                        strTemp1 = item.PANJENG.Trim();
                        SS3.ActiveSheet.Cells[29, 4].Text = strTemp1;

                        //판독일자
                        SS3.ActiveSheet.Cells[32, 4].Text = VB.Mid(item.PANJENGDATE, 1, 4) + "년 " + VB.Mid(item.PANJENGDATE, 6, 2) + "월 " + VB.Mid(item.PANJENGDATE, 9, 2) + "일";

                        //판정의사명, 면허번호
                        SS3.ActiveSheet.Cells[34, 4].Text = hb.READ_License_DrName(item.PANJENGDRNO);
                        SS3.ActiveSheet.Cells[34, 7].Text = item.PANJENGDRNO.Trim();
                        fnDrno = item.PANJENGDRNO;
                    }
                }
            }
        }

        private void Result_Print_Main()
        {
            string strUpdateOK = "";


            HIC_X_MUNJIN item = hicXMunjinService.GetItembyWrtNo(fnWrtno);
            if(!item.IsNullOrEmpty())
            {
                if(item.GBPRINT.IsNullOrEmpty() || item.GBPRINT !="Y")
                {
                    strUpdateOK = "OK";
                }
            }

            if (strUpdateOK == "OK" )
            {
                hicXMunjinService.UpdatePrtinfobyWrtNo(clsType.User.IdNumber, clsPublic.GstrSysDate, fnWrtno);
            }

            //접수테이블에 통보일자 세팅최초값-갱신안됨
            HIC_JEPSU item1 = hicJepsuService.GetItemByWRTNO(fnWrtno);
            if(!item1.IsNullOrEmpty())
            {
                if(item1.TONGBODATE.IsNullOrEmpty() || item1.GBPRINT != "Y")
                {
                    hicJepsuService.UpdatePRINTbyWrtNo(fnWrtno, 0, clsPublic.GstrSysDate);
                }
            }
        }

        private void Result_Print_Main_1()
        {

            //출력
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;
            ComFunc.ReadSysDate(clsDB.DbCon);

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            if (fstrBogunso == "Y")
            {
                strTitle = "";
                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Auto, PrintType.All, 0, 0, false, false, true, true, false, false, true);
                cSpd.setSpdPrint(SS4, PrePrint, setMargin, setOption, strHeader, strFooter);

                //cHPrt.HIC_CERT_INSERT(SS4, fnWrtno, "71", fnDrno);
            }
            else if ( fstrGubun == "Y")
            {
                strTitle = "";
                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Auto, PrintType.All, 0, 0, false, false, true, true, false, false, true);
                cSpd.setSpdPrint(SS2, PrePrint, setMargin, setOption, strHeader, strFooter);
                //cHPrt.HIC_CERT_INSERT(SS2, fnWrtno, "71", fnDrno);
            }
            else
            { 
                strTitle = "";
                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Auto, PrintType.All, 0, 0, false, false, true, true, false, false, true);
                cSpd.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
                //cHPrt.HIC_CERT_INSERT(SS1, fnWrtno, "71", fnDrno);
            }

        }
        private void Result_Print_Main_2()
        {
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
            cSpd.setSpdPrint(SS3, PrePrint, setMargin, setOption, strHeader, strFooter);
            //cHPrt.HIC_CERT_INSERT(SS3, fnWrtno, "72", fnDrno);

        }
    }
}
