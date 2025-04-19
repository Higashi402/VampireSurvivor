using MvcModel.Сreatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcController.Tools
{
    /// <summary>
    /// Класс GameTimer реализует функционал таймера для игрового процесса.
    /// Поддерживает запуск, остановку, паузу и возобновление таймера.
    /// Реализован как синглтон для обеспечения единственного экземпляра в приложении.
    /// </summary>
    public class GameTimer
    {
        // Приватные поля для хранения состояния таймера
        private static GameTimer _instance; // Единственный экземпляр таймера
        private static readonly object _lock = new object(); // Объект для синхронизации потоков
        private long _startTime; // Время начала отсчета
        private long _pauseTime; // Время паузы
        private bool _isRunning; // Флаг, указывающий, запущен ли таймер
        private bool _isPaused; // Флаг, указывающий, находится ли таймер на паузе

        /// <summary>
        /// Свойство для получения и установки времени начала отсчета.
        /// </summary>
        public long StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }

        /// <summary>
        /// Приватный конструктор для предотвращения создания экземпляров извне.
        /// </summary>
        private GameTimer() { }

        /// <summary>
        /// Свойство для получения единственного экземпляра GameTimer.
        /// Реализует потокобезопасный синглтон.
        /// </summary>
        public static GameTimer Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new GameTimer();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Возвращает текущее время в миллисекундах.
        /// </summary>
        /// <returns>Текущее время в миллисекундах.</returns>
        public static long GetCurrentTime()
        {
            return DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// Запускает таймер.
        /// </summary>
        public void Start()
        {
            _startTime = GetCurrentTime();
            _isRunning = true;
            _isPaused = false;
        }

        /// <summary>
        /// Останавливает таймер.
        /// </summary>
        public void Stop()
        {
            _isRunning = false;
            _isPaused = false;
        }

        /// <summary>
        /// Ставит таймер на паузу, если он запущен и не находится на паузе.
        /// </summary>
        public void Pause()
        {
            if (_isRunning && !_isPaused)
            {
                _pauseTime = GetCurrentTime();
                _isPaused = true;
            }
        }

        /// <summary>
        /// Возобновляет таймер, если он был на паузе.
        /// </summary>
        public void Resume()
        {
            if (_isRunning && _isPaused)
            {
                long pausedDuration = GetCurrentTime() - _pauseTime;
                _startTime += pausedDuration;
                _isPaused = false;
            }
        }

        /// <summary>
        /// Проверяет, запущен ли таймер и не находится ли он на паузе.
        /// </summary>
        /// <returns>True, если таймер запущен и не на паузе, иначе False.</returns>
        public bool IsRunning()
        {
            return _isRunning && !_isPaused;
        }

        /// <summary>
        /// Проверяет, находится ли таймер на паузе.
        /// </summary>
        /// <returns>True, если таймер на паузе, иначе False.</returns>
        public bool IsPaused()
        {
            return _isPaused;
        }

        /// <summary>
        /// Возвращает количество секунд, прошедших с момента запуска таймера.
        /// </summary>
        /// <returns>Количество прошедших секунд.</returns>
        public int GetElapsedSeconds()
        {
            if (!_isRunning)
                return 0;

            long currentTime = _isPaused ? _pauseTime : GetCurrentTime();
            long elapsedMilliseconds = currentTime - _startTime;

            return (int)(elapsedMilliseconds / 1000);
        }

        /// <summary>
        /// Добавляет указанное количество секунд к времени начала отсчета.
        /// </summary>
        /// <param name="seconds">Количество секунд для добавления.</param>
        public void AddSeconds(int seconds)
        {
            _startTime -= seconds * 1000;
        }
    }
}