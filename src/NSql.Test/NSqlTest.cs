using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#pragma warning disable CS0246 // The type or namespace name 'NSQL' could not be found (are you missing a using directive or an assembly reference?)
using NSQL;
#pragma warning restore CS0246 // The type or namespace name 'NSQL' could not be found (are you missing a using directive or an assembly reference?)

namespace NSql.Test
{
    [TestClass]
    public class NSqlTest
    {
        [TestMethod]
        public void TestIntance()
        {
            var sqlRaw = "select * from test";
             var nsql = new NSQL.NSQL(sqlRaw);
             
             Assert.AreEqual(nsql.ToSql(), sqlRaw,true);
        }

        [TestMethod]
        public void TestStatic()
        {
            var sqlRaw = "select * from test";
            var nsql = NSQL.NSQL.Create(sqlRaw);

            Assert.AreEqual(nsql.ToSql(), sqlRaw, true);
        }

        [TestMethod]
        public void TestConditionWhereNumber()
        {
            var sqlRaw = "select * from test";
            var nsql = new NSQL.NSQL(sqlRaw);

            var valor =10;

            nsql.Where("coluna1", Op.IGUAL, valor);

            Assert.AreEqual(nsql.ToSql(), sqlRaw + " WHERE coluna1 = 10",true);
        }

        [TestMethod]
        public void TestConditionWhereString()
        {
            var sqlRaw = "select * from test";
            var nsql = new NSQL.NSQL(sqlRaw);

            var texto = "nome do cliente";

            nsql.Where("coluna1", Op.IGUAL, texto);
            nsql.Where("coluna2", Op.Diferente, texto);

            Assert.AreEqual(nsql.ToSql(), sqlRaw + " WHERE coluna1 = 'nome do cliente' AND coluna2 != 'nome do cliente'",true);
        }

        [TestMethod]
        public void TestConditionHavingString()
        {
            var sqlRaw = "select * from test";
            var nsql = new NSQL.NSQL(sqlRaw);

            var texto = "nome do cliente";

            nsql.Having("coluna1", Op.IGUAL, texto);
            nsql.Having("coluna2", Op.Diferente, texto);

            Assert.AreEqual(nsql.ToSql(), sqlRaw + " HAVING coluna1 = 'nome do cliente' AND coluna2 != 'nome do cliente'", true);
        }

        [TestMethod]
        public void TestConditionHavingNumber()
        {
            var sqlRaw = "select * from test";
            var nsql = new NSQL.NSQL(sqlRaw);

            var numero = 10;

            nsql.Having("coluna1", Op.IGUAL, numero);
            nsql.Having("coluna2", Op.Diferente, numero);

            Assert.AreEqual(nsql.ToSql(), sqlRaw + " HAVING coluna1 = 10 AND coluna2 != 10", true);
        }

        [TestMethod]
        public void TestConditionHavingOrNumber()
        {
            var sqlRaw = "select * from test";
            var nsql = new NSQL.NSQL(sqlRaw);

            var numero = 10;

            nsql.Having("coluna1", Op.IGUAL, numero);
            nsql.HavingOr("coluna2", Op.Diferente, numero);
            nsql.HavingOr("coluna3", Op.Diferente, numero);

            Assert.AreEqual(nsql.ToSql(), sqlRaw + " HAVING coluna1 = 10 OR coluna2 != 10 OR coluna3 != 10", true);
        }


        [TestMethod]
        public void TestConditionWhereOr()
        {
            var sqlRaw = "select * from test";
            var nsql = new NSQL.NSQL(sqlRaw);

            var numero = 10;

            nsql.Where("coluna1", Op.IGUAL, numero);
            nsql.WhereOr("coluna2", Op.Diferente, numero);


            var expected = sqlRaw + " WHERE coluna1 = 10 OR coluna2 != 10";
            Assert.AreEqual(expected, nsql.ToSql(), true);
        }

        [TestMethod]
        public void TestConditionParams()
        {
            var sqlRaw = "select * from test t1 join teste2 t2 on t1.id = t2.id and t2.parametro = :parametroTeste";
            var nsql = new NSQL.NSQL(sqlRaw);

            var numero = 10;
            nsql.BindParam("parametroTeste", numero);

            var expected = "select * from test t1 join teste2 t2 on t1.id = t2.id and t2.parametro = 10";
            Assert.AreEqual(expected,nsql.ToSql(), true);
        }

        [TestMethod]
        public void TestConditionOrdeBy()
        {
            var sqlRaw = "select * from test";
            var nsql = new NSQL.NSQL(sqlRaw);

            nsql.OrderBy("coluna1");
            nsql.OrderBy("coluna2",Order.DESC);

            var expected = "select * from test ORDER BY coluna1 ASC,coluna2 DESC";
            Assert.AreEqual(expected, nsql.ToSql(), true);
        }

        [TestMethod]
        public void TestConditionGroupBy()
        {
            var sqlRaw = "select * from test";
            var nsql = new NSQL.NSQL(sqlRaw);

            nsql.GroupBy("coluna1");
            nsql.GroupBy("coluna2");

            var expected = "select * from test GROUP BY coluna1,coluna2";
            Assert.AreEqual(expected, nsql.ToSql(), true);
        }

        [TestMethod]
        public void TestConditionLimit()
        {
            var sqlRaw = "select * from test";
            var nsql = new NSQL.NSQL(sqlRaw);

            nsql.Limit(100);

            var expected = "select * from test LIMIT 100";
            Assert.AreEqual(expected, nsql.ToSql(), true);
        }

        [TestMethod]
        public void TestConditionWhereLike()
        {
            var sqlRaw = "select * from test";
            var nsql = new NSQL.NSQL(sqlRaw);

            nsql.WhereLike("coluna1", "%teste%");

            var expected = "select * from test WHERE coluna1 LIKE '%teste%'";
            Assert.AreEqual(expected, nsql.ToSql(), true);
        }


