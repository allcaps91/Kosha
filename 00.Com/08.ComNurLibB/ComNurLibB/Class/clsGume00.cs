using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComNurLibB
{
    class clsGume00
    {
        public struct TABLE_ORD_IPCH
        {
            public string strINDATE;
            public string strGELCODE;
            public string strIPCHGBN;
            public int intSEQNO        ;
            public string strJEPCODE   ;
            public string strGUBUN;
            public double dBOXQTY    ;    //'포장단위의 수량
            public double dQTY       ;    //'입고,출고 수량
            public double dPRICE     ;
            public double dAMT       ;
            public double dOLDQTY    ;    //'수정전 수량
            public double dOLDAMT    ;    //'수정전 금액
            public string strOLDIPCHGBN;    //'수정전 구분
            public string strBUSECODE  ;
            public string strPANO      ;
            public double dBALNO     ;
            public string strROWID     ;
            public string strGELGE     ;
            public string strREMARK    ;
            public string strGBTAX     ;    //'세금계산서 확정시 1, 미확정시 0
            public string strFEEDBACK;    //'피드백(부서전달사항)
             //'--------( 약국 조영제 청구관련 변수 )--------
            public string strGUIPDRUG ;
            public string strSOBUN    ;
            public string strWRTNO    ;
            public string strWRTNO_ETC;
        }

        public struct TABLE_ORD_SUBUL
        {
            public string strYYMM ;
            public string strJEPCODE;
            public string strBUSECODE;
            public double dIWOLQTY; //   '이월수량
            public double dIWOLAMT; //    '이월금액
            public double dIPGOQTY; //
            public double dIPGOAMT;
            public double dCHULQTY;
            public double dCHULAMT;
            public double dQTY;
            public double dAMT;
            public string strROWID;
            //'--------( 약국 조영제 청구관련 변수 )--------
            public double dIWOLCHAQTY;    //'차용 이월수량
            public double dIWOLCHAAMT;    //'차용 이월금액
            public double dCHAQTY ;
            public double dCHAAMT ;
            public double dJENGQTY;    //'차용정리수량
            public double dJENGAMT;    //'차용정리금액
        }

        public static TABLE_ORD_SUBUL TDS;
    }
}
