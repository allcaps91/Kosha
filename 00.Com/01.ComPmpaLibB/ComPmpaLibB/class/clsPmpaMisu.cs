using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스
using ComDbB; //DB연결

/// <summary>
/// Description : 미수업무 관련 Class
/// Author : 김민철
/// Create Date : 2017.09.14
/// </summary>
/// <history>
/// </history>
namespace ComPmpaLibB
{
    public class clsPmpaMisu
    {
        //clsPmpaPb cPP = new clsPmpaPb();

        //string SQL = "";
        //string SqlErr = ""; //에러문 받는 변수

        public string[] GstrMisuGye = new string[17];
        public string[] GstrMisuGye_TA = new string[19];
        public string GstrClass = "";                                //미수 종류
        public string[] GstrMisuBun = new string[22];             //   '미수분류
        public string[] GstrGels = new string[61];
        public static string GstrTag = "";                             // AS-IS    Me.Tag

        /// <summary>
        /// Description : 개인미수 구분명
        /// Author      : 김민철
        /// Create Date : 2018.04.17
        /// </summary>
        /// <param name="Arg1"></param>
        /// <returns></returns>
        public string READ_MisuGye(string Arg1)
        {
            int ArgInx = 0;
            string ArgReturn = "";

            GstrMisuGye[1] = "01.가퇴원";
            GstrMisuGye[2] = "02.업무착오";
            GstrMisuGye[3] = "03.탈원";
            GstrMisuGye[4] = "04.지불각서";
            GstrMisuGye[5] = "05.응급실";
            GstrMisuGye[6] = "06.외래";
            GstrMisuGye[7] = "07.청구심사";
            GstrMisuGye[8] = "08.책임보험";
            GstrMisuGye[9] = "09.퇴원";
            GstrMisuGye[10] = "10.기타미수";
            GstrMisuGye[11] = "11.기관청구";
            GstrMisuGye[12] = "12.입원정밀";
            GstrMisuGye[13] = "13.필수접종국가지원";
            GstrMisuGye[14] = "14.회사접종";
            GstrMisuGye[15] = "15.금연처방";
            GstrMisuGye[16] = "";
            
            ArgReturn = "";

            for (ArgInx = 1; ArgInx < 17; ArgInx++)
            {
                if (VB.Left(GstrMisuGye[ArgInx], 2) == Arg1)
                {
                    ArgReturn = (VB.Mid(GstrMisuGye[ArgInx], 4, 17)).Trim();
                    break;
                }

            }
            return ArgReturn;
        }

        /// <summary>
        /// Author : 김효성
        /// Create Date : 2017.09.14
        /// </summary>
        /// <param name="Arg1"></param>
        /// <param name="misuta">사용할 시 true </param>
        /// <returns></returns>
        public string READ_MisuGye_TA(string Arg1, bool misuta = false)
        {
            int ArgInx = 0;
            string ArgReturn = "";

            GstrMisuGye_TA[1] = "11.처음청구";
            GstrMisuGye_TA[2] = "12.정산청구";
            GstrMisuGye_TA[3] = "13.재청구";
            GstrMisuGye_TA[4] = "14.추가청구";
            GstrMisuGye_TA[5] = "15.이의신청";
            GstrMisuGye_TA[6] = "19.기타미수";
            GstrMisuGye_TA[7] = "21.입금";
            GstrMisuGye_TA[8] = "22.정산입금";
            GstrMisuGye_TA[9] = "23.주민입금";
            GstrMisuGye_TA[10] = "24.이의입금";
            GstrMisuGye_TA[11] = "25.기타수입";
            GstrMisuGye_TA[12] = "26.심사중입금";
            GstrMisuGye_TA[13] = "31.삭감";
            GstrMisuGye_TA[14] = "32.반송";
            GstrMisuGye_TA[15] = "33.과지급금";
            GstrMisuGye_TA[16] = "34.계산착오";
            GstrMisuGye_TA[17] = "35.삭감절산";
            GstrMisuGye_TA[18] = "**.참고사항";

            if (VB.Val(Arg1) == 9)
            {
                ArgReturn = "지급보류";
                return ArgReturn;
            }

            if (misuta == true)
            {
                switch (Arg1)
                {
                    case "1":
                        ArgReturn = "1차검토액";
                        break;
                    case "2":
                        ArgReturn = "분쟁금액";
                        break;
                    case "3":
                        ArgReturn = "분쟁결과";
                        break;
                }
                return ArgReturn;
            }


            if (VB.Val(Arg1) > 0 && VB.Val(Arg1) < 10)
            {
                ArgReturn = "참고";
                return ArgReturn;
            }
            if (VB.Val(Arg1) == 10)
            {
                ArgReturn = "심사중";
                return ArgReturn;
            }

            ArgReturn = "";
            for (ArgInx = 1; ArgInx <= 18; ArgInx++)
            {
                if (VB.Left(GstrMisuGye_TA[ArgInx], 2) == Arg1)
                {
                    ArgReturn = (VB.Mid(GstrMisuGye_TA[ArgInx] + VB.Space(20), 4, 15)).Trim();
                    break;
                }

            }
            return ArgReturn;
        }

        /// <summary>
        /// Author : 김효성
        /// Create Date : 2017.09.14
        /// </summary>
        /// <param name="Arg1"></param>
        /// <returns></returns>
        public string READ_MisuIpdOpd(string Arg1)
        {
            string ArgReturn = "";

            ArgReturn = "";

            if (Arg1 == "O")
                ArgReturn = "외래";
            else if (Arg1 == "I")
                ArgReturn = "입원";

            return ArgReturn;
        }

        /// <summary>
        /// Author : 김효성
        /// Create Date : 2017.09.14
        /// </summary>
        /// <param name="Arg1"></param>
        /// <returns></returns>
        public string YYMM_SET(string Arg1)
        {
            int ArgYY = 0;
            int ArgMM = 0;
            string ArgReturn = "";

            if (VB.Len(Arg1) != 10)
            {
                ArgReturn = "";
                return ArgReturn;
            }
            ArgYY = (int)VB.Val(VB.Left(Arg1, 4));
            ArgMM = (int)VB.Val(ComFunc.MidH(Arg1, 6, 2));
            if (string.Compare(GstrClass, "07") < 0)
            {
                ArgMM = ArgMM - 1;
            }
            if (ArgMM == 0)
            {
                ArgYY = ArgYY - 1;
                ArgMM = 12;
            }
            ArgReturn = (ArgYY.ToString("0000")) + (ArgMM.ToString("00"));
            return ArgReturn;
        }

