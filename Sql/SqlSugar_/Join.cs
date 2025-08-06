using SqlSugar;
using System.Drawing;


public class Join()
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

            RightJoin_(Db);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    //内联结
    void InnerJoin_(SqlSugarClient db)
    {
        var dt = db.Queryable<Student>()
                   .InnerJoin<School>((st, sc) => st.SchoolId == sc.schoolId)
                   .Select((st, sc) => new { st.Name, st.SchoolId, sc.schoolName })
                   .ToList();

        for (int i = 0; i < dt.Count; i++)
        {
            Console.WriteLine($"{dt[i].Name}     {dt[i].SchoolId}     {dt[i].schoolName}  ");
        }
    }

    //左联结
    void LeftJoin_(SqlSugarClient db)
    {
        var dt = db.Queryable<Student>()
                   .LeftJoin<School>((st, sc) => st.SchoolId == sc.schoolId)
                   .Select((st, sc) => new { st.Name, st.SchoolId, sc.schoolName })
                   .ToList();

        for (int i = 0; i < dt.Count; i++)
        {
            Console.WriteLine($"{dt[i].Name}     {dt[i].SchoolId}     {dt[i].schoolName}  ");
        }
    }

    //右联结
    void RightJoin_(SqlSugarClient db)
    {
        var dt = db.Queryable<Student>()
                   .RightJoin<School>((st, sc) => st.SchoolId == sc.schoolId)
                   .Select((st, sc) => new { st.Name, st.SchoolId, sc.schoolName })
                   .ToList();

        for (int i = 0; i < dt.Count; i++)
        {
            Console.WriteLine($"{dt[i].Name}     {dt[i].SchoolId}     {dt[i].schoolName}  ");
        }
    }


    [SugarTable("dbschool")]//当和数据库名称不一样可以设置表别名 指定表明
    public class School
    {
        public int schoolId { get; set; }

        public string? schoolName { get; set; }
    }


    [SugarTable("dbstudent")]//当和数据库名称不一样可以设置表别名 指定表明
    public class Student
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]//数据库是自增才配自增 
        public int schoolId { get; set; }

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