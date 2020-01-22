using System;
using System.Collections.Generic;

namespace NSQL
{
#pragma warning disable CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
    public class NSQL : INSql
#pragma warning restore CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
    {
        private readonly List<Condition> ListConditionWheres = new List<Condition>();
        private readonly List<Condition> ListConditionHavings = new List<Condition>();
        private readonly List<ConditionBindParam> ListConditionBindParams = new List<ConditionBindParam>();
        private readonly List<ConditionOrderBy> ListConditionOrderBy = new List<ConditionOrderBy>();
        private readonly List<ConditionGroupBy> ListConditionGroupBy = new List<ConditionGroupBy>();
        private ConditionLimit ConditionLimit;
        private ConditionOffSet conditionOffSet;


        private string SqlRaw;

        public NSQL()
        {
            this.SqlRaw = "";
        }
        public NSQL(string sql)
        {
            this.SqlRaw = sql;
        }

#pragma warning disable CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        public static INSql Create(string sql)
#pragma warning restore CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        {
            return new NSQL(sql);
        }

        #region "Private Function"

        /// <summary>
        ///  Verifica se objeto é string ou numero.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string ToTypeSql(object value)
        {
            if (value is int)
            {
                return value.ToString();
            }
            else
            {
                return String.Format("'{0}'", value);
            }
        }

#pragma warning disable CS0246 // The type or namespace name 'Op' could not be found (are you missing a using directive or an assembly reference?)
        /// <summary>
        ///  Retornar operador
        /// </summary>
        /// <param name="operatorValue"></param>
        /// <returns></returns>
        private string GetOperator(Op operatorValue)
#pragma warning restore CS0246 // The type or namespace name 'Op' could not be found (are you missing a using directive or an assembly reference?)
        {
            switch (operatorValue)
            {
                case Op.IGUAL:
                    return "=";
                case Op.Diferente:
                    return "!=";
                case Op.MaiorQue:
                    return ">=";
                case Op.MenorQue:
                    return "<=";
                default:
                    return "";
            }
        }
        
        /// <summary>
        ///  Converte Lista para string separado por virgula.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="checkType"></param>
        /// <returns></returns>
        private string ConvertToIn(List<object> values,bool checkType = true)
        {
            List<String> valuesRest = new List<String>();

            foreach (var item in values)
            {
                if (checkType)
                {
                    valuesRest.Add(ToTypeSql(item));
                }
                else
                {
                    valuesRest.Add(item.ToString());
                }
            }

            return String.Join(",", valuesRest);
        }


        /// <summary>
        ///  Processa condições do SQL.
        /// </summary>
        /// <param name="listCondition"></param>
        /// <returns>String com codições montada.</returns>
        private string ProcessConditions(List<Condition> listCondition)
        {
            string raw = "";
            foreach (var where in listCondition)
            {
                //Se nao for primeiro, verificar codicionar
                if (listCondition.IndexOf(where) != 0)
                {
                    raw += string.Format(" {0}", where.ConditionValue.ToString());
                }

                // Verifica se funcao
                if (where.SubCondition != null)
                {
                    var query = new NSQL();
                    where.SubCondition(query);

                    raw += string.Format(" ( {0} )", query.ToWhere().Trim());
                }
                else
                {
                    switch (where.Operator)
                    {
                        case Op.Null:

                            raw += string.Format(" {0} IS NULL", where.Column);
                            break;
                        case Op.NotNulll:

                            raw += string.Format(" {0} IS NOT NULL", where.Column);
                            break;
                        case Op.Like:

                            raw += string.Format(" {0} LIKE '{1}'", where.Column, where.Value);
                            break;
                        case Op.Between:

                            raw += string.Format(" BETWEEN {0} AND {1} ", where.ValueFrom, where.ValueTo);
                            break;
                        case Op.Raw:

                            raw += " " + where.Raw;
                            break;
                        case Op.In:

                            raw += string.Format(" {0} IN ({1})", where.Column, where.Value);
                            break;
                        case Op.NotIn:

                            raw += string.Format(" {0} NOT IN ({1})", where.Column, where.Value);
                            break;
                        case Op.FindInSet:

                            raw += string.Format(" FIND_IN_SET({0},'{1}')", where.Column, where.Value);
                            break;
                        default:

                            raw += string.Format(" {0} {1} {2}", where.Column, GetOperator(where.Operator), ToTypeSql(where.Value));
                            break;
                    }
                }
            }

            return raw;
        }