        public string READ_MisuBunya(string Arg1)
        {
            int i = 0;
            string ArgReturn = "";

            GstrMisuBun[1] = "01.내과분야";
            GstrMisuBun[2] = "02.외과분야";
            GstrMisuBun[3] = "03.산소아과";
            GstrMisuBun[4] = "04.안.이비";
            GstrMisuBun[5] = "05.피.비뇨";
            GstrMisuBun[6] = "06.치과";
            GstrMisuBun[7] = "07.NP정액";
            GstrMisuBun[8] = "08.장애대불";
            GstrMisuBun[9] = "09.가정간호";
            GstrMisuBun[10] = "10.재청구";
            GstrMisuBun[11] = "11.이의신청";
            GstrMisuBun[12] = "12.정산진료비";
            GstrMisuBun[13] = "13.추가청구";
            GstrMisuBun[14] = "14.NP장애대불";
            GstrMisuBun[15] = "15.HD정액";
            GstrMisuBun[16] = "19.기타청구";
            GstrMisuBun[17] = "20.상한대불";
            GstrMisuBun[18] = "21.희귀지원금";
            GstrMisuBun[19] = "22.결핵지원금";
            GstrMisuBun[20] = "23.DRG(포괄수가)";
            GstrMisuBun[21] = "24.100/100 미만";

            ArgReturn = "";

            for (i = 1; i <= 21; i++)
            {
                if (VB.Left(GstrMisuBun[i], 2) == Arg1)
                {
                    ArgReturn = VB.Mid(GstrMisuBun[i] + VB.Space(23), 4, 23).Trim();
                    break;
                }
            }

            return ArgReturn;
        }

        /// <summary>
        /// Author : 박창욱
        /// Create Date : 2017.09.14
        /// </summary>
        public void TMM_Clear_Rtn()
        {
            int i = 0;

            //원본 MISU_IDMST (OLD)
            clsPmpaType.TMM.WRTNO = 0;
            clsPmpaType.TMM.MisuID = "";
            clsPmpaType.TMM.BDate = "";
            clsPmpaType.TMM.Class = "";
            clsPmpaType.TMM.YYMM = "";
            clsPmpaType.TMM.IpdOpd = "";
            clsPmpaType.TMM.Bi = "";
            clsPmpaType.TMM.GelCode = "";
            clsPmpaType.TMM.Bun = "";
            clsPmpaType.TMM.FromDate = "";
            clsPmpaType.TMM.ToDate = "";
            clsPmpaType.TMM.DeptCode = "";
            clsPmpaType.TMM.MgrRank = "";
            clsPmpaType.TMM.GbEnd = "";
            clsPmpaType.TMM.Remark = "";
            clsPmpaType.TMM.Qty = new int[5];
            clsPmpaType.TMM.Amt = new double[9];
            for (i = 1; i <= 4; i++)
            {
                clsPmpaType.TMM.Qty[i] = 0;
            }
            for (i = 1; i <= 8; i++)
            {
                clsPmpaType.TMM.Amt[i] = 0;
            }
            clsPmpaType.TMM.ROWID = "";
            clsPmpaType.TMM.Ilsu = 0;
            clsPmpaType.TMM.JAmt = 0;
            clsPmpaType.TMM.EntDate = "";
            clsPmpaType.TMM.EntPart = 0;
            clsPmpaType.TMM.JepsuNo = "";
            clsPmpaType.TMM.DrCode = "";


            //변경후 MISU_IDMST (NEW)
            clsPmpaType.TMN.WRTNO = 0;
            clsPmpaType.TMN.MisuID = "";
            clsPmpaType.TMN.BDate = "";
            clsPmpaType.TMN.Class = "";
            clsPmpaType.TMN.YYMM = "";
            clsPmpaType.TMN.IpdOpd = "";
            clsPmpaType.TMN.Bi = "";
            clsPmpaType.TMN.GelCode = "";
            clsPmpaType.TMN.Bun = "";
            clsPmpaType.TMN.FromDate = "";
            clsPmpaType.TMN.ToDate = "";
            clsPmpaType.TMN.DeptCode = "";
            clsPmpaType.TMN.MgrRank = "";
            clsPmpaType.TMN.GbEnd = "";
            clsPmpaType.TMN.Remark = "";
            clsPmpaType.TMN.Qty = new int[5];
            clsPmpaType.TMN.Amt = new double[9];
            for (i = 1; i <= 4; i++)
            {
                clsPmpaType.TMN.Qty[i] = 0;
            }
            for (i = 1; i <= 8; i++)
            {
                clsPmpaType.TMN.Amt[i] = 0;
            }
            clsPmpaType.TMN.ROWID = "";
            clsPmpaType.TMN.Ilsu = 0;
            clsPmpaType.TMN.JAmt = 0;
            clsPmpaType.TMN.JepsuNo = "";
            clsPmpaType.TMN.DrCode = "";

            //MISU_SLIP
            clsPmpaType.TMS.Gubun = new string[201];
            clsPmpaType.TMS.Qty = new int[201];
            clsPmpaType.TMS.TAmt = new double[201];
            clsPmpaType.TMS.Amt = new double[201];
            clsPmpaType.TMS.Del = new string[201];
            for (i = 1; i <= 200; i++)
            {
                clsPmpaType.TMS.Gubun[i] = "";
                clsPmpaType.TMS.Qty[i] = 0;
                clsPmpaType.TMS.TAmt[i] = 0;
                clsPmpaType.TMS.Amt[i] = 0;
                clsPmpaType.TMS.Del[i] = "";
            }
        }

