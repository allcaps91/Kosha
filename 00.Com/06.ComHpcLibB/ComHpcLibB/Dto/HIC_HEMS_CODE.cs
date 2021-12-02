namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;

    public class HIC_HEMS_CODE : BaseDto
    {
        /// <summary>
        /// 구분-아래참조
        /// </summary>
        public string GUBUN { get; set; }

        /// <summary>
        /// 코드1
        /// </summary>
        public string CODE1 { get; set; }

        /// <summary>
        /// 코드2
        /// </summary>
        public string CODE2 { get; set; }

        /// <summary>
        /// 코드3
        /// </summary>
        public string CODE3 { get; set; }

        /// <summary>
        /// 코드4
        /// </summary>
        public string CODE4 { get; set; }

        /// <summary>
        /// 내용
        /// </summary>
        public string NAME { get; set; }

        public string RID { get; set; }
        

        public HIC_HEMS_CODE()
        {
        }
    }
}
