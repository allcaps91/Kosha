using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using System;
using System.Windows.Forms;

namespace ComHpcLibB
{
    public partial class frmHcNhicView : Form
    {
        public delegate void SetGstrValue(WORK_NHIC item);

        public frmHcNhicView()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            Screen_Clear();
        }

        private void Screen_Clear()
        {
            for (int i = 1; i < 11; i++) { SS10.ActiveSheet.Cells[i, 1].Text = ""; }
            for (int i = 12; i < 22; i++) { SS10.ActiveSheet.Cells[i, 1].Text = ""; SS10.ActiveSheet.Cells[i, 4].Text = ""; }
            for (int i = 24; i < 32; i++) { SS10.ActiveSheet.Cells[i, 1].Text = ""; }
        }

        public void SetDisPlay(WORK_NHIC item)
        {
            Screen_Clear();

            if (!item.IsNullOrEmpty())
            {
                clsHcType.TEC.NHICOK = "Y";

                //인적정보 
                SS10.ActiveSheet.Cells[1, 1].Text = item.SNAME;                                   //성명
                SS10.ActiveSheet.Cells[2, 1].Text = clsAES.DeAES(item.JUMIN2);                    //주민등록
                SS10.ActiveSheet.Cells[3, 1].Text = item.REL;                                     //사업구분
                SS10.ActiveSheet.Cells[4, 1].Text = item.YEAR;                                    //사업년도
                SS10.ActiveSheet.Cells[5, 1].Text = item.TRANS;                                   //자격변동
                SS10.ActiveSheet.Cells[6, 1].Text = item.GKIHO;                                   //증번호
                SS10.ActiveSheet.Cells[7, 1].Text = item.JISA;                                    //소속지사
                SS10.ActiveSheet.Cells[8, 1].Text = item.BDATE;                                   //취득일자
                SS10.ActiveSheet.Cells[9, 1].Text = item.CANCER53;                                //관할보건소
                SS10.ActiveSheet.Cells[10, 1].Text = item.KIHO;                                   //회사기호

                //자격조회 시간
                if (item.CTIME.IsNullOrEmpty())
                {
                    SS10.ActiveSheet.Cells[11, 0].Text = "검 진 정 보";
                }
                else
                {
                    SS10.ActiveSheet.Cells[11, 0].Text = "검 진 정 보" + " (자격조회시간: " + item.CTIME + " )";
                }

                //검사정보
                SS10.ActiveSheet.Cells[12, 1].Text = item.FIRST;                                  //1차검진
                SS10.ActiveSheet.Cells[13, 1].Text = item.DENTAL;                                 //구강검진
                SS10.ActiveSheet.Cells[14, 1].Text = item.LIVER;                                  //B형간염
                SS10.ActiveSheet.Cells[15, 1].Text = item.LIVERC;                                 //C형간염
                //SS10.ActiveSheet.Cells[16, 1].Text = item.SECOND;                                 //2차검진
                SS10.ActiveSheet.Cells[16, 1].Text = item.CANCER71 + "(" + item.CANCER72 + ")";   //폐암
                SS10.ActiveSheet.Cells[17, 1].Text = item.CANCER11 + "(" + item.CANCER12 + ")";   //위암
                SS10.ActiveSheet.Cells[18, 1].Text = item.CANCER21 + "(" + item.CANCER22 + ")";   //유방암
                SS10.ActiveSheet.Cells[19, 1].Text = item.CANCER31 + "(" + item.CANCER32 + ")";   //대장암
                if (string.Compare(VB.Mid(DateTime.Now.ToShortDateString(), 6, 5), "06-30") <= 0)
                {
                    SS10.ActiveSheet.Cells[20, 1].Text = item.CANCER41 + "(" + item.CANCER42 + ")";      //간암(전반기)
                }
                else
                {
                    SS10.ActiveSheet.Cells[20, 1].Text = item.CANCER61 + "(" + item.CANCER62 + ")";      //간암(후반기)
                }
                SS10.ActiveSheet.Cells[21, 1].Text = item.CANCER51 + "(" + item.CANCER52 + ")";   //자궁암
                SS10.ActiveSheet.Cells[12, 4].Text = item.EXAMA;                                  //이상지질혈증
                SS10.ActiveSheet.Cells[13, 4].Text = item.EXAMD;                                  //골밀도
                SS10.ActiveSheet.Cells[14, 4].Text = item.EXAME;                                  //인지기능
                SS10.ActiveSheet.Cells[15, 4].Text = item.EXAMF;                                  //정신건강
                SS10.ActiveSheet.Cells[16, 4].Text = item.EXAMG;                                  //생활습관
                SS10.ActiveSheet.Cells[17, 4].Text = item.EXAMH;                                  //노인신체
                SS10.ActiveSheet.Cells[18, 4].Text = item.EXAMI;                                  //치면세균
                //SS10.ActiveSheet.Cells[21, 4].Text = item.CANCER71 + "(" + item.CANCER72 + ")";   //폐암
                //수검정보
                SS10.ActiveSheet.Cells[24, 1].Text = item.GBCHK01 + " " + item.GBCHK01_NAME;      //1차검진
                SS10.ActiveSheet.Cells[25, 1].Text = item.GBCHK02 + " " + item.GBCHK02_NAME;      //2차검진
                SS10.ActiveSheet.Cells[26, 1].Text = item.GBCHK03 + " " + item.GBCHK03_NAME;      //구강검사
                SS10.ActiveSheet.Cells[27, 1].Text = item.GBCHK04 + " " + item.GBCHK04_NAME;      //위암
                SS10.ActiveSheet.Cells[28, 1].Text = item.GBCHK05 + " " + item.GBCHK05_NAME;      //대장암
                SS10.ActiveSheet.Cells[29, 1].Text = item.GBCHK06 + " " + item.GBCHK06_NAME;      //유방암
                SS10.ActiveSheet.Cells[30, 1].Text = item.GBCHK07 + " " + item.GBCHK07_NAME;      //자궁경부암
                SS10.ActiveSheet.Cells[31, 1].Text = item.GBCHK09 + " " + item.GBCHK09_NAME;      //간암
                SS10.ActiveSheet.Cells[32, 1].Text = item.GBCHK10 + " " + item.GBCHK10_NAME;      //폐암

                if (item.GBCHK04.IsNullOrEmpty())
                {
                    SS10.ActiveSheet.Cells[27, 1].Text = item.GBCHK15 + " " + item.GBCHK15_NAME;      //위암
                }

                if (item.GBCHK05.IsNullOrEmpty())
                {
                    SS10.ActiveSheet.Cells[28, 1].Text = item.GBCHK16 + " " + item.GBCHK16_NAME;      //대장암
                }

                if (item.GBCHK05.IsNullOrEmpty() && item.GBCHK16.IsNullOrEmpty())
                {
                    SS10.ActiveSheet.Cells[28, 1].Text = item.GBCHK17 + " " + item.GBCHK17_NAME;      //대장암
                }
            }  
        }

