using System;
using System.Linq;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Linq;
using NHibernate.Tool.hbm2ddl;

namespace Sharp.RemoteQueryable.Tests.Infrastructure
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

    /// <summary>
    /// Фабрика сессии.
    /// </summary>
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

          // INFO Vanyushkin_MV: раскомментировать для генерации схемы БД.
          new SchemaUpdate(configuration).Execute(false, true);
          _sessionFactory = configuration.BuildSessionFactory();
          return _sessionFactory;
        }
      }
    }

    /// <summary>
    /// Открыть сессию работы с БД.
    /// </summary>
    /// <returns>Сессия.</returns>
    /// <remarks>Не потокобезопасна!</remarks>
    public static ISession OpenSession()
    {
      return SessionFactory.OpenSession();
    }

    static NHibernateHelper()
    {
      HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();
    }
  }
}