        /// <summary>
        /// Author : 박창욱
        /// Create Date : 2017.09.14
        /// </summary>
        /// <param name="argWRTNO">WRTNO</param>
        public void READ_MISU_IDMST(long argWRTNO)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            TMM_Clear_Rtn();

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT MisuID, TO_CHAR(Bdate,'YYYY-MM-DD') Bdate, Class,";
                SQL += ComNum.VBLF + "        IpdOpd, Bi, GelCode,";
                SQL += ComNum.VBLF + "        Bun,TO_CHAR(FromDate,'YYYY-MM-DD') Fromdate, TO_CHAR(ToDate,'YYYY-MM-DD') Todate,";
                SQL += ComNum.VBLF + "        Ilsu, DeptCode, MgrRank,";
                SQL += ComNum.VBLF + "        Qty1, Qty2, Qty3,";
                SQL += ComNum.VBLF + "        Qty4, Amt1, Amt2,";
                SQL += ComNum.VBLF + "        Amt3, Amt4, Amt5,";
                SQL += ComNum.VBLF + "        Amt6, Amt7, Amt8,";
                SQL += ComNum.VBLF + "        JepsuNo, GbEnd, Remark,";
                SQL += ComNum.VBLF + "        ROWID, TO_CHAR(EntDate,'YYYY-MM-DD HH24:MI') EntDate, EntPart,";
                SQL += ComNum.VBLF + "        TongGbn, MirYYMM, EdiMirNo,";
                SQL += ComNum.VBLF + "        ChaSu, MukNo, DrCode,";
                SQL += ComNum.VBLF + "        Gubun, TO_CHAR(TDATE,'YYYY-MM-DD') TDATE, TO_CHAR(JDATE,'YYYY-MM-DD') JDATE,";
                SQL += ComNum.VBLF + "        CARNO, DRIVER, COPNAME";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_IDMST";
                SQL += ComNum.VBLF + "  WHERE 1 = 1";
                SQL += ComNum.VBLF + "    AND WRTNO = " + argWRTNO + "";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    clsPmpaType.TMM.WRTNO = argWRTNO;
                    clsPmpaType.TMM.ROWID = "";
                }

                clsPmpaType.TMM.WRTNO = argWRTNO;
                clsPmpaType.TMM.MisuID = dt.Rows[0]["MisuID"].ToString().Trim();

                clsPmpaType.TMM.BDate = dt.Rows[0]["Bdate"].ToString().Trim();
                clsPmpaType.TMM.Class = dt.Rows[0]["Class"].ToString().Trim();
                clsPmpaType.TMM.IpdOpd = dt.Rows[0]["IpdOpd"].ToString().Trim();
                clsPmpaType.TMM.Bi = dt.Rows[0]["Bi"].ToString().Trim();
                clsPmpaType.TMM.GelCode = dt.Rows[0]["GelCode"].ToString().Trim();
                clsPmpaType.TMM.Bun = dt.Rows[0]["Bun"].ToString().Trim();
                clsPmpaType.TMM.FromDate = dt.Rows[0]["Fromdate"].ToString().Trim();
                clsPmpaType.TMM.ToDate = dt.Rows[0]["Todate"].ToString().Trim();
                clsPmpaType.TMM.Ilsu = (int)VB.Val(dt.Rows[0]["Ilsu"].ToString().Trim());
                clsPmpaType.TMM.DeptCode = dt.Rows[0]["DeptCode"].ToString().Trim();
                clsPmpaType.TMM.MgrRank = dt.Rows[0]["MgrRank"].ToString().Trim();
                clsPmpaType.TMM.Qty[1] = (int)VB.Val(dt.Rows[0]["Qty1"].ToString().Trim());
                clsPmpaType.TMM.Qty[2] = (int)VB.Val(dt.Rows[0]["Qty2"].ToString().Trim());
                clsPmpaType.TMM.Qty[3] = (int)VB.Val(dt.Rows[0]["Qty3"].ToString().Trim());
                clsPmpaType.TMM.Qty[4] = (int)VB.Val(dt.Rows[0]["Qty4"].ToString().Trim());
                clsPmpaType.TMM.Amt[1] = VB.Val(dt.Rows[0]["Amt1"].ToString().Trim());
                clsPmpaType.TMM.Amt[2] = VB.Val(dt.Rows[0]["Amt2"].ToString().Trim());
                clsPmpaType.TMM.Amt[3] = VB.Val(dt.Rows[0]["Amt3"].ToString().Trim());
                clsPmpaType.TMM.Amt[4] = VB.Val(dt.Rows[0]["Amt4"].ToString().Trim());
                clsPmpaType.TMM.Amt[5] = VB.Val(dt.Rows[0]["Amt5"].ToString().Trim());
                clsPmpaType.TMM.Amt[6] = VB.Val(dt.Rows[0]["Amt6"].ToString().Trim());
                clsPmpaType.TMM.Amt[7] = VB.Val(dt.Rows[0]["Amt7"].ToString().Trim());
                clsPmpaType.TMM.Amt[8] = VB.Val(dt.Rows[0]["Amt8"].ToString().Trim());
                clsPmpaType.TMM.ROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                clsPmpaType.TMM.Remark = dt.Rows[0]["Remark"].ToString().Trim();
                clsPmpaType.TMM.JepsuNo = dt.Rows[0]["JepsuNo"].ToString().Trim();
                clsPmpaType.TMM.GbEnd = dt.Rows[0]["GbEnd"].ToString().Trim();
                clsPmpaType.TMM.EntDate = dt.Rows[0]["EntDate"].ToString().Trim();
                clsPmpaType.TMM.EntPart = (long)VB.Val(dt.Rows[0]["EntPart"].ToString().Trim());
                clsPmpaType.TMM.TongGbn = dt.Rows[0]["TongGbn"].ToString().Trim();
                clsPmpaType.TMM.MirYYMM = dt.Rows[0]["MirYYMM"].ToString().Trim();
                clsPmpaType.TMM.EdiMirNo = dt.Rows[0]["EdiMirNo"].ToString().Trim();
                clsPmpaType.TMM.ChaSu = dt.Rows[0]["ChaSu"].ToString().Trim();
                clsPmpaType.TMM.MukNo = dt.Rows[0]["MukNo"].ToString().Trim();
                clsPmpaType.TMM.DrCode = dt.Rows[0]["DrCode"].ToString().Trim();
                clsPmpaType.TMM.TDATE = dt.Rows[0]["TDATE"].ToString().Trim();
                clsPmpaType.TMM.JDATE = dt.Rows[0]["JDATE"].ToString().Trim();
                clsPmpaType.TMM.CARNO = dt.Rows[0]["CARNO"].ToString().Trim();
                clsPmpaType.TMM.DRIVER = dt.Rows[0]["DRIVER"].ToString().Trim();
                clsPmpaType.TMM.COPNAME = dt.Rows[0]["COPNAME"].ToString().Trim();
                clsPmpaType.TMM.Gubun = dt.Rows[0]["Gubun"].ToString().Trim();
                clsPmpaType.TMM.JAmt = clsPmpaType.TMM.Amt[2];
                clsPmpaType.TMM.DrCode = dt.Rows[0]["DrCode"].ToString().Trim();
                for (i = 3; i <= 8; i++)
                {
                    clsPmpaType.TMM.JAmt = clsPmpaType.TMM.JAmt - clsPmpaType.TMM.Amt[i];
                }

