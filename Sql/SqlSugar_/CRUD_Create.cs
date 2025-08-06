using SqlSugar;
using System.Diagnostics;
using System.Drawing;


public class CRUD_Create
{
    public void Execute()
    {
        try
        {
            //创建数据库对象 (用法和EF Dappper一样通过new保证线程安全)
            SqlSugarClient Db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = "server=localhost;Database=LaserResult;Uid=root;Pwd=root123;AllowLoadLocalInfile=true",
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

            CreatBatch(Db);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }


    // 单行插入
    void Create_(SqlSugarClient Db)
    {
        //建表
        Db.CodeFirst.SetStringDefaultLength(200).InitTables(typeof(Student));

        var stu = new Student
        {
            SchoolId = 28,
            Name = "xzq4"
        };

        //返回插入行数
        Console.WriteLine(Db.Insertable(stu).ExecuteCommand());

        //插入返回自增列(实体除ORACLE外实体要配置自增，Oracle需要配置序列)
        Console.WriteLine(Db.Insertable(stu).ExecuteReturnIdentity());

        //手动调用获取雪花ID
        var id = SnowFlakeSingle.Instance.NextId();
        Console.WriteLine(id);
    }


    //批量数据插入（万金油写法，适用于小于5W条的数据）
    void CreatBatch(SqlSugarClient Db)
    {
        var stuList = new List<Student>();

        for (int i = 0; i < 100; i++)
        {
            var stu = new Student
            {
                SchoolId = i,
                Name = $"xzq{i}"
            };
            stuList.Add(stu);
        }
        var sw = new Stopwatch();
        sw.Start();
        Db.Insertable(stuList).ExecuteCommand();
        sw.Stop();
        Console.WriteLine(sw.ElapsedMilliseconds);
    }


    //大批量插入
    //插入 100万 数秒时间
    //此时MySql连接字符串要加AllowLoadLocalInfile=true
    void CreatMass(SqlSugarClient db)
    {
        var stuList = new List<Student>();

        for (int i = 0; i < 1000000; i++)
        {
            var stu = new Student
            {
                SchoolId = i,
                Name = $"xzq{i}",
                isdeleted = false
            };
            stuList.Add(stu);
        }

        var sw = new Stopwatch();
        sw.Start();

        db.Fastest<Student>().PageSize(50000).BulkCopy(stuList);//大数据分页插

        sw.Stop();
        Console.WriteLine(sw.ElapsedMilliseconds);
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