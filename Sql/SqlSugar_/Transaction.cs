using SqlSugar;
using System.Drawing;


public class Transaction
{
    public void Execute()
    {
        try
        {
            //创建数据库对象 (用法和EF Dappper一样通过new保证线程安全)
            SqlSugarClient Db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = "server=localhost;Database=LaserResult;Uid=root;Pwd=root123",
                DbType = DbType.MySql,
                IsAutoCloseConnection = true
            },

            //用于配置 SqlSugarClient 的 AOP（面向切面编程）功能和其他设置
            db =>
            {
                //db.Aop.OnLogExecuting 是配置 SQL 执行前的事件处理程序。每当 SqlSugar 要执行一个 SQL 语句时，都会触发这个事件。
                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    //会将参数化的 SQL 转换为完整的、可直接执行的 SQL 语句,并输出控制台
                    //获取原生SQL推荐 5.1.4.63  性能OK
                    Console.WriteLine(UtilMethods.GetNativeSql(sql, pars));

                    //获取无参数化SQL 对性能有影响，特别大的SQL参数多的，调试使用
                    //Console.WriteLine(UtilMethods.GetSqlString(DbType.MySql,sql,pars))
                };

                //注意多租户 有几个设置几个
                //如果有多个租户连接，需要为每个连接单独配置 AOP
                //db.GetConnection(i).Aop

            });


        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }


    //单库事务
    void Transaction1_(SqlSugarClient db)
    {
        try
        {
            db.Ado.BeginTran();



            db.Ado.CommitTran();
        }
        catch (Exception ex)
        {
            db.Ado.RollbackTran();
            Console.WriteLine(ex);
        }
    }


    //多库事务
    void Transaction2_(SqlSugarClient db)
    {
        try
        {
            db.BeginTran();

            //悲观锁,只有执行完整个事务，别处才能看到此处更新的事务，一旦执行事务中断则回滚
            //查询条件记录后锁表
            var data = db.Queryable<Student>()
                .TranLock(DbLockType.Wait)
                .Where(it => it.Id == 1)
                .ToList();//返回条数尽量最少尽量,只锁行，不锁表，锁表的话非常影响性能和效率


            db.CommitTran();
        }
        catch (Exception ex)
        {
            db.RollbackTran();
            Console.WriteLine(ex);
        }
    }


    //语法糖
    void Transaction3_(SqlSugarClient db)
    {
        //不扔出错误
        var result = db.UseTran(() =>
        {
            var beginCount = db.Queryable<Student>().ToList();
            db.Ado.ExecuteCommand("delete Order");
            var endCount = db.Queryable<Student>().Count();
            return true;// 返回值等行result.Data
        });

        if (result.Data == false) //返回值为false
        {
            //result.Data 业务的返回值 
            //如果是上面的逻辑 result.Data==true肯定业务成功并且事务成功
            //if(result.IsSuccess==false)//事务执行了回滚
            //if(result.IsSuccess==true)//事务提交完成
        }
    }


    [SugarTable("dbstudent")]//当和数据库名称不一样可以设置表别名 指定表明
    public class Student
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]//数据库是自增才配自增 
        public int Id { get; set; }

        public int? SchoolId { get; set; }

        //数据库与实体不一样设置列名 
        [SugarColumn(ColumnName = "StudentName")]
        public string Name { get; set; }

        //忽略该列
        [SugarColumn(IsIgnore = true)]
        public PointF MeasCenterPoint { get; set; }

        public bool isdeleted { get; set; }
    }
}