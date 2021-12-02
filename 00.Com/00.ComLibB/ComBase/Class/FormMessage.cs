using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ComBase
{
    /// <summary>
    /// 폼간에 정보를 전달할경우 사용함 : 부모, 자식 분간없이 왔다 갔다 할 수 있음
    /// </summary>
    public interface FormMessage
    {
        void ReciveFormClear();   //폼을 초기화 한다.

        //void ReciveSetData(AcpPatient pAcpP);  //환자정보를 세팅한다.

        void ReciveTodayLabChk(bool ChkVal);  //당일검사여부

        void ReciveOrdDate(string strOrdDate);  //처방일자

        void ReciveSetDataNew(string strAcpNo);  //환자정보를 세팅한다.

        string[] MessageGetOrderToday(); //당일처방

        string[] MessageGetOrderDise();  //당일상병

        string[] MessageGetDischargeOrder(); //퇴원약

    }
}
