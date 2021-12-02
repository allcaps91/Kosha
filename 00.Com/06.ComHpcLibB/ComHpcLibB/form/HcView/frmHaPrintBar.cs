using ComBase;
using ComBase.Controls;
using ComDbB;
using ComHpcLibB.Dto;
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
/// Class Name      : ComHpcLibB
/// File Name       : frmHaPrintBar.cs
/// Description     : 바코드중 출력 로직/// Author          : 김경동
/// Create Date     : 2020-10-06
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "" />
namespace ComHpcLibB
{
    public partial class frmHaPrintBar : Form
    {
        HEA_JEPSU nHJ = null;
        HIC_PATIENT nHP = null;
        clsHcFunc cHF = null;
        ComFunc cf = null;
        clsHaBase hb = null;

        FpSpread ssPrint;

        HeaGroupcodeService heaGroupcodeService = null;
        HeaCodeService heaCodeService = null;
        HeaResultService heaResultService = null;
        HeaResvExamService heaResvExamService = null;
        HeaJepsuService heaJepsuService = null;
        HeaExjongService heaExjongService = null;
        HicLtdService hicLtdService = null;
        HicJepsuWorkService hicJepsuWorkService = null;
        HicIeMunjinNewService hicIeMunjinNewService = null;
        HeaSunapdtlService heaSunapdtlService = null;

        public frmHaPrintBar()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }
        public frmHaPrintBar(HEA_JEPSU aHJ, HIC_PATIENT pHP)
        {
            InitializeComponent();
            SetControl();
            SetEvent();

            nHJ = aHJ;
            nHP = pHP;
        }


