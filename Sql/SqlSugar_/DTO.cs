
#if false

 DTO 是一种设计模式。它是一个简单的容器对象，专门用于在不同层（或子系统）之间传输数据。

 例如，同一个 `User` 实体，在“用户列表”API 中可能只需要 `Id` 和 `Name`（`UserSummaryDto`），而在“用户详情”API 中则需要更多信息（`UserDetailDto`）。

 这种需求可以用mapster映射
    可以避免样板代码：（如 `dto.PropertyA = entity.PropertyA; dto.PropertyB = entity.PropertyB; ...`）非常繁琐、冗长、容易出错且难以维护，
    尤其是在对象有很多属性或需要频繁映射时。

    接下来的代码需要安装nuget包：mapster
#endif




using Mapster;
using SqlSugar;
using System.Drawing;


public class DTO
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

            Dto2_(Db);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }


    //自动转DTO
    void Dto1_(SqlSugarClient db)
    {
        var list = db.Queryable<Student>()
                        .ToList();
        //DTO和List不能是同一个类不然这种映射会失效
        var dtolist = list.Adapt<List<StudentDto>>();
    }

    //手动转DTO
    void Dto2_(SqlSugarClient db)
    {
        //简单的用法   5.1.4.71
        var list = db.Queryable<Student>()
                .Select(x => new StudentDto2
                {
                    Name2 = x.Name

                }, true)//true是自动映射其他属性，匿名对象需要手动
                   .ToList();
    }

    public class StudentDto
    {
        public int Id { get; set; }

        public int? SchoolId { get; set; }

        public string Name { get; set; }

    }

    public class StudentDto2
    {
        public int Id { get; set; }

        public int? SchoolId { get; set; }

        public string? Name2 { get; set; }

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

        [Navigate(NavigateType.OneToOne, nameof(SchoolId), nameof(SchoolA.schoolId))]//变量名不要等类名 
        public School SchoolA { get; set; } //不能赋值只能是null
    }


    [SugarTable("dbschool")]//当和数据库名称不一样可以设置表别名 指定表明
    public class School
    {
        public int? schoolId { get; set; }

        public string? schoolName { get; set; }
    }
}