                dt.Dispose();
                dt = null;

                //Update시 비교용에 별도보관
                clsPmpaType.TMN.WRTNO = clsPmpaType.TMM.WRTNO;
                clsPmpaType.TMN.MisuID = clsPmpaType.TMM.MisuID;
                clsPmpaType.TMN.BDate = clsPmpaType.TMM.BDate;
                clsPmpaType.TMN.Class = clsPmpaType.TMM.Class;
                clsPmpaType.TMN.YYMM = clsPmpaType.TMM.YYMM;
                clsPmpaType.TMN.IpdOpd = clsPmpaType.TMM.IpdOpd;
                clsPmpaType.TMN.Bi = clsPmpaType.TMM.Bi;
                clsPmpaType.TMN.GelCode = clsPmpaType.TMM.GelCode;
                clsPmpaType.TMN.Bun = clsPmpaType.TMM.Bun;
                clsPmpaType.TMN.FromDate = clsPmpaType.TMM.FromDate;
                clsPmpaType.TMN.ToDate = clsPmpaType.TMM.ToDate;
                clsPmpaType.TMN.DeptCode = clsPmpaType.TMM.DeptCode;
                clsPmpaType.TMN.MgrRank = clsPmpaType.TMM.MgrRank;
                clsPmpaType.TMN.GbEnd = clsPmpaType.TMM.GbEnd;
                clsPmpaType.TMN.Remark = clsPmpaType.TMM.Remark;
                clsPmpaType.TMN.ROWID = clsPmpaType.TMM.ROWID;
                clsPmpaType.TMN.MirYYMM = clsPmpaType.TMM.MirYYMM;
                clsPmpaType.TMN.JepsuNo = clsPmpaType.TMM.JepsuNo;
                clsPmpaType.TMN.EdiMirNo = clsPmpaType.TMM.EdiMirNo;
                clsPmpaType.TMN.ChaSu = clsPmpaType.TMM.ChaSu;
                clsPmpaType.TMN.MukNo = clsPmpaType.TMM.MukNo;
                clsPmpaType.TMN.TongGbn = clsPmpaType.TMM.TongGbn;
                clsPmpaType.TMN.TDATE = clsPmpaType.TMM.TDATE;
                clsPmpaType.TMN.JDATE = clsPmpaType.TMM.JDATE;
                clsPmpaType.TMN.CARNO = clsPmpaType.TMM.CARNO;
                clsPmpaType.TMN.DRIVER = clsPmpaType.TMM.DRIVER;
                clsPmpaType.TMN.COPNAME = clsPmpaType.TMM.COPNAME;
                clsPmpaType.TMN.Gubun = clsPmpaType.TMM.Gubun;
                clsPmpaType.TMN.DrCode = clsPmpaType.TMM.DrCode;
                for (i = 1; i <= 3; i++)
                {
                    clsPmpaType.TMN.Qty[i] = clsPmpaType.TMM.Qty[i];
                }
                for (i = 1; i <= 8; i++)
                {
                    clsPmpaType.TMN.Amt[i] = clsPmpaType.TMM.Amt[i];
                }
                clsPmpaType.TMN.Ilsu = clsPmpaType.TMM.Ilsu;
                clsPmpaType.TMN.JAmt = clsPmpaType.TMM.JAmt;

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        /// <summary>
        /// Author : 박창욱
        /// Create Date : 2017.09.14
        /// </summary>
        /// <param name="argCode">Code</param>
        /// <returns></returns>
        public string READ_BAS_MIA(string argCode)
        {
            string rtnVal = "";

            if (argCode.Trim() == "")
            {
                rtnVal = "계약코드 미등록";
                return rtnVal;
            }

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = " SELECT MiaName";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_MIA";
                SQL = SQL + ComNum.VBLF + "  WHERE MiaCode = '" + argCode.Trim() + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    rtnVal = "계약코드 미등록";
                    return rtnVal;
                }
                else
                {
                    rtnVal = dt.Rows[0]["MiaName"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }
        
        /// <summary>
        /// Author : 김효성
        /// Create Date : 2017.09.14
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public string READ_TRANS_JOBNAME(string arg)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string strVal = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT B.NAME, A.KORNAME";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.INSA_MST A, ADMIN.BAS_BUSE B";
                SQL = SQL + ComNum.VBLF + " WHERE 1=1";
                SQL = SQL + ComNum.VBLF + "   AND A.BUSE = B.BUCODE";
                SQL = SQL + ComNum.VBLF + "   AND A.SABUN = '" + VB.Val(arg).ToString("00000") + "' ";

                SQL = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return strVal;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return strVal;

                }

