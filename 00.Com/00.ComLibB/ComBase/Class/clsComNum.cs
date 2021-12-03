using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ComBase
{
    /// <summary>
    /// 전역 상수를 정의 한다
    /// </summary>
    public static class ComNum
    {
        public const string MNGPASSWORD = "2017"; //Bypass 비밀번호
        public const string DEFAULT_JOBGROUP = "JOB901001"; //기본작업그룹

        public const int LENPTNO = 8;  //등록번호 자리숫
        public const int LENUSEID = 8;  //사용자번호 자리숫

        public const string VBLF = "\r\n";  //줄바꿈 문자열
        public const string LF = "\n";  //줄바꿈 문자열
        public const int SPDROWHT = 22;  //스프래드 Row 높이를 공통으로

        //public static Color SPSELCOLOR = Color.PaleTurquoise;  //스프래드 셀렉트      LightGreen
        public static Color SPSELCOLOR = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(243)))), ((int)(((byte)(188)))));  //스프래드 셀렉트      LightGreen
        public static Color SPDESELCOLOR = Color.White;  //스프래드 셀렉트 취소

        #region //DataBase 이름 세팅 
        /// <summary>
        /// ADMIN.
        /// </summary>
        public const string DB_PMPA = "ADMIN."; //원무 DataBase
        /// <summary>
        /// ADMIN.
        /// </summary>
        public const string DB_MED = "ADMIN.";   //진료 DataBase
        /// <summary>
        /// ADMIN.
        /// </summary>
        public const string DB_ERP = "ADMIN.";   //일반관리 DataBase
        /// <summary>
        /// ADMIN.
        /// </summary>
        public const string DB_EMR = "ADMIN.";   //EMR DataBase
        /// <summary>
        /// ADMIN.
        /// </summary>
        public const string DB_SUP = "ADMIN.";  //지원부서 DataBase
        /// <summary>
        /// ADMIN.
        /// </summary>
        public const string DB_ABC = "ADMIN.";   //원가관리 DataBase
        /// <summary>
        /// ADMIN.
        /// </summary>
        public const string DB_PACS = "ADMIN.";        //PACS DataBase
        #endregion //DataBase 이름 세팅 

        #region //AdoConst(adoODBC.bas)
        public static int MAX_PATH = 255;   //Public Const MAX_PATH = 260 ' modmtsemrapi.bas와 vbbitnix.bas에 선언된것을 옮겨옴
        public static string AES_PASS = "fUuEt/RHUhMinUkf/nvU=ZCEilDRm96736bVsvLS71n+";   //Public Const AES_PASS = "fUuEt/RHUhMinUkf/nvU=ZCEilDRm96736bVsvLS71n+" '절대 변경하지 마세요
        #endregion //AdoConst(adoODBC.bas)



    }
}
