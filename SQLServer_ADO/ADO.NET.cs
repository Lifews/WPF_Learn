using Microsoft.Data.SqlClient;
using System.Data;



Delect();
Insert();
Select();
LoginVerification();



#if false //（直观）调用数据库的流程

static void Select()
{
    //连接数据库 此处使用本地连接
    SqlConnection sqlConnection = new();
    sqlConnection.ConnectionString = "Server=Lifews\\SQLEXPRESS;Database=FirstTry;Trusted_Connection=true;TrustServerCertificate=true;";
    sqlConnection.Open();

    SqlCommand sqlCommand = new();
    sqlCommand.Connection = sqlConnection;
    sqlCommand.CommandText = "select * from PasswordTable";

    SqlDataAdapter adapter = new SqlDataAdapter();
    adapter.SelectCommand = sqlCommand;

    DataSet ds = new DataSet();
    adapter.Fill(ds);

    sqlConnection.Close();

    DataTable dt = ds.Tables[0];

    for (int i = 0; i < dt.Rows.Count; i++)
    {
        Console.WriteLine($"{dt.Rows[i]["Web111"]}  {dt.Rows[i]["UserName"]}  {dt.Rows[i]["Password"]}");
    }

    Console.WriteLine("Hello, World!");
}


static void Insert()
{
    //连接数据库 此处使用本地连接
    SqlConnection sqlConnection = new();
    sqlConnection.ConnectionString = "Server=Lifews\\SQLEXPRESS;Database=FirstTry;Trusted_Connection=true;TrustServerCertificate=true;";
    sqlConnection.Open();

    string Web111 = "xzq01";
    string UserName = "11101";
    string Password = "2222";
    string insertSQL = $"Insert into PasswordTable values ('{Web111}','{UserName}','{Password}');";

    SqlCommand sqlCommand = new SqlCommand(insertSQL, sqlConnection);

    int count = sqlCommand.ExecuteNonQuery();

    sqlConnection.Close();

    Console.WriteLine($"受影响的行数是：{count}行");
}

#endif

void LoginVerification()
{
    while (true)
    {
        Console.WriteLine("请输入用户名");
        string? inputName = Console.ReadLine();
        Console.WriteLine("请输入密码");
        string? password = Console.ReadLine();

        string sql = $"select * from PasswordTable where username='{inputName}' and password='{password}'";
        DataTable dt = SelectData(sql);

        if (dt.Rows.Count <= 0)
        {
            Console.WriteLine("用户名和密码不正确！");
            continue;
        }
        break;
    }
    Console.WriteLine("欢迎访问本系统");
}


void Select()
{
    string sql = "select * from PasswordTable";

    DataTable dt = SelectData(sql);

    for (int i = 0; i < dt.Rows.Count; i++)
    {
        Console.WriteLine($"{dt.Rows[i]["Web111"]}  {dt.Rows[i]["UserName"]}  {dt.Rows[i]["Password"]}");
    }
}


void Insert()
{
    string Web111_1 = "xzq01";
    string Web111_2 = "xzq02";
    string UserName = "11101";
    string Password = "22202";
    string insertSQL_1 = $"Insert into PasswordTable values ('{Web111_1}','{UserName}','{Password}');";
    string insertSQL_2 = $"Insert into PasswordTable values ('{Web111_2}','{UserName}','{Password}');";

    int count_1 = EditData(insertSQL_1);
    if (count_1 > 0)
        Console.WriteLine($"新增{count_1}行数据成功");
    else
        Console.WriteLine("新增数据失败");

    int count_2 = EditData(insertSQL_2);
    if (count_2 > 0)
        Console.WriteLine($"新增{count_2}行数据成功");
    else
        Console.WriteLine("新增数据失败");
}


void Delect()
{
    string delectSQL = "delete from PasswordTable;";

    int count = EditData(delectSQL);

    if (count > 0)
        Console.WriteLine($"删除{count}行数据成功");
    else
        Console.WriteLine("删除数据失败");
}


static int EditData(string sql)
{
    //连接数据库 此处使用本地连接
    SqlConnection sqlConnection = new();
    //输入数据库的信息，（包括 服务器实例、账号、密码……）
    sqlConnection.ConnectionString = "Server=Lifews\\SQLEXPRESS;Database=FirstTry;Trusted_Connection=true;TrustServerCertificate=true;";
    sqlConnection.Open();

    SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);

    int count = 0;

    try
    {
        count = sqlCommand.ExecuteNonQuery();
        Console.WriteLine($"受影响的行数为{count}行");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.ToString());
    }

    sqlConnection.Close();
    return count;
}


static DataTable SelectData(string sql)
{
    //连接数据库 此处使用本地连接
    SqlConnection sqlConnection = new();
    sqlConnection.ConnectionString = "Server=Lifews\\SQLEXPRESS;Database=FirstTry;Trusted_Connection=true;TrustServerCertificate=true;";
    sqlConnection.Open();

    SqlCommand sqlCommand = new();
    sqlCommand.Connection = sqlConnection;
    sqlCommand.CommandText = sql;

    SqlDataAdapter adapter = new SqlDataAdapter();
    adapter.SelectCommand = sqlCommand;

    DataSet ds = new DataSet();
    adapter.Fill(ds);

    sqlConnection.Close();

    DataTable dt = ds.Tables[0];

    return dt;
}