        #endregion

        #region "Public Function"

#pragma warning disable CS0246 // The type or namespace name 'Op' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        public INSql Where(string column, Op operador, object value)
#pragma warning restore CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'Op' could not be found (are you missing a using directive or an assembly reference?)
        {
            var condition = new Condition()
            {
                Column = column,
                Operator = operador,
                Value = value,
                ConditionValue = Cond.AND

            };

            this.ListConditionWheres.Add(condition);

            return this;
        }

#pragma warning disable CS0246 // The type or namespace name 'Op' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        public INSql WhereOr(string column, Op operador, object valor)
#pragma warning restore CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'Op' could not be found (are you missing a using directive or an assembly reference?)
        {
            var condition = new Condition()
            {
                Column = column,
                Operator = operador,
                Value = valor,
                ConditionValue = Cond.OR

            };

            this.ListConditionWheres.Add(condition);

            return this;
        }

#pragma warning disable CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        public INSql WhereBetween(string column, string from, string to)
#pragma warning restore CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        {
            var condition = new Condition()
            {
                Operator = Op.Between,
                Column = column,
                ValueFrom = from,
                ValueTo = to,
                ConditionValue = Cond.AND
            };

            this.ListConditionWheres.Add(condition);

            return this;
        }

#pragma warning disable CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        public INSql WhereNotNull(string column)
#pragma warning restore CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        {
            var condition = new Condition()
            {
                Operator = Op.NotNulll,
                Column = column,
                ConditionValue = Cond.AND
            };

            this.ListConditionWheres.Add(condition);

            return this;
        }

#pragma warning disable CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        public INSql WhereNull(string column)
#pragma warning restore CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        {
            var condition = new Condition()
            {
                Operator = Op.Null,
                Column = column,
                ConditionValue = Cond.AND
            };

            this.ListConditionWheres.Add(condition);

            return this;
        }

#pragma warning disable CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0103 // The name 'Cond' does not exist in the current context
#pragma warning disable CS0246 // The type or namespace name 'Cond' could not be found (are you missing a using directive or an assembly reference?)
        public INSql WhereRaw(string sql, Cond condition = Cond.AND)
#pragma warning restore CS0246 // The type or namespace name 'Cond' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0103 // The name 'Cond' does not exist in the current context
#pragma warning restore CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        {
            var cond = new Condition()
            {
                Operator = Op.Raw,
                Raw = sql
            };

            this.ListConditionWheres.Add(cond);

            return this;
        }

        public void SetQuery(string sql)
        {
            this.SqlRaw = sql;
        }

        public string ToSql()
        {
            var raw = this.SqlRaw;

            //
            //  WHERE
            //

            if (this.ListConditionWheres.Count > 0)
            {
                raw += " WHERE";
                raw += ProcessConditions(this.ListConditionWheres);
            }

            //
            //  HAVING
            //

            if (this.ListConditionHavings.Count > 0)
            {
                raw += " HAVING";

                raw += ProcessConditions(this.ListConditionHavings);
            }

            // 
            // GROUP BY 
            //
            if (this.ListConditionGroupBy.Count > 0)
            {
                raw += " GROUP BY ";

                foreach (var group in this.ListConditionGroupBy)
                {
                    //Se nao for primeiro, verificar codicionar
                    if (this.ListConditionGroupBy.IndexOf(group) != 0)
                    {
                        raw += ",";
                    }

                    raw += String.Format("{0}", group.Column);
                }               
            }

            //
            // ORDER BY
            // 

            if (this.ListConditionOrderBy.Count > 0)
            {
                raw += " ORDER BY ";

                foreach (var order in this.ListConditionOrderBy)
                {
                    //Se nao for primeiro, verificar codicionar
                    if (this.ListConditionOrderBy.IndexOf(order) != 0)
                    {
                        raw += ",";
                    }

                    raw += String.Format("{0} {1}", order.Column, order.Order.ToString());
                }
            }

            //
            //  LIMIT
            //
            if (this.ConditionLimit != null)
            {
                raw += " LIMIT " + this.ConditionLimit.Limit;
            }

            //
            //  OFFSET
            //
            if (this.conditionOffSet != null)
            {
                raw += " OFFSET " + this.conditionOffSet.Offset;
            }


            // 
            // Repalce nos PARAMENTROS
            //

            if (this.ListConditionBindParams.Count > 0)
            {
                foreach(var param in this.ListConditionBindParams)
                {

                    var paramRaw = string.Format(":{0}", param.Param);
                    raw = raw.Replace(paramRaw, ToTypeSql(param.Value));
                }
            }

            return raw;
        }

