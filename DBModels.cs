using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Media.Imaging;
using System.IO;
using System.Globalization;

namespace Дипломчик
{
    public abstract class ABCModel
    {
        public abstract string TableName { get; }

        public int Id { get; set; }

        public abstract string GetInsertString();
        public abstract string GetUpdateString();
    }

    public class Size : ABCModel
    {
        public override string TableName { get; } = "Sizes";

        public string Name { get; set; }
        
        public Size() { }

        public Size(SQLiteDataReader reader)
        {
            Id = reader.GetInt32(0);
            Name = reader.GetString(1);
        }

        public static Func<SQLiteDataReader, SQLiteConnection, Size> GetNewFunc() => (r, _) => new Size(r);

        public override string GetInsertString() => $"INSERT INTO [{TableName}] ([Name]) " +
                                                    $"VALUES ('{Name}');";
        public override string GetUpdateString() => $"UPDATE [{TableName}] SET [Name] = '{Name}' " +
                                                    $"WHERE [Id] == {Id};";

        public override string ToString() => $"{Name}";

        public bool CheckFilter(string filter)
        {
            filter = filter.ToUpper();
            return Name.ToUpper().Contains(filter);
        }
    }

    public class Uniform : ABCModel
    {
        public override string TableName { get; } = "Uniforms";

        public int Number { get; set; }
        public string Name { get; set; }
        public int Period { get; set; }

        public Uniform() { }

        public Uniform(SQLiteDataReader reader)
        {
            Id = reader.GetInt32(0);
            Number = reader.GetInt32(1);
            Name = reader.GetString(2);
            Period = reader.GetInt32(3);
        }

        public static Func<SQLiteDataReader, SQLiteConnection, Uniform> GetNewFunc() => (r, _) => new Uniform(r);

        public override string GetInsertString() => $"INSERT INTO [{TableName}] ([Number], [Name], [Period]) " +
                                                    $"VALUES ({Number}, '{Name}', {Period});";
        public override string GetUpdateString() => $"UPDATE [{TableName}] SET [Number] = {Number}, [Name] = '{Name}', " +
                                                    $"[Period] = {Period} " +
                                                    $"WHERE [Id] == {Id};";

        public override string ToString() => $"{Name}";

        public bool CheckFilter(string filter)
        {
            filter = filter.ToUpper();
            return Number.ToString().ToUpper().Contains(filter) || Name.ToUpper().Contains(filter) ||
                   Period.ToString().ToUpper().Contains(filter);
        }
    }

    public class Employee : ABCModel
    {
        public override string TableName { get; } = "Employees";

