namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicMisuMstLtdRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicMisuMstLtdRepository()
        {
        }

        public List<HIC_MISU_MST_LTD> GetBillList(string strFDate, string strTDate, string strMJong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT                    A.LTDCODE,A.WRTNO,A.GJONG,A.MISUAMT                                                      ");
            parameter.AppendSql("                        , B.SAUPNO,B.UPTAE,B.JONGMOK,B.DAEPYO,B.SANGHO,B.JUSO,B.JUSODETAIL,B.JSAUPNO,B.DLTD        ");

            parameter.AppendSql("FROM                      HIC_MISU_MST A,HIC_LTD B                                                                 ");

            parameter.AppendSql("WHERE                     A.BDATE>=TO_DATE( :FDATE, 'YYYY-MM-DD')                                                  ");
            parameter.AppendSql("   AND                    A.BDATE<=TO_DATE( :TDATE, 'YYYY-MM-DD')                                                  ");
            parameter.AppendSql("   AND                    (A.GBTAX IS NULL OR A.GBTAX='N' OR A.GBTAX = ' ')                                        ");  // ���ο����� �߱޵Ȱ��� ����
            parameter.AppendSql("   AND                    B.SAUPNO > ' '                                                                           ");  // ����ڵ�Ϲ�ȣ�� ������ �ƴ� ��

            switch (strMJong)
            {
                case "1":
                    parameter.AppendSql("   AND            A.GJONG <=  '80'                                                                         ");  // �ǰ�����
                    break;
                case "2":
                    parameter.AppendSql("   AND            A.GJONG =  '82'                                                                         ");  // ���ǰ�������
                    break;
                case "3":
                    parameter.AppendSql("   AND            A.GJONG =  '81'                                                                         ");  // �۾�ȯ������
                    break;
                case "4":
                    parameter.AppendSql("   AND            A.GJONG =  '83'                                                                         ");  // ���հ���
                    break;
                case "5":
                    parameter.AppendSql("   AND            A.MISUJONG= '4'                                                                          ");  // ���ι̼�
                    break;
                case "6":
                    parameter.AppendSql("   AND            A.GJONG =  '82'                                                                         ");  // ���ǰ�������
                    break;
            }

            parameter.AppendSql("   AND                    A.MISUJONG IN ('1','4')                                                                  ");  // ȸ��̼�, ���ι̼�
            parameter.AppendSql("   AND                    A.LTDCODE = B.CODE(+)                                                                    ");  
            parameter.AppendSql("ORDER BY                  B.NAME,A.WRTNO,A.LTDCODE                                                                 ");

            parameter.Add("FDATE", strFDate);
            parameter.Add("TDATE", strTDate);

            return ExecuteReader<HIC_MISU_MST_LTD>(parameter);
        }

        public int UpdateBill(long nWrtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE     HIC_MISU_MST SET GbTAX='Y'       ");
            parameter.AppendSql("WHERE      WRTNO = :WRTNO                   ");

            parameter.Add("WRTNO", nWrtno);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_MISU_MST_LTD> GetActingItem(long DLTD)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT                    SAUPNO,SANGHO,DAEPYO,UPTAE,JONGMOK,JUSO,JUSODETAIL,JSAUPNO       ");

            parameter.AppendSql("FROM                      HIC_LTD                                                          ");

            parameter.AppendSql("WHERE                     CODE = :DLTD                                                     ");

            parameter.Add("DLTD", DLTD);

            return ExecuteReader<HIC_MISU_MST_LTD>(parameter);
        }
    }
}