        private void SetControl()
        {
            nHJ = new HEA_JEPSU();
            nHP = new HIC_PATIENT();
            cHF = new clsHcFunc();
            cf = new ComFunc();
            hb = new clsHaBase();


            heaGroupcodeService = new HeaGroupcodeService();
            heaCodeService = new HeaCodeService();
            heaResultService = new HeaResultService();
            heaResvExamService = new HeaResvExamService();
            heaJepsuService = new HeaJepsuService();
            heaExjongService = new HeaExjongService();
            hicLtdService = new HicLtdService();
            hicJepsuWorkService = new HicJepsuWorkService();
            hicIeMunjinNewService = new HicIeMunjinNewService();
            heaSunapdtlService = new HeaSunapdtlService();

        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormload);
        }

        private void eFormload(object sender, EventArgs e)
        {
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread SPR = new clsSpread();
            clsPrint CP = new clsPrint();
            clsVbfunc vb = new clsVbfunc();

            int nSeqNo = 0;
            long nREAD = 0;
            long nAge = 0;
            long nRow = 0;
            long nCNT = 0;
            string strDeptName = "";
            string strDeptCode = "";
            string strPano = "";
            string strPtno = "";
            string strBDate = "";
            string strJepDate = "";
            string strSname = "";
            string strSex = "";
            string strWard = "";
            string strDeptCnt = "";
            string strJumin1 = "";
            string strJumin2 = "";
            string strDeptCode2 = "";
            string strTel = "";
            string strHPhone = "";
            string strJuso = "";
            string strAct = "";
            string strGjjong = "";
            string strRTime = "";
            string strRTimeList = "";
            string strNAMEE = "";
            string strNakSang = "";
            string strTemp = "";
            string strOK = "";
            string strPrintName = "";
            string strFamillyName = "";
            string strAMPM = "";
            string strLtdNamd = "";
            string strJongName = "";

            string strGubun1 = "";
            string strGubun2 = "";
            string strGubun3 = "";
            string strMundate = "";

            nAge = nHJ.AGE;
            strPano = nHJ.PANO.ToString();
            strDeptCode = "TO";
            strBDate = nHJ.SDATE;
            strJepDate = nHJ.JEPDATE;
            strSname = nHJ.SNAME;
            strGjjong = nHJ.GJJONG;
            strJuso = nHJ.JUSO1 + " " + nHJ.JUSO2;
            strTel = nHP.TEL;
            strHPhone = nHP.HPHONE;
            strSex = nHJ.SEX;
            strPtno = nHJ.PTNO;



            strNAMEE = vb.Read_Patient_Ename(clsDB.DbCon, strPtno, "1");

            if (nHJ.WRTNO == 0) { return;}
            if (strPano=="") { return; }

            //2016-07-19 개인종검인 경우 종검 종류명을 표시함
            if (strGjjong =="11" || strGjjong=="12")
            {
                HEA_GROUPCODE item = heaGroupcodeService.GetYNamebyWrtNo(nHJ.WRTNO);
                if (!item.IsNullOrEmpty())
                {
                    strGjjong = item.YNAME;
                }
            }

            //예약시간을 인쇄할 항목을 설정
            List<HEA_CODE> list = heaCodeService.GetItemByGubun("15");
            for (int i = 0; i < list.Count; i++)
            {
                strRTimeList = strRTimeList + list[i].CODE.Trim()+",";
            }

            if( cHF.GET_Naksang_Flag(nHJ.AGE, nHJ.SDATE, nHJ.PTNO) =="Y") { strNakSang = "★낙상주의"; }

            //
            if(nHJ.AMPM2 =="1")
            {
                strAMPM = "오전";
            }
            else if (nHJ.AMPM2 == "2")

            {
                strAMPM = "오후";
            }

            //검진종류이름읽기
            //HEA_EXJONG item5 = heaExjongService.Read_ExJong_CodeName(nHJ.GJJONG);

            strJongName = heaSunapdtlService.GetMainSunapDtlCodeNameByWrtno(nHJ.WRTNO);

            if (!nHJ.LTDCODE.IsNullOrEmpty() && nHJ.LTDCODE!= 0)
            {
                HIC_LTD item6 = hicLtdService.GetItembyCode(nHJ.LTDCODE.ToString());
                strLtdNamd = item6.NAME;
            }
            

            strDeptCode2 = strDeptCode;
            SSBar.ActiveSheet.Cells[1, 1].Text = strPtno + strNAMEE;
            SSBar.ActiveSheet.Cells[4, 1].Text = "★"+nHP.VIPREMARK +" "+ strAMPM + " "+ strNakSang;
            SSBar.ActiveSheet.Cells[5, 1].Text = strSname + "(" + strSex + "/" + nAge + ")";
            SSBar.ActiveSheet.Cells[6, 1].Text = "주민등록번호: " + VB.Left(nHP.JUMIN, 6) + "-" + VB.Mid(nHP.JUMIN,7,7 );
            //일반검진 종류, 인터넷, 수령방식

            strMundate = VB.Left(nHJ.SDATE, 4) + "-01-01";
            HIC_IE_MUNJIN_NEW item1 =  hicIeMunjinNewService.GetItembyPtnoMundate(nHJ.PTNO, strMundate);
            if(!item1.IsNullOrEmpty())
            {
                strGubun2 = "인●";
            }

            List<HIC_JEPSU_WORK> lstHJW2 = hicJepsuWorkService.GetListGjNameByPtnoJepDate(VB.Left(nHJ.SDATE,4), nHJ.PTNO);
            if (lstHJW2.Count > 0)
            {
                for (int i = 0; i < lstHJW2.Count; i++)
                {
                    strGubun1 += lstHJW2[i].GJJONG + ",";
                }
            }

            if (!nHJ.WEBPRINTREQ.IsNullOrEmpty()) { strGubun3 = "톡●"; }


            SSBar.ActiveSheet.Cells[7, 1].Text = strGubun1;
            SSBar.ActiveSheet.Cells[7, 5].Text = strGubun2;
            SSBar.ActiveSheet.Cells[7, 7].Text = strGubun3;
            SSBar.ActiveSheet.Cells[8, 1].Text = "종검종류: " + strJongName;
            //SSBar.ActiveSheet.Cells[8, 1].Text = "종검종류: "+ nHJ.GJJONG+"."+strJongName + "/" + strLtdNamd;

            //가셔야할곳을 찾음
            List<HEA_RESULT> list1 = heaResultService.GetListByWrtnoGubun(nHJ.WRTNO);

            for (int i = 0; i < list1.Count; i++)
            {
                HEA_CODE item2 = heaCodeService.GetItemByActPart(list1[i].ACTPART);
                strAct = item2.NAME;

                //수면-위내시경,위내시경,갑상선초음파,유방초음파
                switch (list1[i].ACTPART)
                {
                    case "0":
                    case "G":
                    case "H":
                    case "K":
                        break;
                    default: strOK = "OK"; break;
                }

                if(!item2.CODE.Trim().IsNullOrEmpty())
                {
                    if(VB.InStr(strRTimeList, "," + VB.Trim(list1[i].ACTPART) +",")>0 && strOK == "OK")
                    {
                        //2014-01-03 수면내시경 등 예약시간 인쇄
                        HEA_RESV_EXAM item3 = heaResvExamService.GetRTimebySdatePanoExcode(strBDate, strPano, VB.Trim(list1[i].ACTPART));
                        if (!item3.IsNullOrEmpty()) 
                        {
                            strRTime = item3.RTIME;
                            if (VB.Left(item3.RTIME,10) == strBDate)
                            {
                                strAct = strAct + " (" + VB.Right(item3.RTIME, 5) + ")";
                            }
                            else
                            {
                                strAct = strAct + "▶" + strRTime + "";
                            }
                        }
                    }

                    //2015-04-09 체성분 ACT 코드가 없으면 "키+몸무게+허리둘레"로 표시 요청
                    if (strAct =="체성분")
                    {
                        if (heaResultService.ChkExamByWrtnoExCode(nHJ.WRTNO, "A918").IsNullOrEmpty())
                        {
                            strAct = "키,몸무게,허리둘레";
                        }
                    }

                    nCNT = nCNT + 1;

                    SSBar.ActiveSheet.Cells[9 + i, 1].Text = "□ " + strAct;
                    nSeqNo = 10 + i;
                }
            }

            //전화번호, 주소, 검사일자, REMARK,가족성명
            SSBar.ActiveSheet.Cells[nSeqNo, 1].Text = "전화번호:  " + strHPhone;
            if (VB.Len(strJuso) > 19)
            {
                SSBar.ActiveSheet.Cells[nSeqNo + 1, 1].Text = "주소: " + VB.Left(strJuso, 19);
                SSBar.ActiveSheet.Cells[nSeqNo + 2, 1].Text = "      " + VB.Mid(strJuso, 20, VB.Len(strJuso));
            }
            else
            {
                SSBar.ActiveSheet.Cells[nSeqNo + 1, 1].Text = "주소: " + strJuso;
            }

            SSBar.ActiveSheet.Cells[nSeqNo + 3, 1].Text = "검사일자: " + strBDate;
            SSBar.ActiveSheet.Cells[nSeqNo + 4, 1].Text = "[Remark]";

            
            HEA_JEPSU item4 = heaJepsuService.GetSnamebyPano(nHP.FAMILLY);
            if (!item4.IsNullOrEmpty())
            {
                strFamillyName = item4.SNAME;
            }
            
            SSBar.ActiveSheet.Cells[nSeqNo + 5, 1].Text = "가족성명: " + strFamillyName;

            strPrintName = CP.getPmpaBarCodePrinter("신용카드"); //Default :신용카드(접수창구용 설정이름)
            setMargin = new clsSpread.SpdPrint_Margin(0, 0, 0, 40, 0, 0);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, true, false, false);
            ssPrint = SSBar;
            SPR.setSpdPrint(ssPrint, false, setMargin, setOption, "", "", strPrintName);

            ComFunc.Delay(1500);

            ssPrint.Dispose();
            ssPrint = null;

            this.Close();
        }
    }
}