        public void SetDisPlayByVariable()
        {
            Screen_Clear();

            if (clsHcType.THNV.hJaGubun.IsNullOrEmpty())
            {
                return;
            }
            else
            {
                //인적정보
                SS10.ActiveSheet.Cells[1, 1].Text = clsHcType.THNV.hSName;                      //성명
                SS10.ActiveSheet.Cells[2, 1].Text = clsHcType.THNV.hJumin;                      //주민등록
                SS10.ActiveSheet.Cells[3, 1].Text = clsHcType.THNV.hJaGubun;                    //사업구분
                SS10.ActiveSheet.Cells[4, 1].Text = clsHcType.THNV.Year;                        //사업년도
                SS10.ActiveSheet.Cells[5, 1].Text = clsHcType.THNV.hJaSTS;                      //자격변동
                SS10.ActiveSheet.Cells[6, 1].Text = clsHcType.THNV.hGKiho;                      //증번호
                SS10.ActiveSheet.Cells[7, 1].Text = clsHcType.THNV.hJisa;                       //소속지사
                SS10.ActiveSheet.Cells[8, 1].Text = clsHcType.THNV.hGetDate;                    //취득일자
                SS10.ActiveSheet.Cells[9, 1].Text = clsHcType.THNV.hBogen;                      //관할보건소
                SS10.ActiveSheet.Cells[10, 1].Text = clsHcType.THNV.hKiho;                      //회사기호

                //검사정보
                SS10.ActiveSheet.Cells[12, 1].Text = clsHcType.THNV.h1Cha;                      //1차검진
                SS10.ActiveSheet.Cells[13, 1].Text = clsHcType.THNV.hDental;                    //구강검진
                SS10.ActiveSheet.Cells[14, 1].Text = clsHcType.THNV.hLiver;                     //B형간염
                SS10.ActiveSheet.Cells[15, 1].Text = clsHcType.THNV.hLiverC;                    //C형간염
                //SS10.ActiveSheet.Cells[16, 1].Text = clsHcType.THNV.h2Cha;                      //2차검진
                SS10.ActiveSheet.Cells[16, 1].Text = clsHcType.THNV.hCan7 + "(" + clsHcType.THNV.hCan72 + ")";      //폐암
                SS10.ActiveSheet.Cells[17, 1].Text = clsHcType.THNV.hCan1 + "(" + clsHcType.THNV.hCan12 + ")";      //위암
                SS10.ActiveSheet.Cells[18, 1].Text = clsHcType.THNV.hCan2 + "(" + clsHcType.THNV.hCan22 + ")";      //유방암
                SS10.ActiveSheet.Cells[19, 1].Text = clsHcType.THNV.hCan3 + "(" + clsHcType.THNV.hCan32 + ")";      //대장암
                if (string.Compare(VB.Mid(DateTime.Now.ToShortDateString(), 6, 5), "06-30") <= 0)
                {
                    SS10.ActiveSheet.Cells[20, 1].Text = clsHcType.THNV.hCan4 + "(" + clsHcType.THNV.hCan42 + ")";  //간암(전반기)
                }
                else
                {
                    SS10.ActiveSheet.Cells[20, 1].Text = clsHcType.THNV.hCan6 + "(" + clsHcType.THNV.hCan62 + ")";  //간암(후반기)
                }
                SS10.ActiveSheet.Cells[21, 1].Text = clsHcType.THNV.hCan5 + "(" + clsHcType.THNV.hCan52 + ")";      //자궁암
                //SS10.ActiveSheet.Cells[21, 4].Text = clsHcType.THNV.hCan7 + "(" + clsHcType.THNV.hCan72 + ")";      //폐암

                SS10.ActiveSheet.Cells[12, 4].Text = clsHcType.TEC.EXAMA;                         //이상지질혈증
                SS10.ActiveSheet.Cells[13, 4].Text = clsHcType.TEC.EXAMD;                         //골밀도
                SS10.ActiveSheet.Cells[14, 4].Text = clsHcType.TEC.EXAME;                         //인지기능
                SS10.ActiveSheet.Cells[15, 4].Text = clsHcType.TEC.EXAMF;                         //정신건강
                SS10.ActiveSheet.Cells[16, 4].Text = clsHcType.TEC.EXAMG;                         //생활습관
                SS10.ActiveSheet.Cells[17, 4].Text = clsHcType.TEC.EXAMH;                         //노인신체
                SS10.ActiveSheet.Cells[18, 4].Text = clsHcType.TEC.EXAMI;                         //치면세균
                
                //수검정보
                SS10.ActiveSheet.Cells[24, 1].Text = clsHcType.THNV.h1ChaDate + " " + clsHcType.THNV.h1ChaHName;      //1차검진
                SS10.ActiveSheet.Cells[25, 1].Text = clsHcType.THNV.h2ChaDate + " " + clsHcType.THNV.h2ChaHName;      //2차검진
                SS10.ActiveSheet.Cells[26, 1].Text = clsHcType.THNV.hDentDate + " " + clsHcType.THNV.hDentHName;      //구강검사
                SS10.ActiveSheet.Cells[27, 1].Text = clsHcType.THNV.h위Date   + " " + clsHcType.THNV.h위HName;        //위암
                SS10.ActiveSheet.Cells[28, 1].Text = clsHcType.THNV.h대장Date + " " + clsHcType.THNV.h대장HName;      //대장암
                SS10.ActiveSheet.Cells[29, 1].Text = clsHcType.THNV.h유방Date + " " + clsHcType.THNV.h유방HName;      //유방암
                SS10.ActiveSheet.Cells[30, 1].Text = clsHcType.THNV.h자궁Date + " " + clsHcType.THNV.h자궁HName;      //자궁경부암
                SS10.ActiveSheet.Cells[31, 1].Text = clsHcType.THNV.h간Date   + " " + clsHcType.THNV.h간HName;        //간암
                SS10.ActiveSheet.Cells[32, 1].Text = clsHcType.THNV.h폐Date   + " " + clsHcType.THNV.h폐HName;        //폐암
            }

        }
    }
}
