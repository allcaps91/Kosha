using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;

namespace ComBase
{
    /// <summary>
    /// 전자인증 Dll 로드
    /// </summary>
    public class clsCertImport
    {
        // API초기화
        [DllImport("LibEMRClient.dll")]
        public extern static uint EMR_Initialize(
            char[] pcIP,
            int nPort,
            char[] pcIP1,
            int nPort1,
            char[] pcHospitalCode,
            int bShowErrorMsg
        );

        // API 릴리즈
        [DllImport("LibEMRClient.dll")]
        public extern static int EMR_Release(uint nCtx);

        // 로그인 
        [DllImport("LibEMRClient.dll")]
        public extern static int EMR_Roaming(
            uint nCtx,
            Byte[] pcSid
        );


        // 에러메세지를 가져온다.
        [DllImport("LibEMRClient.dll")]
        public extern static int EMR_GetCodeToMsg(
            uint nCtx,                  // in
            int nErrCode,               // in
            Byte[] szErrorMsg,          // out
            ref int pnErrorMsgLen       // out
        );


        // 전자서명, 서명 데이터 생성 후 서명값의 길이만 리턴한다.
        [DllImport("LibEMRClient.dll")]
        public extern static int EMR_SignValue(
            uint nCtx,
            Byte[] pcSid,
            Byte[] pucEMRData, // in
            int nEMRDataLen,
            ref int pnSignedDataLen
        );

        // 파일로부터 서명 데이터를 생성한후 서명값의 길이만을 Return 한다.        
        [DllImport("LibEMRClient.dll")]
        public extern static int EMR_SignFile(
            uint nCtx,
            Byte[] pcSid,
            Byte[] pucEMRFilePath,
            int nEMRFilePathLen,
            ref int pnSignedDataLen
        );

        // 전자서명, 서명값의 길이만큼 서명값을 받아온다.
        [DllImport("LibEMRClient.dll")]
        public extern static int EMR_GetSignValue(
            uint nCtx,
            Byte[] aucOutSignedData,
            ref int pnSignedDataLen
        );

        // 서명검증, EMR_Server
        [DllImport("LibEMRClient.dll")]
        public extern static int EMR_ServerVerifySignedData(
            uint nCtx,
            Byte[] pcSid,                // in
            Byte[] pcRoamingMessage,     // in
            int nRoamingMessageLen,      // in
            Byte[] acSignedData,    // out
            ref int pnSignedDataLen,       // out
            Byte[] acEMRDataOutput, // out
            ref int pnEMRDataLen           // out
        );

        // 서명검증
        [DllImport("LibEMRClient.dll")]
        public extern static int EMR_VerifySignedData(
            uint nCtx,
            Byte[] pcSid,
            Byte[] pucSignedData,
            int nSignedDataLen,
            Byte[] aucOutContents,
            ref int pnContentsLen
        );

        // Hash Sign, 
        [DllImport("LibEMRClient.dll")]
        public extern static int EMR_HashSign(
            uint nCtx,
            Byte[] pcSid,
            Byte[] pucEMRData,
            int nEMRDataLen,
            Byte[] aucOutSignedData,
            ref int pnOutSignedDataLen
        );

        // HashVerify 해쉬값의 서명 검증을 수행
        [DllImport("LibEMRClient.dll")]
        public extern static int EMR_HashVerify(
            uint nCtx,
            Byte[] pcSid,
            Byte[] pucEMRData,
            int nEMRDataLen,
            Byte[] pucSignedData,
            int nSignedDataLen
        );

        // Hash
        [DllImport("LibEMRClient.dll")] //, CallingConvention = CallingConvention.Cdecl)]
        public extern static int EMR_Hash(
            uint nCtx,
            int nHashAlg,
            Byte[] pucEMRData,
            int nEMRDataLen,
            Byte[] aucOutHashedData,
            ref int pnOutHashedDataLen
        );

        // 파일로부터 Hash
        [DllImport("LibEMRClient.dll")]
        public extern static int EMR_HashFromFile(
            uint nCtx,
            int nHashAlg,
            Byte[] acFileData,    // File In Path
            Byte[] acFilePlainData, // File Out Data
            Byte[] aucOutHashedData, // Hashed Data
            ref int pnOutHashedDataLen
        );

        // 파일로부터 Hash
        [DllImport("LibEMRClient.dll")]
        public extern static int EMR_HashedFile(
            uint nCtx,
            int nHashAlg,
            Byte[] acFileData,    // File In Path
            Byte[] aucOutHashedData, // Hashed Data
            ref int pnOutHashedDataLen
        );

        // 인증서 등록 및 유효기간 표시
        [DllImport("LibEMRClient.dll")]
        public extern static int EMR_RegisterCertWithTerm(
            uint nCtx,
            Byte[] pcSid,
            Byte[] pcStartDate,
            Byte[] pcEndDate
        );


        // 서명 데이터에 들어 있는 서명에 사용한 인증서를 검증한다.
        [DllImport("LibEMRClient.dll")]
        public extern static int EMR_VerifyCert(
            uint nCtx,
            Byte[] pcSid,
            Byte[] pucSignedData,
            int nSignedDataLen
        );

        // 서명 데이터에 들어 있는 서명에서 DN값을 가져온다.
        [DllImport("LibEMRClient.dll")]
        public extern static int EMR_GetDNFromSignedData(
            uint nCtx,
            Byte[] pcSid,
            Byte[] pcSignedData,
            int nSignedDataLen,
            Byte[] btSubjectFullDN
        );


