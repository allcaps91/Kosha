using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ComHpcLibB
{
    /// <summary>
    /// 건진변수(HaAct.bas)
    /// </summary>
    public class clsHcAct
    {
        ComHpcLibBService comHpcLibBService = new ComHpcLibBService();
        HicResultService hicResultService = new HicResultService();
        ExamDisplayService examDisplayService = new ExamDisplayService();
        HicResultHisService hicResultHisService = new HicResultHisService();
        HicCodeService hicCodeService = new HicCodeService();
        HicExcodeService hicExcodeService = new HicExcodeService();
        HicRescodeService hicRescodeService = new HicRescodeService();
        HicSangdamNewService hicSangdamNewService = new HicSangdamNewService();
        HicSangdamWaitService hicSangdamWaitService = new HicSangdamWaitService();
        HicWaitRoomService hicWaitRoomService = new HicWaitRoomService();
        HicJepsuService hicJepsuService = new HicJepsuService();
        HicXrayResultService hicXrayResultService = new HicXrayResultService();

        /// <summary>
        /// HaAct.bas
        /// </summary>
        public struct HaAct_NOTIFYICONDATA
        {
            public long cbSize;
            public long hwnd;
            public long uId;
            public long uFlags;
            public long ucallbackMessage;
            public long hIcon;
            public string szTip;
        }
        public HaAct_NOTIFYICONDATA t;

        /// <summary>
        /// vbHicDojang.Bas
        /// </summary>
        public class HaAct_API
        {
            //Public Declare Function Shell_NotifyIcon Lib "shell32" Alias "Shell_NotifyIconA" _
            //(ByVal dwMessage As Long, _
            //pnid As NOTIFYICONDATA) As Boolean

            [DllImportAttribute("shell32.dll", EntryPoint = "Shell_NotifyIconA", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
            public static extern bool Shell_NotifyIcon(IntPtr dwMessage, IntPtr NOTIFYICONDATA);

            public const string NIM_ADD = "&H0";
            public const string NIM_MODIFY = "&H1";
            public const string NIM_DELETE = "&H2";
            public const string WM_MOUSEMOVE = "&H200";
            public const string NIF_MESSAGE = "&H1";
            public const string NIF_ICON = "&H2";
            public const string NIF_TIP = "&H4";
            public const string WM_LBUTTONDBLCLK = "&H203";
            public const string WM_LBUTTONDOWN = "&H201";
            public const string WM_LBUTTONUP = "&H202";
            public const string WM_RBUTTONDBLCLK = "&H206";
            public const string WM_RBUTTONDOWN = "&H204";
            public const string WM_RBUTTONUP = "&H205";
        }

        /// <summary>
        /// 액팅코드변환 READ_액팅2검사코드()
        /// </summary>
        /// <param name="argCode1"></param>
        /// <param name="argCode2"></param>
        /// <returns>CODE</returns>
        public string READ_ActingExamCode2(string argCode1, string argCode2)
        {
            string rtnVal = "";

            rtnVal = hicCodeService.Read_Hic_Code2("A1", argCode1);

            return rtnVal;
        }

        /// <summary>
        /// READ_액팅코드구분 
        /// </summary>
        /// <param name="argCode"></param>
        /// <returns>GCode2</returns>
        public string READ_ActingCode_Gubun(string argCode)
        {
            string rtnVal = "";

            rtnVal = hicCodeService.Read_Hic_Code3("A1", argCode);

            return rtnVal;
        }

        /// <summary>
        /// READ_검사코드2방사선코드
        /// </summary>
        /// <param name="argCode"></param>
        /// <returns>XrayCode</returns>
        public string READ_ExamCode2_XrayCode(string argCode)
        {
            string rtnVal = "";

            rtnVal = hicExcodeService.Read_XrayCode(argCode);

            return rtnVal;
        }

        /// <summary>
        /// READ_결과값코드_정상()
        /// </summary>
        /// <param name="argCode"></param>
        /// <returns>Name</returns>
        public string READ_ResultValue_Normal(string argCode)
        {
            string rtnVal = "";

            rtnVal = hicRescodeService.GetNameByGubun(argCode);

            return rtnVal;
        }

        /// <summary>
        /// 다음 검사실명
        /// </summary>
        /// <param name="argWrtNo"></param>
        /// <returns></returns>
        public string READ_NextWait_Room(long argWrtNo)
        {
            string rtnVal = "";
            string strRoom = "";
            string strName = "";

            strRoom = hicSangdamWaitService.GetNextRoomByWrtNo(argWrtNo, "");

            if (strRoom != "")
            {
                strRoom = VB.Pstr(strRoom, ",", 1);
                strName = "";

                switch (strRoom)
                {
                    case "30":
                        strName = "계측1번(시력.소변)";
                        break;
                    case "31":
                        strName = "계측3번(혈압)";
                        break;
                    case "32":
                        strName = "계측4번(채혈실)";
                        break;
                    case "33":
                        strName = "접수창구제출";
                        break;
                    default:
                        break;
                }

                if (strName != "")
                {
                    rtnVal = "";
                    return rtnVal;
                }

                strName = hicWaitRoomService.GetRoomNamebyRoom(strRoom);

                if (strName != "")
                {
                    rtnVal = strRoom + "." + strName;
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 다음 검사실 설정
        /// </summary>
        /// <param name="argWrtNo"></param>
        /// <param name="argPano"></param>
        /// <param name="argRoom"></param>
        /// <returns></returns>
        public bool WAIT_NextRoom_SET(long argWrtNo, long argPano, string argRoom)
        {
            bool rtnVal = false;

            long nWait = 0;
            string strNextRoom = "";
            string strRoom = "";
            string strTemp = "";
            long nWrtNo = 0;
            string strGjJong = "";
            string strSName = "";
            string strSex = "";
            long nAge = 0;

            strNextRoom = hicSangdamWaitService.GetNextRoomByWrtNo(argWrtNo, argRoom);

            //다음 검사실이 없으면
            if (strNextRoom == "")
            {
                int result = hicSangdamWaitService.Update_Patient_Call(argPano, argRoom);

                if (result < 0)
                {
                    rtnVal = false;
                    return rtnVal;
                }
            }

            strRoom = VB.Pstr(strNextRoom, ",", 1);
            strTemp = VB.Pstr(strNextRoom, strRoom + ",", 2);
            strNextRoom = strTemp;

            //다음 가셔야할곳이 접수창구이면 등록 안함
            if (string.Compare(strRoom, "29") > 0)
            {
                //기존 등록된 대기순번을 삭제함
                int result1 = hicSangdamWaitService.Delete_Hic_Sangdam_Wait(argPano);

                if (result1 < 0)
                {
                    rtnVal = false;
                    return rtnVal;
                }
            }

            nWait = hicSangdamWaitService.GetMaxWaitNoByRoom(argRoom);

            //기존 등록된 대기순번을 삭제함
            int result2 = hicSangdamWaitService.Delete_Hic_Sangdam_Wait(argPano);

            if (result2 < 0)
            {
                rtnVal = false;
                return rtnVal;
            }

            if (argRoom=="15" || argRoom == "16" || argRoom == "17" || argRoom == "18")
            {
                List<HIC_JEPSU> list = hicJepsuService.GetJepsuInfobyPano(argPano);

                if (list.Count > 0)
                {
                    string[] arrWrtNo = list.GetStringArray("WRTNO");
                    string[] arrGjJong = list.GetStringArray("GJJONG");
                    string[] arrSName = list.GetStringArray("SNAME");
                    string[] arrSex = list.GetStringArray("SEX");

                    for (int i = 0; i < list.Count; i++)
                    {
                        nWrtNo = long.Parse(arrWrtNo[i]);
                        strGjJong = arrGjJong[i];
                        strSName = arrSName[i];
                        strSex = arrSex[i];

                        //상담대기 등록함
                        int result3 = hicSangdamWaitService.Insert_Hic_Sangdam_Wait(nWrtNo, strSName, strSex, nAge, strGjJong, strRoom, nWait, argPano, strNextRoom);

                        if (result3 < 0)
                        {
                            MessageBox.Show("상담대기 순번등록 중 오류 발생!!!", "Rollback", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            rtnVal = false;
                            return rtnVal;
                        }
                    }
                }
            }
            else
            {
                HIC_JEPSU list = hicJepsuService.GetItemByWRTNO(argWrtNo);

                if (!list.IsNullOrEmpty())
                {
                    nWrtNo = list.WRTNO;
                    strGjJong = list.GJJONG;
                    strSName = list.SNAME;
                    strSex = list.SEX;

                    //상담대기 등록함
                    int result3 = hicSangdamWaitService.Insert_Hic_Sangdam_Wait(nWrtNo, strSName, strSex, nAge, strGjJong, strRoom, nWait, argPano, strNextRoom);

                    if (result3 < 0)
                    {
                        MessageBox.Show("상담대기 순번등록 중 오류 발생!!!", "Rollback", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        rtnVal = false;
                        return rtnVal;
                    }
                }
            }

            
            return rtnVal;
        }

        public void Xray_Exid_Set(long argWrtNo, string argXrayNo, string argExid)
        {
            string strSabun = "";
            string strRowId = "";

            strSabun = VB.Pstr(argExid, ".", 1).Trim();

            //방사선 직촬번호 입력
            strRowId = comHpcLibBService.GetRowIdbyXrayNo(argXrayNo);

            int result = hicXrayResultService.Update_Patient_Exid(strSabun, strRowId);

            if (result < 0)
            {
                MessageBox.Show("촬영자 업데이트 오류 발생!!!", "Rollback", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// READ_검사코드2방사선코드()
        /// </summary>
        /// <param name="argCode"></param>
        /// <returns></returns>
        public string READ_ExamCode2XrayCode(string argCode)
        {
            string rtnVal = "";

            rtnVal = hicExcodeService.Read_XrayCode(argCode);

            return rtnVal;
        }

        /// <summary>
        /// READ_액팅코드구분()
        /// </summary>
        /// <param name="argCode"></param>
        /// <returns></returns>
        public string READ_ActingCodeGubun(string argCode)
        {
            string rtnVal = "";

            rtnVal = hicCodeService.Read_Hic_Code3("A1", argCode);

            return rtnVal;
        }
    }
}
