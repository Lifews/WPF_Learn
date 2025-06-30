using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DataTemplate
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class MainViewModel : ObservableObject
    {
        private List<Person> persons = new List<Person>();
        public List<Person> Persons
        {
            get { return persons; }
            set { persons = value; RaisePropertyChanged(); }
        }

        private Person person1;
        public Person Person1
        {
            get { return person1; }
            set { person1 = value; RaisePropertyChanged(); }
        }
        public MainViewModel()
        {
            person1 = new Person()
            {
                Name = "Michael Jackson",
                Occupation = "Musicians",
                Age = 25,
                Money = 9999999,
                Address = "深圳市光明区智慧招商城B4栋5楼"
            };

            var bill = new Person()
            {
                Name = "比尔·盖茨（Bill Gates）",
                Occupation = "微软公司创始人",
                Age = 61,
                Money = 9999999,
                Address = "美国华盛顿州西雅图"
            };

            var musk = new Person()
            {
                Name = "Elon Reeve Musk",
                Occupation = "首席执行官",
                Age = 50,
                Money = 365214580,
                Address = "出生于南非的行政首都比勒陀利亚"
            };

            var jeff = new Person()
            {
                Name = "杰夫·贝索斯（Jeff Bezos）",
                Occupation = "董事会执行主席",
                Age = 25,
                Money = 85745845,
                Address = "杰夫·贝索斯出生于美国新墨西哥州阿尔布奎克。"
            };

            persons.Add(person1);
            persons.Add(bill);
            persons.Add(musk);
            persons.Add(jeff);
        }
        public class Person : ObservableObject
        {
            private string name;
            public string Name
            {
                get { return name; }
                set { name = value; RaisePropertyChanged(); }
            }

            private string occupation;
            public string Occupation
            {
                get { return occupation; }
                set { occupation = value; RaisePropertyChanged(); }
            }

            private int age;
            public int Age
            {
                get { return age; }
                set { age = value; RaisePropertyChanged(); }
            }

            private int money;
            public int Money
            {
                get { return money; }
                set { money = value; RaisePropertyChanged(); }
            }

            private string address;
            public string Address
            {
                get { return address; }
                set { address = value; RaisePropertyChanged(); }
            }
        }
    }
}
