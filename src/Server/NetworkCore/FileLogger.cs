using System.Collections.Concurrent;
using System.Text;

namespace NetworkCore
{
    public struct CipherLogObject
    {
        public string   AlgorithmName { get; set; }
        public string   OperationModeName { get; set; }
        public double   ElapsedMilliseconds { get; set; }
        public bool     IsEncrypt { get; set; }

        public string GetLogFilePath(string baseDir)
        {
            return Path.Combine(baseDir, AlgorithmName, $"{OperationModeName}.txt");
        }

        public string GetLogContent()
        {
            return $"[{(IsEncrypt ? "Encrypt" : "Decrypt")}] {ElapsedMilliseconds:F3} {Environment.NewLine}";
        }
    }

    public class FileLogger : IDisposable
    {
        private readonly BlockingCollection<CipherLogObject> logQueue = new BlockingCollection<CipherLogObject>();
        private readonly string baseDir;
        private readonly CancellationTokenSource cts = new CancellationTokenSource();
        private Task? loggingTask;
        private bool isDisposed = false;

        private static readonly Lazy<FileLogger> _instance = new Lazy<FileLogger>(() => new FileLogger());
        public static FileLogger Instance { get { return _instance.Value; } }

        private readonly ConcurrentDictionary<string, StreamWriter> writerCache = new ConcurrentDictionary<string, StreamWriter>();

        FileLogger()
        {
            baseDir = "./AlgorithmTimeLog/";
            loggingTask = Task.Factory.StartNew(ProcessLogQueue, TaskCreationOptions.LongRunning);
        }

        public void Log(CipherLogObject log)
        {
            if (isDisposed) throw new ObjectDisposedException(nameof(FileLogger));
            logQueue.Add(log);
        }

        private void ProcessLogQueue()
        {
            try
            {
                while (!logQueue.IsCompleted)
                {
                    if (logQueue.TryTake(out var logObject, -1, cts.Token))
                    {
                        WriteLog(logObject);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Log Canceled");
            }

            while (logQueue.TryTake(out var remainingLog))
            {
                WriteLog(remainingLog);
            }
            
        }

        private void WriteLog(CipherLogObject logObject)
        {
            try
            {
                var fullPath = logObject.GetLogFilePath(baseDir);
                var directoryPath = Path.GetDirectoryName(fullPath);

                if (directoryPath != null && !Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var writer = writerCache.GetOrAdd(fullPath, path => new StreamWriter(path, true, Encoding.UTF8) { AutoFlush = true });

                lock (writer)
                {
                    writer.Write(logObject.GetLogContent());
                }
            }
            catch (IOException ex)
            {
                Console.Error.WriteLine($"Failed to write log: {ex.Message}");
            }
        }

        private void FlushAndCloseWriters()
        {
            foreach (var kvp in writerCache.Values)
            {
                kvp.Flush();
                kvp.Close();
            }
            writerCache.Clear();
        }

        public void Stop()
        {
            if (isDisposed) return;

            cts.Cancel();
            logQueue.CompleteAdding();
            loggingTask?.Wait();
        }

        public void Dispose()
        {
            Stop();
            cts.Dispose();
            logQueue.Dispose();
            FlushAndCloseWriters();
            isDisposed = true;
        }

    }
}
