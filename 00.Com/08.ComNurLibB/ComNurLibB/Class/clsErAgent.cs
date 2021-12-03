using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Data;
using Oracle.DataAccess.Client;
using ComDbB;
using ComBase;
using ComLibB;

namespace ComNurLibB
{
    public class clsErAgent
    {
        public static string READ_NEDISCODE(string ArgPano, string ArgBDate, string ArgBun, string ArgSucode, string ArgGbNgt, string argGbchild, int ArgAge, string ArgGbGisul, int ArgQty)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strBCode = "";
            //string strGIsul = "";

            try
            {
                //'표준코드읽기
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT  SUNEXT, ";
                SQL = SQL + ComNum.VBLF + "      BCODE,  TO_CHAR(B.EDIDATE,'YYYY-MM-DD') EDIDATE, ";
                SQL = SQL + ComNum.VBLF + "      OLDBCODE,  TO_CHAR(B.EDIDATE3,'YYYY-MM-DD') EDIDATE3,  ";
                SQL = SQL + ComNum.VBLF + "      B.BCODE3, TO_CHAR(B.EDIDATE4,'YYYY-MM-DD') EDIDATE4,  ";
                SQL = SQL + ComNum.VBLF + "      B.BCODE4,  TO_CHAR(B.EDIDATE5,'YYYY-MM-DD') EDIDATE5, ";
                SQL = SQL + ComNum.VBLF + "      B.BCODE5  ";
                SQL = SQL + ComNum.VBLF + "  FROM BAS_SUN B";
                SQL = SQL + ComNum.VBLF + " WHERE SUNEXT = '" + ArgSucode + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count >= 1)
                {
                    if (dt.Rows[0]["EdiDate"].ToString().Trim() == "" || string.Compare(ArgBDate, dt.Rows[0]["EdiDate"].ToString().Trim()) >= 0)
                    {
                        strBCode = dt.Rows[0]["BCode"].ToString().Trim();
                    }
                    else if (dt.Rows[0]["EdiDate3"].ToString().Trim() == "" || string.Compare(ArgBDate, dt.Rows[0]["EdiDate3"].ToString().Trim()) >= 0)
                    {
                        strBCode = dt.Rows[0]["OldBcode"].ToString().Trim();
                    }
                    else if (dt.Rows[0]["EdiDate4"].ToString().Trim() == "" || string.Compare(ArgBDate, dt.Rows[0]["EdiDate4"].ToString().Trim()) >= 0)
                    {
                        strBCode = dt.Rows[0]["Bcode3"].ToString().Trim();
                    }
                    else if (dt.Rows[0]["EdiDate5"].ToString().Trim() == "" || string.Compare(ArgBDate, dt.Rows[0]["EdiDate5"].ToString().Trim()) >= 0)
                    {
                        strBCode = dt.Rows[0]["Bcode4"].ToString().Trim();
                    }
                    else
                    {
                        strBCode = dt.Rows[0]["Bcode5"].ToString().Trim();
                    }
                }
                dt.Dispose();
                dt = null;


                if (strBCode == "99999999") //'비급여코드
                { }
                else
                {
                    //'수가변환                    
                    //'앞쪽 5자리 까지만      
                    if ((int)VB.Val(ArgBun) >= 28 && (int)VB.Val(ArgBun) <= 40)
                    {
                        //'처치,수술
                        rtnVal = CODE_08_Process(strBCode, ArgAge, ArgGbNgt, ArgGbGisul, ArgQty);
                    }
                    else if ((int)VB.Val(ArgBun) >= 41 && (int)VB.Val(ArgBun) <= 63)
                    {
                        //'검사
                        rtnVal = CODE_09_Process(strBCode, ArgAge, ArgGbNgt, ArgGbGisul, ArgBun);
                    }
                    else if ((int)VB.Val(ArgBun) >= 65 && (int)VB.Val(ArgBun) <= 70)
                    {
                        //'방사선
                        if (VB.Right(strBCode, 3) == "006")
                        {
                            strBCode = VB.Left(strBCode, 5);
                        }
                        rtnVal = CODE_10_Process(strBCode, ArgAge, ArgGbNgt, ArgGbGisul);
                    }
                    else if ((int)VB.Val(ArgBun) == 71)
                    {
                        //'초음파
                        if (ArgSucode == "US22")
                        {
                            rtnVal = "US001";
                        }
                        else
                        {
                            rtnVal = "US002";
                        }
                    }
                    else if ((int)VB.Val(ArgBun) == 72 || (int)VB.Val(ArgBun) == 73)
                    {
                        //'C/T, MRI
                        rtnVal = CODE_11_Process(strBCode, ArgAge, ArgGbNgt, ArgGbGisul);
                    }
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }


