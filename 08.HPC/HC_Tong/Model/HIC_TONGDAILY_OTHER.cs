namespace HC_Tong.Model
{
    using ComBase.Mvc;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class HIC_TONGDAILY_OTHER : BaseDto
    {

        /// <summary>
        /// �������
        /// </summary>
        public string TDATE { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public string GJJONG { get; set; }

        /// <summary>
        /// ����(1.1�� 2.2�� 3.��Ÿ)
        /// </summary>
        public string CHASU { get; set; }

        /// <summary>
        /// �������(Y.���� N.����)
        /// </summary>
        public string GBCHUL { get; set; }

        /// <summary>
        /// �����ο���
        /// </summary>
        public long JEPCNT { get; set; }

        /// <summary>
        /// ���ϼ��� �Ѱ�����
        /// </summary>
        public long TOTAMT { get; set; }

        /// <summary>
        /// ���ϼ��� ���պδ��
        /// </summary>
        public long JOHAPAMT { get; set; }

        /// <summary>
        /// ���ϼ��� ȸ��δ��
        /// </summary>
        public long LTDAMT { get; set; }

        /// <summary>
        /// ���ϼ��� ���κδ��
        /// </summary>
        public long BONINAMT { get; set; }

        /// <summary>
        /// ���ϼ��� ���αݾ�
        /// </summary>
        public long HALINAMT { get; set; }

        /// <summary>
        /// ���ϼ��� �̼��ݾ�
        /// </summary>
        public long MISUAMT { get; set; }

        /// <summary>
        /// ���ϼ��� �����Աݾ�
        /// </summary>
        public long SUNAPAMT { get; set; }

        /// <summary>
        /// ��豸��(1.���������� 2.�ο����������,3.�������ݿ��༱��,4.�������)
        /// </summary>
        public string GUBUN { get; set; }

        /// <summary>
        /// ���� ���༱���� ������
        /// </summary>
        public long YEYAKAMT { get; set; }

        /// <summary>
        /// ���� ���༱���� ��ü��
        /// </summary>
        public long YDAECHE { get; set; }

        /// <summary>
        /// ���� �۾� �ð�
        /// </summary>
        public DateTime? ENTTIME { get; set; }

        /// <summary>
        /// ���� �۾��� ���
        /// </summary>
        public long JOBSABUN { get; set; }

        /// <summary>
        /// ����1(1: ����)
        /// </summary>
        public string GUBUN1 { get; set; }

        /// <summary>
        /// ���� ī��ݾ�
        /// </summary>
        public long SUNAPAMT2 { get; set; }

        /// <summary>
        /// ���ϼ��� ���Ǽұݾ�
        /// </summary>
        public long BOGENAMT { get; set; }

        /// <summary>
        /// �ǰ����� ��Ī
        /// </summary>
        public string NAME { get; set; }


        public HIC_TONGDAILY_OTHER()
        {
        }
    }
}