#pragma warning disable CS0246 // The type or namespace name 'Op' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        public INSql Having(string column, Op operador, object valor)
#pragma warning restore CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'Op' could not be found (are you missing a using directive or an assembly reference?)
        {
            var condition = new Condition()
            {
                Column = column,
                Operator = operador,
                Value = valor,
                ConditionValue = Cond.AND
            };

            this.ListConditionHavings.Add(condition);

            return this;
        }

#pragma warning disable CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'Op' could not be found (are you missing a using directive or an assembly reference?)
        public INSql HavingOr(string column, Op operador, object valor)
#pragma warning restore CS0246 // The type or namespace name 'Op' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        {
            var condition = new Condition()
            {
                Column = column,
                Operator = operador,
                Value = valor,
                ConditionValue = Cond.OR
            };

            this.ListConditionHavings.Add(condition);

            return this;
        }

#pragma warning disable CS0246 // The type or namespace name 'Order' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0103 // The name 'Order' does not exist in the current context
        public INSql OrderBy(string column, Order order = Order.ASC)
#pragma warning restore CS0103 // The name 'Order' does not exist in the current context
#pragma warning restore CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'Order' could not be found (are you missing a using directive or an assembly reference?)
        {
            var condition = new ConditionOrderBy()
            {
                Column = column,
                Order = order
            };

            this.ListConditionOrderBy.Add(condition);

            return this;
        }

#pragma warning disable CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        public INSql Limit(int value)
#pragma warning restore CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        {
            var condition = new ConditionLimit()
            {
                Limit = value
            };

            this.ConditionLimit = condition;

            return this;
        }

#pragma warning disable CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        public INSql OffSet(int value)
#pragma warning restore CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        {
            var condition = new ConditionOffSet()
            {
                Offset = value
            };

            this.conditionOffSet = condition;

            return this;
        }

#pragma warning disable CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        public INSql Paginate(int limit, int numberPage)
#pragma warning restore CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        {
            throw new NotImplementedException();
        }

#pragma warning disable CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        public INSql GroupBy(string column)
#pragma warning restore CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        {
            var condition = new ConditionGroupBy()
            {
                Column = column,
            };

            this.ListConditionGroupBy.Add(condition);

            return this;
        }

#pragma warning disable CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        public INSql BindParam(string param, object value)
#pragma warning restore CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        {
            var condition = new ConditionBindParam()
            {
                Param = param,
                Value = value
            };

            this.ListConditionBindParams.Add(condition);
            return this;
        }

#pragma warning disable CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        public INSql JoinWhere(INSql query)
#pragma warning restore CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        {
            this.ListConditionWheres.AddRange(query.GetWheres());
            return this;
        }

#pragma warning disable CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        public INSql JoinHaving(INSql query)
#pragma warning restore CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        {
            this.ListConditionHavings.AddRange(query.GetHavings());
            return this;
        }

#pragma warning disable CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0103 // The name 'Cond' does not exist in the current context
#pragma warning disable CS0246 // The type or namespace name 'Cond' could not be found (are you missing a using directive or an assembly reference?)
        public INSql WhereNotIn(string column, string value, Cond condition = Cond.AND)
#pragma warning restore CS0246 // The type or namespace name 'Cond' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0103 // The name 'Cond' does not exist in the current context
#pragma warning restore CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        {
            var condtion = new Condition()
            {
                Column = column,
                Value = value,
                Operator = Op.NotIn,
                ConditionValue = condition
            };

            this.ListConditionWheres.Add(condtion);

            return this;
        }

