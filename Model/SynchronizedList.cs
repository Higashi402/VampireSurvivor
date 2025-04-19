using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcModel
{
    /// <summary>
    /// Класс SynchronizedList представляет потокобезопасный список.
    /// Обеспечивает синхронизированный доступ к элементам списка.
    /// </summary>
    /// <typeparam name="T">Тип элементов списка.</typeparam>
    public class SynchronizedList<T>
    {
        // Внутренний список для хранения элементов
        private readonly List<T> list = new List<T>();

        // Объект для синхронизации доступа к списку
        private readonly object lockObject = new object();

        /// <summary>
        /// Добавляет элемент в список.
        /// </summary>
        /// <param name="parItem">Элемент для добавления.</param>
        public void Add(T parItem)
        {
            lock (lockObject)
            {
                list.Add(parItem);
            }
        }

        /// <summary>
        /// Удаляет элемент из списка.
        /// </summary>
        /// <param name="parItem">Элемент для удаления.</param>
        /// <returns>True, если элемент был успешно удален, иначе False.</returns>
        public bool Remove(T parItem)
        {
            lock (lockObject)
            {
                return list.Remove(parItem);
            }
        }

        /// <summary>
        /// Возвращает копию списка в виде нового списка.
        /// </summary>
        /// <returns>Копия списка.</returns>
        public List<T> GetSnapshot()
        {
            lock (lockObject)
            {
                return new List<T>(list);
            }
        }

        /// <summary>
        /// Выполняет указанное действие для каждого элемента списка.
        /// </summary>
        /// <param name="parAction">Действие, выполняемое для каждого элемента.</param>
        public void ForEach(Action<T> parAction)
        {
            lock (lockObject)
            {
                foreach (var item in list)
                {
                    parAction(item);
                }
            }
        }

        /// <summary>
        /// Возвращает количество элементов в списке.
        /// </summary>
        public int Count
        {
            get
            {
                lock (lockObject)
                {
                    return list.Count;
                }
            }
        }

        /// <summary>
        /// Проверяет, удовлетворяет ли хотя бы один элемент списка указанному условию.
        /// </summary>
        /// <param name="parPredicate">Условие для проверки элементов.</param>
        /// <returns>True, если хотя бы один элемент удовлетворяет условию, иначе False.</returns>
        public bool Any(Func<T, bool> parPredicate)
        {
            lock (lockObject)
            {
                return list.Any(parPredicate);
            }
        }

        /// <summary>
        /// Возвращает элемент списка по указанному индексу.
        /// </summary>
        /// <param name="parIndex">Индекс элемента.</param>
        /// <returns>Элемент списка или null, если индекс некорректен.</returns>
        public Object Get(int parIndex)
        {
            lock (lockObject)
            {
                if (parIndex >= 0)
                {
                    return list[parIndex];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Возвращает список элементов, удовлетворяющих указанному условию.
        /// </summary>
        /// <param name="parPredicate">Условие для фильтрации элементов.</param>
        /// <returns>Список элементов, удовлетворяющих условию.</returns>
        public List<T> Where(Func<T, bool> parPredicate)
        {
            lock (lockObject)
            {
                return list.Where(parPredicate).ToList();
            }
        }
    }
}