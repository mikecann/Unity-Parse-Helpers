using Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityParseHelpers;

namespace UnityParseHelpersTests
{
    public interface IFather
    {
        string Name { get; }
        Child Son { get; set; }
        IChild Daughter { get; set; }
    }

    [ParseClassName("Father")]
    public class Father : ParseObject, IFather
    {
        public int AField;
        public Child AProperty { get; set; }
        public int AMethod() { return 0; }

        [ParseFieldName("name")]
        public string Name { get; set; }   

        [ParseFieldName("son")]
        public Child Son { get; set; }

        [ParseFieldName("daughter")]
        [ParseFieldType(typeof(Child))]
        public IChild Daughter { get; set; }

        [ParseFieldName("children")]
        [ParseFieldType(typeof(Child))]
        public List<IChild> Children { get; set; }
    }

    public interface IChild
    {
        string Name { get; }
        int Age { get; set; }
        IChild Brother { get; set; }
        IChild Sister { get; set; }
        void Shout();
    }

    [ParseClassName("Child")]
    public class Child : ParseObject, IChild
    {
        public int Age { get; set; }

        [ParseFieldName("name")]
        public string Name { get; set; }   

        [ParseFieldName("brother")]
        public IChild Brother { get; set; }

        [ParseFieldName("sister")]
        [ParseFieldType(typeof(Child))]
        public IChild Sister { get; set; }

        public void Shout() { Console.WriteLine("Raaar!"); }
    }

}
