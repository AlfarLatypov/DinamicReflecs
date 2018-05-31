using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace DinamicReflecs
{
    class Program
    {
        static void Main(string[] args)
        {
            Assembly asembly =
       Assembly.LoadFile(@"\\dc\Студенты\ПКО\SEB-171.2\C#\Exception\GeneratorName.dll");

            Type[] types = asembly.GetTypes();

            #region
            foreach (Type item in types)
            {
                Console.WriteLine("-> {0} ({1})", item.Name, item.IsClass);

                foreach (MethodInfo method in item.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                {
                    Console.WriteLine("   -> {0}", method.Name, method.ReturnType);

                    foreach (ParameterInfo param in method.GetParameters())
                    {
                        Console.WriteLine("        -->  {0}  ({1})", param.Name, param.ParameterType.BaseType);
                    }
                }
            }

            #endregion


            Type tGenerator =
                types.FirstOrDefault(f => f.IsClass && f.Name == "Generator");

            //экземпляр класса Генератор
            object metGenerator = Activator.CreateInstance(tGenerator);
            //у меня есть метод Генератор
            MethodInfo GenerateDefault = 
                metGenerator.GetType().GetMethod("GenerateDefault");
            //параметры метода
            ParameterInfo piGender = GenerateDefault.GetParameters()[0];

            object gender = null;

            if (piGender.ParameterType.BaseType == typeof(Enum))
            {
                gender = Enum.ToObject(piGender.ParameterType, 0);

            //т.к. енум, то есть список енумов
            FieldInfo[] fiGender = 
                    piGender.ParameterType.GetFields(BindingFlags.Public | BindingFlags.Static);
                foreach (var item in fiGender)
                {
                    Console.WriteLine(item.Name);
                }
            }
          
                object [] _params = new object[] { gender };
            var result = GenerateDefault.Invoke(metGenerator, _params);
            Console.WriteLine(result);
        }
    }

}
