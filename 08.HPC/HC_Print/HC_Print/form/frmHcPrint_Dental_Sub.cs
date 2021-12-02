using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Windows.Forms;


/// <summary>
/// Class Name      : HC_Print
/// File Name       : frmHcPrint_Dental_Sub.cs
/// Description     : Frm구강검진결과지인쇄
/// Author          : 김경동
/// Create Date     : 2021-01-12
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm공단검진1차결과지_2020.frm(FrmIDateChange)" />

namespace HC_Print
{
    public partial class frmHcPrint_Dental_Sub : Form
    {
        long fnWrtno = 0;
        long fnDrno = 0;
        string fstrTongboGbn = "";
        string fstrGbSend = "";

        ComFunc cf = new ComFunc();
        clsHcPrint cHPrt = new clsHcPrint();
        clsSpread cSpd = new clsSpread();

        HicResDentalService hicResDentalService = null;
        HicJepsuService hicJepsuService = null;
        HicResDentalJepsuPatientService hicResDentalJepsuPatientService = null;
        HicSunapdtlService hicSunapdtlService = null;
        HicJepsuGundateService hicJepsuGundateService = null;

        public frmHcPrint_Dental_Sub()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }

        public frmHcPrint_Dental_Sub(long argWRTNO)
        {
            InitializeComponent();
            SetControl();
            SetEvent();

            fnWrtno = argWRTNO;
        }

        private void SetControl()
        {

            hicResDentalService = new HicResDentalService();
            hicJepsuService = new HicJepsuService();
            hicResDentalJepsuPatientService = new HicResDentalJepsuPatientService();
            hicSunapdtlService = new HicSunapdtlService();
            hicJepsuGundateService = new HicJepsuGundateService();
        }
        private void SetEvent()
        {
            this.Load += new EventHandler(eFormload);
        }



        private void eFormload(object sender, EventArgs e)
        {
            Result_Print_Sub();
        }

