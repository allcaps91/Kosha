using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using ComLibB;

namespace ComSupLibB
{
    public class clsSupPthlDrug
    {
        public string GstrValue;                    //우편번호 Help용
        public string GstrLTD;                      //거래처코드 Help용
        public string[] GstrBuse = new string[50];  //약품출고처
        public string GstrWard;                     //Ward Code (약국:**,3A,3B,IU, ...)
        public string GstrKimsROWID;                //DRUG_KIMS ROWID
        public string GstrJepCode;                  //약품코드
        public string GstrFdate;
        public string GstrTdate;
        public string GstrJep;

        public string GstrBDATE;

        public struct Table_DRUG_JEP
        {
            public string JepCode;
            public string Bun;
            public string JepName;
            public string Unit;
            public string AdmDept;
            public int CovQty;
            public string CovUnit;
            public int Jqty1;
            public int Jqty2;
            public int Jqty3;
            public string GbRepeat;
            public string Lap;
            public string Sucode;
            public string GAE;
            public string GelCode;
            public string GbBal;
            public string GuipDrug;
        }

        public struct Table_DRUG_DAN
        {
            public string JepCode;
            public string GelCode;
            public string LastDate;
            public int Price1;
            public int Price2;
        }

        //입고,출고,출금내역
        public struct TABLE_DRUG_IPCH
        {
            public string InDate;
            public string GelCode;
            public string IpchGbn;
            public int SEQNO;
            public string JepCode;
            public string Gubun;
            public double BoxQty;           //포장단위의 수량
            public double Qty;              //입고,출고 수량
            public double Price;
            public double Amt;
            public double OldQty;           //수정전 수량
            public double OldAmt;           //수정전 금액
            public string OldIpchGbn;       //수정전 구분
            public string BuseCode;
            public double BalNo;
            public string ROWID;
            public string SoBun;
            public string GuipDrug;         //구입처코드
        }

        public struct TABLE_DRUG_SUBUL
        {

        }
    }
}
