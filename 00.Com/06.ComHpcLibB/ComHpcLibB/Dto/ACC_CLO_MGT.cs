namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class ACC_CLO_MGT : BaseDto
    {

        /// <summary>
        /// ���� ����
        /// </summary>
        public string CLO_BSNS_GB { get; set; }

        /// <summary>
        /// ���� ����
        /// </summary>
        public string CLO_YMD { get; set; }

        /// <summary>
        /// �� ���� ����
        /// </summary>
        public string DD_CLO_YN { get; set; }

        /// <summary>
        /// �� ���� ����
        /// </summary>
        public string MM_CLO_YN { get; set; }

        /// <summary>
        /// �� ���� ����
        /// </summary>
        public string YY_CLO_YN { get; set; }

        /// <summary>
        /// �Է��Ͻ�
        /// </summary>
        public DateTime? CREATED { get; set; }

        /// <summary>
        /// �Է�ID
        /// </summary>
        public string CREATEDUSER { get; set; }

        /// <summary>
        /// �Է�IP
        /// </summary>
        public string CREATEDIP { get; set; }

        /// <summary>
        /// �����Ͻ�
        /// </summary>
        public DateTime? MODIFIED { get; set; }

        /// <summary>
        /// ����ID
        /// </summary>
        public string MODIFIEDUSER { get; set; }

        /// <summary>
        /// ����IP
        /// </summary>
        public string MODIFIEDIP { get; set; }



        public ACC_CLO_MGT()
        {
        }
    }
}
