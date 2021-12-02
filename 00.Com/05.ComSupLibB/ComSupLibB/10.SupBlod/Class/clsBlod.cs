using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComSupLibB.SupBlod
{
    /// <summary>혈액은행 공용</summary>
    public class clsBlod : Com.clsMethod
    {

        /// <summary>혈액정보</summary>
        public DataTable gDtBlodInfo;

        /// <summary>혈액정보생성자</summary>
        public clsBlod()
        {
            setDataTable();
        }

        void setDataTable()
        {           
            gDtBlodInfo = new DataTable();
            gDtBlodInfo.Columns.Add(new DataColumn("CODE"       , typeof(string)));
            gDtBlodInfo.Columns.Add(new DataColumn("NAME"       , typeof(string)));
            gDtBlodInfo.Columns.Add(new DataColumn("CODE_400"   , typeof(string)));
            gDtBlodInfo.Columns.Add(new DataColumn("CODE_320"   , typeof(string)));

            setDataTable_Row("","","","");                                           // 00
            setDataTable_Row("BT021", "P/C(농축적혈구)"      , "X2022"   , "X2021"); // 01
            setDataTable_Row("BT041", "FFP(신선동결혈장)"    , "X2042"   , "X2041"); // 02
            setDataTable_Row("BT023", "PLT/C(농축혈소판)"    , "X2082"   , "X2081"); // 03
            setDataTable_Row("BT011", "W/B(전혈)"            , "X1002"   , "X1001"); // 04
            setDataTable_Row("BT051", "Cyro(동결침전제제)"   , "X2062"   , "X2061"); // 05
            setDataTable_Row("BT071", "W/RBC(세척적혈구)"    , "X2112"   , "X2111"); // 06
            setDataTable_Row("BT101", "WBC/C(농축백혈구)"    , "X2101"   , "X2101"); // 07
            setDataTable_Row("BT31" , "PRP(혈소판풍부혈장)"  , "O2040"   , "O2040"); // 08
            setDataTable_Row("BT27" , "ph-P"                 , ""        , ""     ); // 09
            setDataTable_Row("BT24" , "ph-PLT"               , ""        , ""     ); // 10
            setDataTable_Row("BT25" , "ph-WBC"                , ""        , ""     ); // 11
            setDataTable_Row("BT26" , "ph-CB"                 , ""        , ""     ); // 12

            //2019-04-25 안정수 추가
            setDataTable_Row("BT081", "F/RBC(백혈구여과제거 적혈구)", "X2112", "X2112"); // 12

        }

        void setDataTable_Row(string code, string name, string c400, string c320 )
        {
            DataRow dr = null;

            dr = gDtBlodInfo.NewRow();
            dr["CODE"] = code;
            dr["NAME"] = name;
            dr["CODE_400"] = c400;
            dr["CODE_320"] = c320;

        }

        /// <summary>혈액번호를 변환</summary>
        public string Blood_NO_CONV(TextBox txtBloodNo1, TextBox TxtBloodNo)
        {
            // TODO : 2017.05.16.김홍록 : 텍스트 박스에 값 변환을 필 확인
            string Blood_NO_CONV = null;

            string strBloodNo1, strBloodNo2;

            if (string.IsNullOrEmpty(txtBloodNo1.Text)){
                return Blood_NO_CONV;
            }


            strBloodNo1 = "";
            strBloodNo1 = txtBloodNo1.Text.Replace("-", "").Replace(" ","");

            switch (strBloodNo1.Length)
            {
                case 10:
                    TxtBloodNo.Text = strBloodNo1.Substring(0, 9);
                    break;
                case 12:
                    TxtBloodNo.Text = strBloodNo1.Substring(0, 9);
                    break;
                case 13:
                    TxtBloodNo.Text = strBloodNo1.Substring(1, 9);
                    break;
                default:
                    TxtBloodNo.Text = "";
                    txtBloodNo1.Text = "";
                    Blood_NO_CONV = "ERROR";

                    return Blood_NO_CONV;
            }


            strBloodNo1 = TxtBloodNo.Text;

            strBloodNo2 = "";
            strBloodNo2 = strBloodNo1.Substring(1) + "-";
            strBloodNo2 += strBloodNo1.Substring(2, 2) + "-";
            strBloodNo2 += strBloodNo1.Substring(4, 6);

            txtBloodNo1.Text = strBloodNo2;

            Blood_NO_CONV = strBloodNo2;

            return Blood_NO_CONV;
        }
    }
}
