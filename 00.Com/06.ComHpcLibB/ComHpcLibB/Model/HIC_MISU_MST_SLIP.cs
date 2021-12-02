namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class HIC_MISU_MST_SLIP : BaseDto
    {

        /// <summary>
        /// �̼� �Ϸù�ȣ
        /// </summary>
        public long WRTNO { get; set; }

        /// <summary>
        /// ȸ���ڵ�
        /// </summary>
        public long LTDCODE { get; set; }

        /// <summary>
        /// ȸ���ڵ�
        /// </summary>
        public string LTDNAME { get; set; }

        /// <summary>
        /// �ǰ����� �����ڵ�(ȸ��̼��� NULL)
        /// </summary>
        public string JISA { get; set; }

        /// <summary>
        /// ȸ���ȣ(ȸ��̼��� NULL)
        /// </summary>
        public string KIHO { get; set; }

        /// <summary>
        /// �̼�����(1.ȸ��̼� 2.�ǰ����� 3.����̼� 4.���ι̼� 5.���Ǽ�)
        /// </summary>
        public string MISUJONG { get; set; }

        /// <summary>
        /// û��(�߻�)����
        /// </summary>
        public string BDATE { get; set; }

        /// <summary>
        /// �̼������ڵ�
        /// </summary>
        public string GEACODE { get; set; }


        /// <summary>
        /// �̼������ڵ�
        /// </summary>
        public string JISACODE { get; set; }
        /// <summary>
        /// �̼������ڵ�
        /// </summary>
        public string KIHOCODE { get; set; }



        /// <summary>
        /// ��������(�����ڵ��� �����ڵ� ����)
        /// </summary>
        public string GJONG { get; set; }

        /// <summary>
        /// ��������(�����ڵ��� �����ڵ� ����)
        /// </summary>
        public string GJNAME { get; set; }

        /// <summary>
        /// �ԱݿϷῩ��(Y.�Ϸ� N.�̿Ϸ�)
        /// </summary>
        public string GBEND { get; set; }

        /// <summary>
        /// �̼�����(�Ʒ�����)
        /// </summary>
        public string MISUGBN { get; set; }

        /// <summary>
        /// �̼��ݾ�
        /// </summary>
        public long MISUAMT { get; set; }

        /// <summary>
        /// �Աݱݾ�
        /// </summary>
        public long IPGUMAMT { get; set; }

        /// <summary>
        /// ���ױݾ�
        /// </summary>
        public long GAMAMT { get; set; }

        /// <summary>
        /// �谨�ݾ�
        /// </summary>
        public long SAKAMT { get; set; }

        /// <summary>
        /// �ݼ۾�
        /// </summary>
        public long BANAMT { get; set; }

        /// <summary>
        /// �̼��ܾ�
        /// </summary>
        public long JANAMT { get; set; }

        /// <summary>
        /// DAMT
        /// </summary>
        public long DAMT { get; set; }


        /// <summary>
        /// ���۾� ����� �̼���ȣ
        /// </summary>
        public string GIRONO { get; set; }

        /// <summary>
        /// ����ڸ�
        /// </summary>
        public string DAMNAME { get; set; }

        /// <summary>
        /// �̼�����
        /// </summary>
        public string REMARK { get; set; }

        /// <summary>
        /// ���� �����Ͻ�
        /// </summary>
        public DateTime? ENTDATE { get; set; }

        /// <summary>
        /// ���� �۾��� ���
        /// </summary>
        public long ENTSABUN { get; set; }

        /// <summary>
        /// ���ο����� �߱޿���(Y/N)
        /// </summary>
        public string GBGIRO { get; set; }

        /// <summary>
        /// ��꼭 ���࿩��(Y/N)
        /// </summary>
        public string GBTAX { get; set; }

        /// <summary>
        /// ǰ��
        /// </summary>
        public string PUMMOK { get; set; }

        /// <summary>
        /// û������(�Ʒ�����)
        /// </summary>
        public string MIRGBN { get; set; }

        /// <summary>
        /// û������ �ݾ�
        /// </summary>
        public long MIRCHAAMT { get; set; }

        /// <summary>
        /// û������ �߻�����
        /// </summary>
        public string MIRCHAREMARK { get; set; }

        /// <summary>
        /// û�����׹߻�����
        /// </summary>
        public string MIRCHADATE { get; set; }

        /// <summary>
        /// ȸ�� �̼��߻�����(Y:�ڵ����� ,NULL or N:�����Է�)
        /// </summary>
        public string GBMISUBUILD { get; set; }

        /// <summary>
        /// �̼���ġ���׿� ��� (Y: �ӽ�����)
        /// </summary>
        public string CHK { get; set; }

        /// <summary>
        /// ���� �̼��߻�����(Y:�ڵ����� ,NULL or N:�����Է�)
        /// </summary>
        public string GBMISUBUILD2 { get; set; }

        /// <summary>
        /// ���� �����̼��߻�����(Y:�ڵ����� ,NULL or N:�����Է�)
        /// </summary>
        public string GBMISUBUILD3 { get; set; }

        /// <summary>
        /// û�����
        /// </summary>
        public string YYMM_JIN { get; set; }

        /// <summary>
        /// û����ȣ
        /// </summary>
        public long CNO { get; set; }

        /// <summary>
        /// ���Ǽ� �̼��߻�����(Y:�ڵ�����, Null or N: ����)
        /// </summary>
        public string GBMISUBUILD4 { get; set; }

        /// <summary>
        /// ���Ǽ������ڵ�(ȸ��̼��� NULL)
        /// </summary>
        public string BOGUNSO { get; set; }

        /// <summary>
        /// ���� ����⵵
        /// </summary>
        public string GJYEAR { get; set; }

        /// <summary>
        /// ���ڰ�꼭 �̹��� �����ڵ�(BCode����)
        /// </summary>
        public string TAXSAYU { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LTDCODE2 { get; set; }

        /// <summary>
        /// �̼� Slip �ݾ�
        /// </summary>
        public long SLIPAMT { get; set; }
        public string ROWID { get; set; }
        public HIC_MISU_MST_SLIP()
        {
        }
    }
}