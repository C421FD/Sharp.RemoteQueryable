using System;
using System.Linq;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Linq;
using NHibernate.Tool.hbm2ddl;

namespace Sharp.RemoteQueryable.Samples.WcfServer
{
  public static class NHibernateHelper
  {
    [ThreadStatic]
    private static ISessionFactory _sessionFactory;

    private static readonly object _sessionLock = new object();

    public static string GetGeneratedSql(IQueryable queryable, ISession session)
    {
      var sessionImp = (ISessionImplementor)session;
      var nhLinqExpression = new NhLinqExpression(queryable.Expression, sessionImp.Factory);
      var translatorFactory = new ASTQueryTranslatorFactory();
      var translators = translatorFactory.CreateQueryTranslators(nhLinqExpression, null, false, sessionImp.EnabledFilters, sessionImp.Factory);
      return translators[0].SQLString;
    }

    private static ISessionFactory SessionFactory
    {
      get
      {
        lock (_sessionLock)
        {
          if (_sessionFactory != null)
            return _sessionFactory;

          var configuration = new Configuration();
          configuration.Configure();

          new SchemaUpdate(configuration).Execute(false, true);
          _sessionFactory = configuration.BuildSessionFactory();
          return _sessionFactory;
        }
      }
    }

    public static ISession OpenSession()
    {
      return SessionFactory.OpenSession();
    }

    static NHibernateHelper()
    {
      
    }
  }
}
