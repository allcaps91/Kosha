using FarPoint.Win.Spread;

namespace ComBase
{
    /// <summary>
    /// Description : 검진자격 조회 관련 모듈 및 변수
    /// Author : 박병규
    /// Create Date : 2017.07.20
    /// </summary>
    /// <history>
    /// </history>
    public class clsOiGuide_nhic
    {
        public struct TABLE_HIC_NHIC_VIEW
        {
            //'기본정보
            public string strhSName;            //'성명
            public string strhJumin;            //'주민번호
            public string strhJaGubun;          //'자격구분
            public string strhJaSTS;            //'자격상태
            public string strhGKiho;            //'증기호
            public string strhKiho;             //'회사기호
            public string strhJisa;             //'소속지사
            public string strhGetDate;          //'취득일자
            public string strhBogen;            //'관할보건소

            //'검사정보
            public string strh1Cha;             //'1차검진
            public string strhEkg;              //'심전도
            public string strhLiver;            //'간염검사
            public string strh2ChaB;            //'2차B형
            public string strhCan1;             //'암-위
            public string strhCan2;             //'암-유방
            public string strhCan3;             //'암-대장
            public string strhCan4;             //'암-간
            public string strhCan5;             //'암-자궁

            //'수검정보
            public string strh1ChaDate;         //'1차검진일자
            public string strh1ChaHName;        //'1차검진 병원명
            public string strh2ChaDate;         //'2차검진일자
            public string strh2ChaHName;        //'2차검진 병원명
            public string strhDentDate;         //'구강진일자
            public string strhDentHName;        //'구강검진 병원명

            public string strh위Date;           //'위 검진일
            public string strh위HName;          //'위검진 병원명
            public string strh대장Date;         //'검진일
            public string strh대장HName;        //'검진 병원명
            public string strh유방Date;         //'검진일
            public string strh유방HName;        //'검진 병원명
            public string strh자궁Date;         //'검진일
            public string strh자궁HName;        //'검진 병원명
            public string strh간Date;           //'검진일
            public string strh간HName;          //'검진 병원명            
        }
        public static TABLE_HIC_NHIC_VIEW THNV;

