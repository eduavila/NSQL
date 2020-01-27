using NSQL.Classes;
using System;
using System.Collections.Generic;

namespace NSQL.Interfaces
{
    public interface INSql
    {

        void SetQuery(String sql);

        /// <summary>
        ///  Retorna lista de Condições Wheres
        /// </summary>
        /// <returns></returns>
        List<Condition> GetWheres();

        /// <summary>
        ///  Retornar lista de Condições Havings
        /// </summary>
        /// <returns></returns>
        List<Condition> GetHavings();

        /// <summary>
        ///  Adicona
        /// </summary>
        /// <param name="column"></param>
        /// <param name="operador"></param>
        /// <returns></returns>
        INSql Where(Action<INSql> SubCondition, Cond condition = Cond.AND);


        /// <summary>
        ///  
        /// </summary>
        /// <param name="column"></param>
        /// <param name="operador"></param>
        /// <returns></returns>
        INSql Where(INSql subquery, Cond condition = Cond.AND);

        /// <summary>
        ///  
        /// </summary>
        /// <param name="column"></param>
        /// <param name="operador"></param>
        /// <returns></returns>
        INSql Where(string column, Op operador, object value);

        ///
        INSql Where(string column, object value);

        /// <summary>
        ///  
        /// </summary>
        /// <param name="column"></param>
        /// <param name="operador"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        INSql Having(string column, Op operador, object valor);

        /// <summary>
        ///  
        /// </summary>
        /// <param name="column"></param>
        /// <param name="operador"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        INSql Having(INSql subquery, Cond condition = Cond.AND);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <param name="operador"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        INSql HavingOr(string column, Op operador, object valor);

        /// <summary>
        ///  Adicione uma cláusula "or where" básica à consulta
        /// </summary>
        /// <param name="column">Nome coluna</param>
        /// <param name="operador">Operador</param>
        /// <param name="valor">Valor a se passado</param>
        /// <returns></returns>
        INSql WhereOr(string column, Op operador, object valor);

        INSql WhereLike(string column, string value);

        INSql WhereLikeOr(string column, string value);

        /// <summary>
        ///  Adicione uma cláusula "where in" básica à consulta
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        INSql WhereIn(string column, string value, Cond condition = Cond.AND);


        /// <summary>
        ///  Adicione uma cláusula "where not in" básica à consulta
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        INSql WhereNotIn(string column, string value, Cond condition = Cond.AND);

        /// <summary>
        ///  Adicione uma cláusula "where in" básica à consulta
        /// </summary>
        /// <param name="column"></param>
        /// <param name="values"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        INSql WhereIn(string column, List<object> values, Cond condition = Cond.AND);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <param name="values"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        INSql WhereNotIn(string column, List<object> values, Cond condition = Cond.AND);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        INSql WhereBetween(string column, string from, string to);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        INSql WhereNotNull(string column);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        INSql WhereNull(string column);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        INSql WhereRaw(string sql, Cond condition = Cond.AND);

        /// <summary>
        ///  Adicione uma cláusula "FIND_IN_SET(str,strlist)" básica à consulta
        /// </summary>
        /// <param name="column"></param>
        /// <param name="values"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        INSql WhereFindInSet(string column, List<object> values, Cond condition = Cond.AND);

        /// <summary>
        ///  Adicione uma cláusula "FIND_IN_SET(str,strlist)" básica à consulta
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        INSql WhereFindInSet(string column, string value, Cond condition = Cond.AND);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        INSql OrderBy(string column, Order order = Order.ASC);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        INSql Limit(int value);

        INSql OffSet(int value);

        INSql Paginate(int limit, int numberPage);

        INSql GroupBy(string column);

        String ToSql();

        String ToWhere();

        INSql BindParam(string param, object value, TipoParam tipoParam = TipoParam.Nada);

        INSql JoinWhere(INSql query);

        INSql JoinHaving(INSql query);
    }

    public enum Cond
    {
        AND,
        OR
    }

    public enum Op
    {
        IGUAL,
        Diferente,
        MaiorQue,
        MenorQue,
        Null,
        NotNulll,
        Like,
        Between,
        Raw,
        In,
        NotIn,
        FindInSet
    }

    public enum Order
    {
        ASC,
        DESC
    }

    public enum TipoParam
    {
        Nada,
        Aspas,
        SemAspas
    }
}
