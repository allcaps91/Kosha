namespace ComEmrBase
{
    public class EasParam
    {
        public string Ptno { get; set; }
        public string Name { get; set; }
        public string DoctorName { get; set; }
        public string DeptName { get; set; }
        public string Sex { get; set; }
        public string Jumin { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string Hp { get; set; }
        public string BirthDay { get; set; }
        public string Bohumsa { get; set; }
        /// <summary>
        /// 보험종별
        /// </summary>
        public string Bohum { get; set; }
        /// <summary>
        /// 급여 보험종별
        /// </summary>
        public string BohumJong { get; set; }
        /// <summary>
        /// 입원경로
        /// </summary>
        public string FromAdmission { get; set; }

        //병동
        public string Ward { get; set; }
        /// <summary>
        /// 병실
        /// </summary>
        public string Room { get; set; }
        /// <summary>
        ///  인실
        /// </summary>
        public string RoomType { get; set; }
        /// <summary>
        /// 병실차액
        /// </summary>
        public string RoomOverAmt { get; set; }

        /// <summary>
        /// 컨트롤 초기값 연동
        /// </summary>
        public string ControlInit { get; set; }
    }
}