#pragma warning disable CS0103 // The name 'Cond' does not exist in the current context
#pragma warning disable CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'Cond' could not be found (are you missing a using directive or an assembly reference?)
        public INSql WhereIn(string column, string value, Cond condition = Cond.AND)
#pragma warning restore CS0246 // The type or namespace name 'Cond' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0103 // The name 'Cond' does not exist in the current context
        {
            var condtion = new Condition()
            {
                Column = column,
                Value = value,
                Operator = Op.In,
                ConditionValue = condition
            };

            this.ListConditionWheres.Add(condtion);

            return this;
        }


#pragma warning disable CS0103 // The name 'Cond' does not exist in the current context
#pragma warning disable CS0246 // The type or namespace name 'Cond' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        public INSql WhereIn(string column, List<object> values, Cond condition = Cond.AND)
#pragma warning restore CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'Cond' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0103 // The name 'Cond' does not exist in the current context
        {
            var condtion = new Condition()
            {
                Column = column,
                Value = ConvertToIn(values),
                Operator = Op.In,
                ConditionValue = condition
            };

            this.ListConditionWheres.Add(condtion);

            return this;
        }

#pragma warning disable CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0103 // The name 'Cond' does not exist in the current context
#pragma warning disable CS0246 // The type or namespace name 'Cond' could not be found (are you missing a using directive or an assembly reference?)
        public INSql WhereNotIn(string column, object value, Cond condition = Cond.AND)
#pragma warning restore CS0246 // The type or namespace name 'Cond' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0103 // The name 'Cond' does not exist in the current context
#pragma warning restore CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        {
            var condtion = new Condition()
            {
                Column = column,
                Value = value,
                Operator = Op.In,
                ConditionValue = condition
            };

            this.ListConditionWheres.Add(condtion);

            return this;
        }

#pragma warning disable CS0103 // The name 'Cond' does not exist in the current context
#pragma warning disable CS0246 // The type or namespace name 'Cond' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        public INSql WhereNotIn(string column, List<object> values, Cond condition = Cond.AND)
#pragma warning restore CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'Cond' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0103 // The name 'Cond' does not exist in the current context
        {
            var condtion = new Condition()
            {
                Column = column,
                Value = ConvertToIn(values),
                Operator = Op.NotIn,
                ConditionValue = condition
            };

            this.ListConditionWheres.Add(condtion);

            return this;
        }

#pragma warning disable CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        public INSql WhereLike(string column, string value)
#pragma warning restore CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        {
            var condtion = new Condition()
            {
                Column = column,
                Value = value,
                Operator = Op.Like,
                ConditionValue = Cond.AND
            };

            this.ListConditionWheres.Add(condtion);

            return this;
        }

#pragma warning disable CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        public INSql WhereLikeOr(string column, string value)
#pragma warning restore CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        {
            var condtion = new Condition()
            {
                Column = column,
                Value = value,
                Operator = Op.Like,
                ConditionValue = Cond.OR
            };

            this.ListConditionWheres.Add(condtion);

            return this;
        }

#pragma warning disable CS0103 // The name 'Cond' does not exist in the current context
#pragma warning disable CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'Cond' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        public INSql Where(Action<INSql> SubCondition, Cond condition = Cond.AND)
#pragma warning restore CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'Cond' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0103 // The name 'Cond' does not exist in the current context
        {
            var condtion = new Condition()
            {
                SubCondition = SubCondition,
                ConditionValue = condition
            };

            this.ListConditionWheres.Add(condtion);

            return this;
        }

#pragma warning disable CS0246 // The type or namespace name 'Cond' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0103 // The name 'Cond' does not exist in the current context
        public INSql Where(INSql subquery, Cond condition = Cond.AND)
#pragma warning restore CS0103 // The name 'Cond' does not exist in the current context
#pragma warning restore CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'Cond' could not be found (are you missing a using directive or an assembly reference?)
        {
            var cond = new Condition()
            {
                Operator = Op.Raw,
                Raw = String.Format("( {0} )", subquery.ToSql())
            };

            this.ListConditionWheres.Add(cond);

            return this;
        }

