using Microsoft.EntityFrameworkCore;

namespace FW.EntityFramework
{
    ///<summary>
    ///Класс, от которого можно наследоваться для базовых функций: добавление, удаление, изменение записи, вывод всех записей.
    ///Если мы будем использовать параллелизм, для каждого потока надо отдельно создавать контекст бд (ApplicationContext).
    ///Поэтому тут необходимо передавать контекст в функцию.
    ///<summary>
    public class RootDatabaseWorker<T> where T: class
    {
        public void AddEntry(T Entry, ApplicationContext context)
        {
            try
            {
                context.Set<T>().Add(Entry);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message+e.StackTrace);
            }
        }

        public void DeleteEntry(T Entry, ApplicationContext context)
        {
            try
            {
                context.Set<T>().Remove(Entry);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message+e.StackTrace);
            }
        }

        public void UpdateEntry(T Entry, ApplicationContext context)
        {
            try
            {
                context.Entry(Entry).State = EntityState.Modified;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message+e.StackTrace);
            }
        }

        public List<T> ShowEntries(ApplicationContext context)
        {
            try
            {
                return context.Set<T>().ToList();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message+e.StackTrace);
                return null;
            }
        }
    }
}