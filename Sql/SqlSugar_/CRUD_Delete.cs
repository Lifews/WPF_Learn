using SqlSugar;
using System.Drawing;


public class CRUD_Delete
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

            SoftDelete(Db);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }


    void Delete1(SqlSugarClient db)
    {
        //根据主键删除
        var result = db.Deleteable<Student>(new Student() { Id = 2 }).ExecuteCommand();
        Console.WriteLine(result);
    }

    void Delete2(SqlSugarClient db)
    {
        //根据主键删除
        var result = db.Deleteable<Student>().In(1).ExecuteCommand();
        Console.WriteLine(result);
    }


    //软删除（逻辑删除）
    void SoftDelete(SqlSugarClient db)
    {
        //如实体属性有isdelete或者isdeleted
        //假删除、软删除，会把isdelete或者isdeleted属性置为1
        //当然也可以指定属性，不一定是isdelete或者isdeleted
        var result = db.Deleteable<Student>().In(1).IsLogic().ExecuteCommand(); 
        Console.WriteLine(result);
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