#pragma warning disable CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0103 // The name 'Cond' does not exist in the current context
#pragma warning disable CS0246 // The type or namespace name 'Cond' could not be found (are you missing a using directive or an assembly reference?)
        public INSql Having(INSql subquery, Cond condition = Cond.AND)
#pragma warning restore CS0246 // The type or namespace name 'Cond' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0103 // The name 'Cond' does not exist in the current context
#pragma warning restore CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        {
            var cond = new Condition()
            {
                Operator = Op.Raw,
                Raw = String.Format("( {0} )", subquery.ToSql())
            };

            this.ListConditionHavings.Add(cond);

            return this;
        }

        public string ToWhere()
        {
            return this.ProcessConditions(this.ListConditionWheres);
        }

        public List<Condition> GetWheres()
        {
            return this.ListConditionWheres;
        }

        public List<Condition> GetHavings()
        {
            return this.ListConditionHavings;
        }

        #endregion

        #region "Overrides"

#pragma warning disable CS0115 // 'NSQL.ToString()': no suitable method found to override
        public override string ToString()
#pragma warning restore CS0115 // 'NSQL.ToString()': no suitable method found to override
        {
            return base.ToString();
        }

#pragma warning disable CS0103 // The name 'Cond' does not exist in the current context
#pragma warning disable CS0246 // The type or namespace name 'Cond' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        public INSql WhereFindInSet(string column, List<object> values, Cond condition = Cond.AND)
#pragma warning restore CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'Cond' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0103 // The name 'Cond' does not exist in the current context
        {
            var cond = new Condition()
            {
                Operator = Op.FindInSet,
                ConditionValue = condition,
                Column = column,
                Value = ConvertToIn(values)
            };

            this.ListConditionWheres.Add(cond);
            return this;
        }

#pragma warning disable CS0103 // The name 'Cond' does not exist in the current context
#pragma warning disable CS0246 // The type or namespace name 'Cond' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        public INSql WhereFindInSet(string column, string value, Cond condition = Cond.AND)
#pragma warning restore CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'Cond' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0103 // The name 'Cond' does not exist in the current context
        {
            var cond = new Condition()
            {
                Operator = Op.FindInSet,
                ConditionValue = condition,
                Column  = column,
                Value = value
            };

            this.ListConditionWheres.Add(cond);
            return this;
        }

        #endregion
    }


    #region "Class util"
    public class Condition
    {
        public string Column { get; set; }
#pragma warning disable CS0246 // The type or namespace name 'Op' could not be found (are you missing a using directive or an assembly reference?)
        public Op Operator { get; set; }
#pragma warning restore CS0246 // The type or namespace name 'Op' could not be found (are you missing a using directive or an assembly reference?)
        public object Value { get; set; }
        public string ValueFrom { get; set; }
        public string ValueTo { get; set; }
        public string Raw { get; set; }
#pragma warning disable CS0246 // The type or namespace name 'Cond' could not be found (are you missing a using directive or an assembly reference?)
        public Cond ConditionValue { get; set; }
#pragma warning restore CS0246 // The type or namespace name 'Cond' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
        public Action<INSql> SubCondition { get; set; }
#pragma warning restore CS0246 // The type or namespace name 'INSql' could not be found (are you missing a using directive or an assembly reference?)
    }

    public class ConditionOrderBy
    {
        public string Column { get; set; }
#pragma warning disable CS0246 // The type or namespace name 'Order' could not be found (are you missing a using directive or an assembly reference?)
        public Order Order { get; set; }
#pragma warning restore CS0246 // The type or namespace name 'Order' could not be found (are you missing a using directive or an assembly reference?)
    }

    public class ConditionGroupBy
    {
        public string Column { get; set; }
    }

    public class ConditionLimit
    {
        public int Limit { get; set; }
    }

    public class ConditionOffSet
    {
        public int Offset { get; set; }
    }

    public class ConditionBindParam
    {
        public string Param { get; set; }
        public object Value { get; set; }
    }

    public static class TypeExtensions
    {
        public static bool IsArrayOf<T>(this Type type)
        {
            return type == typeof(T[]);
        }
    }

    #endregion
}