                strVal = dt.Rows[0]["NAME"].ToString().Trim() + "/" + dt.Rows[0]["KORNAME"].ToString().Trim();

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            return strVal;
        }

        /// <summary>
        /// Author : 박창욱
        /// Create Date : 2017.09.20
        /// </summary>
        /// <param name="strArg"></param>
        /// <returns></returns>
        public string READ_MisuClass(string strArg)
        {
            string rtnVal = "";
            int i = 0;

            Get_MISU_BasBcode_ToArray(clsDB.DbCon, "MISU_CLASS", clsPmpaPb.GstrMisuClass);

            for (i = 0; i < 18; i++)
            {
                if (VB.Left(clsPmpaPb.GstrMisuClass[i], 2) == strArg)
                {
                    rtnVal = VB.Mid(clsPmpaPb.GstrMisuClass[i] + VB.Space(23), 4, 20).Trim();
                    return rtnVal;
                }
            }
            return rtnVal;
        }

        /// <summary>
        /// Author : 박창욱
        /// Create Date : 2017.09.20
        /// </summary>
        /// <param name="argCode"></param>
        /// <returns></returns>
        public string READ_BuseName(string argCode)
        {
            string rtnVal = "";

            switch (argCode)
            {
                case "1":
                    rtnVal = "외래수납";
                    break;
                case "2":
                    rtnVal = "응급실";
                    break;
                case "3":
                    rtnVal = "입원수납";
                    break;
                case "4":
                    rtnVal = "심사계";
                    break;
                case "5":
                    rtnVal = "원무과";
                    break;
                default:
                    rtnVal = "";
                    break;
            }
            return rtnVal;
        }

        /// <summary>
        /// Author : 박창욱
        /// Create Date : 2017.09.21
        /// </summary>
        /// <param name="strArg"></param>
        /// <returns></returns>
        public string READ_PerMisuGye(string strArg)
        {
            string rtnVal = "";

            switch (VB.Val(strArg).ToString("00"))
            {
                case "01":
                    rtnVal = "가퇴원미수";
                    break;
                case "02":
                    rtnVal = "업무착오미수";
                    break;
                case "03":
                    rtnVal = "탈원미수";
                    break;
                case "04":
                    rtnVal = "지불각서";
                    break;
                case "05":
                    rtnVal = "응급미수";
                    break;
                case "06":
                    rtnVal = "외래미수";
                    break;
                case "07":
                    rtnVal = "심사청구미수";
                    break;
                case "08":
                    rtnVal = "책임보험";
                    break;
                case "09":
                    rtnVal = "퇴원";
                    break;
                case "10":
                    rtnVal = "기타";
                    break;
                case "11":
                    rtnVal = "기관청구미수";
                    break;
                case "12":
                    rtnVal = "입원정밀";
                    break;
                case "13":
                    rtnVal = "필수접종국가지원";
                    break;
                case "14":
                    rtnVal = "회사접종";
                    break;
                case "15":
                    rtnVal = "금연처방";
                    break;
                default:
                    rtnVal = "";
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// Description : 미수마스타의 분류명칭
        /// Author : 박창욱
        /// Create Date : 2017.09.25
        /// </summary>
        /// <param name="argBun">미수 분류번호</param>
        /// <returns></returns>
        public string READ_MisuBunName(string argBun)
        {
            string rtnVal = "";

            switch (argBun)
            {
                case "01":
                    rtnVal = "내과분야";
                    break;
                case "02":
                    rtnVal = "외과분야";
                    break;
                case "03":
                    rtnVal = "산소아과";
                    break;
                case "04":
                    rtnVal = "피.비뇨기";
                    break;
                case "05":
                    rtnVal = "안.이비인";
                    break;
                case "06":
                    rtnVal = "치과";
                    break;
                case "07":
                    rtnVal = "보호정신과";
                    break;
                case "08":
                    rtnVal = "장애대불";
                    break;
                case "09":
                    rtnVal = "가정간호";
                    break;
                case "10":
                    rtnVal = "재청구";
                    break;
                case "11":
                    rtnVal = "이의신청";
                    break;
                case "12":
                    rtnVal = "정산진료비";
                    break;
                case "13":
                    rtnVal = "추가청구";
                    break;
                case "14":
                    rtnVal = "NP장애대불";
                    break;
                case "19":
                    rtnVal = "기타청구";
                    break;
                case "20":
                    rtnVal = "상한대불";
                    break;
                case "21":
                    rtnVal = "지원금";
                    break;
                case "22":
                    rtnVal = "약제상한";
                    break;
                case "23":  // 2020-09-22
                    rtnVal = "DRG";
                    break;
                case "24": // 2020-09-22
                    rtnVal = "100/100";
                    break;
                case "25": // 2020-09-22
                    rtnVal = "국가재난지원";
                    break;
                default:
                    rtnVal = "";
                    break;
            }
            return rtnVal;
        }

        /// <summary>
        /// Description : 환자종류로 수입통계에서 사용하는 환자구분으로 변환
        /// Author : 김효성
        /// </summary>
        /// <param name="ArgBi"></param>
        /// <param name="ArgJobDate"></param>
        /// <returns></returns>
        public int READ_Bi_SuipTong(string ArgBi, string ArgJobDate)
        {
            int ArgBiNo = 0;

            if (Convert.ToDateTime(ArgJobDate) >= Convert.ToDateTime("2003-11-03"))
                switch (ArgBi)
                {
                    case "11":
                    case "12":
                    case "13":
                    case "32":
                    case "41":
                    case "42":
                    case "43":
                    case "44":
                        ArgBiNo = 1; //'보험
                        break;
                    case "21":
                    case "22":
                    case "23":
                    case "24":
                        ArgBiNo = 2; //'보호
                        break;
                    case "31":
                    case "33":
                        ArgBiNo = 3; //'산재
                        break;
                    case "52":
                        ArgBiNo = 4; //'자보
                        break;
                    default:
                        ArgBiNo = 5; //'일반
                        break;
                }
            else
            {
                switch (ArgBi)
                {
                    case "11":
                    case "12":
                    case "13":
                    case "41":
                    case "42":
                    case "43":
                    case "44":
                        ArgBiNo = 1;// '보험
                        break;
                    case "21":
                    case "22":
                    case "23":
                    case "24":
                        ArgBiNo = 2; //'보호
                        break;
                    case "31":
                    case "32":
                    case "33":
                        ArgBiNo = 3;// '산재
                        break;
                    case "52":
                        ArgBiNo = 4; //'자보
                        break;
                    default:
                        ArgBiNo = 5;// '일반
                        break;
                }
            }
            return ArgBiNo;
        }

        /// <summary>
        /// Description : DATE_ADD
        /// Author : 김효성
        /// </summary>
        /// <param name="ArDate"></param>
        /// <param name="ArgIlsu"></param>
        /// <returns></returns>
        public string DATE_ADD(PsmhDb pDbCon, string ArDate, int ArgIlsu)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string strVal = "";

            if (VB.Len(ArDate) != 10)
            {
                return strVal;
            }


            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(TO_DATE('" + ArDate + "','YYYY-MM-DD')";

                if (ArgIlsu < 0)
                {
                    SQL = SQL + ComNum.VBLF + "-" + ArgIlsu * -1;
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "+" + ArgIlsu;
                }
                SQL = SQL + ComNum.VBLF + ",'YYYY-MM-DD') AddDate FROM DUAL";

                SQL = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return strVal;
                }
                if (dt.Rows.Count == 1)
                {
                    strVal = dt.Rows[0]["AddDate"].ToString().Trim();

                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return strVal;
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            return strVal;
        }

        /// <summary>
        /// Description : BAS_환자주소일부_안내
        /// Author : 김효성
        /// </summary>
        /// <param name="ArDate"></param>
        /// <param name="ArgIlsu"></param>
        /// <returns></returns>
        public string BAS_JUSO_Gaide(PsmhDb pDbCon, string ArgPano)
        {
            DataTable dt = null;
            DataTable dtFn = null;
            string strJusoDetail = "";
            string strZipCode = "";
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string strVal = "";

            try
            {
                SQL = " SELECT ";
                SQL = SQL + ComNum.VBLF + "ZipCode1 || ZipCode2 ZipCode, Juso ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + "   WHERE Pano ='" + ArgPano + "' ";

                SQL = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return strVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strZipCode = dt.Rows[0]["ZipCode"].ToString().Trim();
                    strJusoDetail = dt.Rows[0]["Juso"].ToString().Trim();
                }
                else
                {
                    dt.Dispose();
                    dt = null;
                    return strVal;
                }
                dt.Dispose();
                dt = null;


                SQL = "SELECT MailJuso ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_MailNew ";
                SQL = SQL + ComNum.VBLF + " WHERE  MailCode='" + strZipCode + "' ";

                SQL = clsDB.GetDataTable(ref dtFn, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return strVal;
                }
                if (dtFn.Rows.Count > 0)
                {
                    strVal = dtFn.Rows[0]["MailJuso"].ToString().Trim();
                }

                dtFn.Dispose();
                dtFn = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            return strVal;
        }

        public string Misu_Gubun_Chk(PsmhDb pDbCon, string argClass, string argPano)
        {
            string rtnVal = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT GUBUN";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_IDMST ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND MisuID ='" + argPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND CLASS ='" + argClass + "' ";
                SQL = SQL + ComNum.VBLF + "   AND Gubun > '00' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY BDate DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["Gubun"].ToString().Trim() + "." + Misu_Gubun_NameChk(pDbCon, argClass, dt.Rows[0]["Gubun"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        public string Misu_Gubun_NameChk(PsmhDb pDbCon, string argClass, string argGubun)
        {
            string rtnVal = "";

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT NAME";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND GUBUN ='계약처미수_지역구분'";
                SQL = SQL + ComNum.VBLF + "   AND SUBSTR(CODE,1,2) ='" + argClass + "'";
                SQL = SQL + ComNum.VBLF + "   AND SUBSTR(CODE,3,2) ='" + argGubun + "'";
                SQL = SQL + ComNum.VBLF + " ORDER BY CODE";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        rtnVal = dt.Rows[i]["Name"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        /// <summary>
        /// 미수 BAS_BCODE TABLE DATA를 가지고 와서 배열에 입력한다.
        /// 2017-10-13 박창욱
        /// </summary>
        /// <param name="strGubun"></param>
        /// <param name="argArr"></param>
        /// <param name="strCode"></param>
        public void Get_MISU_BasBcode_ToArray(PsmhDb pDbCon, string strGubun, string[] argArr, string strCode = "")
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += ComNum.VBLF + " SELECT GUBUN, CODE, NAME,";
            SQL += ComNum.VBLF + "        TO_CHAR(JDATE, 'YYYY-MM-DD') JDATE,";
            SQL += ComNum.VBLF + "        TO_CHAR(DELDATE, 'YYYY-MM-DD') DELDATE,";
            SQL += ComNum.VBLF + "        ENTSABUN, ";
            SQL += ComNum.VBLF + "        TO_CHAR(ENTDATE, 'YYYY-MM-DD') ENTDATE,";
            SQL += ComNum.VBLF + "        SORT, PART, CNT, ";
            SQL += ComNum.VBLF + "        GUBUN2, GUBUN3, ROWID";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1";
            SQL += ComNum.VBLF + "    AND GUBUN = '" + strGubun + "'";
            if (strCode != "")
            {
                SQL += ComNum.VBLF + "AND CODE = '" + strCode + "'";

            }
            SQL += ComNum.VBLF + "  ORDER BY CODE";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            for (i = 0; i < dt.Rows.Count; i++)
            {
                argArr[i] = dt.Rows[i]["CODE"].ToString().Trim() + "." + dt.Rows[i]["NAME"].ToString().Trim();
            }

            if (SqlErr != "")
            {
                ComFunc.MsgBox("BAS_BCODE 조회중 문제가 발생했습니다." + ComNum.VBLF + ComNum.VBLF + SQL);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
            return;
        }

        /// <summary>
        /// Author : 박창욱
        /// Create Date : 2017.10.19
        /// </summary>
        /// <param name="argData"></param>
        /// <returns></returns>
        /// 
        public string READ_MisuClassName(string argData)
        {
            string rtnVal = "";

            switch (argData)
            {
                case "01":
                    rtnVal = "공단";
                    break;
                case "02":
                    rtnVal = "직장";
                    break;
                case "03":
                    rtnVal = "지역";
                    break;
                case "04":
                    rtnVal = "보호";
                    break;
                case "05":
                    rtnVal = "산재";
                    break;
                case "07":
                    rtnVal = "자보";
                    break;
                case "08":
                    rtnVal = "계약처";
                    break;
                case "09":
                    rtnVal = "혈액원";
                    break;
                case "11":
                    rtnVal = "보훈청";
                    break;
                case "12":
                    rtnVal = "시각장애";
                    break;
                case "13":
                    rtnVal = "심신장애";
                    break;
                case "14":
                    rtnVal = "보장구";
                    break;
                case "15":
                    rtnVal = "직원대납";
                    break;
                default:
                    rtnVal = "오류";
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// Author : 박창욱
        /// Create Date : 2017.10.19</summary>
        /// <param name="argData"></param>
        /// <returns></returns>
        public string READ_SlipBunName(string argData)
        {
            string rtnVal = "";

            switch (argData)
            {
                case "11":
                    rtnVal = "처음청구";
                    break;
                case "12":
                    rtnVal = "정산청구";
                    break;
                case "13":
                    rtnVal = "재청구";
                    break;
                case "14":
                    rtnVal = "추가청구";
                    break;
                case "15":
                    rtnVal = "이의신청";
                    break;
                case "19":
                    rtnVal = "기타청구";
                    break;
                case "21":
                    rtnVal = "입금";
                    break;
                case "22":
                    rtnVal = "정산입금";
                    break;
                case "23":
                    rtnVal = "주민입금";
                    break;
                case "24":
                    rtnVal = "이의입금";
                    break;
                case "25":
                    rtnVal = "기타수입";
                    break;
                case "26":
                    rtnVal = "심사중입금";
                    break;
                case "31":
                    rtnVal = "삭감";
                    break;
                case "32":
                    rtnVal = "반송";
                    break;
                case "33":
                    rtnVal = "과지급금";
                    break;
                case "34":
                    rtnVal = "계산착오";
                    break;
                case "35":
                    rtnVal = "절산삭감";
                    break;
                case "01":
                case "02":
                case "03":
                case "04":
                case "05":
                case "06":
                case "07":
                case "08":
                case "09":
                    rtnVal = "참고사항";
                    break;
                case "10":
                    rtnVal = "심사중";
                    break;
                case "20":
                    rtnVal = "보류";
                    break;
                default:
                    rtnVal = "오류";
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// Description : 개인미수 잔액 확인
        /// Author : 김민철
        /// Create Date : 2018.02.12
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argPano"></param>
        /// <returns></returns>
        public DataTable sel_MISU_GAINMST(PsmhDb pDbCon, string argPano)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT JAmt ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GAINMST ";
                SQL += ComNum.VBLF + "  WHERE Pano = '" + argPano + "' ";
                SQL += ComNum.VBLF + "    AND JAmt > 0 ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        /// <summary>
        /// Description : ListIndex_MisuIO
        /// Author : 김해수
        /// Create Date : 2018.11.27
        /// </summary>
        /// <returns></returns>
        public int ListIndex_MisuIO(string arg1)
        {
            string[] GstrMisuIO = new string[2];
        
            GstrMisuIO[0] = "O.외래";
            GstrMisuIO[1] = "I.입원";

            int i = 0;
            int ArgReturn = 0;

            for (i = 0; i < GstrMisuIO.Length; i++)
            {
                if(VB.Left(GstrMisuIO[i],1) == arg1)
                {
                    ArgReturn = i;
                    return ArgReturn;
                }
            }

            return ArgReturn;
        }

        /// <summary>
        /// Description : ListIndex_MisuBun
        /// Author : 김해수
        /// Create Date : 2018.11.27
        /// </summary>
        /// <returns></returns>
        public int ListIndex_MisuBun(string arg1,string gubun)
        {
            string[] GstrMisuBun = null;

            if (gubun == "")
            {
                 GstrMisuBun = new string[23];

                GstrMisuBun[0] = "01.내과분야";
                GstrMisuBun[1] = "02.외과분야";
                GstrMisuBun[2] = "03.산소아과";
                GstrMisuBun[3] = "04.안.이비";
                GstrMisuBun[4] = "05.피.비뇨";
                GstrMisuBun[5] = "06.치과";
                GstrMisuBun[6] = "07.NP정액";
                GstrMisuBun[7] = "08.장애대불";
                GstrMisuBun[8] = "09.가정간호";
                GstrMisuBun[9] = "10.재청구";
                GstrMisuBun[10] = "11.이의신청";
                GstrMisuBun[11] = "12.정산진료비";
                GstrMisuBun[12] = "13.추가청구";
                GstrMisuBun[13] = "14.NP장애대불";
                GstrMisuBun[14] = "15.HD정액";
                GstrMisuBun[15] = "16.HU호스피스";
                GstrMisuBun[16] = "19.기타청구";
                GstrMisuBun[17] = "20.상한대불";
                GstrMisuBun[18] = "21.희귀지원금";
                GstrMisuBun[19] = "22.결핵지원금";
                GstrMisuBun[20] = "23.DRG(포괄수가)";
                GstrMisuBun[21] = "24.100/100 미만";
                GstrMisuBun[22] = "25.국가재난지원";
            }
            else if(gubun =="TA")
            {
                GstrMisuBun = new string[4];

                GstrMisuBun[0] = "01.처음창구";
                GstrMisuBun[1] = "10.재청구";
                GstrMisuBun[2] = "11.이의신청";
                GstrMisuBun[3] = "19.기타청구";
            }
          

            int i = 0;
            int ArgReturn = -1;

            for (i = 0; i < GstrMisuBun.Length; i++)
            {
                if (ComFunc.LeftH(GstrMisuBun[i], 2) == arg1)
                {
                    ArgReturn = i;
                    return ArgReturn;
                }
            }

            return ArgReturn;
        }

        /// <summary>
        /// Description : ListIndex_MisuSayu
        /// Author : 김해수
        /// Create Date : 2018.11.27
        /// </summary>
        /// <returns></returns>
        public int ListIndex_MisuSayu(string arg1)
        {
            string[] GstrMisuSayu = new string[8];

            GstrMisuSayu[1] = "01.가퇴원";
            GstrMisuSayu[2] = "02.업무착오";
            GstrMisuSayu[3] = "03.탈원";
            GstrMisuSayu[4] = "04.지불각서";
            GstrMisuSayu[5] = "05.응급실";
            GstrMisuSayu[6] = "06.외래";
            GstrMisuSayu[7] = "07.심사청구";
            GstrMisuSayu[8] = "10.기타";

            int i = 0;
            int ArgReturn = -1;

            for (i = 0; i < GstrMisuSayu.Length; i++)
            {
                if (ComFunc.LeftH(GstrMisuSayu[i], 1) == arg1)
                {
                    ArgReturn = i - 1;
                    return ArgReturn;
                }
            }

            return ArgReturn;
        }

        /// <summary>
        /// Description : ListIndex_MisuDept
        /// Author : 김해수
        /// Create Date : 2018.11.27
        /// </summary>
        /// <returns></returns>
        public int ListIndex_MisuDept(string arg1)
        {
            int i = 0;
            int ArgReturn = 0;
            string[] GstrDept = new string[31];

            if(arg1 == "")
            {
                ArgReturn = -1;
                return ArgReturn;
            }


            for (i = 0; i < 31; i++)
            {
                GstrDept[i] = "";
            }

            #region DB에서 가져오기
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT DEPTCODE, DEPTNAMEK";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT";
                SQL += ComNum.VBLF + "WHERE PRINTRANKING < 31";
                SQL += ComNum.VBLF + "ORDER BY PrintRanking";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장                    
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        GstrDept[i] = dt.Rows[i]["DeptCode"].ToString().Trim() + "." + dt.Rows[i]["DeptNameK"].ToString().Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt.Dispose();
            dt = null;
            #endregion

            ArgReturn = -1;

            for (i = 0; i < 30; i++)
            {
                if(VB.Left(GstrDept[i],2) == arg1)
                {
                    ArgReturn = i;
                    break;
                }
            }

            return ArgReturn;
        }

        /// <summary>
        /// Description : READ_MISU_MAGAM_DATE
        /// Author : 김해수
        /// Create Date : 2018.11.28
        /// </summary>
        /// <returns></returns>
        public string READ_MISU_MAGAM_DATE()
        {
            string strVal = "";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT NAME";
                SQL += ComNum.VBLF + "FROM ADMIN.BAS_BCODE";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "  AND Gubun='ACC_전표입력가능시작일자'";
                SQL += ComNum.VBLF + "  AND Code='02' ";

                SQL = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strVal;
                }
                if(dt.Rows.Count >0)
                {
                    strVal = dt.Rows[0]["NAME"].ToString().Trim();
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return strVal;
            }

            dt.Dispose();
            dt = null; 

            return strVal;
        }

        /// <summary>
        /// Description : CHECK_MISU_MAGAM_SPREAD
        /// Author : 김해수
        /// Create Date : 2018.11.28
        /// </summary>
        /// <returns></returns>
        public Boolean CHECK_MISU_MAGAM_SPREAD(FarPoint.Win.Spread.FpSpread argSpd, int argCol)
        {
            int i = 0;
            string strMDATE = "", strChk = "";
            Boolean strVal = false;

            strChk = "OK";

            strMDATE = READ_MISU_MAGAM_DATE();

            for (i = 0; i < argSpd.ActiveSheet.RowCount; i++)
            {
                //if(argSpd.ActiveSheet.Cells[i,argCol].Text <= strMDATE)
                if(string.Compare(argSpd.ActiveSheet.Cells[i,argCol].Text,strMDATE) <= 0)
                {
                    strChk = "NO";
                    break;
                }
            }    
            
            if(strChk != "OK")
            {
                ComFunc.MsgBox("등록하려는 일자 중 미수마감일보다 빠른 일자가 있습니다." + ComNum.VBLF + 
                                "미수등록이 불가능합니다" + ComNum.VBLF + "* 등록된 미수마감일 : " + 
                                strMDATE,"등록불가");
                strVal = false;
            }
            else
            {
                strVal = true;
            }

            return strVal;
        }

        /// <summary>
        /// Description : DATA_CHECK
        /// Author : 김해수
        /// Create Date : 2018.11.30
        /// </summary>
        /// <returns></returns>
        public string DATA_CHECK(string Arg1,string argDate)
        {
            string argVal = "";

            if(Arg1 == "")
            {
                argVal = "OK";
                return argVal;
            }

            if (string.Compare(Arg1, argDate) > 0)
            {
                ComFunc.MsgBox("작업일이 오늘보다 클 수 없습니다.","확인");
                argVal = "NO";
                return argVal;
            }

            if(clsType.User.IdNumber == "4349")
            {
                argVal = "OK";
            }

            argVal = "OK";

                return argVal;
        }

        /// <summary>
        /// Description : DATA_MisuGye_MISU
        /// Author : 김해수
        /// Create Date : 2018.12.06
        /// </summary>
        /// <returns></returns>
        public string READ_MisuGye_MISU(string arg1,string gubun)
        {
            string argVal = "";
            string[] GstrMisuGye = null;
            int argInx = 0;
            string argRetrun = "";
            int TAgubun = 0;

            if (VB.Val(arg1) == 9)
            {
                argVal = "지급보류";
                return argVal;
            }

            if (arg1.Length != 2)
            {
                TAgubun = 1;
                arg1 = VB.Left(arg1, 2);
            }
            if(gubun == "")
            {
                GstrMisuGye = new string[18];

                GstrMisuGye[0] = "11.처음청구";
                GstrMisuGye[1] = "12.정산청구";
                GstrMisuGye[2] = "13.재청구";
                GstrMisuGye[3] = "14.추가청구";
                GstrMisuGye[4] = "15.이의신청";
                GstrMisuGye[5] = "19.기타미수";
                GstrMisuGye[6] = "21.입금";
                GstrMisuGye[7] = "22.정산입금";
                GstrMisuGye[8] = "23.주민입금";
                GstrMisuGye[9] = "24.이의입금";
                GstrMisuGye[10] = "25.기타수입";
                GstrMisuGye[11] = "26.심사중입금";
                GstrMisuGye[12] = "31.삭감";
                GstrMisuGye[13] = "32.반송";
                GstrMisuGye[14] = "33.과지급금";
                GstrMisuGye[15] = "34.계산착오";
                GstrMisuGye[16] = "35.삭감절산";
                GstrMisuGye[17] = "**.참고사항";
            }else if(gubun =="TA")
            {
                GstrMisuGye = new string[18];

                GstrMisuGye[1] = "11.청구미수";
                GstrMisuGye[2] = "14.이의신청";
                GstrMisuGye[3] = "15.재청구";
                GstrMisuGye[4] = "16.추가청구";
                GstrMisuGye[5] = "19.기타미수";
                GstrMisuGye[6] = "21.입금";
                GstrMisuGye[7] = "25.기타수입";
                GstrMisuGye[8] = "31.삭감";
                GstrMisuGye[9] = "32.반송";
            }

            if(TAgubun == 1)
            {
                switch (arg1)
                {
                    case "1":
                        argVal = "1차검토액";
                        return argVal;
                    case "2":
                        argVal = "분쟁금액";
                        return argVal;
                    case "3":
                        argVal = "분쟁결과";
                        return argVal; 
                }
            }

            if (VB.Val(arg1) > 0 && VB.Val(arg1) < 10)
            {
                argVal = "참고";
                return argVal;
            }

            if(VB.Val(arg1) == 10)
            {
                argVal = "심사중";
                return argVal;
            }

            for(argInx = 0; argInx < 17; argInx++)
            {
                if(VB.Left(GstrMisuGye[argInx],2) == arg1)
                {
                    argRetrun = VB.Mid(GstrMisuGye[argInx] + VB.Space(20) ,4,15);
                }
            }
            argVal = argRetrun;

            return argVal;
        }

        /// <summary>
        /// Description : Message_MISU_Magam
        /// Author : 김해수
        /// Create Date : 2018.12.14
        /// </summary>
        /// <returns></returns>
        public void Message_MISU_Magam(string argDATE)
        {
            ComFunc.MsgBox("등록하려는 일자 중 미수마감일보다 빠른 일자가 있습니다." + ComNum.VBLF +
                                "미수등록이 불가능합니다" + ComNum.VBLF + "* 등록된 미수마감일 : " +
                                argDATE, "등록불가");
            return;
        }

        public string READ_BAS_PATIENT(string argCode)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string argVal = "";

            if(argCode.Trim() == "")
            {
                argVal = "";
                return argVal;
            }

            argCode = VB.Val(argCode).ToString("00000000");

            try
            {
                SQL = "";
                SQL = " SELECT SNAME FROM " + ComNum.DB_PMPA + "BAS_PATIENT WHERE PANO ='" + argCode + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return argVal;
                }
                    
                if(dt.Rows.Count == 0)
                {
                    argVal = "";
                }else
                {
                    argVal = dt.Rows[0]["Sname"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return argVal;
        }
    }
}
