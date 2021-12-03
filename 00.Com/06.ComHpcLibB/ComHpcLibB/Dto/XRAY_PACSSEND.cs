namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class XRAY_PACSSEND : BaseDto
    {

        /// <summary>
        /// ������� �� �ð�
        /// </summary>
        public DateTime? ENTDATE { get; set; }

        /// <summary>
        /// Accession_Number
        /// </summary>
        public string PACSNO { get; set; }

        /// <summary>
        /// ���۱���(�Ʒ�����)
        /// </summary>
        public string SENDGBN { get; set; }

        /// <summary>
        /// ��Ϲ�ȣ
        /// </summary>
        public string PANO { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public string SNAME { get; set; }

        /// <summary>
        /// �����̸�
        /// </summary>
        public string ENAME { get; set; }

        /// <summary>
        /// ����(M.���� F.����)
        /// </summary>
        public string SEX { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public long AGE { get; set; }

        /// <summary>
        /// �����
        /// </summary>
        public string DEPTCODE { get; set; }

        /// <summary>
        /// �ǻ��ڵ�
        /// </summary>
        public string DRCODE { get; set; }

        /// <summary>
        /// �ܷ�,�Կ�(I/O)
        /// </summary>
        public string IPDOPD { get; set; }

        /// <summary>
        /// �����ڵ�
        /// </summary>
        public string WARDCODE { get; set; }

        /// <summary>
        /// �����ڵ�
        /// </summary>
        public string ROOMCODE { get; set; }

        /// <summary>
        /// �Կ�����
        /// </summary>
        public string XJONG { get; set; }

        /// <summary>
        /// �����ڵ�
        /// </summary>
        public string XSUBCODE { get; set; }

        /// <summary>
        /// ��缱�ڵ�
        /// </summary>
        public string XCODE { get; set; }

        /// <summary>
        /// �����ڵ�
        /// </summary>
        public string ORDERCODE { get; set; }

        /// <summary>
        /// �Կ��������� �� �ð�
        /// </summary>
        public DateTime? SEEKDATE { get; set; }

        /// <summary>
        /// �Կ����(PA,AP��)
        /// </summary>
        public string REMARK { get; set; }

        /// <summary>
        /// �Կ���
        /// </summary>
        public string XRAYROOM { get; set; }

        /// <summary>
        /// �������� �� �ð�
        /// </summary>
        public DateTime? SENDTIME { get; set; }

        /// <summary>
        /// �Կ���Ī
        /// </summary>
        public string XRAYNAME { get; set; }

        /// <summary>
        /// �ǵ���ȣ
        /// </summary>
        public long READNO { get; set; }

        /// <summary>
        /// �ǻ�REMARK
        /// </summary>
        public string DRREMARK { get; set; }

        /// <summary>
        /// SENDTIME2
        /// </summary>
        public DateTime? SENDTIME2 { get; set; }

        /// <summary>
        /// �Կ��ڼ���(EN)
        /// </summary>
        public string OPERATOR { get; set; }

        /// <summary>
        /// ���� gbinfo - 2014-05-13
        /// </summary>
        public string GBINFO { get; set; }

        /// <summary>
        /// �Է���
        /// </summary>
        public string INPS { get; set; }

        /// <summary>
        /// ���ʽð�
        /// </summary>
        public DateTime? INPT_DT { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        public string UPPS { get; set; }

        /// <summary>
        /// ����ð�
        /// </summary>
        public DateTime? UP_DT { get; set; }



        /// <summary>
        /// 
        /// </summary>
        public XRAY_PACSSEND()
        {
        }
    }
}
