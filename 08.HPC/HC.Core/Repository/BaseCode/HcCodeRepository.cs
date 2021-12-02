namespace HC.Core.Repository
{
    using ComBase.Mvc;
    using HC.Core.Dto;
    using HC.Core.Service;
    using System.Collections.Generic;


    /// <summary>
    /// 
    /// </summary>
    public class HcCodeRepository : BaseRepository
    {
        public List<HC_CODE> FindAll()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_CODES       ");

            return ExecuteReader<HC_CODE>(parameter);
        }

        /// <summary>
        /// 삭제되지 않은 모든 그룹코드 가져오기
        /// </summary>
        /// <returns></returns>
        public List<HC_CODE> FindGroupCodeAll(string program)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.ID, A.CODE, A.GROUPCODE, A.GROUPCODENAME, A.DESCRIPTION, A.SORTSEQ, A.ISACTIVE, A.MODIFIED, B.NAME AS MODIFIEDUSER, to_char(A.MODIFIED,'YYYY-MM-DD') as CTEST FROM HIC_CODES A        ");
            parameter.AppendSql("LEFT OUTER JOIN HIC_USERS B  ");
            parameter.AppendSql("ON A.MODIFIEDUSER = B.USERID  ");
            parameter.AppendSql("WHERE A.GROUPCODE = 'GROUP'  ");
            parameter.AppendSql("AND A.ISDELETED = 'N'  ");
            parameter.AppendSql("AND A.PROGRAM = :program  ");
            parameter.AppendSql("ORDER BY A.SORTSEQ  ");

            parameter.Add("program", program);

            return ExecuteReader<HC_CODE>(parameter);
        }
        /// <summary>
        /// 삭제여부 관계없이 기초코드 목록
        /// </summary>
        /// <param name="groupCode"></param>
        /// <returns></returns>
        public List<HC_CODE> FindCodeByGroupCode(string groupCode, string program)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.ID, A.CODE, CODENAME, A.GROUPCODE, A.GROUPCODENAME, A.DESCRIPTION, A.SORTSEQ, A.ISACTIVE, A.MODIFIED, B.NAME AS MODIFIEDUSER FROM HIC_CODES A        ");
            parameter.AppendSql("LEFT OUTER JOIN HIC_USERS B              ");
            parameter.AppendSql("ON A.MODIFIEDUSER = B.USERID       ");
            parameter.AppendSql("WHERE A.GROUPCODE = :GROUPCODE       ");
            parameter.AppendSql("AND A.PROGRAM = :program  ");
            parameter.AppendSql("ORDER BY A.SORTSEQ  ");
            parameter.Add("GROUPCODE", groupCode);
            parameter.Add("program", program);
            return ExecuteReader<HC_CODE>(parameter);
        }

        public void Insert(HC_CODE code)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_CODES    "); 
            parameter.AppendSql("(ID, CODE, CODENAME, GROUPCODE, GROUPCODENAME, SORTSEQ, EXTEND1, EXTEND2, ISACTIVE, ISDELETED, DESCRIPTION, MODIFIED, MODIFIEDUSER, PROGRAM)     ");
            parameter.AppendSql("VALUES(HC_CODE_ID_SEQ.NEXTVAL, :CODE, :CODENAME, :GROUPCODE, :GROUPCODENAME, :SORTSEQ, :EXTEND1, :EXTEND2, :ISACTIVE, 'N', :DESCRIPTION, SYSTIMESTAMP, :MODIFIEDUSER, :PROGRAM)     ");
            
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
            parameter.Add("PROGRAM",code.Program);
            ExecuteNonQuery(parameter);
        }
        public void Update(HC_CODE code)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_CODES   "); 
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

        public List<HC_CODE> GetComboBoxData(string argGrpCD, string argPgrCD)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE, CODENAME FROM HIC_CODES       ");
            parameter.AppendSql("WHERE GROUPCODE = :GROUPCODE       ");
            parameter.AppendSql("AND ISACTIVE = 'Y'                 ");
            parameter.AppendSql("AND ISDELETED = 'N'                 ");
            parameter.AppendSql("AND PROGRAM = :program  ");
            parameter.Add("GROUPCODE", argGrpCD);
            parameter.Add("program", argPgrCD);
            return ExecuteReader<HC_CODE>(parameter);
        }

        public void Delete(HC_CODE code)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_CODES    ");            
            parameter.AppendSql("WHERE ID = :ID     ");

            parameter.Add("ID", code.Id);
            ExecuteNonQuery(parameter);
        }

        /// <summary>
        /// 삭제되지 않고 액티브인 기초코드 목록
        /// </summary>
        /// <param name="groupCode"></param>
        /// <returns></returns>
        public List<HC_CODE> FindActiveCodeByGroupCode(string groupCode, string program)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_CODES       ");
            parameter.AppendSql("WHERE GROUPCODE = :GROUPCODE       ");
            parameter.AppendSql("AND ISACTIVE = 'Y'                 ");
            parameter.AppendSql("AND ISDELETED = 'N'                 ");
            parameter.AppendSql("AND PROGRAM = :program  ");
            parameter.AppendSql("ORDER BY SORTSEQ  ");
            parameter.Add("GROUPCODE", groupCode);
            parameter.Add("program", program);
            return ExecuteReader<HC_CODE>(parameter);
        }
        public HC_CODE FindActiveCodeByGroupAndCode(string groupCode, string code, string program)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_CODES               ");
            parameter.AppendSql("WHERE GROUPCODE = :GROUPCODE        ");
            parameter.AppendSql("AND CODE = :CODE                    ");
            parameter.AppendSql("AND ISACTIVE = 'Y'                  ");
            parameter.AppendSql("AND ISDELETED = 'N'                 ");
            parameter.AppendSql("AND PROGRAM = :program  ");
            parameter.Add("GROUPCODE", groupCode);
            parameter.Add("CODE", code);
            parameter.Add("program", program);
            return ExecuteReaderSingle< HC_CODE>(parameter);
        }
        public HC_CODE FindActiveCodeByGroupAndCodeAndCodeName(string groupCode, string code, string codeName, string program)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_CODES               ");
            parameter.AppendSql("WHERE GROUPCODE = :GROUPCODE        ");
            parameter.AppendSql("AND CODE = :CODE                    ");
            parameter.AppendSql("AND CODENAME = :CODENAME                    ");
            parameter.AppendSql("AND ISDELETED = 'N'                 ");
            parameter.AppendSql("AND PROGRAM = :program  ");

            parameter.Add("GROUPCODE", groupCode);
            parameter.Add("CODE", code);
            parameter.Add("CODENAME", codeName);
            parameter.Add("program", program);
            return ExecuteReaderSingle<HC_CODE>(parameter);
        }
    }
}

