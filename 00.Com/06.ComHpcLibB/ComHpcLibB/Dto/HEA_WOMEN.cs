namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;

    /// <summary>
    /// 
    /// </summary>
    public class HEA_WOMEN : BaseDto
    {   
        /// <summary>
        /// 접수번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 난포자극호르몬(E908)최저
        /// </summary>
		public string MIN_ONE { get; set; } 

        /// <summary>
        /// 난포자극호르몬(E908)최고
        /// </summary>
		public string MAX_ONE { get; set; } 

        /// <summary>
        /// 에스트라데올(E909)최저
        /// </summary>
		public string MIN_TWO { get; set; } 

        /// <summary>
        /// 에스트라데올(E909)최고
        /// </summary>
		public string MAX_TWO { get; set; } 

        /// <summary>
        /// 단위
        /// </summary>
		public string UNIT { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 종검 여성정밀 참고치
        /// </summary>
        public HEA_WOMEN()
        {
        }
    }
}
