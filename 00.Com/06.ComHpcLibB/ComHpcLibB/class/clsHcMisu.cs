
using ComBase;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ComHpcLibB
{
    public class clsHcMisu
    {

        HicJepsuService hicJepsuService = new HicJepsuService();
        BasDoctorService basDoctorService = new BasDoctorService();
        ExamMasterService examMasterService = new ExamMasterService();
        BasPatientService basPatientService = new BasPatientService();
        BasSunService basSunService = new BasSunService();
        BasSutService basSutService = new BasSutService();
        HicMcodeService hicMcodeService = new HicMcodeService();
        HicGroupcodeService hicGroupcodeService = new HicGroupcodeService();
        HicLtdService hicLtdService = new HicLtdService();

        /// <summary>
        /// 검진년도 확인
        /// </summary>
        /// <returns></returns>
        public string READ_GJYEAR(long ArgWRTNO)
        {
            string rtnVal = "";
            HIC_JEPSU item = hicJepsuService.Read_Jepsu_Wrtno(ArgWRTNO);
            rtnVal = item.GJYEAR.Trim();

            return rtnVal;

        }

        /// <summary>
        /// BAS_DOCTOR 의사명 확인
        /// </summary>
        /// <returns></returns>
        public string READ_DRNAME(string ArgCODE)
        {
            string rtnVal = "";

            BAS_DOCTOR item = basDoctorService.GetItembyDrCord(ArgCODE);
            rtnVal = item.DRNAME.Trim();

            return rtnVal;

        }

        /// <summary>
        /// EXAM_MASTER 검체명 확인
        /// </summary>
        /// <returns></returns>
        public string READ_EXAM_NAME(string ArgCODE)
        {
            string rtnVal = "";

            EXAM_MASTER item = examMasterService.FindExamName(ArgCODE);
            rtnVal = item.EXAMFNAME.Trim();

            return rtnVal;

        }

        public string READ_BAS_PATIENT(string ArgPANO)
        {
            string rtnVal = "";
            ArgPANO = string.Format("{0:00000000}", ArgPANO);
            BAS_PATIENT item = basPatientService.GetItembyPano(ArgPANO);
            rtnVal = item.SNAME.Trim();
            return rtnVal;

        }

        /// <summary>
        /// BAS_SUN 수가명 확인
        /// </summary>
        /// <returns></returns>
        public string READ_SUGA_NAME(string ArgCODE)
        {
            string rtnVal = "";

            BAS_SUN item = basSunService.FindSugaName(ArgCODE);
            rtnVal = item.SUNAMEK.Trim();

            return rtnVal;

        }

        /// <summary>
        /// BAS_SUT 수가명 확인
        /// </summary>
        /// <returns></returns>
        public string READ_BOHUM_AMT(string ArgCODE)
        {
            string rtnVal = "";

            BAS_SUT item = basSutService.GetItembySuCode(ArgCODE);
            rtnVal = item.BAMT.ToString();

            return rtnVal;

        }

        /// <summary>
        /// 유해인자코드(Ucode) 표시
        /// </summary>
        /// <returns></returns>
        public string Ucode_Name_Display(string ArgCODE)
        {
            string rtnVal = string.Empty;
            string ArgSql = string.Empty;

            StringBuilder strSQL = new StringBuilder();



            if (string.IsNullOrEmpty(ArgCODE))
            {
                return rtnVal;
            }

            for (int i = 0; i < VB.L(ArgCODE, ","); i++)
            {
                if (VB.Pstr(ArgCODE, ",", i).Trim() != "")
                {
                    strSQL.Append("'");
                    strSQL.Append(VB.Pstr(ArgCODE, ",", i).Trim());
                    strSQL.Append("',");
                }
            }

            if (Convert.ToString(strSQL) == "") { return rtnVal; }

            ArgSql = strSQL.ToString();
            ArgSql = VB.Left(ArgSql, ArgSql.Length - 1);

            strSQL = new StringBuilder();
            IList <HIC_MCODE>  list = hicMcodeService.SelMCode_Many(ArgSql);

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].YNAME != null)
                {
                    strSQL.Append(list[i].YNAME);
                    strSQL.Append(",");
                }
                else
                {
                    strSQL.Append(list[i].NAME);
                    strSQL.Append(",");
                }
            }

            rtnVal = strSQL.ToString();
            rtnVal = VB.Left(rtnVal, rtnVal.Length - 1);

            return rtnVal;

        }

        /// <summary>
        /// 특수검진 선택검사명칭 표시
        /// </summary>
        /// <returns></returns>
        public string SExam_Names_Display(string ArgCODE)
        {
            string rtnVal = string.Empty;
            string ArgSql = string.Empty;

            StringBuilder strSQL = new StringBuilder();

            if (string.IsNullOrEmpty(ArgCODE))
            {
                return rtnVal;
            }

            for (int i = 0; i < VB.L(ArgCODE, ","); i++)
            {
                if (VB.Pstr(ArgCODE, ",", i).Trim() != "")
                {
                    strSQL.Append("'");
                    strSQL.Append(VB.Pstr(ArgCODE, ",", i).Trim());
                    strSQL.Append("',");
                }
            }

            if (Convert.ToString(strSQL) == "") { return rtnVal; }

            ArgSql = strSQL.ToString();
            ArgSql = VB.Left(ArgSql, ArgSql.Length - 1);

            strSQL = new StringBuilder();
            IList<HIC_GROUPCODE> list = hicGroupcodeService.FindCodeIn(ArgSql);


            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].YNAME != null)
                {
                    strSQL.Append(list[i].YNAME);
                    strSQL.Append(",");
                }
                else
                {
                    strSQL.Append(list[i].NAME);
                    strSQL.Append(",");
                }
            }

            rtnVal = strSQL.ToString();
            rtnVal = VB.Left(rtnVal, rtnVal.Length - 1);

            return rtnVal;

        }

        /// <summary>
        /// 미수관리의 SLIP의 계정명 표시
        /// </summary>
        /// <returns></returns>
        public string READ_MisuGea_Name(string ArgCODE)
        {
            string rtnVal = "";

            switch (ArgCODE)
            {
                case "11": rtnVal = "미수발생";     break;
                case "21": rtnVal = "현금입금";     break;
                case "22": rtnVal = "지로입금";     break;
                case "23": rtnVal = "통장입금";     break;
                case "24": rtnVal = "기타입금";     break;
                case "25": rtnVal = "수수료";       break;
                case "55": rtnVal = "카드입금";     break;
                case "31": rtnVal = "감액";         break;
                case "32": rtnVal = "삭감";         break;
                case "33": rtnVal = "반송";         break;
                case "99": rtnVal = "청구차액";     break;
                default:   rtnVal = "";             break;

            }

            return rtnVal;
        }


        /// <summary>
        /// 신규미수번호 부여
        /// </summary>
        /// <returns></returns>
        public long READ_NEW_MisuNo()
        {
            long rtnVal = 0;
            long result = 0;

            //result = hicGroupcodeService.FindMisuNo();
            
            if(result == 0)
            {
                rtnVal = 0;
                MessageBox.Show("미수번호 신규 부여시 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                rtnVal = result;
            }

            return rtnVal;
        }


        /// <summary>
        /// 미수종류 표시
        /// </summary>
        /// <returns></returns>
        public string READ_MisuJong(string ArgCODE)
        {
            string rtnVal = "";

            switch (ArgCODE)
            {
                case "1": rtnVal = "회사"; break;
                case "2": rtnVal = "조합"; break;
                case "3": rtnVal = "국고"; break;
                case "4": rtnVal = "개인"; break;
                default: rtnVal = ""; break;
            }

            return rtnVal;
        }

        /// <summary>
        /// 회사이름 표시
        /// </summary>
        /// <returns></returns>
        public string READ_LTDNAME(string ArgCODE)
        {
            string rtnVal = "";

            HIC_LTD item = hicLtdService.FindOne(ArgCODE);

            if (item.DLTD.ToString() != "")
            {
                HIC_LTD item1 = hicLtdService.FindOne(ArgCODE);
                rtnVal = item1.NAME.Trim();
            }

            return rtnVal;
        }

        /// <summary>
        /// 회사이름 표시
        /// </summary>
        /// <returns></returns>
        public string READ_LTD(string ArgCODE)
        {
            string rtnVal = "";

            HIC_LTD item = hicLtdService.FindOne(ArgCODE);

            if (item.DLTD.ToString() != "")
            {
                HIC_LTD item1 = hicLtdService.FindOne(ArgCODE);
                rtnVal = item1.SAUPNO.Trim();
            }

            return rtnVal;
        }

    }
}