        public string Name { get; set; }
        public string Gender { get; set; }
        public int Growth { get; set; }
        public DateTime? Birthday { get; set; }
        public string BirthdayStr
        {
            get
            {
                return Birthday != null ? ((DateTime)Birthday).ToString("dd.MM.yyyy") : "";
            }
            set
            {
                try
                {
                    Birthday = DateTime.ParseExact(value, "M/d/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                }
                catch
                {
                    Birthday = DateTime.Parse(value);
                }
            }
        }
        public Size Size { get; set; }
        public Employee() { }

        public Employee(SQLiteDataReader reader, SQLiteConnection conn)
        {
            Id = reader.GetInt32(0);
            Name = reader.GetString(1);
            Gender = reader.GetString(2);
            Growth = reader.GetInt32(3);
            BirthdayStr = reader.GetString(4);

            SQLiteCommand command = new SQLiteCommand($"SELECT * FROM [Sizes] WHERE [Id] = {reader.GetInt32(5)};", conn);
            SQLiteDataReader _reader = command.ExecuteReader();
            _reader.Read();
            Size = new Size(_reader);
        }

        public static Func<SQLiteDataReader, SQLiteConnection, Employee> GetNewFunc() => (r, c) => new Employee(r, c);

        public override string GetInsertString() => $"INSERT INTO [{TableName}] ([Name], [Gender], [Growth], [Birthday], [Size])" +
                                                    $"VALUES ('{Name}', '{Gender}', {Growth}, '{BirthdayStr}', {Size.Id});";
        public override string GetUpdateString() => $"UPDATE [{TableName}] SET [Name] = '{Name}', [Gender] = '{Gender}', " +
                                                    $"[Growth] = {Growth}, [Birthday] = '{BirthdayStr}', [Size] = {Size.Id} " +
                                                    $"WHERE [Id] = {Id};";

        public override string ToString() => $"{Name}";

        public bool CheckFilter(string filter)
        {
            filter = filter.ToUpper();
            return Name.ToUpper().Contains(filter) || Gender.ToUpper().Contains(filter) || 
                   Growth.ToString().ToUpper().Contains(filter) || BirthdayStr.ToUpper().Contains(filter) ||
                   Size.ToString().ToUpper().Contains(filter);
        }
    }

    public class User : ABCModel
    {
        public override string TableName { get; } = "Users";

        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public bool IsAdmin { get { return Role == "Администратор"; } }

        public User() { }
        
        public User(SQLiteDataReader reader)
        {
            Id = reader.GetInt32(0);
            Name = reader.GetString(1);
            Login = reader.GetString(2);
            Password = reader.GetString(3);
            Role = reader.GetString(4);
        }

        public  static Func<SQLiteDataReader, SQLiteConnection, User> GetNewFunc() => (r, _) => new User(r);

        public override string GetInsertString() => $"INSERT INTO [{TableName}] ([Name], [Login], [Password], [Role]) " +
                                                    $"VALUES ('{Name}', '{Login}', '{Password}', '{Role}');";
        public override string GetUpdateString() => $"UPDATE [{TableName}] SET [Name] = '{Name}', [Login] = '{Login}', " +
                                                    $"[Password] = '{Password}', [Role] = '{Role}' " +
                                                    $"WHERE [Id] == {Id};";

        public override string ToString() => $"{Name}";

        public bool CheckFilter(string filter)
        {
            filter = filter.ToUpper();
            return Name.ToUpper().Contains(filter) || Login.ToUpper().Contains(filter) ||
                   Password.ToUpper().Contains(filter) || Role.ToUpper().Contains(filter);
        }
    }

    public class Issued : ABCModel
    {
        public override string TableName { get; } = "Issueds";

        public Uniform Uniform { get; set; }
        public Employee Employee { get; set; }
        public Size Size { get; set; }
        public DateTime? IssuedDate { get; set; }
        public string IssuedDateStr
        {
            get
            {
                return IssuedDate != null ? ((DateTime)IssuedDate).ToString("dd.MM.yyyy") : "";
            }
            set
            {
                try
                {
                    IssuedDate = DateTime.ParseExact(value, "M/d/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                }
                catch
                {
                    IssuedDate = DateTime.Parse(value);
                }
            }
        }
        public DateTime? ToDate { get; set; }
        public string ToDateStr
        {
            get
            {
                return ToDate != null ? ((DateTime)ToDate).ToString("dd.MM.yyyy") : "";
            }
            set
            {
                try
                {
                    ToDate = DateTime.ParseExact(value, "M/d/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                }
                catch
                {
                    ToDate = DateTime.Parse(value);
                }
            }
        }

        public bool Returned { get; set; }
        public string ReturnedText { get { return Returned ? "Да" : "Нет"; } }

        public Issued() 
        {
            IssuedDate = DateTime.Now;
            Returned = false;
        }

        public Issued(SQLiteDataReader reader, SQLiteConnection conn)
        {
            Id = reader.GetInt32(0);

            SQLiteCommand command = new SQLiteCommand($"SELECT * FROM [Uniforms] WHERE [Id] = {reader.GetInt32(1)};", conn);
            SQLiteDataReader _reader = command.ExecuteReader();
            _reader.Read();
            Uniform = new Uniform(_reader);

            command = new SQLiteCommand($"SELECT * FROM [Employees] WHERE [Id] = {reader.GetInt32(2)};", conn);
            _reader = command.ExecuteReader();
            _reader.Read();
            Employee = new Employee(_reader, conn);

            command = new SQLiteCommand($"SELECT * FROM [Sizes] WHERE [Id] = {reader.GetInt32(3)};", conn);
            _reader = command.ExecuteReader();
            _reader.Read();
            Size = new Size(_reader);

            IssuedDateStr = reader.GetString(4);
            ToDateStr = reader.GetString(5);
            Returned = reader.GetBoolean(6);
        }

        public static Func<SQLiteDataReader, SQLiteConnection, Issued> GetNewFunc() => (r, c) => new Issued(r, c);

        public override string GetInsertString() => $"INSERT INTO [{TableName}] ([Uniform], [Employee], [Size], [IssuedDate], [ToDate], [Returned]) " +
                                                    $"VALUES ({Uniform.Id}, {Employee.Id}, {Size.Id}, '{IssuedDateStr}', '{ToDateStr}', {Convert.ToInt32(Returned)});";
        public override string GetUpdateString() => $"UPDATE [{TableName}] SET [Uniform] = {Uniform.Id}, [Employee] = {Employee.Id}, [Size] = {Size.Id}, " +
                                                    $"[IssuedDate] = '{IssuedDateStr}', [ToDate] = '{ToDateStr}', [Returned] = {Convert.ToInt32(Returned)} " +
                                                    $"WHERE [Id] == {Id};";

        public bool CheckFilter(string filter)
        {
            filter = filter.ToUpper();
            return Uniform.ToString().ToUpper().Contains(filter) || Employee.ToString().ToUpper().Contains(filter) ||
                   IssuedDateStr.ToUpper().Contains(filter) || ToDateStr.ToUpper().Contains(filter);
        }
    }
}