        // 서명데이타에서 인증서를 추출 하여 인증서 보기를 수행한다.
        [DllImport("LibEMRClient.dll")]
        public extern static int EMR_ViewCertFromSignedData(
            uint nCtx,
            Byte[] pcSid,
            Byte[] pucSignedData,
            int nSignedDataLen
        );

        // 서명데이타에서 서명시간을 가져온다..
        [DllImport("LibEMRClient.dll")]
        public extern static int EMR_GetSigningTime(
            uint nCtx,
            Byte[] pcSid,
            Byte[] pcSignedData,
            int nSignedDataLen,
            Byte[] acSigningTime
        );

        // 로밍서버에 인증서 등록(인증서 창 없이)
        [DllImport("LibEMRClient.dll")]
        public extern static int EMR_RegisterCertAuto(
            uint nCtx,
            Byte[] pcSid,
            Byte[] pcCertPath,
            Byte[] pcKeyPath,
            Byte[] pcCertPwd,
            Byte[] pcStartDate,
            Byte[] pcEndDate
        );

        // 인증서를 발급
        [DllImport("LibEMRClient.dll")]
        public extern static int EMR_NewIssueCertValidity(
            uint nCtx,
            Byte[] pcSid
        );

        // 인증서를 재발급
        [DllImport("LibEMRClient.dll")]
        public extern static int EMR_ReissueCertValidity(
            uint nCtx,
            Byte[] pcSid
        );

        // 인증서를 갱신
        [DllImport("LibEMRClient.dll")]
        public extern static int EMR_UpdateCertValidity(
            uint nCtx,
            Byte[] pcSid
        );

        // 인증서를 보기를 수행
        [DllImport("LibEMRClient.dll")]
        public extern static int EMR_ViewCert(
            uint nCtx,
            Byte[] pcSid
        );

        // 인증서 암호를 변경 한다.(비밀번호 유효성검사여부를 결정한다.)        
        // nCheck : 0 유효성 검사 안함, 1 : 유효성 검사 함.                             
        [DllImport("LibEMRClient.dll")]
        public extern static int EMR_ChangeCertPasswordWPCheck(
            uint nCtx,
            Byte[] pcSid,
            Byte[] acNewPriKeyPwd,
            int nCheck
        );

        // 로밍 서버에 저장되어 있는 인증서를 삭제한다.                                 
        [DllImport("LibEMRClient.dll")]
        public extern static int EMR_DeleteCert(
            uint nCtx,
            Byte[] pcSid
        );

        // 인증서의 유효기간을 받아온다.                                                
        [DllImport("LibEMRClient.dll")]
        public extern static int EMR_GetTermOfValidity(
            uint nCtx,
            Byte[] pcSid,
            Byte[] pcStartDate,
            Byte[] pcEndDate
        );

        // 인증서의 DN을 받아온다.			                                            
        [DllImport("LibEMRClient.dll")]
        public extern static int EMR_GetSubjectFullDN(
            uint nCtx,
            Byte[] pcSid,
            Byte[] pcSubjectFullDN
        );

        // 로컬 인증서를 삭제한다. 			                                            
        [DllImport("LibEMRClient.dll")]
        public extern static int EMR_DeleteLocalCert(
            uint nCtx,
            Byte[] pcSid
        );

        // 로컬PC에 인증서를 저장한다. 		                                            
        [DllImport("LibEMRClient.dll")]
        public extern static int EMR_SaveLocalCert(
            uint nCtx,
            Byte[] pcSid
        );


        /////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////



        // 로밍서버에 인증서를 등록한다.        
        [DllImport("LibEMRClient.dll")]
        public extern static int EMR_RegisterCert(
            uint nCtx,
            Byte[] pcSid
        );

        // 창없이 로그인
        [DllImport("LibEMRClient.dll")]
        public extern static int EMR_RoamingWP(
            uint nCtx,
            Byte[] pcSid,
            Byte[] pcPriPass
        );

        // 클라이언트 API를 사용하다가 에러가 발생한 경우 다음 API를 이용하여   
        // 일반 에러 메시지를 가져 올 수 있다.                                          
        // 로그인(EMR_Roaming)에 주석처리
        [DllImport("LibEMRClient.dll")]
        public extern static int EMR_GetGeneralErrorMsg(
            uint nCtx,
            StringBuilder szErrorMsg,
            ref int pnErrorMsgLen
        );

        // 클라이언트 API를 사용하다가 에러가 발생한 경우 다음 API를 이용하여   
        // 자세한 에러 메시지를 가져 올 수 있다.                                
        // 로그인(EMR_Roaming)에 주석처리
        [DllImport("LibEMRClient.dll")]
        public extern static int EMR_GetDetailErrorMsg(
            uint nCtx,
            StringBuilder szErrorMsg,
            ref int pnErrorMsgLen
        );

        /************************************************************************/
        /* 인증서 암호를 변경 한다.                                             */
        /************************************************************************/
        [DllImport("LibEMRClient.dll")]
        public extern static int EMR_ChangeCertPassword(
            uint nCtx,
            Byte[] pcSid
        );


        /************************************************************************/
        /* 인증서 암호를 변경 한다.(비밀번호를 리턴 받는다.)                    */
        /************************************************************************/
        [DllImport("LibEMRClient.dll")]
        public extern static int EMR_ChangeCertPasswordWP(
            uint nCtx,
            Byte[] pcSid,
            Byte[] acNewPriKeyPwd
        );
    }
}
