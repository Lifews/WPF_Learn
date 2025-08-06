using SqlSugar;


public class DbFirst
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

            DbFirst_(Db);
            Select_(Db);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }


    void DbFirst_(SqlSugarClient Db)
    {
        //Db First 实体生成
        Db.DbFirst.IsCreateAttribute().StringNullable().CreateClassFile("D:\\SQL_Datas\\test1", "test1");
        Db.DbFirst.Where(it => it.ToLower().StartsWith("pcb01")).CreateClassFile("D:\\SQL_Datas\\test2", "test2");
    }


    void Select_(SqlSugarClient Db)
    {
        //查询
        List<pcb01> list = Db.Queryable<pcb01>().ToList();

        List<pcb01> list1 = Db.Queryable<pcb01>().Where(it => it.AreaIndex == 1).ToList();

        List<pcb01> list2 = Db.Queryable<pcb01>().Where(it => it.Index > 1 && it.AreaIndex == 1).ToList();

        var formatted = list1.Select(p => $"Index={p.Index} (AreaIndex={p.AreaIndex})");
        Console.WriteLine(string.Join(Environment.NewLine, formatted));
    }


    //实体与数据库结构一样
    public partial class pcb01
    {
        public pcb01()
        {


        }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        [SugarColumn(IsPrimaryKey = true)]
        public int Index { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? AreaIndex { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public float? PointFCenterX { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public float? PointFCenterY { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public float? Z { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? ProgramT { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public float? Diameter { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? RowIndex { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? ColIndex { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public float? MeasAreaCenterX { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public float? MeasAreaCenterY { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public float? MeasAreaHoleOffsetX { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public float? MeasAreaHoleOffsetY { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public float? MeasAreaHoleCenterX { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public float? MeasAreaHoleCenterY { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public float? MeasAreaHoleRadius { get; set; }

    }
}
