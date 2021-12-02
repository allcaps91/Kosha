namespace ComEmrBase
{
    public class EmrForm
    {
        public long FmFORMNO;
        public int FmUPDATENO;
        public int FmGRPFORMNO;
        public int FmDISPSEQ;
        public string FmFORMNAME;
        public string FmFORMNAMEPRINT;
        /// <summary>
        /// 0: 정형화서식, 2: 전자동의서, 3: 플로우시트, 4: 코딩서식지
        /// </summary>
        public string FmFORMTYPE;

        public string FmFORMCD;
        /// <summary>
        /// 코딩서식지 폼 이름.
        /// </summary>
        public string FmPROGFORMNAME;
        public string FmDOCFORMNAME;
        public string FmINOUTCLS;
        public int FmFORMCNT;

        public string FmFLOWGB;
        public int FmFLOWITEMCNT;
        public int FmFLOWHEADCNT;
        /// <summary>
        /// 플로우시트 입력 너비(높이) 기본값
        /// </summary>
        public int FmFLOWINPUTSIZE;

        public string FmUSEGB;
        public string FmUSEDEPT;
        public string FmVIEWAUTH;
        public string FmWRITEAUTH;
        /// <summary>
        /// 내원내역 과
        /// </summary>
        public string FmVISITSDEPT;

        public int FmUSECHECK;
        public int FmMIBICHECK;
        public int FmCERTCHECK;
        public int FmCERTBOTH;
        public int FmCERTNUM;
        public int FmALIGNGB;
        public int FmVIEWGROUP;
        public int FmDOCPRINTHEAD;

        public string FmREGDATE;
        public int FmCONVIMAGE;
        public int FmOLDGB;
        /// <summary>
        /// 0: 일반서식지 1: (고정)세로플로우시트 2: (고정)가로플로우시트 3: 가변플로우시트 4:동의서
        /// </summary>
        public int FmPRINTTYPE;

        
    }
}