        private void Result_Print_Sub()
        {
            int result = 0;
            int nJumsu1 = 0;
            int nJumsu2 = 0;
            int nJumsu3 = 0;
            int nJumsu4 = 0;
            int nJumsu5 = 0;
            int nJumsu6 = 0;
            int nJumsu7 = 0;

            long nCntR = 0;



            string strBangi = "";
            string strGbn = "";
            string strGjjong = "";
            string strJepdate = "";
            string strUpdateOK = "";
            string strREC = "";
            string strJumin = "";
            string strLtdcode = "";
            string strData = "";
            string strSname = "";
            string strTemp = "";
            string strTemp1 = "";
            string strList = "";
            string strRES1 = "";
            string strRES2 = "";
            string strRES3 = "";
            string strJuso = "";
            string strGjyear = "";
            string strSogen1 = "";
            string strSogen2 = "";
            string strGubun = "";

            if (fstrTongboGbn.IsNullOrEmpty()) { fstrTongboGbn = "2"; } //1.사업장 2.주소지 3.내원

            HIC_RES_DENTAL item = hicResDentalService.GetItemByWrtno(fnWrtno);

            if (!item.IsNullOrEmpty())
            {
                strUpdateOK = "OK";
            }
            else
            {
                if (item.GBPRINT.Trim() != "Y") { strUpdateOK = "OK"; }
                if (item.TONGBODATE.IsNullOrEmpty()) { strUpdateOK = "OK"; }
                if (item.TONGBOGBN.IsNullOrEmpty()) { strUpdateOK = "OK"; }
            }

            //구강만 검진받은분 출력자 확인
            HIC_JEPSU item2 = hicJepsuService.GetItemByWRTNO(fnWrtno);
            if (!item2.IsNullOrEmpty())
            {
                result = hicJepsuService.UpdateTongbodatePrtsabunbyWrtNo(fnWrtno, clsType.User.IdNumber.To<long>());

                if (result < 0)
                {
                    MessageBox.Show("구강결과지 출력자 UPDATE 오류", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }

            //
            if (strUpdateOK == "OK")
            {
                if (!hicResDentalService.UpdateTongBoInfobyWrtNo(fnWrtno, clsPublic.GstrSysDate, fstrTongboGbn))
                {
                    MessageBox.Show(" 통보일 UPDATE 오류 발생", "오류");
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }
            }


            //인적사항,과거직력, 문진, 임상진찰, 현재증상, 검진일자, 사업주등등...


            HIC_RES_DENTAL_JEPSU_PATEINT item3 = hicResDentalJepsuPatientService.GetItemByWrtno(fnWrtno);

            if (item3.IsNullOrEmpty())
            {
                MessageBox.Show("판정자료가 없습니다.", "확인", MessageBoxButtons.OK);
                return;
            }
            else
            {
                //strJumin = clsAES.DeAES(item3.JUMIN2);
                strLtdcode = item3.LTDCODE;
                strJepdate = item3.JEPDATE;
                strGjyear = item3.GJYEAR;
                strSname = item3.SNAME;
                strRES1 = item3.RES_MUNJIN;
                strRES2 = item3.RES_RESULT;
                strRES3 = item3.RES_JOCHI;

                //인적사항
                SS1.ActiveSheet.Cells[18, 1].Text = " 성명 :  " + strSname;
                SS1.ActiveSheet.Cells[18, 6].Text = VB.Left(strJumin, 6) + "-" + VB.Mid(strJumin, 7, 1) + "******";

                //출장구분

                SS1.ActiveSheet.Cells[18, 12].Text = "□내원 ■출장";
                SS1.ActiveSheet.Cells[18, 12].Text = "■내원 □출장";

                //종합판정
                switch (item3.T_PANJENG1)
                {
                    case "1": SS1.ActiveSheet.Cells[22, 1].Text = "  판정 - ■ 정상A  □ 정상B  □ 주의  □ 치료필요"; break;
                    case "2": SS1.ActiveSheet.Cells[22, 1].Text = "  판정 - □ 정상A  ■ 정상B  □ 주의  □ 치료필요"; break;
                    case "3": SS1.ActiveSheet.Cells[22, 1].Text = "  판정 - □ 정상A  □ 정상B  ■ 주의  □ 치료필요"; break;
                    case "4": SS1.ActiveSheet.Cells[22, 1].Text = "  판정 - □ 정상A  □ 정상B  □ 주의  ■ 치료필요"; break;
                    default: SS1.ActiveSheet.Cells[22, 1].Text = "  판정 - □ 정상A  □ 정상B  □ 주의  □ 치료필요"; break;
                }

                //바로조치
                strSogen1 = item3.SANGDAM;
                if (strSogen1.Trim().IsNullOrEmpty()) { strSogen1 = "특이사항없음"; }
                strSogen2 = "";

                if (!item3.T_PANJENG_SOGEN.IsNullOrEmpty())
                {
                    strSogen2 = strSogen2 + "," + VB.Replace(item3.T_PANJENG_SOGEN, ComNum.VBLF, "");
                }
                if (strSogen2.IsNullOrEmpty()) { strSogen2 = "특이사항없음"; }

                SS1.ActiveSheet.Cells[23, 1].Text = "● " + strSname + "님은 다음 사항에 대해 바로 조치가 필요합니다.";
                SS1.ActiveSheet.Cells[24, 1].Text = strSogen1;
                SS1.ActiveSheet.Cells[25, 1].Text = "● " + strSname + "님은 다음 사항에 대해 적극적인 관리가 필요합니다.";
                SS1.ActiveSheet.Cells[26, 1].Text = strSogen2;

                //1.(치과) 병력문제
                if (VB.Mid(strRES1, 1, 1) == "1")
                {
                    SS1.ActiveSheet.Cells[28, 5].Text = " ■ 없음 □ 있음";
                }
                else if (VB.Mid(strRES1, 1, 1) == "2")
                {
                    SS1.ActiveSheet.Cells[28, 5].Text = " □ 없음 ■ 있음";
                }
                else
                {
                    SS1.ActiveSheet.Cells[28, 5].Text = " □ 없음 □ 있음";
                }

                //2.구강건강인식도문제
                if (VB.Mid(strRES1, 2, 1) == "1")
                {
                    SS1.ActiveSheet.Cells[29, 5].Text = " ■ 없음 □ 있음";
                }
                else if (VB.Mid(strRES1, 2, 1) == "2")
                {
                    SS1.ActiveSheet.Cells[29, 5].Text = " □ 없음 ■ 있음";
                }
                else
                {
                    SS1.ActiveSheet.Cells[29, 5].Text = " □ 없음 □ 있음";
                }

                //3.구강위생
                if (VB.Mid(strRES1, 3, 1) == "1")
                {
                    SS1.ActiveSheet.Cells[30, 5].Text = "구강위생: ■ 없음 □ 있음";
                }
                else if (VB.Mid(strRES1, 3, 1) == "2")
                {
                    SS1.ActiveSheet.Cells[30, 5].Text = "구강위생: □ 없음 ■ 있음";
                }
                else
                {
                    SS1.ActiveSheet.Cells[30, 5].Text = "구강위생: □ 없음 □ 있음";
                }

                //4.불소이용
                if (VB.Mid(strRES1, 4, 1) == "1")
                {
                    SS1.ActiveSheet.Cells[30, 9].Text = "불소이용: ■ 없음 □ 있음";
                }
                else if (VB.Mid(strRES1, 4, 1) == "2")
                {
                    SS1.ActiveSheet.Cells[30, 9].Text = "불소이용: □ 없음 ■ 있음";
                }
                else
                {
                    SS1.ActiveSheet.Cells[30, 9].Text = "불소이용: □ 없음 □ 있음";
                }

                //5.설탕섭취
                if (VB.Mid(strRES1, 5, 1) == "1")
                {
                    SS1.ActiveSheet.Cells[31, 5].Text = "설탕섭취: ■ 없음 □ 있음";
                }
                else if (VB.Mid(strRES1, 5, 1) == "2")
                {
                    SS1.ActiveSheet.Cells[31, 5].Text = "설탕섭취: □ 없음 ■ 있음";
                }
                else
                {
                    SS1.ActiveSheet.Cells[31, 5].Text = "설탕섭취: □ 없음 □ 있음";
                }

                //6.흡연
                if (VB.Mid(strRES1, 6, 1) == "1")
                {
                    SS1.ActiveSheet.Cells[31, 9].Text = "흡    연: ■ 없음 □ 있음";
                }
                else if (VB.Mid(strRES1, 6, 1) == "2")
                {
                    SS1.ActiveSheet.Cells[31, 9].Text = "흡    연: □ 없음 ■ 있음";
                }
                else
                {
                    SS1.ActiveSheet.Cells[31, 9].Text = "흡    연: □ 없음 □ 있음";
                }

                //--------( 구강검사 결과 )-----------
                //1.우식치아
                if (VB.Mid(strRES2, 1, 1) == "1")
                {
                    SS1.ActiveSheet.Cells[33, 5].Text = " ■ 없음 □ 있음";
                }
                else if (VB.Mid(strRES2, 1, 1) == "2")
                {
                    SS1.ActiveSheet.Cells[33, 5].Text = " □ 없음 ■ 있음";
                }
                else
                {
                    SS1.ActiveSheet.Cells[33, 5].Text = " □ 없음 □ 있음";
                }

                //2.인접면 우식 의심치아
                if (VB.Mid(strRES2, 2, 1) == "1")
                {
                    SS1.ActiveSheet.Cells[34, 5].Text = " ■ 없음 □ 있음";
                }
                else if (VB.Mid(strRES2, 2, 1) == "2")
                {
                    SS1.ActiveSheet.Cells[34, 5].Text = " □ 없음 ■ 있음";
                }
                else
                {
                    SS1.ActiveSheet.Cells[34, 5].Text = " □ 없음 □ 있음";
                }

                //3.수복치아
                if (VB.Mid(strRES2, 3, 1) == "1")
                {
                    SS1.ActiveSheet.Cells[35, 5].Text = " ■ 없음 □ 있음";
                }
                else if (VB.Mid(strRES2, 3, 1) == "2")
                {
                    SS1.ActiveSheet.Cells[35, 5].Text = " □ 없음 ■ 있음";
                }
                else
                {
                    SS1.ActiveSheet.Cells[35, 5].Text = " □ 없음 □ 있음";
                }

                //4.상실치아
                if (VB.Mid(strRES2, 4, 1) == "1")
                {
                    SS1.ActiveSheet.Cells[36, 5].Text = " ■ 없음 □ 있음";
                }
                else if (VB.Mid(strRES2, 4, 1) == "2")
                {
                    SS1.ActiveSheet.Cells[36, 5].Text = " □ 없음 ■ 있음";
                }
                else
                {
                    SS1.ActiveSheet.Cells[36, 5].Text = " □ 없음 □ 있음";
                }

                //5.치은염증
                if (VB.Mid(strRES2, 5, 1) == "1")
                {
                    SS1.ActiveSheet.Cells[34, 11].Text = " ■ 없음 □ 경증 □ 중증";
                }
                else if (VB.Mid(strRES2, 5, 1) == "2")
                {
                    SS1.ActiveSheet.Cells[34, 11].Text = " ■ 없음 □ 경증 □ 중증";
                }
                else if (VB.Mid(strRES2, 5, 1) == "3")
                {
                    SS1.ActiveSheet.Cells[34, 11].Text = " ■ 없음 □ 경증 □ 중증";
                }
                else
                {
                    SS1.ActiveSheet.Cells[34, 11].Text = " ■ 없음 □ 경증 □ 중증";
                }

                //6.치석
                if (VB.Mid(strRES2, 6, 1) == "1")
                {
                    SS1.ActiveSheet.Cells[35, 11].Text = " ■ 없음 □ 경증 □ 중증";
                }
                else if (VB.Mid(strRES2, 6, 1) == "2")
                {
                    SS1.ActiveSheet.Cells[35, 11].Text = " ■ 없음 □ 경증 □ 중증";
                }
                else if (VB.Mid(strRES2, 6, 1) == "3")
                {
                    SS1.ActiveSheet.Cells[35, 11].Text = " ■ 없음 □ 경증 □ 중증";
                }
                else
                {
                    SS1.ActiveSheet.Cells[35, 11].Text = " ■ 없음 □ 경증 □ 중증";
                }

                //기타소견
                SS1.ActiveSheet.Cells[37, 3].Text = item3.T_PANJENG_ETC.Trim();

                strGubun = "";
                if (hicSunapdtlService.GetCountbyWrtNoCode(fnWrtno, "1158") > 0) { strGubun = "OK"; }

                nJumsu1 = 0; nJumsu2 = 0; nJumsu3 = 0; nJumsu4 = 0; nJumsu5 = 0; nJumsu6 = 0; nJumsu7 = 0;
                nJumsu1 = Convert.ToInt32(item3.T40_PAN1_NEW);
                nJumsu2 = Convert.ToInt32(item3.T40_PAN2_NEW);
                nJumsu3 = Convert.ToInt32(item3.T40_PAN3_NEW);
                nJumsu4 = Convert.ToInt32(item3.T40_PAN4_NEW);
                nJumsu5 = Convert.ToInt32(item3.T40_PAN5_NEW);
                nJumsu6 = Convert.ToInt32(item3.T40_PAN6_NEW);

                nJumsu7 = ((nJumsu1 + nJumsu2 + nJumsu3 + nJumsu4 + nJumsu5 + nJumsu6 + nJumsu7) / 6);

                if (strGubun == "OK")
                {
                    SS1.ActiveSheet.Cells[40, 8].Text = nJumsu1.ToString();
                    SS1.ActiveSheet.Cells[41, 8].Text = nJumsu2.ToString();
                    SS1.ActiveSheet.Cells[42, 8].Text = nJumsu3.ToString();
                    SS1.ActiveSheet.Cells[43, 8].Text = nJumsu4.ToString();
                    SS1.ActiveSheet.Cells[44, 8].Text = nJumsu5.ToString();
                    SS1.ActiveSheet.Cells[45, 8].Text = nJumsu6.ToString();
                    SS1.ActiveSheet.Cells[46, 8].Text = nJumsu7.ToString();
                }
                else
                {
                    SS1.ActiveSheet.Cells[40, 8].Text = "";
                    SS1.ActiveSheet.Cells[41, 8].Text = "";
                    SS1.ActiveSheet.Cells[42, 8].Text = "";
                    SS1.ActiveSheet.Cells[43, 8].Text = "";
                    SS1.ActiveSheet.Cells[44, 8].Text = "";
                    SS1.ActiveSheet.Cells[45, 8].Text = "";
                    SS1.ActiveSheet.Cells[46, 8].Text = "";
                }

                //실검진일
                HIC_JEPSU_GUNDATE item4 = hicJepsuGundateService.GetGunDateByWrtno(fnWrtno);
                if (!item4.IsNullOrEmpty())
                {
                    SS1.ActiveSheet.Cells[48, 4].Text = item4.GUNDATE;
                }
                else
                {
                    SS1.ActiveSheet.Cells[48, 4].Text = item3.JEPDATE;
                }

                //통보일
                if (item3.TONGBODATE.IsNullOrEmpty())
                {
                    SS1.ActiveSheet.Cells[48, 8].Text = clsPublic.GstrSysDate;
                }
                else
                {
                    SS1.ActiveSheet.Cells[48, 8].Text = item3.TONGBODATE;
                }


                fnDrno = 0;
                SS1.ActiveSheet.Cells[48, 12].Text = item3.PANJENGDRNO.ToString();
                fnDrno = item3.PANJENGDRNO;

                //싸인



                //주소세팅
                strJuso = cf.TextBox_2_MultiLine(item3.JUSO1 + " " + VB.Replace(item3.JUSO2, ComNum.VBLF, ""), 60);
                strJuso = VB.Replace(strJuso, "{{@}}", ComNum.VBLF);

                SS1.ActiveSheet.Cells[8, 6].Text = strJuso;
                SS1.ActiveSheet.Cells[10, 7].Text = " " + item3.SNAME + " 귀하";

                SS1.ActiveSheet.Cells[10, 7].Text = "";
                string strMailCode = "";
                for (int i = 1; i < 4; i++)
                {
                    strMailCode += VB.Mid(item3.MAILCODE, i, 1) + " ";
                }
                strMailCode += "- ";
                for (int i = 4; i < 7; i++)
                {
                    strMailCode += VB.Mid(item3.MAILCODE, i, 1) + " ";
                }

            }

            HIC_RES_DENTAL item5 = hicResDentalService.GetItemByWrtno(fnWrtno);
            strUpdateOK = "";
            if (item5.IsNullOrEmpty())
            {
                strUpdateOK = "OK";
            }
            else
            {
                if (item5.GBPRINT != "Y") { strUpdateOK = "OK"; }
                if (item5.TONGBODATE.IsNullOrEmpty()) { strUpdateOK = "OK"; }
                if (item5.TONGBOGBN.IsNullOrEmpty()) { strUpdateOK = "OK"; }
            }

            if (strUpdateOK == "OK")
            {
                if (fstrTongboGbn == "") { fstrTongboGbn = "2"; }

                result = hicResDentalService.UpdateTongboByWrtno(fnWrtno, fstrTongboGbn);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("구강검진 결과지 인쇄 완료 변경 오류", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }


            if (fstrGbSend == "P")
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
                cSpd.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);

                cHPrt.HIC_CERT_INSERT(SS1, fnWrtno, "4A", fnDrno);

            }
            else
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
                cSpd.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
                cHPrt.HIC_CERT_INSERT(SS1, fnWrtno, "4A", fnDrno);
            }
        }

    }
}
