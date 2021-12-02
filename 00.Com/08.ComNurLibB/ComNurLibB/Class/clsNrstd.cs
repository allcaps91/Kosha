using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComLibB;
using ComBase;
using System.Reflection;
using System.Windows.Forms;
using ComDbB;
using System.Data;

namespace ComNurLibB
{
    public class clsNrstd
    {
        /// <summary>
        /// Author : 안정수
        /// Create : 2018-01-27
        /// <seealso cref="nrstd.bas : READ_EDU_JONG"/>
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static string READ_EDU_JONG(string arg)
        {
            string rtnVal = "";

            switch (arg)
            {
                case "01":
                    rtnVal = "병동교육";
                    break;

                case "02":
                    rtnVal = "감염교육";
                    break;

                case "03":
                    rtnVal = "QI교육";
                    break;

                case "04":
                    rtnVal = "CS교육";
                    break;

                case "05":
                    rtnVal = "CPR교육";
                    break;

                case "06":
                    rtnVal = "학술강좌";
                    break;

                case "07":
                    rtnVal = "간호부주최 직무교육";
                    break;

                case "08":
                    rtnVal = "전직원교육";
                    break;

                case "09":
                    rtnVal = "특강(간협)";
                    break;

                case "10":
                    rtnVal = "연수교육";
                    break;

                case "11":
                    rtnVal = "10대질환";
                    break;

                case "12":
                    rtnVal = "보수교육";
                    break;

                case "13":
                    rtnVal = "기타Report";
                    break;

                case "14":
                    rtnVal = "강사활동(교육)";
                    break;

                case "15":
                    rtnVal = "프리셉터교육";
                    break;

                case "16":
                    rtnVal = "Cyber 교육";
                    break;

                case "17":
                    rtnVal = "승진자교육";
                    break;

                case "18":
                    rtnVal = "기타";
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// Author : 안정수
        /// Create : 2018-01-27
        /// <seealso cref="nrstd.bas : READ_EDU_TIME"/>        
        /// </summary>
        /// <param name="Arg1"></param>
        /// <param name="Arg2"></param>
        /// <returns></returns>
        public static string READ_EDU_TIME(string Arg1, string Arg2)
        {
            string rtnVal = "";
            string strTIME = "";
            string strSECOND = "";

            if(VB.Fix(Convert.ToInt32(VB.Val(Arg1) / (double)60)) > 0)
            {
                strTIME = VB.Fix(Convert.ToInt32(VB.Val(Arg1) / (double)60)) + "시간 ";
            }

            if(VB.Val(Arg1) % (double)60 > 0)
            {
                strSECOND = VB.Val(Arg1) % (double)60 + "분";
            }

            rtnVal = strTIME + strSECOND;

            return rtnVal;
        }

        /// <summary>
        /// Author : 안정수
        /// Create : 2018-01-27
        /// <seealso cref="nrstd.bas : SET_EDU_JONG"/>        
        /// </summary>
        /// <param name="arg"></param>
        public static void SET_EDU_JONG(ComboBox arg)
        {
            arg.Items.Clear();

            arg.Items.Add(" ");
            arg.Items.Add("01.병동교육");
            arg.Items.Add("02.감염교육");
            arg.Items.Add("03.QI");
            arg.Items.Add("04.CS교육");
            arg.Items.Add("05.CPR교육");
            arg.Items.Add("06.학술강좌");
            arg.Items.Add("07.간호부주최 직무교육");
            arg.Items.Add("08.전직원교육");
            arg.Items.Add("09.특강(간협)");
            arg.Items.Add("10.연수교육");
            arg.Items.Add("11.10대질환");
            arg.Items.Add("12.보수교육");
            arg.Items.Add("13.기타Report");
            arg.Items.Add("14.강사활동(교육)");
            arg.Items.Add("15.프리셉터교육");
            arg.Items.Add("16.Cyber 교육");
            arg.Items.Add("17.승진자교육");
            arg.Items.Add("18.기타");

            arg.SelectedIndex = 0;
        }

        /// <summary>
        /// Author : 안정수
        /// Create : 2018-01-29
        /// <seealso cref="Nrinfo.bas : READ_NUR_EDU_SEQ"/>        
        /// </summary>
        public static string READ_NUR_EDU_SEQ()
        {
            ComFunc.ReadSysDate(clsDB.DbCon);
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string rtnVal = "";

            //시퀀스 번호를 생성함
            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  KOSMOS_PMPA. SEQ_NUR_EDU_SEQ.NEXTVAL WRTNO";
            SQL += ComNum.VBLF + "FROM DUAL";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }

            if(dt.Rows.Count > 0)
            {
                rtnVal = VB.Left(clsPublic.GstrSysDate, 4) + VB.Mid(clsPublic.GstrSysDate, 6, 2) + VB.Right(clsPublic.GstrSysDate, 2) + ComFunc.SetAutoZero(dt.Rows[0]["WRTNO"].ToString().Trim(), 3);
            }

            dt.Dispose();
            dt = null;

            return rtnVal;
        }
    }
}
