namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;


    /// <summary>
    /// 
    /// </summary>
    public class OCS_ORDERCODE : BaseDto
    {
        
        /// <summary>
        /// SLIP코드
        /// </summary>
		public string SLIPNO { get; set; } 

        /// <summary>
        /// SLIP별 일련번호(0.SLIP의 Header)
        /// </summary>
		public long SEQNO { get; set; } 

        /// <summary>
        /// ORDER코드
        /// </summary>
		public string ORDERCODE { get; set; } 

        /// <summary>
        /// 수가코드
        /// </summary>
		public string SUCODE { get; set; } 

        /// <summary>
        /// 수가분류
        /// </summary>
		public string BUN { get; set; } 

        /// <summary>
        /// ORDER명칭(약:단위)
        /// </summary>
		public string ORDERNAME { get; set; } 

        /// <summary>
        /// 약:명칭
        /// </summary>
		public string ORDERNAMES { get; set; } 

        /// <summary>
        /// 수량
        /// </summary>
		public double QTY { get; set; } 

        /// <summary>
        /// 날수(SeqNo가 0인경우 SORT번호)
        /// </summary>
		public long NAL { get; set; } 

        /// <summary>
        /// A항목:(오더발생여부 1/0)
        /// </summary>
		public string GBINPUT { get; set; } 

        /// <summary>
        /// B항목:Order명 Prefix space(2*B)
        /// </summary>
		public long DISPSPACE { get; set; } 

        /// <summary>
        /// C항목:금액입력여부(1/0)
        /// </summary>
		public string GBBOTH { get; set; } 

        /// <summary>
        /// D항목:(검사추가정보 입력여부 1/0)
        /// </summary>
		public string GBINFO { get; set; } 

        /// <summary>
        /// E항목:수량,날수 입력여부(1/0)
        /// </summary>
		public string GBQTY { get; set; } 

        /// <summary>
        /// F항목:용법,검체입력여부(1/2)
        /// </summary>
		public string GBDOSAGE { get; set; } 

        /// <summary>
        /// G항목: 골수등 구분(아래참조)
        /// </summary>
		public string GBIMIV { get; set; } 

        /// <summary>
        /// H항목:ORDER SUB항목
        /// </summary>
		public string GBSUB { get; set; } 

        /// <summary>
        /// I항목:삭제여부(N:삭제,NULL:오더가능)
        /// </summary>
		public string SENDDEPT { get; set; } 

        /// <summary>
        /// J항목:1.관리과 2.공급실전달
        /// </summary>
		public string GBGUME { get; set; } 

        /// <summary>
        /// 용법코드
        /// </summary>
		public string SPECCODE { get; set; } 

        /// <summary>
        /// FollowUP코드(B.관리과수령물품)
        /// </summary>
		public string NEXTCODE { get; set; } 

        /// <summary>
        /// 검사코드
        /// </summary>
		public string ITEMCD { get; set; } 

        /// <summary>
        /// Sub 항목 계열코드
        /// </summary>
		public string SUBRATE { get; set; } 

        /// <summary>
        /// ICON파일의 경로
        /// </summary>
		public string DISPHEADER { get; set; } 

        /// <summary>
        /// 색상
        /// </summary>
		public string DISPRGB { get; set; } 

        /// <summary>
        /// 외래용법
        /// </summary>
		public string ODOSCODE { get; set; } 

        /// <summary>
        /// 병동용법
        /// </summary>
		public string IDOSCODE { get; set; } 

        /// <summary>
        /// 팍스전송명칭 사용안함
        /// </summary>
		public string PACSNAME { get; set; } 

        /// <summary>
        /// 팍스검사코드
        /// </summary>
		public string PACS_CODE { get; set; } 

        /// <summary>
        /// 약수가명칭-2014-06-09 (ocs_druginfo_new)
        /// </summary>
		public string DRUGNAME { get; set; } 

        /// <summary>
        /// ACT 전송 - Y : 외래수납전 입원한케이스 OS 물품만
        /// </summary>
		public string GBACTSEND { get; set; } 

        /// <summary>
        /// 표준코드
        /// </summary>
		public string BCODE { get; set; } 

        /// <summary>
        /// 4대중증코드 여부
        /// </summary>
		public string SEVERE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string CORDERCODE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string CSUCODE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string CBUN { get; set; } 

        
        /// <summary>
        /// OCS 오더발생용 코드 마스타
        /// </summary>
        public OCS_ORDERCODE()
        {
        }
    }
}