        //'처치및수술료
        public static string CODE_08_Process(string ArgCode, int nAge, string ArgGbNgt, string ArgGbGisul, int ArgQty)
        {
            string strPCode = "";
            strPCode = VB.Trim(ArgCode);

            //'주사수기료 소아가산(8세미만)
            if (nAge < 8 && VB.Trim(strPCode) == "KK052")
            {
                strPCode = VB.Left(strPCode + "00000000", 8);
                strPCode = VB.Left(strPCode, 5) + "3" + VB.Right(strPCode, 2);
            }
            else if (nAge < 8 && ArgGbGisul == "1" && strPCode != "JJJJJJ")
            {
                strPCode = VB.Left(strPCode + "00000000", 8);
                strPCode = VB.Left(strPCode, 5) + "3" + VB.Right(strPCode, 2);
            }

            //'부수술코드 SET
            if (ArgGbGisul == "1" && ArgQty == 0.5 && strPCode != "JJJJJJ")
            {
                switch (VB.Left(strPCode, 5))
                {
                    case "S5117":
                    case "R4275":
                        break;
                    default:
                        if (VB.Len(strPCode) < 8)
                        {
                            strPCode = VB.Left(strPCode + "00000000", 8);
                        }
                        strPCode = VB.Left(strPCode, 7) + "1";
                        break;
                }
            }

            //'수량이 0.5이고 부수술이면 *1
            if (ArgQty == 0.5 && strPCode != "JJJJJJ" && VB.Len(strPCode) != 8)
            {
                switch (VB.Left(strPCode, 5))
                {
                    case "S5117":
                    case "R4275":
                        break;
                    default:
                        if (string.Compare(ArgGbNgt, "5") >= 0)
                        {
                            strPCode = VB.Left(strPCode + "0000000", 7) + "1";
                        }
                        break;
                }
            }

            //'야간,공휴 SET
            if (strPCode != "JJJJJJ")
            {
                //'JJY 2005-01-22) 8 추가 (주간응급)
                //'If TDL.GbNgt < "0" Or TDL.GbNgt > "2" Then TDL.GbNgt = "0"
                if (string.Compare(ArgGbNgt, "0") < 0)
                {
                    ArgGbNgt = "0";
                }
                else if (string.Compare(ArgGbNgt, "9") > 0)
                {
                    ArgGbNgt = "0";
                }
                else if (ArgGbNgt == "3" || ArgGbNgt == "4" || ArgGbNgt == "5" || ArgGbNgt == "6" || ArgGbNgt == "7" || ArgGbNgt == "8")
                {
                    ArgGbNgt = "0";
                }

                if (ArgGbGisul == "1" && ArgGbNgt != "0")
                {
                    if (VB.Len(strPCode) < 8) strPCode = VB.Left(strPCode + "00000000", 8);
                    if (ArgGbNgt == "1") strPCode = VB.Left(strPCode, 6) + "5" + VB.Right(strPCode, 1); //'공휴
                    if (ArgGbNgt == "2") strPCode = VB.Left(strPCode, 6) + "1" + VB.Right(strPCode, 1); //'야간
                    if (ArgGbNgt == "6") strPCode = VB.Left(strPCode, 6) + "5" + VB.Right(strPCode, 1); //'공휴(부수술)
                    if (ArgGbNgt == "7") strPCode = VB.Left(strPCode, 6) + "1" + VB.Right(strPCode, 1); //'야간(부수술)
                    if (ArgGbNgt == "9") strPCode = VB.Left(strPCode, 6) + "2" + VB.Right(strPCode, 1); //'주간응급
                }
            }

            return strPCode;
            //'Call READ_EDI_SUGA(strPCode, TDL.Sunext)    
        }

        //'검사료
        public static string CODE_09_Process(string ArgCode, int nAge, string ArgGbNgt, string ArgGbGisul, string ArgBun)
        {
            string strPCode = "";
            strPCode = VB.Trim(ArgCode);

            //'내시경,천자,생검,순환기능검사시 소아가산 20%
            if (strPCode != "JJJJJJ")
            {
                if (ArgGbGisul == "1" && nAge < 8)
                {
                    if (VB.Left(strPCode, 5) == "FA145" || VB.Left(strPCode, 5) == "FA141" || VB.Left(strPCode, 5) == "FA144" || VB.Left(strPCode, 5) == "F6101")
                    {
                        strPCode = VB.Left(strPCode + "00000000", 8);
                        strPCode = VB.Left(strPCode, 5) + "6" + VB.Right(strPCode, 2);
                    }
                    else if (ArgBun != "41")
                    {
                        strPCode = VB.Left(strPCode + "00000000", 8);
                        strPCode = VB.Left(strPCode, 5) + "3" + VB.Right(strPCode, 2);
                    }
                    else //'핵의학검사 2000.7.1일 소아가산10% 신설
                    {
                        strPCode = VB.Left(strPCode + "00000000", 8);
                        strPCode = VB.Left(strPCode, 5) + "3" + VB.Right(strPCode, 2);
                    }
                }
            }

            return strPCode;
        }

        //'방사선
        public static string CODE_10_Process(string ArgCode, int nAge, string ArgGbNgt, string ArgGbGisul)
        {
            string strPCode = "";
            strPCode = VB.Trim(ArgCode);

            //'소아 20%가산 SET
            //'1999.11.15 방사선촬영 소아가산율 변경됨

            if (strPCode != "JJJJJJ")
            {
                if (nAge < 8)
                {
                    strPCode = VB.Left(strPCode + "00000000", 8);
                    strPCode = VB.Left(strPCode, 5) + "3" + VB.Right(strPCode, 2);
                }
                //'판독처리 ?
                //'If TDL.XrayRead = "1" Then
                //'     strPCode = VB.Left(strPCode + "000", 7) + "6"    '산정코드 3번째 6
                //'End If                
            }

            return strPCode;
        }

        //'CT, MRI
        public static string CODE_11_Process(string ArgCode, int nAge, string ArgGbNgt, string ArgGbGisul)
        {
            string strPCode = "";
            strPCode = VB.Trim(ArgCode);


            if (strPCode != "JJJJJJ")
            {
                if (nAge < 8)
                {
                    strPCode = VB.Left(strPCode + "00000000", 8);
                    strPCode = VB.Left(strPCode, 5) + "3" + VB.Right(strPCode, 2);
                }
                //'If TDL.XrayRead = "1" Then
                //'   strPCode = VB.Left(strPCode + "000", 7) + "6"    '산정코드 3번째 6
                //'End If                
            }
                
            return strPCode;
        }

    }
}