        [TestMethod]
        public void TestConditionWhereLikeOr()
        {
            var sqlRaw = "select * from test";
            var nsql = new NSQL.NSQL(sqlRaw);

            nsql.WhereLike("coluna1", "%teste%");
            nsql.WhereLikeOr("coluna2", "%teste%");

            var expected = "select * from test WHERE coluna1 LIKE '%teste%' OR coluna2 LIKE '%teste%'";
            Assert.AreEqual(expected, nsql.ToSql(), true);
        }

        [TestMethod]
        public void TestConditionWhereIn()
        {
            var sqlRaw = "select * from test";
            var nsql = new NSQL.NSQL(sqlRaw);

            nsql.WhereIn("coluna1", "2,3,4");

            var expected = "select * from test WHERE coluna1 IN (2,3,4)";
            Assert.AreEqual(expected, nsql.ToSql(), true);
        }

        [TestMethod]
        public void TestConditionWhereInArrayString()
        {
            var sqlRaw = "select * from test";
            var nsql = new NSQL.NSQL(sqlRaw);

            var valores = new List<Object>() { "teste", "teste2" };

            nsql.WhereIn("coluna1", valores);

            var expected = "select * from test WHERE coluna1 IN ('teste','teste2')";
            Assert.AreEqual(expected, nsql.ToSql(), true);
        }

        [TestMethod]
        public void TestConditionWhereNotInArrayInt()
        {
            var sqlRaw = "select * from test";
            var nsql = new NSQL.NSQL(sqlRaw);

            var valores = new List<Object>() { 151, 65656};

            nsql.WhereNotIn("coluna1", valores);

            var expected = "select * from test WHERE coluna1 NOT IN (151,65656)";
            Assert.AreEqual(expected, nsql.ToSql(), true);
        }

        [TestMethod]
        public void TestConditionWhereSubCondition()
        {
            var sqlRaw = "select * from test";
            var nsql = new NSQL.NSQL(sqlRaw);

            nsql.Where("coluna2", Op.IGUAL, "teste").Where(x =>
              {
                  x.Where("coluna1", Op.IGUAL, "teste");
              });

            var expected = "select * from test WHERE coluna2 = 'teste' AND ( coluna1 = 'teste' )";
            Assert.AreEqual(expected, nsql.ToSql(), true);
        }

        [TestMethod]
        public void TestConditionWhereRaw()
        {
            var sqlRaw = "select * from test";
            var nsql = new NSQL.NSQL(sqlRaw);

            nsql.WhereRaw("COALESCE(coluna1) = 1");
            nsql.WhereRaw("( select * from teste2 t where t.coluna2 = 1 )",Cond.OR);

            var expected = "select * from test WHERE COALESCE(coluna1) = 1 AND ( select * from teste2 t where t.coluna2 = 1 )";
            Assert.AreEqual(expected, nsql.ToSql(), true);
        }

        [TestMethod]
        public void TestConditionWhereSubQuery()
        {
            // Query Principal
            var sqlRaw = "select * from test";
            var nsql = new NSQL.NSQL(sqlRaw);

            // SUb Query
            var nSubSql = new NSQL.NSQL("select * from teste2");
            nSubSql.Where("coluna1", Op.IGUAL, "TESTE");

            // Adiciona sub query 
            nsql.Where(nSubSql);
    
            var expected = "select * from test WHERE ( select * from teste2 WHERE coluna1 = 'TESTE' )";
            Assert.AreEqual(expected, nsql.ToSql(), true);
        }


        [TestMethod]
        public void TestConditionWhereJoinWhere()
        {
            // Query Principal
            var sqlRaw = "select * from test";
            var nsql = new NSQL.NSQL(sqlRaw);

            // Query 2
            var nSubSql = new NSQL.NSQL();
            nSubSql.Where("coluna1", Op.IGUAL, "TESTE");

            // Adiciona sub query 
            nsql.JoinWhere(nSubSql);

            var expected = "select * from test WHERE coluna1 = 'TESTE'";
            Assert.AreEqual(expected, nsql.ToSql(), true);
        }

        [TestMethod]
        public void TestConditionWhereJoinHaving()
        {
            // Query Principal
            var sqlRaw = "select * from test";
            var nsql = new NSQL.NSQL(sqlRaw);

            // Query 2
            var nSubSql = new NSQL.NSQL();
            nSubSql.Having("coluna1", Op.IGUAL, "TESTE");

            // Adiciona sub query 
            nsql.JoinHaving(nSubSql);

            var expected = "select * from test HAVING coluna1 = 'TESTE'";
            Assert.AreEqual(expected, nsql.ToSql(), true);
        }

        [TestMethod]
        public void TestConditionWhereFindInSetString()
        {
            // Query Principal
            var sqlRaw = "select * from test";
            var nsql = new NSQL.NSQL(sqlRaw);

            nsql.WhereFindInSet("coluna1", "1,2,3,4,5");
      

            var expected = "select * from test WHERE FIND_IN_SET(coluna1,'1,2,3,4,5')";
            Assert.AreEqual(expected, nsql.ToSql(), true);
        }

        [TestMethod]
        public void TestConditionWhereFindInSetList()
        {
            // Query Principal
            var sqlRaw = "select * from test";
            var nsql = new NSQL.NSQL(sqlRaw);

            nsql.WhereFindInSet("coluna1", new List<object>() {1,2,3,4,5});

            var expected = "select * from test WHERE FIND_IN_SET(coluna1,'1,2,3,4,5')";
            Assert.AreEqual(expected, nsql.ToSql(), true);
        }
    }
}
