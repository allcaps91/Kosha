namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class HIC_MISU_MST_LTD : BaseDto
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
        /// ��������(�����ڵ��� �����ڵ� ����)
        /// </summary>
        public string GJONG { get; set; }

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
        /// ////////////////////////////////////////
        /// </summary>
        /// 


        /// <summary>
        /// ��ü�ڵ�
        /// </summary>
        public long CODE { get; set; }

        /// <summary>
        /// ��ȣ
        /// </summary>
        public string SANGHO { get; set; }

        /// <summary>
        /// ��Ī ��ȣ
        /// </summary>
        public string NAME { get; set; }

        /// <summary>
        /// ��ȭ��ȣ
        /// </summary>
        public string TEL { get; set; }

        /// <summary>
        /// FAX��ȣ
        /// </summary>
        public string FAX { get; set; }

        /// <summary>
        /// E-MAIL��ȣ
        /// </summary>
        public string EMAIL { get; set; }

        /// <summary>
        /// �����ȣ
        /// </summary>
        public string MAILCODE { get; set; }

        /// <summary>
        /// �ּ�
        /// </summary>
        public string JUSO { get; set; }

        /// <summary>
        /// ����ڵ�Ϲ�ȣ
        /// </summary>
        public string SAUPNO { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public string UPTAE { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public string JONGMOK { get; set; }

        /// <summary>
        /// ��ǥ�ڼ���
        /// </summary>
        public string DAEPYO { get; set; }

        /// <summary>
        /// ��ǥ�� �ֹι�ȣ
        /// </summary>
        public string JUMIN { get; set; }


        /// <summary>
        /// �����ڵ�
        /// </summary>
        public string UPJONG { get; set; }

        /// <summary>
        /// ���缺����ȣ
        /// </summary>
        public string SANKIHO { get; set; }

        /// <summary>
        /// ��������ڵ�
        /// </summary>
        public string GWANSE { get; set; }

        /// <summary>
        /// �������ڵ�
        /// </summary>
        public string JIDOWON { get; set; }

        /// <summary>
        /// ���Ǵ����
        /// </summary>
        public string BONAME { get; set; }

        /// <summary>
        /// ���Ǵ���� ����
        /// </summary>
        public string BOJIK { get; set; }

        /// <summary>
        /// �Ը𱸺�(1.50�ι̸� 2.300�̸� 3.1000�̸� 4.1000�̻�)
        /// </summary>
        public string GYUMOGBN { get; set; }

        /// <summary>
        /// ���ù�ȣ
        /// </summary>
        public string GESINO { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public DateTime? SELDATE { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        public string GYEDATE { get; set; }

        /// <summary>
        /// ��ǰ�ڵ�1
        /// </summary>
        public string JEPUM1 { get; set; }

        /// <summary>
        /// ��ǰ�ڵ�2
        /// </summary>
        public string JEPUM2 { get; set; }

        /// <summary>
        /// ��ǰ�ڵ�3
        /// </summary>
        public string JEPUM3 { get; set; }

        /// <summary>
        /// ��ǰ�ڵ�4
        /// </summary>
        public string JEPUM4 { get; set; }

        /// <summary>
        /// ��ǰ�ڵ�5
        /// </summary>
        public string JEPUM5 { get; set; }

        /// <summary>
        /// ��������(Y/N)
        /// </summary>
        public string GBGEMJIN { get; set; }

        /// <summary>
        /// ��������(Y/N)
        /// </summary>
        public string GBCHUKJENG { get; set; }

        /// <summary>
        /// ��������(Y/N)
        /// </summary>
        public string GBDAEHANG { get; set; }

        /// <summary>
        /// ��������(Y/N)
        /// </summary>
        public string GBJONGGUM { get; set; }

        /// <summary>
        /// ��������(Y/N)
        /// </summary>
        public string GBGUKGO { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public DateTime? DELDATE { get; set; }

        /// <summary>
        /// �ֿ����ǰ
        /// </summary>
        public string JEPUMLIST { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        public string ARMY_HSP { get; set; }

        /// <summary>
        /// �����ұ�ȣ
        /// </summary>
        public string YOUNGUPSO { get; set; }

        /// <summary>
        /// �����Ҹ�
        /// </summary>
        public string UPSONAME { get; set; }

        /// <summary>
        /// ���ּ�
        /// </summary>
        public string JUSODETAIL { get; set; }

        /// <summary>
        /// ���˰������
        /// </summary>
        public DateTime? NEGODATE { get; set; }

        /// <summary>
        /// ���� ���� �ݾ�
        /// </summary>
        public long MAMT { get; set; }

        /// <summary>
        /// ���� ���� �ݾ�
        /// </summary>
        public long FAMT { get; set; }

        /// <summary>
        /// ȸ���ο�
        /// </summary>
        public long INWON { get; set; }

        /// <summary>
        /// 0.�ش����  1.�ʵ��б�   2.��/���
        /// </summary>
        public string GBSCHOOL { get; set; }

        /// <summary>
        /// Ư��û������(Y/N)
        /// </summary>
        public string SPCHUNGGU { get; set; }

        /// <summary>
        /// HEMS ���ۿ� ����� ��ȣ
        /// </summary>
        public string HEMSNO { get; set; }

        /// <summary>
        /// ���� �������
        /// </summary>
        public string HAREMARK { get; set; }

        /// <summary>
        /// �޴���ȭ ��ȣ(���幮�ڸ޽��� ���ۿ�)
        /// </summary>
        public string HTEL { get; set; }

        /// <summary>
        /// ���� ������ ���� ����� ����(Y.����)
        /// </summary>
        public string GBGARESV { get; set; }

        /// <summary>
        /// ���� ���˰�����(����1��) ����(Y/N)
        /// </summary>
        public string HEAGAJEPSU1 { get; set; }

        /// <summary>
        /// ���� ���˰�����(�ϰ���) ����(Y/N)
        /// </summary>
        public string HEAGAJEPSU2 { get; set; }

        /// <summary>
        /// ���� ���˰�����(����1��) ����(Y/N)
        /// </summary>
        public string HEAGAJEPSU3 { get; set; }

        /// <summary>
        /// ���� ���˰�����(�ϰ���) ����(Y/N)
        /// </summary>
        public string HEAGAJEPSU4 { get; set; }

        /// <summary>
        /// ä��,��ġ��,Ư�� ���޻���
        /// </summary>
        public string SPC_REMARK { get; set; }

        /// <summary>
        /// ���ڼ��ݰ�꼭 ���� �������
        /// </summary>
        public string TAX_REMARK { get; set; }

        /// <summary>
        /// ��������ȣ(2015-07-01 �ż�)
        /// </summary>
        public string JSAUPNO { get; set; }

        /// <summary>
        /// �����ȣ_�ǹ���ȣ
        /// </summary>
        public string BUILDNO { get; set; }

        /// <summary>
        /// ���ڰ�꼭�� �����ȣ
        /// </summary>
        public string TAX_MAILCODE { get; set; }

        /// <summary>
        /// ���ڰ�꼭�� �ּ�
        /// </summary>
        public string TAX_JUSO { get; set; }

        /// <summary>
        /// ���ڰ�꼭�� ���ּ�
        /// </summary>
        public string TAX_JUSODETAIL { get; set; }

        /// <summary>
        /// ����ȸ�� �ڵ�(�¶��θ޵� ��)
        /// </summary>
        public long DLTD { get; set; }

        /// <summary>
        /// �������ڵ�(2017-02-18)
        /// </summary>
        public string CODE2 { get; set; }

        /// <summary>
        /// <������>
        /// </summary>
        public string DLTD2 { get; set; }

        /// <summary>
        /// ������� �Ұ��� ����
        /// </summary>
        public string CHULNOTSAYU { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CHREMARK { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BOREMARK { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LTDCHARGE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LTDGRADE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LTDHPHONE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LTDEMAIL { get; set; }

public HIC_MISU_MST_LTD()
        {
        }
    }
}