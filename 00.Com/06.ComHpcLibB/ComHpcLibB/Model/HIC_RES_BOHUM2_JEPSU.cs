namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_RES_BOHUM2_JEPSU : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public string IPSADATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string JEPDATE { get; set; }

        /// <summary>
        /// �����Ϸù�ȣ
        /// </summary>
		public long WRTNO { get; set; }

        /// <summary>
        /// �����⵵
        /// </summary>
        public string GJYEAR { get; set; }

        /// <summary>
        /// ��Ϲ�ȣ
        /// </summary>
        public long PANO { get; set; }

        /// <summary>
        /// �����ڸ�
        /// </summary>
        public string SNAME { get; set; }

        /// <summary>
        /// ����(M.���� F.����)
        /// </summary>
        public string SEX { get; set; }

        /// <summary>
        /// ����(�������ڱ��� �� ����)
        /// </summary>
        public long AGE { get; set; }

        /// <summary>
        /// �������� �ڵ�
        /// </summary>
        public string GJJONG { get; set; }

        /// <summary>
        /// ��������(1.1�� 2.2�� 3.��Ÿ����)
        /// </summary>
        public string GJCHASU { get; set; }

        /// <summary>
        /// �����б�(1.���ݱ� 2.�Ϲݱ� 3.��Ÿ)
        /// </summary>
        public string GJBANGI { get; set; }

        /// <summary>
        /// �����������(Y.������� N.��������)
        /// </summary>
        public string GBCHUL { get; set; }

        /// <summary>
        /// �۾�����(�Ʒ�����)
        /// </summary>
        public string GBSTS { get; set; }

        /// <summary>
        /// �ο���豸��
        /// </summary>
        public string GBINWON { get; set; }

        /// <summary>
        /// �����������
        /// </summary>
        public DateTime? DELDATE { get; set; }

        /// <summary>
        /// ȸ���ڵ�
        /// </summary>
        public long LTDCODE { get; set; }

        /// <summary>
        /// �����ȣ
        /// </summary>
        public string MAILCODE { get; set; }

        /// <summary>
        /// �ּ�1
        /// </summary>
        public string JUSO1 { get; set; }

        /// <summary>
        /// �ּ�2
        /// </summary>
        public string JUSO2 { get; set; }

        /// <summary>
        /// ��ȭ��ȣ
        /// </summary>
        public string TEL { get; set; }

        /// <summary>
        /// ���� ��Ϲ�ȣ
        /// </summary>
        public string PTNO { get; set; }

        /// <summary>
        /// �δ��� �ڵ�(�Ʒ�����)
        /// </summary>
        public string BURATE { get; set; }

        /// <summary>
        /// �ǰ����� �����ڵ�
        /// </summary>
        public string JISA { get; set; }

        /// <summary>
        /// ������ȣ
        /// </summary>
        public string KIHO { get; set; }

        /// <summary>
        /// ����ȣ
        /// </summary>
        public string GKIHO { get; set; }

        /// <summary>
        /// ��������(1.������ 2.Ư���� 3.�繫��)
        /// </summary>
        public string JIKGBN { get; set; }

        /// <summary>
        /// ��������(�������� �Է� ����:�ĸ��� �и�)
        /// </summary>
        public string UCODES { get; set; }

        /// <summary>
        /// ���ð˻� �ڵ�
        /// </summary>
        public string SEXAMS { get; set; }

        /// <summary>
        /// �����ڵ�
        /// </summary>
        public string JIKJONG { get; set; }

        /// <summary>
        /// �����ȣ(�����)
        /// </summary>
        public string SABUN { get; set; }

        /// <summary>
        /// ���۾��μ���
        /// </summary>
        public string BUSENAME { get; set; }

        /// <summary>
        /// �������ڵ�
        /// </summary>
        public string OLDJIKJONG { get; set; }

        /// <summary>
        /// ������������
        /// </summary>
        public DateTime? BUSEIPSA { get; set; }

        /// <summary>
        /// ������ �ٹ� ������
        /// </summary>
        public DateTime? OLDSDATE { get; set; }

        /// <summary>
        /// ������ �ٹ� ������
        /// </summary>
        public DateTime? OLDEDATE { get; set; }

        /// <summary>
        /// ��ø��������(Y/N)
        /// </summary>
        public string GBSUCHEP { get; set; }

        /// <summary>
        /// �߱޳⵵
        /// </summary>
        public string BALYEAR { get; set; }

        /// <summary>
        /// �߱޳⵵�� �Ϸù�ȣ
        /// </summary>
        public long BALSEQ { get; set; }

        /// <summary>
        /// ���ڵ���࿩��(Y/N or NULL)
        /// </summary>
        public string GBEXAM { get; set; }

        /// <summary>
        /// ���Ǽ�
        /// </summary>
        public string BOGUNSO { get; set; }

        /// <summary>
        /// ��������(1.�������� 2.������)
        /// </summary>
        public string JEPSUGBN { get; set; }

        /// <summary>
        /// �������ڵ�
        /// </summary>
        public string YOUNGUPSO { get; set; }

        /// <summary>
        /// û������ȣ
        /// </summary>
        public string JEPNO { get; set; }

        /// <summary>
        /// Ư������ �����Է� ����(Y:�Է� N.���Է�)
        /// </summary>
        public string GBSPCMUNJIN { get; set; }

        /// <summary>
        /// ����� �μ⿩��(������).. �̾���
        /// </summary>
        public string GBPRINT { get; set; }

        /// <summary>
        /// ������õ���
        /// </summary>
        public long GBSABUN { get; set; }

        /// <summary>
        /// �Ϲ�������㿩��(Y:���� NULL:������)
        /// </summary>
        public string GBJINCHAL { get; set; }

        /// <summary>
        /// �ǰ����� ���� ����(NULL:���� N:���Է� Y:�Է¿Ϸ�)
        /// </summary>
        public string GBMUNJIN1 { get; set; }

        /// <summary>
        /// �����˻� ���� ����(NULL:���� N:���Է� Y:�Է¿Ϸ�)
        /// </summary>
        public string GBMUNJIN2 { get; set; }

        /// <summary>
        /// Ư������ ���� ����(NULL:���� N:���Է� Y:�Է¿Ϸ�)
        /// </summary>
        public string GBMUNJIN3 { get; set; }

        /// <summary>
        /// �����˻� ����(Y/N): �����˻�(ZD00)
        /// </summary>
        public string GBDENTAL { get; set; }

        /// <summary>
        /// 2��������� ���� (NULL:���� N:���� Y:���)
        /// </summary>
        public string SECOND_FLAG { get; set; }

        /// <summary>
        /// 2��������(2���������� ���� ��� NULL)
        /// </summary>
        public DateTime? SECOND_DATE { get; set; }

        /// <summary>
        /// 2��������� ��������
        /// </summary>
        public DateTime? SECOND_TONGBO { get; set; }

        /// <summary>
        /// 2������ �˻��׸�(ex,1,2,L,...)
        /// </summary>
        public string SECOND_EXAMS { get; set; }

        /// <summary>
        /// 2������ ����(�������ǽ�,��������,...)
        /// </summary>
        public string SECOND_SAYU { get; set; }

        /// <summary>
        /// û������(Y.û��, " " ��û��)
        /// </summary>
        public string CHUNGGUYN { get; set; }

        /// <summary>
        /// �����˻�û��(Y.û��, " " ��û��)
        /// </summary>
        public string DENTCHUNGGUYN { get; set; }

        /// <summary>
        /// �ϰ˻�û��(Y.û��, " " ��û��)
        /// </summary>
        public string CANCERCHUNGGUYN { get; set; }

        /// <summary>
        /// 2�� ����
        /// </summary>
        public string LIVER2 { get; set; }

        /// <summary>
        /// �ݾ�
        /// </summary>
        public long MAMT { get; set; }

        /// <summary>
        /// �ݾ�
        /// </summary>
        public long FAMT { get; set; }

        /// <summary>
        /// ���ϸ����� ����
        /// </summary>
        public string MILEAGEAM { get; set; }

        /// <summary>
        /// ����ϴ�� ����
        /// </summary>
        public string MURYOAM { get; set; }

        /// <summary>
        /// �������    ��) 1.���尡���� 2.����,�Ǻξ���
        /// </summary>
        public string GUMDAESANG { get; set; }

        /// <summary>
        /// ������������
        /// </summary>
        public string JONGGUMYN { get; set; }

        /// <summary>
        /// �ڷ����� ����
        /// </summary>
        public string SEND { get; set; }

        /// <summary>
        /// ���ϸ����ϱ���
        /// </summary>
        public string MILEAGEAMGBN { get; set; }

        /// <summary>
        /// ����ϱ���
        /// </summary>
        public string MURYOGBN { get; set; }

        /// <summary>
        /// ȸ��û�� ����
        /// </summary>
        public DateTime? NEGODATE { get; set; }

        /// <summary>
        /// �۾��� ���
        /// </summary>
        public long JOBSABUN { get; set; }

        /// <summary>
        /// ���� �۾� �ð�
        /// </summary>
        public DateTime? ENTTIME { get; set; }

        /// <summary>
        /// 2������ �̽ǽ� ����
        /// </summary>
        public string SECOND_MISAYU { get; set; }

        /// <summary>
        /// �ǰ�����1,2�� û����ȣ
        /// </summary>
        public long MIRNO1 { get; set; }

        /// <summary>
        /// �������� û����ȣ
        /// </summary>
        public long MIRNO2 { get; set; }

        /// <summary>
        /// �ϰ���   û����ȣ
        /// </summary>
        public long MIRNO3 { get; set; }

        /// <summary>
        /// E-mail(����ǥ �ۼ��� ������ ����� �Է�)
        /// </summary>
        public string EMAIL { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        public string REMARK { get; set; }

        /// <summary>
        /// �������� Ư������ ���� �Ϸù�ȣ
        /// </summary>
        public long OHMSNO { get; set; }

        /// <summary>
        /// ȸ��û�� �̼���ȣ
        /// </summary>
        public long MISUNO1 { get; set; }

        /// <summary>
        /// 2���˻��� ��� 1������ �ǻ� �����ȣ
        /// </summary>
        public long FIRSTPANDRNO { get; set; }

        /// <summary>
        /// ���� ȯ�� ǥ��
        /// </summary>
        public string ERFLAG { get; set; }

        /// <summary>
        /// �ϰ��� ���Ǽ�û����ȣ
        /// </summary>
        public long MIRNO4 { get; set; }

        /// <summary>
        /// û�� ���� ����
        /// </summary>
        public string MIRSAYU { get; set; }

        /// <summary>
        /// ���� ȯ�� �뺸����
        /// </summary>
        public DateTime? ERTONGBO { get; set; }

        /// <summary>
        /// ����û�� �̼���ȣ
        /// </summary>
        public long MISUNO2 { get; set; }

        /// <summary>
        /// �Ƿ�޿��� ��� : 2005��
        /// </summary>
        public string GUBDAESANG { get; set; }

        /// <summary>
        /// �ϰ��� �Ƿ�޿���û����ȣ
        /// </summary>
        public long MIRNO5 { get; set; }

        /// <summary>
        /// ����û�� �����̼���ȣ
        /// </summary>
        public long MISUNO3 { get; set; }

        /// <summary>
        /// ��缱�Կ���ȣ()2005.07.13�߰�,2009����� ���Կܷ���ȣ���յ�
        /// </summary>
        public string XRAYNO { get; set; }

        /// <summary>
        /// �ſ�ī�� �����Ϸù�ȣ(1~9999999999)
        /// </summary>
        public long CARDSEQNO { get; set; }

        /// <summary>
        /// 69�� �߰���������
        /// </summary>
        public string GBADDPAN { get; set; }

        /// <summary>
        /// �ϰ������� ��)1,1,0,1,1,0 (��UGI,��GFS,�����,����,����,�ڱ�)
        /// </summary>
        public string GBAM { get; set; }

        /// <summary>
        /// ����߼�����
        /// </summary>
        public DateTime? BALDATE { get; set; }

        /// <summary>
        /// �����뺸����-������������
        /// </summary>
        public DateTime? TONGBODATE { get; set; }

        /// <summary>
        /// ������Ͽ��� (Y.�ڵ����)
        /// </summary>
        public string GBSUJIN_SET { get; set; }

        /// <summary>
        /// ����������(Y:������ ������ó�� N or Null: �ش���׾���)
        /// </summary>
        public string GBCHUL2 { get; set; }

        /// <summary>
        /// ����������㿩��(Y:���� NULL:������)
        /// </summary>
        public string GBJINCHAL2 { get; set; }

        /// <summary>
        /// ����ǻ� ���
        /// </summary>
        public long SANGDAMDRNO { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        public DateTime? SANGDAMDATE { get; set; }

        /// <summary>
        /// ������ ���� �޼���
        /// </summary>
        public string SENDMSG { get; set; }

        /// <summary>
        /// ����(1:�ʵ�,2:��,���)
        /// </summary>
        public string GBN { get; set; }

        /// <summary>
        /// �г�
        /// </summary>
        public long CLASS { get; set; }

        /// <summary>
        /// �й�
        /// </summary>
        public long BAN { get; set; }

        /// <summary>
        /// �й�
        /// </summary>
        public long BUN { get; set; }

        /// <summary>
        /// HEMS Data ������ȣ
        /// </summary>
        public long HEMSNO { get; set; }

        /// <summary>
        /// HEMS DATA ���� ����
        /// </summary>
        public string HEMSMIRSAYU { get; set; }

        /// <summary>
        /// �ڵ����� ���� (Y/N or Null)
        /// </summary>
        public string AUTOJEP { get; set; }

        /// <summary>
        /// ���Ǽ�û�� �̼���ȣ
        /// </summary>
        public long MISUNO4 { get; set; }

        /// <summary>
        /// ������ û���� ������ȣ
        /// </summary>
        public long P_WRTNO { get; set; }

        /// <summary>
        /// ��������(2014-08-02)
        /// </summary>
        public DateTime? PANJENGDATE { get; set; }

        /// <summary>
        /// �����ǻ� �����ȣ(2014-08-02)
        /// </summary>
        public long PANJENGDRNO { get; set; }

        /// <summary>
        /// 2�� ����û�� �˻� �����(Y/N)
        /// </summary>
        public string GBAUDIO2EXAM { get; set; }

        /// <summary>
        /// ����ǥ �μⱸ��(1.���ͳ� 2.�ۼ����� 3.�μ� NULL:���μ�)
        /// </summary>
        public string GBMUNJINPRINT { get; set; }

        /// <summary>
        /// ���������� ���޻���
        /// </summary>
        public string WAITREMARK { get; set; }

        /// <summary>
        /// �޴��� ��� ��ȸ�� PDF ���ϸ�
        /// </summary>
        public string PDFPATH { get; set; }

        /// <summary>
        /// �����ۿ���
        /// </summary>
        public string WEBSEND { get; set; }

        /// <summary>
        /// �������Ͻ�
        /// </summary>
        public DateTime? WEBSENDDATE { get; set; }

        /// <summary>
        /// �������� ����(Y/N)
        /// </summary>
        public string GBNAKSANG { get; set; }

        /// <summary>
        /// 1������ ���������� �ǽÿ���(Y/N)
        /// </summary>
        public string GBDENTONLY { get; set; }

        /// <summary>
        /// �ڵ���������(Y:����A�ڵ�����,N:���ƴ�,NULL:������)
        /// </summary>
        public string GBAUTOPAN { get; set; }

        /// <summary>
        /// ���ͳݹ�����ȣ
        /// </summary>
        public long IEMUNNO { get; set; }

        /// <summary>
        /// ��������μ� ��û�Ͻ�(NULL:�̽�û)
        /// </summary>
        public DateTime? WEBPRINTREQ { get; set; }

        /// <summary>
        /// ���� ������� Ȯ�� ����(Y/N)
        /// </summary>
        public string GBSUJIN_CHK { get; set; }

        /// <summary>
        /// ��������μ� �����Ͻ�(NULL:������)
        /// </summary>
        public DateTime? WEBPRINTSEND { get; set; }

        /// <summary>
        /// LTDCODE2
        /// </summary>
        public string LTDCODE2 { get; set; }

        /// <summary>
        /// ��������û���ݾ�
        /// </summary>
        public long DENTAMT { get; set; }

        /// <summary>
        /// ���ϰ��꿩��(Y/N)
        /// </summary>
        public string GBHUADD { get; set; }

        /// <summary>
        /// ����ȭ�� �������
        /// </summary>
        public string EXAMREMARK { get; set; }

        /// <summary>
        /// ������μ� ���
        /// </summary>
        public long PRTSABUN { get; set; }

        /// <summary>
        /// ���˳��ð�� ����(Y/N)
        /// </summary>
        public string GBHEAENDO { get; set; }

        /// <summary>
        /// �ڰ�����1(01:����� 02:������ 03:���κ�, 04:�Ƿ�޿�)
        /// </summary>
        public string GBCHK1 { get; set; }

        /// <summary>
        /// �ڰ�����2(01:�����Ƿ�޿�)
        /// </summary>
        public string GBCHK2 { get; set; }

        /// <summary>
        /// �ӽ�
        /// </summary>
        public string GBCHK3 { get; set; }

        /// <summary>
        /// ��Ȱ��������
        /// </summary>
        public string GBLIFE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GBJUSO { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// NAME (HIC_EXJONG �ǰ����� ��Ī)
        /// </summary>
		public string NAME { get; set; }

        /// <summary>
        /// �׷������Ϸù�ȣ
        /// </summary>
		public long GWRTNO { get; set; }

        /// <summary>
        /// ���� �������� (1:����, 2:Ư��, 3:��, 4:����, 5:��Ÿ)
        /// </summary>
		public string JEPBUN { get; set; }

        /// <summary>
        /// ����ǻ��
        /// </summary>
		public string SANGDAMDRNAME { get; set; }

        /// <summary>
        /// ���ð汸��
        /// </summary>
		public string ENDOGBN { get; set; }

        /// <summary>
        /// ������
        /// </summary>
		public string LTDNAME { get; set; }

        /// <summary>
        /// �ֹι�ȣ
        /// </summary>
		public string JUMINNO { get; set; }

        /// <summary>
        /// ����������������
        /// </summary>
		public string PRIVACY_DATE { get; set; }

        /// <summary>
        /// ���ͳݹ��� ����
        /// </summary>
		public string GBIEMUN { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public DateTime? MINDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public DateTime? MAXDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public DateTime? STARTDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public DateTime? ENDDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long CNT { get; set; }

        /// <summary>
        /// �����ȯ 2�����(Y/N)
        /// </summary>
        public string GBCHEST { get; set; }

        /// <summary>
        /// ��ȯ�����ȯ 2�� ���(Y/N)
        /// </summary>
        public string GBCYCLE { get; set; }

        /// <summary>
        /// �������� 2�����(Y/N)
        /// </summary>
        public string GBGOJI { get; set; }

        /// <summary>
        /// ������ȯ 2�����(Y/N)
        /// </summary>
        public string GBLIVER { get; set; }

        /// <summary>
        /// ������ȯ 2�����(Y/N)
        /// </summary>
        public string GBKIDNEY { get; set; }

        /// <summary>
        /// ������ 2�� ���(Y/N)
        /// </summary>
        public string GBANEMIA { get; set; }

        /// <summary>
        /// �索��ȯ 2�� ���(Y/N)
        /// </summary>
        public string GBDIABETES { get; set; }

        /// <summary>
        /// ��Ÿ��ȯ 2�� ���(Y/N)
        /// </summary>
        public string GBETC { get; set; }

        /// <summary>
        /// ��ι�缱�˻�
        /// </summary>
        public string CHEST1 { get; set; }

        /// <summary>
        /// ���ٱ����յ����˻�(1.���� 2.�缺)
        /// </summary>
        public string CHEST2 { get; set; }

        /// <summary>
        /// ����ٰ˻�Ұ�(1.���� 4.����ȯ)
        /// </summary>
        public string CHEST3 { get; set; }

        /// <summary>
        /// ��ΰ˻�Ұ�(1.����A 2.����B 3.�ǰ����� 4.����ȯ)
        /// </summary>
        public string CHEST_RES { get; set; }

        /// <summary>
        /// ������(����:�ְ�)
        /// </summary>
        public string CYCLE1 { get; set; }

        /// <summary>
        /// ������(����:����)
        /// </summary>
        public string CYCLE2 { get; set; }

        /// <summary>
        /// ���о����˻�(1.���� 2.������1~2 3.������3 4.������4)
        /// </summary>
        public string CYCLE3 { get; set; }

        /// <summary>
        /// �������˻�(�Ʒ�����)
        /// </summary>
        public string CYCLE4 { get; set; }

        /// <summary>
        /// �����а˻�Ұ�(1.����A 2.����B 3.�ǰ����� 4.����ȯ)
        /// </summary>
        public string CYCLE_RES { get; set; }

        /// <summary>
        /// ��������(Ʈ���׸������̵�)
        /// </summary>
        public string GOJI1 { get; set; }

        /// <summary>
        /// ��������(HDL�ݷ����׷�)
        /// </summary>
        public string GOJI2 { get; set; }

        /// <summary>
        /// ���������Ұ�(1.����A 2.����B 3.�ǰ����� 4.����ȯ)
        /// </summary>
        public string GOJI_RES { get; set; }

        /// <summary>
        /// ������ȯ(�˺ι�)
        /// </summary>
        public string LIVER11 { get; set; }

        /// <summary>
        /// ������ȯ(�Ѵܹ�����)
        /// </summary>
        public string LIVER12 { get; set; }

        /// <summary>
        /// ������ȯ(�Ѻ������)
        /// </summary>
        public string LIVER13 { get; set; }

        /// <summary>
        /// ������ȯ(�����������)
        /// </summary>
        public string LIVER14 { get; set; }

        /// <summary>
        /// ������ȯ(��ī������Ÿ����)
        /// </summary>
        public string LIVER15 { get; set; }

        /// <summary>
        /// ������ȯ(����Ż��ȿ��(LDH)
        /// </summary>
        public string LIVER16 { get; set; }

        /// <summary>
        /// ������ȯ(��������ܹ�) (�Ʒ�����)
        /// </summary>
        public string LIVER17 { get; set; }

        /// <summary>
        /// ������ȯ(�����˻��׿�) :1.���� 2.�缺
        /// </summary>
        public string LIVER18 { get; set; }

        /// <summary>
        /// ������ȯ(�����˻���ü):1.���� 2.�缺
        /// </summary>
        public string LIVER19 { get; set; }

        /// <summary>
        /// �����˻���: 1.���������� 2.�鿪�� 3.���������
        /// </summary>
        public string LIVER20 { get; set; }

        /// <summary>
        /// ������ȯ�Ұ�(1.����A 2.����B 3.�ǰ����� 4.����ȯ)
        /// </summary>
        public string LIVER_RES { get; set; }

        /// <summary>
        /// ������ȯ(��ħ�����̰�˻�(RBC))
        /// </summary>
        public string KIDNEY1 { get; set; }

        /// <summary>
        /// ������ȯ(��ħ�����̰�˻�(WBC))
        /// </summary>
        public string KIDNEY2 { get; set; }

        /// <summary>
        /// ������ȯ(�������)
        /// </summary>
        public string KIDNEY3 { get; set; }

        /// <summary>
        /// ������ȯ(ũ����Ƽ��)
        /// </summary>
        public string KIDNEY4 { get; set; }

        /// <summary>
        /// ������ȯ(���)
        /// </summary>
        public string KIDNEY5 { get; set; }

        /// <summary>
        /// ������ȯ�Ұ�(1.����A 2.����B 3.�ǰ����� 4.����ȯ)
        /// </summary>
        public string KIDNEY_RES { get; set; }

        /// <summary>
        /// ������(������ũ��Ʈ)
        /// </summary>
        public string ANEMIA1 { get; set; }

        /// <summary>
        /// ������(��������)
        /// </summary>
        public string AMEMIA2 { get; set; }

        /// <summary>
        /// ������(��������)
        /// </summary>
        public string AMEMIA3 { get; set; }

        /// <summary>
        /// �������Ұ�(1.����A 2.����B 3.�ǰ����� 4.����ȯ)
        /// </summary>
        public string AMEMIA_RES { get; set; }

        /// <summary>
        /// �索��ȯ(����:�ְ�) ����
        /// </summary>
        public string DIABETES1 { get; set; }

        /// <summary>
        /// �索��ȯ(����:����) ����
        /// </summary>
        public string DIABETES2 { get; set; }

        /// <summary>
        /// �索��ȯ(���о����˻�) ** �Ʒ����� **
        /// </summary>
        public string DIABETES3 { get; set; }

        /// <summary>
        /// �索��ȯ�Ұ�(1.����A 2.����B 3.�ǰ����� 4.����ȯ)
        /// </summary>
        public string DIABETES_RES { get; set; }

        /// <summary>
        /// ��Ÿ��ȯ �˻�Ұ�
        /// </summary>
        public string ETC_RES { get; set; }

        /// <summary>
        /// ��Ÿ��ȯ �˻系��
        /// </summary>
        public string ETC_EXAM { get; set; }

        /// <summary>
        /// ��������(1.����A 2.����B 3.�ǰ����� 4.����ȯ)
        /// </summary>
        public string PANJENG { get; set; }

        /// <summary>
        /// ������������ȯ(������D1):������ "5"�� ���(2002�������)
        /// </summary>
        public string PANJENG_D1 { get; set; }

        /// <summary>
        /// �������Ұ��� �Ϲ�����
        /// </summary>
        public string PANJENG_SO1 { get; set; }

        /// <summary>
        /// �������Ұ��� ������
        /// </summary>
        public string PANJENG_SO2 { get; set; }

        /// <summary>
        /// ��� �뺸���(1.����� 2.�ּ��� 3.����)
        /// </summary>
        public string TONGBOGBN { get; set; }

        /// <summary>
        /// �Ұ� �� ��ġ����
        /// </summary>
        public string SOGEN { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public string GUNDATE { get; set; }

        /// <summary>
        /// �ǰ����� ���İ����Ұ�
        /// </summary>
        public string PANJENG_SO3 { get; set; }

        /// <summary>
        /// ������������ȯ(������D1):������ "5"�� ���(2003�������)
        /// </summary>
        public string PANJENG_D11 { get; set; }

        /// <summary>
        /// ������������ȯ(������D1):������ "5"�� ���(2003�������)
        /// </summary>
        public string PANJENG_D12 { get; set; }

        /// <summary>
        /// �������ռ�����(**) �����ڵ�:13 �Ϲ�+Ư���� �����
        /// </summary>
        public string WORKYN { get; set; }

        /// <summary>
        /// ��� �� �����������˻� (1:�Ƿ� 2:���Ƿ�)
        /// </summary>
        public string CHEST4 { get; set; }

        /// <summary>
        /// ����ȯ - �Ϲ����� D2 (K,J,A,)
        /// </summary>
        public string PANJENG_D2 { get; set; }

        /// <summary>
        /// ������ȯ - C������ǥ����ü �˻�-1�Ϲ�, 2����
        /// </summary>
        public string LIVER21 { get; set; }

        /// <summary>
        /// ������ȯ - C������ǥ����ü ���-1.���� 2.�缺
        /// </summary>
        public string LIVER22 { get; set; }

        /// <summary>
        /// �索��-ġ���ȹ(�Ʒ�����)
        /// </summary>
        public string DIABETES_RES_CARE { get; set; }

        /// <summary>
        /// ������-ġ���ȹ(�Ʒ�����)
        /// </summary>
        public string CYCLE_RES_CARE { get; set; }

        /// <summary>
        /// �����������(1:Ư�̼Ұ߾���0-5�� ,2:�����������~ 6���̻�)
        /// </summary>
        public string T66_MEM { get; set; }

        /// <summary>
        /// ��-����ƾ ������
        /// </summary>
        public string T_SMOKE1 { get; set; }

        /// <summary>
        /// ��-ó��
        /// </summary>
        public string T_SMOKE2 { get; set; }

        /// <summary>
        /// ��-������
        /// </summary>
        public long T_SMOKE3 { get; set; }

        /// <summary>
        /// ����-��
        /// </summary>
        public string T_DRINK1 { get; set; }

        /// <summary>
        /// ����-ó��
        /// </summary>
        public string T_DRINK2 { get; set; }

        /// <summary>
        /// ����-������
        /// </summary>
        public long T_DRINK3 { get; set; }

        /// <summary>
        /// �-��
        /// </summary>
        public string T_HELTH1 { get; set; }

        /// <summary>
        /// �-ó��(����)
        /// </summary>
        public string T_HELTH2 { get; set; }

        /// <summary>
        /// �-ó��(�ð�)
        /// </summary>
        public string T_HELTH3 { get; set; }

        /// <summary>
        /// �-ó��(��,Ƚ��)
        /// </summary>
        public string T_HELTH4 { get; set; }

        /// <summary>
        /// �-������
        /// </summary>
        public long T_HELTH5 { get; set; }

        /// <summary>
        /// ����-��
        /// </summary>
        public string T_DIET1 { get; set; }

        /// <summary>
        /// ����-��������(����ǰ)
        /// </summary>
        public string T_DIET2 { get; set; }

        /// <summary>
        /// ����-��������(�ܹ�����)
        /// </summary>
        public string T_DIET3 { get; set; }

        /// <summary>
        /// ����-��������(��ä�Ͱ���)
        /// </summary>
        public string T_DIET4 { get; set; }

        /// <summary>
        /// ����-��������(����)
        /// </summary>
        public string T_DIET5 { get; set; }

        /// <summary>
        /// ����-��������(�ܼ���)
        /// </summary>
        public string T_DIET6 { get; set; }

        /// <summary>
        /// ����-��������(����,�ұ�)
        /// </summary>
        public string T_DIET7 { get; set; }

        /// <summary>
        /// ����-�ùٸ��Ļ����(��ħ�Ļ�)
        /// </summary>
        public string T_DIET8 { get; set; }

        /// <summary>
        /// ����-�ùٸ��Ļ����(����Ա�)
        /// </summary>
        public string T_DIET9 { get; set; }

        /// <summary>
        /// ����-ó�濬��(���米��)
        /// </summary>
        public string T_DIET10 { get; set; }

        /// <summary>
        /// ����-������
        /// </summary>
        public long T_DIET11 { get; set; }

        /// <summary>
        /// ��-ü��������
        /// </summary>
        public string T_BIMAN1 { get; set; }

        /// <summary>
        /// ��-�㸮�ѷ�
        /// </summary>
        public string T_BIMAN2 { get; set; }

        /// <summary>
        /// ��-ó��(1.�Ļ緮�� ���̽ʽÿ�)
        /// </summary>
        public string T_BIMAN3 { get; set; }

        /// <summary>
        /// ��-ó��(2.���İ� �߽��� ���̽ʽÿ�)
        /// </summary>
        public string T_BIMAN4 { get; set; }

        /// <summary>
        /// ��-ó��(3.���ַ��� Ƚ���� ���̽ʽÿ�)
        /// </summary>
        public string T_BIMAN5 { get; set; }

        /// <summary>
        /// ��-ó��(4.�ܽ��̳� �н�ƮǪ�带 ���̽ʽÿ�)
        /// </summary>
        public string T_BIMAN6 { get; set; }

        /// <summary>
        /// ��-ó��(5.�ó���� �����Ͻʽÿ�)
        /// </summary>
        public string T_BIMAN7 { get; set; }

        /// <summary>
        /// ��-ó��(6����(��Ŭ����))
        /// </summary>
        public string T_BIMAN8 { get; set; }

        /// <summary>
        /// ��-ó��(7.��Ÿ)
        /// </summary>
        public string T_BIMAN9 { get; set; }

        /// <summary>
        /// �����CES-D
        /// </summary>
        public string T_CESD { get; set; }

        /// <summary>
        /// ���μ� ����� GDS������
        /// </summary>
        public string T_GDS { get; set; }

        /// <summary>
        /// ���������� KDSQ-C
        /// </summary>
        public string T_KDSQC { get; set; }

        /// <summary>
        /// ���ֻ�㿩��(Y or Null)
        /// </summary>
        public string T_SANGDAM { get; set; }

        /// <summary>
        /// ���İ���(R1-C)
        /// </summary>
        public string PANJENGSAHU1 { get; set; }

        /// <summary>
        /// ���İ���(D2)
        /// </summary>
        public string PANJENGSAHU2 { get; set; }

        /// <summary>
        /// 1�� �ǰ����� ��� ���(����)
        /// </summary>
        public string T_SANGDAM_1 { get; set; }

        /// <summary>
        /// ���ռҰ�(�������� ����):2015-01-08 �߰�
        /// </summary>
        public string SOGENB { get; set; }

        /// <summary>
        /// ��.������ ����� ����(Y/N)
        /// </summary>
        public string GBGONGHU { get; set; }

        public HIC_RES_BOHUM2_JEPSU()
        {
        }
    }
}
