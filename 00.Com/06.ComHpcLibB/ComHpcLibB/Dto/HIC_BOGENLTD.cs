namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_BOGENLTD : BaseDto
    {



        /// <summary>
        /// ȸ���ڵ�
        /// </summary>
        public long LTDCODE { get; set; }

        /// <summary>
        /// �����������
        /// </summary>
        public DateTime? GEDATE { get; set; }

        /// <summary>
        /// ����ο�
        /// </summary>
        public long INWON { get; set; }

        /// <summary>
        /// ���ܰ�
        /// </summary>
        public double PRICE { get; set; }

        /// <summary>
        /// ���ݾ�
        /// </summary>
        public long AMT { get; set; }

        /// <summary>
        /// ���� �ο�
        /// </summary>
        public long OLDINWON { get; set; }

        /// <summary>
        /// ���� �ܰ�
        /// </summary>
        public long OLDPRICE { get; set; }

        /// <summary>
        /// ���� �ݾ�
        /// </summary>
        public long OLDAMT { get; set; }

        /// <summary>
        /// ����������
        /// </summary>
        public DateTime? DELDATE { get; set; }

        /// <summary>
        /// ���� �۾��� ���
        /// </summary>
        public long JOBSABUN { get; set; }

        /// <summary>
        /// ���� ���� �Ͻ�
        /// </summary>
        public DateTime? ENTTIME { get; set; }

        /// <summary>
        /// ��뿩��
        /// </summary>
        public string USE { get; set; }

        /// <summary>
        /// �ܰ��ݾ�(�ο�*�ܰ�)
        /// </summary>
        public long PRAMT { get; set; }

        /// <summary>
        /// ���� �ܰ��ݾ�(�ο�*�ܰ�)
        /// </summary>
        public long OLDPRAMT { get; set; }

        /// <summary>
        /// ���װ�࿩��
        /// </summary>
        public string FIX { get; set; }

        /// <summary>
        /// ��½� ��꼭 �ο�&�ܰ� ǥ�ÿ���
        /// </summary>
        public string BILL { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LTDCODE2 { get; set; }

        public HIC_BOGENLTD()
        {
        }
    }
}