        //'자격조회된 정보 담기.
        public static void HIC_NHIC_READ(FpSpread o)
        {            
            if (o.ActiveSheet.Cells[1, 1].Text != "")
            {
                THNV.strhSName = o.ActiveSheet.Cells[1, 1].Text;
                THNV.strhJumin = o.ActiveSheet.Cells[2, 1].Text;        //'주민번호
                THNV.strhJaGubun = o.ActiveSheet.Cells[3, 1].Text;      //'자격구분
                THNV.strhJaSTS = o.ActiveSheet.Cells[5, 1].Text;        //'자격상태
                THNV.strhGKiho = o.ActiveSheet.Cells[6, 1].Text;        //'증기호
                THNV.strhJisa = o.ActiveSheet.Cells[7, 1].Text;         //'소속지사
                THNV.strhGetDate = o.ActiveSheet.Cells[8, 1].Text;      //'취득일자
                THNV.strhBogen = o.ActiveSheet.Cells[9, 1].Text;        //'관할보건소
                THNV.strhKiho = o.ActiveSheet.Cells[10, 1].Text;        //'회사기호                

                //'검사정보
                THNV.strh1Cha = o.ActiveSheet.Cells[12, 1].Text;          //'1차검진
                THNV.strhEkg = o.ActiveSheet.Cells[13, 1].Text;           //'심전도
                THNV.strhLiver = o.ActiveSheet.Cells[14, 1].Text;         //'간염검사
                THNV.strh2ChaB = o.ActiveSheet.Cells[15, 1].Text;         //'2차B형
                THNV.strhCan1 = o.ActiveSheet.Cells[16, 1].Text;          //'암-위
                THNV.strhCan2 = o.ActiveSheet.Cells[17, 1].Text;          //'암-유방
                THNV.strhCan3 = o.ActiveSheet.Cells[18, 1].Text;          //'암-대장
                THNV.strhCan4 = o.ActiveSheet.Cells[19, 1].Text;          //'암-간
                THNV.strhCan5 = o.ActiveSheet.Cells[20, 1].Text;          //'암-자궁

                //'수검정보
                THNV.strh1ChaDate = VB.Trim(VB.Pstr(o.ActiveSheet.Cells[23, 1].Text, " ", 1));      //'1차검진일자
                THNV.strh1ChaHName = VB.Trim(VB.Pstr(o.ActiveSheet.Cells[23, 1].Text, " ", 2));     //'1차검진 병원명
                THNV.strh2ChaDate = VB.Trim(VB.Pstr(o.ActiveSheet.Cells[24, 1].Text, " ", 1));      //'2차검진일자
                THNV.strh2ChaHName = VB.Trim(VB.Pstr(o.ActiveSheet.Cells[24, 1].Text, " ", 2));     //'2차검진 병원명
                THNV.strhDentDate = VB.Trim(VB.Pstr(o.ActiveSheet.Cells[25, 1].Text, " ", 1));     //'구강진일자
                THNV.strhDentHName = VB.Trim(VB.Pstr(o.ActiveSheet.Cells[25, 1].Text, " ", 2));     //'구강검진 병원명

                THNV.strh위Date = VB.Trim(VB.Pstr(o.ActiveSheet.Cells[26, 1].Text, " ", 1));  //'위 검진일
                THNV.strh위HName = VB.Trim(VB.Pstr(o.ActiveSheet.Cells[26, 1].Text, " ", 1));  //'위검진 병원명
                THNV.strh대장Date = VB.Trim(VB.Pstr(o.ActiveSheet.Cells[27, 1].Text, " ", 1));  //' 검진일
                THNV.strh대장HName = VB.Trim(VB.Pstr(o.ActiveSheet.Cells[27, 1].Text, " ", 1));  //'검진 병원명
                THNV.strh유방Date = VB.Trim(VB.Pstr(o.ActiveSheet.Cells[28, 1].Text, " ", 1));  //' 검진일
                THNV.strh유방HName = VB.Trim(VB.Pstr(o.ActiveSheet.Cells[28, 1].Text, " ", 1));  //'검진 병원명
                THNV.strh자궁Date = VB.Trim(VB.Pstr(o.ActiveSheet.Cells[29, 1].Text, " ", 1));  //' 검진일
                THNV.strh자궁HName = VB.Trim(VB.Pstr(o.ActiveSheet.Cells[29, 1].Text, " ", 1));  //'검진 병원명
                THNV.strh간Date = VB.Trim(VB.Pstr(o.ActiveSheet.Cells[30, 1].Text, " ", 1));  //' 검진일
                THNV.strh간HName = VB.Trim(VB.Pstr(o.ActiveSheet.Cells[30, 1].Text, " ", 1));  //'검진 병원명
            }
            else
            {
                THNV.strhSName = "";
                THNV.strhJumin = "";         //'주민번호
                THNV.strhJaGubun = "";       //'자격구분
                THNV.strhJaSTS = "";         //'자격상태
                THNV.strhGKiho = "";         //'증기호
                THNV.strhKiho = "";          //'회사기호
                THNV.strhJisa = "";          //'소속지사
                THNV.strhGetDate = "";       //'취득일자
                THNV.strhBogen = "";         //'관할보건소
                
                //'검사정보
                THNV.strh1Cha = "";          //'1차검진
                THNV.strhEkg = "";           //'심전도
                THNV.strhLiver = "";         //'간염검사
                THNV.strh2ChaB = "";         //'2차B형
                THNV.strhCan1 = "";          //'암-위
                THNV.strhCan2 = "";          //'암-유방
                THNV.strhCan3 = "";          //'암-대장
                THNV.strhCan4 = "";          //'암-간
                THNV.strhCan5 = "";          //'암-자궁

                //'수검정보
                THNV.strh1ChaDate = "";      //'1차검진일자
                THNV.strh1ChaHName = "";     //'1차검진 병원명
                THNV.strh2ChaDate = "";      //'2차검진일자
                THNV.strh2ChaHName = "";     //'2차검진 병원명
                THNV.strhDentDate = "";     //'구강진일자
                THNV.strhDentHName = "";     //'구강검진 병원명


                THNV.strh위Date = ""      ;  //'위 검진일
                THNV.strh위HName = ""     ;  //'위검진 병원명
                THNV.strh대장Date = ""    ;  //' 검진일
                THNV.strh대장HName = ""   ;  //'검진 병원명
                THNV.strh유방Date = ""    ;  //' 검진일
                THNV.strh유방HName = ""   ;  //'검진 병원명
                THNV.strh자궁Date = ""    ;  //' 검진일
                THNV.strh자궁HName = ""   ;  //'검진 병원명
                THNV.strh간Date = ""      ;  //' 검진일
                THNV.strh간HName = "";  //'검진 병원명
            }
        }
    }    
}
