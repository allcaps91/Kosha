using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 사용자 정보
/// </summary>
namespace ComBase
{
    /// <summary>
    /// 사용자 정보
    /// </summary>
    public class clsUser : MTSDisposable
    {
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        //OCS
        public string GnJobSabun = "";
        public string GstrJobName = "";
        public string GstrJobPart = "";
        public string GstrJobGrade = "";


        //EMR
        public string strAcert; //전자인증 사용여부 1: 사용 0: 사용안함 
        public string strCertId; //전자인증 아이디 
        public string strCertPw; //전자인증 비밀번호 
        //EMR 권한 관련 정보
        public string AuAVIEW;   //조회
        public string AuAPRINT;  //출력
        public string AuAPRINTOUT;   //외부출력
        public string AuAPRINTSIM;   //심사용출력
        public string AuASCAN;   //스캔 권한
        public string AuAVERIFY; //검수 권한
        public string AuACOPY;   //사본 발급
        public string AuAWRITE;  //작성 권한
        
        /// <summary>
        /// 사용자 정보 가져오기
        /// </summary>
        public void GetUserInfo()
        {

        }




    }
}
