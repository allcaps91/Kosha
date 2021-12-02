using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.Core.Service
{
    public class Session
    {
        public string UserId { get; set; }
        public string Program { get; set; }
        public Role Role { get; set; }
        //   public string UserName { get; set; }
        public Session(string userId)
        {
            this.UserId = userId;
        }
    }

    public enum Role
    {
        NONE, DOCTOR, NURSE, ENGINEER, MEASUREMENT, ANALYST
    }

    public enum Program
    {
        WEM, OSHA
    }

    public enum MailType
    {
        /// <summary>
        /// 상태보고서 - 사업장 의사
        /// </summary>
        STATUS_COMPANY_DOCTOR,
        /// <summary>
        /// 상태보고서 - 사업장 간호사
        /// </summary>
        STATUS_COMPANY_NURS,
        /// <summary>
        /// 상태보고서 - 사업장 위생사
        /// </summary>
        STATUS_COMPANY_ENGINEER,
        /// <summary>
        /// 상태보고서 - 근로자 의사
        /// </summary>
        STATUS_WORKER_DOCTOR,
        /// <summary>
        /// 상태보고서 - 근로자 간호사
        /// </summary>
        STATUS_WORKER_NURS,
        /// <summary>
        /// 상태보고서 - 근로자 위생사
        /// </summary>
        STATUS_WORKER_ENGINEER,
        /// <summary>
        /// 방문공문
        /// </summary>
        VISIT,
        /// <summary>
        /// 청구공문
        /// </summary>
        CHARGE,
    }
}
