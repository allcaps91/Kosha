namespace HC.Core.BaseCode.Management.Repository
{
    using System.Collections.Generic;
    using ComBase.Mvc;
    using HC.Core.BaseCode.Management.Dto;
    using HC.Core.Common.Service;


    /// <summary>
    /// 
    /// </summary>
    public class HcCodeRepository : BaseRepository
    {
        public List<HC_CODE> FindAll()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HC_CODE       ");

            return ExecuteReader<HC_CODE>(parameter);
        }

        /// <summary>
        /// 삭제되지 않은 모든 그룹코드 가져오기
        /// </summary>
        /// <returns></returns>
        public List<HC_CODE> FindGroupCodeAll()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.ID, A.CODE, A.GROUPCODE, A.GROUPCODENAME, A.DESCRIPTION, A.SORTSEQ, A.ISACTIVE, A.MODIFIED, B.NAME AS MODIFIEDUSER FROM HC_CODE A        ");
            parameter.AppendSql("INNER JOIN HC_USERS B  ");
            parameter.AppendSql("ON A.MODIFIEDUSER = B.USERID  ");
            parameter.AppendSql("WHERE A.GROUPCODE = 'GROUP'  ");
            parameter.AppendSql("AND A.ISDELETED = 'N'  ");
            parameter.AppendSql("ORDER BY A.SORTSEQ  ");

            return ExecuteReader<HC_CODE>(parameter);
        }
        /// <summary>
        /// 삭제여부 관계없이 기초코드 목록
        /// </summary>
        /// <param name="groupCode"></param>
        /// <returns></returns>
        public List<HC_CODE> FindCodeByGroupCode(string groupCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.ID, A.CODE, CODENAME, A.GROUPCODE, A.GROUPCODENAME, A.DESCRIPTION, A.SORTSEQ, A.ISACTIVE, A.MODIFIED, B.NAME AS MODIFIEDUSER FROM HC_CODE A        ");
            parameter.AppendSql("INNER JOIN HC_USERS B              ");
            parameter.AppendSql("ON A.MODIFIEDUSER = B.USERID       ");
            parameter.AppendSql("WHERE GROUPCODE = :GROUPCODE       ");
            parameter.AppendSql("ORDER BY A.SORTSEQ  ");
            parameter.Add("GROUPCODE", groupCode);

            return ExecuteReader<HC_CODE>(parameter);
        }

        public void Insert(HC_CODE code)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HC_CODE    "); 
            parameter.AppendSql("(ID, CODE, CODENAME, GROUPCODE, GROUPCODENAME, SORTSEQ, EXTEND1, EXTEND2, ISACTIVE, ISDELETED, DESCRIPTION, MODIFIED, MODIFIEDUSER)     ");
            parameter.AppendSql("VALUES(HC_CODE_ID_SEQ.NEXTVAL, :CODE, :CODENAME, :GROUPCODE, :GROUPCODENAME, :SORTSEQ, :EXTEND1, :EXTEND2, :ISACTIVE, 'N', :DESCRIPTION, SYSTIMESTAMP, :MODIFIEDUSER)     ");
            
            parameter.Add("CODE", code.Code);
            parameter.Add("CODENAME", code.CodeName);
            parameter.Add("GROUPCODE", code.GroupCode);
            parameter.Add("GROUPCODENAME", code.GroupCodeName);
            parameter.Add("SORTSEQ", code.SortSeq);
            parameter.Add("EXTEND1", code.Extend1);
            parameter.Add("EXTEND2", code.Extend2);
            parameter.Add("DESCRIPTION", code.Description);
            parameter.Add("ISACTIVE", code.IsActive);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            ExecuteNonQuery(parameter);
        }
        public void Update(HC_CODE code)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HC_CODE   "); 
            parameter.AppendSql("SET CODE = :CODE, CODENAME = :CODENAME, GROUPCODE = :GROUPCODE, GROUPCODENAME = :GROUPCODENAME, SORTSEQ = :SORTSEQ, EXTEND1 = :EXTEND1, EXTEND2 = :EXTEND2, ISACTIVE = :ISACTIVE, DESCRIPTION = :DESCRIPTION, MODIFIED = SYSTIMESTAMP, MODIFIEDUSER = :MODIFIEDUSER   ");
            parameter.AppendSql("WHERE ID = :ID    ");
            parameter.Add("CODE", code.Code);
            parameter.Add("CODENAME", code.CodeName);
            parameter.Add("GROUPCODE", code.GroupCode);
            parameter.Add("GROUPCODENAME", code.GroupCodeName);
            parameter.Add("SORTSEQ", code.SortSeq);
            parameter.Add("EXTEND1", code.Extend1);
            parameter.Add("EXTEND2", code.Extend2);
            parameter.Add("DESCRIPTION", code.Description);
            parameter.Add("ISACTIVE", code.IsActive);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("ID", code.Id);
            ExecuteNonQuery(parameter);
        }

        public void Delete(HC_CODE code)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HC_CODE    ");
            parameter.AppendSql("SET ISDELETED ='Y'   ");
            parameter.AppendSql("WHERE ID = :ID     ");

            parameter.Add("ID", code.Id);
            ExecuteNonQuery(parameter);
        }

        /// <summary>
        /// 삭제되지 않고 액티브인 기초코드 목록
        /// </summary>
        /// <param name="groupCode"></param>
        /// <returns></returns>
        public List<HC_CODE> FindActiveCodeByGroupCode(string groupCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HC_CODE       ");
            parameter.AppendSql("WHERE GROUPCODE = :GROUPCODE       ");
            parameter.AppendSql("AND ISACTIVE = 'Y'                 ");
            parameter.AppendSql("AND ISDELETED = 'N'                 ");
            parameter.Add("GROUPCODE", groupCode);

            return ExecuteReader<HC_CODE>(parameter);
        }

     
    }
}
